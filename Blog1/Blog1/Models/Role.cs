using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class Role
    {
        [Key]
        public string RolesID { get; set; }
        public string RoleName { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
