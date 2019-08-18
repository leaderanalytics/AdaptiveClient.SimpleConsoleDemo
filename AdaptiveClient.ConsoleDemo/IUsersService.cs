using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptiveClient.ConsoleDemo
{
    public interface IUsersService 
    {
        User GetUserByID(int id);
    }
}
