using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Item
    {
        [Key]
        public int ID { get; set; }
        public string SerNumber { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<Invoice> Elements { get; set; }
    }
}
