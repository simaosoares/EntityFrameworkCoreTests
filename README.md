# Entity Framework Core Tests

Sample solution featuring entity framework core unit tests.

Table of contents:

* [Create Solution](#create-solution)
* [Project List](#project-list)
    * [EntityFrameworkCoreTests.Data](#entityframeworkcoretestsdata)
    * [EntityFrameworkCoreTests.Services](#entityframeworkcoretestsservices)
* [Build Solution](#build-solution)
* [Test Solution](#test-solution)
* [References](#references)

## Create Solution

Type the following to initialize an empty solution:

```
mkdir EntityFrameworkCoreTests
cd EntityFrameworkCoreTests
dotnet new sln
```

## Project List

The solution is composed by the following projects:

* [EntityFrameworkCoreTests.Data](#entityframeworkcoretestsdata)
* [EntityFrameworkCoreTests.Services](#entityframeworkcoretestsservices)
* [EntityFrameworkCoreTests.Tests](#entityframeworkcoreteststests)

### EntityFrameworkCoreTests.Data 

This project contains all the required entities and the database context.

Type the following steps to add the `EntityFrameworkCoreTests.Data` project to the existing solution.

```
dotnet new classlib -n EntityFrameworkCoreTests.Data -f netcoreapp2.2
dotnet sln add EntityFrameworkCoreTests.Data
```
The option `-f netcoreapp2.2` specifies the target framework for the project.  

The following line will add the reference to `Microsoft.EntityFrameworkCore` package to the project necessary to build the database context.  

```
dotnet add package Microsoft.EntityFrameworkCore
```

### EntityFrameworkCoreTests.Services 

This project contains the application services.

Type the following steps to add the `EntityFrameworkCoreTests.Services` project to the existing solution.

```
dotnet new classlib -n EntityFrameworkCoreTests.Services -f netcoreapp2.2
dotnet sln add EntityFrameworkCoreTests.Services
```

Since the project needs a reference to `EntityFrameworkCoreTests.Data` type the following command on the  `EntityFrameworkCoreTests.Services` root folder to add the dependency.

```
dotnet add reference ../EntityFrameworkCoreTests.Data/EntityFrameworkCoreTests.Data.csproj
```

The file `EntityFrameworkCoreTests.Services.csproj` is updated with the required reference: 
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <ProjectReference Include="..\EntityFrameworkCoreTests.Data\EntityFrameworkCoreTests.Data.csproj" />
  </ItemGroup>

</Project>
```

## Build Solution

```
dotnet clean
dotnet build
```

```
dotnet test
```

# References

* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new
* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-sln
* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package
* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-reference