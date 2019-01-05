using System.Data.Entity.Migrations;
using Core;
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