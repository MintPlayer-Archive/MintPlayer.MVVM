using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MintPlayer.MVVM.Demo.Models;
using MintPlayer.MVVM.Demo.ViewModels;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Demo.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [ViewModel(typeof(ItemDetailVM))]
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage(ItemDetailVM viewModel)
        {
            InitializeComponent();
        }
    }
}