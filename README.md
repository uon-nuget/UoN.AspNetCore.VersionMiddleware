# UoN.AspNetCore.VersionMiddleware

[![License](https://img.shields.io/badge/licence-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://travis-ci.org/uon-nuget/UoN.AspNetCore.VersionMiddleware.svg?branch=master)](https://travis-ci.org/uon-nuget/UoN.AspNetCore.VersionMiddleware)
[![NuGet](https://img.shields.io/nuget/v/UoN.AspNetCore.VersionMiddleware.svg)](https://www.nuget.org/packages/UoN.AspNetCore.VersionMiddleware/)


## What is it?

This is middleware for ASP.Net Core designed to report on version information related to your project.

We use it at UoN so that we can check the version of a web app wherever it's deployed, without having to display it publicly to users who don't care. This is useful for ensuring testers know which builds they're working with and therefore what fixes to test.

## What are its features?

It exposes the version output of [UoN.VersionInformation](https://github.com/uon-nuget/UoN.VersionInformation) as JSON data at an http endpoint.

### Middleware Extension Methods
It provides three `IApplicationBuilder` Extension methods for you to use in `Startup.Configure()`:

- `app.UseVersion(source)`
  - adds a `/version` route to the ASP.Net Core pipeline.
  - expects a valid source accepted by `VersionInformationService`
  - if `source` is `null` then defaults to using [`UoN.VersionInformation.Providers.AssemblyInformationalVersionProvider`](https://github.com/uon-nuget/UoN.VersionInformation), which in turn uses `AssemblyInformationalVersion` from the current assembly's metadata.
- `app.MapVersion(path, source)`
  - behaves as above but with a custom route path
- `app.MapVersion(path, assembly)`
  - provides a custom route path
  - returns `AssemblyInformationalVersion` for the specified assembly as a JSON string

## Dependencies

The library targets `netstandard2.0` and depends upon ASP.NET Core 2.0 and UoN.VersionInformation.

If you can use ASP.NET Core 2, you can use this library.

## Example usage

#### `Startup.cs`

``` csharp
public class Startup
{
  ...

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IHostingEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseVersion(); //adds `/version` to the pipeline, so if that endpoint is requested, the pipeline will short circuit here
    
    app.UseStaticFiles();

    app.UseMvc(routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
    });
  }
}
```

## Building from source

We recommend building with the `dotnet` cli, but since the package targets `netstandard2.0` and depends only on ASP.Net Core 2.0, you should be able to build it in any tooling that supports those requirements.

- Have the .NET Core SDK 2.0 or newer
- `dotnet build`
- Optionally `dotnet pack`
- Reference the resulting assembly, or NuGet package.

## Contributing

Contributions are unlikely to be needed often as this is a library with a very specific purpose.

If there are issues open, please feel free to make pull requests for them, and they will be reviewed.
