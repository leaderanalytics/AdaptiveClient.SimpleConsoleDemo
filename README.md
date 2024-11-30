# AdaptiveClient.SimpleConsoleDemo
A simple console application that demonstrates the basic concepts of AdaptiveClient.

See the [Zamagon Demo](https://github.com/leaderanalytics/AdaptiveClient.EntityFramework.Zamagon) for an example of working Web and desktop applications.

---

### Change log
2024-11-30 v3.0.0 - LeaderAnalytics.AdaptiveClient 5.0.1
2021-02-07 v2.0.0 - AdaptiveClient 2.0. 

---

#### [AdaptiveClient](https://github.com/leaderanalytics/AdaptiveClient)

---

This demo is designed to showcase the functionality of AdaptiveClient - not to illustrate how a production application  should use it.  Most applications that use AdaptiveClient will set the working endpoint when the application starts and just run.  This demo allows the user to choose an endpoint at runtime.  While this is makes for an interesting demo it is not practical for most applications.  The takeaway from the preceding is that this demo intentionally complicates things a bit.  In a production app AdaptiveClient handles the heavy lifting.

-----



Start by looking at the EndPoints section in appsettings.json.
Each EndPoint is a connection string with some additional metadata such as Name, API_Name, etc.

Note that there are three implementations of `UsersService`: `UsersService_MSSQL`, `UsersService_MySQL`, and `UsersService_WebAPI`.  Each implementation illustrates how a service might be specific to a specific transport or database provider.  In case you are wondering - AdaptiveClient does not impose naming requirements on your services.  These services are named for easy identification for this demo and can just as easily be created using any other legal name.

AdaptiveClient uses the metadata associated with each EndPoint as a key. Each key helps AdaptiveClient resolve the dependency graph for an implementation of a service that is tied to a specific connection string.  

For example, one of the connection strings is for use with Microsoft SQL Server.  The `ProviderName` attribute for that connection string is "MSSQL".  That connection string, and the implementation of `UsersService` named `UsersService_MSSQL` are both registered with AdaptiveClient using "MSSQL" as a key.


In this simple demo the value of using keys to resolve dependencies for a specific transport or persistence layer is not readily apparent.  In larger applications where there are dozens or hundreds of services and the dependency graph is deeper the value is much more obvious.  For a more realistic AdaptiveClient example see the [Zamagon Demo](https://github.com/leaderanalytics/AdaptiveClient.EntityFramework.Zamagon).  The [Zamagon Demo](https://github.com/leaderanalytics/AdaptiveClient.EntityFramework.Zamagon) also illustrates the use of a `Service Manifest` object that allows you to easily inject multiple services at once.



---


