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

using System;
using System.IO;
using System.Text;

using Harmony;

using Newtonsoft.Json;

namespace ONI_AchievementEnabler.Config
{
    internal static class Config
    {
        internal class ModArgs
        {
            [JsonProperty]
            public string ver;

            [JsonProperty]
            public bool isEnable;

            public override string ToString()
            {
                return $"ver: {ver} > isEnable: {isEnable}";
            }
        }

        private static readonly string _modDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private const string _fileName = "Config.json";

        private static readonly string _fullPath = $"{_modDir}\\{_fileName}";

        private static readonly FileSystemWatcher _configWatcher = new FileSystemWatcher();

        public static ModArgs Args = new ModArgs()
        {
            ver = "1.0",
            isEnable = true
        };

        static Config()
        {
            _configWatcher.Path = _modDir;
            _configWatcher.Filter = _fileName;

            _configWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _configWatcher.Changed += OnChanged;

            _configWatcher.EnableRaisingEvents = true;
        }

        internal static void Init()
        {
            if (File.Exists(_fullPath))
                Read();
            else
                Writer();
        }

        private static void Read()
        {
            try
            {
                Args = JsonConvert.DeserializeObject<ModArgs>(File.ReadAllText(_fullPath));
            }
            catch
            {
                Debug.LogWarning($"=== [Achievement Enabler] => Configuration file read failed. ===");
            }
        }

        private static void Writer()
        {
            try
            {
                var jsonBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Args));

                using (var configFile = File.Create(_fullPath))
                {
                    configFile.Write(jsonBytes, 0, jsonBytes.Length);
                }
            }
            catch
            {
                Debug.LogWarning($"=== [Achievement Enabler] => Configuration file write failed. ===");
            }
        }

        public static void OnChanged(object source, FileSystemEventArgs e)
        {
#if DEBUG
            Debug.Log($"=== [Achievement Enabler] => Args: {Args.ToString()} ===");
            Debug.Log($"=== [Achievement Enabler] => OnChanged ===");
#endif

            if (File.Exists(_fullPath))
            {
                Read();
            }
        }
    }

    [HarmonyPatch(typeof(Game), "OnPrefabInit", new Type[0] { })]
    internal class AchievementEnabler_Game_OnPrefabInit
    {
        private static void Prefix()
        {
            Config.Init();
        }
    }
}