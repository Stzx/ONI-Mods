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

using Harmony;
using ONI_AchievementEnabler.Model;

namespace ONI_AchievementEnabler.Patch
{
    [HarmonyPatch(typeof(ColonyAchievementTracker), "UnlockPlatformAchievement", new Type[1] { typeof(string) })]
    public class AchievementEnabler_ColonyAchievementTracker_UnlockPlatformAchievement
    {
        private static bool isInstantBuildMode = false;

        private static bool isSandboxEnabled = false;

        private static bool isDebugEnabled = false;

        public static void Prefix()
        {
#if DEBUG
            Debug.Log($"[Achievement Enabler] [Prefix] [In] (ColonyAchievementTracker > UnlockPlatformAchievement)");
            LogState();
#endif

            if (Config.Args.isEnable)
            {
                if (isInstantBuildMode = DebugHandler.InstantBuildMode) DebugHandler.InstantBuildMode = false;
                if (isSandboxEnabled = SaveGame.Instance.sandboxEnabled) SaveGame.Instance.sandboxEnabled = false;
                if (isDebugEnabled = Game.Instance.debugWasUsed) Game.Instance.debugWasUsed = false;
            }

#if DEBUG
            Debug.Log($"[Achievement Enabler] [Prefix] [Out] (ColonyAchievementTracker > UnlockPlatformAchievement)");
            LogState();
#endif
        }

        public static void Postfix()
        {
#if DEBUG
            Debug.Log($"[Achievement Enabler] [Prefix] [In] (ColonyAchievementTracker > UnlockPlatformAchievement)");
            LogState();
#endif

            if (Config.Args.isEnable)
            {
                if (isInstantBuildMode) DebugHandler.InstantBuildMode = isInstantBuildMode;
                if (isSandboxEnabled) SaveGame.Instance.sandboxEnabled = isSandboxEnabled;
                if (isDebugEnabled) Game.Instance.debugWasUsed = isDebugEnabled;
            }

#if DEBUG
            Debug.Log($"[Achievement Enabler] [Prefix] [Out] (ColonyAchievementTracker > UnlockPlatformAchievement)");
            LogState();
#endif
        }

#if DEBUG
        private static void LogState()
        {
            Debug.Log($"[Achievement Enabler] (isInstantBuildMode > {isInstantBuildMode})");
            Debug.Log($"[Achievement Enabler] (DebugHandler.InstantBuildMode : {DebugHandler.InstantBuildMode})");
            Debug.Log($"[Achievement Enabler] (isSandboxEnabled > {isSandboxEnabled})");
            Debug.Log($"[Achievement Enabler] (SaveGame.Instance.sandboxEnabled > {SaveGame.Instance.sandboxEnabled})");
            Debug.Log($"[Achievement Enabler] (isDebugEnabled > {isDebugEnabled})");
            Debug.Log($"[Achievement Enabler] (Game.debugWasUsed > {Game.Instance.debugWasUsed})");
        }
#endif
    }
}