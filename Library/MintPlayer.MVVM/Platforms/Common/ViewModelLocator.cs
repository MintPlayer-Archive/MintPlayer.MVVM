using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MintPlayer.MVVM.Platforms.Common
{
    internal interface IViewModelLocator
    {
        Type Resolve<TViewModel>();
    }
    internal class ViewModelLocator : IViewModelLocator
    {
        public ViewModelLocator(Type platformStartupType)
        {
            viewmodelMapping = platformStartupType.BaseType.Assembly.DefinedTypes
                .Select(t => new
                {
                    Type = t,
                    Attribute = t.GetCustomAttribute<ViewModelAttribute>()
                })
                .Where(t => t.Attribute != null)
                .ToDictionary(
                    t => t.Attribute.ViewModelType,
                    t => t.Type.AsType()
                );
        }

        public Type Resolve<TViewModel>()
        {
            return viewmodelMapping[typeof(TViewModel)];
        }

        private readonly IDictionary<Type, Type> viewmodelMapping;
    }
}
