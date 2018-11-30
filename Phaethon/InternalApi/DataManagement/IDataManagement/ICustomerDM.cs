using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Core.Model.Filters;

namespace InternalApi.DataManagement.IDataManagement
{
    internal interface ICustomerDM
    {
        List<Customer> ReadAll(CustomerQueryFilter jobQueryFilter);
    }
}
