
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
        public string ContactMobile { get; set; }
        [Required]
        [Display(Name = "User Email")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
                        ErrorMessage = "Неверный формат электронной почты")]
        public string ContactEmail { get; set; }
        public string ContactWebPage { get; set; }
        public virtual User User { get; set; }
    }
}