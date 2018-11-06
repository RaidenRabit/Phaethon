using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Invoice
    {
        [Key]
        public int ID { get; set; }
        public decimal Transport { get; set; }
        public string DocNumber { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public DateTime ReceptionDate { get; set; }
        public DateTime PaymentDate { get; set; }
        
        public virtual Representative Sender { get; set; }
        public virtual Representative Receiver { get; set; }
        public virtual ICollection<Element> Elements { get; set; }
    }
}
