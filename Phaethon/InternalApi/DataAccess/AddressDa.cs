using System.Data.Entity.Migrations;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class AddressDa
    {
        internal void CreateOrUpdate(DatabaseContext db, Address address)
        {
            db.Addresses.AddOrUpdate(address);
            db.SaveChanges();
        }
    }
}