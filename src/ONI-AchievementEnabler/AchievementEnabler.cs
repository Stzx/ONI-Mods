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
using System.Collections.Generic;
using System.Linq;

using Harmony;

using KMod;

using ONI_AchievementEnabler.Config;

using UnityEngine;

namespace ONI_SandboxAchievementEnabler
{
    /// <summary>
    /// Remove obsolete mod name
    /// </summary>
    [HarmonyPatch(typeof(Manager), "MatchFootprint", new Type[2] { typeof(List<Label>), typeof(Content) })]
    internal class AchievementEnabler_Manager_MatchFootprint
    {
        private static void Prefix(List<Label> footprint)
        {
#if DEBUG
            Debug.Log($"=== AchievementEnabler Manager MatchFootprint [Prefix] ===");

            footprint.ForEach(item => Debug.Log($"=== {item.id} - {item.title} {item.version}"));
#endif
            // Remove old dll
            // Version: 1654370652

            try
            {
                footprint = footprint.Where(item => !item.title.Equals("Sandbox Achievement Enabler")).ToList();
            }
            catch { }
        }
    }

    [HarmonyPatch(typeof(ColonyAchievementTracker), "UnlockPlatformAchievement", new Type[1] { typeof(string) })]
    internal class AchievementEnabler_ColonyAchievementTracker_UnlockPlatformAchievement
    {
        private static bool isInstantBuildMode = false;

        private static bool isSandboxEnabled = false;

        private static bool isDebugEnabled = false;

        private static void Prefix()
        {
#if DEBUG
            Debug.Log($"=== AchievementEnabler ColonyAchievementTracker UnlockPlatformAchievement [Prefix] ===");
            Debug.Log($"=== [Prefix STATUS] isInstantBuildMode: {isInstantBuildMode}");
            Debug.Log($"=== [Prefix STATUS] DebugHandler.InstantBuildMode: {DebugHandler.InstantBuildMode}");
            Debug.Log($"=== [Prefix STATUS] isSandboxEnabled: {isSandboxEnabled}");
            Debug.Log($"=== [Prefix STATUS] SaveGame.SandboxEnabled: {SaveGame.Instance.sandboxEnabled}");
            Debug.Log($"=== [Prefix STATUS] isDebugEnabled: {isDebugEnabled}");
            Debug.Log($"=== [Prefix STATUS] Game.debugWasUsed: {Game.Instance.debugWasUsed}");
#endif
            if (Config.Args.isEnable)
            {
                if (isInstantBuildMode = DebugHandler.InstantBuildMode) DebugHandler.InstantBuildMode = false;
                if (isSandboxEnabled = SaveGame.Instance.sandboxEnabled) SaveGame.Instance.sandboxEnabled = false;
                if (isDebugEnabled = Game.Instance.debugWasUsed) Game.Instance.debugWasUsed = false;
            }

#if DEBUG
            Debug.Log($"=== [Prefix out] isEnable: {Config.Args.isEnable}");
            Debug.Log($"=== [Prefix out] DebugHandler.InstantBuildMode: {DebugHandler.InstantBuildMode}");
            Debug.Log($"=== [Prefix out] SaveGame.SandboxEnabled: {SaveGame.Instance.sandboxEnabled}");
            Debug.Log($"=== [Prefix out] Game.debugWasUsed: {Game.Instance.debugWasUsed}");
#endif
        }

        private static void Postfix()
        {
#if DEBUG
            Debug.Log($"=== AchievementEnabler ColonyAchievementTracker UnlockPlatformAchievement [Postfix] ===");
            Debug.Log($"=== [Postfix STATUS] isInstantBuildMode: {isInstantBuildMode}");
            Debug.Log($"=== [Postfix STATUS] DebugHandler.InstantBuildMode: {DebugHandler.InstantBuildMode}");
            Debug.Log($"=== [Postfix STATUS] isSandboxEnabled: {isSandboxEnabled}");
            Debug.Log($"=== [Postfix STATUS] SaveGame.Instance.sandboxEnabled: {SaveGame.Instance.sandboxEnabled}");
            Debug.Log($"=== [Postfix STATUS] isDebugEnabled: {isDebugEnabled}");
            Debug.Log($"=== [Postfix STATUS] Game.debugWasUsed: {Game.Instance.debugWasUsed}");
#endif

            if (isInstantBuildMode) DebugHandler.InstantBuildMode = isInstantBuildMode;
            if (isSandboxEnabled) SaveGame.Instance.sandboxEnabled = isSandboxEnabled;
            if (isDebugEnabled) Game.Instance.debugWasUsed = isDebugEnabled;

#if DEBUG
            Debug.Log($"=== [Postfix out] isEnable: {Config.Args.isEnable}");
            Debug.Log($"=== [Postfix out] DebugHandler.InstantBuildMode: {DebugHandler.InstantBuildMode}");
            Debug.Log($"=== [Postfix out] SaveGame.SandboxEnabled: {SaveGame.Instance.sandboxEnabled}");
            Debug.Log($"=== [Postfix out] Game.debugWasUsed: {Game.Instance.debugWasUsed}");
#endif
        }
    }

    [HarmonyPatch(typeof(RetiredColonyInfoScreen), "OnShow", new Type[1] { typeof(bool) })]
    internal class AchievementEnabler_RetiredColonyInfoScreen_OnShow
    {
        private static void Postfix(GameObject ___disabledPlatformUnlocks)
        {
#if DEBUG
            Debug.Log($"=== AchievementEnabler RetiredColonyInfoScreen OnShow [Postfix] ===");
#endif
            if (Config.Args.isEnable)
            {
                if (___disabledPlatformUnlocks.activeSelf)
                {
                    ___disabledPlatformUnlocks.GetComponent<HierarchyReferences>().GetReference("enabled").gameObject.SetActive(true);
                    ___disabledPlatformUnlocks.GetComponent<HierarchyReferences>().GetReference("disabled").gameObject.SetActive(false);
                }
            }

#if DEBUG
            Debug.Log($"=== [OnShow out] isEnable: {Config.Args.isEnable}");
            Debug.Log($"=== [OnShow out] SaveGame.SandboxEnabled: {SaveGame.Instance.sandboxEnabled}");
            Debug.Log($"=== [OnShow out] Game.debugWasUsed: {Game.Instance.debugWasUsed}");
#endif
        }
    }
}