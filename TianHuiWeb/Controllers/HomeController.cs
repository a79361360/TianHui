using Fgly.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TianHuiWeb.CBLL;
using TianHuiWeb.CCommanBll;

namespace TianHuiWeb.Controllers
{
    public class HomeController : Controller
    {
        BLL bll = new BLL();
        public ActionResult Index()
        {
            //Home1();
            Home2();
            Home3();
            return View();
        }
        private void Home1(int channelid) {
            string ll = "", ll1 = "";
            var dt = bll.FindLastChanel();
            if (dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    if (dr["Id"].ToString() == channelid.ToString())
                        ll += "<li class=\"cur\"><a href=\"/\">" + dr["ChannelName"].ToString() + "</a>";
                    else
                        ll += "<li><a href=\"" + dr["FilePath"].ToString() + "\" target=\"_self\">" + dr["ChannelName"].ToString() + "</a>";
                    if (dr["Id"].ToString() == "50") {
                        var dt_1 = bll.FindChanelByParentId(50);
                        if (dt_1.Rows.Count > 0) {
                            ll1 += "<dl>";
                            foreach (DataRow dr_1 in dt_1.Rows)
                            {
                                ll1 += "<dd><a href=\""+ dr_1["FilePath"].ToString() + "\">"+ dr_1["ChannelName"].ToString() + "</a></dd>";
                            }
                            ll1 += "</dl>";
                        }
                        ll += ll1;
                    }
                    ll += "</li>";
                }
                ViewBag.Home1 = ll;
            }
        }
        /// <summary>
        /// 首页banner图列表
        /// </summary>
        private void Home2() {
            string ll = "", ll1 = "";
            var dt = bll.FindContentByChannelId(1);
            if (dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows)
                {
                    ll += "<li class=\"pc-banner1\"><a href=\"/\" target=\"_blank\"><img src=\"" + dr["ImageUrl"].ToString() + "\" width=\"1920\" height=\"665\" alt=\"" + dr["Title"].ToString() + "\"></a></li>";
                    ll1 += "<div class=\"swiper-slide\" style=\"max-width: 500px;\"><a href=\"\"><img src=\"../assets/static/images/index/bannera1m.jpg\" style=\"width:100%; height:100%; \"></a></div>";
                }
            }
            ViewBag.Home2 = ll;
            ViewBag.Home21 = ll1;
        }
        /// <summary>
        /// 首页主营业务小图列表
        /// </summary>
        private void Home3() {
            string ll = "";
            var dt = bll.FindChanelByParentId(50);
            if (dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    ll += "<li><a href=\"/business\"><img src=\"" + dr["ImageUrl"].ToString() + "\" width=\"468\" height=\"226\" alt=\"\"><div class=\"maskw\"></div><div class=\"text\"><h3><span>" + dr["ChannelName"].ToString() + "</span></h3></div></a></li>";
                }
            }
            ViewBag.Home3 = ll;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chanelid"></param>
        /// <returns></returns>
        public ActionResult HomePartial(int chanelid) {
            Home1(chanelid);
            return PartialView();
        }













        public ActionResult About()
        {
            var dt = bll.FindContentByChannelId(49);
            if (dt.Rows.Count > 0) {
                DataRow dr = dt.Rows[0];
                ViewBag.banner = "<div class=\"img\"><img src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\"></div><div class=\"cont\"><div class=\"title\">" + dr["Title"].ToString() + "</div></div>";
                ViewBag.title = "<div class=\"title\">" + dr["Title"].ToString() + "</div><br />";
                ViewBag.Content = dr["Content"].ToString();
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}