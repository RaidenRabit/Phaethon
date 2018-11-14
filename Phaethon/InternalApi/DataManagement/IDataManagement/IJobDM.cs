using System.Collections.Generic;
using Core.Model;

namespace InternalApi.DataManagement.IDataManagement
{
    internal interface IJobDm
    {
        int Create(Job job);

        Job Read(string id);

        List<Job> ReadAll();
    }
}
