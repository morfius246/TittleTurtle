using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public enum type { Create, Edit, Deleted }

    public class Edit
    {
        [Key]
        public int EditID { get; set; }
        [ForeignKey("Article")]
        public int ArticleID { get; set; }
        public DateTime Date { get; set; }
        public type Type { get; set; }
        
        public Article Article { get; set; }
    }
}