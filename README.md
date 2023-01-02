# logsnag.net

[![CI](https://github.com/joaope/logsnag.net/actions/workflows/ci.yml/badge.svg)](https://github.com/joaope/logsnag.net/actions/workflows/ci.yml)

LogSnag .NET client

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
var client = new LogSnagHttpClient("token");
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
