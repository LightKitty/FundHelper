using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FundHelper
{
    public partial class MainForm : Form
    {
        #region 字段
        List<Fund> funds = new List<Fund>(); // 基金列表
        DataTable fundTable = new DataTable("fund"); // 基金数据表

        List<Stock> stocks = new List<Stock>(); // 股票列表
        DataTable stockTable = new DataTable("stock"); //股票数据表

        Gold gold; //黄金

        DateTime hisTime = DateTime.Now; //历史计算时间
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            Initfunds(); //初始化基金
            InitStocks(); //初始化股票
            InitGold(); //初始化黄金

            InitFundDataView(); // 初始化基金DataView
            InitStockDataView(); // 初始化股票DataView
            GoldLableUpdate(); // 初始化黄金文本

            Calculate();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Calculate()
        {
            timerUpdate.Stop();

            Dictionary<DateTime, double> datas = new Dictionary<DateTime, double>();
            Fund fund = funds.First(x => x.Code == "fu_005918");
            List<Tuple<DateTime, double>> fundValues = fund.historyDic.ToList().ConvertAll(x => new Tuple<DateTime, double>(x.Key, (double)x.Value));
            List<Tuple<DateTime, double, int>> fundPoints = new List<Tuple<DateTime, double, int>>();
            for (int i=fundValues.FindIndex(x=>x.Item1.Equals(new DateTime(2018,4,25)));i>0;i--)
            {
                double valueNow = fundValues[i].Item2;
                double valueLast = fundValues[i - 1].Item2;
                double valueNext = fundValues[i + 1].Item2;
                if(valueNow> valueLast&&valueNow> valueNext)
                {
                    fundPoints.Add(new Tuple<DateTime, double, int>(fundValues[i].Item1, fundValues[i].Item2, 1));
                }
                else if (valueNow < valueLast && valueNow < valueNext)
                {
                    fundPoints.Add(new Tuple<DateTime, double, int>(fundValues[i].Item1, fundValues[i].Item2, -1));
                }
            }
            List<Tuple<DateTime, double, int>> fundPointsFinal = new List<Tuple<DateTime, double, int>>();
            for (int i= fundPoints.FindIndex(x=>x.Item3==-1); i< fundPoints.Count;i++)
            { //从第一个极小值开始
                for(int j=i+1;j<fundPoints.Count;j++)
                { //后面每个
                    if(fundPoints[j].Item3==1)
                    { //极大值
                        if(j-i>5)
                        { //超过5天
                            fundPointsFinal.Add(fundPoints[i]);
                            fundPointsFinal.Add(fundPoints[j]);
                            i = j + 1;
                        }
                        else
                        { //小于等于5天

                        }
                    }
                    else if(fundPoints[j].Item3 == -1)
                    { //极小值
                        if(fundPoints[j].Item2< fundPoints[i].Item2)
                        { //极小值更小
                            i = j;
                        }
                    }
                }
            }
            double inc = 0.0;
            for(int i=0;i< fundPointsFinal.Count;i++)
            {
                inc += 100 * (fundPointsFinal[i + 1].Item2 - fundPointsFinal[i].Item2) / fundPointsFinal[i].Item2;
                i++;
            }
        }

        /// <summary>
        /// 窗体加载后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //long jsTimeNow = GetJsTimestampNow();
            ////long jsTimeNow = GetJsTimestamp(new DateTime(2019, 12, 01));
            //string getUrl = $"http://hq.sinajs.cn/rn={jsTimeNow}&list=s_sh000001,s_sh000002,s_sh000003,s_sh000004,s_sh000005,s_sh000006,s_sh000007,s_sh000008,s_sh000009";
            //byte[] buffer = SimpleGet(getUrl);
            //string result = Encoding.Default.GetString(buffer);
            //double fundValue = GetfundValue(new DateTime(2019, 12, 02));

            //FundTableUpdate();

            //StockTableUpdate();
        }

        /// <summary>
        /// 初始化基金表和视图
        /// </summary>
        private void InitFundDataView()
        {
            fundTable.Columns.Add(new DataColumn() { ColumnName = "Code", DataType = typeof(string), Caption = "编码" });
            fundTable.Columns.Add(new DataColumn() { ColumnName = "Name", DataType = typeof(string), Caption = "名称" });
            fundTable.Columns.Add(new DataColumn() { ColumnName = "RealInc", DataType = typeof(double), Caption = "实时" });
            fundTable.Columns.Add(new DataColumn() { ColumnName = "day1", DataType = typeof(double), Caption = "1日" });
            fundTable.Columns.Add(new DataColumn() { ColumnName = "day3", DataType = typeof(double), Caption = "3日" });
            fundTable.Columns.Add(new DataColumn() { ColumnName = "day7", DataType = typeof(double), Caption = "7日" });
            fundTable.Columns.Add(new DataColumn() { ColumnName = "day15", DataType = typeof(double), Caption = "15日" });
            fundTable.Columns.Add(new DataColumn() { ColumnName = "month1", DataType = typeof(double), Caption = "1月" });
            fundTable.Columns.Add(new DataColumn() { ColumnName = "month3", DataType = typeof(double), Caption = "3月" });
            fundTable.Columns.Add(new DataColumn() { ColumnName = "month6", DataType = typeof(double), Caption = "6月" });
            fundTable.Columns.Add(new DataColumn() { ColumnName = "year1", DataType = typeof(double), Caption = "1年" });
            //fundTable.Columns.Add(new DataColumn() { ColumnName = "year2", DataType = typeof(double), Caption = "2年" });
            //fundTable.Columns.Add(new DataColumn() { ColumnName = "year3", DataType = typeof(double), Caption = "3年" });
            dataGridViewFund.DataSource = fundTable; // 绑定
            for (int i = 0; i < this.dataGridViewFund.Columns.Count; i++)
            { //更改表头显示信息
                dataGridViewFund.Columns[i].HeaderText = fundTable.Columns[i].Caption;
                dataGridViewFund.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            foreach (var fund in funds)
            {
                double? day1Value = fund.GetIncrease(1);
                double? day3Value = fund.GetIncrease(3);
                double? day7Value = fund.GetIncrease(7);
                double? day15Value = fund.GetIncrease(15);
                double? month1Value = fund.GetIncrease(30);
                double? month3Value = fund.GetIncrease(91);
                double? month6Value = fund.GetIncrease(183);
                double? year1Value = fund.GetIncrease(365);
                //double? year2Value = fund.GetIncrease(730);
                //double? year3Value = fund.GetIncrease(1095);
                object realIncrease;
                if (fund.realIncrease != null) realIncrease = Math.Round((double)fund.realIncrease, 2);
                else realIncrease = null;
                fundTable.Rows.Add(fund.Code, fund.Name, realIncrease, day1Value, day3Value, day15Value, day7Value, month1Value, month3Value, month6Value, year1Value);
            }
        }

        /// <summary>
        /// 初始化股票表和视图
        /// </summary>
        private void InitStockDataView()
        {
            stockTable.Columns.Add(new DataColumn() { ColumnName = "Code", DataType = typeof(string), Caption = "编码" });
            stockTable.Columns.Add(new DataColumn() { ColumnName = "Name", DataType = typeof(string), Caption = "名称" });
            stockTable.Columns.Add(new DataColumn() { ColumnName = "RealInc", DataType = typeof(double), Caption = "实时涨幅(%)" });
            dataGridViewStock.DataSource = stockTable; // 绑定
            for (int i = 0; i < this.dataGridViewStock.Columns.Count; i++)
            { //更改表头显示信息
                dataGridViewStock.Columns[i].HeaderText = stockTable.Columns[i].Caption;
                dataGridViewStock.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            foreach (var stock in stocks)
            {
                stockTable.Rows.Add(stock.Code, stock.Name,stock.realIncrease);
            }
        }

        /// <summary>
        /// 黄金字段更新
        /// </summary>
        private void GoldLableUpdate()
        {
            labelGold.Text = gold.realValue.ToString();
        }

        /// <summary>
        /// 初始化股票
        /// </summary>
        private void InitStocks()
        {

            if (!File.Exists(Common.stocksPath)) File.Create(Common.stocksPath).Close();
            else
            {
                StreamReader sr = new StreamReader(Common.stocksPath, Encoding.Default);
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] lineValue = line.Split(' ');
                    Stock stock = new Stock() { Code = lineValue[0], Name = lineValue[1] };
                    stock.GetHistory();
                    stocks.Add(stock);
                    //dataGridViewStock.Rows.Add(lineValue[0], lineValue[1]);
                    line = sr.ReadLine();
                }
                sr.Close();
            }

            StockRealUpdate(); // 股票实时刷新
        }

        /// <summary>
        /// 初始化基金
        /// </summary>
        private void Initfunds()
        {
            if (!File.Exists(Common.fundsPath)) File.Create(Common.fundsPath).Close();
            else
            {
                StreamReader sr = new StreamReader(Common.fundsPath, Encoding.Default);
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] lineValue = line.Split(' ');
                    Fund fund = new Fund() { Code = lineValue[0], Name = lineValue[1] };
                    fund.GetHistory();
                    funds.Add(fund);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            //fundHistoryInit(); //获取基金历史信息
            fundsRealUpdate(); // 基金实时刷新
        }

        /// <summary>
        /// 初始化黄金
        /// </summary>
        private void InitGold()
        {
            gold = new Gold() { Code = Common.goldCode, Name = "黄金" };
            GoldRealUpadte();
        }

        /// <summary>
        /// 黄金实时更新
        /// </summary>
        private void GoldRealUpadte()
        {
            string text = GetRealTimeValue(gold.Code)[0];
            gold.realValue = Convert.ToDouble(text.Substring(text.IndexOf('\"') + 1));
        }

        /// <summary>
        /// 获取基金历史信息
        /// </summary>
        private void fundHistoryInit()
        {
            //foreach(var fund in funds)
            //{
            //    fund.GetHistory();
            //}
        }

        /// <summary>
        /// 股票表更新
        /// </summary>
        private void StockTableUpdate()
        {
            foreach(Stock stock in stocks)
            {
                stockTable.Select($"Code = '{stock.Code}'").FirstOrDefault()["RealInc"] = stock.realIncrease;
            }
        }

        /// <summary>
        /// 股票实时更新
        /// </summary>
        private void StockRealUpdate()
        {
            foreach (var stock in stocks)
            {
                stock.realIncrease = GetStockIncNow(stock.Code);
            }
        }

        /// <summary>
        /// 基金表更新
        /// </summary>
        private void FundTableUpdate()
        {
            foreach (var fund in funds)
            {
                if(fund.realIncrease != null)
                {
                    fundTable.Select($"Code = '{fund.Code}'").FirstOrDefault()["RealInc"] = Math.Round((double)fund.realIncrease, 2);
                }
            }
        }

        /// <summary>
        /// 基金实时更新
        /// </summary>
        private void fundsRealUpdate()
        {
            foreach (var fund in funds)
            {
                fund.realIncrease = GetFundIncNow(fund.Code);
            }
        }

        /// <summary>
        /// 获取今日基金当前涨跌
        /// </summary>
        /// <returns></returns>
        private double? GetFundIncNow(string fundCode)
        {
            var result = GetRealTimeValue(fundCode);
            if(result?.Count()>6)
            {
                return Convert.ToDouble(result[6]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取今日股票当前涨跌
        /// </summary>
        /// <returns></returns>
        private double GetStockIncNow(string fundCode)
        {
            return Convert.ToDouble(GetRealTimeValue(fundCode)[3]);
        }

        /// <summary>
        /// 获取股票基金信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string[] GetRealTimeValue(string code)
        {
            string requestUrl = string.Format(Common.realTimeUrl, code);
            string result = Helper.ByresToString(Helper.SimpleGet(requestUrl));
            return result.Split(',');
        }

        /// <summary>
        /// 实时刷新定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            DateTime timeNow = DateTime.Now;
            if(timeNow.Hour == 9 && timeNow.Minute == 00 &&timeNow.Day != hisTime.Day)
            { //重启 重新计算历史
                Reboot();
            }

            fundsRealUpdate();
            FundTableUpdate();

            StockRealUpdate();
            StockTableUpdate();

            GoldRealUpadte();
            GoldLableUpdate();
        }

        /// <summary>
        /// 重启程序
        /// </summary>
        private void Reboot()
        {
            Run(Application.ExecutablePath); //启动程序
            Close();
        }

        private static void Run(Object appName)
        {//启动程序
            Process ps = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = appName.ToString()
                }
            };
            ps.Start();
        }
    }
}
