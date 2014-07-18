using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class TagInArticle
    {
        [Key]
        public int TagInArticleID { get; set; }
        [ForeignKey("Article")]
        public int ArticleID { get; set; }
        [ForeignKey("Tag")]
        public int TagID { get; set; }
        public virtual Article Article { get; set; }
        public virtual Tag Tag { get; set; }
    }
}