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
using InternalApi.DataManagement;

namespace Tests.IntegrationTests
{

   public class LoginTest : InternalTestFakeServerBase
    {
        private Login userModel;
        private Login userModel2;
        private int count1;
        private int count2;
        private string name;
        private string name2;
        private LoginDa loginDa;

        private void InitializeData()
        {
            userModel = new Login { Username = "Subject1", Password = "12355557" };
            userModel2 = new Login { Username = "Subject2", Password = "123456" };

           // userModel2.Salt = GenerateSalt();
            // userModel2.Password = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(userModel2.Password), userModel2.Salt));
            loginDa = new LoginDa { }; 

            using (var db = new DatabaseContext())
            {
                loginDa.CreateOrUpdate(db, userModel2);
            }
        }

        private void Cleanup()
        {
            loginDa = new LoginDa { };
            
            using (var db = new DatabaseContext())
            {
                Login deleted = db.Login.Attach(userModel2);
                loginDa.Delete(db, deleted);
            }
        }

        #region Post

        [Test]
        public async Task Login_CorrectLoginInfo_OK()
        {
            //Setup
            InitializeData();
            
            var response = await _client.PostAsJsonAsync("Login/Login", userModel2);
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            Cleanup();
        }

        [Test]
        public async Task Login_NullPasswordLoginInfo_BadRequest()
        {
            //Setup
            InitializeData();
            
            //Act
            userModel2.Password = null;
            var response = await _client.PostAsJsonAsync("Login/Login", userModel2);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            Cleanup();
        }

        [Test]
        public async Task Login_NullUserNameLoginInfo_BadRequest()
        {
            //Setup
            InitializeData();

            //Act
            userModel2.Username = null;
            var response = await _client.PostAsJsonAsync("Login/Login", userModel2);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            Cleanup();
        }
        #endregion

        #region Delete

        [Test]
        public async Task Delete_CorrectLoginInfo_OK()
        {
            // Setup
           
                InitializeData();
           
            //Act
           
            var response = await _client.PostAsJsonAsync("Login/Delete", userModel2.ID.ToString());

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
            }
            var response = await _client.PostAsJsonAsync("Login/CreateOrUpdate", userModel);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(count1, count2);
            loginDa = new LoginDa { };
            using (var db = new DatabaseContext())
            {
                Login deleted = db.Login.Attach(userModel);
                loginDa.Delete(db, deleted);
            }
            Cleanup();
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
            name2 = userModel2.Username;
            //Act
            var response = await _client.PostAsJsonAsync("Login/CreateOrUpdate", userModel2);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(name, name2);
            Cleanup();
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
