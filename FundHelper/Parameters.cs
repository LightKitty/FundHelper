using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    public class Parameters
    {
        public int startIndex;

        public int endIndex;

        public int index;

        public DateTime startTime;

        public DateTime endTime;

        public DateTime time;

        public double k; //拟合直线参数k

        public double b; //拟合直线参数b

        public double μInc; //增长平均值

        public double σInc; //增长方差
    }
}
