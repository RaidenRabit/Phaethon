using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using InternalApi.DataAccess;
using NUnit.Framework;
using System.Security.Cryptography;

namespace Tests.IntegrationTests
{
   public class LoginTest : InternalTestFakeServerBase
    {
        private Login _userModel;
        private Login _userModel2;
        private string _name;
        private string _name2;
        private string _originalPass;
        private LoginDa _loginDa;

        private void InitializeData()
        {
            _userModel = new Login { Username = "Subject1", Password = "12355557" };
            _userModel2 = new Login { Username = "Subject2", Password = "123456" };

            _userModel2.Salt = GenerateSalt();
            _originalPass = _userModel2.Password;
            _userModel2.Password = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(_userModel2.Password), _userModel2.Salt));
            _loginDa = new LoginDa { }; 

            using (var db = new DatabaseContext())
            {
                _loginDa.CreateOrUpdate(db, _userModel2);
            }
        }
        [TearDown]
        public void Cleanup()
        {
            _loginDa = new LoginDa();

            using (var db = new DatabaseContext())
            {
                if (_userModel2.Username != null)

            {
                    _userModel2 = db.Login.SingleOrDefault(a => a.Username.Equals(_userModel2.Username));
                
                    Login deleted = db.Login.Attach(_userModel2);
                    _loginDa.Delete(db, deleted);
                }
                else
                {
                    db.SaveChanges();
                }
               
            }
        }

        #region Post
        [Test]
        public async Task Login_CorrectLoginInfo_OK()
        {
            //Setup
            InitializeData();
            _userModel2.Password = _originalPass;

            //Act
            var response = await _client.PostAsJsonAsync("Login/Login", _userModel2);
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            
        }

        [Test]
        public async Task Login_NullPasswordLoginInfo_BadRequest()
        {
            //Setup
            InitializeData();
            
            //Act
            _userModel2.Password = null;
            var response = await _client.PostAsJsonAsync("Login/Login", _userModel2);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Test]
        public async Task Login_NullUserNameLoginInfo_BadRequest()
        {
            //Setup
            InitializeData();

            //Act
            _userModel2.Username = null;
            var response = await _client.PostAsJsonAsync("Login/Login", _userModel2);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }
        #endregion

        #region Delete
        [Test]
        public async Task Delete_CorrectLoginInfo_OK()
        {
            // Setup
           
                InitializeData();
           
            //Act          
            var response = await _client.PostAsJsonAsync("Login/Delete", _userModel2.ID.ToString());
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            _userModel2.Username = null;
        }
        #endregion

        #region Add
        [Test]
        public async Task New_User_OK()
        {
            // Setup
            _userModel2 = new Login { Username = "Subject2", Password = "123456" };
            //Act
            var response = await _client.PostAsJsonAsync("Login/CreateOrUpdate", _userModel2);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            



        }
        #endregion

        #region Edit
        [Test]
        public async Task Edit_User_OK()
        {
            // Setup
            InitializeData();
            _name = _userModel2.Username;
            _userModel2.Username = "Maybe";
            _name2 = _userModel2.Username;
            //Act
            var response = await _client.PostAsJsonAsync("Login/CreateOrUpdate", _userModel2);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(_name, _name2);
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
