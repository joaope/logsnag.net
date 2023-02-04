<!-- omit from toc -->
# LogSnag.NET

[LogSnag](https://logsnag.com) client for .NET

[![CI](https://github.com/joaope/logsnag.net/actions/workflows/ci.yml/badge.svg)](https://github.com/joaope/logsnag.net/actions/workflows/ci.yml)
[![Publish Pre-Release](https://github.com/joaope/logsnag.net/actions/workflows/publish-pre.yml/badge.svg)](https://github.com/joaope/logsnag.net/actions/workflows/publish-pre.yml)
[![Publish Release](https://github.com/joaope/logsnag.net/actions/workflows/publish-stable.yml/badge.svg)](https://github.com/joaope/logsnag.net/actions/workflows/publish-stable.yml)

| Package  | NuGet | MyGet (Pre-Releases) |
| - | :-: | :-: |
| *LogSnag.NET* | [![NuGet](https://img.shields.io/nuget/v/LogSnag.NET.svg)](https://www.nuget.org/packages/LogSnag.NET/) | [![MyGet](https://img.shields.io/myget/logsnag/v/LogSnag.NET.svg)](https://www.myget.org/feed/logsnag/package/nuget/LogSnag.NET) |
| *LogSnag.NET.Extensions.Microsoft.DependencyInjection* | [![NuGet](https://img.shields.io/nuget/v/LogSnag.NET.Extensions.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/LogSnag.NET.Extensions.Microsoft.DependencyInjection/) | [![NuGet](https://img.shields.io/myget/logsnag/v/LogSnag.NET.Extensions.Microsoft.DependencyInjection.svg)](https://www.myget.org/feed/logsnag/package/nuget/LogSnag.NET.Extensions.Microsoft.DependencyInjection) |

- [Install](#install)
- [How to use](#how-to-use)
  - [Publish event](#publish-event)
  - [Publish insight](#publish-insight)
- [Dependency Injection (`IHttpClientFactory`)](#dependency-injection-ihttpclientfactory)

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

Or with your own HttpClient

```c#
ILogSnagClient client = new LogSnagClient(new HttpClient(), "token");
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

## Dependency Injection (`IHttpClientFactory`)

Given you're in an application environment with access to an `IServiceCollection` container you can use this package instead:

```bash
dotnet add package LogSnag.NET.Extensions.Microsoft.DependencyInjection
```

Which allows one to register `ILogSnagClient` coupled to a [managed HttpClient](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests):

```c#
builder.Services.AddLogSnagClient("token");
```
