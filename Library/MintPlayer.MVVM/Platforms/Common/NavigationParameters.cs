using System.Collections.Generic;

namespace MintPlayer.MVVM.Platforms.Common
{
    public class NavigationParameters : Dictionary<string, object>
    {
        public void AddOrUpdate<T>(string key, T newValue)
        {
            if (ContainsKey(key))
                this[key] = newValue;
            else
                Add(key, newValue);
        }

        public T GetValue<T>(string key)
        {
            if (ContainsKey(key))
                return (T)this[key];
            else
                return default(T);
        }
    }
}
