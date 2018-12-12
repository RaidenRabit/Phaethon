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
    public class TaxGroup
    {
        [Key]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [DataMember]
        [Range(0, 100)]
        public int Tax { get; set; }
    }
}
