using System;
using System.Xml.Linq;

namespace TRXunit {
    internal static class TrxParser {
        internal static Test Parse(XElement trx) {
            var result = new Test();
            result = result.SetCreationTime(trx);
            result = result.SetAssemblies(trx);
            return result;
        }
    }
}
