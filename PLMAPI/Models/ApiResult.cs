using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLMAPI.Models
{
    /// <summary>
    /// API呼叫回傳統一物件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public Dictionary<string, string> error = null;
        /// <summary>
        /// 資料本體
        /// </summary>
        public T data { get; set; }

        public ApiResult() { }

        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <param name="data"></param>
        public ApiResult(T redata)
        {
            data = redata;
        }
    }

    public class ApiError : ApiResult<object>
    {
        /// <summary>
        /// 建立失敗結果
        /// </summary>
        /// <param name="getcode"></param>
        /// <param name="getmsg"></param>
        public ApiError(string getcode, string getmsg)
        {
            error = new Dictionary<string, string>();
            error.Add("code", getcode);
            error.Add("message", getmsg);
        }
    }
}