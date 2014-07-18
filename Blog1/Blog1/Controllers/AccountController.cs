using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog1.Models;

namespace Blog1.Controllers
{
    public class AccountController : HomeController
    {
        //
        // GET: /Account/
        public ActionResult Autorization()
        {
            Registration model = new Registration();
            model.Result = "Авторизуйтесь";
            return View(model);
        }
        [HttpPost]
        public ActionResult Autorization(Registration Umodel)
        {
            Registration model = new Registration();
            model.NewUser = new Account();
            model.NewUser = db.Accounts.FirstOrDefault(x => x.Login == Umodel.NewUser.Login);
            if (model.NewUser != null && model.NewUser.Login == Umodel.NewUser.Login && model.NewUser.Password == Umodel.NewUser.Password)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Umodel.Result = "Логин или пароль введен не верно";
                return View(Umodel);
            }
        }
        public ActionResult Registration()
        {
            Registration model = new Registration();
            return View(model);
        }

        //Save after registration
        [HttpPost]
        public ActionResult Registration(Registration user)
        {
            if (user.NewUser.Login != null && user.NewUser.Password != null && user.UserContact.ContactEmail != null)
            {
                db.Accounts.Add(user.NewUser);
                User newUser = new User();
                newUser.UserID = user.NewUser.UserID;
                Contact userContact = new Contact();
                userContact.ContactEmail = user.UserContact.ContactEmail;
                userContact.UserID = user.NewUser.UserID;
                db.Users.Add(newUser);
                db.Contacts.Add(userContact);
                db.SaveChanges();
                user.Result = "Регистрация прошла успешно!";
                return View(user);
            }
            else
            {
                user.Result = "Заполните все поля!";
                return View(user);
            }
        }
        [HttpGet]
        public ActionResult AllUsers()
        {
            return View(db.Accounts.ToList());
        }

    }
}
