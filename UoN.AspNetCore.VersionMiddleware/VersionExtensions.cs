using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using UoN.VersionInformation.Providers;

namespace UoN.AspNetCore.VersionMiddleware
{
    /// <summary>
    /// Extension methods to make using the VersionMiddleware easier.
    /// </summary>
    public static class VersionExtensions
    {
        // This extension is limited to Assembly params to preserve 1.0.0 compatibility
        /// <summary>
        /// Adds a custom endpoint to the pipeline,
        /// responding only to GET requests with the
        /// `AssemblyInformationalVersion` string from the specified Assembly.
        /// </summary>
        /// <param name="app">The `IApplicationBuilder` instance from `Startup.Configure()`.</param>
        /// <param name="path">The endpoint path to use.</param>
        /// <param name="assembly">The .NET Assembly to get `AssemblyInformationalVersion` data from.</param>
        /// <returns>The `IApplicationBuilder` instance</returns>
        public static IApplicationBuilder MapVersion(this IApplicationBuilder app, string path, Assembly assembly)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.Map(path, appBuilder =>
                appBuilder.UseMiddleware<VersionMiddleware>(
                    new AssemblyInformationalVersionProvider(assembly)));
        }

        /// <summary>
        /// Adds a custom endpoint to the pipeline,
        /// responding only to GET requests with the output
        /// from the specified version source.
        /// </summary>
        /// <param name="app">The `IApplicationBuilder` instance from `Startup.Configure()`.</param>
        /// <param name="path">The endpoint path to use.</param>
        /// <param name="source">
        /// An optional source supported by the configured VersionInformationService.
        /// 
        /// Defaults to Entry Assembly Informational Version.
        /// </param>
        /// <returns>The `IApplicationBuilder` instance</returns>
        public static IApplicationBuilder MapVersion(
            this IApplicationBuilder app,
            string path,
            object source = null)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            // switch on what we've been passed
            // to choose appropriate shortcut behaviours
            switch (source ?? Assembly.GetEntryAssembly()) // default to 1.0.0 behaviour
            {
                // If we've been passed an assembly, use the assembly extension above.
                // This is a good safety in case Assembly Type Handlers have been cleared
                // from the configured service.
                case Assembly a:
                    return app.MapVersion(path, a);

                // Otherwise just let the middleware deal with it as configured
                default:
                    return app.Map(path, appBuilder =>
                        appBuilder.UseMiddleware<VersionMiddleware>(source));
            }
        }

        /// <summary>
        /// Adds the endpoint `/version` to the pipeline,
        /// responding only to GET requests with the output
        /// from the specified version source.
        /// </summary>
        /// <param name="app">The `IApplicationBuilder` instance from `Startup.Configure()`.</param>
        /// <param name="source">
        /// An optional source supported by the configured VersionInformationService.
        /// 
        /// Defaults to Entry Assembly Informational Version.
        /// </param>
        /// <returns>The `IApplicationBuilder` instance</returns>
        public static IApplicationBuilder UseVersion(
            this IApplicationBuilder app,
            object source = null)
            => app?.MapVersion("/version", source)
               ?? throw new ArgumentNullException(nameof(app));
    }
}
