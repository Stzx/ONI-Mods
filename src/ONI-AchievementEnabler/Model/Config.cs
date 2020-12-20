/**
 * Copyright (c) 2019-2020 Silence Tai
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Harmony;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Text;

namespace ONI_AchievementEnabler.Model
{
    internal static class Config
    {
        internal class ModArgs
        {
            [JsonProperty]
            public string ver = "1.1";

            [JsonProperty]
            public bool isEnable = true;

#if EXPERIMENTAL
            public bool overwriteArchiveSandbox = true;
#endif
        }

        private static readonly string _modDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private const string _fileName = "Config.json";

        private static readonly string _fullPath = $"{_modDir}{Path.DirectorySeparatorChar}{_fileName}";

        private static readonly FileSystemWatcher _watcher = new FileSystemWatcher();

        public static bool isInit = false;

        public static ModArgs Args = new ModArgs();

        internal static void Init()
        {
            Debug.Log("[Achievement Enabler] Initializing...");

            if (File.Exists(_fullPath)) { Read(); } else { Writer(); }

            _watcher.Path = _modDir;
            _watcher.Filter = _fileName;

            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.Changed += OnChanged;

            _watcher.EnableRaisingEvents = true;

            isInit = true;
        }

        private static void Read()
        {
            try
            {
                var config = File.ReadAllText(_fullPath);
                Args = JsonConvert.DeserializeObject<ModArgs>(config);

                Debug.LogFormat("[Achievement Enabler Configuration file loaded successfully. ({0})", new[] { config });
            }
            catch (Exception e)
            {
                Debug.LogWarningFormat("[Achievement Enabler Configuration file parsing failed. (Default configuration will be used)\t({0})", new[] { e.Message });
            }
        }

        private static void Writer()
        {
            try
            {
                var config = JsonConvert.SerializeObject(Args);
                var jsonBytes = Encoding.UTF8.GetBytes(config);

                using (var configFile = File.Create(_fullPath))
                {
                    configFile.Write(jsonBytes, 0, jsonBytes.Length);
                }

                Debug.LogFormat("[Achievement Enabler Configuration file saved successfully. ({0})", new[] { config });
            }
            catch (Exception e)
            {
                Debug.LogWarningFormat("[Achievement Enabler Unable to save configuration. ({0})", new[] { e.Message });
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Debug.LogFormat("[Achievement Enabler Trigger configuration reload.");

            if (File.Exists(_fullPath))
            {
                Read();
            }
        }
    }

    [HarmonyPatch(typeof(Game), "OnPrefabInit", new Type[0] { })]
    public class AchievementEnabler_Game_OnPrefabInit
    {
        public static void Prefix()
        {
            if (!Config.isInit)
            {
                Config.Init();
            }
        }
    }
}