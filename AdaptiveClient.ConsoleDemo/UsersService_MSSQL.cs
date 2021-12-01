namespace AdaptiveClient.ConsoleDemo;

class UsersService_MSSQL : IUsersService
{
    public User GetUserByID(int id)
    {
        // call database client here and select using MSSQL specific SQL...
        return new User { ID = id, Name = "Bob (retrieved from MSSQL)" };
    }
}
