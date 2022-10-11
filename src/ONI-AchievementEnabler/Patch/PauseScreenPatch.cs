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

using AchievementEnablerLib.Model;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using static KButtonMenu;

namespace AchievementEnablerLib.Patch
{
    public class ModOptionsMenuScreen
    {
        internal static void Show(PauseScreen pauseScreen)
        {
            var screen = GameScreenManager.Instance.StartScreen(
                ScreenPrefabs.Instance.OptionsScreen.gameObject,
                pauseScreen.transform.parent.gameObject
            ) as OptionsMenuScreen;

            screen.SetButtons(new ButtonInfo[] {
                EnableModButtonInfo(screen, 0),
                new ButtonInfo(text: "Manual force check achievements", on_click: delegate { Game.Instance.Trigger((int) GameHashes.CheckColonyAchievements, null); }),
#if EXPERIMENTAL
                new ButtonInfo(text: "Disable sandbox mode and save backup first", on_click: delegate { OnSave(pauseScreen); screen.Deactivate(); }, is_enabled: SaveGame.Instance.sandboxEnabled),
#endif
#if DEBUG
                EnableDebugButtonInfo(screen, 2)
#endif
            });

            screen.Subscribe(screen.buttonObjects[0].GetHashCode(), delegate
            {
                var go = screen.buttonObjects[1];

                go.GetComponent<KButton>().isInteractable = !Configure.IsDisable;

                Array.ForEach(go.GetComponentsInChildren<LocText>(), comp => { comp.color = Configure.IsDisable ? new Color(0.5f, 0.5f, 0.5f) : new Color(1f, 1f, 1f); });
            });


            screen.Trigger(screen.buttonObjects[0].GetHashCode());
        }

        private static ButtonInfo EnableModButtonInfo(OptionsMenuScreen screen, uint infoIdx)
        {
            string GetText() => Configure.IsDisable ? "Mod disabled" : "Mod enabled";

            void OnClick()
            {
                Configure.Enable = Configure.IsDisable;

                var go = screen.buttonObjects[infoIdx];

                go.GetComponentInChildren<LocText>().text = GetText();

                screen.Trigger(go.GetHashCode());
            };

            return new ButtonInfo(text: GetText(), on_click: OnClick);
        }

        private static ButtonInfo EnableDebugButtonInfo(OptionsMenuScreen screen, uint infoIdx)
        {
            string GetText() => DebugHandler.enabled ? "Debug enabled" : "Debug disabled";

            void OnClick()
            {
                DebugHandler.SetDebugEnabled(!DebugHandler.enabled);

                screen.buttonObjects[infoIdx].GetComponentInChildren<LocText>().text = GetText();
            };

            return new ButtonInfo(text: GetText(), on_click: OnClick);
        }

#if EXPERIMENTAL
        private static void OnSave(PauseScreen pauseScreen)
        {
        }
#endif
    }

    [HarmonyPatch(typeof(PauseScreen), "OnPrefabInit", new Type[] { })]
    static class PauseScreenPatch
    {
        static void Postfix(PauseScreen __instance, ref IList<ButtonInfo> ___buttons)
        {
            ___buttons = new List<ButtonInfo>(___buttons).Append(new ButtonInfo(text: "Achievement Enabler", on_click: delegate { ModOptionsMenuScreen.Show(__instance); })).ToArray();

#if DEBUG
            Assets.instance.PrefabAssets.ForEach((i) => Debug.Log(i.name));
            Debug.Log($"[PauseScreenPatch] [{__instance.GetHashCode()}] Postfix");
#endif
        }
    }
}
