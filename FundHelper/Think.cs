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
            Fund fund = funds.First(x => x.Code == "fu_005918");
            List<Tuple<DateTime, double>> fundValues = fund.historyDic.ToList().ConvertAll(x => new Tuple<DateTime, double>(x.Key, (double)x.Value));

            List<Tuple<DateTime, double, int>> fundPoints = GetExtremePoints(fundValues);
            if (fundPoints.First().Item3 != -1) fundPoints.RemoveAt(0); //第一个不是极小值
            if (fundPoints.Last().Item3 != 1) fundPoints.RemoveAt(fundPoints.Count - 1); //最后一个不是极大值
            for (int i=0;i<fundPoints.Count-1;i++)
            {
                if(fundPoints[i].Item3*fundPoints[i+1].Item3!=-1)
                {
                    int a = 0;
                }
            }

            List<Tuple<DateTime, double, int>> fundPointsFinal = ExtremePointsFiltrate(fundPoints);
            double inc = 0.0;
            for (int i = 0; i < fundPointsFinal.Count; i++)
            {
                inc += 100 * (fundPointsFinal[i + 1].Item2 - fundPointsFinal[i].Item2) / fundPointsFinal[i].Item2;
                i++;
            }
        }

        /// <summary>
        /// 获取极值点
        /// </summary>
        /// <param name="fundValues"></param>
        /// <returns></returns>
        public static List<Tuple<DateTime, double, int>> GetExtremePoints(List<Tuple<DateTime, double>> fundValues)
        {
            List<Tuple<DateTime, double, int>> fundPoints = new List<Tuple<DateTime, double, int>>(); //极值点
            for (int i = fundValues.FindIndex(x => x.Item1.Equals(new DateTime(2018, 4, 25))); i > 0; i--)
            {
                if(fundPoints.Count==23)
                {
                    int a = 0;
                }
                double valueNow = fundValues[i].Item2;
                double valueLast = fundValues[i - 1].Item2;
                double valueNext = fundValues[i + 1].Item2;
                if (valueNow > valueLast && valueNow >= valueNext || valueNow >= valueLast && valueNow > valueNext)
                {
                    fundPoints.Add(new Tuple<DateTime, double, int>(fundValues[i].Item1, fundValues[i].Item2, 1));
                }
                else if (valueNow < valueLast && valueNow <= valueNext || valueNow <= valueLast && valueNow < valueNext)
                {
                    fundPoints.Add(new Tuple<DateTime, double, int>(fundValues[i].Item1, fundValues[i].Item2, -1));
                }
            }
            return fundPoints;
        }

        /// <summary>
        /// 极值点过滤
        /// </summary>
        /// <param name="fundPoints"></param>
        /// <returns></returns>
        public static List<Tuple<DateTime, double, int>> ExtremePointsFiltrate(List<Tuple<DateTime, double, int>> fundPoints)
        {
            List<Tuple<DateTime, double, int>> fundPointsFinal = new List<Tuple<DateTime, double, int>>(); //筛选后的极值点
            for (int i = fundPoints.FindIndex(x => x.Item3 == -1); i < fundPoints.Count; i++)
            { //从第一个极小值开始
                for (int j = i + 1; j < fundPoints.Count; j++)
                { //后面每个
                    if (fundPoints[j].Item3 == 1)
                    { //极大值
                        if ((fundPoints[j].Item1-fundPoints[i].Item1).TotalDays > 5)
                        { //超过5天
                            fundPointsFinal.Add(fundPoints[i]);
                            fundPointsFinal.Add(fundPoints[j]);
                            i = j + 1;
                        }
                        else
                        { //小于等于5天
                            if (fundPointsFinal.Any() && fundPoints[j].Item2 > fundPointsFinal.Last().Item2)
                            { //极大值更大
                                fundPointsFinal.RemoveAt(fundPointsFinal.Count - 1);
                                fundPointsFinal.Add(fundPoints[j]);
                            }
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
            return fundPointsFinal;
        }

        #region 废弃
        /*
        ///
        public static List<Tuple<DateTime, double, int>> ExtremePointsFiltrate2(List<Tuple<DateTime, double, int>> fundPoints)
        {
            List<Tuple<DateTime, double, int>> fundPointsFinal = new List<Tuple<DateTime, double, int>>(); //筛选后的极值点
            List<Tuple<DateTime, double, int, bool>> temp = new List<Tuple<DateTime, double, int, bool>>(); 

            for (int i=0;i<fundPoints.Count; i+=2)
            { //遍历
                if((fundPoints[i+1].Item1-fundPoints[i].Item1).TotalDays>5)
                { //符合条件
                    temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i].Item1, fundPoints[i].Item2, fundPoints[i].Item3, true)); //极小值
                    temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i+1].Item1, fundPoints[i+1].Item2, fundPoints[i+1].Item3, true)); //极大值
                }
                else
                { //不符合条件
                    //int id1 = i - 2;
                    //int id2 = i - 1;
                    if (i-2 > 0&& fundPoints[i+1].Item2> fundPoints[i-1].Item2)
                    { //极大值更大
                        temp[i - 1] = new Tuple<DateTime, double, int, bool>(fundPoints[i - 1].Item1, fundPoints[i - 1].Item2, fundPoints[i - 1].Item3, false); //上一个极大值 标记去掉
                        temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i].Item1, fundPoints[i].Item2, fundPoints[i].Item3, false)); //极小值 标记去掉
                        if ((fundPoints[i + 1].Item1 - fundPoints[i-2].Item1).TotalDays>5)
                        { //符合条件了
                            temp[i - 2] = new Tuple<DateTime, double, int, bool>(fundPoints[i - 2].Item1, fundPoints[i - 2].Item2, fundPoints[i - 2].Item3, true); 
                            temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i + 1].Item1, fundPoints[i + 1].Item2, fundPoints[i + 1].Item3, true)); 
                        }
                        else
                        { //还是不符合
                            temp[i - 2] = new Tuple<DateTime, double, int, bool>(fundPoints[i - 2].Item1, fundPoints[i - 2].Item2, fundPoints[i - 2].Item3, false);
                            temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i + 1].Item1, fundPoints[i + 1].Item2, fundPoints[i + 1].Item3, false));
                        }
                    }
                    else if (i-2 > 0 && temp[i-2].Item4==false && temp[i-1].Item4 == false)
                    { //上两个标记去除
                        if(fundPoints[i-2].Item2< fundPoints[i].Item2)
                        { //极小值更小
                            if((fundPoints[i+1].Item1- fundPoints[i-2].Item1).TotalDays>5)
                            { //符合条件了
                                temp[i - 2] = new Tuple<DateTime, double, int, bool>(temp[i - 2].Item1, temp[i - 2].Item2, temp[i - 2].Item3, true);
                                temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i].Item1, fundPoints[i].Item2, fundPoints[i].Item3, false));
                                temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i + 1].Item1, fundPoints[i + 1].Item2, fundPoints[i + 1].Item3, true));
                            }
                            else
                            {
                                temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i].Item1, fundPoints[i].Item2, fundPoints[i].Item3, false));
                                temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i + 1].Item1, fundPoints[i + 1].Item2, fundPoints[i + 1].Item3, false));
                            }
                        }
                        else
                        {
                            temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i].Item1, fundPoints[i].Item2, fundPoints[i].Item3, false));
                            temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i + 1].Item1, fundPoints[i + 1].Item2, fundPoints[i + 1].Item3, false));
                        }
                    }
                    else
                    {
                        temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i].Item1, fundPoints[i].Item2, fundPoints[i].Item3, false));
                        temp.Add(new Tuple<DateTime, double, int, bool>(fundPoints[i + 1].Item1, fundPoints[i + 1].Item2, fundPoints[i + 1].Item3, false));
                    }

                }

            }

            for(int i=0;i<temp.Count;i++)
            {
                if(temp[i].Item4==true)
                {
                    fundPointsFinal.Add(new Tuple<DateTime, double, int>(temp[i].Item1, temp[i].Item2, temp[i].Item3));
                }
            }
            return fundPointsFinal;

            //int count = 0;
            //while(count>0)
            //{
            //    List<Tuple<DateTime, double, int>> fundPointsFinal2 = new List<Tuple<DateTime, double, int>>();
            //    for (int i = 0; i < fundPointsFinal.Count; i += 2)
            //    {
            //        if(fundPointsFinal[i + 1].Item2> fundPointsFinal[i].Item2)
            //        { //符合条件
            //            fundPointsFinal2.Add(fundPointsFinal[i]);
            //            fundPointsFinal2.Add(fundPointsFinal[i+1]);
            //        }
            //    }
            //    fundPointsFinal = fundPointsFinal2;
            //    count = 0;
            //}
            //return fundPointsFinal;
        }
        */
        #endregion
    }
}
