using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeManagement.Models;

namespace TimeManagement.Controllers
{
    public class HomeController : Controller
    {
        TimeMGTEntities db = new TimeMGTEntities();
        mMember mb = new mMember();

        // GET: Home
        public ActionResult Index()
        {
            //如果判斷是會員就導入會員首頁
            if (Session["who"].ToString() != "guest")
            {
                return Redirect("/home/memberIndex");
            }
            return View();
        }

        public ActionResult memberIndex(member m)
        {
            //如果判斷非會員就導入歡迎頁
            if (Session["who"].ToString() == "guest")
            {
                return Redirect("/home/Index");
            }
            return View();
        }

        public ActionResult Login()
        {
            if (Session["who"].ToString() != "guest")
            {
                return Redirect("/home/Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username,string pass)
        {
            member m = mb.logIn(username,pass);
            if (m == null)
            {
                TempData["errorMessage"] = "資料錯誤，請重新輸入。";
                return Redirect("/home/Login");
            }
            Session["who"] = username;
            return Redirect("/home/memberIndex");
        }

        public ActionResult Logout()
        {
            if (Session["who"].ToString() != "guest")
            {
                Session["who"] = "guest";
                TempData["logoutMessage"] = "已登出，歡迎再度光臨！";
                return Redirect("/home/Index");
            }
            return Redirect("/home/Index");
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(member m, string username,string pass)
        {
            if (username.ToLower() == "guest")
            {
                TempData["errorMessage"] = "使用者名稱錯誤。";
                return Redirect("/home/SignUp");
            }
            else if (mb.isMember(username) == null)
            {
                mb.signUp(m, username,pass);
                TempData["successMessage"] = "註冊成功，請重新登入。";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["errorMessage"] = "使用者名稱已被使用，請重新輸入。";
                return Redirect("/home/SignUp");
            }
        }
    }
}