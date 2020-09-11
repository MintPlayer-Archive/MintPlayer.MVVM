namespace MintPlayer.MVVM.Demo.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            Platforms.UWP.Platform.Init<Demo.App, Startup>(this);
        }
    }
}
