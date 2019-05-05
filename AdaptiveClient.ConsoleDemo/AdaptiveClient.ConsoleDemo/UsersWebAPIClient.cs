using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptiveClient.ConsoleDemo
{
    public class UsersWebAPIClient : IUsersService
    {
        public void SaveUser(User user)
        {
            // httpClient.PostAsync(...)
        }

        public User GetUserByID(int id)
        {
            // httpClient.GetStringAsync(...)
            return new User { ID = id, Name = "Bob (retrieved from Web API call)" };
        }
    }
}
