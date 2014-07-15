using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;



namespace Blog1.Models
{

    public class Article
    {

        [Key]
        public int ArticleID { get; set; }
        public int UserID { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleText { get; set; }
        public int ArticleStatus { get; set; }

        public virtual Category Category{get;set;}
        public virtual User User { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Edit> Edits { get; set; }
        public virtual ICollection<MediaInArticle> MedialInArticles { get; set; }
        public virtual ICollection<TagInArticle> TagInArticles { get; set; }

    }
}

