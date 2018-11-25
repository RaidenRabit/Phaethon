using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Model
{
    [Serializable]
    [DataContract]
    public class Address : IEquatable<Address>
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string Number { get; set; }
        
        /// <summary>
        /// Ex: apartment, room, etc.
        /// </summary>
        [DataMember]
        public string Extra { get; set; }

        public bool Equals(Address other)
        {
            if (other == null)
                return false;
            return this.ID.Equals(other.ID) &&
                   this.City.Equals(other.City) &&
                   this.Street.Equals(other.Street) &&
                   this.Number.Equals(other.Number) &&
                   this.Extra.Equals(other.Extra);
        }
    }
}
