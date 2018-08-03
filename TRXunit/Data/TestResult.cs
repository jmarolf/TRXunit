using System;
using System.Collections.Generic;

namespace TRXunit {
    public class TestResult {
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public string Method { get; internal set; }
        public TimeSpan? Time { get; internal set; }
        public string TimeString { get; internal set; }
        public string Result { get; internal set; }
        public TestErrorDetail Failure { get; internal set; }
    }
}
