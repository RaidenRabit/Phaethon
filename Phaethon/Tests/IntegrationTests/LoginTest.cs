using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using InternalApi.DataAccess;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Security.Cryptography;

namespace Tests.IntegrationTests
{

   public class LoginTest : InternalTestFakeServerBase
    {
        private Login userModel;
        private Login userModel2;
        private Login userModel3;
        private int count1;
        private int count2;
        private string name;
        private string name2;

        private void InitializeData()
        {
            userModel3 = new Login { Username = "Subject1", Password = "12000" };
            userModel = new Login { Username = "Philip", Password = "14789632" };
            userModel2 = new Login { Username = "Subject2", Password = "123456" };
            
        }

        #region Post

        [Test]
        public async Task Login_CorrectLoginInfo_OK()
        {
            //Setup
            InitializeData();
            userModel2.Salt = GenerateSalt();
            userModel2.Password = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(userModel2.Password), userModel2.Salt));
            using (var db = new DatabaseContext())
            {
                
                db.Login.Add(userModel2);
                db.SaveChanges();
            }
            var response = await _client.PostAsJsonAsync("Login/Login", userModel2);
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            using (var db = new DatabaseContext())
            {
                db.Login.Attach(userModel2);
                db.Login.Remove(userModel2);
                db.SaveChanges();
            }
        }

        [Test]
        public async Task Login_IncorrectPasswordLoginInfo_InternalServerError()
        {
            //Setup
            InitializeData();
            
            //Act
            using (var db = new DatabaseContext())
            {
                db.Login.Add(userModel);
                db.SaveChanges();
            }

            
            var response = await _client.PostAsJsonAsync("Login/Login", userModel);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            
            using (var db = new DatabaseContext())    
            {
                db.Login.Attach(userModel);
                db.Login.Remove(userModel);
                db.SaveChanges();
            }
        }

        [Test]
        public async Task Login_IncorrectUserNameLoginInfo_InternalServerError()
        {
            //Setup
            InitializeData();
            
            userModel2.Salt = GenerateSalt();
            userModel2.Password = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(userModel2.Password), userModel2.Salt));
            //Act
            using (var db = new DatabaseContext())
            {
                db.Login.Add(userModel2);
                db.SaveChanges();
            }

            userModel2.Username = "Complete";
            var response = await _client.PostAsJsonAsync("Login/Login", userModel2);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            
            using (var db = new DatabaseContext())
            {
                db.Login.Attach(userModel2);
                db.Login.Remove(userModel2);
                db.SaveChanges();
            }
        }
        #endregion

        #region Delete

        [Test]
        public async Task Delete_CorrectLoginInfo_OK()
        {
            // Setup
           
                InitializeData();
           
            //Act
            using (var db = new DatabaseContext())
            {
                db.Login.Add(userModel);
                db.SaveChanges();

            }
            var response = await _client.PostAsJsonAsync("Login/Delete", userModel.ID.ToString());

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            

        }
        #endregion

        #region Add
        [Test]
        public async Task New_User_OK()
        {
            // Setup
            InitializeData();

            //Act
            using (var db = new DatabaseContext())
            {
                count1 = db.Login.Count();
                db.Login.Add(userModel);
                db.SaveChanges();
                
            }
            using (var db = new DatabaseContext())
            {
                count2 = db.Login.Count();
                
                db.SaveChanges();

            }
            var response = await _client.PostAsJsonAsync("Login/CreateOrUpdate", userModel);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(count1, count2);
            using (var db = new DatabaseContext())
            {
                db.Login.Attach(userModel);
                db.Login.Remove(userModel);
                db.SaveChanges();
            }
        }
        #endregion

        #region Edit
        [Test]
        public async Task Edit_User_OK()
        {
            // Setup
            InitializeData();
            name = userModel2.Username;
            userModel2.Username = "Maybe";
            using (var db = new DatabaseContext())
            {
                db.Login.Add(userModel2);
                db.SaveChanges();
                name2 = userModel2.Username;
            }
            

            //Act
            var response = await _client.PostAsJsonAsync("Login/CreateOrUpdate", userModel2);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(name, name2);
            
            using (var db = new DatabaseContext())
            {
                db.Login.Attach(userModel2);
                db.Login.Remove(userModel2);
                db.SaveChanges();
            }
        }
        #endregion

        #region Encryption
        private const int SaltSize = 32;

        private byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[SaltSize];
                rng.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        private byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }
        #endregion
    }
}
