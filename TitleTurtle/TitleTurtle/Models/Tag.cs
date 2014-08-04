using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TitleTurtle.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        [Display(Name = "Имя")]
        public string TagName { get; set; }
        public virtual ICollection<TagInArticle> TagInArticles { get; set; }
    }
}
