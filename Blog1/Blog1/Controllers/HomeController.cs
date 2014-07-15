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

        public ActionResult Index(int? categoryId)
        {
            Main model = new Main();
            if (categoryId == null)
            {
                model.ArticleList = db.Articles.ToList();
            }
            else
            {
                model.ArticleList = db.Articles.Where(x => x.CategoryID == categoryId).ToList();     
            }
            model.CategoryList = db.Categories.ToList();
            return View(model);
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
        }
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(Category model)
        {
            db.Categories.Add(model);
            db.SaveChanges();
            return View("Index");
        }

    }
}
