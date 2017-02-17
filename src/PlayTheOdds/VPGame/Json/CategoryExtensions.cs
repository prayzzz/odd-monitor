using System;
using PlayTheOdds.Models;

namespace PlayTheOdds.VPGame.Json
{
    public static class VpEnum
    {
        public static Category GetCategory(string value)
        {
            switch (value)
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
    }
}