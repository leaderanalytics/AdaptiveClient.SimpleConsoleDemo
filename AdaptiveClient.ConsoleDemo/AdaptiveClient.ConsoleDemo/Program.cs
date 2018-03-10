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
            // This program runs the Demo application three times:
            //
            // First run simulates injecting a client for Microsoft SQL Server into the DemoController.
            //
            // Second run simulates injecting a client for MySQL into the DemoController.
            //
            // Third run simulates a fall back scenario where we have no LAN connectivity. We
            // fall back to the WebAPI client and inject that into the DemoController .


            // First run:
            Console.WriteLine("Starting AdaptiveClient Demo");
            Console.WriteLine("Simulating retrieving a user from a Microsoft SQL Server database");
            Console.WriteLine("Press any key...");
            Console.ReadKey();

            using (var scope = new ContainerBuilder().Build().BeginLifetimeScope(builder => AutofacHelper.RegisterComponents(builder)))
            {
                Demo demo = scope.Resolve<Demo>();
                demo.Run();
            }


            // Second run:
            Console.WriteLine();
            Console.WriteLine("Simulating retrieving a user from a MySQL database.");
            Console.WriteLine("To accomplish switching between Data Providers such as MS SQL and MySQL in a real application");
            Console.WriteLine("you only need to toggle the Active flag on the connection string.  AdaptiveClient will resolve ");
            Console.WriteLine("the correct client for the EndPoint you choose.");
            Console.WriteLine("You may also leave both EndPoints flagged as Active and Adaptive client will use the one with the highest priority.");
            Console.WriteLine("Press any key...");
            Console.ReadKey();

            using (var scope = new ContainerBuilder().Build().BeginLifetimeScope(builder => AutofacHelper.RegisterMySQLMocks(builder)))
            {
                Demo demo = scope.Resolve<Demo>();
                demo.Run();
            }

            Console.WriteLine();

            // Third run:
            Console.WriteLine();
            Console.WriteLine("Simulating failure to connect to SQL box on local area network.");
            Console.WriteLine("An error message will be shown and AdaptiveClient will fall back to the WebAPI server.");
            Console.WriteLine("Visual Studio may break with an exception. This is expected - just click Continue.");
            Console.WriteLine("Press any key...");
            Console.ReadKey();

            using (var scope = new ContainerBuilder().Build().BeginLifetimeScope(builder => AutofacHelper.RegisterFallbackMocks(builder)))
            {
                Demo demo = scope.Resolve<Demo>();
                demo.Run();
            }

            Console.WriteLine();


            Console.WriteLine("AdaptiveClient Demo Complete");
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        
    }
}