using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TitleTurtle.Models
{

    public class Article
    {

        [Key]
        public int ArticleID { get; set; }
        public int UserID { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        [Required]
        [Display(Name = "Title")]
        [StringLength(50,ErrorMessage = "String cannot be longer than 50 characters.")]
        public string ArticleTitle { get; set; }
        [Required]
        [Display(Name = "Text")]
        [StringLength(50, ErrorMessage = "String cannot be longer than 50 characters.")]
        public string ArticleText { get; set; }
        [Required]
        [Display(Name = "Status")]
        public int ArticleStatus { get; set; }
        [Display(Name = "Quantity of comments")]
        public int CommentCount { get; set; }
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Edit> Edits { get; set; }
        public virtual ICollection<MediaInArticle> MedialInArticles { get; set; }
        public virtual ICollection<TagInArticle> TagInArticles { get; set; }

    }
}

