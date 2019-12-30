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
        public double? realValue { get; set; } //实时价值
        public double? RealIncrease
        {
            get
            {
                return (realValue - historyList.Last().Item2) / historyList.Last().Item2;
            }
        }
        //public double? realValue { get; set; } //实时
        public Dictionary<DateTime,double?> historyDic { get; set; }
        public List<Tuple<DateTime, double>> historyList { get; set; }
        public DateTime lastDay { get; set; }

        public abstract void GetHistory();
        public abstract double? GetIncrease(int days);
        public abstract string GetShortCode();
        public void HistoryDicToList()
        {
            historyList = historyDic.ToList().ConvertAll(x => new Tuple<DateTime, double>(x.Key, (double)x.Value));
            historyList.Reverse(); //倒置
        }
    }
}
