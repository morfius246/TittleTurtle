using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class EditUser
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public DateTime PersDataDate { get; set; }
        public string PersDataAdress { get; set; }
        public string PersDataOther { get; set; }
        public int ContactID { get; set; }
        public Media NewMedia { get; set; }
        public UserPhoto NewUserPhoto { get; set; }

        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string ContactWebPage { get; set; }

    }
}