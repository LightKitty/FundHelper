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
            score = (IncOnce + IncSum + Regress)/3;
            return score;
        }

        public double GetScore(double v1,double v2,double v3)
        {
            score = v1*IncOnce + v2*IncSum + v3*Regress;
            return score;
        }
    }
}
