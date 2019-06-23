using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TimeManagement.Models
{
    public class mTimeData
    {
        TimeMGTEntities db = new TimeMGTEntities();

        public List<timeData> timeList(string userName)
        {
            var query = from td in db.timeDatas
                        where td.dataId.Substring(0, userName.Length) == userName
                        orderby td.ymd descending, td.startTime
                        select td;
            return query.ToList();
        }

        public List<timeData> selectDate(string ymd, string userName)
        {
            //2019-05-07%7E2019-05-27
            //01234567890123456789012
            string sDate = ymd.Substring(0, 10);//2019-05-07
            string eDate = ymd.Substring(13, 10);//2019-05-27
            string format = "yyyy-MM-dd";
            DateTime d1 = DateTime.ParseExact(sDate, format, CultureInfo.InvariantCulture);
            DateTime d2 = DateTime.ParseExact(eDate, format, CultureInfo.InvariantCulture);
            var res = db.timeDatas.ToList().
                Where(a => DateTime.ParseExact(a.ymd, format, CultureInfo.InvariantCulture) >= d1
                && DateTime.ParseExact(a.ymd, format, CultureInfo.InvariantCulture) <= d2
                && a.dataId.Substring(0, userName.Length) == userName).OrderBy(a => a.ymd)
                .ToList();
            return res;
        }

        public List<timeData> getNewestTD(string userName) {
            var query = from td in db.timeDatas
                        where td.dataId.Substring(0, userName.Length) == userName
                        orderby td.dataId descending
                        select td;
            return query.ToList();
        }

        public void add(timeData newTD, string userName, string startHour, string startMin, string endHour, string endMin)
        {
            if (getNewestTD(userName).Count == 0)
            {
                newTD.dataId = userName + "0001";
                newTD.startTime = startHour + ":" + startMin;
                newTD.endTime = endHour + ":" + endMin;
                db.timeDatas.Add(newTD);
                db.SaveChanges();
            }
            else
            {
                timeData qTd = getNewestTD(userName).FirstOrDefault();
                int count = Convert.ToInt32(qTd.dataId.Substring(userName.Length, 4));

                newTD.dataId = userName + (count + 1).ToString().PadLeft(4, '0');
                newTD.startTime = startHour + ":" + startMin;
                newTD.endTime = endHour + ":" + endMin;
                db.timeDatas.Add(newTD);
                db.SaveChanges();
            }
        }

        public timeData selectTD(string id) {
            var query = from o in db.timeDatas
                        where o.dataId == id
                        select o;
            return query.FirstOrDefault();
        }

        public void renewTD(timeData td) {
            timeData dbTD = selectTD(td.dataId);
            dbTD.ymd = td.ymd;
            dbTD.dataId = td.dataId;
            dbTD.startTime = td.startTime;
            dbTD.endTime = td.endTime;
            dbTD.category = td.category;
            dbTD.memo = td.memo;
            db.SaveChanges();
        }

        public void delete(timeData td)
        {
            db.timeDatas.Remove(td);
            db.SaveChanges();
        }
    }
}