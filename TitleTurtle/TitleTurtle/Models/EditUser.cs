using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class EditUser
    {
        public int UserID { get; set; }
        [Display(Name = "Логин")]
        [StringLength(30, ErrorMessage = "Логин пользователя не может быть длиннее 30 символов.")]
        public string Login { get; set; }
        [Required]
        [Display(Name = "Имя")]
        [StringLength(30, ErrorMessage = "Имя пользователя не может быть длиннее 30 символов.")]
        public string UserFirstName { get; set; }
        [Required]
        [Display(Name = "Фамилия")]
        [StringLength(30, ErrorMessage = "Фамилия пользователя не может быть длиннее 30 символов.")]
        public string UserLastName { get; set; }
        [Display(Name = "Дата рождения")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PersDataDate { get; set; }
        [Display(Name = "Адрес")]
        [StringLength(100, ErrorMessage = "Адрес пользователя не может быть длиннее 100 символов.")]
        public string PersDataAdress { get; set; }
        [Display(Name = "Другое")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Ета строка не может быть длиннее 500 символов.")]
        public string PersDataOther { get; set; }
        [Display(Name = "Контактный номер")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Номер не может включать в себя другие символы кроме цифр!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Номер должен включать в себя 10 цифр.")]
        public string ContactMobile { get; set; }
        [Required]
        [Display(Name = "E-mail")]
        [RegularExpression(@"[\w\.-]*[a-zA-Z0-9_]@[\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]",
            ErrorMessage = "Неверный адрес электронной почты.")]
        public string ContactEmail { get; set; }
        [Display(Name = "Веб-страницы")]
        [RegularExpression(@"(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?",
            ErrorMessage = "Неверный адрес веб-страницы.")]
        public string ContactWebPage { get; set; }
        public int ContactID { get; set; }
        public Media NewMedia { get; set; }
        public UserPhoto NewUserPhoto { get; set; }
        public bool UserIsFollowed { get; set; }
    }
}