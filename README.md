# Dime.Repositories

![Build Status](https://dev.azure.com/dimenicsbe/Utilities/_apis/build/status/Repositories%20-%20MAIN%20-%20CI?branchName=master)

## Introduction

Implementation of the repository pattern with Entity Framework (Core).

## Getting Started

- You must have Visual Studio 2019 Community or higher.
- The dotnet cli is also highly recommended.

## About this project

Generic repository pattern with an implementation using Entity Framework.

## Build and Test

- Run dotnet restore
- Run dotnet build
- Run dotnet test

## Installation

Use the package manager NuGet to install Dime.Repositories:

`dotnet add package Dime.Repositories`

## Usage

``` csharp
[HttpPost]
[Route(Routes.Appointments.Get)]
public async Task<IPage<BackOfficeAppointmentDto>> Get([FromBody]DataSourceRequest request)
    => await Service.GetAsync(request.Take, request.Skip, request.Page, request.PageSize, request.Filter, request.Sort);
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)