using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    [Serializable]
    [DataContract(Name = "Company")]
    public class Company
    {
        [Key]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "RegNumber")]
        public string RegNumber { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "Address")]
        public string Address { get; set; }

        [DataMember(Name = "BankNumber")]
        public string BankNumber { get; set; }


        public virtual ICollection<Representative> Representatives { get; set; } = new List<Representative>();
    }
}
