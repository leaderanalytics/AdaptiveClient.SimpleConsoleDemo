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
        private static IContainer container;

        static void Main(string[] args)
        {
            // This program runs the Demo application twice:
            //
            // First run simulates normal service calls where the preferred SQL server on the LAN is used.
            //
            // Second run simulates a fall back scenario where we have no LAN connectivity so we
            // fall back to the WebAPI server.


            // First run:
            container = new ContainerBuilder().Build();
            Console.WriteLine("Starting AdaptiveClient Demo");
            Console.WriteLine();

            using (var scope = container.BeginLifetimeScope(builder => RegisterComponents(builder)))
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

            using (var scope = container.BeginLifetimeScope(builder => RegisterMocks(builder)))
            {
                Demo demo = scope.Resolve<Demo>();
                demo.Run();
            }

            Console.WriteLine();
            Console.WriteLine("AdaptiveClient Demo Complete");
            Console.WriteLine("Press any key...");
            Console.Read();
        }

        private static void RegisterComponents(ContainerBuilder builder)
        {
            AutofacRegistrationHelper registrationHelper = new AutofacRegistrationHelper(builder);
            IEnumerable<IEndPointConfiguration> endPoints = ReadEndPoints();

            // API Name is an arbitrary name of your choosing that AdaptiveClient uses to link interfaces (IUsersService) to the
            // EndPoints that expose them (Prod_SQL_01, Prod_WebAPI_01).  The API_Name used here must match the API_Name 
            // of related EndPoints in EndPoints.json file.
            string apiName = API_Name.UsersAPI.ToString();

            // register endPoints before registering clients
            registrationHelper.RegisterEndPoints(endPoints);

            // register clients
            registrationHelper.Register<UsersWebAPIClient, IUsersService>(EndPointType.HTTP, apiName);
            registrationHelper.Register<UsersService, IUsersService>(EndPointType.InProcess, apiName);

            // register logger (optional)
            registrationHelper.RegisterLogger(logMessage => Console.WriteLine(logMessage.Substring(0,203)));

            builder.RegisterType<Demo>();
        }

        private static void RegisterMocks(ContainerBuilder builder)
        {
            // this method is for mocks - see RegisterComponents for examples of how to register your components with AdaptiveClient.
            AutofacRegistrationHelper registrationHelper = new AutofacRegistrationHelper(builder);
            IEnumerable<IEndPointConfiguration> endPoints = ReadEndPoints();
            string apiName = API_Name.UsersAPI.ToString();
            registrationHelper.RegisterEndPoints(endPoints);
            registrationHelper.Register<UsersWebAPIClient, IUsersService>(EndPointType.HTTP, apiName);
            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(x => x.SaveUser(It.IsAny<User>())).Throws(new Exception("Cant find database server."));
            usersServiceMock.Setup(x => x.GetUserByID(It.IsAny<int>())).Throws(new Exception("Cant find database server."));
            builder.RegisterInstance(usersServiceMock.Object).Keyed<IUsersService>(EndPointType.InProcess);
            registrationHelper.RegisterLogger(x => Console.WriteLine(x.Substring(0, 203)));
            builder.RegisterType<Demo>();
        }

        private static IEnumerable<IEndPointConfiguration> ReadEndPoints()
        {
            string filePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "EndPoints.json");
            JObject obj = JsonConvert.DeserializeObject(File.ReadAllText(filePath)) as JObject;
            return obj["EndPointConfigurations"].ToObject<List<EndPointConfiguration>>();
        }
    }
}