using System;
using Common.Logging;
using Qart.Core.DataStore;
using Qart.Core.Text;
using Qart.Testing;
using System.Linq;

namespace Qart.CyberTester
{
    static class CyberTesterFactory
    {
        static readonly ILog Logger = LogManager.GetLogger("");

        static public CyberTester Build(CommandLineOptions options)
        {
            Logger.DebugFormat("Rebaseline [{0}], TestCases [{1}]", options.Rebaseline, options.Dir);
            var container = Bootstrapper.CreateContainer(new FileBasedDataStore(options.Dir));
            var testSystem = container.Resolve<ITestSystem>();
            var customSession = container.Kernel.HasComponent(typeof(ITestSession)) ? container.Resolve<ITestSession>() : null;
            var tester = new Tester(testSystem, container.Resolve<ITestCaseProcessorResolver>(), 
                container.Resolve<ITestCaseLoggerFactory>(), container.Resolve<ILogManager>());
            var parsedOptions = options.Options.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .ToDictionary(_ => _.LeftOf("="), _ => _.RightOf("="));
            return new CyberTester(tester, customSession, parsedOptions);
        }
    }
}
