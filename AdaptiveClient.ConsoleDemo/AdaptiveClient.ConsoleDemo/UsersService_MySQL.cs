using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptiveClient.ConsoleDemo
{
    class UsersService_MySQL : IUsersService
    {
        public void SaveUser(User user)
        {
           // call database client here and insert....
        }

        public User GetUserByID(int id)
        {
            // call database client here and select...
            return new User { ID = id, Name = "Bob (retrieved from MySQL)" };
        }
    }
}
