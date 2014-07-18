﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitleTurtle.Models
{
    public class Media
    {
        [Key]
        public int MediaID { get; set; }
        public byte[] MediaData { get; set; }
        public ICollection<MediaInArticle> MediaInArticles { get; set; }
        public ICollection<UserPhoto> UserPhotos { get; set; }
    }
}