using NetLah.Diagnostics;
using NetLah.Extensions.CommandLineUtils;

var libAsmInfo = new AssemblyInfo(typeof(CommandLineApplication).Assembly);

// CommandLineApp.exe [hello <fullname>] [-?]
CommandLineApplication commandLineApplication =
    new(throwOnUnexpectedArg: true)
    {
        Name = "CommandLineApp.exe",
        FullName = "CommandLineApp: greeting a fullname"
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

string GetShortVersion() => typeof(CommandLineApplication).Assembly.GetName().Version?.ToString() ?? "1.0.0";

string GetLongVersion() => libAsmInfo.InformationalVersion;
