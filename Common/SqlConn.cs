using System;
using System.Data.SqlClient;
using Fgly.Common.Expand;

namespace Fgly.Common
{
    /// <summary>
    ///SqlConn 的摘要说明
    /// </summary>
    public class SqlConn : IDisposable
    {

        protected SqlConnection MSqlConn;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlConn()
        {
            string ll = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnString"].ConnectionString.DESDecrypt();
            MSqlConn = new SqlConnection(ll);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="contionstr">字符串</param>
        public SqlConn(string contionstr)
        {
            string ll = System.Configuration.ConfigurationManager.ConnectionStrings[contionstr].ConnectionString.DESDecrypt();
            //string aaa = Fgly.Common.Expand._String.DESEncrypt("Data Source=192.168.1.110,1333;User ID=sa;Password=lianwen;Initial Catalog=fgly_db");
            MSqlConn = new SqlConnection(ll);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (MSqlConn != null)
            {
                MSqlConn.Close();
                MSqlConn.Dispose();
            }
        }

    }
}
