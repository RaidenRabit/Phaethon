using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace InternalApi.DataManagement.IDataManagement
{
    internal interface ICompanyManagement
    {
        Company Read(int id);

        List<Company> GetCompanies();
    }
}
