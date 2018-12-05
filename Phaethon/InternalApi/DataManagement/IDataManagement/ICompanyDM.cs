using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace InternalApi.DataManagement.IDataManagement
{
    internal interface ICompanyDM
    {
        List<Company> GetCompanies();

        Company GetCompany(int id);
    }
}
