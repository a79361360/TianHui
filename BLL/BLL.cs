using Fgly.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianHuiWeb.CCommanBll;

namespace TianHuiWeb.CBLL
{
    public class BLL
    {
        SqlDal dal = new SqlDal();
        string sql = "";
        /// <summary>
        /// 根据栏目ID取得列表
        /// </summary>
        /// <param name="chanerlid"></param>
        /// <returns></returns>
        public DataTable FindChanel(int ChannelId) {
            sql = "SELECT ChannelName,Id,ImageUrl,FilePath,Content,ContentNum FROM siteserver_Channel WHERE Id=" + ChannelId + " ORDER BY Id";
            var dt = dal.ExtSql(sql);
            dt = FilterFilePath(dt);
            return dt;
        }
        /// <summary>
        /// 取得首页和一级菜单
        /// </summary>
        /// <returns></returns>
        public DataTable FindLastChanel() {
            sql = "SELECT ChannelName,Id,ImageUrl,FilePath FROM siteserver_Channel WHERE ParentId in(0,1) ORDER BY Id";
            var dt = dal.ExtSql(sql);
            dt = FilterFilePath(dt);
            return dt;
        }
        /// <summary>
        /// 根据栏目ID取得列表
        /// </summary>
        /// <param name="chanerlid"></param>
        /// <returns></returns>
        public DataTable FindChanelByParentId(int ChannelId)
        {
            sql = "SELECT ChannelName,Id,ImageUrl,FilePath FROM siteserver_Channel WHERE ParentId=" + ChannelId + " ORDER BY Id";
            var dt = dal.ExtSql(sql);
            dt = FilterFilePath(dt);
            return dt;
        }
        /// <summary>
        /// 内容列表
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        public DataTable FindContentByChannelId(int ChannelId) {
            sql = "SELECT Id,Title,SubTitle,ImageUrl,Content,CONVERT(VARCHAR(19),LastEditDate,120) LastEditDate FROM model_Content WHERE ChannelId=" + ChannelId+ " AND IsChecked='True'";
            var dt = dal.ExtSql(sql);
            dt = FilterFilePath(dt);
            return dt;
        }
        /// <summary>
        /// 根据ID查询内容
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        public DataTable FindContentById(int id)
        {
            sql = "SELECT Title,SubTitle,ImageUrl,Content,CONVERT(VARCHAR(19),LastEditDate,120) LastEditDate FROM model_Content WHERE Id=" + id + " AND IsChecked='True'";
            var dt = dal.ExtSql(sql);
            dt = FilterFilePath(dt);
            return dt;
        }
        /// <summary>
        /// 取得产品服务与内容的前10条
        /// </summary>
        /// <returns></returns>
        public DataTable FindContentTop() {
            sql = "SELECT TOP 10 Id,Title,SubTitle,ImageUrl,Content FROM dbo.model_Content WHERE ChannelId IN(SELECT ID FROM siteserver_Channel WHERE ParentId=3)";
            var dt = dal.ExtSql(sql);
            dt = FilterFilePath(dt);
            return dt;
        }
        /// <summary>
        /// 将地址中的/index.html去掉
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable FilterFilePath(DataTable dt) {
            foreach (DataRow dr in dt.Rows) {
                if (dt.Columns.Contains("FilePath"))
                    dr["FilePath"] = dr["FilePath"].ToString().Replace("/index.html", "");
                if (dt.Columns.Contains("ImageUrl"))
                    dr["ImageUrl"] = dr["ImageUrl"].ToString().Replace("@", CommanBll.WebSite);
            }
            return dt;
        }
    }
}
