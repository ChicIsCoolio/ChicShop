using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicShop
{
    static class Extensions
    {
        public static bool Contains<T>(this IEnumerable<T> e, Func<T, bool> predicate)
        {
            try
            {
                var x = e.First(predicate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Don't ask about the name, it just works
        public static string ToLowerAndUpper(this string s)
        {
            List<string> split = s.Split(" ").ToList();
            List<string> n = new List<string>();
            split.ForEach(x =>
            {
                string y = x[0].ToString().ToUpper();

                y += x.Replace(x[0].ToString(), "").ToLower();

                n.Add(y);
            });

            return string.Join(' ', n);
        }

        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> dic, KeyValuePair<TKey, TValue> value)
            => dic.Add(value.Key, value.Value);
    }
}
