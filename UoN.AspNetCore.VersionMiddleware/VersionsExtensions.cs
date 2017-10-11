using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace UoN.NetCore.VersionExtensionMiddleware
{
    public static class VersionExtensions
    {
        public static IApplicationBuilder MapVersion(this IApplicationBuilder app, string path, Assembly assembly)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.Map(path, appBuilder =>
            {
                appBuilder.UseMiddleware<VersionMiddleware>(assembly);
            });
        }

        public static IApplicationBuilder MapVersion(this IApplicationBuilder app, string path)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.MapVersion(path, Assembly.GetEntryAssembly());
        }

        public static IApplicationBuilder UseVersion(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.MapVersion("/version");
        }
    }
}
