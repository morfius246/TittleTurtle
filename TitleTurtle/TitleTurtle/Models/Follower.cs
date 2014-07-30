using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class Follower
    {
        [Key]
        public int FollowerID { get; set; }
        [ForeignKey("Follow")]
        public int? FollowID { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }
        public virtual User Follow { get; set; }
        public virtual User User { get; set; }
    }
}