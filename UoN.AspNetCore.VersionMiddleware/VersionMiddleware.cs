using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UoN.AspNetCore.VersionMiddleware
{
    public class VersionMiddleware
    {
        private readonly IVersionInformationProvider _provider;

        public VersionMiddleware(RequestDelegate next, IVersionInformationProvider provider)
        {
            _provider = provider;
        }

        public async Task Invoke(HttpContext context)
            => await context.Response.WriteAsync(
                await _provider.GetVersionInformationAsync());
    }
}
