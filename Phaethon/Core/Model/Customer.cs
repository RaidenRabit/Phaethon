using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Model
{
    [Serializable]
    [DataContract]
    public class Customer :IEquatable<Customer>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        [DefaultValue(null)]
        public virtual Address Address { get; set; }

        [DataMember]
        public string GivenName { get; set; }

        [DataMember]
        public string FamilyName { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Email { get; set; }

        public bool Equals(Customer other)
        {
            if (other == null)
                return false;
            return this.ID.Equals(other.ID) &&
                   this.Address.ID.Equals(other.Address.ID) &&
                   this.Email.Equals(other.Email) &&
                   this.FamilyName.Equals(other.FamilyName) &&
                   this.GivenName.Equals(other.GivenName) &&
                   this.Phone.Equals(other.Phone);
        }
    }
}
