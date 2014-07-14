using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class Categorie
    {
        [Key]
        public int CategorieID { get; set; }
        public string CategorieName { get; set; }
        public string CategorieDescription { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}