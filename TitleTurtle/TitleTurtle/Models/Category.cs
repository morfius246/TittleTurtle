using System.ComponentModel.DataAnnotations;

namespace TitleTurtle.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        [Display(Name = "Имя")]
        [StringLength(50, ErrorMessage = "Строка не может быть больше, чем 50 символов.")]
        public string CategoryName { get; set; }
        [Display(Name = "Описание")]
        [StringLength(500, ErrorMessage = "Строка не может быть больше, чем 500 символов.")]
        public string CategoryDescription { get; set; }
    }
}