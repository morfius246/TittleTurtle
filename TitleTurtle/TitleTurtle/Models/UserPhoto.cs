using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class UserPhoto
    {
        [Key]
        public int UserPhotoID { get; set; }
        [ForeignKey("Media")]
        public int MediaID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public int UserPhotoCurrent { get; set; }
        public Media Media { get; set; }
        public User User { get; set; }
    }
}