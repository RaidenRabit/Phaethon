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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Company name")]
        public string Name { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Registration Number")]
        public string RegNumber { get; set; }

        [Required]
        [DataMember]
        public string Location { get; set; }

        [Required]
        [DataMember]
        public string Address { get; set; }
        
        [DataMember]
        public string BankNumber { get; set; }


        [DataMember]
        [DefaultValue(null)]
        public virtual ICollection<Representative> Representatives { get; set; }
    }
}
