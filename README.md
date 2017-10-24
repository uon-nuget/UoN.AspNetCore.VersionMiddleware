# UoN.AspNetCore.VersionMiddleware

[![License](https://img.shields.io/badge/licence-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://travis-ci.org/UniversityOfNottingham/UoN.AspNetCore.VersionMiddleware.svg?branch=develop)](https://travis-ci.org/UniversityOfNottingham/UoN.AspNetCore.VersionMiddleware)

## What is it?

This is middleware for ASP.Net Core designed to report on the assembly informational version of your web project's assembly.

We use it at UoN so that we can check the version of a web app wherever it's deployed, without having to display it publicly to users who don't care. This is useful for ensuring testers know which builds they're working with and therefore what fixes to test.

## What are its features?

It provides three `IApplicationBuilder` Extension methods for you to use in `Startup.Configure()`:

- `app.UseVersion()`
  - adds a `/version` route to the ASP.Net Core pipeline.
  - this returns as plaintext the informational version of the entry assembly (i.e. containing this `Startup` class.)
- `app.MapVersion(path)`
  - adds a route with the specified path string to the ASP.Net Core pipeline
  - behaves otherwise the same as above
- `app.MapVersion(path, assembly)`
  - behaves the same as above, but gives the version of the specified assembly instead of the entry assembly.

## Dependencies

The library targets `netstandard2.0` and depends upon ASP.Net Core 2.0.

If you can use ASP.Net Core 2, you can use this library.

## Usage

Acquire the library via one of the methods below, and add one of the above extension methods to your ASP.Net Core pipeline.

### NuGet

This library will be hosted on nuget.org from `1.0.0` at the latest.

### Build from source

We recommend building with the `dotnet` cli, but since the package targets `netstandard2.0` and depends only on ASP.Net Core 2.0, you should be able to build it in any tooling that supports those requirements.

- Have the .NET Core SDK 2.0 or newer
- `dotnet build`
- Optionally `dotnet pack`
- Reference the resulting assembly, or NuGet package.

### An example:

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

## Contributing

Contributions are unlikely to be needed often as this is a library with a very specific purpose.

If there are issues open, please feel free to make pull requests for them, and they will be reviewed.
