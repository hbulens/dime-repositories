# Dime.Repositories

![Build Status](https://dev.azure.com/dimenicsbe/Utilities/_apis/build/status/Dime.Repositories?branchName=master)

## Introduction

Implementation of the repository pattern with Entity Framework (Core).

## Getting Started

- You must have Visual Studio 2019 Community or higher.
- The dotnet cli is also highly recommended.

## About this project

Generic repository pattern with an implementation using Entity Framework. This project revolves around the `IRepository<T>` interface. This interfaces defines the capabilities of a repository which - rather unsurprisingly - are simple CRUD operations.

In addition, this project is also concerned with instantiating the repositories. Rather than accessing the repository's implementation directly, a repository factory (defined by `IRepositoryFactory`) can be used and injected into the application. Support for multi-tenancy is built-in with the `IMultiTenantRepositoryFactory` interface.

The projects in the `Providers` folder provide the implementation of the contracts defined in the Dime.Repositories assembly.

## Build and Test

- Run dotnet restore
- Run dotnet build
- Run dotnet test

## Installation

Use the package manager NuGet to install Dime.Repositories:

`dotnet add package Dime.Repositories`

## Usage

Here's a simple example which demonstrates the usage of the repository.

``` csharp
using Dime.Repositories;
...

public class CustomerService
{
  private readonly IRepositoryFactory _repositoryFactory;

  public CustomerService(IRepositoryFactory repositoryFactory)
  {
    _repositoryFactory = repositoryFactory;
  }

  public async IEnumerable<Customer> GetCustomers()
  {
      using IRepository<Customer> customerRepository = _repositoryFactory.Create<Customer>();
      return await customerRepository.FindAllAsync(x => x.IsActive == true);
  }
}

```

This is an example of the dependency injection registration in Unity:

```csharp
public sealed class UnityConfig
{
   public static void RegisterTypes(IUnityContainer container)
   {
     container.RegisterType<IMultiTenantEfRepositoryFactory, EfRepositoryFactory<MyDbContext>>(
         new PerRequestOrTransientLifeTimeManager(),
         new InjectionConstructor(new MyDbContextEfContextFactory()));
   }
}

public class MyDbContextEfContextFactory : MultiTenantContextFactory<MyDbContext>
{
    ...

    protected override SchedulerContext ConstructContext()
    {
        MyDbContext ctx = new MyDbContext();
        ctx.Configuration.ProxyCreationEnabled = false;
        ctx.Configuration.LazyLoadingEnabled = false;
        ctx.Configuration.AutoDetectChangesEnabled = false;
        ctx.Configuration.UseDatabaseNullSemantics = true;
        ctx.Database.CommandTimeout = 60;        
        return ctx;
    }
}

```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)