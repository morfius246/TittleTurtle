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
            model.NewUser = new Autorization();
            model.NewUser = db.Autorization.FirstOrDefault(x => x.Login == Umodel.NewUser.Login);
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
            if (user.NewUser.Login != null && user.NewUser.Password != null)
            {
                db.Autorization.Add(user.NewUser);
                User newUser = new User();
                newUser.UserID = user.NewUser.UserID;
                db.Users.Add(newUser);
                db.SaveChanges();
                user.Result = "Регистрация прошла успешно!";
                return View(user);
            }
            else
            {
                user.Result = "Логин и пароль - обязательные поля для ввода!";
                return View(user);
            }
        }
        [HttpGet]
        public ActionResult AllUsers()
        {
            return View(db.Autorization.ToList());
        }

    }
}
