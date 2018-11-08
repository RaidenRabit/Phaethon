using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Element
    {
        [Key, ForeignKey("Invoice"), Column(Order = 0)]
        public int Invoice_ID { get; set; }
        [Key, ForeignKey("Item"), Column(Order = 1)]
        public int Item_ID { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Item Item { get; set; }
    }
}
