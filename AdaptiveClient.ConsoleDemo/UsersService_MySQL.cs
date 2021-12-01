namespace AdaptiveClient.ConsoleDemo;

class UsersService_MySQL : IUsersService
{
    public User GetUserByID(int id)
    {
        // call database client here and select using MySQL specific SQL...
        return new User { ID = id, Name = "Bob (retrieved from MySQL)" };
    }
}
