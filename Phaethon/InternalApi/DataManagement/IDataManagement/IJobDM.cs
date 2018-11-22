using System.Collections.Generic;
using Core.Model;

namespace InternalApi.DataManagement.IDataManagement
{
    internal interface IJobDm
    {
        int Create(Job job);

        Job Read(string id);

        List<Job> ReadAll(int numOfRecords, int jobId, string jobName, int jobStatus, string customerName, string description, string dateOption, string from, string to);
    }
}
