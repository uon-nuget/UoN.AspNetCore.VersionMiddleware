using System.Reflection;
using System.Threading.Tasks;

namespace UoN.AspNetCore.VersionMiddleware.Providers
{
    public class AssemblyInformationalVersionProvider : IVersionInformationProvider
    {
        private readonly Assembly _versionAssembly;

        public AssemblyInformationalVersionProvider(Assembly versionAssembly)
        {
            _versionAssembly = versionAssembly;
        }

        public async Task<string> GetVersionInformationAsync()
            => _versionAssembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

    }
}
