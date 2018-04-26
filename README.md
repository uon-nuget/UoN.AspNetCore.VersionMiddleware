# UoN.AspNetCore.VersionMiddleware

[![License](https://img.shields.io/badge/licence-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://travis-ci.org/UniversityOfNottingham/UoN.AspNetCore.VersionMiddleware.svg?branch=develop)](https://travis-ci.org/UniversityOfNottingham/UoN.AspNetCore.VersionMiddleware)
[![NuGet](https://img.shields.io/nuget/v/UoN.AspNetCore.VersionMiddleware.svg)](https://www.nuget.org/packages/UoN.AspNetCore.VersionMiddleware/)


## What is it?

This is middleware for ASP.Net Core designed to report on version information related to your project.

We use it at UoN so that we can check the version of a web app wherever it's deployed, without having to display it publicly to users who don't care. This is useful for ensuring testers know which builds they're working with and therefore what fixes to test.

## What are its features?

### Middleware Extension Methods
It provides three `IApplicationBuilder` Extension methods for you to use in `Startup.Configure()`:

- `app.UseVersion([provider|object])`
  - adds a `/version` route to the ASP.Net Core pipeline.
  - returns one of the following:
    - if an implementation of `IVersionInformationProvider` was passed, returns the result of its `GetVersionInformationAsync()` method serialized as JSON
    - if an object that **does not** implement `IVersionInformationProvider` was passed, returns that object serialized as JSON
    - otherwise returns the `AssemblyInformationalVersion` of the entry assembly (i.e. containing this `Startup` class.) as a JSON string
- `app.MapVersion(path, [provider|object])`
  - behaves as above but with a custom route path
- `app.MapVersion(path, assembly)`
  - provides a custom route path
  - returns `AssemblyInformationalVersion` for the specified assembly as a JSON string

### Version Information Providers

It also provides three basic implementations of `IVersionInformationProvider`.

#### AssemblyInformationalVersionProvider

This provides the default behaviour of each extension method, and is the behaviour the package used in version 1.0.0.

It simply gets the `AssemblyInformationalVersion` of a given .NET Assembly and returns it as a string.

You're unlikely to use this directly except as part of an `AggregateProvider` configuration, because you can instead use the default extension method behaviours as follows:

```csharp
// this extension method usage:
app.UseVersion();
app.MapVersion("/route");
app.MapVersion("/route", MyAssembly);

// is equivalent to
app.UseVersion(new AssemblyInformationalVersionProvider());
app.MapVersion("/route", new AssemblyInformationalVersionProvider());
app.MapVersion("/route", new AssemblyInformationalVersionProvider(MyAssembly));
```

## Dependencies

The library targets `netstandard2.0` and depends upon ASP.NET Core 2.0.

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
