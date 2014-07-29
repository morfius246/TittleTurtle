using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TitleTurtle.Filters;
using TitleTurtle.Models;
using PagedList;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.Security;
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
        public ActionResult Index(Main model, int? categoryId, string sort)
        {
            model = new Main
            {
                ArticleList =
                    categoryId == null
                        ? (from article in Db.Articles
                           where !(from comment in Db.Comments
                                   select comment.ArticleID).Contains(article.ArticleID)
                           select article).ToList().OrderByDescending(a => (sort == "rating" ? (a.Ratings.ElementAt(0).RatingLike - a.Ratings.ElementAt(0).RatingDislike) : (object)a.Edits.ElementAt(0).Date))
                        : (from article in Db.Articles
                           where article.CategoryID == categoryId && !(from comment in Db.Comments
                                                                       select comment.ArticleID).Contains(article.ArticleID)
                           select article).ToList().OrderByDescending(a => (sort == "rating" ? (a.Ratings.ElementAt(0).RatingLike - a.Ratings.ElementAt(0).RatingDislike) : (object)a.Edits.ElementAt(0).Date)),
                CategoryList = Db.Categories.ToList()
            };
            if (categoryId != null)
            {
                ViewBag.CategoryID = categoryId;
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult SortByUserDesire(string sort)
        {
            var model = (Main)TempData["TempModel"];
            if (sort == "rating")
            {
                model.ArticleList.OrderBy(a => a.Ratings.ElementAt(0).RatingLike - a.Ratings.ElementAt(0).RatingDislike);
            }
            if (sort == "date")
            {
                model.ArticleList.OrderBy(a => a.Edits.ElementAt(0).Date);
            }
            return View("Index", model);
        }
        /// <summary>
        /// Redirect to CreateAction page
        /// </summary>
        /// <returns>View with Main model to create new Article</returns>
        [Authorize(Roles = "Admin, Author")]
        public ActionResult CreateArticle()
        {
            var model = new Main { CategoryList = Db.Categories.ToList() };
            return View(model);
        }

        [Authorize(Roles = "Admin, Author")]
        public ActionResult MyArticles(int? categoryId)
        {
            var model = new Main
            {
                ArticleList =
                    categoryId == null
                        ? (from article in Db.Articles
                           where !(from comment in Db.Comments
                                   select comment.ArticleID).Contains(article.ArticleID) && (article.User.UserFirstName == User.Identity.Name)
                           select article).ToList()
                        : (from article in Db.Articles
                           where (article.User.UserFirstName == User.Identity.Name) && (article.CategoryID == categoryId) && !(from comment in Db.Comments
                                                                                                                               select comment.ArticleID).Contains(article.ArticleID)
                           select article).ToList(),
                CategoryList = Db.Categories.ToList()
            };
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
        [ValidateInput(false)]
        public ActionResult CreateArticle(Main model, Media pic, HttpPostedFileBase uploadImage)
        {
            var mediainart = new MediaInArticle();
            model.CategoryList = Db.Categories.ToList();
            if (model.NewArticle.ArticleTitle == null)
            {
                model.NewArticle.ArticleTitle = "Без названия";
            }
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
            try
            {
                if (uploadImage != null && uploadImage.ContentType == "image/jpeg" || uploadImage.ContentType == "image/jpg" || uploadImage.ContentType == "image/gif" || uploadImage.ContentType == "image/png" || uploadImage.ContentType == "image/bmp" || uploadImage.ContentType == "image/ico")
                {
                    if(uploadImage.ContentLength <=100000)
                    { 
                    // Read the uploaded file into a byte array
                    byte[] imageData;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    // Set byte array
                    pic.MediaData = imageData;
                    Db.Medias.Add(pic);
                    mediainart.MediaID = pic.MediaID;
                    Db.MediaInArticles.Add(mediainart);
                    ViewBag.Error = "";
                    }
                    else
                    {
                        ViewBag.Error = "Недопустимый размер файла";
                        return View(model);

                    }

                }
            }
            catch
            {

            }
       
            Db.Edits.Add(newEdit);
            Db.Ratings.Add(newRating);   
            Db.Articles.Add(newArticle); 
            mediainart.ArticleID = newArticle.ArticleID;
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

       

        public ActionResult AddPictureInEdit()
        {
            return View();
        }

        /// <summary>
        /// Open Article with ID
        /// </summary>
        /// <param name="id">ID of article to open</param>
        /// <returns>View with model of article</returns>
        [AllowAnonymous]
        public ActionResult ShowArticle(int? id)
        {
            ShowArticle model = new ShowArticle();
            model.Article = Db.Articles.SingleOrDefault(x => x.ArticleID == id);
            model.CurrentArticle = Db.Articles.SingleOrDefault(x => x.ArticleID == id);

            var t =
                from comment in Db.Comments
                join article in Db.Articles
                on comment.ArticleID equals article.ArticleID
                where comment.MainArticleID == id
                select comment;

            model.CommentList = t.ToArray();

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
        [ValidateInput(false)]
        public ActionResult EditArticle(Main model, int? id, Media pic, HttpPostedFileBase uploadImage)
        {
            var newArticle = model.NewArticle;
            model.CategoryList = Db.Categories.ToList();
            var mediainart = new MediaInArticle();
            Media media = new Media();
            Article my = Db.Articles.First(x => x.ArticleID == model.NewArticle.ArticleID);
            my.ArticleTitle = model.NewArticle.ArticleTitle;
            my.ArticleText = model.NewArticle.ArticleText;
            my.CategoryID = model.NewArticle.CategoryID;
            //my.Edits.Add(new Edit { Edition = DateTime.Now });
            try
            {

                if (uploadImage != null && uploadImage.ContentType == "image/jpeg" || uploadImage.ContentType == "image/jpg" || uploadImage.ContentType == "image/gif" || uploadImage.ContentType == "image/png" || uploadImage.ContentType == "image/bmp" || uploadImage.ContentType == "image/ico")
                {
                    if(uploadImage.ContentLength <=100000)
                    { 
                    // Read the uploaded file into a byte array
                    byte[] imageData;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    // Set byte array
                    pic.MediaData = imageData;
                    Db.Medias.Add(pic);
                    mediainart.MediaID = pic.MediaID;
                    Db.MediaInArticles.Add(mediainart);
                    }
                    
                }
            }
            catch
            {

            }
            mediainart.ArticleID = newArticle.ArticleID;
            Db.SaveChanges();
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Delete article with id
        /// </summary>
        /// <param name="id">Id of article to delete</param>
        /// <returns>View 'Index'</returns>

        public ActionResult DeletePicFromArticle(Main model, int? MediaId, int? ArticleId)
        {
            Media media = new Media();
            media = Db.Medias.FirstOrDefault(x => x.MediaID == MediaId.Value);
            if (media != null)
            {
                Db.Medias.Remove(media);
            }
            Db.SaveChanges();
            return RedirectToAction("EditArticle", new { id = ArticleId });
        }

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
            if (model.NewCategory.CategoryName != null)
            {
                Db.Categories.Add(newCategory);
                Db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public ActionResult RemoveCategory(int? id)
        {
            Db.Categories.Remove(Db.Categories.First(x => x.CategoryID == id.Value));
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
            var sortArticle = new Article();
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
            const int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(articles.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Vote(int _id, bool up)
        {
            if (up)
                ++Db.Articles.First(x => x.ArticleID == _id).Ratings.First().RatingLike;
            else
                ++Db.Articles.First(x => x.ArticleID == _id).Ratings.First().RatingDislike;
            Db.SaveChanges();
            return RedirectToAction("ShowArticle", new { id = _id });
        }

        public ActionResult CreateComment(ShowArticle model, string userName)
        {
            Comment newComment = model.NewComment;
            int currentArticleId = model.CurrentArticle.ArticleID;//отримаэм ід поточ статті
            Article temp = Db.Articles.SingleOrDefault(x => x.ArticleID == currentArticleId);//отрим цю статтю за ід
            newComment.Article.Category = temp.Category;//коментар має таку ж каегор як стаття
            newComment.Article.ArticleStatus = 1;
            newComment.ArticleID = temp.ArticleID;
            newComment.MainArticle = temp;
            newComment.MainArticleID = temp.ArticleID;
            newComment.Article.UserID = Db.Users.First(x => x.UserFirstName == userName).UserID;
            //newComment.Article.User.UserFirstName = db.Users.First(x => x.UserFirstName == userName).UserFirstName;
            //newComment.UserID = db.Users.First(x => x.UserFirstName == User.Identity.Name).UserID;
            Db.Comments.Add(newComment);
            Db.SaveChanges();
            return RedirectToAction("ShowArticle/" + temp.ArticleID.ToString());
        }

        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Feedback(FeedbackModel model)
        {
            if (ModelState.IsValid)
            {

                if (Request.IsAjaxRequest())
                {
                    return PartialView("FeedbackSent");
                }

                return View("FeedbackSent");
            }
            return View();
        }
    }
}
