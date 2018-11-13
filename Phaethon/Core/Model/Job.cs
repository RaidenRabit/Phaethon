using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Model
{
    [Serializable]
    [DataContract]
    public class Job : IEquatable<Job>
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

        public bool Equals(Job other)
        {
            if (other == null)
                return false;
            return this.Customer.ID.Equals(other.Customer.ID) &&
                   this.ID.Equals(other.ID) &&
                   this.JobName.Equals(other.JobName) &&
                   this.Cost.Equals(other.Cost) &&
                   this.Description.Equals(other.Description) &&
                   this.JobStatus.Equals(other.JobStatus);
        }
    }
}
