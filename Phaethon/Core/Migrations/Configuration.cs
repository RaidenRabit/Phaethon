using System.Text;
using Core.Model;

namespace Core.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Core.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Core.DatabaseContext";
        }

        protected override void Seed(DatabaseContext context)
        {
            context.Login.AddOrUpdate(
                new Login() { Username = "test", Password = "we3unzpp73x19fEH177omGzM+4HbLc/cxzFtSHLfplU=", Salt = Encoding.ASCII.GetBytes("0xD0E37B9A16E43A01006693B7710EE85F0908DC9F7CBEDBB7EFDA05C32B8F9D1F") }
            );//password= 12345
        }
    }
}
