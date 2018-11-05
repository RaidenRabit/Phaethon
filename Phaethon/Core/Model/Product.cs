using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ProductGroup ProductGroup { get; set; }
    }
}
