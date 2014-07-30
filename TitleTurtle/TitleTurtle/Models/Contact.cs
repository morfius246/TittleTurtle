using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TitleTurtle.Models
{
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string ContactWebPage { get; set; }
        public virtual User User { get; set; }
    }
}