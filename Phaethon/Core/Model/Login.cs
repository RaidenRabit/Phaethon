using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [DisplayName("User Name")]
        [Required (ErrorMessage = "Please enter User Name")]
        public string Username { get; set; }

        [DataMember]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; }

        public string PasswordSalt { get; set; }

    }
}
