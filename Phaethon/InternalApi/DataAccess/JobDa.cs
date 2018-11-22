using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
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

        public List<Job> ReadAll(string jobName, string customerName, int jobStatus, int numOfRecords, int jobId, string description, int dateOption, DateTime from, DateTime to)
        {
            using (var db = new DatabaseContext())
            {
                return db.Jobs
                    .Include(x => x.Customer)
                    .Include(x => x.Customer.Address)
                    .Where(x => (jobId == 0 || x.ID == jobId) &&
                                x.JobName.Contains(jobName) &&
                                (x.Customer.GivenName + x.Customer.FamilyName).Contains(customerName) &&
                                x.Description.Contains(description) &&
                                (jobStatus == 0 || (int)x.JobStatus == jobStatus)
                    )
                    .Take(numOfRecords)
                    .ToList();


            }
        }
    }
}