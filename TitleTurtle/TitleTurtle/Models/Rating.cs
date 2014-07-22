using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TitleTurtle.Models
{
    public class Rating
    {
        [Key]
        public int RatingID { get; set; }
        [ForeignKey("Article")]
        public int ArticleID { get; set; }
        public int RatingLike { get; set; }
        public int RatingDislike { get; set; }
        public int RatingView { get; set; }
        public int RatingRepost { get; set; }
        public virtual Article Article { get; set; }
    }
}