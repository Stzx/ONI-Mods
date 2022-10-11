/**
 * Copyright (c) 2019-2022 Silence Tai
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

using KMod;

using Newtonsoft.Json;

using System;
using System.ComponentModel;
using System.IO;
using System.Text;


namespace AchievementEnablerLib.Model
{
    public static class Configure
    {
        internal class Options
        {
            [DefaultValue(526233u)]
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
            public uint ver;

            [DefaultValue(true)]
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
            public bool enable;

        }

        public static bool IsDisable { get => !options.enable || SaveGame.Instance == null; }

        public static bool Enable { set { options.enable = value; Sync(); } }

        public static void Load(UserMod2 mod)
        {
            Debug.Log("Achievement Enabler loading...");

            LoadFile(mod.mod);

            Debug.LogFormat("Achievement Enabler configure: {0}", new[] { JsonConvert.SerializeObject(options) });

            Debug.Log("Achievement Enabler loaded");
        }

        private static void LoadFile(Mod mod)
        {
            var contentDir = new DirectoryInfo(mod.ContentPath);

            // C:\Users\<user>\Documents\Klei\OxygenNotIncluded\mods
            LoadConfigureFile(contentDir.Parent.Parent, $"{contentDir.Name}.json");
        }

        private static void LoadConfigureFile(DirectoryInfo parent, string fileName)
        {
            var cfgDir = new DirectoryInfo(Path.Combine(parent.FullName, "Config"));

            if (!cfgDir.Exists) parent.CreateSubdirectory("Config");

            fs = new FileStream(Path.Combine(cfgDir.FullName, fileName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

            if (fs.Length != 0)
            {
                var sr = new StreamReader(fs);
                try
                {
                    options = JsonConvert.DeserializeObject<Options>(sr.ReadToEnd());

                    Compatible();

                    return;
                }
                catch (Exception)
                {
                    Debug.Log("Configure file failed to load, will reset.");
                }
            }

            Debug.Log("Achievement Enabler configure does not exist");

            options = JsonConvert.DeserializeObject<Options>("{}");
            Sync();
        }

        private static void Sync()
        {
            var json = JsonConvert.SerializeObject(options, Formatting.None);
            var bytes = Encoding.UTF8.GetBytes(json);

            Debug.LogFormat("Achievement Enabler configure sync.. {0}", new[] { json });

            try
            {
                fs.SetLength(0);
                fs.Write(bytes, 0, bytes.Length);
                fs.Flush();
            }
            catch (Exception ex)
            {
                Debug.LogWarningFormat("Achievement Enabler configure sync faild. {0}", new[] { ex.ToString() });
            }
        }

        private static void Compatible()
        {

        }

        private static Options options;

        private static FileStream fs;

        private static StreamWriter sw;

    }
}
