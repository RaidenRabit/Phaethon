using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class JobDa
    {
        internal int InsertOrUpdate(Job job)
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

        internal Job Read(int id)
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

        internal List<Job> ReadAll(JobQueryFilter jobQueryFilter)
        {
            jobQueryFilter.To = jobQueryFilter.To.AddDays(1);
            using (var db = new DatabaseContext())
            {
                return db.Jobs
                    .Include(x => x.Customer)
                    .Include(x => x.Customer.Address)
                    .Where(x => (jobQueryFilter.JobId == 0 || x.ID == jobQueryFilter.JobId) &&
                                x.JobName.Contains(jobQueryFilter.JobName) &&
                                (x.Customer.GivenName + x.Customer.FamilyName).Contains(jobQueryFilter.CustomerName) &&
                                x.Description.Contains(jobQueryFilter.Description) &&
                                (jobQueryFilter.JobStatus == 0 || (int)x.JobStatus == jobQueryFilter.JobStatus)
                    )
                    .Where(x => (jobQueryFilter.DateOption == 0 && jobQueryFilter.From <= x.StartedTime && x.StartedTime < jobQueryFilter.To) ||
                                (jobQueryFilter.DateOption == 1 && jobQueryFilter.From <= x.FinishedTime && x.FinishedTime < jobQueryFilter.To))
                    .Take(jobQueryFilter.NumOfRecords)
                    .ToList();
            }
        }

        internal DateTime GetEarliestEntry()
        {
            using (var db = new DatabaseContext())
            {
                return db.Jobs
                    .OrderBy(c => c.StartedTime)
                    .FirstOrDefault().StartedTime;
            }
        }

        internal DateTime? GetLatestEntry()
        {
            using (var db = new DatabaseContext())
            {
                return db.Jobs
                    .OrderByDescending(c => c.FinishedTime)
                    .FirstOrDefault().FinishedTime;
            }
        }
    }
}