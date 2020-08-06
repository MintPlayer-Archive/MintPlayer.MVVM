using System.ComponentModel;
using Xamarin.Forms;
using MintPlayer.MVVM.Platforms.Common;
using MintPlayer.MVVM.Demo.ViewModels;

namespace MintPlayer.MVVM.Demo.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [ViewModel(typeof(NewItemVM))]
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public NewItemPage()
        {
            InitializeComponent();
        }
    }
}