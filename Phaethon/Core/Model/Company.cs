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
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [DataMember]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string RegNumber { get; set; }

        [Required]
        [DataMember]
        [StringLength(100)]
        public string BankName { get; set; }

        [Required]
        [DataMember]
        [StringLength(100)]
        public string BankNumber { get; set; }

        [DataMember]
        [ForeignKey("LegalAddress_ID")]
        public virtual Address LegalAddress { get; set; }
        public int? LegalAddress_ID { get; set; }

        [DataMember]
        [ForeignKey("ActualAddress_ID")]
        public virtual Address ActualAddress { get; set; }
        public int? ActualAddress_ID { get; set; }

        [DataMember]
        public virtual ICollection<Representative> Representatives { get; set; }
    }
}
