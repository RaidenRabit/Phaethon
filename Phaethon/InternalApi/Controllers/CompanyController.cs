using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalApi.DataAccess;

namespace InternalApi.Controller
{
    public class CompanyController
    {
        private readonly CompanyDa _companyDa = null;

        public CompanyController()
        {
            _companyDa = new CompanyDa();
        }

        public Company Read(int id)
        {
            try
            {
                return _companyDa.Read(id);
            }
            catch
            {
                return null;
            }
        }

        public List<Company> GetCompanies()
        {
            try
            {
                return _companyDa.GetCompanies();
            }
            catch
            {
                return null;
            }
        }
    }
}
