using System.Threading.Tasks;

namespace MintPlayer.MVVM.Platforms.Common
{
    public abstract class BaseViewModel : BaseModel
    {
        protected internal virtual Task OnNavigatedTo()
        {
            return Task.CompletedTask;
        }

        protected internal virtual Task OnNavigatedFrom()
        {
            return Task.CompletedTask;
        }
    }
}
