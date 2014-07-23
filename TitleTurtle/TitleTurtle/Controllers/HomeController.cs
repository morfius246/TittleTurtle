﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TitleTurtle.Filters;
using TitleTurtle.Models;
using PagedList;
using System.IO;
namespace TitleTurtle.Controllers
{
    /// <summary>
    /// Main controller with Actions for Articles and Managemant
    /// </summary>
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        protected HomeContext Db = new HomeContext();
        
        /// <summary>
        /// Opens main page 'Index' with list of articles from current category (or from ALL categories)
        /// </summary>
        /// <param name="categoryId">ID of category to show Articles from</param>
        /// <returns>View 'Index' with Articles in Main model</returns>
        [AllowAnonymous]
        public ActionResult Index(int? categoryId)
        {
            var model = new Main
            {
                ArticleList =
                    categoryId == null
                        ? Db.Articles.ToList()
                        : Db.Articles.Where(x => x.CategoryID == categoryId).ToList(),
                CategoryList = Db.Categories.ToList()
            };
            return View(model);
        }

        /// <summary>
        /// Redirect to CreateAction page
        /// </summary>
        /// <returns>View with Main model to create new Article</returns>
        [Authorize(Roles="Admin, Author")]
        public ActionResult CreateArticle()
        {
            var model = new Main {CategoryList = Db.Categories.ToList()};
            return View(model);
        }

        /// <summary>
        /// Create new article
        /// </summary>
        /// <param name="model">Main model</param>
        /// <param name="pic">Media pic to Article</param>
        /// <param name="uploadImage"></param>
        /// <returns>Redirect to 'Index'</returns>
        [HttpPost]
        public ActionResult CreateArticle(Main model,Media pic,HttpPostedFileBase uploadImage)
        {
            var mediainart = new MediaInArticle();
            var newArticle = model.NewArticle;
            newArticle.ArticleStatus = 1; //1 -- active //0 -- not confirmed //2 -- deleted
            newArticle.UserID = Db.Users.First(x => x.UserFirstName == User.Identity.Name).UserID;
            Db.Articles.Add(newArticle);
            var newEdit = new Edit
            {
                Article = newArticle,
                ArticleID = model.NewArticle.ArticleID,
                Date = DateTime.Now,
                Type = type.Create
            };
            var newRating = new Rating
            {
                RatingID = 1,
                Article = newArticle,
                ArticleID = model.NewArticle.ArticleID,
                RatingDislike = 0,
                RatingLike = 0,
                RatingRepost = 0,
                RatingView = 0
            };
            if (uploadImage != null)
            {
                // Read the uploaded file into a byte array
                byte[] imageData;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // Set byte array
                pic.MediaData = imageData;
            }
            Db.Edits.Add(newEdit);
            Db.Ratings.Add(newRating);
            Db.Medias.Add(pic);
            Db.Articles.Add(newArticle);
            mediainart.MediaID = pic.MediaID;
            mediainart.ArticleID = newArticle.ArticleID;
            Db.MediaInArticles.Add(mediainart);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Open Article with ID
        /// </summary>
        /// <param name="id">ID of article to open</param>
        /// <returns>View with model of article</returns>
        [AllowAnonymous]
        public ActionResult ShowArticle(int id)
        {
            var model = Db.Articles.First(x => x.ArticleID == id);
            
            return View(model);
        }

        /// <summary>
        /// Open article with ID to edit
        /// </summary>
        /// <param name="id">ID of Article to edit</param>
        /// <returns>Main model with article and edit objects</returns>
        public ActionResult EditArticle(int id)
        {
            var model = new Main
            {
                CategoryList = Db.Categories.ToList(),
                NewArticle = Db.Articles.First(x => x.ArticleID == id)
            };
            var newEdit = new Edit
            {
                Article = model.NewArticle,
                ArticleID = model.NewArticle.ArticleID,
                Date = DateTime.Now,
                Type = type.Edit
            };
            Db.Edits.Add(newEdit);
            Db.SaveChanges();
            return View(model);
        }

        /// <summary>
        /// Edits current article
        /// </summary>
        /// <param name="model">Main model</param>
        /// <returns>View 'Index'</returns>
        [HttpPost]
        public ActionResult EditArticle(Main model)
        {
            var my = Db.Articles.First(x => x.ArticleID == model.NewArticle.ArticleID);
            my.ArticleTitle = model.NewArticle.ArticleTitle;
            my.ArticleText = model.NewArticle.ArticleText;
            my.CategoryID = model.NewArticle.CategoryID;
            //my.Edits.Add(new Edit { Edition = DateTime.Now });
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delete article with id
        /// </summary>
        /// <param name="id">Id of article to delete</param>
        /// <returns>View 'Index'</returns>
        public ActionResult DeleteArticle(int id)
        {
            Db.Articles.Remove(Db.Articles.First(x => x.ArticleID == id));

            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Creates new category
        /// </summary>
        /// <param name="model">Main model</param>
        /// <returns>View Index</returns>
        [HttpPost]
        public ActionResult CreateCategory(Main model)
        {
            var newCategory = model.NewCategory;
            Db.Categories.Add(newCategory);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <returns>View with result</returns>
        [AllowAnonymous]
        public ActionResult Sort(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //var sortArticle = new Article();
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
            var articles = from s in Db.Articles
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
            const int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(articles.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Vote(int _id, bool up)
        {
            if (up)
                ++Db.Articles.First(x => x.ArticleID == _id).Ratings.First(x => x.RatingID == 1).RatingLike;
            else
                ++Db.Articles.First(x => x.ArticleID == _id).Ratings.First(x => x.RatingID == 1).RatingDislike;
            Db.SaveChanges();
            return RedirectToAction("ShowArticle", new {id = _id});
        }
    }
}
