using System.Collections.Generic;

namespace TRXunit {
    public class Collection {
        public string Total { get; internal set; }
        public string Passed { get; internal set; }
        public string Failed { get; internal set; }
        public string Skipped { get; internal set; }
        public string Time { get; internal set; }
        public string Name { get; internal set; }
        public IEnumerable<TestResult> Tests { get; internal set; }
    }
}
