using System.Collections.Generic;
using Core.Model;
using Core.Model.Filters;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class CustomerDM : ICustomerDM
    {
        private readonly CustomerDa _customerDa;

        internal CustomerDM()
        {
            _customerDa = new CustomerDa();
        }

        public List<Customer> ReadAll(CustomerQueryFilter customerQueryFilter)
        {
            if (customerQueryFilter.Email == null)
                customerQueryFilter.Email = "";
            if (customerQueryFilter.FamilyName == null)
                customerQueryFilter.FamilyName = "";
            if (customerQueryFilter.GivenName == null)
                customerQueryFilter.GivenName = "";
            if (customerQueryFilter.Phone == null)
                customerQueryFilter.Phone = "";

            return _customerDa.ReadAll(customerQueryFilter);
        }
    }
}