using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    public class TaxGroupManagement: ITaxGroupManagement
    {
        private readonly TaxGroupDa _taxGroupDa;

        internal TaxGroupManagement()
        {
            _taxGroupDa = new TaxGroupDa();
        }

        public bool Create(TaxGroup taxGroup)
        {
            using (var db = new DatabaseContext())
            {
                return _taxGroupDa.Create(db, taxGroup);
            }
        }

        public List<TaxGroup> GetTaxGroups()
        {
            using (var db = new DatabaseContext())
            {
                return _taxGroupDa.GetTaxGroups(db);
            }
        }
    }
}