using Core.Model;
using InternalApi.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalApi.DataManagement
{
    public class RepresentativeData
    {
        public void Create(Representative representative)
        {
            using (var db = new DatabaseContext())
            {
                db.Representatives.Add(representative);
                db.SaveChanges();
            }
        }

        public Representative Read(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Representatives.Find(id);
            }
        }

        public List<Representative> GetCompaniesRepresentatives(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Representatives.Where(r => r.Company.ID == id).ToList();
            }
        }

        public void Update(Representative representative)
        {
            using (var db = new DatabaseContext())
            {
                db.Representatives.AddOrUpdate(representative);
                db.SaveChanges();
            }
        }

        public void Delete(Representative representative)
        {
            using (var db = new DatabaseContext())
            {
                db.Representatives.Attach(representative);
                db.Representatives.Remove(representative);
                db.SaveChanges();
            }
        }
    }
}
