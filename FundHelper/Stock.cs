using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    /// <summary>
    /// 股票
    /// </summary>
    class Stock : Security
    {
        public override void GetHistory()
        {
            string path = Common.historyDir + GetShortCode() + ".csv";
            if(File.Exists(path))
            {
                HistoryDic = new Dictionary<DateTime, double?>();
                StreamReader sr = new StreamReader(path, Encoding.Default);
                sr.ReadLine(); //标题
                string line = sr.ReadLine();
                while(line!=null)
                {
                    string[] lineValues = line.Split(',');
                    DateTime time = Convert.ToDateTime(lineValues[0]);
                    double value = Convert.ToDouble(lineValues[3]);
                    HistoryDic.Add(time, value);

                    line = sr.ReadLine();
                }
                sr.Close();
            }
        }

        public override double? GetIncrease(int days)
        {
            throw new NotImplementedException();
        }

        public override string GetShortCode()
        {
            return Code.Substring(Code.Length - 6);
        }
    }
}
