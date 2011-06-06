using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Collections;

namespace Strive.Common
{
    public static class Extensions
    {
        public static string Description(this TimeSpan ts)
        {
            if (ts <= TimeSpan.Zero)
                return "now";
            if (ts < TimeSpan.FromMinutes(1))
                return "soon";
            if (ts < TimeSpan.FromHours(1))
                return "later";
            if (ts < TimeSpan.FromDays(1))
                return "today";
            if (ts < TimeSpan.FromDays(7))
                return "this week";
            if (ts < TimeSpan.FromDays(30))
                return "this month";
            if (ts < TimeSpan.FromDays(365))
                return "this year";
            return "never";
        }

        public static TValue ValueOrDefault<TKey, TValue>(this FSharpMap<TKey, TValue> map, TKey key)
        {
            var option = map.TryFind(key);
            return option == null ? default(TValue) : option.Value;
        }

        public static IEnumerable<TValue> Values<TKey, TValue>(this FSharpMap<TKey, TValue> map)
        {
            return map.Select(kvp => kvp.Value);
        }
    }
}
