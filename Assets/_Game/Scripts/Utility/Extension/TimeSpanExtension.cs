using System;

namespace _Game.Scripts.Utility.Extension
{
    public static class TimeSpanExtension
    {
        /// 29d 12h
        /// 2h 25m
        /// 15m 43s
        /// 15s
        public static string ToTimerString(this TimeSpan timeSpan)
        {
            return timeSpan > TimeSpan.FromDays(1)
                ? $"{(int) timeSpan.TotalDays}d {timeSpan:%h}h"
                : timeSpan > TimeSpan.FromHours(1)
                    ? $"{(int) timeSpan.TotalHours}h {timeSpan:%m}m"
                    : timeSpan > TimeSpan.FromMinutes(1)
                        ? $"{timeSpan:%m}m {timeSpan:%s}s"
                        : $"{timeSpan:%s}s";
        }
    }
}