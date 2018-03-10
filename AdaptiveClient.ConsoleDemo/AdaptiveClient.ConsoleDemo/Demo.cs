﻿using System;
using System.Collections.Generic;
using System.Text;
using LeaderAnalytics.AdaptiveClient;

namespace AdaptiveClient.ConsoleDemo
{
    // In a real application this Demo class would most likely be a Controller or a ViewModel.
    // Here we inject a single service, UsersService, as is shown in the constructor below.
    // However we can also inject a facade or manifest that will give us easy access to hundreds of services.
    // 
    public class Demo
    {
        private IAdaptiveClient<IUsersService> client;

        public Demo(IAdaptiveClient<IUsersService> client)
        {
            this.client = client;
        }

        public void Run() // In a real web app this method might be called by a Get or Post
        {
            // Make some calls to UsersService which we have mocked up to simulate a service that reads/writes to a database.
            // Three calls are made to this method:  
            // First call we show we can make calls to a service that reads/writes a MSSQL database.
            // Second call we show we can make calls to a service that reads/writes a MySQL database.
            // Third call we simulate a failure and we fall back to a mocked WebAPI client.


            // An error here is expected on the web api call as we are simulating an error connecting to a server.  Click "Continue" in Visual Studio IDE.
            User user = client.Try(usersService => usersService.GetUserByID(1));  
            Console.WriteLine($"User {user.Name} was found.  EndPoint used was {client.CurrentEndPoint.Name}.");

            client.Try(usersService => usersService.SaveUser(user));
            Console.WriteLine($"User {user.Name} was saved.  EndPoint used was {client.CurrentEndPoint.Name}.");
        }
    }
}
