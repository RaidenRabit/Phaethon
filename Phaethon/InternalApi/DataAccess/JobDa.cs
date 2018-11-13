using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
                db.Addresses.AddOrUpdate(job.Customer.Address);
                job.Customer.Address_ID = job.Customer.Address.ID;
                db.Customers.AddOrUpdate(job.Customer);
                job.Customer_ID = job.Customer.ID;
                db.Jobs.AddOrUpdate(job);
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