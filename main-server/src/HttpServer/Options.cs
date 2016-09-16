using CommandLine;
using CommandLine.Text;

namespace HttpServer
{
    class Options
    {
        [Option('h', "host", Required = true,
        HelpText = "Specify a host ip.")]
        public string Host { get; set; }

        [Option('p', "port", Required = true,
        HelpText = "Specify a port.")]
        public int Port { get; set; }
        
        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
            (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
