using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TianHuiWeb.CBLL;

namespace TianHuiWeb.Controllers
{
    public class BusinessController : Controller
    {
        BLL bll = new BLL();
        // GET: Business
        public ActionResult Index()
        {
            int channel = Convert.ToInt32(Request["channelid"]);
            var dt = bll.FindChanel(50);   //取得栏目信息,直接使用栏目里面的信息
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ViewBag.banner = "<div class=\"product-banner\"><div class=\"img\"><img class=\"pc\" src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\"><img class=\"m\" src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\">";
                ViewBag.banner += "</div><div class=\"m-txt\">" + dr["ChannelName"].ToString() + "</div></div>";

                ViewBag.title = "<div class=\"module-title\">" + dr["ChannelName"].ToString() + "</div><br />";
                ViewBag.Content = dr["Content"].ToString();
            }
            return View();
        }
        /// <summary>
        /// 产品列表
        /// </summary>
        public void LeftProducttList() {
            var dt = bll.FindChanelByParentId(50);
            string ll1 = "";
            if (dt.Rows.Count > 0)
            {
                ll1 += "<dl>";
                foreach (DataRow dr in dt.Rows)
                {
                    ll1 += "<dd><a href=\"" + dr["FilePath"].ToString() + "\">" + dr["ChannelName"].ToString() + "</a></dd>";
                }
                ll1 += "</dl>";
            }
            ll += ll1;
        }
    }
}