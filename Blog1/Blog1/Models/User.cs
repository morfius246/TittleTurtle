using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Blog1.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public virtual ICollection<UserPhoto> UserPhotos { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<PersonalData> PersonalData { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }

}