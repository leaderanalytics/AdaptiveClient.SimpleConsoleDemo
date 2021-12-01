namespace AdaptiveClient.ConsoleDemo;

// In a real application this Demo class would most likely be a Controller or a ViewModel.

public class Demo
{
    private IAdaptiveClient<IUsersService> client;

    public Demo(IAdaptiveClient<IUsersService> client)
    {
        this.client = client;
    }

    public void DisplayUserName(IEndPointConfiguration endPoint)
    {
        // We need to pass in the endPoint because this is a demo and the endPoint is selected 
        // at runtime.  In a real application AdaptiveClient will read the EndPoints file 
        // and use the active EndPoint.

        // Here we simulate making a call to an API server. Adaptive Client will use 
        // properties of the endPoint to resolve the correct implementation of IUsersService.

        User user = client.Try(usersService => usersService.GetUserByID(1), endPoint.Name);
        Console.WriteLine(user?.Name);
    }
}
