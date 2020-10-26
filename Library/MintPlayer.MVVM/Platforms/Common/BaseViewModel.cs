using System.Threading.Tasks;

namespace MintPlayer.MVVM.Platforms.Common
{
    public abstract class BaseViewModel : BaseModel.BaseModel
    {
        protected internal virtual Task OnNavigatedTo(NavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        protected internal virtual Task OnNavigatedFrom()
        {
            return Task.CompletedTask;
        }

        public bool IsModal { get; internal set; }

        #region Title
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        #endregion
    }
}
