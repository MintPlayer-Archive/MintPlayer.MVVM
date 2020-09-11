using System;

namespace MintPlayer.MVVM.Platforms.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewModelAttribute : Attribute
    {
        public ViewModelAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        internal Type ViewModelType { get; }
    }
}
