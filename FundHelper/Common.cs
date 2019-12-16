using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundHelper
{
    /// <summary>
    /// 通用类
    /// </summary>
    static class Common
    {
        // 新浪基金：http://finance.sina.com.cn/fund/quotes/005919/bc.shtml
        // 新浪基金实时API（GET）：http://hq.sinajs.cn/list=fu_001549
        // 新浪基金历史API（GET）：http://finance.sina.com.cn/fund/api/xh5Fund/nav/005919.js
        // 新浪财经：https://finance.sina.com.cn/realstock/company/sh000016/nc.shtml
        // 新浪黄金实时：http://hq.sinajs.cn/etag.php?rn=17441592592465982&list=gds_AUTD,hf_GC,gds_AGTD
        // 新浪指数实时：http://hq.sinajs.cn/rn={jsTimeNow}&list=s_sh000001,s_sh000002,s_sh000003,s_sh000004,s_sh000005,s_sh000006,s_sh000007,s_sh000008,s_sh000009

        public const string fundsPath = "funds.ini"; // 基金配置文件
        public const string stocksPath = "stock.ini"; // 股票配置文件

        public const string realTimeUrl = "http://hq.sinajs.cn/list={0}"; //实时数据获取
        public const string fundHistoryUrl = "http://finance.sina.com.cn/fund/api/xh5Fund/nav/{0}.js";
    }
}
