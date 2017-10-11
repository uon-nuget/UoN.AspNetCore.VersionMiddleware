using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UoN.NetCore.VersionExtensionMiddleware
{
    public class VersionMiddleware
    {
        private readonly Assembly _versionAssembly;

        public VersionMiddleware(RequestDelegate next, Assembly versionAssembly)
        {
            _versionAssembly = versionAssembly;
        }

        public async Task Invoke(HttpContext context)
            => await context.Response.WriteAsync(
                _versionAssembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion);
    }
}
