using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    [Serializable]
    [DataContract(Name = "Representative")]
    public class Representative
    {
        [Key]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }


        [DataMember(Name = "Company")]
        public virtual Company Company { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
