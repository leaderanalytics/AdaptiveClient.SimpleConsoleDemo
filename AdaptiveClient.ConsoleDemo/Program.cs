using System;
using System.Collections.Generic;
using Autofac;
using LeaderAnalytics.AdaptiveClient;
using System.Linq;

namespace AdaptiveClient.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IEndPointConfiguration> endPoints = EndPointUtilities.LoadEndPoints("appsettings.json").ToList();
            ContainerBuilder builder = new ContainerBuilder();
            AutofacModule.RegisterComponents(builder);
            IContainer container = builder.Build();
            Console.Clear();
            DisplayIntro();
            int i = 0;
            endPoints.ForEach(ep => Console.WriteLine($"{i++}.     {ep.Name}"));
            Console.WriteLine();

            while (true)
            {
                int cursorRow = 13;
                SetCursorToRow(cursorRow++);
                Console.Write("Choose an EndPoint (connection string) from the list or enter Q to exit:");
                ConsoleKeyInfo lastKey = Console.ReadKey();

                if (lastKey.Key == ConsoleKey.Q)
                    break;

                bool isParsed = int.TryParse(lastKey.KeyChar.ToString(), out int index);

                if (!isParsed || index >= endPoints.Count)
                    continue;

                SetCursorToRow(cursorRow++);
                IEndPointConfiguration ep = endPoints[index];
                Demo demo = container.Resolve<Demo>();


                demo.DisplayUserName(ep);
            }

            Console.WriteLine("Adaptive Client demo ended.");
        }

        private static void SetCursorToRow(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.WriteLine(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, row);
        }
        private static void DisplayIntro()
        {
            Console.WriteLine("AdaptiveClient Simple Console Demo");
            Console.WriteLine();
            Console.WriteLine("In this demo you choose a connection string from the list below.  Each connection");
            Console.WriteLine("string theoretically connects to a different kind of server - i.e. MSSQL, MySQL, WebAPI.");
            Console.WriteLine("AdaptiveClient will resolve an implementation of IUsersService that is correct for");
            Console.WriteLine("the selected connection string and display a fictitious User object.");
            Console.WriteLine("This demo is a very basic introduction.  See AdaptiveClient.EntityFramework.Zamagon");
            Console.WriteLine("for a working Web/Desktop application.");
            Console.WriteLine("");
        }
    }
}