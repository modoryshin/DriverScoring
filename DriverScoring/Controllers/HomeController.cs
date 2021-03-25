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
        public static DBModels.mainEntitiesDB db = new DBModels.mainEntitiesDB();
        static DBModels.Пользователи currentuser;
        public static long RequestIdent;
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
            ViewData.Clear();
            db.Database.Connection.Open();
            List<DBModels.Пользователи> codes = (from e in db.Пользователи where (e.Login == SignInLogin && e.Password == SignInPassword) select e).ToList();
            if (codes.Count != 0)
            {
                List<DBModels.Водители> obj = (from e in db.Водители where (e.ПользовательID == codes[0].ПользовательID) select e).ToList();
                if (obj.Count() != 0)
                {
                    currentuser = codes[0];
                    db.Database.Connection.Close();
                    return RedirectToAction("DriverPanel");
                }
                else
                {

                    List<long> id1 = (from e in db.Аналитики where (e.ПользовательID == codes[0].ПользовательID) select e.АналитикID).ToList();
                    currentuser = codes[0];

                    db.Database.Connection.Close();
                    return RedirectToAction("AdministratorPanel");
                }
            }
            else
            {
                ViewData["Login"] = SignInLogin;
                TempData["alertMessage"] = "Такой аккаунт не существует";

                db.Database.Connection.Close();
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
            db.Database.Connection.Open();
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
                db.SaveChanges(); db.Database.Connection.Close();
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
                db.Database.Connection.Close();
                return View();
            }

        }

        public ActionResult AdministratorPanel()
        {
            ViewData.Clear();
            db.Database.Connection.Open();
            ViewData["User"] = currentuser.Login;
            List<DBModels.Запросы> list = (from e in db.Запросы select e).ToList();
            db.Database.Connection.Close();
            ViewData["RequestList"] = list;
            return View();
        }

        public ActionResult DriverPanel()
        {
            ViewData.Clear();
            ViewData["User"] = currentuser.Login;
            return View();
        }

        public ActionResult DriverApplication()
        {
            ViewData.Clear();
            ViewData["User"] = currentuser.Login;
            return View();
        }

        public ActionResult ApplicationInfo(string Id)
        {
            ViewData.Clear();
            ViewData["Id"] = Id;
            ViewData["Name"] = currentuser.Login;
            return View();
        }

        [HttpPost]
        public ActionResult AdministratorPanel(string ReqId)
        {
            return RedirectToAction("Application info",new { Id=ReqId});
        }

        public ActionResult ApplicationDecision(string dec,long requestid)
        {
            if (dec == "Принять")
            {
                db.Database.Connection.Open();
                List<DBModels.Запросы> obj = (from e in db.Запросы where(e.ЗапросID==requestid) select e).ToList();
                obj[0].ЗапросРассмотрен = 0;
                db.SaveChanges();
                DBModels.РезультатЗапроса obj1 = new DBModels.РезультатЗапроса();
                obj1.АналитикID = currentuser.АналитикID;
                obj1.МашинаВыдана = 0;
                obj1.ПричинаОтказа = "-";
                obj1.ЗапросID = requestid;
                long id = 0;
                try
                {
                    id = (long)db.РезультатЗапроса.Max(e => e.РезультатID) + 1;
                }
                catch
                {

                }
                obj1.РезультатID = id;
                db.РезультатЗапроса.Add(obj1);
                db.SaveChanges();
                db.Database.Connection.Close();
                return RedirectToAction("AdministratorPanel");
            }
            else
            {
                RequestIdent = requestid;
                return RedirectToAction("EnterReason",new {reqid=requestid });
            }
            
        }
        
        public ActionResult EnterReason(long reqid)
        {
            ViewData.Clear();
            ViewData["Name"] = currentuser.Login;
            return View();
        }

        [HttpPost]
        public ActionResult EnterReason(string ReasonField)
        {
            db.Database.Connection.Open();
            List<DBModels.Запросы> obj = (from e in db.Запросы where (e.ЗапросID == RequestIdent) select e).ToList();
            obj[0].ЗапросРассмотрен = 0;
            db.SaveChanges();
            DBModels.РезультатЗапроса obj1 = new DBModels.РезультатЗапроса();
            obj1.АналитикID = currentuser.АналитикID;
            obj1.МашинаВыдана = 1;
            obj1.ПричинаОтказа = ReasonField;
            obj1.ЗапросID = RequestIdent;
            long id = 0;
            try
            {
                id = (long)db.РезультатЗапроса.Max(e => e.РезультатID) + 1;
            }
            catch
            {

            }
            obj1.РезультатID = id;
            db.РезультатЗапроса.Add(obj1);
            db.SaveChanges();
            db.Database.Connection.Close();
            return RedirectToAction("AdministratorPanel");
        }
    }
}