using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class CompanyDa
    {
        internal void CreateOrUpdate(DatabaseContext db, Company company)
        {
            db.Companies.AddOrUpdate(company);
            db.SaveChanges();
        }

        internal List<Company> GetCompanies()
        {
            using (var db = new DatabaseContext())
            {
                return db.Companies.ToList();
            }
        }

        internal Company GetCompany(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Companies
                    .Include(x => x.Representatives)
                    .SingleOrDefault(x => x.ID == id);
            }
        }
    }
}
