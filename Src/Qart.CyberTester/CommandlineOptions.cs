using CommandLine;
using System.IO;
using CommandLine.Text;

namespace Qart.CyberTester
{
    class CommandLineOptions
    {
        [Option('d', "dir", Required = false, HelpText = "Path to the directory(s) with test case(s).")]
        public string Dir { get; set; }

        [Option('r', "rebaseline", Required = false, HelpText = "Overwrites expected results.")]
        public bool Rebaseline { get; set; }

        [Option('o', "options", Required = false, HelpText = "Custom options in format '<name>=<value>;<name>=<value>'", DefaultValue = "")]
        public string Options { get; set; }

        [Option('l', "list", Required = false, HelpText = "Lists all test cases given current configuration.")]
        public bool List { get; set; }

        [HelpOption]
        public string GetUsage() {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public static CommandLineOptions Parse(string[] args)
        {
            var options = new CommandLineOptions();
            if (Parser.Default.ParseArgumentsStrict(args, options))
            {
                if (string.IsNullOrEmpty(options.Dir))
                {
                    options.Dir = Directory.GetCurrentDirectory();
                }
            }
            return options;
        }
    }
}

