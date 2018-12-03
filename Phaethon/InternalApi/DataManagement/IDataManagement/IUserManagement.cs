using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace InternalApi.DataManagement.IDataManagement
{
    interface IUserManagement
    {
        bool CreateOrUpdate(User user);

        bool DeleteUser(int id);

        User GetUser(int id);

        int Login(User user);


    }
}
