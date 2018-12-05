using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class RepresentativeDa
    {
        internal void CreateOrUpdate(DatabaseContext db, Representative representative)
        {
            db.Representatives.AddOrUpdate(representative);
            db.SaveChanges();
        }
    }
}