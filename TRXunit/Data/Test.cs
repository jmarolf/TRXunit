using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TRXunit {
    internal class Test {
        public string CreationTime { get; set; }
        public IEnumerable<Assembly> Assemblies { get; set; }
    }
}
