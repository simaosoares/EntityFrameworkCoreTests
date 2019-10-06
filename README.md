# Entity Framework Core Tests

Sample solution featuring entity framework core unit tests.

Table of contents:

* [Create Solution](#create-solution)
* [Project List](#project-list)
    * [EntityFrameworkCoreTests.Data](#entityframeworkcoretestsdata)
    * [EntityFrameworkCoreTests.Services](#entityframeworkcoretestsservices)
    * [EntityFrameworkCoreTests.Tests](#entityframeworkcoreteststests)    
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

### EntityFrameworkCoreTests.Tests

This project contains the application persistence and services unit tests for the libraries `EntityFrameworkCoreTests.Data` and `EntityFrameworkCoreTests.Services` using the `XUnit` unit testing tool combined with an in-memory database.

Type the following steps to add the `EntityFrameworkCoreTests.Tests` project to the existing solution.

```
dotnet new xunit -n EntityFrameworkCoreTests.Tests
dotnet sln add EntityFrameworkCoreTests.Tests
```

The following commands will add the required project dependencies:

```
dotnet add reference ../EntityFrameworkCoreTests.Data/EntityFrameworkCoreTests.Data.csproj
dotnet add reference ../EntityFrameworkCoreTests.Services/EntityFrameworkCoreTests.Services.csproj
```

The InMemory provider is useful to test components using something that approximates connecting to the real database, without the overhead of actual database operations.


> EF Core database providers do not have to be relational databases. InMemory is designed to be a general purpose database for testing, and is not designed to mimic a relational database.

* InMemory will allow you to save data that would violate referential integrity constraints in a relational database.
* If you use DefaultValueSql(string) for a property in your model, this is a relational database API and will have no effect when running against InMemory.


The following command will add the required package to run our tests with an in-memory database.

```
dotnet add package Microsoft.EntityFrameworkCore.InMemory -v 2.2.6
```


The following command will add the required package to run our tests with a relational database `Sqlite` that will be configured to run as in-memory data source.

```
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```
 
```c#
// ...
new SqliteConnection("DataSource=:memory:");
// ...
```

## Build Solution

```
dotnet clean
dotnet build
```

## Test Solution

Type the following command to run the solution unit tests:   

```
dotnet test
```

# References

* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new
* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-sln
* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package
* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-reference
* https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory
* https://docs.microsoft.com/en-us/ef/core/providers/in-memory/
* https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/
* https://xunit.net/#projects
* https://www.meziantou.net/testing-ef-core-in-memory-using-sqlite.htm