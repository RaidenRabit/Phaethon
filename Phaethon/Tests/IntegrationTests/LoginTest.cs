using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Tests.IntegrationTests
{

   public class LoginTest : InternalTestFakeServerBase
    {
        private Login userModel;
        private Login userModel2;
        private Login userModel3;
        private int count1;
        private int count2;

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
            
            //Act
            using (var db = new DatabaseContext())
            {
                db.Login.Add(userModel);
                db.SaveChanges();
            }
            var response = await _client.PostAsJsonAsync("User/Login", userModel);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            using (var db = new DatabaseContext())
            {
                db.Login.Remove(userModel);
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
            var response = await _client.PostAsJsonAsync("User/Login", userModel2);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            using (var db = new DatabaseContext())
            {
                db.Login.Remove(userModel);
                db.SaveChanges();
            }
        }

        [Test]
        public async Task Login_IncorrectUserNameLoginInfo_InternalServerError()
        {
            //Setup
            InitializeData();
            //Act
            using (var db = new DatabaseContext())
            {
                db.Login.Add(userModel);
                db.SaveChanges();
            }
            var response = await _client.PostAsJsonAsync("User/Login", userModel2);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            using (var db = new DatabaseContext())
            {
                db.Login.Remove(userModel);
                db.SaveChanges();
            }
        }
        #endregion

        #region Delete

        [Test]
        public async Task Delete_CorrectLoginInfo_OK()
        {
            // Setup
            try
            {
                InitializeData();
            }
            catch (Exception e)
            {
                
                throw e;
            }
            InitializeData();
            userModel.ID = 3007;

            //Act
            using (var db = new DatabaseContext())
            {
                db.Login.Add(userModel);
                db.SaveChanges();

            }
            var response = await _client.PostAsJsonAsync("User/Delete", userModel.ID.ToString());

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            using (var db = new DatabaseContext())
            {
                db.Login.Remove(userModel);
                db.SaveChanges();
            }

        }
        #endregion

        #region Add
        public async Task New_User_OK()
        {
            // Setup
            InitializeData();

            //Act
            using (var db = new DatabaseContext())
            {
                count1 = db.Login.Count();
                db.Login.Add(userModel);
                count2 = db.Login.Count();
                db.SaveChanges();
            }
            var response = await _client.PostAsJsonAsync("User/CreateOrUpdate", userModel);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(count1, count2);
            using (var db = new DatabaseContext())
            {
                db.Login.Remove(userModel);
                db.SaveChanges();
            }
        }
        #endregion

        #region Edit
        public async Task Edit_User_OK()
        {
            // Setup
            InitializeData();
            count1 = userModel2.ID;
            userModel2.ID = 111;
            using (var db = new DatabaseContext())
            {
                db.Login.Add(userModel2);
                db.SaveChanges();
                count2 = userModel2.ID;
            }
            

            //Act
            var response = await _client.PostAsJsonAsync("User/CreateOrUpdate", userModel);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(count1, count2);
            
            using (var db = new DatabaseContext())
            {
                db.Login.Remove(userModel);
                db.SaveChanges();
            }
        }
        #endregion
    }
}
