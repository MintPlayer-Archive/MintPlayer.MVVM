using System.ComponentModel;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Demo.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
        }
    }
}