using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    static class Think
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Calculate(List<Fund> funds)
        {
            Dictionary<DateTime, double> datas = new Dictionary<DateTime, double>();
            Fund fund = funds.First(x => x.Code == "fu_001559");
            List<Tuple<DateTime, double>> fundValues = fund.historyDic.ToList().ConvertAll(x => new Tuple<DateTime, double>(x.Key, (double)x.Value));
            List<Tuple<DateTime, double, int>> fundPoints = new List<Tuple<DateTime, double, int>>(); //极值点
            for (int i = fundValues.FindIndex(x => x.Item1.Equals(new DateTime(2018, 4, 25))); i > 0; i--)
            {
                double valueNow = fundValues[i].Item2;
                double valueLast = fundValues[i - 1].Item2;
                double valueNext = fundValues[i + 1].Item2;
                if (valueNow > valueLast && valueNow > valueNext)
                {
                    fundPoints.Add(new Tuple<DateTime, double, int>(fundValues[i].Item1, fundValues[i].Item2, 1));
                }
                else if (valueNow < valueLast && valueNow < valueNext)
                {
                    fundPoints.Add(new Tuple<DateTime, double, int>(fundValues[i].Item1, fundValues[i].Item2, -1));
                }
            }
            List<Tuple<DateTime, double, int>> fundPointsFinal = new List<Tuple<DateTime, double, int>>(); //筛选后的极值点
            for (int i = fundPoints.FindIndex(x => x.Item3 == -1); i < fundPoints.Count; i++)
            { //从第一个极小值开始
                for (int j = i + 1; j < fundPoints.Count; j++)
                { //后面每个
                    if (fundPoints[j].Item3 == 1)
                    { //极大值
                        if (j - i > 5)
                        { //超过5天
                            fundPointsFinal.Add(fundPoints[i]);
                            fundPointsFinal.Add(fundPoints[j]);
                            i = j + 1;
                        }
                        else
                        { //小于等于5天

                        }
                    }
                    else if (fundPoints[j].Item3 == -1)
                    { //极小值
                        if (fundPoints[j].Item2 < fundPoints[i].Item2)
                        { //极小值更小
                            i = j;
                        }
                    }
                }
            }
            double inc = 0.0;
            for (int i = 0; i < fundPointsFinal.Count; i++)
            {
                inc += 100 * (fundPointsFinal[i + 1].Item2 - fundPointsFinal[i].Item2) / fundPointsFinal[i].Item2;
                i++;
            }
        }
    }
}
