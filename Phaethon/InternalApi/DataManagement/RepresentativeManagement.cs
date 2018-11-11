using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    public class RepresentativeManagement : IRepresentativeManagement
    {
        private readonly RepresentativeDa _representativeDa = null;

        public RepresentativeManagement()
        {
            _representativeDa = new RepresentativeDa();
        }

        public List<Representative> GetRepresentatives(int id)
        {
            return _representativeDa.GetRepresentatives(id);
        }
    }
}