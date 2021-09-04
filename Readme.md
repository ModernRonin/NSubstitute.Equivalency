# NSubstitute.Equivalency
![CI Status](https://github.com/ModernRonin/NSubstitute.Equivalency/actions/workflows/dotnet.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/NSubstituteEquivalency.svg)](https://www.nuget.org/packages/NSubstituteEquivalency/)
[![NuGet](https://img.shields.io/nuget/dt/NSubstituteEquivalency.svg)](https://www.nuget.org/packages/NSubstituteEquivalency)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com) 

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

## Release History
2.0.0
* breaking change: upgraded to FluentAssertions 6.x

1.1.0:
* feature: added ArgEx.IsCollectionEquivalentTo()

1.0.0: initial nuget release


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

or 

```csharp
var service = Substitute.For<ISomeInterface>();
DoSomethingWith(service);

service.Received()
    .UseCollection(ArgEx.IsCollectionEquivalentTo(new []
    {
        new Person(){FirstName = "Alice", LastName = "Wonderland", Birthday = new DateTime(1968, 6, 1)},
        new Person(){FirstName = "Bob", LastName = "Peanut", Birthday = new DateTime(1972, 9, 13)},
    }));
```

