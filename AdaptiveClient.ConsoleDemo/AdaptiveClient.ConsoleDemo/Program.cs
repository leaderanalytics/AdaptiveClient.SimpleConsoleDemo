using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Moq;
using LeaderAnalytics.AdaptiveClient;

namespace AdaptiveClient.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // This program runs the Demo application twice:
            //
            // First run simulates normal service calls where the preferred SQL server on the LAN is used.
            //
            // Second run simulates a fall back scenario where we have no LAN connectivity so we
            // fall back to the WebAPI server.


            // First run:
            Console.WriteLine("Starting AdaptiveClient Demo");
            Console.WriteLine();

            using (var scope = new ContainerBuilder().Build().BeginLifetimeScope(builder => AutofacHelper.RegisterComponents(builder)))
            {
                Demo demo = scope.Resolve<Demo>();
                demo.Run();
            }


            // Second run:
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Simulating failure to connect to SQL box on local area network.");
            Console.WriteLine("An error message will be shown and AdaptiveClient will fall back to the WebAPI server.");
            Console.WriteLine();

            using (var scope = new ContainerBuilder().Build().BeginLifetimeScope(builder => AutofacHelper.RegisterMocks(builder)))
            {
                Demo demo = scope.Resolve<Demo>();
                demo.Run();
            }

            Console.WriteLine();
            Console.WriteLine("AdaptiveClient Demo Complete");
            Console.WriteLine("Press any key...");
            Console.Read();
        }

        
    }
}