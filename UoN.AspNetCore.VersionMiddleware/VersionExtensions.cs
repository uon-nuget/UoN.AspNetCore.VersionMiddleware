using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using UoN.AspNetCore.VersionMiddleware.Providers;

namespace UoN.AspNetCore.VersionMiddleware
{
    public static class VersionExtensions
    {
        public static IApplicationBuilder MapVersion(this IApplicationBuilder app, string path, Assembly assembly)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.Map(path, appBuilder =>
                appBuilder.UseMiddleware<VersionMiddleware>(
                    new AssemblyInformationalVersionProvider(assembly)));
        }

        public static IApplicationBuilder MapVersion(
            this IApplicationBuilder app,
            string path,
            IVersionInformationProvider provider = null)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            if (provider == null)
                // essentially default to AssemblyInformationalVersionProvider
                // with the EntryAssembly, as per 1.0.0 behaviour
                return app.MapVersion(path, Assembly.GetEntryAssembly());

            return app.Map(path, appBuilder =>
                appBuilder.UseMiddleware<VersionMiddleware>(provider));
        }


        public static IApplicationBuilder UseVersion(
            this IApplicationBuilder app,
            IVersionInformationProvider provider = null)
            => app?.MapVersion("/version", provider)
               ?? throw new ArgumentNullException(nameof(app));
    }
}
