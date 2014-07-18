﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace TitleTurtle.Models
{
    public class User
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.None), Required]
        public int UserID { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public virtual ICollection<UserPhoto> UserPhotos { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<PersonalData> PersonalDatas { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }

}