using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
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

        internal Item GetItem(DatabaseContext db, int id)
        {
            Item item = db.Items
                .Include(x => x.Product)
                .SingleOrDefault(x => x.ID == id);

            item.Quantity = 1;

            return item;
        }

        internal void Delete(DatabaseContext db, Item item)
        {
            db.Items.Remove(item);
            db.SaveChanges();
        }
    }
}