using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            
        }

        public override double? GetIncrease(int days)
        {
            throw new NotImplementedException();
        }
    }
}
