using System;
using System.Collections.Generic;
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
    public class JobApis : InternalApiFakeServer
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
            _job = new Job { Customer = _customer, Description = "Repair TestProduct1", FinishedTime = a, StartedTime = b, JobName = "Repair", JobStatus = JobStatus_enum.Unassigned };
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
            var deserializedResponse = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
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
        public async Task ReadJob_EmptyString_BadRequest()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = "";

            //Act
            var result = await _client.GetAsync("Job/Read?" + parameters);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);//check if internal server error
            
        }

        [Test]
        public async Task ReadJob_NegativeString_BadRequest()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = "-1";

            //Act
            var result = await _client.GetAsync("Job/Read?" + parameters);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);//check if internal server error

        }
        [Test]
        public async Task ReadJob_Character_BadRequest()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = "as";

            //Act
            var result = await _client.GetAsync("Job/Read?" + parameters);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);//check if internal server error

        }


        [Test]
        public async Task ReadJob_ReadAllNullFilter_ListOfAllJobs()
        {
            //Setup
            InitializeData();
            JobDa _jobDa = new JobDa();
            _jobDa.InsertOrUpdate(_job);
            _job.ID = 0;
            _jobDa.InsertOrUpdate(_job);

            //Act
            var result = await _client.PostAsync("Job/ReadAll", null);
            string json = await result.Content.ReadAsStringAsync();
            List<Job> jobs = JsonConvert.DeserializeObject<List<Job>>(json);

            //Assert
            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.IsTrue(jobs.Count > 0, $"Job Count: {jobs.Count}");

        }
        [Test]
        public async Task ReadJob_ReadAllEmptyFilter_ListOfAllJobs()
        {
            //Setup
            InitializeData();
            JobDa _jobDa = new JobDa();
            _jobDa.InsertOrUpdate(_job);
            _job.ID = 0;
            _jobDa.InsertOrUpdate(_job);

            //Act
            var result = await _client.PostAsJsonAsync("Job/ReadAll", new JobQueryFilter());
            string json = await result.Content.ReadAsStringAsync();
            List<Job> jobs = JsonConvert.DeserializeObject<List<Job>>(json);

            //Assert
            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.IsTrue(jobs.Count > 0, $"Job Count: {jobs.Count}");

        }

        [Test]
        public async Task ReadJob_ReadAllFullFilter_ListOfJobs()
        {
            //Setup
            InitializeData();
            JobDa _jobDa = new JobDa();
            _jobDa.InsertOrUpdate(_job);
            _job.ID = 0;
            _jobDa.InsertOrUpdate(_job);
            JobQueryFilter jobQueryFilter = new JobQueryFilter
            {
                NumOfRecords = 10,
                DateOption = 1,
                From = DateTime.Today,
                To = DateTime.Now.AddDays(1),
                JobName = "Repair",
                CustomerName = "Test",
                Description = "Test",
                JobStatus = 0,
                JobId = 0,
            };

            //Act
            var result = await _client.PostAsJsonAsync("Job/ReadAll", jobQueryFilter);
            string json = await result.Content.ReadAsStringAsync();
            List<Job> jobs = JsonConvert.DeserializeObject<List<Job>>(json);

            //Assert
            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.IsTrue(jobs.Count >= 2, $"Job Count: {jobs.Count}");

        }
        #endregion

        #region Put

        [Test]
        public async Task PutJob_UpdateAddress_JobId()
        {
            //Set up
            InitializeData();
            JobDa jobDa = new JobDa();
            int jobId = jobDa.InsertOrUpdate(_job);
            Job job = jobDa.Read(jobId);
            _address = new Address{ID = job.Customer.Address.ID, City = "TestChangedCity1", Number = "21", Street = "TestChangedStreet1"};
            _job.Customer.Address = _address;

            //Act
            var response = await _client.PostAsJsonAsync("/Job/InsertOrUpdate", _job);
            var deserializedResponse = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            Job putJob = jobDa.Read(deserializedResponse);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(putJob.Equals(_job));

        }

        [Test]
        public async Task PutJob_UpdateCustomer_JobId()
        {
            //Set up
            InitializeData();
            JobDa jobDa = new JobDa();
            int jobId = jobDa.InsertOrUpdate(_job);
            Job job = jobDa.Read(jobId);
            _customer = new Customer { ID = job.Customer.ID, Email = "changedTestEmail@email.com", FamilyName = "TestChangedFamily", GivenName = "TestChangedGiven", Phone = "072379899" };
            _job.Customer.Address = _address;

            //Act
            var response = await _client.PostAsJsonAsync("/Job/InsertOrUpdate", _job);
            var deserializedResponse = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            Job putJob = jobDa.Read(deserializedResponse);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(putJob.Equals(_job));
        }

        [Test]
        public async Task PutJob_UpdateJob_JobId()
        {
            //Set up
            InitializeData();
            JobDa jobDa = new JobDa();
            int jobId = jobDa.InsertOrUpdate(_job);
            Job job = jobDa.Read(jobId);
            _job.JobName = "testChangedJobName";

            //Act
            var response = await _client.PostAsJsonAsync("/Job/InsertOrUpdate", _job);
            var deserializedResponse = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            Job putJob = jobDa.Read(deserializedResponse);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(!putJob.Equals(job));
            Assert.IsTrue(putJob.ID.Equals(job.ID));
            Assert.IsTrue(putJob.JobName.Equals("testChangedJobName"));
        }

        [Test]
        public async Task PutJob_UpdateJobStatus_JobId()
        {

            //Set up
            InitializeData();
            JobDa jobDa = new JobDa();
            int jobId = jobDa.InsertOrUpdate(_job);
            Job job = jobDa.Read(jobId);
            _job.JobStatus = JobStatus_enum.InProgress;

            //Act
            var response = await _client.PostAsJsonAsync("/Job/InsertOrUpdate", _job);
            var deserializedResponse = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            Job putJob = jobDa.Read(deserializedResponse);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(!putJob.Equals(job));
            Assert.IsTrue(putJob.ID.Equals(job.ID));
            Assert.IsTrue(putJob.JobStatus.Equals(JobStatus_enum.InProgress));
        }

        [Test]
        public async Task PutJob_UpdateJobStatusComplete_JobId()
        {

            //Set up
            InitializeData();
            JobDa jobDa = new JobDa();
            int jobId = jobDa.InsertOrUpdate(_job);
            Job job = jobDa.Read(jobId);
            _job.JobStatus = JobStatus_enum.Completed;

            //Act
            var response = await _client.PostAsJsonAsync("/Job/InsertOrUpdate", _job);
            var deserializedResponse = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            Job putJob = jobDa.Read(deserializedResponse);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(!putJob.Equals(job));
            Assert.IsTrue(putJob.ID.Equals(job.ID));
            Assert.IsTrue(putJob.JobStatus.Equals(JobStatus_enum.Completed));
        }

        [Test]
        public async Task PutJob_UpdateJobNoOrInvalidDateTime_BadRequest()
        {
            //Set up
            InitializeData();
            JobDa jobDa = new JobDa();
            int jobId = jobDa.InsertOrUpdate(_job);
            Job job = jobDa.Read(jobId);
            _job = new Job { ID = job.ID, Customer = job.Customer, JobName = "testChangedJobName" };

            //Act
            var response = await _client.PostAsJsonAsync("/Job/InsertOrUpdate", _job);

            //Assert
            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }
        #endregion



    }
}
