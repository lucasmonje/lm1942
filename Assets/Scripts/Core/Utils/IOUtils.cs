using System.IO;

namespace Core.Utils
{
    public static class IOUtils
    {
        public static string Load(string path)
        {
            if (!File.Exists(path)) return null;
            var reader = new StreamReader(path);
            var json = reader.ReadToEnd();
            reader.Close();
            return json;
        }
        
        public static void Save(string path, string str)
        {
            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var writer = new StreamWriter(fs))
                {
                    writer.Write(str);
                }
            }
        }
    }
}