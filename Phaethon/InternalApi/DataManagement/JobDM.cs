using System;
using System.Collections.Generic;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class JobDm : IJobDm
    {
        private readonly JobDa _jobDa;

        internal JobDm()
        {
            _jobDa = new JobDa();
        }

        public int Create(Job job)
        {
            return _jobDa.InsertOrUpdate(job);
        }

        public Job Read(string id)
        {
            if (id != null && !id.Equals(""))
            {
                int Id = Int32.Parse(id);
                Job job = _jobDa.Read(Id);
                if (job != null)
                    return job;
            }
            throw new Exception("No Job with such id");
        }

        public List<Job> ReadAll(JobQueryFilter jobQueryFilter)
        {
            if (jobQueryFilter.CustomerName == null)
                jobQueryFilter.CustomerName = "";
            if (jobQueryFilter.JobName == null)
                jobQueryFilter.JobName = "";
            if (jobQueryFilter.Description == null)
                jobQueryFilter.Description = "";
            if (jobQueryFilter.NumOfRecords <= 0)
                jobQueryFilter.NumOfRecords = 10;
            if (jobQueryFilter.From == jobQueryFilter.To && jobQueryFilter.From == new DateTime())
            {
                jobQueryFilter.From = _jobDa.GetEarliestEntry();
                jobQueryFilter.To = _jobDa.GetLatestEntry()?? DateTime.Now;
            }

            return _jobDa.ReadAll(jobQueryFilter);
        }
    }
}