using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UoN.AspNetCore.VersionMiddleware.Providers
{
    /// <summary>
    /// Gets version information from multiple `IVersionInformationProviders`
    /// aggregating the results into a Dictionary.
    /// </summary>
    public class AggregateProvider : IVersionInformationProvider
    {
        public IDictionary<string, IVersionInformationProvider> Providers { get; set; }

        /// <summary>
        /// Create a version provider that aggregates version information from
        /// multiple other providers.
        /// </summary>
        /// <param name="providers">Optional providers to initialise this Aggregate Provider</param>
        public AggregateProvider(
            IDictionary<string, IVersionInformationProvider> providers = null)
        {
            Providers = providers;
        }

        /// <summary>
        /// Gets Version Information from all the `IVersionInformationProviders
        /// </summary>
        /// <returns>A Dictionary of all the version information from each keyed provider.</returns>
        public async Task<object> GetVersionInformationAsync()
            => (await Task.WhenAll(Providers.Select(async x =>
                    new KeyValuePair<string, object>(
                        x.Key,
                        await x.Value.GetVersionInformationAsync()))))
                .ToDictionary(x => x.Key, x => x.Value);
    }
}
