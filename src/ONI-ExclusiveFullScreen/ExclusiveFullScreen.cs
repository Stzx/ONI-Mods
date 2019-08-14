/**
 * Copyright (c) 2019 Silence Tai
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

using UnityEngine;

using Internal = UnityEngine.Internal;

namespace ONI_ExclusiveFullScreen
{

    [HarmonyPatch(typeof(DistributionPlatform), "Initialize", new Type[0])]
    internal static class HarmonyPatchDistributionPlatform
    {
        private static bool Prefix()
        {
            Screen.fullScreenMode = (Screen.fullScreen) ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;

            return true;
        }
    }

    [HarmonyPatch(typeof(Screen), "SetResolution", new[] { typeof(int), typeof(int), typeof(bool), typeof(int) })]
    internal static class HarmonyPatchScreen
    {
        private static bool Prefix(Screen __instance, int width, int height, bool fullscreen, [Internal.DefaultValue("0")] int preferredRefreshRate)
        {
            Screen.SetResolution(width, height, (fullscreen) ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed, preferredRefreshRate);

            return false;
        }
    }
}
