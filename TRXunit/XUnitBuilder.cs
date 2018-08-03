using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TRXunit {
    internal static class XUnitBuilder {
        internal static XElement Build(Test test) {
            var xUnit = new XElement("assemblies");
            xUnit.Add(new XAttribute("timestamp", test.CreationTime));

            foreach (var assembly in test.Assemblies) {
                var xAssembly = new XElement("assembly");
                xAssembly.Add(new XAttribute("name", assembly.Name));
                xAssembly.Add(new XAttribute("environment", assembly.Environment));
                xAssembly.Add(new XAttribute("test-framework", assembly.TestFramework));
                xAssembly.Add(new XAttribute("run-date", assembly.RunDate));
                xAssembly.Add(new XAttribute("run-time", assembly.RunTime));
                xAssembly.Add(new XAttribute("config-file", assembly.ConfigFile));
                xAssembly.Add(new XAttribute("total", assembly.Total));
                xAssembly.Add(new XAttribute("passed", assembly.Passed));
                xAssembly.Add(new XAttribute("failed", assembly.Failed));
                xAssembly.Add(new XAttribute("skipped", assembly.Skipped));
                xAssembly.Add(new XAttribute("time", assembly.Time));
                xAssembly.Add(new XAttribute("errors", assembly.Errors));

                var xErrors = new XElement("errors");
                foreach (var error in assembly.ErrorDetails) {
                    var xError = new XElement("error");
                    xError.Add(new XAttribute("type", error.Type));
                    xError.Add(new XAttribute("name", error.Name));
                    xErrors.Add(xError);
                }
                xAssembly.Add(xErrors);

                foreach (var collection in assembly.Collections) {
                    var xCollection = new XElement("collection");
                    xCollection.Add(new XAttribute("total", collection.Total));
                    xCollection.Add(new XAttribute("passed", collection.Passed));
                    xCollection.Add(new XAttribute("failed", collection.Failed));
                    xCollection.Add(new XAttribute("skipped", collection.Skipped));
                    xCollection.Add(new XAttribute("name", collection.Name));
                    xCollection.Add(new XAttribute("time", collection.Time));
                    foreach (var testResult in collection.Tests) {
                        var xTest = new XElement("test");
                        xTest.Add(new XAttribute("name", testResult.Name));
                        xTest.Add(new XAttribute("type", testResult.Type));
                        xTest.Add(new XAttribute("method", testResult.Method));
                        xTest.Add(new XAttribute("time", testResult.Time?.TotalSeconds));
                        xTest.Add(new XAttribute("result", testResult.Result));
                        var xTraits = new XElement("traits");
                        var xTrait = new XElement("trait");
                        xTrait.Add(new XAttribute("name", "IntegrationTest"));
                        xTrait.Add(new XAttribute("value", "ProjectSystem"));
                        xTraits.Add(xTrait);
                        xTest.Add(xTraits);
                        if (testResult.Failure != null) {
                            var xFailure = new XElement("failure");

                            xFailure.Add(new XAttribute("exception-type", testResult.Failure.ExceptionType));

                            var xMessage = new XElement("message");
                            xMessage.Add(new XCData(testResult.Failure.Message));
                            xFailure.Add(xMessage);

                            var xStackTrace = new XElement("stack-trace");
                            xStackTrace.Add(new XCData(testResult.Failure.StackTrace));
                            xFailure.Add(xStackTrace);

                            xTest.Add(xFailure);
                        }

                        xCollection.Add(xTest);
                    }
                    xAssembly.Add(xCollection);
                }

                xUnit.Add(xAssembly);
            }

            return xUnit;
        }
    }
}
