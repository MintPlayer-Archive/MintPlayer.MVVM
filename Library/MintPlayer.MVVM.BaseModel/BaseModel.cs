using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MintPlayer.MVVM.BaseModel
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region SetProperty
        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                // Property value hasn't changed
                return false;
            }
            else
            {
                backingField = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
        #endregion
    }
}
