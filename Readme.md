# NSubstitute.Equivalency

[![NuGet](https://img.shields.io/nuget/v/NSubstituteEquivalency.svg)](https://www.nuget.org/packages/NSubstituteEquivalency/)
[![NuGet](https://img.shields.io/nuget/dt/NSubstituteEquivalency.svg)](https://www.nuget.org/packages/NSubstituteEquivalency)

## Motivation
For an intro what this library is solving, check out my [blog article](https://modernronin.github.io/2021/01/06/NSubstitute-and-equivalency-argument-matching/).

## Get it
```shell
dotnet add package NSubstituteEquivalency
```
or
```shell
dotnet paket add NSubstituteEquivalency
```

## Use it
```csharp
var service = Substitute.For<ISomeInterface>();
DoSomethingWith(service);

service.Received()
    .Use(ArgEx.IsEquivalentTo(new Person
    {
        FirstName = "John",
        LastName = "Doe",
        Birthday = new DateTime(1968, 6, 1)
    }));
```

or

```csharp
var service = Substitute.For<ISomeInterface>();
DoSomethingWith(service);

service.Received()
    .Use(ArgEx.IsEquivalentTo(new Person
    {
        FirstName = "John",
        LastName = "Doe"
    }, cfg => cfg.Excluding(p => p.Birthday)));
```
