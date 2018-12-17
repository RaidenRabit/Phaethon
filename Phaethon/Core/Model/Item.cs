using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

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
        [StringLength(50)]
        [DefaultValue(null)]
        public string SerNumber { get; set; }

        [Required]
        [DataMember]
        public decimal Price { get; set; }

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

        [DataMember]
        public virtual ICollection<Element> Elements { get; set; }

        //extra
        [NotMapped]
        [DataMember]
        public int Quantity { get; set; }

        [NotMapped]
        [DataMember]
        public bool Delete { get; set; }
    }
}
