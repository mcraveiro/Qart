using Qart.Testing;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using System.Xml.Linq;

namespace Qart.CyberTester
{
    class CyberTester
    {
        private const string ReportFilename = "TestSessionResults.xml";

        private readonly Tester tester_;
        private readonly ITestSession customSession_;
        private readonly Dictionary<string, string> parsedOptions_;
        static readonly ILog Logger = LogManager.GetLogger("");

        public CyberTester(Tester tester, ITestSession customSession, Dictionary<string, string> parsedOptions)
        {
            tester_ = tester;
            customSession_ = customSession;
            parsedOptions_ = parsedOptions;
        }

        private static XElement GenerateReportNode(TestCaseResult result)
        {
            return new XElement("Test", 
                new XAttribute("id", result.TestCase.Id),
                new XAttribute("status", result.Exception == null ? "succeeded" : "failed"),
                result.Description == null ? null : result.Description.Root);
        }

        private static void GenerateReport(IEnumerable<TestCaseResult> results)
        {
            var root = new XElement("TestResults", results.Select(_ => GenerateReportNode(_)));
            root.Save(ReportFilename);
        }

        public int ListTests()
        {
            var tests = tester_.DiscoverTests();
            if (!tests.Any()) {
                System.Console.WriteLine ("Tests: <no tests found>");
                return 0;
            }

            System.Console.WriteLine("Tests: ");
            foreach(var test in tests)
            {
                System.Console.WriteLine("    {0}", test.Id);
            }
            return 0;
        }

        public int RunTests()
        {
            var results = tester_.RunTests(customSession_, parsedOptions_);
            var failedTestsCount = results.Count(_ => _.Exception != null);
            Logger.InfoFormat("Tests execution finished. Number of failed testcases: {0}", failedTestsCount);
            GenerateReport(results);
            return failedTestsCount;
        }
    }
}