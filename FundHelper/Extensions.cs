using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 移除一定比例的最大最小值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="proportion"></param>
        public static void RemoveAbnormalValue<T>(this List<T> list, double proportion)
        {
            list.Sort();
            int r1 = (int)Math.Ceiling(list.Count * proportion / 2.0);
            for (int i = 0; i < r1; i++)
            {
                list.RemoveAt(list.Count - 1);
                list.RemoveAt(0);
            }
        }
    }
}
