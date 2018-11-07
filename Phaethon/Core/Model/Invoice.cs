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
    [DataContract(Name = "Invoice")]
    public class Invoice
    {
        [Key]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Transport")]
        public decimal Transport { get; set; }

        [DataMember(Name = "DocNumber")]
        public string DocNumber { get; set; }

        [DataMember(Name = "PrescriptionDate")]
        public DateTime PrescriptionDate { get; set; }

        [DataMember(Name = "ReceptionDate")]
        public DateTime ReceptionDate { get; set; }

        [DataMember(Name = "PaymentDate")]
        public DateTime PaymentDate { get; set; }


        [DataMember(Name = "Sender")]
        public virtual Representative Sender { get; set; }

        [DataMember(Name = "Receiver")]
        public virtual Representative Receiver { get; set; }
        public virtual ICollection<Element> Elements { get; set; }
    }
}
