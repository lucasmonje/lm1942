using Core.Utils;
using Core.Utils.Unity;
using UnityEngine;

namespace Core.Configurations
{
    public sealed class ConfigurationLoader
    {
        public const string Path = "Assets/Resources/levels.json";
        private static ConfigurationLoader instance;

        public Configuration Configuration { get; private set; }

        public static ConfigurationLoader GetInstance()
        {
            return instance ?? Initialize();
        }

        private static ConfigurationLoader Initialize()
        {
            return instance = new ConfigurationLoader();
        }

        public static void Reload()
        {
            Debug.Log("Reload " + Path);
            if (instance == null)
                Initialize();
            else
                instance.LoadAndParseJson();
        }

        private ConfigurationLoader()
        {
            var json = Application.isPlaying ? LoadJsonFromResources() : IOUtils.Load(Path);
            Configuration = string.IsNullOrEmpty(json)
                ? Configuration.Create()
                : ConfigurationParser.Parse(JsonUtility.FromJson(json, ConfigurationParser.GetDtoType()));
        }

        private void LoadAndParseJson()
        {
            var json = Application.isPlaying ? LoadJsonFromResources() : IOUtils.Load(Path);
            Configuration = string.IsNullOrEmpty(json)
                ? Configuration.Create()
                : ConfigurationParser.Parse(JsonUtility.FromJson(json, ConfigurationParser.GetDtoType()));
        }

        private static string LoadJsonFromResources()
        {
            var textAsset = Resources.Load<TextAsset>(ResourceHelper.GetResourcePath(Path));
            return textAsset != null && !string.IsNullOrEmpty(textAsset.text) ? textAsset.text : null;
        }
    }
}