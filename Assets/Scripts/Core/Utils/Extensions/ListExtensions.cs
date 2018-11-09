using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;

namespace Core.Utils.Extensions
{
    public static class ListExtensions
    {
        public static void Resize<T>(this List<T> list, int sz, T c)
        {
            int cur = list.Count;
            if(sz < cur)
                list.RemoveRange(sz, cur - sz);
            else if(sz > cur)
            {
                if(sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                    list.Capacity = sz;
                list.AddRange(Enumerable.Repeat(c, sz - cur));
            }
        }
        public static void Resize<T>(this List<T> list, int sz, Func<T> instantiator  = null) where T : new()
        {
            Resize(list, sz, instantiator != null ? instantiator.Invoke() : new T());
        }

        public static Maybe<T> Next<T>(this List<T> list, T item)
        {
            var indexOf = list.IndexOf(item);
            if (indexOf < 0 || indexOf == list.Count-1) return Maybe<T>.Nothing;
            return list.ElementAtOrDefault(indexOf + 1).ToMaybe();
        }
    }
}