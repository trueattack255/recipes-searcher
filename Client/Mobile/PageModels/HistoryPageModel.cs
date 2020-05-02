using System.Collections.ObjectModel;
using Xamarin.Forms;
using FreshMvvm;
using Mobile.Interfaces;
using Mobile.Models;

namespace Mobile.PageModels
{
    public class HistoryPageModel : FreshBasePageModel
    {
        private bool _isEmpty;
        private string _message;

        private ISQLiteService _sqLiteService;

        public ObservableCollection<Receipt> Receipts { get; set; }

        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                RaisePropertyChanged(nameof(IsEmpty));
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

        public Receipt SelectedReceipt
        {
            set
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    CoreMethods.RemoveFromNavigation<SearchPageModel>();
                    await CoreMethods.PushPageModel<SearchPageModel>(value);

                    CoreMethods.RemoveFromNavigation<HistoryPageModel>();
                    RaisePropertyChanged(nameof(SelectedReceipt));
                });
            }
        }

        public HistoryPageModel(ISQLiteService sqLiteService)
        {   
            _sqLiteService = sqLiteService;

            Receipts = new ObservableCollection<Receipt>();
            Message = "История отсутствует";

            IsEmpty = true;
        }

        public override async void Init(object initData)
        {
            base.Init(initData);

            foreach (var item in await _sqLiteService.GetItemsAsync())
            {
                Receipts.Add(item);
            }

            if (Receipts.Count != 0)
                IsEmpty = false;
        }
    }
}
