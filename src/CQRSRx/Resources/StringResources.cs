using System;

namespace CQRSRx.Resources
{
    internal static class StringResources
    {
        internal static string AggregateSourceStreamIsCorrupted()
        {
            return "Aggregate source stream produced an error. See inner exception for details.";
        }

        internal static string AggregateSourceStreamProvidedEventOutOfOrder(long expectedVersion, long actualVersion)
        {
            return $"The aggregate source stream provided an event out of order. version [actual: {actualVersion}, expected: {expectedVersion}]";
        }
    }
}
