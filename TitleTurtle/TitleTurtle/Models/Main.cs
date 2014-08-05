using PagedList;
using System.Collections.Generic;

namespace TitleTurtle.Models
{
    public class Main
    {
        public IEnumerable<Article> ArticleList { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }
        public Category NewCategory { get; set; }
        public Article NewArticle { get; set; }
        public Media NewMedia { get; set; }
        public MediaInArticle MediaInArticles { get; set; }
        public PagedList<Article> PagedList { get; set; }
    }

}