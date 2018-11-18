using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class ElementDa
    {
        internal void CreateOrUpdate(DatabaseContext db, Element element)
        {
            db.Elements.AddOrUpdate(element);
            db.SaveChanges();
        }
    }
}