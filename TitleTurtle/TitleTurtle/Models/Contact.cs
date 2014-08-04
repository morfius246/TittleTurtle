
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace TitleTurtle.Models
{
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        [Display(Name = "Контактный номер")]
        [StringLength(9, ErrorMessage = "Номер не может быть больше, чем 50 символов.")]
        public string ContactMobile { get; set; }
        [Required]
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[\w\.-]*[a-zA-Z0-9_]@[\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]",
            ErrorMessage = "Неверный адрес электронной почты.")]
        public string ContactEmail { get; set; }
        [Display(Name = "Веб-страницы")]
        [RegularExpression(@"(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?",
            ErrorMessage = "Неверный адрес веб-страницы.")]
        public string ContactWebPage { get; set; }
        public virtual User User { get; set; }
    }
}