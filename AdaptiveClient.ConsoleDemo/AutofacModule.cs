namespace AdaptiveClient.ConsoleDemo;

// ------------------------------------------------------------
// DataProvider, API_Name, and EndPointType are keys that 
// AdaptiveClient uses link EndPoints (connection strings) to 
// specific services.  In this demo specific implementations 
// of IUsersService are linked to individual EndPoints.
// See the "Register services" comment below.
// These keys are also used by EndPoints defined in appsettings.json.
// ------------------------------------------------------------

public static class DataProvider
{
    public static string MSSQL = "MSSQL";
    public static string MySQL = "MySQL";
    public static string WebAPI = "WebAPI";
}

public static class API_Name
{
    public const string UsersAPI = "UsersAPI";
}

public static class EndPointType
{
    public const string InProcess = "InProcess";
    public const string HTTP = "HTTP";
}

public class AutofacModule
{
    public static void RegisterComponents(ContainerBuilder builder)
    {
        RegistrationHelper registrationHelper = new RegistrationHelper(builder);

        // Register endPoints before registering clients
        registrationHelper.RegisterEndPoints(EndPointUtilities.LoadEndPoints("appsettings.json"));

        // Register services
        registrationHelper.RegisterService<UsersService_WebAPI, IUsersService>(EndPointType.HTTP, API_Name.UsersAPI, DataProvider.WebAPI);
        registrationHelper.RegisterService<UsersService_MSSQL, IUsersService>(EndPointType.InProcess, API_Name.UsersAPI, DataProvider.MSSQL);
        registrationHelper.RegisterService<UsersService_MySQL, IUsersService>(EndPointType.InProcess, API_Name.UsersAPI, DataProvider.MySQL);

        // Register logger (optional)
        registrationHelper.RegisterLogger(logMessage => Console.WriteLine(logMessage.Substring(0, 203)));

        builder.RegisterType<Demo>();
    }
}
