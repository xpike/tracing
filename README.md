# XPike.IoC

[![Build Status](https://dev.azure.com/xpike/xpike/_apis/build/status/xpike-ioc?branchName=master)](https://dev.azure.com/xpike/xpike/_build/latest?definitionId=2&branchName=master)
![Nuget](https://img.shields.io/nuget/v/XPike.IoC)

Provides interfaces to standardize IoC functionality across providers.

Strongly encourages the use of a Dependency Injection / Inversion of Control paradigm.

Also allows for a Service Location paradigm, by allowing the IDependencyContainer to be injected.  *This is not recommended, however, as it can introduce run-time binding errors which otherwise would have been caught using a purely-DI approach.*

## Components

### IDependencyContainer

Represents the Dependency Container which holds mappings between requested services and the configured implementation(s).

**NOTE:** Behavior may vary between providers.  For example, SimpleInjector validates all registrations at startup (assuming you are not injecting IDependencyContainer itself).  It also prohibits modifying registrations after the first call to `ResolveDependency`.

### IDependencyPackage

Provides a standard mechanism for registering multiple mappings (and other Packages) into a container, to simplify DI configuration.

A recommended pattern is to have one Package which represents the entire application, by including other Packages.

### PackageLoader

An optional utility to reduce startup time by skipping any Dependency `Package` which has already been registered.

#### Usage

##### Loading a Package during initialization

Wherever your application configures the `IDependencyContainer` (for example, `Startup.cs` in ASP.NET Core), just call `IDependencyContainer.LoadPackage(new Package());`.

##### Loading a Package from another Package

```cs
using XPike.IoC;

public class Package : IDependencyPackage
{
    void RegisterPackage(IDependencyContainer container)
    {
        container.LoadPackage(new XPike.Configuration.Package());
            
        // Registrations for your library go here.
    }
}
```

## Important!

Dependency injection itself is not wired up just by including this package - this only contains the basics for standardization.

A specific Provider package for the DI mechanism of your choice (eg: SimpleInjector, Microsoft.Extensions, etc) must also be loaded.

Additionally, if you are hosting inside ASP.NET, a host-specific (eg: IIS vs. NetCore) package must be loaded to handle controller registrations.

## Building and Testing

Building from source and running unit tests requires a Windows machine with:

- .Net Core 3.0 SDK
- .Net Framework 4.5.2 Developer Pack
- .Net Framework 4.6.1 Developer Pack

## Issues

Issues are tracked on [GitHub](https://github.com/xpike/xpike-ioc/issues). Anyone is welcome to file a bug,
an enhancement request, or ask a general question. We ask that bug reports include:

1. A detailed description of the problem
2. Steps to reproduce
3. Expected results
4. Actual results
5. Version of the package xPike
6. Version of the .Net runtime

## Contributing

See our [contributing guidelines](https://xpike.github.io/articles/contributing.html)
in our documentation for information on how to contribute to xPike.

## License

xPike is licensed under the [MIT License](LICENSE).
