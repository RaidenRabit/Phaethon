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
        internal void Create(DatabaseContext db, TaxGroup taxGroup)
        {
            db.TaxGroups.Add(taxGroup);
            db.SaveChanges();
        }

        internal List<TaxGroup> GetTaxGroups(DatabaseContext db)
        {
            return db.TaxGroups.ToList();
        }
    }
}