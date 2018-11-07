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
    [DataContract]
    public class Invoice
    {
        [Key]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public decimal Transport { get; set; }

        [Required]
        [DataMember]
        public string DocNumber { get; set; }

        [Required]
        [DataMember]
        public DateTime PrescriptionDate { get; set; }

        [Required]
        [DataMember]
        public DateTime ReceptionDate { get; set; }

        [Required]
        [DataMember]
        public DateTime PaymentDate { get; set; }
        
        [DataMember]
        public virtual Representative Sender { get; set; }
        [DataMember]
        public virtual Representative Receiver { get; set; }
        public virtual ICollection<Element> Elements { get; set; }
    }
}
