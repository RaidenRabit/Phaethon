using System.Collections.Generic;
using Core.Model;

namespace InternalApi.DataManagement.IDataManagement
{
    internal interface IJobDm
    {
        int Create(Job job);

        Job Read(string id);

        List<Job> ReadAll(int numOfRecords, int jobId, string jobName, string from, string to, int jobStatus, int dateOption, string customerName, string description);
    }
}
