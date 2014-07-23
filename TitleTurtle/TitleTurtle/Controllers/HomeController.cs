using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TitleTurtle.Filters;
using TitleTurtle.Models;
using PagedList;
using System.IO;
namespace TitleTurtle.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        protected HomeContext db = new HomeContext();
        [AllowAnonymous]
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

        [Authorize(Roles="Admin, Author")]
        public ActionResult CreateArticle()
        {
            Main model = new Main();
            model.CategoryList = db.Categories.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateArticle(Main model,Media pic,HttpPostedFileBase uploadImage)
        {
            Media media = new Media();
            MediaInArticle mediainart = new MediaInArticle();
            Article NewArticle = model.NewArticle;
            NewArticle.ArticleStatus = 1; //1 -- active //0 -- not confirmed //2 -- deleted
            NewArticle.UserID = db.Users.First(x => x.UserFirstName == User.Identity.Name).UserID;
            db.Articles.Add(NewArticle);
            Edit NewEdit = new Edit();
            NewEdit.Article = NewArticle;
            NewEdit.ArticleID = model.NewArticle.ArticleID;
            NewEdit.Date = DateTime.Now;
            NewEdit.Type = type.Create;
            if (uploadImage != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // установка массива байтов
                pic.MediaData = imageData;
            }
            db.Edits.Add(NewEdit);
            db.Medias.Add(pic);
            db.Articles.Add(NewArticle);
            mediainart.MediaID = pic.MediaID;
            mediainart.ArticleID = NewArticle.ArticleID;
            db.MediaInArticles.Add(mediainart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult ShowArticle(int id)
        {
            ArticleModel model = new ArticleModel();
            model.currentArticle = db.Articles.SingleOrDefault(x => x.ArticleID == id);
            var t =
                from comment in db.Comments
                join article in db.Articles
                on comment.ArticleID equals article.ArticleID
                where comment.MainArticleID == id
                select comment;
            model.CommentList = t.ToArray();
            return View(model);
        }

        public ActionResult EditArticle(int id)
        {
            Main model = new Main();
            model.CategoryList = db.Categories.ToList();
            model.NewArticle = db.Articles.First(x => x.ArticleID == id);
            Edit NewEdit = new Edit();
            NewEdit.Article = model.NewArticle;
            NewEdit.ArticleID = model.NewArticle.ArticleID;
            NewEdit.Date = DateTime.Now;
            NewEdit.Type = type.Edit;
            db.Edits.Add(NewEdit);
            db.SaveChanges();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditArticle(Main model)
        {
            Article my = db.Articles.First(x => x.ArticleID == model.NewArticle.ArticleID);
            my.ArticleTitle = model.NewArticle.ArticleTitle;
            my.ArticleText = model.NewArticle.ArticleText;
            my.CategoryID = model.NewArticle.CategoryID;
            //my.Edits.Add(new Edit { Edition = DateTime.Now });
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteArticle(int id)
        {
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
        [AllowAnonymous]
        public ActionResult Sort(string sortOrder, string currentFilter, string searchString, int? page)
        {
            Article SortArticle = new Article();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "Title" : "";
            ViewBag.TextSortParm = String.IsNullOrEmpty(sortOrder) ? "Text" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var articles = from s in db.Articles
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(s => s.ArticleTitle.ToUpper().Contains(searchString.ToUpper())
                                       || s.ArticleText.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Title":
                    articles = articles.OrderByDescending(s => s.ArticleTitle);
                    break;
                case "Text":
                    articles = articles.OrderByDescending(s => s.ArticleText);
                    break;
                default:
                    articles = articles.OrderBy(s => s.ArticleTitle);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(articles.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CreateComment(ArticleModel model, string userName)
        {
            Comment newComment = model.NewComment;
            int currentArticleId = model.currentArticle.ArticleID;//отримаэм ід поточ статті
            Article temp = db.Articles.SingleOrDefault(x => x.ArticleID == currentArticleId);//отрим цю статтю за ід
            newComment.Article.Category = temp.Category;//коментар має таку ж каегор як стаття
            newComment.Article.ArticleStatus = 1;
            newComment.ArticleID = temp.ArticleID;
            newComment.MainArticle = temp;
            newComment.MainArticleID = temp.ArticleID;
            newComment.Article.UserID = db.Users.First(x => x.UserFirstName == userName).UserID;
            //newComment.Article.User.UserFirstName = db.Users.First(x => x.UserFirstName == userName).UserFirstName;
            //newComment.UserID = db.Users.First(x => x.UserFirstName == User.Identity.Name).UserID;
            db.Comments.Add(newComment);
            db.SaveChanges();
            return RedirectToAction("ShowArticle/" + temp.ArticleID.ToString());
        }
    }
}
