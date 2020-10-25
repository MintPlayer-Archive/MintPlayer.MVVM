using System.Threading.Tasks;

namespace MintPlayer.MVVM.Platforms.Common
{
    public abstract class BaseViewModel : BaseModel
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
    }
}
