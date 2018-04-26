using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UoN.AspNetCore.VersionMiddleware.Providers
{
    /// <summary>
    /// Simply passes through a .NET object as version information.
    /// </summary>
    public class ObjectProvider : IVersionInformationProvider
    {
        /// <summary>
        /// The .NET Object that makes up version information
        /// returned by this provider.
        /// </summary>
        public object VersionObject { get; set; }

        /// <summary>
        /// Create a version provider that aggregates version information from
        /// multiple other providers.
        /// </summary>
        /// <param name="versionObject">The .NET Object that makes up the version information</param>
        public ObjectProvider(
            object versionObject)
        {
            VersionObject = versionObject;
        }

        /// <summary>
        /// Provides the .NET object as version information
        /// </summary>
        /// <returns>a .NET Object containing version information.</returns>
        public async Task<object> GetVersionInformationAsync()
            => VersionObject;
    }
}
