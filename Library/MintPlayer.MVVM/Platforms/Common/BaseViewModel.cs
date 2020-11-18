using System.Threading.Tasks;

namespace MintPlayer.MVVM.Platforms.Common
{
    public abstract class BaseViewModel : BaseModel.BaseModel
    {
        /// <summary>Occurs when the page is pushed onto the navigation.</summary>
        protected internal virtual Task OnNavigatedTo(NavigationParameters parameters) => Task.CompletedTask;

        /// <summary>Occurs when the page is popped from the navigation.</summary>
        protected internal virtual Task OnNavigatedFrom() => Task.CompletedTask;

        /// <summary>Occurs when either the page is pushed onto the navigation or the page reappears.</summary>
        /// <param name="pushed">Indicates whether the page was pushed onto the navigation just yet.</param>
        protected internal virtual Task OnAppearing(bool pushed) => Task.CompletedTask;

        /// <summary>Occurs when either the page is popped from the navigation or another page covers this one.</summary>
        /// <param name="popped">Indicates whether or not the page is actually removed from the stack.</param>
        protected internal virtual Task OnDisappearing(bool popped) => Task.CompletedTask;

        /// <summary>Indicates whether the page was pushed modally.</summary>
        public bool IsModal { get; internal set; }

        #region Title
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        #endregion

        internal bool isPushing = false;
        internal bool isPopping = false;
    }
}
