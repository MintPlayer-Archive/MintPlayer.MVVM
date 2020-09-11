using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MintPlayer.MVVM.Demo.Models;
using MintPlayer.MVVM.Demo.Views;
using MintPlayer.MVVM.Demo.ViewModels;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Demo.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [ViewModel(typeof(ItemsVM))]
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage()
        {
            InitializeComponent();
        }
    }
}