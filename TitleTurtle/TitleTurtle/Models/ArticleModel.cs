using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class ArticleModel
    {
        public Comment NewComment { get; set; }
        public IEnumerable<Comment> CommentList { get; set; }
        public Article currentArticle { get; set; }
        public int currentArticleId { get; set; }
    }
}