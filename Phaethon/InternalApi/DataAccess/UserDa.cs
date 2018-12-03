using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    public class UserDa
    {
        public void CreateOrUpdate(DatabaseContext db ,User user)
        {
            db.Users.AddOrUpdate(user);
            db.SaveChanges();
        }

        public bool Delete(DatabaseContext db, User user)
        {
            db.Users.Remove(user);
            return db.SaveChanges() > 0; 
        }

        public User GetUser(DatabaseContext db, int id)
        {
            return db.Users.SingleOrDefault(x => x.id == id);
        }

        public User Login(DatabaseContext db, User user)
        {
            return db.Users.Where(x => x.UserName.Equals(user.UserName) && x.Password.Equals(user.Password)).FirstOrDefault();
        }
    }
}