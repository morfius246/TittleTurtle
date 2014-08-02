
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
        [Required]
        [Display(Name = "Contact mobile")]
        [StringLength(9, ErrorMessage = "Number cannot be longer or smaller than 9 characters.")]
         [RegularExpression(@"^[0-9]+$", ErrorMessage = "Incorect type of number.")]
        public string ContactMobile { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[\w\.-]*[a-zA-Z0-9_]@[\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]",
            ErrorMessage = "Incorect email.")]
        public string ContactEmail { get; set; }
        [Display(Name = "Web page")]
        [RegularExpression(@"(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?",
            ErrorMessage = "Incorect web page.")]
        public string ContactWebPage { get; set; }
        public virtual User User { get; set; }
    }
}