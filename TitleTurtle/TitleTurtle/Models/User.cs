using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TitleTurtle.Models
{
    public class User
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.None), Required]
        public int UserID { get; set; }
        [Required]
        [Display(Name = "Имя")]
        [StringLength(30, ErrorMessage = "Имя пользователя не может быть длиннее 30 символов.")]
        public string UserFirstName { get; set; }
        [Required]
        [Display(Name = "Фамилия")]
        [StringLength(30, ErrorMessage = "Фамилия пользователя не может быть длиннее 30 символов.")]
        public string UserLastName { get; set; }
        [Required]
        [Display(Name = "Логин")]
        [StringLength(15, ErrorMessage = "Логин пользователя не может быть длиннее 30 символов.", MinimumLength = 5)]
        public string UserName { get; set; }
        public string FullName
        {
            get { return UserFirstName + " " + UserLastName; }
        }
        public virtual ICollection<UserPhoto> UserPhotos { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<PersonalData> PersonalDatas { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }

}