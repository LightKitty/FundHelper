using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FundHelper
{
    public partial class Form1 : Form
    {
        // 新浪基金：http://finance.sina.com.cn/fund/quotes/005919/bc.shtml
        // 新浪基金实时API（GET）：http://hq.sinajs.cn/list=fu_001549
        // 新浪基金历史API（GET）：http://finance.sina.com.cn/fund/api/xh5Fund/nav/005919.js
        // 新浪财经：https://finance.sina.com.cn/realstock/company/sh000016/nc.shtml

        List<Found> founds = new List<Found>(); //基金s
        string foundsPath = "founds.ini";

        string percentTodayNowUrl = "http://hq.sinajs.cn/list=fu_{0}"; //获取今日当前涨跌百分比地址
        public Form1()
        {
            InitializeComponent();
            InitFoundDic(); //初始化基金字典
        }

        /// <summary>
        /// 初始化基金字典
        /// </summary>
        private void InitFoundDic()
        {
            if (!File.Exists(foundsPath)) File.Create(foundsPath).Close();
            else
            {
                StreamReader sr = new StreamReader(foundsPath, Encoding.Default);
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] lineValue = line.Split(' ');
                    founds.Add(new Found() { Code=lineValue[0], Name=lineValue[1] });
                    line = sr.ReadLine();
                }
                sr.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //long jsTimeNow = GetJsTimestampNow();
            ////long jsTimeNow = GetJsTimestamp(new DateTime(2019, 12, 01));
            //string getUrl = $"http://hq.sinajs.cn/rn={jsTimeNow}&list=s_sh000001,s_sh000002,s_sh000003,s_sh000004,s_sh000005,s_sh000006,s_sh000007,s_sh000008,s_sh000009";
            //byte[] buffer = SimpleGet(getUrl);
            //string result = Encoding.Default.GetString(buffer);
            //double foundValue = GetFoundValue(new DateTime(2019, 12, 02));

            FoundsRealUpdate();
            FoundsSort();
            textBoxTextUpdate();
        }

        /// <summary>
        /// 文本内容更新
        /// </summary>
        private void textBoxTextUpdate()
        {
            textBox1.Clear();
            foreach (var found in founds)
            {
                textBox1.AppendText($"{found.realIncrease} {found.Name}" + Environment.NewLine);
            }
        }

        /// <summary>
        /// 基金排序
        /// </summary>
        private void FoundsSort()
        {
            founds.Sort((l, r) =>
            {
                if (l.realIncrease > r.realIncrease)
                    return -1;
                else if (l.realIncrease < r.realIncrease)
                    return 1;
                else
                    return 0;
            });
        }

        /// <summary>
        /// 基金实时更新
        /// </summary>
        private void FoundsRealUpdate()
        {
            foreach (var found in founds)
            {
                found.realIncrease = GetPercentTodayNow(found.Code);
            }
        }

        /// <summary>
        /// 获取今日当前涨跌
        /// </summary>
        /// <returns></returns>
        private double GetPercentTodayNow(string foundCode)
        {
            string requestUrl = string.Format(percentTodayNowUrl, foundCode);
            string result = Encoding.Default.GetString(SimpleGet(requestUrl));
            string[] resultArr = result.Split(',');
            return Convert.ToDouble(resultArr[6]);
        }

        private double GetFoundValue(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 简易HttpGet请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public byte[] SimpleGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
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

        private long GetJsTimestampNow()
        {
            return GetJsTimestamp(DateTime.Now);
        }

        private long GetJsTimestamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }

        /// <summary>
        /// 实时刷新定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            FoundsRealUpdate();
            FoundsSort();
            textBoxTextUpdate();
        }
    }
}
