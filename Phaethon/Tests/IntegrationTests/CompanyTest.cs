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
    public class CompanyTest
    {
        #region GetCompanies
        [Test]
        public void GetCompanies_MethodCalled_CompaniesReturned()
        {
            //Setup
            HttpClient client = new HttpClient();

            //Act
            var result = client.GetAsync("http://localhost:64007/Company/GetCompanies").Result;
            string json = result.Content.ReadAsStringAsync().Result;
            List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(json);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if internal server error
            Assert.AreNotEqual(null, companies);//Check if result returned
        }
        #endregion

        #region Read
        [Test]
        public void Read_CorrectID_SameObjectReturned()
        {
            //Setup
            HttpClient client = new HttpClient();
            string json = client.GetAsync("http://localhost:64007/Company/GetCompanies").Result.Content.ReadAsStringAsync().Result;
            List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(json);
            if (companies.Count < 1)//if no invoices create new
            {
                new InvoiceTest().Create_NewInvoiceObject_ObjectCreated();

                json = client.GetAsync("http://localhost:64007/Company/GetCompanies").Result.Content.ReadAsStringAsync().Result;
                companies = JsonConvert.DeserializeObject<List<Company>>(json);
            }

            //Act
            var result = client.GetAsync("http://localhost:64007/Company/Read?id=" + companies[0].ID).Result;
            json = result.Content.ReadAsStringAsync().Result;
            Company company = JsonConvert.DeserializeObject<Company>(json);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if internal server error
            Assert.AreEqual(true, company.ID == companies[0].ID &&
                                      company.Address.Equals(companies[0].Address) &&
                                      company.BankNumber.Equals(companies[0].BankNumber) &&
                                      company.Location.Equals(companies[0].Location) &&
                                      company.Name.Equals(companies[0].Name) &&
                                      company.RegNumber.Equals(companies[0].RegNumber));//info of existing was updated
        }

        [Test]
        public void Read_WrongId_NullReturned()
        {
            //Setup
            HttpClient client = new HttpClient();

            //Act
            var result = client.GetAsync("http://localhost:64007/Company/Read?id=" + 0).Result;
            string json = result.Content.ReadAsStringAsync().Result;
            Company company = JsonConvert.DeserializeObject<Company>(json);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if internal server error
            Assert.AreEqual(null, company);//check if object received is the sames
        }
        #endregion
    }
}
