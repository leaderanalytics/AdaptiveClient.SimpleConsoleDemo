using System;
using System.Collections.Generic;
using System.Text;
using LeaderAnalytics.AdaptiveClient;

namespace AdaptiveClient.ConsoleDemo
{

    public class Demo
    {
        private IAdaptiveClient<IUsersService> client;

        public Demo(IAdaptiveClient<IUsersService> client)
        {
            this.client = client;
        }

        public void Run()
        {
            // Simulate making some service calls.
            // Ideally we have access to a SQL server on the LAN but if 
            // that fails we fall back to a WebAPI server:

            User user = GetAUser(1);
            SaveAUser(user);
        }

        private User GetAUser(int id)
        {
            User user = client.Try(x => x.GetUserByID(id));
            Console.WriteLine($"User {user.Name} was found.  EndPoint used was {client.CurrentEndPoint.Name}.");
            return user;
        }

        private void SaveAUser(User user)
        {
            client.Try(x => x.SaveUser(user));
            Console.WriteLine($"User {user.Name} was saved.  EndPoint used was {client.CurrentEndPoint.Name}.");
        }
    }
}
