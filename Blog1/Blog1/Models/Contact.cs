using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public int ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string ContactWebPage { get; set; }
        public virtual User User { get; set; }
    }
}