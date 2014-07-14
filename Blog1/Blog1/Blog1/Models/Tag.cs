using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        public string TagName { get; set; }
        public virtual ICollection<TagInArticle> TagInArticles { get; set; }
    }
}
