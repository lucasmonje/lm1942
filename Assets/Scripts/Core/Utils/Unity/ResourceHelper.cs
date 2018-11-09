using System;
using System.IO;
using UnityEngine;

namespace Core.Utils.Unity
{
    public static class ResourceHelper
    {
        public static GameObject LoadPrefab(string name, string assetPath)
        {
            var hasPath = !string.IsNullOrEmpty(assetPath);
            if (!hasPath) return null;
            var resourcePath = ResourceHelper.GetResourcePath(assetPath);
            var gameObject = Resources.Load<GameObject>(resourcePath);
            if (gameObject == null) gameObject = Resources.Load<GameObject>(resourcePath.ToLower());

            if (gameObject == null)
            {
                var e = string.Format("{0} not found. AbsPath={1}; ResourcePath={2}", name, assetPath, resourcePath);
                Debug.LogError(e);
            }

            return gameObject;
        }

        public static string GetResourcePath(string path)
        {
            var a = path.IndexOf("Resources", StringComparison.Ordinal);
            if (a < 0) return "";
            var b = path.IndexOf(Path.DirectorySeparatorChar.ToString(), a, StringComparison.Ordinal);
            if (b < 0) b = path.IndexOf(Path.AltDirectorySeparatorChar.ToString(), a, StringComparison.Ordinal);
            var c = path.Substring(b + 1);
            var resourcePath = c.Substring(0, c.IndexOf(".", StringComparison.Ordinal));
            return resourcePath;
        }

        private static string NameFromPath(string path, int extensionLength = 3)
        {
            path = path.Substring(path.LastIndexOf('/') + 1);
            return path.Substring(0, path.Length - extensionLength);
        }
    }
}