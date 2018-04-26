using System.Reflection;
using System.Threading.Tasks;

namespace UoN.AspNetCore.VersionMiddleware.Providers
{
    /// <summary>
    /// Gets version information from a .NET Assembly's `AssemblyInformationalVersion` metadata.
    /// </summary>
    public class AssemblyInformationalVersionProvider : IVersionInformationProvider
    {
        private readonly Assembly _versionAssembly;

        /// <summary>
        /// Create a version provider for a given .NET Assembly.
        /// </summary>
        /// <param name="versionAssembly">The .NET Assembly to get `AssemblyInformationalVersion` from.</param>
        public AssemblyInformationalVersionProvider(Assembly versionAssembly)
        {
            _versionAssembly = versionAssembly;
        }

        /// <summary>
        /// Gets version information from a .NET Assembly's `AssemblyInformationalVersion` metadata.
        /// </summary>
        /// <returns>`AssemblyInformationalVersion` as a string.</returns>
        public async Task<object> GetVersionInformationAsync()
            => _versionAssembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

    }
}
