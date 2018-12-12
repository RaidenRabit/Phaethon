using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    public class TaxGroupDM: ITaxGroupDM
    {
        private readonly TaxGroupDa _taxGroupDa;

        internal TaxGroupDM()
        {
            _taxGroupDa = new TaxGroupDa();
        }

        public bool Create(TaxGroup taxGroup)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    _taxGroupDa.Create(db, taxGroup);
                    return true;
                }
                catch
                {
                    return false;
                }
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