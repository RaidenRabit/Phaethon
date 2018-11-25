using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    public class ItemDa
    {
        public void CreateOrUpdate(DatabaseContext db, Item item)
        {
            db.Items.AddOrUpdate(item);
            db.SaveChanges();
        }

        public Item GetItem(DatabaseContext db, int id)
        {
            return db.Items.SingleOrDefault(x => x.ID == id);
        }

        public void Delete(DatabaseContext db, Item item)
        {
            db.Items.Remove(item);
            db.SaveChanges();
        }
    }
}