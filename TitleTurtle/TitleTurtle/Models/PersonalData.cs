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
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PersDataDate { get; set; }
        [Display(Name = "Адрес")]
        [StringLength(100, ErrorMessage = "Адрес пользователя не может быть длиннее 100 символов.")]
        public string PersDataAdress { get; set; }
        [Display(Name = "Другое")]
        [StringLength(500, ErrorMessage = "Ета строка не может быть длиннее 500 символов.")]
        public string PersDataOther { get; set; }
        public virtual User User { get; set; }
    }
}