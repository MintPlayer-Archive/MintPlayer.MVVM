using System;

namespace MintPlayer.MVVM.Platforms.Common.Exceptions
{
    public class RegionNotFoundException : Exception
    {
        public RegionNotFoundException(string regionName) : base($"Region with name {regionName} does not exist")
        {
        }
    }
}
