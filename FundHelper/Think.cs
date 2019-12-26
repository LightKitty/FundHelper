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
        public static void Calculate(DateTime startTime, List<Fund> funds, out List<Tuple<DateTime, double>> needFundValues, out List<Tuple<DateTime, double, int>> fundPointsFinal, out Tuple<double, double> t1, out Tuple<double, double> t2)
        {
            // = new DateTime(2019, 1, 1);
            Dictionary<DateTime, double> datas = new Dictionary<DateTime, double>();
            Fund fund = funds.First(x => x.Code == "fu_001549");
            //List<Tuple<DateTime, double>> fundValues = fund.historyDic.ToList().ConvertAll(x => new Tuple<DateTime, double>(x.Key, (double)x.Value));
            List<Tuple<DateTime, double>>  fundValues = fund.historyDic.ToList().ConvertAll(x => new Tuple<DateTime, double>(x.Key, (double)x.Value));
            fundValues.Reverse();
            List<Tuple<DateTime, double, int>> fundPoints = GetExtremePoints(fundValues, startTime);
            int needIndex = fundValues.FindIndex(x => x.Item1 >= startTime) - 1;
            needFundValues = fundValues.GetRange(needIndex, fundValues.Count - needIndex);
            //needfundValues.Reverse();
            int arrLength = needFundValues.Count;
            double[] arrX = new double[arrLength];
            double[] arrY = new double[arrLength];
            for (int i = 0; i < needFundValues.Count; i++)
            {
                arrX[i] = i;
                arrY[i] = needFundValues[i].Item2;
            }

            double[] _arrX = new double[3] { 1, 2, 3 };
            double[] _arrY = new double[3] { 1, 2, 3 };

            double[] coefs = MultiLine(arrX, arrY, arrLength, 1);
            double y1 = coefs[0] + coefs[1] * 1;
            double y2 = coefs[0] + coefs[1] * (arrLength - 2);

            t1 = new Tuple<double, double>(1, y1);
            t2 = new Tuple<double, double>(arrLength - 2, y2);

            //fundPoints = GetExtremePoints(fundValues);
            if (fundPoints.First().Item3 != -1) fundPoints.RemoveAt(0); //第一个不是极小值
            if (fundPoints.Last().Item3 != 1) fundPoints.RemoveAt(fundPoints.Count - 1); //最后一个不是极大值
            for (int i=0;i<fundPoints.Count-1;i++)
            {
                if(fundPoints[i].Item3*fundPoints[i+1].Item3!=-1)
                {
                    int a = 0;
                }
            }

            //List<Tuple<DateTime, double, int>> fundPointsFinal = ExtremePointsFiltrate(fundPoints);
            fundPointsFinal = ExtremePointsFiltrate(fundPoints);
            List<double> adds = new List<double>();
            double inc = 0.0;
            for (int i = 0; i < fundPointsFinal.Count; i++)
            {
                double add = 100 * (fundPointsFinal[i + 1].Item2 - fundPointsFinal[i].Item2) / fundPointsFinal[i].Item2;
                adds.Add(add);
                inc += add;
                i++;
            }

            double vMax = double.MinValue;
            double v1Max = 0.0;
            double v2Max = 0.0;
            double v3Max = 0.0;
            double maxMeanWin = 0.0;
            double maxVarianceWin = 0.0;

            double minMeanWin = 0.0;
            double minVarianceWin = 0.0;

            double nolMeanWin = 0.0;
            double nolVarianceWin = 0.0;
            for (double v1=0.01;v1<1;v1+=0.01)
            {
                for (double v2 = 0.01; v2 < (1-v1); v2 += 0.01)
                {
                    double v3 = 1 - v1 - v2;
                    if (v3 <= 0.0) continue;
                    //分析
                    List<ExtremePoint> extremePoints = new List<ExtremePoint>();
                    for (int i = 1; i < needFundValues.Count; i++)
                    {
                        DateTime time = needFundValues[i].Item1;
                        //int indexNow = needFundValues.FindIndex(x => x.Item1 == time);
                        double lastValue = needFundValues[i - 1].Item2;
                        double valueNow = needFundValues[i].Item2;
                        double incOnce = valueNow - lastValue; ;

                        var last = fundPointsFinal.FindLast(x => x.Item1 < time);
                        if (last == null) continue;
                        //DateTime timeLast = fundPointsFinal[lastIndex].Item1;
                        int lastIndex = needFundValues.FindIndex(x => x.Item1.Equals(last.Item1));
                        double lastFinalValue = last.Item2;
                        double incSum = lastValue - lastFinalValue;

                        double regress = needFundValues[i].Item2 - EquationCalculate(coefs[1], coefs[0], i);

                        var point = fundPointsFinal.Find(x => x.Item1.Equals(time));
                        var ep = new ExtremePoint() { Time = time, IncOnce = incOnce, IncSum = incSum, Regress = regress, Value = needFundValues[i].Item2, Type = point == null ? 0 : point.Item3 };
                        ep.GetScore(v1, v2, v3);
                        extremePoints.Add(ep);
                    }
                    //List<ExtremePoint> p1s = extremePoints.FindAll(x => x.Type == 1);
                    //List<ExtremePoint> p2s = extremePoints.FindAll(x => x.Type == -1);

                    List<double> pMaxs = extremePoints.FindAll(x => x.Type == 1).ConvertAll(x => x.Score);
                    List<double> pMins = extremePoints.FindAll(x => x.Type == -1).ConvertAll(x => x.Score);
                    List<double> pNols = extremePoints.FindAll(x => x.Type == 0).ConvertAll(x => x.Score);

                    double maxMean = Mean(pMaxs);
                    double maxVariance = Variance(pMaxs);

                    double minMean = Mean(pMins);
                    double minVariance = Variance(pMins);

                    double nolMean = Mean(pNols);
                    double nolVariance = Variance(pNols);

                    if (!(maxMean > nolMean && nolMean > minMean))
                    {
                        //不符合条件
                        continue;
                    }

                    double value = (maxMean - maxVariance) - (nolMean + nolVariance) + (nolMean - nolVariance) - (minMean + nolVariance);
                    if(value> vMax)
                    {
                        vMax = value;
                        v1Max = v1;
                        v2Max = v2;
                        v3Max = v3;

                        maxMeanWin = maxMean;
                        maxVarianceWin = maxVariance;

                        minMeanWin = minMean;
                        minVarianceWin = minVariance;

                        nolMeanWin = nolMean;
                        nolVarianceWin = nolVariance;
                    }
                }
            }
            //95%=置信区间计算
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
        public static List<Tuple<DateTime, double, int>> GetExtremePoints(List<Tuple<DateTime, double>> fundValues, DateTime date)
        {
            List<Tuple<DateTime, double, int>> fundPoints = new List<Tuple<DateTime, double, int>>(); //极值点
            int index = -1;
            while(index==-1)
            {
                index = fundValues.FindIndex(x => x.Item1.Equals(date));
                date = date.AddDays(1);
            }
            for (int i = index; i < (fundValues.Count - 1); i++)
            {
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
                                i = j + 1;
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
