using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Autofac;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LeaderAnalytics.AdaptiveClient;

namespace AdaptiveClient.ConsoleDemo
{
    public static class DataProvider
    {
        public static string MSSQL = "MSSQL";
        public static string MySQL = "MySQL";
        public static string HTTP = "HTTP";
        //public static string Oracle = "Oracle";
    }

    public static class API_Name
    {
        public const string UsersAPI = "UsersAPI";
        // ,YourAPIName
    }


    public static class EndPointType
    {
        public const string InProcess = "InProcess";
        public const string HTTP = "HTTP";
        public const string WCF = "WCF";
        public const string ESB = "ESB";
        public const string File = "File";
        public const string FTP = "FTP";
    }

    public class AutofacHelper
    {

        public static void RegisterComponents(ContainerBuilder builder)
        {
            RegistrationHelper registrationHelper = new RegistrationHelper(builder);
            IEnumerable<IEndPointConfiguration> endPoints = ReadEndPoints();

            // API Name is an arbitrary name of your choosing that AdaptiveClient uses to link interfaces (IUsersService) to the
            // EndPoints that expose them (Prod_SQL_01, Prod_WebAPI_01).  The API_Name used here must match the API_Name 
            // of related EndPoints in EndPoints.json file.
            string apiName = API_Name.UsersAPI.ToString();

            // register endPoints before registering clients
            registrationHelper.RegisterEndPoints(endPoints);

            // register clients

            // We can register a url as a connection string.  If we pass AdaptiveClient an EndPoint of type HTTP it will return an instance of UsersWebAPIClient
            registrationHelper.RegisterService<UsersWebAPIClient, IUsersService>(EndPointType.HTTP, apiName, DataProvider.HTTP);

            // If our EndPoint has a DataProvider type of MSSQL, AdaptiveClient will return an instance of UsersService_MSSQL which (fictitiously) contains SQL for Microsoft SQL Server.
            registrationHelper.RegisterService<UsersService_MSSQL, IUsersService>(EndPointType.InProcess, apiName, DataProvider.MSSQL);

            // If our EndPoint has a DataProvider type of MySQL, AdaptiveClient will return an instance of UsersService_MySQL which (fictitiously) contains SQL for MySQL.
            registrationHelper.RegisterService<UsersService_MySQL, IUsersService>(EndPointType.InProcess, apiName, DataProvider.MySQL);


            // register logger (optional)
            registrationHelper.RegisterLogger(logMessage => Console.WriteLine(logMessage.Substring(0, 203)));

            builder.RegisterType<Demo>();
        }

        private static IEnumerable<IEndPointConfiguration> ReadEndPoints()
        {
            string filePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "EndPoints.json");
            JObject obj = JsonConvert.DeserializeObject(File.ReadAllText(filePath)) as JObject;
            return obj["EndPointConfigurations"].ToObject<List<EndPointConfiguration>>();
        }

        // Mocks for the demo

        public static void RegisterFallbackMocks(ContainerBuilder builder)
        {
            // this method is for mocks - see RegisterComponents for examples of how to register your components with AdaptiveClient.
            RegistrationHelper registrationHelper = new RegistrationHelper(builder);
            IEnumerable<IEndPointConfiguration> endPoints = ReadEndPoints();
            registrationHelper.RegisterEndPoints(endPoints);
            registrationHelper.RegisterService<UsersWebAPIClient, IUsersService>(EndPointType.HTTP, API_Name.UsersAPI, DataProvider.HTTP);
            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(x => x.SaveUser(It.IsAny<User>())).Throws(new Exception("Cant find database server."));
            usersServiceMock.Setup(x => x.GetUserByID(It.IsAny<int>())).Throws(new Exception("Cant find database server."));
            builder.RegisterInstance(usersServiceMock.Object).Keyed<IUsersService>(EndPointType.InProcess+DataProvider.MSSQL);
            registrationHelper.RegisterLogger(x => Console.WriteLine(x));
            builder.RegisterType<Demo>();
        }

        public static void RegisterMySQLMocks(ContainerBuilder builder)
        {
            RegistrationHelper registrationHelper = new RegistrationHelper(builder);
            IEnumerable<IEndPointConfiguration> endPoints = ReadEndPoints().Where(x => x.ProviderName != DataProvider.MSSQL);
            registrationHelper.RegisterEndPoints(endPoints);
            registrationHelper.RegisterService<UsersWebAPIClient, IUsersService>(EndPointType.HTTP, API_Name.UsersAPI, DataProvider.HTTP);
            registrationHelper.RegisterService<UsersService_MSSQL, IUsersService>(EndPointType.InProcess, API_Name.UsersAPI, DataProvider.MSSQL);
            registrationHelper.RegisterService<UsersService_MySQL, IUsersService>(EndPointType.InProcess, API_Name.UsersAPI, DataProvider.MySQL);
            registrationHelper.RegisterLogger(logMessage => Console.WriteLine(logMessage.Substring(0, 203)));
            builder.RegisterType<Demo>();
        }
    }
}
