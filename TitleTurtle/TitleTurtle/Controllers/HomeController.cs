using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TitleTurtle.Filters;
using TitleTurtle.Models;
using PagedList;
using System.IO;
using System.Drawing;
using WebMatrix.WebData;
using System.Net;
using System.Net.Mail;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
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
        public ActionResult Index(Main model, int? categoryId, string sort, int? page)
        {
            if (categoryId == null)
            {
                if (sort == null)
                {
                    sort = "rating";
                    ViewBag.Sort = "rating";
                }
                switch (sort)
                {
                    case "rating":
                        {
                            model = new Main
                            {
                                ArticleList =
                                     (from article in Db.Articles
                                      where !(from comment in Db.Comments
                                              select comment.ArticleID).Contains(article.ArticleID)
                                      select article).Where(x => x.ArticleStatus == 1).ToList().OrderByDescending(a => a.Ratings.ElementAt(0).RatingLike - a.Ratings.ElementAt(0).RatingDislike),
                                CategoryList = Db.Categories.ToList()
                            };
                            ViewBag.Sort = "rating";
                        }; break;
                    case "date":
                        {
                            model = new Main
                            {
                                ArticleList =
                                     (from article in Db.Articles
                                      where !(from comment in Db.Comments
                                              select comment.ArticleID).Contains(article.ArticleID)
                                      select article).Where(x => x.ArticleStatus == 1).ToList().OrderByDescending(a => a.Edits.ElementAt(0).Date),
                                CategoryList = Db.Categories.ToList()
                            };
                            ViewBag.Sort = "date";
                        }; break;
                    case "discussed":
                        {
                            model = new Main
                            {
                                ArticleList =
                                (from article in Db.Articles
                                 where !(from comment in Db.Comments
                                         select comment.ArticleID).Contains(article.ArticleID)
                                 select article).Where(x => x.ArticleStatus == 1).ToList().OrderByDescending(a => a.CommentCount),
                                CategoryList = Db.Categories.ToList()
                            };
                            ViewBag.Sort = "discussed";
                        }; break;
                    case "news":
                        {
                            model = new Main
                            {
                                ArticleList = Db.Articles.Where(x => Db.Followers.Where(t => t.UserID == WebSecurity.CurrentUserId).Select(y => y.FollowID).Contains(x.UserID)
                                                                        && !Db.Comments.Select(y => y.ArticleID).Contains(x.ArticleID)).Where(r => r.ArticleStatus == 1).ToList().OrderByDescending(a => a.Edits.ElementAt(0).Date),
                                CategoryList = Db.Categories.ToList()
                            };
                            ViewBag.Sort = "news";
                        }; break;
                    case "my":
                        {
                            model = new Main
                            {
                                ArticleList =
                                     (from article in Db.Articles
                                      where !(from comment in Db.Comments
                                              select comment.ArticleID).Contains(article.ArticleID) && (article.User.Login == User.Identity.Name)
                                      select article).Where(x => x.ArticleStatus == 1).ToList().OrderByDescending(a => a.Edits.ElementAt(0).Date),
                                CategoryList = Db.Categories.ToList()
                            };
                            ViewBag.Sort = "news";
                        }; break;
                };
            }
            else
            {
                if (sort == null)
                {
                    sort = "rating";
                    ViewBag.Sort = "rating";
                }
                switch (sort)
                {
                    case "rating":
                        {
                            model = new Main
                               {
                                   ArticleList = (from article in Db.Articles
                                                  where article.CategoryID == categoryId && !(from comment in Db.Comments
                                                                                              select comment.ArticleID).Contains(article.ArticleID)
                                                  select article).Where(x => x.ArticleStatus == 1).ToList().OrderByDescending(a => a.Ratings.ElementAt(0).RatingLike - a.Ratings.ElementAt(0).RatingDislike),
                                   CategoryList = Db.Categories.ToList()
                               };
                            ViewBag.Sort = "rating";
                        }; break;
                    case "date":
                        {
                            model = new Main
               {
                   ArticleList = (from article in Db.Articles
                                  where article.CategoryID == categoryId && !(from comment in Db.Comments
                                                                              select comment.ArticleID).Contains(article.ArticleID)
                                  select article).Where(x => x.ArticleStatus == 1).ToList().OrderByDescending(a => a.Edits.ElementAt(0).Date),
                   CategoryList = Db.Categories.ToList()
               };
                            ViewBag.Sort = "date";
                        }; break;
                    case "discussed":
                        {
                            model = new Main
                            {
                                ArticleList =
                                (from article in Db.Articles
                                 where article.CategoryID == categoryId && !(from comment in Db.Comments
                                                                             select comment.ArticleID).Contains(article.ArticleID)
                                 select article).Where(x => x.ArticleStatus == 1).ToList().OrderByDescending(a => a.CommentCount),
                                CategoryList = Db.Categories.ToList()
                            };
                            ViewBag.Sort = "discussed";
                        }; break;
                    case "news":
                        {
                            model = new Main
                            {

                                ArticleList = (from article in Db.Articles
                                               where article.CategoryID == categoryId && article.ArticleStatus == 1 && !(from comment in Db.Comments
                                                                                                                         select comment.ArticleID).Contains(article.ArticleID)
                                               select article).Where(x => Db.Followers.Where(t => t.UserID == WebSecurity.CurrentUserId).Select(y => y.FollowID).Contains(x.UserID)
                                                                        && !Db.Comments.Select(y => y.ArticleID).Contains(x.ArticleID)).ToList().OrderByDescending(a => a.Edits.ElementAt(0).Date),
                                CategoryList = Db.Categories.ToList()
                            };
                            ViewBag.Sort = "news";
                        }; break;
                    case "my":
                        {
                            model = new Main
                            {
                                ArticleList = (from article in Db.Articles
                                               where (article.User.Login == User.Identity.Name) && (article.CategoryID == categoryId) && !(from comment in Db.Comments
                                                                                                                                           select comment.ArticleID).Contains(article.ArticleID)
                                               select article).Where(x => x.ArticleStatus == 1).ToList().OrderByDescending(a => a.Edits.ElementAt(0).Date),
                                CategoryList = Db.Categories.ToList()
                            };
                            ViewBag.Sort = "my";
                        }; break;
                }
            }
            if (categoryId != null)
            {
                ViewBag.CategoryID = categoryId;
            }
            int pageNumber;
            if (page == null)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber = page.Value;
            }

            int pageSize = 10;
            model.PagedList = (PagedList<Article>)model.ArticleList.ToPagedList<Article>(pageNumber, pageSize);
            return View(model);
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
            newArticle.UserID = WebSecurity.GetUserId(User.Identity.Name);
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
                RatingLike = 1,
                RatingRepost = 0,
                RatingView = 1
            };
            try
            {
                if (uploadImage != null && uploadImage.ContentType == "image/jpeg" || uploadImage.ContentType == "image/jpg" || uploadImage.ContentType == "image/gif" || uploadImage.ContentType == "image/png")
                {
                    if (uploadImage.ContentLength <= 2000000)
                    {

                        //Read the uploaded file into a byte array
                        byte[] imageData;
                        using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        }
                        /*byte[] n = new byte[imageData.Length];
                        uploadImage.InputStream.Read(n, 0, (int)imageData.Length);
                        pic.MediaData = GetCompressedImage(uploadImage.InputStream);*/
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
                else
                {
                    if (uploadImage.ContentLength >= 2000000)
                    {
                        ViewBag.Error = "Недопустимый размер и формат файла ";
                        return View(model);
                    }
                    else
                    {
                        ViewBag.Error = "Недопустимый формат файла";
                        return View(model);
                    }
                }
            }
            catch (Exception e)
            {
                //ViewBag.Error = "Ошибка добавления файла";
                //return View(model);
            }

            Db.Edits.Add(newEdit);
            Db.Ratings.Add(newRating);
            Db.Articles.Add(newArticle);
            Db.Likes.Add(new Like { ArticleID = model.NewArticle.ArticleID, Likes = true, UserID = model.NewArticle.UserID });
            mediainart.ArticleID = newArticle.ArticleID;
            Db.SaveChanges();
            return RedirectToAction("ShowArticle", new { id = newArticle.ArticleID });
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
        public ActionResult ShowArticle(int id)
        {
            var model = new ShowArticle();
            model.Article = Db.Articles.SingleOrDefault(x => x.ArticleID == id);
            model.CurrentArticle = Db.Articles.SingleOrDefault(x => x.ArticleID == id);
            var singleOrDefault = Db.Articles.SingleOrDefault(x => x.ArticleID == id);
            if (singleOrDefault != null)
                singleOrDefault.Ratings.First().RatingView++;
            Db.SaveChanges();
            model.CommentList = readArticles(id);
            return View(model);
        }

        IEnumerable<ShowArticle> readArticles(int id)
        {
            var artLst = new List<ShowArticle>();
            var t =
                (from comment in Db.Comments
                 join article in Db.Articles
                 on comment.ArticleID equals article.ArticleID
                 where comment.MainArticleID == id
                 select article).ToList();
            foreach (var item in t)
            {
                var sm = new ShowArticle()
                {
                    Article = item
                };
                if (item.CommentCount > 0)
                {
                    sm.CommentList = readArticles(item.ArticleID);
                }
                artLst.Add(sm);
            }
            return artLst;
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

                if (uploadImage != null && uploadImage.ContentType == "image/jpeg" || uploadImage.ContentType == "image/jpg" || uploadImage.ContentType == "image/gif" || uploadImage.ContentType == "image/png")
                {
                    if (uploadImage.ContentLength <= 2000000)
                    {
                       
                        byte[] imageData;
                        using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        }
                        //pic.MediaData = GetCompressedImage(uploadImage.InputStream);
                        pic.MediaData = imageData;
                        Db.Medias.Add(pic);
                        mediainart.MediaID = pic.MediaID;
                        Db.MediaInArticles.Add(mediainart);
                    }
                    else
                    {
                        ViewBag.Error = "Недопустимый размер файла";
                        return View(model);

                    }

                }
                else
                {
                    if (uploadImage.ContentLength >= 2000000)
                    {
                        ViewBag.Error = "Недопустимый размер и формат файла ";
                        return View(model);
                    }
                    else
                    {
                        ViewBag.Error = "Недопустимый формат файла";
                        return View(model);
                    }

                }
            }
            catch
            {

            }
            mediainart.ArticleID = newArticle.ArticleID;
            Db.SaveChanges();
            return RedirectToAction("ShowArticle", new { id = model.NewArticle.ArticleID });
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
            Article article = Db.Articles.First(x => x.ArticleID == id);
            article.ArticleStatus = 2;
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
            if (model.NewCategory.CategoryName == null) return RedirectToAction("Index");
            Db.Categories.Add(newCategory);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult RemoveCategory(int? id)
        {
            foreach (var article in Db.Articles.Where(x => x.CategoryID == id))
            {
                article.CategoryID = 1011;
            }
            Db.SaveChanges();
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
                articles = articles.Where(s => s.ArticleTitle.ToUpper().Contains(searchString.ToUpper()) && (s.ArticleStatus != 2)
                                       || s.ArticleText.ToUpper().Contains(searchString.ToUpper()) && (s.ArticleStatus != 2));
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
            if (Db.Likes.FirstOrDefault(x => x.UserID == WebSecurity.CurrentUserId && x.ArticleID == _id) == null)
            {
                Db.Likes.Add(new Like { ArticleID = _id, UserID = WebSecurity.CurrentUserId, Likes = up });
                if (up)
                    ++Db.Articles.First(x => x.ArticleID == _id).Ratings.First().RatingLike;
                else
                    ++Db.Articles.First(x => x.ArticleID == _id).Ratings.First().RatingDislike;
            }
            else
            {
                if (Db.Likes.First(x => x.UserID == WebSecurity.CurrentUserId && x.ArticleID == _id).Likes)
                {
                    if (up)
                    {
                        --Db.Articles.First(x => x.ArticleID == _id).Ratings.First().RatingLike;
                        Db.Likes.Remove(Db.Likes.FirstOrDefault(x => x.UserID == WebSecurity.CurrentUserId && x.ArticleID == _id));
                    }
                    else
                    {
                        --Db.Articles.First(x => x.ArticleID == _id).Ratings.First().RatingLike;
                        ++Db.Articles.First(x => x.ArticleID == _id).Ratings.First().RatingDislike;
                        Db.Likes.FirstOrDefault(x => x.UserID == WebSecurity.CurrentUserId && x.ArticleID == _id).Likes = false;
                    }
                }
                else
                {
                    if (up)
                    {
                        ++Db.Articles.First(x => x.ArticleID == _id).Ratings.First().RatingLike;
                        --Db.Articles.First(x => x.ArticleID == _id).Ratings.First().RatingDislike;
                        Db.Likes.Remove(Db.Likes.FirstOrDefault(x => x.UserID == WebSecurity.CurrentUserId && x.ArticleID == _id));
                        Db.Likes.FirstOrDefault(x => x.UserID == WebSecurity.CurrentUserId && x.ArticleID == _id).Likes = true;
                    }
                    else
                    {
                        --Db.Articles.First(x => x.ArticleID == _id).Ratings.First().RatingDislike;
                        Db.Likes.Remove(Db.Likes.FirstOrDefault(x => x.UserID == WebSecurity.CurrentUserId && x.ArticleID == _id));
                    }
                }
            }
            Db.SaveChanges();
            return Redirect(Url.RouteUrl(new { controller = "Home", action = "ShowArticle", id = GetMainArticleId(_id) }) + @"#comment" + _id);
        }

        private void IncreaseParentCoommentCount(int id)
        {
            while (true)
            {
                var mainArticle = (
                    from article in Db.Articles
                    join comment in Db.Comments
                    on article.ArticleID equals comment.MainArticleID
                    where comment.ArticleID == id
                    select article
                    ).FirstOrDefault();

                if (mainArticle != null)
                {
                    mainArticle.CommentCount++;
                    id = mainArticle.ArticleID;
                    continue;
                }
                break;
            }
        }

        public ActionResult CreateComment(ShowArticle model, string userName)
        {
            int currentArticleId = model.CurrentArticle.ArticleID;//отримаэм ід поточ статті
            Article currentArticle = Db.Articles.SingleOrDefault(x => x.ArticleID == currentArticleId);//отрим цю статтю за ід
            currentArticle.CommentCount++;
            IncreaseParentCoommentCount(currentArticleId);
            Comment newComment = model.NewComment;
            newComment.Article.ArticleTitle = "Комментарий";
            newComment.Article.Category = currentArticle.Category;//коментар має таку ж каегор як стаття
            newComment.Article.ArticleStatus = 1;
            newComment.ArticleID = currentArticle.ArticleID;
            newComment.MainArticle = currentArticle;
            newComment.MainArticleID = currentArticle.ArticleID;
            newComment.Article.UserID = Db.Users.First(x => x.Login == userName).UserID;
            newComment.Article.Edits = new List<Edit>
            {
                new Edit
                {
                    Article = newComment.Article,
                    ArticleID = newComment.ArticleID.Value,
                    Date = DateTime.Now,
                    Type = type.Create
                }
            };
            newComment.Article.Ratings = new List<Rating>
            {
                new Rating
                {
                    RatingID = 1,
                    Article = newComment.Article,
                    ArticleID = newComment.ArticleID.Value,
                    RatingDislike = 0,
                    RatingLike = 1,
                    RatingRepost = 0,
                    RatingView = 1
                }
            };
            Db.Comments.Add(newComment);
            Db.SaveChanges();
            //return RedirectToAction("ShowArticle/" + currentArticle.ArticleID);
            return Redirect(Url.RouteUrl(new { controller = "Home", action = "ShowArticle", id = GetMainArticleId(newComment.MainArticleID.Value) }) + @"#comment" + newComment.ArticleID);
        }

        private int GetMainArticleId(int id)
        {
            var query =
                (from comment in Db.Comments
                 where comment.ArticleID == id
                 select comment.MainArticleID).ToList();
            return query.FirstOrDefault().HasValue ? GetMainArticleId(query.First().Value) : id;
        }


        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Feedback(FeedbackModel model)
        {
            var Message = new FeedbackModel();
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("titleturtleua@gmail.com", "54321erhnx");
                client.EnableSsl = true;
                client.Send("titleturtleua@gmail.com", "vgrinda97@gmail.com",
                    model.MessageTopic, model.MessageText + '\n' + model.Email);
            }
            return View("FeedbackSent");
        }
        public ActionResult FeedbackSent()
        {
            return View();
        }
        private byte[] GetCompressedImage(Stream originalBytes)
        {
            Size size = new Size();
            size.Width = 480;
            size.Height = 320;
            ImageFormat format = ImageFormat.Jpeg;
            //using (var streamOriginal = new MemoryStream(originalBytes))
            using (var imgOriginal = Image.FromStream(originalBytes))
            {

                //get original width and height of the incoming image
                var originalWidth = imgOriginal.Width; // 1000
                var originalHeight = imgOriginal.Height; // 800

                //get the percentage difference in size of the dimension that will change the least
                var percWidth = ((float)size.Width / (float)originalWidth); // 0.2
                var percHeight = ((float)size.Height / (float)originalHeight); // 0.25
                var percentage = Math.Max(percHeight, percWidth); // 0.25

                //get the ideal width and height for the resize (to the next whole number)
                var width = (int)Math.Max(originalWidth * percentage, size.Width); // 250
                var height = (int)Math.Max(originalHeight * percentage, size.Height); // 200

                //actually resize it
                using (var resizedBmp = new Bitmap(width, height))
                {
                    using (var graphics = Graphics.FromImage((Image)resizedBmp))
                    {
                        graphics.InterpolationMode = InterpolationMode.Default;
                        graphics.DrawImage(imgOriginal, 0, 0, width, height);
                    }

                    //work out the coordinates of the top left pixel for cropping
                    var x = (width - size.Width) / 2; // 25
                    var y = (height - size.Height) / 2; // 0

                    //create the cropping rectangle
                    var rectangle = new Rectangle(x, y, size.Width, size.Height); // 25, 0, 200, 200

                    //crop
                    using (var croppedBmp = resizedBmp.Clone(rectangle, resizedBmp.PixelFormat))
                    using (var ms = new MemoryStream())
                    {
                        //get the codec needed
                        var imgCodec = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == format.Guid);

                        //make a paramater to adjust quality
                        var codecParams = new EncoderParameters(1);

                        //reduce to quality of 80 (from range of 0 (max compression) to 100 (no compression))
                        codecParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);

                        //save to the memorystream - convert it to an array and send it back as a byte[]
                        croppedBmp.Save(ms, imgCodec, codecParams);
                        return ms.ToArray();
                    }
                }
            }
        }


    }

}
