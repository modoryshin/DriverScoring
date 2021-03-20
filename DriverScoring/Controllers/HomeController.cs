using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DriverScoring.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*
             * 
             * не получилось нормальный unit-test для контекста БД сделать пока. Поэтому через контроллер запускал. Эта штука работает и добавляет тестового пользователя
            
            DriverScoring.DBModels.mainEntitiesDB contextDB = new DriverScoring.DBModels.mainEntitiesDB();
            

            DriverScoring.DBModels.Пользователи person = new DriverScoring.DBModels.Пользователи();
            person.Login = "LOGIN_TEST22";
            person.Password = "PASSWORD_TEST22";

            // Act
            contextDB.Пользователи.Add(person);
            contextDB.SaveChanges();

            person.Login = "LOGIN_TEST";
            person.Password = "PASSWORD_TEST";

            // Act
            contextDB.Пользователи.Add(person);
            contextDB.SaveChanges();
            */

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult AdministratorPanel()
        {
            return View();
        }

        public ActionResult Survey()
        {
            return View();
        }
    }
}