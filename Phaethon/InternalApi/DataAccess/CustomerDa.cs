using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Core.Model;
using Core.Model.Filters;

namespace InternalApi.DataAccess
{
    public class CustomerDa
    {
        public int InsertOrUpdate(Customer customer)
        {
            using (var db = new DatabaseContext())
            {
                db.Addresses.AddOrUpdate(customer.Address);
                customer.Address_ID = customer.Address.ID;
                db.Customers.AddOrUpdate(customer);
                db.SaveChanges();
                return customer.ID;
            }
        }

        public List<Customer> ReadAll(CustomerQueryFilter customerQueryFilter)
        {
            using (var db = new DatabaseContext())
            {
                return db.Customers
                    .Include(x => x.Address)
                    .Where(x => x.Email.Contains(customerQueryFilter.Email) &&
                                (x.GivenName).Contains(customerQueryFilter.GivenName) &&
                                x.FamilyName.Contains(customerQueryFilter.FamilyName) &&
                                x.Phone.Contains(customerQueryFilter.Phone)
                            )
                    .ToList();
            }
        }
    }
}