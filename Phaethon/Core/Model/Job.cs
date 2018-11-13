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
    public class Job
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [DataMember]
        public int ID { get; set; }

        //TODO: add FK to User, once Login is implemented
        [DataMember]
        [DefaultValue(null)]
        public virtual Customer Customer { get; set; }

        [DataMember]
        public JobStatus_enum JobStatus { get; set; }

        [DataMember]
        public string JobName { get; set; }

        [DataMember]
        public DateTime StartedTime { get; set; }

        [DataMember]
        public DateTime FinishedTime { get; set; }

        [DataMember]
        public decimal Cost { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
