using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    class ExtremePoint
    {
        public DateTime Time { get; set; }
        public int Type { get; set; }
        public double Value { get; set; }
        public double IncOnce { get; set; }
        public double IncSum { get; set; }
        public double Regress { get; set; }

        private double score;
        public double Score { get { return score; } }

        public double GetScore()
        {
            score = IncOnce + IncSum + Regress;
            return score;
        }
    }
}
