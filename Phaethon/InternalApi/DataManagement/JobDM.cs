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

        public List<Job> ReadAll(int numOfRecords, int jobId, string jobName, int jobStatus, string customerName, string description, string dateOption, string from, string to)
        {
            DateTime.TryParse(from, out var fromTime);
            DateTime.TryParse(from, out var toTime);
            Int32.TryParse(dateOption, out var dateOp);
            return _jobDa.ReadAll(jobName, customerName, jobStatus, numOfRecords, jobId, description, dateOp, fromTime, toTime);
        }
    }
}