using System.ComponentModel.DataAnnotations;

namespace TitleTurtle.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "String cannot be longer than 50 characters.")]
        public string CategoryName { get; set; }
        [Required]
        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "String cannot be longer than 500 characters.")]
        public string CategoryDescription { get; set; }
    }
}