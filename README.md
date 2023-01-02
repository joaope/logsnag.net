<!-- omit from toc -->
# logsnag.net

LogSnag .NET client

[![CI](https://github.com/joaope/logsnag.net/actions/workflows/ci.yml/badge.svg)](https://github.com/joaope/logsnag.net/actions/workflows/ci.yml)

- [Install](#install)
- [How to use](#how-to-use)
  - [Publish event](#publish-event)
  - [Publish insight](#publish-insight)
- [Dependency Injection (ASP.NET Core)](#dependency-injection-aspnet-core)

## Install

You should install with NuGet:

```bash
Install-Package LogSnag.NET
```

Or via the .NET Core command line interface:

```bash
dotnet add package LogSnag.NET
```

## How to use

```c#
ILogSnagClient client = new LogSnagClient("token");
```

### Publish event

```c#
await client.Publish(new LogSnagEvent("project", "channel", "event")
{
    Tags =
    {
        new LogSnagTag("tag-one", 1),
        new LogSnagTag("tag-two", "val2"),
        new LogSnagTag("tag-bool", true)
    },
    Icon = "🫡",
    Description = "This is an event",
    Notify = true,
    Parser = LogSnagParser.Text
});
```

### Publish insight

```c#
await client.Insight(new LogSnagInsight("project", "title", "value")
{
    Icon = "👌"
});
```

## Dependency Injection (ASP.NET Core)

Given you're in an application environment with access to an `IServiceCollection` container you can use this package instead:

```bash
dotnet add package LogSnag.NET.Extensions.Microsoft.DependencyInjection
```

Which allows one to register `ILogSnagClient` automatically coupled to a [managed HttpClient](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests):

```c#
builder.Services.AddLogSnagClient("token");
```