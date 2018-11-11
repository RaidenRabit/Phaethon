using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    public class RepresentativeDa
    {
        internal List<Representative> GetRepresentatives(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Representatives.Where(r => r.Company.ID == id).ToList();
            }
        }
    }
}