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
    public class Representative
    {
        [Key]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Representative name")]
        [StringLength(100)]
        public string Name { get; set; }

        [DataMember]
        [ForeignKey("Company_ID")]
        public virtual Company Company { get; set; }
        public int? Company_ID { get; set; }
    }
}
