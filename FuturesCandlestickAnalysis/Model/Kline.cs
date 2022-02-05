using System;
using System.Collections.Generic;
using System.Text;

namespace FuturesCandlestickAnalysis.Model
{
    class Kline
    {
        public DateTime TimeClose { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal Minimum { get; set; }
    }
}
