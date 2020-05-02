using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using FreshMvvm;

namespace Mobile.PageModels
{
    public class ScannerPageModel : FreshBasePageModel
    {
        private readonly ZXingScannerView zxing;
        private readonly ZXingDefaultOverlay overlay;
        private readonly MobileBarcodeScanningOptions options;

        public Grid Context { get; private set; }

        ICommand _apperingCommand;
        public ICommand AppearingCommand
        {
            get => _apperingCommand ?? (_apperingCommand = new Command(() => zxing.IsScanning = true));
        }

        ICommand _disapperingCommand;
        public ICommand DisappearingCommand
        {
            get => _disapperingCommand ?? (_disapperingCommand = new Command(() => zxing.IsScanning = true));
        }

        public ScannerPageModel()
        {
            options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true,
                PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }
            };

            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Options = options,
                AutomationId = "zxingScannerView",
            };

            zxing.OnScanResult += Handle_OnScanResult;

            overlay = new ZXingDefaultOverlay
            {
                TopText = string.Empty,
                BottomText = string.Empty,
                ShowFlashButton = zxing.HasTorch,
                AutomationId = "zxingDefaultOverlay",
            };

            overlay.Children.RemoveAt(2);
            overlay.Children.Add(new BoxView
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 3,
                BackgroundColor = Color.FromHex("b794f6"),
                Opacity = 0.4,
            }, 0, 1);

            overlay.FlashButtonClicked += (sender, e) => 
            {
                zxing.IsTorchOn = !zxing.IsTorchOn;
            };

            Context = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            Context.Children.Add(zxing);
            Context.Children.Add(overlay);
        }


        public void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                zxing.IsAnalyzing = false;

                CoreMethods.RemoveFromNavigation<SearchPageModel>();
                await CoreMethods.PushPageModel<SearchPageModel>(result.Text);

                CoreMethods.RemoveFromNavigation<ScannerPageModel>();
            });
        }
    }
}
