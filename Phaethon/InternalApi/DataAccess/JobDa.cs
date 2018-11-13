using System.Data.Entity;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class JobDa
    {
        internal bool Create(Job job)
        {
            using (var db = new DatabaseContext())
            {
                if (db.Invoices.Any(i => i.ID == job.ID))
                    db.Entry(job).State = EntityState.Modified;
                if (db.Customers.Any(i => i.ID == job.Customer.ID))
                    db.Entry(job.Customer).State = EntityState.Modified;
                if (db.Addresses.Any(i => i.ID == job.Customer.Address.ID))
                    db.Entry(job.Customer.Address).State = EntityState.Modified;
                db.Jobs.Add(job);

                return db.SaveChanges() > 0;
            }
        }

        internal Job Read(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Jobs
                    .Include(x => x.Customer)
                    .Include(x => x.Customer.Address)
                    .SingleOrDefault(x => x.ID == id);
            }
        }
    }
}