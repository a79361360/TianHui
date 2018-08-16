using Fgly.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TianHuiWeb.Controllers
{
    public class HomeController : Controller
    {
        SqlDal dal = new SqlDal();
        public ActionResult Index()
        {
            string sql = "SELECT ChannelName,Id FROM siteserver_Channel WHERE ParentId IN(0,1) ORDER BY Id";
            var dt = dal.ExtSql(sql);
            string str = "";
            string channeln = "";
            foreach (DataRow dr in dt.Rows) {
                channeln = dr["ChannelName"].ToString();
                str += "<p>"+ channeln + "</p>";
            }
            ViewBag.str = str;
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
    }
}