using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Representative
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        
        public virtual Company Company { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
