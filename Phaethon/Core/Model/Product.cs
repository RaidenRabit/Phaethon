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
    public class Product
    {
        [Key]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Product name")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Barcode")]
        [Index(IsUnique = true)]
        public int Barcode { get; set; }

        [DataMember]
        [ForeignKey("ProductGroup_ID")]
        public virtual ProductGroup ProductGroup { get; set; }
        public int? ProductGroup_ID { get; set; }

        [DataMember]
        public virtual ICollection<Item> Items { get; set; }
    }
}
