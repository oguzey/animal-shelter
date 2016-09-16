using CommandLine;
using System;
using System.Net;
using System.Threading;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var maxThreads = Environment.ProcessorCount * 4;
            ThreadPool.SetMinThreads(2, 2);
            ThreadPool.SetMaxThreads(maxThreads, maxThreads);
            Server server;
#if DEBUG
            var port = 80;
            server = new Server(IPAddress.Loopback, port);
            server.Start();
#else
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                IPAddress parsed;
                if (IPAddress.TryParse(options.Host, out parsed))
                    server = new Server(parsed, options.Port);
                else
                {
                    var addressesByDNS = Dns.GetHostEntry(options.Host).AddressList;
                    server = new Server(addressesByDNS[0], options.Port);
                }
                server.Start();
            }
#endif
            while (true) { Thread.Sleep(100); };
        }
    }
}