using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TitleTurtle.Models
{
    public class PersonalData
    {
        [Key]
        public int PersonalDataID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        [Required]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PersDataDate { get; set; }
        [Display(Name = "Adress")]
        [StringLength(300, ErrorMessage = "String cannot be longer than 300 characters.")]
        public string PersDataAdress { get; set; }
        [Display(Name = "Other")]
        [StringLength(500, ErrorMessage = "String cannot be longer than 500 characters.")]
        public string PersDataOther { get; set; }
        public virtual User User { get; set; }
    }
}