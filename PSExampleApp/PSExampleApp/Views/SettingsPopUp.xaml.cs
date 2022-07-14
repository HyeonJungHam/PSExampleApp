﻿using PSExampleApp.Forms.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPopUp : PopupPage
    {
        public SettingsPopUp()
        {
            BackgroundColor = new Xamarin.Forms.Color(0, 0, 0, 0.8);
            BindingContext = App.GetViewModel<SettingsViewModel>();
            InitializeComponent();
        }
    }
}