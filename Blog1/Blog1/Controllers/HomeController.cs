using Blog1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog1.Controllers
{
    public class HomeController : Controller
    {
        HomeContext db = new HomeContext();

        public ActionResult Index()
        {
            //PersonalData p = new PersonalData { PersDataAdress = "Franka", PersDataDate = DateTime.Now, PersDataOther = "wa", User = new User {  } };
            //db.PersonalDatas.Add(p);
            //db.Users.Add(new User { UserFirstName = "Alex", UserLastName = "Ivanov"});
            Article art = new Article { ArticleTitle = "New art", ArticleText = "MOst", ArticleStatus = 1, UserID = 1 };
            //db.Articles.Add(art);
            db.Tags.Add(new Tag { TagName = "ddd" });
            db.SaveChanges();
            return View(db.Articles.Find(art));
        }

        public ActionResult CreateArticle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateArticle(Article model)
        {
            db.Articles.Add(model);
            db.SaveChanges();
            return View();
        }

    }
}
