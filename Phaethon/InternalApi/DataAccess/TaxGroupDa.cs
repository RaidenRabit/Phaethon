using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    public class TaxGroupDa
    {
        public void Create(DatabaseContext db, TaxGroup taxGroup)
        {
            db.TaxGroups.Add(taxGroup);
            db.SaveChanges();
        }

        public List<TaxGroup> GetTaxGroups(DatabaseContext db)
        {
            return db.TaxGroups.ToList();
        }
    }
}