using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeManagement.Models;

namespace TimeManagement.Controllers
{
    public class EventController : Controller
    {
        TimeMGTEntities db = new TimeMGTEntities();
        Models.mTimeData td = new mTimeData();
        // GET: CRUD
        public ActionResult timeList()
        {
            string userName = Session["who"].ToString();
            //如果判斷非會員就導入歡迎頁
            if (userName == "guest")
            {
                return Redirect("/home/Login");
            }
            return View(td.timeList(userName));
        }

        [HttpPost]
        public ActionResult timeList(string ymd)
        {
            string userName = Session["who"].ToString();
            if (ymd.ToLower() == "all")
            {
                return View(td.timeList(userName));
            }
            return View(td.selectDate(ymd, userName));
        }

        public ActionResult Add()
        {
            if (Session["who"].ToString() == "guest")
            {
                return Redirect("/home/Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Add(timeData newTD, string startHour, string startMin, string endHour, string endMin, string OkOrCancel)
        {
            if (OkOrCancel == "ok")
            {
                string userName = Session["who"].ToString();
                td.add(newTD, userName, startHour, startMin, endHour, endMin);
                TempData["CRUD"] = "事件已新增成功。";
            }
            return Redirect("/Event/timeList");
        }

        public ActionResult Edit(string id)
        {
            if (td.selectTD(id) == null)
            {
                return Redirect("/home/Login");
            }
            else
            {
                return View(td.selectTD(id));
            }          
        }

        [HttpPost]
        public ActionResult Edit(timeData originTD, string startHour, string startMin, string endHour, string endMin, string OkOrCancel)
        {
            originTD.startTime = startHour + ":" + startMin;
            originTD.endTime = endHour + ":" + endMin;
            if (OkOrCancel == "ok")
            {
                td.renewTD(originTD);
                TempData["CRUD"] = "事件已修改成功。";
            }
            return Redirect("/Event/timeList");
        }

        public ActionResult Delete(string id)
        {
            if (td.selectTD(id) == null)
            {
                TempData["CRUD"] = "錯誤，導回清單。";
            }
            else
            {
                td.delete(td.selectTD(id));
                TempData["CRUD"] = "事件已刪除成功。";
            }
            return Redirect("/Event/timeList");
        }


        public ActionResult Statistics()
        {
            string userName = Session["who"].ToString();
            if (userName == "guest")
            {
                return Redirect("/home/Login");
            }
            return View(td.timeList(userName));       
        }

        [HttpPost]
        public ActionResult Statistics(string yymd)
        {
            string userName = Session["who"].ToString();
            if (yymd.ToLower() == "all")
            {
                return View(td.timeList(userName));
            }
            else
            {
                return View(td.selectDate(yymd, userName));
            }           
        }

    }
}