using MintPlayer.MVVM.Demo.Events;
using MintPlayer.MVVM.Platforms.Common.EventAggregator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class MainVM : BaseVM
    {
        private readonly IEventAggregator eventAggregator;
        public MainVM(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<MenuItemSelectedEvent>().Subscribe(OnMenuItemSelected);
        }

        #region Bindings
        #region SidebarVisible
        private bool isSidebarVisible;
        public bool IsSidebarVisible
        {
            get => isSidebarVisible;
            set => SetProperty(ref isSidebarVisible, value);
        }
        #endregion
        #endregion

        #region Methods
        private void OnMenuItemSelected(object obj)
        {
            IsSidebarVisible = false;
        }
        #endregion
    }
}
