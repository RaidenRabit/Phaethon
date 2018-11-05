using Core.Model;
using InternalApi.DataAccess;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace InternalApi.DataManagement
{
    public class CompanyData
    {
        public void Create(Company company)
        {
            using (var db = new DatabaseContext())
            {
                db.Companys.Add(company);
                db.SaveChanges();
            }
        }

        public Company Read(int id)
        {
            using (var db = new DatabaseContext())
            {
                Company company = db.Companys.Find(id);
                return company;
            }
        }

        public List<Company> GetCompanies() 
        {
            using (var db = new DatabaseContext())
            {
                return db.Companys.ToList();
            }
        } 

        public void Update(Company company)
        {
            using (var db = new DatabaseContext())
            {
                db.Companys.AddOrUpdate(company);

                //var entity = db.Companys.Find(company);
                //if (entity == null)
                //{
                //    return;
                //}

                //db.Entry(entity).CurrentValues.SetValues(company);
                db.SaveChanges();
            }
        }

        public void Delete(Company company)
        {
            using (var db = new DatabaseContext())
            {
                db.Companys.Attach(company);
                db.Companys.Remove(company);
                db.SaveChanges();
            }
        }
    }
}
