namespace AdaptiveClient.ConsoleDemo;

public class UsersService_WebAPI : IUsersService
{
    public User GetUserByID(int id)
    {
        // httpClient.GetStringAsync(...)
        return new User { ID = id, Name = "Bob (retrieved from Web API call)" };
    }
}
