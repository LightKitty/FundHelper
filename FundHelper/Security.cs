using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    /// <summary>
    /// 证券
    /// </summary>
    abstract class Security
    {
        public string Code { get; set; } // 基金编码
        public string Name { get; set; } //基金名称
        public double? RealIncrease { get; set; } //实时涨幅
        public double? RealValue { get; set; } //实时价值
        public Dictionary<DateTime,double?> HistoryDic { get; set; }
        public List<Tuple<DateTime, double>> HistoryList { get; set; }
        public DateTime LastDay { get; set; }

        public abstract void GetHistory();
        public abstract double? GetIncrease(int days);
        public abstract string GetShortCode();
        public void CreateHistoryList()
        {
            HistoryList = HistoryDic.ToList().ConvertAll(x => new Tuple<DateTime, double>(x.Key, (double)x.Value));
            HistoryList.Reverse(); //倒置
        }
    }
}
