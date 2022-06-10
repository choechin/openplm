using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NLog;
using System.Linq;
using System.Web;

namespace PLMAPI.Connection
{
    public class DBTool
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private SqlConnection connPLM = new SqlConnection(ConfigurationManager.ConnectionStrings["PLM"].ConnectionString);

        public DataTable PLMDataTable(string sql, Dictionary<string, string> mapList)
        {
            DataTable dt = new DataTable();
            //開啟資料庫
            connPLM.Open();
            SqlCommand cmd = new SqlCommand(sql, connPLM);
            foreach (KeyValuePair<string, string> paraName in mapList)
            {
                cmd.Parameters.AddWithValue(paraName.Key, paraName.Value);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            dt = ds.Tables[0];
            if (connPLM.State == ConnectionState.Open) connPLM.Close();

            return dt;
        }

        /// <summary>
        /// 執行PLM Edit
        /// </summary>
        /// <param name="sql">sql cmd</param>
        /// <param name="mapList">參數</param>
        /// <param name="editCnt"></param>
        /// <returns></returns>
        public bool EditPLM(string sql, Dictionary<string, string> mapList, ref int editCnt)
        {
            bool booEdit = false;
            editCnt = 0;
            SqlCommand cmd = new SqlCommand(sql, connPLM);
            foreach (KeyValuePair<string, string> paraName in mapList)
            {
                cmd.Parameters.AddWithValue(paraName.Key, paraName.Value);
            }
            try
            {
                connPLM.Open();
                editCnt = cmd.ExecuteNonQuery();
                booEdit = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw new Exception(ex.ToString());
            }
            finally
            {
                connPLM.Close();
                connPLM.Dispose();
            }
            return booEdit;
        }
    }
}