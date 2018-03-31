using System;
using System.Resources;
using SimpleInjector;

namespace CQRSRx.Integration.SimpleInjector.Resources
{
    internal static class StringResources
    {
        internal static string CannotResolveHandler(Type type)
        {
            return $"Processor could not resolve handler of type {type.ToFriendlyName()}. See inner exception for details.";
        }
    }
}
