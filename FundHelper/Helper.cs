using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    /// <summary>
    /// 帮助类
    /// </summary>
    static class Helper
    {
        /// <summary>
        /// 简易HttpGet请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] SimpleGet(string url, string referer = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            if (referer != null) request.Referer = referer;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            int count = (int)response.ContentLength;
            int offset = 0;
            byte[] buf = new byte[count];
            while (count > 0)
            { //循环度可以避免数据不完整
                int n = stream.Read(buf, offset, count);
                if (n == 0) break;
                count -= n;
                offset += n;
            }
            return buf;
        }

        public static string ByresToString(byte[] bytes)
        {
            return ByresToString(bytes, Encoding.Default);
        }

        public static string ByresToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        private static long GetJsTimestampNow()
        {
            return GetJsTimestamp(DateTime.Now);
        }

        private static long GetJsTimestamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
    }
}
