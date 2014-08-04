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
        [Display(Name = "Лайки")]
        public int RatingLike { get; set; }
        [Display(Name = "Дислайки")]
        public int RatingDislike { get; set; }
        [Display(Name = "Просмотры")]
        public int RatingView { get; set; }
        [Display(Name = "Репосты")]
        public int RatingRepost { get; set; }
        public virtual Article Article { get; set; }
    }
}