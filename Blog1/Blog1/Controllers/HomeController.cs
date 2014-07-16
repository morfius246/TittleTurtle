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
            Main model = new Main();
            model.CategoryList = db.Categories.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateArticle(Main model)
        {
            Article NewArticle = model.NewArticle;
            NewArticle.ArticleStatus = 1;
            NewArticle.UserID = 1;
            db.Articles.Add(NewArticle);
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
            Main model = new Main();
            model.CategoryList = db.Categories.ToList();
            model.NewArticle = db.Articles.First(x => x.ArticleID == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Main model)
        {
            Article my = db.Articles.First(x => x.ArticleID == model.NewArticle.ArticleID);
            my.ArticleTitle = model.NewArticle.ArticleTitle;
            my.ArticleText = model.NewArticle.ArticleText;
            my.CategoryID = model.NewArticle.CategoryID;
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
        [HttpPost]
        public ActionResult CreateCategory(Main model)
        {
            Category NewCategory = new Category();
            NewCategory = model.NewCategory;
            db.Categories.Add(NewCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
