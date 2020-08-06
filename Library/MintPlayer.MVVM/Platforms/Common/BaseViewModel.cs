using System.Threading.Tasks;

namespace MintPlayer.MVVM.Platforms.Common
{
    public abstract class BaseViewModel : BaseModel
    {
        protected virtual Task OnNavigatedTo()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnNavigatedFrom()
        {
            return Task.CompletedTask;
        }
    }
}
