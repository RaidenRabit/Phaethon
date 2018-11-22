using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class TaxGroupDa
    {
        internal bool Create(DatabaseContext db, TaxGroup taxGroup)
        {
            db.TaxGroups.Add(taxGroup);
            return db.SaveChanges() > 0;
        }

        internal List<TaxGroup> GetTaxGroups(DatabaseContext db)
        {
            return db.TaxGroups.ToList();
        }
    }
}