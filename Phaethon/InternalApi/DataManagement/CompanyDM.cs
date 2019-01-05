using System.Collections.Generic;
using Core;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class CompanyDM : ICompanyDM
    {
        private readonly CompanyDa _companyDa;

        internal CompanyDM()
        {
            _companyDa = new CompanyDa();
        }

        public List<Company> GetCompanies()
        {
            using (var db = new DatabaseContext())
            {
                return _companyDa.GetCompanies(db);
            }
        }

        public Company GetCompany(int id)
        {
            using (var db = new DatabaseContext())
            {
                return _companyDa.GetCompany(db, id);
            }
        }
    }
}