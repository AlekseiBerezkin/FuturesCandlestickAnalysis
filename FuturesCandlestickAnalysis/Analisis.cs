using FuturesCandlestickAnalysis.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuturesCandlestickAnalysis
{
    class CandleAnalysis
    {
        private Dictionary<string, Kline> first = new Dictionary<string, Kline>();
        private Dictionary<string, Kline> second = new Dictionary<string, Kline>();
        private decimal necessaryAttitude;
        private decimal drawdown;
        public CandleAnalysis(Dictionary<string, Kline> first, Dictionary<string, Kline> second)
        {
            this.first = first;
            this.second = second;
            drawdown = decimal.Parse(Properties.Resources.Drawdown);
            necessaryAttitude = decimal.Parse(Properties.Resources.Attitude);
        }

        public async void StartAnalysis()
        {

            foreach(var k in first)
            {
                if (CheckFall(k.Value.Open, k.Value.Close) && CheckFall(second[k.Key].Open, second[k.Key].Close))
                    continue;

                if(CheckDrawdown(second[k.Key].Open, second[k.Key].Minimum) && CheckAttitude(k.Value,second[k.Key]))
                {
                    //алерт в телегу
                }
            }
        }

        private bool CheckFall(decimal open,decimal close)
        {
            if (open >= close)
                return true;
            return false;
        }

        private bool CheckAttitude(Kline first,Kline second)=>
            (CalcPercent(first.Open, first.Close) / CalcPercent(second.Open, second.Close)) <= necessaryAttitude;
        private decimal CalcPercent(decimal open,decimal close)=> ((close - open) / close) * 100;
        private bool CheckDrawdown(decimal open, decimal min) => Math.Abs(CalcPercent(open, min)) < drawdown;

    }
}
