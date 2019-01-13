using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core;
using Core.Model;
using Core.Model.Filters;

namespace InternalApi.DataAccess
{
    public class CustomerDa
    {
        public void InsertOrUpdate(Customer customer)
        {
            using (var db = new DatabaseContext())
            {
                db.Addresses.AddOrUpdate(customer.Address);
                customer.Address_ID = customer.Address.ID;
                db.Customers.AddOrUpdate(customer);
                db.SaveChanges();
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