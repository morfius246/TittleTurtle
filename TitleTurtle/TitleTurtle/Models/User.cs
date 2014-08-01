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
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Login { get; set; }
        public string FullName
        {
            get { return UserLastName + ", " + UserFirstName; }
        }
        public virtual ICollection<UserPhoto> UserPhotos { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<PersonalData> PersonalDatas { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }

}