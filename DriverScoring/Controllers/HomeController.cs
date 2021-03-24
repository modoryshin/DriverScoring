using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace DriverScoring.Controllers
{
    public class HomeController : Controller
    {
        static DBModels.mainEntitiesDB db = new DBModels.mainEntitiesDB();
        static DBModels.Пользователи currentuser;
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
        [HttpPost]
        public ActionResult LogIn(string SignInLogin, string SignInPassword)
        {
            List<DBModels.Пользователи> codes = (from e in db.Пользователи where (e.Login == SignInLogin && e.Password == SignInPassword) select e).ToList();
            if (codes.Count != 0)
            {
                List<DBModels.Водители> obj = (from e in db.Водители where (e.ПользовательID == codes[0].ПользовательID) select e).ToList();
                if (obj.Count() != 0)
                {
                    currentuser = codes[0];
                    return RedirectToAction("DriverPanel");
                }
                else
                {

                    List<long> id1 = (from e in db.Аналитики where (e.ПользовательID == codes[0].ПользовательID) select e.АналитикID).ToList();
                    currentuser = codes[0];
                    return RedirectToAction("AdministratorPanel");
                }
            }
            else
            {
                ViewData["Login"] = SignInLogin;
                TempData["alertMessage"] = "Такой аккаунт не существует";
                return View();
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string RegisterLogin, string RegisterPassword, string RegisterPhone, string RegisterName, string BirthDate, string Series, string ID)
        {
            
            List<long> codes = (from e in db.Пользователи where (e.Login == RegisterLogin && e.Password == RegisterPassword) select e.ПользовательID).ToList();
            if (codes.Count == 0)
            {
                ViewData.Clear();
                DBModels.Пользователи obj = new DBModels.Пользователи();
                long id=0;
                try
                {
                    id = db.Пользователи.Max(e => e.ПользовательID) + 1;
                }
                catch
                {

                }
                obj.Login = RegisterLogin; obj.Password = RegisterPassword;obj.ПользовательID = id;
                db.Пользователи.Add(obj);
                db.SaveChanges();
                DBModels.Водители obj1 = new DBModels.Водители();
                obj1.ПользовательID = id;obj1.ПаспортныеДанные = Series + "|" + ID;obj1.ДеньРождения = BirthDate;obj1.ДатаРегистрации = DateTime.Today.ToString();
                id = 0;
                try
                {
                    id = db.Водители.Max(e => e.ВодительID) + 1;
                }
                catch
                {

                }
                obj1.ВодительID = id;
                db.Водители.Add(obj1);
                db.SaveChanges();
                return RedirectToAction("LogIn");
            }
            else
            {
                ViewData["Login"] = RegisterLogin;
                ViewData["Password"] = RegisterPassword;
                ViewData["Phone"] = RegisterPhone;
                ViewData["Name"] = RegisterName;
                ViewData["BirthDate"] = BirthDate;
                ViewData["Series"] = Series;
                ViewData["ID"] = ID;
                TempData["alertMessage"] = "Аккаунт с указанными данными уже существует";
                return View();
            }

        }

        public ActionResult AdministratorPanel()
        {
            return View();
        }

        public ActionResult DriverPanel()
        {
            ViewData["User"] = currentuser.Login;
            return View();
        }

        public ActionResult DriverApplication()
        {
            ViewData["User"] = currentuser.Login;
            return View();
        }
    }
}