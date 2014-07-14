using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class Edit
    {
        [Key]
        public int EditID { get; set; }
        [ForeignKey("Article")]
        public int ArticleID { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Edition { get; set; }
        public DateTime Delition { get; set; }
        public Article Article { get; set; }
    }
}