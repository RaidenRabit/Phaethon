using Core.Model;

namespace InternalApi.DataManagement.IDataManagement
{
    internal interface IJobDm
    {
        bool Create(Job job);

        Job Read(int id);
    }
}
