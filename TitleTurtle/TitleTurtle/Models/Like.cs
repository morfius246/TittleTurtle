using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class Like
    {
        [Key]
        public int LikeID { get; set; }
        [ForeignKey("Article")]
        public int ArticleID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        [Display(Name = "Лайки")]
        public bool Likes { get; set; }
        public virtual ICollection<Article> Article { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}