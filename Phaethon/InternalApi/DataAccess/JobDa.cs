using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    public class JobDa
    {
        public int InsertOrUpdate(Job job)
        {
            using (var db = new DatabaseContext())
            {
                if (db.Jobs.Any(i => i.ID == job.ID))
                    db.Entry(job).State = EntityState.Modified;
                if (db.Customers.Any(i => i.ID == job.Customer.ID))
                    db.Entry(job.Customer).State = EntityState.Modified;
                if (db.Addresses.Any(i => i.ID == job.Customer.Address.ID))
                    db.Entry(job.Customer.Address).State = EntityState.Modified;
                db.Jobs.Add(job);
                db.SaveChanges();
                return job.ID;
            }
        }

        public Job Read(int id)
        {
            using (var db = new DatabaseContext())
            {
                var result = db.Jobs
                    .Include(x => x.Customer)
                    .Include(x => x.Customer.Address)
                    .SingleOrDefault(x => x.ID == id);
                return result;
            }
        }

        public List<Job> ReadAll()
        {
            using (var db = new DatabaseContext())
            {
                var result = db.Jobs
                    .Include(x => x.Customer)
                    .Include(x => x.Customer.Address)
                    .ToList();
                return result;
            }
        }
    }
}