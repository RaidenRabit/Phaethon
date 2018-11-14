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
    public class Invoice
    {
        [Key]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Transport cost")]
        public decimal Transport { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Document number")]
        public string DocNumber { get; set; }

        [DataType(DataType.DateTime), Required]
        [DataMember]
        [DisplayName("Prescription date")]
        public DateTime PrescriptionDate { get; set; }

        [DataType(DataType.DateTime), Required]
        [DataMember]
        [DisplayName("Reception date")]
        public DateTime ReceptionDate { get; set; }

        [DataType(DataType.DateTime), Required]
        [DataMember]
        [DisplayName("Payment date")]
        public DateTime PaymentDate { get; set; }



        [DataMember]
        [ForeignKey("Sender_ID")]
        public virtual Representative Sender { get; set; }
        public int? Sender_ID { get; set; }

        [DataMember]
        [ForeignKey("Receiver_ID")]
        public virtual Representative Receiver { get; set; }
        public int? Receiver_ID { get; set; }

        [DataMember]
        public virtual ICollection<Element> Elements { get; set; }
    }
}
