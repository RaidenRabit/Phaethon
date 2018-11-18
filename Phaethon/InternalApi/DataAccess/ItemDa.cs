using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class ItemDa
    {
        internal void CreateOrUpdate(DatabaseContext db, Item item)
        {
            db.Items.AddOrUpdate(item);
            db.SaveChanges();
        }

        internal void Delete(DatabaseContext db, int id)
        {
            db.Items.Remove(db.Items.SingleOrDefault(x => x.ID == id));
            db.SaveChanges();
        }
    }
}