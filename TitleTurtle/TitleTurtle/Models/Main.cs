using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class Main
    {
        public IEnumerable<Article> ArticleList { get; set;}
        public IEnumerable<Category> CategoryList { get; set; }
        public Category NewCategory { get; set; }
        public Article NewArticle { get; set; }
    }
}