using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    class CompanyDa
    {
        internal Company Read(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Companies
                    .Include(x => x.Representatives)
                    .SingleOrDefault(x => x.ID == id);
            }
        }

        internal List<Company> GetCompanies()
        {
            using (var db = new DatabaseContext())
            {
                return db.Companies.ToList();
            }
        }
    }
}
