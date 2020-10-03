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
using UnityEngine;

namespace ONI_AchievementEnabler.Patch
{
    [HarmonyPatch(typeof(RetiredColonyInfoScreen), "OnShow", new Type[1] { typeof(bool) })]
    public class AchievementEnabler_RetiredColonyInfoScreen_OnShow
    {
        public static void Postfix(GameObject ___disabledPlatformUnlocks)
        {
#if DEBUG
            Debug.Log("[Achievement Enabler] [Postfix] (RetiredColonyInfoScreen > OnShow)");
#endif

            if (Config.Args.isEnable && SaveGame.Instance != null)
            {
                ___disabledPlatformUnlocks.GetComponent<HierarchyReferences>().GetReference("enabled").gameObject.SetActive(true);
                ___disabledPlatformUnlocks.GetComponent<HierarchyReferences>().GetReference("disabled").gameObject.SetActive(false);
            }
        }
    }
}