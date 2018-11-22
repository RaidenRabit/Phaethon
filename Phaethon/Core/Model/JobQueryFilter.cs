using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class JobQueryFilter
    {
        public int NumOfRecords { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int JobStatus { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public int DateOption { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
