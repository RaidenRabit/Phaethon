using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    [Serializable]
    [DataContract]
    public class Company
    {
        [Key]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataMember]
        [StringLength(100)]
        public string RegNumber { get; set; }
        
        [DataMember]
        [StringLength(100)]
        public string BankNumber { get; set; }

        [DataMember]
        [ForeignKey("Address_ID")]
        public virtual Address Address { get; set; }
        public int? Address_ID { get; set; }

        [DataMember]
        [ForeignKey("Location_ID")]
        public virtual Address Location { get; set; }
        public int? Location_ID { get; set; }

        [DataMember]
        public virtual ICollection<Representative> Representatives { get; set; }
    }
}
