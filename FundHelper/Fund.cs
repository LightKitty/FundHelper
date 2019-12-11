using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    /// <summary>
    /// 基金
    /// </summary>
    class Fund : Security
    {
        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="code"></param>
        public override void GetFundHistory(string code)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取编码简称 如 fu_000001 => 000001
        /// </summary>
        /// <returns></returns>
        public string GetShortCode()
        {
            return Code.Substring(3);
        }
    }
}
