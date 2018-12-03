using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    public class UserManagement : IUserManagement
    {
        private readonly UserDa _UserDa;

        internal UserManagement()
        {
            _UserDa = new UserDa();
        }

        public bool CreateOrUpdate(User user)
        {
            using (var db = new DatabaseContext())
            {
                _UserDa.CreateOrUpdate(db, user);
                return true;
            }
        }

        public bool DeleteUser(int id)
        {
            using (var db = new DatabaseContext())
            {
                User user = _UserDa.GetUser(db, id);
                return _UserDa.Delete(db, user);
            }
        }

        public User GetUser(int id)
        {
            using (var db = new DatabaseContext())
            {
                return _UserDa.GetUser(db, id);
            }
        }

        public int Login(User user)
        {
            using (var db = new DatabaseContext())
            {
                user = _UserDa.Login(db, user);
                if (user != null)
                {
                    return user.id;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}