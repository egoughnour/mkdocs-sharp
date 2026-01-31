using CommandLine;

namespace PxtlCa.XmlCommentMarkDownGenerator
{
    class Options
    {
        [Option('i', "inputfile", HelpText = "Input xml file to read.")]
        public string? InputFile { get; set; }

        [Option("cin", HelpText = "Read input from console instead of file.")]
        public bool ConsoleIn { get; set; }

        [Option('o', "outputfile", HelpText = "Output md file to write.")]
        public string? OutputFile { get; set; }

        [Option("cout", HelpText = "Write output to console instead of file.")]
        public bool ConsoleOut { get; set; }
    }
}
