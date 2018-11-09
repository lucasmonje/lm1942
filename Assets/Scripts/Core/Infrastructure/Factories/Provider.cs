using System;
using System.Collections.Generic;

namespace Core.Infrastructure.Factories
{
    internal static class Provider
    {
        static readonly Dictionary<string, object> instances = new Dictionary<string, object>();

        static Provider()
        {
        }

        internal static T Get<T>(string id = null)
        {
            var type = typeof(T).FullName;
            if (id != null)
                type = id;
            object singletonObj;

            if (instances.TryGetValue(type, out singletonObj))
                return (T) singletonObj;

            return default(T);
        }

        internal static void Set<T>(object singletonObj, string id = null)
        {
            var type = typeof(T).FullName;
            if (id != null)
                type = id;
            instances[type] = singletonObj;
        }

        internal static T GetOrInstanciate<T>(Func<object> instanciator, string id = null)
        {
            var type = typeof(T).FullName;
            if (id != null)
                type = id;

            object singletonObj = Get<T>(type);
            object nullValue = default(T);

            if (singletonObj != nullValue)
                return (T) singletonObj;
            return Instanciate<T>(instanciator, type);
        }

        private static T Instanciate<T>(Func<object> instanciator, string id)
        {
            var singletonObj = instanciator();

            instances[id] = singletonObj;
            return (T) singletonObj;
        }

        internal static void Flush()
        {
            instances.Clear();
        }
    }
}