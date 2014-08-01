using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using TitleTurtle.Filters;
using TitleTurtle.Models;
using System.IO;
using System.Web;

namespace TitleTurtle.Controllers
{
    /// <summary>
    /// Class for WebSecurity. Login, LogOff, Registration etc.
    /// </summary>
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        HomeContext db = new HomeContext();
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    Roles.AddUserToRole(model.UserName, "RegUser");
                    db.Contacts.Add(new Contact { ContactEmail = "", ContactMobile = "", ContactWebPage = "", UserID = WebSecurity.GetUserId(model.UserName) });
                    db.PersonalDatas.Add(new PersonalData { PersDataAdress = "", PersDataDate = DateTime.Now, PersDataOther = "", UserID = WebSecurity.GetUserId(model.UserName) });
                    var user = new User { Login = model.UserName, UserID = WebSecurity.GetUserId(model.UserName), UserFirstName = "", UserLastName = "" };
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            // User is new, ask for their desired membership name
            string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider;
            string providerUserId;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (var context = new UsersContext())
                {
                    UserProfile user = context.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        context.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        context.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        [HttpGet]
        public ActionResult Control()
        {
            List<User> listOfUser = new List<User>();
            foreach (var user in db.Users.ToList())
                if (user.Login != User.Identity.Name)
                {
                    listOfUser.Add(user);
                }
            return View(listOfUser);
        }

        public ActionResult MakeAuthor(string userName)
        {
            try
            {
                Roles.AddUserToRole(userName, "Author");
                return RedirectToAction("Control");
            }
            catch
            {
                return RedirectToAction("Control");
            }
        }

        public ActionResult MakeAdmin(string userName)
        {
            try
            {
                Roles.AddUserToRole(userName, "Admin");
                return RedirectToAction("Control");
            }
            catch
            {
                return RedirectToAction("Control");
            }

        }
        public ActionResult RemoveFromAuthor(string userName)
        {
            try
            {
                Roles.RemoveUserFromRole(userName, "Author");
                return RedirectToAction("Control");
            }
            catch
            {
                return RedirectToAction("Control");
            }
        }

        public ActionResult RemoveFromAdmins(string userName)
        {
            try
            {
                Roles.RemoveUserFromRole(userName, "Admin");
                return RedirectToAction("Control");
            }
            catch
            {
                return RedirectToAction("Control");
            }
        }

        public ActionResult DeleteUser(string userName)
        {
            try
            {
                db.Users.Remove(db.Users.First(x => x.Login == userName));
                db.SaveChanges();
                foreach (var role in Roles.GetRolesForUser(userName))
                    Roles.RemoveUserFromRole(userName, role);
                Membership.DeleteUser(userName, true);
                return RedirectToAction("Control");
            }
            catch
            {
                //you cant delete current user
                ViewBag.Message = "Пользователь не может быть удалён";
                return RedirectToAction("Control");
            }
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion


        [HttpGet]
        public ActionResult EditUser(String userName)
        {
            EditUser editUser = new EditUser();

            User user = db.Users.FirstOrDefault(c => c.Login == userName);
            editUser.UserID = user.UserID;
            editUser.Login = user.Login;
            editUser.UserFirstName = user.UserFirstName;
            editUser.UserLastName = user.UserLastName;
            editUser.ContactEmail = user.Contacts.ElementAt(0).ContactEmail;
            editUser.PersDataDate = user.PersonalDatas.ElementAt(0).PersDataDate;
            editUser.ContactEmail = user.Contacts.ElementAt(0).ContactEmail;
            editUser.ContactMobile = user.Contacts.ElementAt(0).ContactMobile;

            if (db.UserPhotos.Where(y => (y.UserID == user.UserID && y.UserPhotoCurrent == 1)).Count() != 0)
            {
                editUser.NewMedia = db.Medias.Where(x => x.MediaID == db.UserPhotos.Where(y => (y.UserID == user.UserID && y.UserPhotoCurrent == 1)).FirstOrDefault().MediaID).First();

            }
            return View(editUser);

        }

        [HttpPost]
        public ActionResult EditUser(EditUser model, Media pic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid || model.NewMedia.MediaID == 0)
            {
                var usphoto = new UserPhoto();
                if (uploadImage != null)
                {
                    if (uploadImage.ContentType == "image/jpeg" || uploadImage.ContentType == "image/jpg" || uploadImage.ContentType == "image/gif" || uploadImage.ContentType == "image/png" || uploadImage.ContentType == "image/bmp" || uploadImage.ContentType == "image/ico")
                    {
                        if (uploadImage.ContentLength <= 10000000)
                        {
                            // Read the uploaded file into a byte array
                            byte[] imageData;
                            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                            {
                                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                            }
                            // Set byte array
                            pic.MediaData = imageData;
                            db.Medias.Add(pic);
                            usphoto.MediaID = pic.MediaID;
                            usphoto.UserID = model.UserID;
                            var tmp = db.UserPhotos.FirstOrDefault(x => x.UserPhotoCurrent == 1);
                            if (tmp != null) tmp.UserPhotoCurrent = 0;
                            usphoto.UserPhotoCurrent = 1;
                            db.UserPhotos.Add(usphoto);
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
                        if (uploadImage.ContentLength >= 10000000)
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

                User user = new User();
                user = db.Users.FirstOrDefault(c => c.UserID == model.UserID);
                user.UserLastName = model.UserLastName;
                user.UserFirstName = model.UserFirstName;
                user.Contacts.ElementAt(0).ContactEmail = model.ContactEmail;
                user.Contacts.ElementAt(0).ContactMobile = model.ContactMobile;
                db.SaveChanges();
                ViewBag.Message = "Изменения успешно сохранены";
                return RedirectToAction("EditUser", new { userName = model.Login });
            }
            else
            {
                ViewBag.Message = "Проверте правильность введённых данных";
                return RedirectToAction("EditUser", new { userName = model.Login });
            }
        }

        public ActionResult ShowUser(int? id)
        {
            EditUser editUser = new EditUser();

            User user = db.Users.FirstOrDefault(c => c.UserID == id.Value);
            editUser.UserID = user.UserID;
            editUser.Login = user.Login;
            editUser.UserFirstName = user.UserFirstName;
            editUser.UserLastName = user.UserLastName;
            editUser.ContactEmail = user.Contacts.ElementAt(0).ContactEmail;
            editUser.PersDataDate = user.PersonalDatas.ElementAt(0).PersDataDate;
            editUser.ContactEmail = user.Contacts.ElementAt(0).ContactEmail;
            editUser.ContactMobile = user.Contacts.ElementAt(0).ContactMobile;
            if (db.UserPhotos.Where(y => (y.UserID == user.UserID && y.UserPhotoCurrent == 1)).Count() != 0)
            {
                editUser.NewMedia = db.Medias.Where(x => x.MediaID == db.UserPhotos.Where(y => (y.UserID == user.UserID && y.UserPhotoCurrent == 1)).FirstOrDefault().MediaID).First();
            }
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            if (db.Followers.Where(x => x.FollowID == id.Value && x.UserID == userId).Count() != 0)
            {
                editUser.UserIsFollowed = true;
            }
            else
            {
                editUser.UserIsFollowed = false;
            }
            return View(editUser);
        }

        public ActionResult Follow(int followerID, int followedID)
        {
            db.Followers.Add(new Follower { UserID = followerID, FollowID = followedID });
            db.SaveChanges();
            return RedirectToAction("ShowUser", new { id = followedID });
        }
        public ActionResult UnFollow(int followerID, int followedID)
        {
            db.Followers.Remove(db.Followers.First(x => x.FollowID == followedID && x.UserID == followerID));
            db.SaveChanges();
            return RedirectToAction("ShowUser", new { id = followedID });
        }

        public ActionResult News(int userId)
        {
            var model = new Main
            {
                ArticleList = db.Articles.Where(x => db.Followers.Where(t => t.UserID == userId).Select(y => y.FollowID).Contains(x.UserID)
                                                        && !db.Comments.Select(y => y.ArticleID).Contains(x.ArticleID)).ToList(),
                CategoryList = db.Categories.ToList()
            };
            return View(model);
        }
    }
}
