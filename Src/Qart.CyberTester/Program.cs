namespace Qart.CyberTester
{
    class Program
    {
        static int Main(string[] args)
        {
            var options = CommandLineOptions.Parse(args);
            var cyberTester = CyberTesterFactory.Build(options);
            return options.List ? cyberTester.ListTests() : cyberTester.RunTests();
        }
    }
}
