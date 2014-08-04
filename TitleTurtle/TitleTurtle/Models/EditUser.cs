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
        public DateTime PersDataDate { get; set; }
        [Display(Name = "Адрес")]
        public string PersDataAdress { get; set; }
        [Display(Name = "Другое")]
        public string PersDataOther { get; set; }
        public int ContactID { get; set; }
        public Media NewMedia { get; set; }
        public UserPhoto NewUserPhoto { get; set; }

        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string ContactWebPage { get; set; }

        public bool UserIsFollowed { get; set; }
    }
}