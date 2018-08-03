using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TRXunit {
    internal class Program {
        private static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("first arg must be the trx-file");
                Environment.Exit(1);
            }

            try {
                Run(args);
            }
            catch (Exception ex) when (!Debugger.IsAttached) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(ex.Message);
                Console.ResetColor();
                Environment.Exit(2);
            }

            if (Debugger.IsAttached) {
                Console.ReadKey();
            }
        }

        private static void Run(string[] args) {
            string trxFile = args[0];
            string xUnitFile = Path.ChangeExtension(trxFile, "xml");

            Console.WriteLine($"Converting trx-file '{trxFile}' to XUnit-xml...");
            DateTime start = DateTime.Now;

            var utf8 = new UTF8Encoding(false);
            using (Stream input = File.OpenRead(trxFile))
            using (TextWriter output = new StreamWriter(xUnitFile, false, utf8)) {
                var converter = new TRXToXUnitConverter();
                converter.Convert(input, output);
            }

            Console.WriteLine($"done in {(DateTime.Now - start).TotalSeconds} seconds.");
        }
    }
}
