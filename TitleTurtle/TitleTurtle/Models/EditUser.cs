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
        [Required]
        [Display(Name = "Login")]
        [StringLength(30, ErrorMessage = "Login cannot be longer than 30 characters.")]
        public string Login { get; set; }
        [Required]
        [Display(Name = "First name")]
        [StringLength(30, ErrorMessage = "First name cannot be longer than 30 characters.")]
        public string UserFirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        [StringLength(30, ErrorMessage = "last name cannot be longer than 30 characters.")]
        public string UserLastName { get; set; }
        [Required]
        [Display(Name = "Date of birth")]
        [DataType(DataType.DateTime)]
        public DateTime PersDataDate { get; set; }
        [Display(Name = "Adress")]
        public string PersDataAdress { get; set; }
        [Display(Name = "Other")]
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