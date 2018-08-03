using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TRXunit {
    internal class TRXToXUnitConverter {
        internal void Convert(Stream input, TextWriter output) {
            var trx = XElement.Load(input, LoadOptions.None);
            var parseResult = TrxParser.Parse(trx);
            var buildResult = XUnitBuilder.Build(parseResult);
            buildResult.Save(output, SaveOptions.None);
        }
    }
}
