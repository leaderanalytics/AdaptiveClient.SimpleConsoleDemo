using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptiveClient.ConsoleDemo
{
    public interface IUsersService : IDisposable
    {
        void SaveUser(User user);
        User GetUserByID(int id);
    }
}
