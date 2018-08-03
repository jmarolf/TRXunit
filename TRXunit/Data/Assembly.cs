using System.Collections.Generic;

namespace TRXunit {
    public class Assembly {
        public string Name { get; set; }
        public string Environment { get; set; }
        public string TestFramework { get; set; }
        public string RunDate { get; set; }
        public string RunTime { get; set; }
        public string ConfigFile { get; set; }
        public string Total { get; set; }
        public string Passed { get; set; }
        public string Failed { get; set; }
        public string Skipped { get; set; }
        public string Time { get; set; }
        public string Errors { get; set; }
        public IEnumerable<Collection> Collections { get; internal set; }
        public IEnumerable<ErrorDetail> ErrorDetails { get; internal set; }
    }
}
