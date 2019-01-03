using System.Collections.Generic;
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

        public void Create(TaxGroup taxGroup)
        {
            using (var db = new DatabaseContext())
            {
                _taxGroupDa.Create(db, taxGroup);
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