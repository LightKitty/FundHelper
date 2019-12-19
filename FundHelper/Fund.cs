using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    /// <summary>
    /// 基金
    /// </summary>
    class Fund : Security
    {
        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="code"></param>
        public override void GetHistory()
        {
            string getResult = Helper.ByresToString(Helper.SimpleGet(string.Format(Common.fundHistoryUrl, GetShortCode())));
            JObject jo = JObject.Parse(getResult.Substring(0, getResult.IndexOf(')')).Substring(getResult.IndexOf('{')));
            string hisData = jo["data"].ToString();
            string[] dayDatas = hisData.Split('#');
            historyDic = new Dictionary<DateTime, double?>();
            foreach (string dayData in dayDatas)
            {
                string[] dayVales = dayData.Split(',');
                DateTime time = DateTime.ParseExact(dayVales[0], "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                string value = dayVales[1];
                historyDic.Add(time, Convert.ToDouble(value));
            }
            lastDay = historyDic.Keys.FirstOrDefault();
        }

        /// <summary>
        /// 返回增值（%）
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public override double? GetIncrease(int days)
        {
            DateTime time = lastDay.AddDays(-days);
            DateTime realTime = time;
            int addDays = -1;
            while(!historyDic.Keys.Contains(realTime))
            {
                realTime = time.AddDays(addDays);
                addDays = addDays < 0 ? -addDays : -addDays - 1;
                if (addDays > 2) return null; //超过范围 返回空
            }
            double? value = historyDic[lastDay];
            double? hisValue = historyDic[realTime];
            if (days < 5) hisValue = historyDic.Values.Take(days + 1).Last(); // 五天内按连续天数取
            double? result = 100 * (value - hisValue) / hisValue;
            return Math.Round((double)result, 2);
        }

        /// <summary>
        /// 获取编码简称 如 fu_000001 => 000001
        /// </summary>
        /// <returns></returns>
        public override string GetShortCode()
        {
            return Code.Substring(3);
        }
    }
}
