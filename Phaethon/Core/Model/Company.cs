using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Company
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string RegNumber { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string BankNumber { get; set; }

        public virtual ICollection<Representative> Representatives { get; set; } = new List<Representative>();
    }
}
