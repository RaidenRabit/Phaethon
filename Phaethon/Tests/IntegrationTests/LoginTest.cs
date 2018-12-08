using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{

   public class LoginTest : InternalTestFakeServerBase
    {
        private Login userModel;

        private void InitializeData()
        {
            userModel = new Login { Username = "Philip", Password = "14789632" };
        }

        #region Post

        [Test]
        public async Task Login_CorrectLoginInfo_OK()
        {
            //Setup
            InitializeData();
            //Act
            var response = await _client.PostAsJsonAsync("User/Login", userModel);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Login_IncorrectPasswordLoginInfo_InternalServerError()
        {
            //Setup
            userModel = new Login { Username = "Philip", Password = "1" };
            //Act
            var response = await _client.PostAsJsonAsync("User/Login", userModel);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Test]
        public async Task Login_IncorrectUserNameLoginInfo_InternalServerError()
        {
            //Setup
            userModel = new Login { Username = "Wrong", Password = "14789632" };
            //Act
            var response = await _client.PostAsJsonAsync("User/Login", userModel);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        #endregion

        #region Delete

        [Test]
        public async Task Delete_CorrectLoginInfo_OK()
        {
            // Setup
            InitializeData();
            userModel.ID = 2;

            //Act
            var response = await _client.PostAsJsonAsync("User/Delete", userModel.id.ToString());

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
        #endregion
    }
}
