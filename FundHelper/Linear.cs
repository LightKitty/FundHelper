using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    static class Linear
    {
        public static double[] LinearResult(double[] arrayX, double[] arrayY)
        {
            double[] result = { 0, 0 };

            if (arrayX.Length == arrayY.Length)
            {
                double averX = arrayX.Average();
                double averY = arrayY.Average();
                result[0] = Scale(averX, averY, arrayX, arrayY);
                result[1] = Offset(result[0], averX, averY);
            }

            return result;
        }

        private static double Scale(double averX, double averY, double[] arrayX, double[] arrayY)
        {
            double scale = 0;
            if (arrayX.Length == arrayY.Length)
            {
                double Molecular = 0;
                double Denominator = 0;
                for (int i = 0; i < arrayX.Length; i++)
                {
                    Molecular += (arrayX[i] - averX) * (arrayY[i] - averY);
                    Denominator += Math.Pow((arrayX[i] - averX), 2);
                }
                scale = Molecular / Denominator;
            }

            return scale;
        }

        private static double Offset(double scale, double averX, double averY)
        {
            double offset = 0;
            offset = averY - scale * averX;
            return offset;
        }
    }
}
