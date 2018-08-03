using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TRXunit {
    internal static class TestExtensions {
        public static Test SetCreationTime(this Test test, XElement trx) {
            var xTimes = trx.Element(Constants.XN + "Times");
            test.CreationTime = xTimes.GetDateTime("creation")?.ToString("MM/dd/yyyy HH:mm:ss");
            return test;
        }

        public static Test SetAssemblies(this Test test, XElement trx) {
            test = test.SetAssembly(trx);
            return test;
        }

        public static Test SetAssembly(this Test test, XElement trx) {
            var xTimes = trx.Element(Constants.XN + "Times");
            var dt = xTimes.GetDateTime("creation");
            var start = xTimes.GetDateTime("start");
            var finish = xTimes.GetDateTime("finish");
            var time = finish - start;
            var timeString = time == null ? "0" : time?.TotalSeconds.ToString();


            var xTestDefinitions = trx.Element(Constants.XN + "TestDefinitions");
            var xUnitTest = xTestDefinitions.Elements(Constants.XN + "UnitTest").First();

            var xResults = trx.Element(Constants.XN + "ResultSummary");
            var xCounters = xResults.Element(Constants.XN + "Counters");

            var assembly = new Assembly() {
                Name = (string)xUnitTest.Attribute("storage"),
                Environment = "64-bit .NET 4.0.30319.42000",
                TestFramework = "MSTest.TestFramework 1.3.2",
                RunDate = dt?.ToString("yyyy-MM-dd"),
                RunTime = dt?.ToString("HH:mm:ss"),
                ConfigFile = (string)xUnitTest.Attribute("storage") + ".config",
                Total = xCounters.ReadInt("total").ToString(),
                Passed = xCounters.ReadInt("passed").ToString(),
                Failed = xCounters.ReadInt("failed").ToString(),
                Skipped = xCounters.ReadInt("notExecuted").ToString(),
                Time = timeString,
                Errors = xCounters.ReadInt("error").ToString(),
            };

            assembly = assembly.Setcollections(trx);
            test.Assemblies = new[] { assembly };
            return test;
        }

        public static Assembly Setcollections(this Assembly assembly, XElement trx) {
            var xUnitTestDefinitions = trx.Element(Constants.XN + "TestDefinitions").Elements(Constants.XN + "UnitTest");

            var testResults = GetTestRestults(trx, xUnitTestDefinitions).ToArray();
            var typeDictionary = (from testResult in testResults
                                  let key = testResult.Type
                                  let value = testResults.Where(x => x.Type == key)
                                  select (key, value)).ToDictionary(x => x.key, x => x.value);

            var collections = new List<Collection>();
            foreach (var testType in typeDictionary.Keys) {
                var tests = typeDictionary[testType];
                var totalTime = tests.Select(x => x.Time.GetValueOrDefault()).Aggregate((x , y) => x+y);
                var collection = new Collection() {
                    Total = tests.Count().ToString(),
                    Passed = tests.CountOfPassed().ToString(),
                    Failed = tests.CountOfFailed().ToString(),
                    Skipped = tests.CountOfSkipped().ToString(),
                    Name = "Test collection for " + testType,
                    Time = totalTime.TotalSeconds.ToString(),
                    Tests = tests
                };

                collections.Add(collection);
            }

            assembly.Collections = collections;
            assembly.ErrorDetails = Enumerable.Empty<ErrorDetail>();
            return assembly;
        }

        private static int CountOfSkipped(this IEnumerable<TestResult> tests) {
            return tests.CountOfResult("Skipped");
        }

        private static int CountOfFailed(this IEnumerable<TestResult> tests) {
            return tests.CountOfResult("Fail");
        }

        private static int CountOfPassed(this IEnumerable<TestResult> tests) {
            return tests.CountOfResult("Pass");
        }

        private static int CountOfResult(this IEnumerable<TestResult> tests, string result) {
            return tests.Where(test => test.Result == result).Count();
        }

        private static IEnumerable<TestResult> GetTestRestults(XElement trx, IEnumerable<XElement> xUnitTestDefinitions) { 
                foreach (var xUnitTestDefinition in xUnitTestDefinitions) {
                var xResult = trx.FindResult(xUnitTestDefinition.GetId());
                var time = xResult.GetElapsedTime();
                var name = (string)xUnitTestDefinition.Attribute("name");
                yield return new TestResult() {
                    Method = name,
                    Name = name,
                    Type = (string)xUnitTestDefinition.Element(Constants.XN + "TestMethod").Attribute("className"),
                    Time = time,
                    TimeString = time?.ToString(@"hh\:mm\:ss"),
                    Result = xResult.GetResult(),
                    Failure = xResult.GetErrors()
                };
            }
        }
    }
}
