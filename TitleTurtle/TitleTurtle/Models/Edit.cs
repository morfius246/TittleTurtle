using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TitleTurtle.Models
{
    public enum type { Create, Edit, Deleted }

    public class Edit
    {
        [Key]
        public int EditID { get; set; }
        [ForeignKey("Article")]
        public int ArticleID { get; set; }
        [Display(Name = "Дата")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public type Type { get; set; }
        public Article Article { get; set; }
    }
}