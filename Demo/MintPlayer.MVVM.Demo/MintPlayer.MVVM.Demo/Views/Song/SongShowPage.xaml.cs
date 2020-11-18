using MintPlayer.MVVM.Demo.ViewModels.Song;
using MintPlayer.MVVM.Platforms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MintPlayer.MVVM.Demo.Views.Song
{
    [ViewModel(typeof(SongShowVM))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongShowPage : ContentPage
    {
        public SongShowPage()
        {
            InitializeComponent();
        }
    }
}