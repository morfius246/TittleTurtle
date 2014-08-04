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
        [Display(Name = "Название")]
        [StringLength(50, ErrorMessage = "Строка не может быть больше, чем 50 символов.")]
        public string ArticleTitle { get; set; }
        [Required]
        [Display(Name = "Текст статьи")]
        [StringLength(500000, ErrorMessage = "Строка не может быть больше, чем 500000 символов.")]
        public string ArticleText { get; set; }
        [Display(Name = "Статус")]
        public int ArticleStatus { get; set; }
        [Display(Name = "Количество коментариев")]
        public int CommentCount { get; set; }
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Edit> Edits { get; set; }
        public virtual ICollection<MediaInArticle> MedialInArticles { get; set; }
        public virtual ICollection<TagInArticle> TagInArticles { get; set; }

    }
}

