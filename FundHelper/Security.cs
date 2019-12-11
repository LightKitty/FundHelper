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
        public double? realIncrease { get; set; } //实时涨幅

        public abstract void GetFundHistory(string code);
    }
}
