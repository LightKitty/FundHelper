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
    public partial class MainForm : Form
    {

        List<Fund> funds = new List<Fund>(); // 基金列表
        DataTable fundTable = new DataTable("fund"); //基金数据表

        List<Stock> stocks = new List<Stock>(); // 股票列表
        DataTable stockTable = new DataTable("stock"); //股票数据表

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //long jsTimeNow = GetJsTimestampNow();
            ////long jsTimeNow = GetJsTimestamp(new DateTime(2019, 12, 01));
            //string getUrl = $"http://hq.sinajs.cn/rn={jsTimeNow}&list=s_sh000001,s_sh000002,s_sh000003,s_sh000004,s_sh000005,s_sh000006,s_sh000007,s_sh000008,s_sh000009";
            //byte[] buffer = SimpleGet(getUrl);
            //string result = Encoding.Default.GetString(buffer);
            //double fundValue = GetfundValue(new DateTime(2019, 12, 02));

            Initfunds(); //初始化基金
            InitStocks(); //初始化股票

            InitFundDataView(); // 初始化基金DataView
            InitStockDataView(); // 初始化股票DataView

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
            fundTable.Columns.Add(new DataColumn() { ColumnName = "RealInc", DataType = typeof(double), Caption = "实时涨幅" });
            dataGridViewFund.DataSource = fundTable;
            for (int i = 0; i < this.dataGridViewFund.Columns.Count; i++)
            { //更改表头显示信息
                dataGridViewFund.Columns[i].HeaderText = fundTable.Columns[i].Caption;
                dataGridViewFund.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            foreach (var fund in funds)
            {
                fundTable.Rows.Add(fund.Code, fund.Name, fund.realIncrease);
            }
        }

        /// <summary>
        /// 初始化股票表和视图
        /// </summary>
        private void InitStockDataView()
        {
            stockTable.Columns.Add(new DataColumn() { ColumnName = "Code", DataType = typeof(string), Caption = "编码" });
            stockTable.Columns.Add(new DataColumn() { ColumnName = "Name", DataType = typeof(string), Caption = "名称" });
            stockTable.Columns.Add(new DataColumn() { ColumnName = "RealInc", DataType = typeof(double), Caption = "实时涨幅" });
            dataGridViewStock.DataSource = stockTable;
            for (int i = 0; i < this.dataGridViewStock.Columns.Count; i++)
            { //更改表头显示信息
                dataGridViewStock.Columns[i].HeaderText = stockTable.Columns[i].Caption;
            }
            foreach (var stock in stocks)
            {
                stockTable.Rows.Add(stock.Code, stock.Name,stock.realIncrease);
            }
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
                    stocks.Add(new Stock() { Code = lineValue[0], Name = lineValue[1] });
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
                    funds.Add(new Fund() { Code=lineValue[0], Name=lineValue[1] });
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            fundHistoryInit(); //获取基金历史信息
            fundsRealUpdate(); // 基金实时刷新
        }

        private void fundHistoryInit()
        {
            foreach(var fund in funds)
            {

            }
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
                fundTable.Select($"Code = '{fund.Code}'").FirstOrDefault()["RealInc"] = fund.realIncrease;
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
        private double GetFundIncNow(string fundCode)
        {
            return Convert.ToDouble(GetRealTimeValue(fundCode)[6]);
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
            string result = Encoding.Default.GetString(Helper.SimpleGet(requestUrl));
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
            if(timeNow.Hour >= 9 && timeNow.Hour <= 16)
            { // 9-16点刷新
                fundsRealUpdate();
                FundTableUpdate();
                
                StockRealUpdate();
                StockTableUpdate();
            }
        }
    }
}
