using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace UoN.AspNetCore.VersionMiddleware
{
    /// <summary>
    /// ASP.NET Core middleware for providing a GET endpoint
    /// which returns version information.
    /// </summary>
    public class VersionMiddleware
    {
        private readonly IVersionInformationProvider _provider;

        /// <summary>
        /// Construct an instance of the middleware.
        /// </summary>
        /// <param name="next">See ASP.NET Core Docs.</param>
        /// <param name="provider">
        /// Some kind of version provider which will be used to provide the output of the middleware.
        /// </param>
        public VersionMiddleware(RequestDelegate next, IVersionInformationProvider provider)
        {
            _provider = provider;
        }

        public async Task Invoke(HttpContext context)
        {
            // make it clear that we're always returning json, even if it's just a string
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(await _provider.GetVersionInformationAsync()));
        }
    }
}
