using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.IServices
{
    using Mesher.Entity;
    public interface IUserServices
    {
        int UserAdd(User user);
        List<User> GetUsers();
        List<User> GetUsersByname(string name, string pass);
    }
}
