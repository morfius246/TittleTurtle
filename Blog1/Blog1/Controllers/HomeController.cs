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
            return View(db.Articles.ToList());
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
<<<<<<< HEAD
            return View();
=======
            return RedirectToAction("Index");
        }
        public ActionResult Show(int id)
        {
            Article model = new Article();
            model = db.Articles.First(x => x.ArticleID == id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Article model = new Article();
            model = db.Articles.First(x => x.ArticleID == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Article model)
        {
            Article my = db.Articles.First(x => x.ArticleID == model.ArticleID);
            my.ArticleTitle = model.ArticleTitle;
            my.ArticleText = model.ArticleText;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Article model = new Article();
            db.Articles.Remove(db.Articles.First(x => x.ArticleID == id));
            db.SaveChanges();
            return RedirectToAction("Index");
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> parent of 19c040e... Add category
=======
>>>>>>> parent of 19c040e... Add category
=======
>>>>>>> parent of 19c040e... Add category
        }

    }
}
