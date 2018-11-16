using System;
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

        public List<Job> ReadAll(string jobName, string from, string to, int dateOption, string customerName, int jobStatus, int numOfRecords, int jobId, string description)
        {
            using (var db = new DatabaseContext())
            {
                var query = db.Jobs
                    .Include(x => x.Customer)
                    .Include(x => x.Customer.Address)
                    .Where(x => (jobId == 0 || x.ID == jobId) &&
                                x.JobName.Contains(jobName) &&
                                (x.Customer.GivenName + x.Customer.FamilyName).Contains(customerName) &&
                                x.Description.Contains(description) &&
                                (jobStatus == 0 || (int)x.JobStatus == jobStatus)
                    )
                    .Take(numOfRecords);

                if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                {
                    DateTime.TryParse(from, out var fromTime);
                    DateTime.TryParse(to, out var toTime);
                    query = query.Where(x => (dateOption == 0 && fromTime <= x.StartedTime && x.StartedTime <= toTime ||
                                              dateOption == 1 && fromTime <= x.FinishedTime && x.FinishedTime <= toTime)
                    );

                }

                return query.ToList();
            }
        }
    }
}