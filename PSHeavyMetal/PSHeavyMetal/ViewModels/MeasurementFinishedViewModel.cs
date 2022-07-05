﻿using MvvmHelpers;
using Plugin.Media;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using PSHeavyMetal.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms.ViewModels
{
    internal class MeasurementFinishedViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IShareService _shareService;
        private bool _IsCreatingReport;

        public MeasurementFinishedViewModel(IMeasurementService measurementService, IShareService shareService)
        {
            _shareService = shareService;
            _popupNavigation = PopupNavigation.Instance;
            _measurementService = measurementService;
            ActiveMeasurement = _measurementService.ActiveMeasurement;

            ShowPlotCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.MeasurementPlotView));
            NavigateToHomeCommand = CommandFactory.Create(NavigateToHome);
            RepeatMeasurementCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.ConfigureMeasurementView));
            OnPhotoSelectedCommand = CommandFactory.Create(async photo => await OpenPhoto(photo as ImageSource));
            TakePhotoCommand = CommandFactory.Create(TakePhoto);
            ShareMeasurementCommand = CommandFactory.Create(ShareMeasurement);
            OnPageAppearingCommand = CommandFactory.Create(OnAppearing);
            OnPageDisappearingCommand = CommandFactory.Create(OnDisappearing);
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        /// <summary>
        /// Gets if the measurement has a maximum amount of photos. This is 3
        /// </summary>
        public bool HasMaxPhotos => MeasurementPhotos.Count == 3;

        public bool IsCreatingReport
        {
            get => _IsCreatingReport;
            set => SetProperty(ref _IsCreatingReport, value);
        }

        public ObservableCollection<MeasurementPhotoPresenter> MeasurementPhotos { get; } = new ObservableCollection<MeasurementPhotoPresenter>();

        public ICommand NavigateToHomeCommand { get; }

        public ICommand OnPageAppearingCommand { get; }

        public ICommand OnPageDisappearingCommand { get; }

        public ICommand OnPhotoSelectedCommand { get; }

        public ICommand RepeatMeasurementCommand { get; }

        public ICommand ShareMeasurementCommand { get; }

        public ICommand ShowPlotCommand { get; }

        public ICommand TakePhotoCommand { get; }

        public async Task NavigateToHome()
        {
            _measurementService.ResetMeasurement();
            await NavigationDispatcher.PopToRoot();
        }

        private void LoadPhoto(byte[] image)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var photo = new MeasurementPhotoPresenter
                {
                    Photo = ImageSource.FromStream(() =>
                    {
                        return new MemoryStream(image);
                    })
                };

                MeasurementPhotos.Add(photo);
            });
        }

        private void MeasurementPhotos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(MeasurementPhotos));
            this.OnPropertyChanged(nameof(HasMaxPhotos));
        }

        private void OnAppearing()
        {
            MeasurementPhotos.CollectionChanged += MeasurementPhotos_CollectionChanged;

            this.OnPropertyChanged(nameof(HasMaxPhotos));
        }

        private void OnDisappearing()
        {
            MeasurementPhotos.CollectionChanged -= MeasurementPhotos_CollectionChanged;
        }

        private async Task OpenPhoto(ImageSource imageSource)
        {
            await _popupNavigation.PushAsync(new MeasurementPhotoPopup(imageSource));
        }

        private async Task ShareMeasurement()
        {
            var cacheFile = Path.Combine(FileSystem.CacheDirectory, $"Report-{ActiveMeasurement.Name}");
            IsCreatingReport = true;

            //CreatePDFfile is a long running proces that isn't async by itself.
            await Task.Run(() => _shareService.CreatePdfFile(ActiveMeasurement, cacheFile));

            IsCreatingReport = false;

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Sharing is Caring",
                File = new ShareFile(cacheFile)
            });
        }

        private async Task TakePhoto()
        {
            MeasurementPhotos.CollectionChanged += MeasurementPhotos_CollectionChanged;

            var stream = await TakePictureAsync();

            var memoryStream = new MemoryStream();

            stream.CopyTo(memoryStream);
            stream.Dispose();

            var byteArray = memoryStream.ToArray();
            ActiveMeasurement.MeasurementImages.Add(byteArray);
            await _measurementService.SaveMeasurement(ActiveMeasurement);

            memoryStream.Dispose();

            LoadPhoto(byteArray);
        }

        private async Task<Stream> TakePictureAsync()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return null;
            }

            var result = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                CompressionQuality = 30,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
            });

            if (result == null)
                return null;

            var stream = result.GetStream();

            return stream;
        }
    }
}