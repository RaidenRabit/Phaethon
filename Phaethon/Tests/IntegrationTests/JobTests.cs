using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Core.Model;
using NUnit.Framework;
using Newtonsoft.Json;
using InternalApi.DataAccess;

namespace Tests.IntegrationTests
{
    public class JobTests : InternalTestFakeServerBase
    {

        Address _address;
        Customer _customer;
        private Job _job;
        
        private void InitializeData()
        {
            DateTime a = DateTime.Now;
            DateTime b = DateTime.Now;
            _address = new Address { City = "TestCity1", Number = "21", Street = "TestStreet1" };
            _customer = new Customer { Address = _address, Email = "testEmail@email.com", FamilyName = "TestFamily", GivenName = "TestGiven", Phone = "072379899" };
            _job = new Job { Customer = _customer, Description = "Repair TestProduct1", FinishedTime = a, StartedTime = b, JobName = "Repair", JobStatus = JobStatus_enum.Completed };
        }
        
        #region Post
        [Test]
        public async Task PostJob_NewJob_True()
        {
            //Setup
            InitializeData();
            string json = JsonConvert.SerializeObject(_job);
            var content = new StringContent(json);

            //Act
            var response = await _client.PostAsync("/Job/InsertOrUpdate", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(deserializedResponse);
        }

        [Test]
        public async Task PostJob_EmptyContent_InternalServerErrorReturned()
        {
            //Setup
            var content = new StringContent("");

            //Act
            var response = await _client.PostAsync("/Job/InsertOrUpdate", content);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            
        }
        #endregion

        #region Read
        [Test]
        public async Task ReadJob_CorrectID_SameObjectReturned()
        {
            //Setup
            InitializeData();
            JobDa jobDa = new JobDa();
            int id = jobDa.InsertOrUpdate(_job);

            //Act
            var result = await _client.GetAsync($"/Job/Read?id={id}");
            string json = await result.Content.ReadAsStringAsync();
            Job job = JsonConvert.DeserializeObject<Job>(json);

            //Assert
            Assert.IsTrue(result.IsSuccessStatusCode, $"{result.StatusCode}");//check if internal server error
            Assert.IsTrue(job.Equals(_job), "objects are not equal");//check if object received is the same
            
        }

        [Test]
        public async Task ReadJob_WrongId_BadRequest()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = "";

            //Act
            var result = await _client.GetAsync("Job/Read?" + parameters);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);//check if internal server error
            
        }
        #endregion


        
    }
}
