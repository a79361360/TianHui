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
            int channelid = Convert.ToInt32(Request["channelid"]);
            if (channelid != 3& channelid != 4& channelid != 5& channelid != 6 & channelid != 7 & channelid != 8)
                return View("Error");
            if (channelid == 3)
                return Redirect("/Business/Index?channelid=4");
            var dt = bll.FindChanel(channelid);   //取得栏目信息,直接使用栏目里面的信息
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ViewBag.banner = "<div class=\"product-banner\"><div class=\"img\"><img class=\"pc\" src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\"><img class=\"m\" src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\">";
                ViewBag.banner += "</div><div class=\"m-txt\">" + dr["ChannelName"].ToString() + "</div></div>";

                ViewBag.title = "<div class=\"module-title\">" + dr["ChannelName"].ToString() + "</div><br />";
                ViewBag.Content = dr["Content"].ToString();
                //快速导航
                LeftProducttList(channelid);
            }
            return View();
        }
        /// <summary>
        /// 产品列表
        /// </summary>
        public void LeftProducttList(int channelid) {
            var dt = bll.FindChanelByParentId(3);
            string ll1 = "",ll2 = "", ll3 = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr["Id"]) == channelid)
                    {
                        ll1 += "<li class=\"cur\"><a href=" + dr["FilePath"].ToString() + " title=" + dr["ChannelName"].ToString() + ">" + dr["ChannelName"].ToString() + "</a></li>";
                        ll2 += "<a id=\"subMenu-tit\" href=\"javascript:;\">" + dr["ChannelName"].ToString() + "</a>";
                    }
                    else
                    {
                        ll1 += "<li><a href=" + dr["FilePath"].ToString() + " title=" + dr["ChannelName"].ToString() + ">" + dr["ChannelName"].ToString() + "</a></li>";
                    }
                    ll3 += "<li><a href=" + dr["FilePath"].ToString() + ">" + dr["ChannelName"].ToString() + "</a></li>";
                }
            }
            ViewBag.QuickNav1 = ll1;
            ViewBag.QuickNav2 = ll2;
            ViewBag.QuickNav3 = ll3;
            //< li class="cur"><a href = "/business" title="公交广告">公交广告</a></li>
            //<li><a href = "/elevator" title="灯箱广告">灯箱广告</a></li>
            //<li><a href = "/screen" title="活动推广">活动推广</a></li>
            //<li><a href = "/store" title="户外大牌">户外大牌</a></li>
            //<li><a href = "/store" title="户外大牌">户外大牌</a></li>
            //<a id="subMenu-tit" href="javascript:;">天汇公交广告</a>

            //<li><a href="/business">天汇公交广告</a></li>
            //<li><a href="/elevator">天汇灯箱广告</a></li>
            //<li><a href="/screen">天汇活动推广</a></li>
            //<li><a href="/store">天汇户外大牌</a></li>
            //<li><a href="/store">天汇视频机</a></li>
        }
        /// <summary>
        /// 安例中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Cases() {
            var dt = bll.FindChanel(9);   //取得栏目信息,直接使用栏目里面的信息
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ViewBag.banner = "<div class=\"product-banner\"><div class=\"img\"><img class=\"pc\" src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\"><img class=\"m\" src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\">";
                ViewBag.banner += "</div><div class=\"case-banner-title\">" + dr["ChannelName"].ToString() + "</div></div>";

                ViewBag.title = "<div class=\"module-title\">" + dr["ChannelName"].ToString() + "</div><br />";

                ViewBag.Content = dr["Content"].ToString();
                //安全导航
                PcCasesList();
            }
            return View();
        }
        /// <summary>
        /// 案例的5个值
        /// </summary>
        private void PcCasesList() {
            var dt = bll.FindContentByChannelId(9);
            string liststr = ""; int index = 1;
            if (dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    if (index == 1)
                        liststr += "<li class=\"case-list-item case-list-m\"><a href=\"" + dr["SubTitle"].ToString() + "\"><span class=\"item-img\"><img src=\"" + dr["ImageUrl"].ToString() + "\"></span><span class=\"item-title\">" + dr["Title"].ToString() + "</span></a></li>";
                    liststr += "<li class=\"case-list-item case-list-item" + index + "\"><a href=\"" + dr["SubTitle"].ToString() + "\"><span class=\"item-img\"><img src=\"" + dr["ImageUrl"].ToString() + "\"></span><span class=\"item-title\">" + dr["Title"].ToString() + "</span></a></li>";
                    index++;
                }
            }
            ViewBag.Case1 = liststr;
            dt.Clear();liststr = "";    //清空
            //取得10条内容
            dt = bll.FindContentTop();
            if (dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    liststr += "<li class=\"caselist-item\"><a class=\"fn-clear\" href=\"/Business/CaseDetail?id=" + dr["Id"].ToString() + "\">";
                    liststr += "<span class=\"item-img\"><img src=\"" + dr["ImageUrl"].ToString() + "\"></span>";
                    liststr += "<span class=\"item-info\"><span class=\"item-title\">" + dr["Title"].ToString() + "</span><span class=\"item-desc\">" + dr["SubTitle"].ToString() + "</span></span></a></li>";
                }
            }
            ViewBag.Case2 = liststr;
        }
        /// <summary>
        /// 内容详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CaseDetail(int id) {
            var dt = bll.FindChanel(9);   //取得栏目信息,直接使用栏目里面的信息
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ViewBag.banner = "<div class=\"product-banner\"><div class=\"img\"><img class=\"pc\" src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\"><img class=\"m\" src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\">";
                ViewBag.banner += "</div><div class=\"case-banner-title\">" + dr["ChannelName"].ToString() + "</div></div>";
            }
            //
            dt.Clear();
            dt = bll.FindContentById(id);
            if (dt.Rows.Count > 0) {
                DataRow dr = dt.Rows[0];
                ViewBag.Content = "<div class=\"all\"><div class=\"detail-title\">" + dr["Title"].ToString() + "</div><div class=\"detail-subtitle\">" + dr["SubTitle"].ToString() + "</div>";
                ViewBag.Content += "<div class=\"detail-date\"><img src=\"../assets/static/images/case/casedetail-date.png\">" + dr["LastEditDate"].ToString() + "</div>";
                ViewBag.Content += "<div class=\"detail-cont\">" + dr["Content"].ToString() + "</div></div>";
            }
            //产品快捷跳转
            LeftProducttList(4);
            return View();
        }
        /// <summary>
        /// 案例中心的产品内容列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Award() {
            LeftProducttList(4);          //快捷
            var dt = bll.FindChanel(9);   //取得栏目信息,直接使用栏目里面的信息
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ViewBag.banner = "<div class=\"product-banner\"><div class=\"img\"><img class=\"pc\" src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\"><img class=\"m\" src=\"" + dr["ImageUrl"].ToString() + "\" alt=\"\">";
                ViewBag.banner += "</div><div class=\"case-banner-title\">" + dr["ChannelName"].ToString() + "</div></div>";
            }
            int channelid = Convert.ToInt32(Request["channelid"]);
            dt.Clear();
            dt = bll.FindChanel(channelid);   //取得栏目信息,直接使用栏目里面的信息
            if (dt.Rows.Count > 0) {
                DataRow dr = dt.Rows[0];
                ViewBag.ChannelName = dr["ChannelName"].ToString();
                ViewBag.ContentNum = dr["ContentNum"].ToString();
            }
            dt.Clear();
            dt = bll.FindContentByChannelId(channelid);
            if (dt.Rows.Count > 0) {
                int index = 1;
                foreach (DataRow dr in dt.Rows) {
                    ViewBag.Content += "<li class=\"caselist-item caselist-item"+ index + "\"><a href=\"/Business/CaseDetail?id="+dr["Id"].ToString() +"\">";
                    ViewBag.Content += "<span class=\"item-img\"><img src=\"" + dr["ImageUrl"].ToString() + "\"></span>";
                    ViewBag.Content += "<span class=\"item-title\">" + dr["Title"].ToString() + "</span></a></li>";
                    index++;
                    if (index == 4) index = 0;
                }
            }
            return View();
        }
        /// <summary>
        /// 荣誉
        /// </summary>
        /// <returns></returns>
        public ActionResult Honor() {
            return View();
        }
    }
}