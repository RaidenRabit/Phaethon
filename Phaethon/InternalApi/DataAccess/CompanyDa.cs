using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    public class CompanyDa
    {
        public void CreateOrUpdate(DatabaseContext db, Company company)
        {
            db.Companies.AddOrUpdate(company);
            db.SaveChanges();
        }

        public List<Company> GetCompanies(DatabaseContext db)
        {
            return db.Companies.ToList();
        }

        public Company GetCompany(DatabaseContext db, int id)
        {
            return db.Companies
                .Include(x => x.Representatives)
                .SingleOrDefault(x => x.ID == id);
        }
    }
}
