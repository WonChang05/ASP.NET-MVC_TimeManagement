using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeManagement.Models
{
    public class mMember
    {
        TimeMGTEntities db = new TimeMGTEntities();
        public member logIn(string username,string pass)
        {
            var query = from o in db.members
                        where o.userName == username && o.password == pass
                        select o;
            member m = query.FirstOrDefault();
            return m;
        }

        public member isMember(string username)
        {
            var query = from o in db.members
                        where o.userName == username 
                        select o;
            member m = query.FirstOrDefault();
            return m;
        }

        public void signUp(member m, string username,string pass)
        {
            m.userName = username;
            m.password = pass;
            db.members.Add(m);
            db.SaveChanges();
        }
    }
}