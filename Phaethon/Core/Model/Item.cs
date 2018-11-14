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
    public class Item
    {
        [Key]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Serial number")]
        public string SerNumber { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Price")]
        public decimal Price { get; set; }

        [Required]
        [DataMember]
        [DisplayName("In stock")]
        public bool InStock { get; set; }

        [DataMember]
        [ForeignKey("Product_ID")]
        public virtual Product Product { get; set; }
        public int? Product_ID { get; set; }

        public virtual ICollection<Invoice> Elements { get; set; }
    }
}
