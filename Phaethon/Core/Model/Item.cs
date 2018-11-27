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
        
        [DataMember]
        [DisplayName("Serial number")]
        [StringLength(50)]
        [DefaultValue(null)]
        public string SerNumber { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Price with tax")]
        public decimal IncomingPrice { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Outgoing price")]
        public decimal OutgoingPrice { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Discount")]
        public int Discount { get; set; }

        [DataMember]
        [ForeignKey("Product_ID")]
        public virtual Product Product { get; set; }
        public int? Product_ID { get; set; }

        [DataMember]
        [ForeignKey("IncomingTaxGroup_ID")]
        public virtual TaxGroup IncomingTaxGroup { get; set; }
        [DataMember]
        public int? IncomingTaxGroup_ID { get; set; }

        [DataMember]
        [ForeignKey("OutgoingTaxGroup_ID")]
        public virtual TaxGroup OutgoingTaxGroup { get; set; }
        [DataMember]
        public int? OutgoingTaxGroup_ID { get; set; }
    }
}
