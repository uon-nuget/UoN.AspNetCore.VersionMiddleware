using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using UoN.AspNetCore.VersionMiddleware.Providers;

namespace UoN.AspNetCore.VersionMiddleware
{
    /// <summary>
    /// ASP.NET Core middleware for providing a GET endpoint
    /// which returns version information.
    /// </summary>
    public class VersionMiddleware
    {
        private readonly object _provider;

        /// <summary>
        /// Construct an instance of the middleware.
        /// </summary>
        /// <param name="next">See ASP.NET Core Docs.</param>
        /// <param name="provider">
        /// An object that version information can be gotten from.
        /// Can be an Assembly to get `AssemblyInformationalVersion` from,
        /// or an `IVersionInformationProvider` to execute,
        /// or a regular .NET Object.
        /// 
        /// Defaults to Entry Assembly.
        /// </param>
        public VersionMiddleware(RequestDelegate next, object provider = null)
        {
            _provider = provider;
        }

        public async Task Invoke(HttpContext context)
        {
            // make it clear that we're always returning json, even if it's just a string
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(
                    GetObjectFromVersionSourceAsync(_provider)));
        }

        /// <summary>
        /// Used to get Version Information objects from the built in providers
        /// based on arbitrary object types.
        /// </summary>
        /// <param name="source">The version information source.</param>
        /// <returns>An object containing the actual version information.</returns>
        public static async Task<object> GetObjectFromVersionSourceAsync(object source)
        {
            // switch on what we've been passed
            // to choose appropriate shortcut behaviours
            switch (source ?? Assembly.GetEntryAssembly()) // default to 1.0.0 behaviour
            {
                // if we've been passed an assembly, use the assembly provider
                case Assembly a:
                    return await new AssemblyInformationalVersionProvider(a).GetVersionInformationAsync();

                // if we've been passed a provider, use it directly
                case IVersionInformationProvider p:
                    return await p.GetVersionInformationAsync();

                // If we've been given an object that doesn't match the above
                // assume it is intended to be version information
                // and use it as is
                default:
                    return source;
            }
        }
    }
}
