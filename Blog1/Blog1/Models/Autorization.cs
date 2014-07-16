using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class Autorization
    {
        [Key]
        public int AutorizationID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public User User { get; set; }
    }
}