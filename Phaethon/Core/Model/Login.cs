using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Model
{
    [Serializable]
    [DataContract]
    public class Login
    {
        [Key]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        [DisplayName("Username")]
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "min 3, max 50 letters")]
        [Index(IsUnique = true)]
        public string Username { get; set; }

        [DataMember]
        [DataType(DataType.Password)]
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "min 5, max 50 letters")]
        public string Password { get; set; }

        public byte[] Salt { get; set; }
    }
}
