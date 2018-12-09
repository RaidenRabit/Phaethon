using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    public class LoginDa
    {
        public void CreateOrUpdate(DatabaseContext db ,Login login)
        {
            db.Login.AddOrUpdate(login);
            db.SaveChanges();
        }

        public bool Delete(DatabaseContext db, Login login)
        {
            db.Login.Remove(login);
            return db.SaveChanges() > 0; 
        }

        public Login GetLogin(DatabaseContext db, int id)
        {
            return db.Login.SingleOrDefault(x => x.ID == id);
        }

        public Login Login(DatabaseContext db, string username)
        {
            return db.Login.SingleOrDefault(a => a.Username.Equals(username));
        }
    }
}