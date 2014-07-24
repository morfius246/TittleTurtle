using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class ShowArticle
    {
        public Article Article { get; set; }
        public Comment NewComment { get; set; }
        public IEnumerable<Comment> CommentList { get; set; }
        public Article CurrentArticle { get; set; }
        public int CurrentArticleId { get; set; }
    }
}