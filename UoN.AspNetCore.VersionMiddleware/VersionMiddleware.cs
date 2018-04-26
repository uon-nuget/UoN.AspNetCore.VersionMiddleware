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
        {
            // make it clear that we're always returning json, even if it's just a string
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                await _provider.GetVersionInformationAsync());
        }
    }
}
