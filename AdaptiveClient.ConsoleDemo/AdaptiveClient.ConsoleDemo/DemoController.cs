using System;
using System.Collections.Generic;
using System.Text;
using LeaderAnalytics.AdaptiveClient;

namespace AdaptiveClient.ConsoleDemo
{
    // In a real application this Demo class would most likely be a Controller or a ViewModel.
    // Here we inject a single service, UsersService, as is shown in the constructor below.
    // However we can also inject a facade or manifest that will give us easy access to hundreds of services.
    // 
    public class DemoController
    {
        private IAdaptiveClient<IUsersService> client;

        public DemoController(IAdaptiveClient<IUsersService> client)
        {
            this.client = client;
        }

        public void Run() // In a real app this method might be called by a Get or Post
        {
            // Make some calls to UsersService which we have mocked up to simulate a service that reads/writes to a database.
            // Two calls are made to this method:  
            // On the first call we simply show we can make calls to our service.
            // On the second call we simulate a failure and we fall back to a mocked WebAPI client.

            User user = client.Try(usersService => usersService.GetUserByID(1));  // An error here is expected on the second call as we are simulating an error connecting to a server.  Click "Continue" in Visual Studio IDE.
            Console.WriteLine($"User {user.Name} was found.  EndPoint used was {client.CurrentEndPoint.Name}.");

            client.Try(usersService => usersService.SaveUser(user));
            Console.WriteLine($"User {user.Name} was saved.  EndPoint used was {client.CurrentEndPoint.Name}.");
        }
    }
}
