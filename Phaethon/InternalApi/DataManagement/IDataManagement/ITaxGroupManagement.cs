using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace InternalApi.DataManagement.IDataManagement
{
    interface ITaxGroupManagement
    {
        bool Create(TaxGroup taxGroup);
        List<TaxGroup> GetTaxGroups();
    }
}
