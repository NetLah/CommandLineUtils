# NetLah.Extensions.CommandLineUtils - .NET Library

[NetLah.Extensions.CommandLineUtils](https://www.nuget.org/packages/NetLah.Extensions.CommandLineUtils/) Command line parsing for .NET.

## Nuget package

[![NuGet](https://img.shields.io/nuget/v/NetLah.Extensions.CommandLineUtils.svg?style=flat-square&label=nuget&colorB=00b200)](https://www.nuget.org/packages/NetLah.Extensions.CommandLineUtils/)

## Build Status

[![.NET](https://github.com/NetLah/CommandLineUtils/actions/workflows/dotnet.yml/badge.svg)](https://github.com/NetLah/CommandLineUtils/actions/workflows/dotnet.yml)

## Getting started

### 1. Add/Update PackageReference to web .csproj

```xml
<ItemGroup>
  <PackageReference Include="NetLah.Extensions.CommandLineUtils" />
</ItemGroup>
```

### 2. Sample Command Line Application

```cs
CommandLineApplication commandLineApplication =
    new(throwOnUnexpectedArg: false)
    {
        Name = "SampleCommandLine.exe",
        FullName = "SampleCommandLine: greeting a fullname"
    };

CommandArgument? names = null;

commandLineApplication.Command("hello",
    (target) =>
    {
        target.FullName = "Greeting a fullname";
        target.Description = "The hello command";
        target.ShortVersionGetter = GetShortVersion;
        target.LongVersionGetter = GetLongVersion;

        names = target.Argument(
            "fullname",
            "Enter the full name of the person to be greeted.",
            multipleValues: true);

        target.HelpOption("-? | -h | --help");

        target.OnExecute(() =>
        {
            if (names?.Values.Any() == true)
            {
                Greet(names.Values);
            }
            else
            {
                // show help if the required argument is missing
                commandLineApplication.ShowHelp();
            }
            return 0;
        });
    });

commandLineApplication.HelpOption("-? | -h | --help");
commandLineApplication.VersionOption("--version", GetShortVersion, GetLongVersion);

commandLineApplication.OnExecute(() =>
{
    // show help if root command
    commandLineApplication.ShowHelp();
    return 0;
});

commandLineApplication.Execute(args);

static void Greet(IEnumerable<string> values) => Console.WriteLine($"Hello {string.Join(" ", values)}!");

static string GetLongVersion() => "1.2.3";

static string GetShortVersion() => "v1.2.3+456abcd";
```

## Play around

### 1. Get help on root

To know the available commands and options `SampleCommandLine.exe --help`

```txt
SampleCommandLine: greeting a fullname v1.2.3+456abcd

Usage: SampleCommandLine.exe [options] [command]

Options:
  -? | -h | --help  Show help information
  --version         Show version information

Commands:
  hello  The hello command

Use "SampleCommandLine.exe [command] --help" for more information about a command.
```

### 2. Get help on hello command

To know the available arguments and options of a command `SampleCommandLine.exe hello --help`

```txt
Greeting a fullname v1.2.3+456abcd

Usage: SampleCommandLine.exe hello [arguments] [options]

Arguments:
  fullname  Enter the full name of the person to be greeted.

Options:
  -? | -h | --help  Show help information
```

### 3. Greeting a fullname

Command line `SampleCommandLine.exe hello John Doe`

```txt
Hello John Doe!
```

### 4. Check version

Command line `SampleCommandLine.exe --version`

```txt
SampleCommandLine: greeting a fullname
1.2.3
```

## Project origin and status

This repos a fork of [Microsoft.Extensions.CommandLineUtils](https://github.com/dotnet/extensions). [Microsoft announces](https://github.com/dotnet/extensions/issues/257) discontinue support the library. You may check this [repos](https://github.com/natemcmaster/CommandLineUtils) if prefer the more enrich features.
