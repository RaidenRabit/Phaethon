using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace InternalApi.DataManagement.IDataManagement
{
    interface ILoginManagement
    {
        bool CreateOrUpdate(Login login);

        bool Delete(int id);

        Login GetLogin(int id);

        int Login(string username, string password);


    }
}
