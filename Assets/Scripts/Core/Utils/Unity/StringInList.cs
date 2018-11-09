using System;
using Functional.Maybe;
using UnityEngine;

namespace Core.Utils
{
    public class StringInList : PropertyAttribute
    {
        public string[] List { get; private set; }
        
        public delegate string[] GetStringList();

        public StringInList(params string[] list)
        {
            List = list;
        }

        public StringInList(Type type, string methodName)
        {
            var method = type.GetMethod(methodName);
            if (method != null)
            {
                var result = method.Invoke(null, null);
                List = result.ToMaybe().SelectOrElse(r => r as string[], () => new string[]{});
            }
            else
            {
                Debug.LogError("NO SUCH METHOD " + methodName + " FOR " + type);
            }
        }

    }
}