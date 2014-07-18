using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class PersonalData
    {
        [Key]
        public int PersonalDataID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public DateTime PersDataDate { get; set; }
        public string PersDataAdress { get; set; }
        public string PersDataOther { get; set; }
        public virtual User User { get; set; }
    }
}