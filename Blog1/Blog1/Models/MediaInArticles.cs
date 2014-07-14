using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class MediaInArticle
    {
        [Key]
        public int MediaInArticleID { get; set; }
        [ForeignKey("Article")]
        public int ArticleID { get; set; }
        [ForeignKey("Media")]
        public int MediaID { get; set; }
        public virtual Article Article { get; set; }
        public virtual Media Media { get; set; }
    }
}