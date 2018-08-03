using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TRXunit {

    internal static class XElementExtensions {

        public static TestErrorDetail GetErrors(this XElement xUnitTestResult) {
            var xErrorInfo = xUnitTestResult.Element(Constants.XN + "Output").Elements(Constants.XN + "ErrorInfo").FirstOrDefault();
            if (xErrorInfo == null) {
                return null;
            }

            return new TestErrorDetail() {
                ExceptionType = "MSTest Exception",
                Message = xErrorInfo.Element(Constants.XN + "Message").Value,
                StackTrace = xErrorInfo.Element(Constants.XN + "StackTrace").Value
            };
        }
        public static string GetResult(this XElement xUnitTestResult) {
                var outcome = xUnitTestResult.Attribute("outcome").Value;
            switch (outcome) {
                case "Passed":
                case "Warning":
                    return "Pass";
                case "Failed":
                case "Error":
                case "Timeout":
                case "Aborted":
                case "Inconclusive":
                    return "Fail";
                default:
                    return "Skip";
            }
        }

        public static TimeSpan? GetElapsedTime(this XElement xUnitTestResult) {
            var start = xUnitTestResult.GetDateTime("startTime");
            var end = xUnitTestResult.GetDateTime("endTime");
            return end - start;
        }

        public static XElement FindResult(this XElement trx, string id) {
            var xUnitTestResults = trx.Element(Constants.XN + "Results").Elements(Constants.XN + "UnitTestResult");
            return xUnitTestResults.Where(x => x.Attribute("executionId").Value as string == id).Single();
        }

        public static string GetId(this XElement xUnitTestDefinition) {
            return (string)xUnitTestDefinition.Element(Constants.XN + "Execution").Attribute("id");
        }

        public static DateTime? GetDateTime(this XElement element, string attributeName) {
            var value = (string)element.Attribute(attributeName);

            if (!DateTime.TryParse(value, out var dt)) {
                return null;
            }

            return dt;
        }

        public static TimeSpan? ReadTimeSpan(this XElement element, string attributeName) {
            var value = (string)element.Attribute(attributeName);

            if (!TimeSpan.TryParse(value, out var ts)) {
                return null;
            }

            return ts;
        }

        public static int ReadInt(this XElement element, string attributeName) {
            var value = element.Attribute(attributeName).Value;
            return int.Parse(value);
        }

        public static Guid ReadGuid(this XElement element, string attributeName) {
            var value = element.Attribute(attributeName).Value;
            return Guid.Parse(value);
        }
    }
}
