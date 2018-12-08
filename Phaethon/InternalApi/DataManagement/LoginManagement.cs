using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;
using InternalApi.Security;
namespace InternalApi.DataManagement
{
    public class LoginManagement : ILoginManagement
    {
        
        private readonly LoginDa _LoginDa;


        internal LoginManagement()
        {
            _LoginDa = new LoginDa();
        }

        public bool CreateOrUpdate(Login login)
        {
            using (var db = new DatabaseContext())
            {
               // RegHash hash = new RegHash();
               //user.Password = hash.RegEncrypt(user.Password);
                
                _LoginDa.CreateOrUpdate(db, login);
                return true;
            }
        }

        public bool Delete(int id)
        {
            using (var db = new DatabaseContext())
            {
                Login login = _LoginDa.GetLogin(db, id);
                return _LoginDa.Delete(db, login);

            }
        }

        public Login GetLogin(int id)
        {
            using (var db = new DatabaseContext())
            {
                return _LoginDa.GetLogin(db, id);
                

            }
        }

        public int Login(Login login)
        {
            using (var db = new DatabaseContext())
            {
                return _LoginDa.Login(db, login);
            }
        }
    }
}