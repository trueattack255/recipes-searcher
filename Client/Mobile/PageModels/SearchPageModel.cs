using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using Mobile.Interfaces;
using Mobile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refractored.FabControl;
using Xamarin.Forms;

namespace Mobile.PageModels
{
    public class SearchPageModel : FreshBasePageModel
    {
        private const string PathError = "error.error_msg";
        private const string PathResponse = "response.array";
        private const string Concurrency = "50";

        private bool _isBusy;
        private bool _isFailure;
        private bool _isInitialized;
        private int _appearingListItemIndex;
        private string _message;

        private IWebService _webService;
        private ISQLiteService _sqLiteService;

        private ICommand _openScannerCommand;
        private ICommand _openHistoryCommand;
        private ICommand _apperingCommand;
        private ICommand _disapperingCommand;

        private ListView _listView;
        private FloatingActionButtonView _scannerButton;
        private FloatingActionButtonView _historyButton;

        public ObservableCollection<Recipe> Recipes { get; set; }

        public Recipe SelectedRecipe
        {
            set
            {
                CoreMethods.PushPageModel<RecipePageModel>(value);
                RaisePropertyChanged(nameof(SelectedRecipe));
            }
        }

        public bool IsVisible => !(_isBusy ^ _isInitialized ^ _isFailure);

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
                RaisePropertyChanged(nameof(IsVisible));
            }
        }

        public bool IsFailure
        {
            get => _isFailure;
            set
            {
                _isFailure = value;
                RaisePropertyChanged(nameof(IsFailure));
            }
        }

        public bool IsInitialized
        {
            get => _isInitialized;
            set
            {
                _isInitialized = value;
                RaisePropertyChanged(nameof(IsInitialized));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }

        public ICommand OpenHistoryCommand
        {
            get =>
                _openHistoryCommand ?? (_openHistoryCommand = new Command(async () =>
                {
                    _historyButton.Hide();
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await CoreMethods.PushPageModel<HistoryPageModel>();
                    _historyButton.Show();
                }));
        }

        public ICommand OpenScannerCommand
        {
            get =>
                _openScannerCommand ?? (_openScannerCommand = new Command(async () =>
                {
                    _scannerButton.Hide();
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await CoreMethods.PushPageModel<ScannerPageModel>(data: true);
                    _scannerButton.Show();
                }));
        }

        public ICommand AppearingCommand
        {
            get =>
                _apperingCommand ?? (_apperingCommand = new Command(() =>
                {
                    _listView.ItemAppearing += List_ItemAppearing;
                    _listView.ItemDisappearing += List_ItemDisappearing;
                }));
        }

        public ICommand DisappearingCommand
        {
            get =>
                _disapperingCommand ?? (_disapperingCommand = new Command(() =>
                {
                    _listView.ItemAppearing -= List_ItemAppearing;
                    _listView.ItemDisappearing -= List_ItemDisappearing;
                }));
        }

        public SearchPageModel(IWebService webService, ISQLiteService sqLiteService)
        {
            _webService = webService;
            _sqLiteService = sqLiteService;
            _appearingListItemIndex = 0;

            Recipes = new ObservableCollection<Recipe>();
            Message = "Отсканируйте кассовый чек перед началом поиска рецептов";

            IsBusy = false;
            IsFailure = false;
            IsInitialized = false;
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            SearchRecipe(initData);

            _scannerButton = CurrentPage.FindByName<FloatingActionButtonView>("ScannerButton");
            _historyButton = CurrentPage.FindByName<FloatingActionButtonView>("HistoryButton");
            _listView = CurrentPage.FindByName<ListView>("listView");
        }

        private async Task SearchRecipe(object target)
        {
            switch (target)
            {
                case Receipt receipt:
                {
                    await FindByReceipt(receipt);
                    break;
                }
                case string code:
                {
                    await FindByQRCode(code);
                    break;
                }
            }
        }

        private async Task FindByReceipt(Receipt receipt)
        {
            if (IsInitialized) return;
            IsBusy = true;

            var response = default(JObject);

            try
            {
                response = await _webService.Search(Concurrency, receipt.IngredientList);
                var recipeList = response
                    .SelectToken(PathResponse, true)
                    .ToObject<List<Recipe>>();

                receipt.DateUpdate = DateTime.Now;
                receipt.Status = "Рецепты найдены";

                if (recipeList.Count == 0)
                {
                    receipt.Status = "Чек получен";

                    Message = "Ничего не найдено";
                    IsFailure = true;
                }

                Recipes.Clear();

                foreach (var recipe in recipeList)
                {
                    recipe.Photo = _webService.Site + recipe.Photo;
                    Recipes.Add(recipe);
                }

                await _sqLiteService.SaveItemAsync(receipt);
            }
            catch (JsonException)
            {
                Message = response
                    ?.SelectToken(PathError)
                    ?.ToString();

                IsFailure = true;
            }
            finally
            {
                IsInitialized = true;
                IsBusy = false;
            }
        }

        private async Task FindByQRCode(string code)
        {
            if (IsInitialized) return;
            IsBusy = true;

            var ingredientList = string.Empty;
            var receiptHash = string.Empty;
            var response = default(JObject);

            try
            {
                response = await _webService.CheckReceipt(code);
                receiptHash = response
                    .SelectToken($"{PathResponse}.id", true)
                    .ToString();

                response = await _webService.GetIngredients(receiptHash);
                ingredientList = string.Join(",", response
                    .SelectToken(PathResponse, true)
                    .ToObject<List<Ingredient>>()
                    .Select(it => it.Name));

                response = await _webService.Search(Concurrency, ingredientList);
                var recipeList = response
                    .SelectToken(PathResponse, true)
                    .ToObject<List<Recipe>>();

                var receipt = await _sqLiteService.GetItemAsync(receiptHash) ?? new Receipt();

                receipt.Hash = receiptHash;
                receipt.IngredientList = ingredientList;
                receipt.DateUpdate = DateTime.Now;
                receipt.Status = "Рецепты найдены";

                if (recipeList.Count == 0)
                {
                    receipt.Status = "Чек получен";

                    Message = "Ничего не найдено";
                    IsFailure = true;
                }

                Recipes.Clear();

                foreach (var recipe in recipeList)
                {
                    recipe.Photo = _webService.Site + recipe.Photo;
                    Recipes.Add(recipe);
                }

                await _sqLiteService.SaveItemAsync(receipt);
            }
            catch (JsonException)
            {
                Message = response
                    ?.SelectToken(PathError)
                    ?.ToString();

                IsFailure = true;
            }
            finally
            {
                IsInitialized = true;
                IsBusy = false;
            }
        }

        private async void List_ItemDisappearing(object sender, ItemVisibilityEventArgs e)
        {
            await Task.Run(() =>
            {
                var items = _listView.ItemsSource as IList;
                if (items != null)
                {
                    var index = items.IndexOf(e.Item);
                    if (index < _appearingListItemIndex)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _scannerButton.Hide();
                            _historyButton.Hide();
                        });
                    }
                    _appearingListItemIndex = index;
                }
            });
        }

        private async void List_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await Task.Run(() =>
            {
                var items = _listView.ItemsSource as IList;
                if (items != null)
                {
                    var index = items.IndexOf(e.Item);
                    if (index < _appearingListItemIndex)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _scannerButton.Show();
                            _historyButton.Show();
                        });
                    }
                    _appearingListItemIndex = index;
                }
            });
        }
    }
}
