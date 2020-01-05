using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    static class Think
    {
        public static void Calculate(DateTime startTime, Fund fund)
        {
            //if (startTime <= fund.HistoryList.First().Item1) startTime = fund.HistoryList[3].Item1;
            DateTime endTime = fund.HistoryList.Last().Item1;
            Calculate(startTime, endTime, fund);
        }
        /// <summary>
        /// 
        /// </summary>
        public static void Calculate(DateTime startTime, DateTime endTime, Fund fund) //, out List<Tuple<DateTime, double>> needFundValues, out List<Tuple<DateTime, double, int>> fundPointsFinal, out Tuple<double, double> t1, out Tuple<double, double> t2
        {
            //fund.ThinkStartTime = startTime;
            //fund.ThinkEndTime = endTime;
            int startIndex = fund.HistoryList.FindIndex(x => x.Item1 >= startTime);
            int endIndex = fund.HistoryList.FindLastIndex(x => x.Item1 <= endTime) + 1;
            var needList = fund.HistoryList.GetRange(startIndex, endIndex - startIndex); //需要用到的历史纪录
            int length = endIndex - startIndex;
            // = new DateTime(2019, 1, 1);
            //Dictionary<DateTime, double> datas = new Dictionary<DateTime, double>();
            //List<Tuple<DateTime, double>> fundValues = fund.historyDic.ToList().ConvertAll(x => new Tuple<DateTime, double>(x.Key, (double)x.Value));
            //List<Tuple<DateTime, double>> fundValues = fund.HistoryDic.ToList().ConvertAll(x => new Tuple<DateTime, double>(x.Key, (double)x.Value));
            //fundValues.Reverse();
            int[] incFlags = GetExtremePoints(needList); //涨跌标识
            //int needStartIndex = fundValues.FindIndex(x => x.Item1 >= startTime) - 1;
            //fund.CoorZeroIndex = needStartIndex;
            //int needEndIndex = fundValues.FindLastIndex(x => x.Item1 <= endTime) - 1;
            //needFundValues = fundValues.GetRange(needStartIndex, needEndIndex - needStartIndex + 1);
            //needfundValues.Reverse();
            //int arrLength = needFundValues.Count;
            double[] arrX = new double[length];
            double[] arrY = new double[length];
            for (int i = 0; i < length; i++)
            {
                arrX[i] = i;
                arrY[i] = needList[i].Item2;
            }

            double[] coefs = MultiLine(arrX, arrY, length, 1);
            fund.Coefs = coefs;
            //double y1 = coefs[0] + coefs[1] * 1;
            //double y2 = coefs[0] + coefs[1] * (length - 2);

            //t1 = new Tuple<double, double>(1, y1);
            //t2 = new Tuple<double, double>(arrLength - 2, y2);

            //fundPoints = GetExtremePoints(fundValues);
            //if (fundPoints.First().Item3 != -1) fundPoints.RemoveAt(0); //第一个不是极小值
            //if (fundPoints.Last().Item3 != 1) fundPoints.RemoveAt(fundPoints.Count - 1); //最后一个不是极大值

            //List<Tuple<DateTime, double, int>> fundPointsFinal = ExtremePointsFiltrate(fundPoints);
            int[] incFinalFlags = ExtremePointsFiltrate(needList, incFlags);
            //List<double> adds = new List<double>();
            //double inc = 0.0;
            //for (int i = 0; i < fundPointsFinal.Count; i++)
            //{
            //    double add = 100 * (fundPointsFinal[i + 1].Item2 - fundPointsFinal[i].Item2) / fundPointsFinal[i].Item2;
            //    adds.Add(add);
            //    inc += add;
            //    i++;
            //}

            double vMax = double.MinValue;
            double v1Max = 0.0;
            double v2Max = 0.0;
            double v3Max = 0.0;
            double maxMeanWin = 0.0;
            double maxVarianceWin = 0.0;

            double nolMeanWin = 0.0;
            double nolVarianceWin = 0.0;

            double minMeanWin = 0.0;
            double minVarianceWin = 0.0;

            //List<FundDayPoint> extremePointsMax = null;
            for (double v1=0.00;v1<1;v1+=0.01)
            {
                for (double v2 = 0.00; v2 < (1-v1); v2 += 0.01)
                {
                    double v3 = 1 - v1 - v2;
                    if (v3 < 0.0) continue;
                    //分析

                    List<double> maxScores = new List<double>();
                    List<double> nolScores = new List<double>();
                    List<double> minScores = new List<double>();
                    //double[] scores = new double[length];
                    for (int i = 1; i < needList.Count; i++)
                    {
                        //DateTime time = needList[i].Item1;
                        //int indexNow = needFundValues.FindIndex(x => x.Item1 == time);
                        double lastValue = needList[i - 1].Item2;
                        double valueNow = needList[i].Item2;
                        double incOnce = (valueNow - lastValue);

                        int lastIndex = -1;
                        for(int j=i-1;j>=0;j--)
                        {
                            if(incFinalFlags[j]!=0)
                            {
                                lastIndex = j;
                                break;
                            }
                        }
                        if (lastIndex < 0) continue;
                        //DateTime timeLast = fundPointsFinal[lastIndex].Item1;
                        //int lastIndex = needFundValues.FindIndex(x => x.Item1.Equals(last.Item1));
                        double lastFinalValue = needList[lastIndex].Item2;
                        double incSum = (valueNow - lastFinalValue); //可优化，三点判断

                        double equation = EquationCalculate(coefs[1], coefs[0], i);
                        double regress = (valueNow - equation) ;

                        //var point = fundPointsFinal.Find(x => x.Item1.Equals(time));
                        //var ep = new FundDayPoint() { Time = time, IncOnce = incOnce, IncSum = incSum, Regress = regress, Value = needFundValues[i].Item2, Type = point == null ? 0 : point.Item3 };
                        double score = GetScore(v1, v2, v3, incOnce, incSum, regress);
                        //scores[i] = score;
                        switch(incFinalFlags[i])
                        {
                            case 1: //极大值
                                maxScores.Add(score);
                                break;
                            case 0: //常值
                                nolScores.Add(score);
                                break;
                            case -1: //极小值
                                minScores.Add(score);
                                break;
                        }
                    }
                    //List<ExtremePoint> p1s = extremePoints.FindAll(x => x.Type == 1);
                    //List<ExtremePoint> p2s = extremePoints.FindAll(x => x.Type == -1);


                    double maxMean = Mean(maxScores);
                    double maxVariance = Variance(maxScores);

                    double nolMean = Mean(nolScores);
                    double nolVariance = Variance(nolScores);

                    double minMean = Mean(minScores);
                    double minVariance = Variance(minScores);

                    if (!(maxMean > nolMean && nolMean > minMean))
                    {
                        //不符合条件
                        continue;
                    }

                    double value = (maxMean - maxVariance) - (nolMean + nolVariance) + (nolMean - nolVariance) - (minMean + nolVariance);
                    if(value> vMax)
                    {
                        //extremePointsMax = extremePoints;
                        vMax = value;
                        v1Max = v1;
                        v2Max = v2;
                        v3Max = v3;

                        maxMeanWin = maxMean;
                        maxVarianceWin = maxVariance;

                        nolMeanWin = nolMean;
                        nolVarianceWin = nolVariance;

                        minMeanWin = minMean;
                        minVarianceWin = minVariance;
                    }
                }
            }

            fund.V1 = v1Max;
            fund.V2 = v2Max;
            fund.V3 = v3Max;
            //fund.ExtremePoints = extremePointsMax;
            fund.μMax = maxMeanWin;
            fund.σMax = maxVarianceWin;
            fund.μNol = nolMeanWin;
            fund.σNol = nolVarianceWin;
            fund.μMin = minMeanWin;
            fund.σMin = minVarianceWin;
            fund.ThinkStartIndex = startIndex;
            fund.ThinkEndIndex = endIndex;
            fund.incFlags = incFinalFlags;
        }

        private static double GetScore(double v1, double v2, double v3, double x1, double x2, double x3)
        {
            return v1 * x1 + v2 * x2 + v3 * x3;
        }

        private static double Variance(List<double> values)
        {
            double mean = values.Sum() / values.Count;
            double sum = 0;
            for(int i=0; i< values.Count; i++)
            {
                sum += Math.Pow(values[i] - mean, 2);
            }
            double result = Math.Sqrt(sum / values.Count);
            return result;
        }

        private static double Mean(List<double> values)
        {
            return values.Sum() / values.Count;
        }

        /// <summary>
        /// 二元一次方程计算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static double EquationCalculate(double k, double b, int x)
        {
            return k * x + b;
        }



        /// <summary>
        /// 获取增长
        /// </summary>
        /// <param name="lastValue"></param>
        /// <param name="valueNow"></param>
        /// <returns></returns>
        private static double GetInc(double lastValue, double valueNow)
        {
            double inc = (valueNow - lastValue);
            return inc;
        }

        /// <summary>
        /// 获取极值点
        /// </summary>
        /// <param name="fundValues"></param>
        /// <returns></returns>
        public static int[] GetExtremePoints(List<Tuple<DateTime, double>> list)
        {
            int[] result = new int[list.Count];
            //List<Tuple<DateTime, double, int>> fundPoints = new List<Tuple<DateTime, double, int>>(); //极值点
            //int index = -1;
            //while(index==-1)
            //{
            //    index = fundValues.FindIndex(x => x.Item1.Equals(startDate));
            //    startDate = startDate.AddDays(1);
            //}
            //int lastIndex = -1;
            //while(lastIndex==-1)
            //{
            //    lastIndex = fundValues.FindLastIndex(x => x.Item1.Equals(endDate));
            //    endDate = endDate.AddDays(-1);
            //}
            //int index = fundValues.FindIndex(x => x.Item1 >= startDate) - 1;
            //int lastIndex = fundValues.FindLastIndex(x => x.Item1 <= endDate) - 1;
            for (int i = 1; i < list.Count - 1; i++)
            {
                double valueNow = list[i].Item2;
                double valueLast = list[i - 1].Item2;
                double valueNext = list[i + 1].Item2;
                if (valueNow > valueLast && valueNow >= valueNext || valueNow >= valueLast && valueNow > valueNext)
                {
                    result[i] = 1;
                }
                else if (valueNow < valueLast && valueNow <= valueNext || valueNow <= valueLast && valueNow < valueNext)
                {
                    result[i] = -1;
                }
            }
            return result;
        }

        /// <summary>
        /// 极值点过滤
        /// </summary>
        /// <param name="fundPoints"></param>
        /// <returns></returns>
        public static int[] ExtremePointsFiltrate(List<Tuple<DateTime, double>> list, int[] incFlags)
        {
            //List<Tuple<DateTime, double, int>> fundPointsFinal = new List<Tuple<DateTime, double, int>>(); //筛选后的极值点
            int[] result = new int[incFlags.Length];
            int minIndex = -1;
            double minValue = -1;
            int maxIndex = -1;
            double maxValue = -1;
            for(int i=0; i< incFlags.Length; i++)
            { //第一层寻找极小值
                if (incFlags[i] != -1) continue;
                minIndex = i;
                minValue = list[i].Item2;
                for (int j=i+1;j< incFlags.Length;j++)
                { //第二层寻找极大值
                    if(incFlags[j]==1)
                    { //极大值
                        if (j - i > 5)
                        { //符合条件
                            result[minIndex] = -1;
                            maxIndex = j;
                            maxValue = list[j].Item2;
                            result[j] = 1;
                            i = j;
                            break;
                        }
                        else if (maxValue > 0 && list[j].Item2 > maxValue)
                        { //不符合条件 极大值更大
                            result[maxIndex] = 0;
                            maxIndex = j;
                            result[j] = 1;
                            maxValue = list[j].Item2;
                            i = j;
                            break;
                        }
                    }
                    else if(incFlags[j] == -1)
                    { //极小值
                        if (minValue > 0 && list[j].Item2 < minValue)
                        { //不符合条件 极小值更小
                            i = j;
                            minIndex = j;
                            minValue = list[j].Item2;
                        }
                    }
                }
            }
            return result;
            //for (int i = fundPoints.FindIndex(x => x.Item3 == -1); i < fundPoints.Count; i++)
            //{ //从第一个极小值开始
            //    for (int j = i + 1; j < fundPoints.Count; j++)
            //    { //后面每个
            //        if (fundPoints[j].Item3 == 1)
            //        { //极大值
            //            if ((fundPoints[j].Item1-fundPoints[i].Item1).TotalDays > 5)
            //            { //超过5天
            //                fundPointsFinal.Add(fundPoints[i]);
            //                fundPointsFinal.Add(fundPoints[j]);
            //                i = j + 1;
            //            }
            //            else
            //            { //小于等于5天
            //                if (fundPointsFinal.Any() && fundPoints[j].Item2 > fundPointsFinal.Last().Item2)
            //                { //极大值更大
            //                    fundPointsFinal.RemoveAt(fundPointsFinal.Count - 1);
            //                    fundPointsFinal.Add(fundPoints[j]);
            //                    i = j + 1;
            //                }
            //            }
            //        }
            //        else if (fundPoints[j].Item3 == -1)
            //        { //极小值
            //            if (fundPoints[j].Item2 < fundPoints[i].Item2)
            //            { //极小值更小
            //                i = j;
            //            }
            //        }
            //    }
            //}
            //return fundPointsFinal;
        }

        public static double Predict(Fund fund)
        {
            return Predict(fund, (double)fund.RealValue);
        }

        public static double Predict(Fund fund, double todayValue)
        {
            //double lastValue = fund.HistoryList.FindLast(x => x.Item1 <= fund.ThinkEndTime).Item2;
            double incOnce = todayValue - fund.HistoryList[fund.ThinkEndIndex - 1].Item2;
            double minValue = double.MaxValue;
            double lastMaxValue = -1;
            double lastMinValue = -1;
            for(int i=fund.ThinkEndIndex-1;i>=fund.ThinkStartIndex;i--)
            {
                if(lastMaxValue<0)
                {
                    if(fund.incFlags[i- fund.ThinkStartIndex] ==1)
                    { //找到极大值
                        lastMaxValue = fund.HistoryList[i].Item2;
                    }
                    if(fund.HistoryList[i].Item2<minValue)
                    { //记录最小值
                        minValue = fund.HistoryList[i].Item2;
                    }
                }
                else if(lastMinValue<0)
                {
                    if(fund.incFlags[i-fund.ThinkStartIndex] ==-1)
                    {
                        lastMinValue= fund.HistoryList[i].Item2;
                        break; //寻找结束
                    }
                }
            }
            //var epIndexMax = fund.ExtremePoints.FindLastIndex(x => x.Type == 1);
            //var epIndexMin = fund.ExtremePoints.FindLastIndex(x => x.Type == -1);
            //double minValue = double.MaxValue;
            //for(int i= epIndexMax + 1;i < fund.ExtremePoints.Count;i++)
            //{
            //    if (fund.ExtremePoints[i].Value < minValue) minValue = fund.ExtremePoints[i].Value;
            //}
            //double lastSumValue1 = minValue;
            //double lastSumValue2 = fund.ExtremePoints[epIndexMax].Value;
            //double lastSumValue3 = fund.ExtremePoints[epIndexMin].Value;

            double incSum1 = todayValue - minValue;
            double incSum2 = todayValue - lastMaxValue;
            double incSum3 = todayValue - lastMinValue;
            double incSum = 0.0;
            if (Math.Abs(incSum1) > Math.Abs(incSum2) && Math.Abs(incSum1) > Math.Abs(incSum3))
            {
                incSum = incSum1;
            }
            else if (Math.Abs(incSum2) > Math.Abs(incSum3))
            {
                incSum = incSum2;
            }
            else
            {
                incSum = incSum3;
            }
            //int index = fund.HistoryList.FindLastIndex(x => x.Item1 <= fund.ThinkEndTime) + 1 - fund.CoorZeroIndex;
            double equation = EquationCalculate(fund.Coefs[1], fund.Coefs[0], fund.ThinkEndIndex);
            double regress = todayValue - equation;

            double score = GetScore(fund.V1, fund.V2, fund.V3, incOnce, incSum, regress); //fund.V1 * incOnce + fund.V2 * incSum + fund.V3 * regress;
            return GetFundResult(fund, score);

            //以下版本
            //double score1 = fund.V1 * incOnce + fund.V2 * incSum1 + fund.V3 * regress;
            //double score2 = fund.V1 * incOnce + fund.V2 * incSum2 + fund.V3 * regress;
            //double score3 = fund.V1 * incOnce + fund.V2 * incSum3 + fund.V3 * regress;

            //double r1 = GetFundResult(fund, score1);
            //double r2 = GetFundResult(fund, score2);
            //double r3 = GetFundResult(fund, score3);

            //if(Math.Abs(r1)> Math.Abs(r2)&& Math.Abs(r1)> Math.Abs(r3))
            //{
            //    return r1;
            //}
            //else if(Math.Abs(r2)> Math.Abs(r3))
            //{
            //    return r2;
            //}
            //else
            //{
            //    return r3;
            //}
            //return 0;
        }

        private static double GetFundResult(Fund fund, double score)
        {
            double result = 0;
            if (score > fund.μMax - fund.σMax)
            { //卖出
                double vμ = fund.MaxNormalDistribution(fund.μMax);
                double vx = fund.MaxNormalDistribution(score);
                double vσ = fund.MaxNormalDistribution(fund.μMax - fund.σMax);
                result = score > fund.μMax ? 2 * (vμ - vσ) - (vx - vσ) : vx - vσ;
                return -result; //卖出为负
            }
            else if (score < fund.μMin + fund.σMin)
            { //买入
                double vμ = fund.MinNormalDistribution(fund.μMin);
                double vx = fund.MinNormalDistribution(score);
                double vσ = fund.MinNormalDistribution(fund.μMin - fund.σMin);
                result = score < fund.μMin ? 2 * (vμ - vσ) - (vx - vσ) : vx - vσ;
                return result; //卖出为负
            }
            return result;
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

        ///<summary>
        ///用最小二乘法拟合二元多次曲线
        ///例如：y=a0+a1*x 返回值则为a0 a1
        ///例如：y=a0+a1* x+a2* x*x 返回值则为a0 a1 a2
        ///http://blog.sina.com.cn/s/blog_6e51df7f0100thie.html
        ///</summary>
        ///<param name="arrX">已知点的x坐标集合</param>
        ///<param name="arrY">已知点的y坐标集合</param>
        ///<param name="length">已知点的个数</param>
        ///<param name="dimension">方程的最高次数</param>
        public static double[] MultiLine(double[] arrX, double[] arrY, int length, int dimension)//二元多次线性方程拟合曲线
        {
            int n = dimension + 1;                  //dimension次方程需要求 dimension+1个 系数
            double[,] Guass = new double[n, n + 1];      //高斯矩阵 例如：y=a0+a1*x+a2*x*x
            for (int i = 0; i < n; i++)
            {
                int j;
                for (j = 0; j < n; j++)
                {
                    Guass[i, j] = SumArr(arrX, j + i, length);
                }
                Guass[i, j] = SumArr(arrX, i, arrY, 1, length);
            }
            return ComputGauss(Guass, n);

        }

        public static double SumArr(double[] arr, int n, int length) //求数组的元素的n次方的和
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != 0 || n != 0)
                    s = s + Math.Pow(arr[i], n);
                else
                    s = s + 1;
            }
            return s;
        }
        public static double SumArr(double[] arr1, int n1, double[] arr2, int n2, int length)
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if ((arr1[i] != 0 || n1 != 0) && (arr2[i] != 0 || n2 != 0))
                    s = s + Math.Pow(arr1[i], n1) * Math.Pow(arr2[i], n2);
                else
                    s = s + 1;
            }
            return s;

        }

        public static double[] ComputGauss(double[,] Guass, int n)
        {
            int i, j;
            int k, m;
            double temp;
            double max;
            double s;
            double[] x = new double[n];

            for (i = 0; i < n; i++) x[i] = 0.0;//初始化


            for (j = 0; j < n; j++)
            {
                max = 0;

                k = j;
                for (i = j; i < n; i++)
                {
                    if (Math.Abs(Guass[i, j]) > max)
                    {
                        max = Guass[i, j];
                        k = i;
                    }
                }



                if (k != j)
                {
                    for (m = j; m < n + 1; m++)
                    {
                        temp = Guass[j, m];
                        Guass[j, m] = Guass[k, m];
                        Guass[k, m] = temp;

                    }
                }

                if (0 == max)
                {
                    // "此线性方程为奇异线性方程" 

                    return x;
                }


                for (i = j + 1; i < n; i++)
                {

                    s = Guass[i, j];
                    for (m = j; m < n + 1; m++)
                    {
                        Guass[i, m] = Guass[i, m] - Guass[j, m] * s / (Guass[j, j]);

                    }
                }


            }//结束for (j=0;j<n;j++)


            for (i = n - 1; i >= 0; i--)
            {
                s = 0;
                for (j = i + 1; j < n; j++)
                {
                    s = s + Guass[i, j] * x[j];
                }

                x[i] = (Guass[i, n] - s) / Guass[i, i];

            }

            return x;
        }//返回值是函数的系数
    }
}
