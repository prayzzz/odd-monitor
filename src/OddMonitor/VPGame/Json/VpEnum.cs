using System;
using OddMonitor.Models;

namespace OddMonitor.VPGame.Json
{
    public static class VpEnum
    {
        public static Category GetCategory(string value)
        {
            switch (value.ToLower())
            {
                case "":
                    return Category.None;
                case "basketball":
                    return Category.Basketball;
                case "csgo":
                    return Category.Csgo;
                case "dota":
                    return Category.Dota2;
                case "football":
                    return Category.Soccer;
                case "tennis":
                    return Category.Tennis;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown Category");
            }
        }

        public static MatchFormat GetMatchFormat(string value)
        {
            switch (value.ToLower())
            {
                case "":
                    return MatchFormat.None;
                case "bo1":
                    return MatchFormat.Bo1;
                case "bo2":
                    return MatchFormat.Bo2;
                case "bo3":
                    return MatchFormat.Bo3;
                case "bo5":
                    return MatchFormat.Bo5;
                case "bo7":
                    return MatchFormat.Bo7;
                case "bo9":
                    return MatchFormat.Bo9;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown MatchFormat");
            }
        }

        public static WagerStatus GetWagerStatus(string value)
        {
            value = value.ToLower();
            switch (value)
            {
                case "":
                    return WagerStatus.None;
                case "live":
                    return WagerStatus.Live;
                case "settled":
                    return WagerStatus.Settled;
                case "canceled":
                    return WagerStatus.Canceled;
                default:
                    {
                        if (value.Contains("later"))
                        {
                            return WagerStatus.Open;
                        }

                        if (value.Contains("now"))
                        {
                            return WagerStatus.Live;
                        }

                        if (value.Contains("settling"))
                        {
                            return WagerStatus.Settled;
                        }

                        if (value.Contains("canceling"))
                        {
                            return WagerStatus.Canceled;
                        }

                        throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown WagerStatus");
                    }
            }
        }
    }
}