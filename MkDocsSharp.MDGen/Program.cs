using System.Xml.Linq;
using CommandLine;

namespace MkDocsSharp.MDGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunWithOptions)
                .WithNotParsed(errors => Environment.Exit(1));
        }

        static void RunWithOptions(Options options)
        {
            if (!options.ConsoleIn && options.InputFile == null
                || !options.ConsoleOut && options.OutputFile == null)
            {
                Console.WriteLine("Error: Must specify input and output (file or console).");
                Environment.Exit(1);
                return;
            }

            using var inReader = options.ConsoleIn
                ? Console.In
                : new StreamReader(options.InputFile!);
            
            using var outWriter = options.ConsoleOut
                ? Console.Out
                : new StreamWriter(options.OutputFile!);

            var xml = inReader.ReadToEnd();
            var doc = XDocument.Parse(xml);
            var md = doc.Root?.ToMarkDown() ?? string.Empty;
            outWriter.Write(md);
        }
    }
}
