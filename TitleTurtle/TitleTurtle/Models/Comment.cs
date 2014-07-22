using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TitleTurtle.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        [ForeignKey("MainArticle")]
        public int? MainArticleID { get; set; }
        [ForeignKey("Article")]
        public int? ArticleID { get; set; }

        public virtual Article MainArticle { get; set; }
        public virtual Article Article { get; set; }
    }
}