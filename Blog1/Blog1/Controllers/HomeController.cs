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
<<<<<<< HEAD
            List<Article> list = new List<Article>();
            return View(list);
=======
            return View();
>>>>>>> origin/master
        }

        public ActionResult CreateArticle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateArticle(Article model)
        {
            model.ArticleStatus = 1;
            model.CategoryID = 1;
            model.UserID = 1;
            db.Articles.Add(model);
            db.SaveChanges();
            return View();
        }

    }
}
