using Common.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Qart.Testing
{
    public class Tester
    {
        private readonly ITestSystem _testSystem;
        private readonly ITestCaseLoggerFactory _testCaseLoggerFactory;
        private readonly ITestCaseProcessorResolver _processorResolver;
        private readonly ILogManager _logManager;

        public Tester(ITestSystem testSystem, ITestCaseProcessorResolver processorResolver, 
            ITestCaseLoggerFactory testCaseLoggerFactory, ILogManager logManager)
        {
            _testSystem = testSystem;
            _testCaseLoggerFactory = testCaseLoggerFactory;
            _processorResolver = processorResolver;
            _logManager = logManager;
        }

        public IEnumerable<TestCase> DiscoverTests() 
        {
            return _testSystem.GetTestCaseIds().Select(_ => _testSystem.GetTestCase(_));
        }

        public IEnumerable<TestCaseResult> RunTests(ITestSession customSession, IDictionary<string, string> options)
        {
            using (var testSession = new TestSession(customSession, _processorResolver, _testCaseLoggerFactory, _logManager, options))
            {
                foreach (var testCase in DiscoverTests())
                {
                    testSession.OnTestCase(testCase);
                }

                return testSession.Results;
            }
        }
    }
}
