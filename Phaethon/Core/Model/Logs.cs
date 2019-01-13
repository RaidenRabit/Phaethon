using System;
using System.Collections.Generic;
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
    public class Logs
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public string RequestUrl { get; set; }

        [Required]
        [DataMember]
        public string RequestHeaders { get; set; }

        [Required]
        [DataMember]
        public string RequestBody { get; set; }

        [Required]
        [DataMember]
        public string ResponseBody { get; set; }
        
        [DataMember]
        public string UserToken { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DataMember]
        public DateTime TimeStamp { get; set; }
    }
}
