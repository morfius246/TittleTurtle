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
        [Display(Name = "First name")]
        [StringLength(30, ErrorMessage = "First name cannot be longer than 30 characters.")]
        public string UserFirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        [StringLength(30, ErrorMessage = "Last name cannot be longer than 30 characters.")]
        public string UserLastName { get; set; }
        [Required]
        [Display(Name = "User name")]
        [StringLength(15, ErrorMessage = "User name cannot be longer than 15 characters.", MinimumLength = 5)]
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