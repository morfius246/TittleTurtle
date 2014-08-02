using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class FeedbackModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Сообщение")]
        public string MessageText { get; set; }
    }
}