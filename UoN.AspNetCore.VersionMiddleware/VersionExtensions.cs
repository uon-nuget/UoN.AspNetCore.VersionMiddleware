using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using UoN.AspNetCore.VersionMiddleware.Providers;

namespace UoN.AspNetCore.VersionMiddleware
{
    /// <summary>
    /// Extension methods to make using the VersionMiddleware easier.
    /// </summary>
    public static class VersionExtensions
    {
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
        /// from the specified `IVersionInformationProvider`.
        /// </summary>
        /// <param name="app">The `IApplicationBuilder` instance from `Startup.Configure()`.</param>
        /// <param name="path">The endpoint path to use.</param>
        /// <param name="provider">
        /// An optional `IVersionInformationProvider` to provide the response body.
        /// 
        /// Alternatively can be an arbitrary .NET Object to be used as version information.
        /// 
        /// If none is specified, `AssemblyInformationVersonProvider` is used on the Entry Assembly.
        /// </param>
        /// <returns>The `IApplicationBuilder` instance</returns>
        public static IApplicationBuilder MapVersion(
            this IApplicationBuilder app,
            string path,
            object provider = null)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            if (provider == null)
                // essentially default to AssemblyInformationalVersionProvider
                // with the EntryAssembly, as per 1.0.0 behaviour
                return app.MapVersion(path, Assembly.GetEntryAssembly());

            // If we've been given an object that is NOT a provider implementation
            // assume it is intended to be version information
            // and use it with an ObjectProvider
            if (!(provider is IVersionInformationProvider))
                provider = new ObjectProvider(provider);

            return app.Map(path, appBuilder =>
                appBuilder.UseMiddleware<VersionMiddleware>(provider));

        }

        /// <summary>
        /// Adds the endpoint `/version` to the pipeline,
        /// responding only to GET requests with the output
        /// from the specified `IVersionInformationProvider`.
        /// </summary>
        /// <param name="app">The `IApplicationBuilder` instance from `Startup.Configure()`.</param>
        /// <param name="provider">
        /// An optional `IVersionInformationProvider` to provide the response body.
        /// 
        /// Alternatively can be an arbitrary .NET Object to be used as version information.
        /// 
        /// If none is specified, `AssemblyInformationVersonProvider` is used on the Entry Assembly.
        /// </param>
        /// <returns>The `IApplicationBuilder` instance</returns>
        public static IApplicationBuilder UseVersion(
            this IApplicationBuilder app,
            object provider = null)
            => app?.MapVersion("/version", provider)
               ?? throw new ArgumentNullException(nameof(app));
    }
}
