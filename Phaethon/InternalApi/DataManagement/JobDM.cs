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

        public bool Create(Job job)
        {
            return _jobDa.Create(job);
        }

        public Job Read(int id)
        {
            return _jobDa.Read(id);
        }
    }
}