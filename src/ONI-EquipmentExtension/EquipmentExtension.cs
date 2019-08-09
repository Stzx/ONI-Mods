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
using System.Collections.Generic;

using Harmony;

using ONI_EquipmentExtension.Config;

namespace ONI_EquipmentExtension
{

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings", new Type[1] { typeof(List<Type>) })]
    internal class EquipmentExtension_GeneratedBuildings_LoadGeneratedBuildings
    {

        private static void Prefix()
        {
#if DEBUG
            Debug.Log("=== Equipment Extension GeneratedBuildings [LoadGeneratedBuildings] Prefix ===");
#endif

            #region Hand-knitted sweater 

            Strings.Add($"STRINGS.EQUIPMENT.PREFABS.{HandKnittedSweaterConfig.ID.ToUpper()}.NAME", HandKnittedSweaterConfig.NAME);
            Strings.Add($"STRINGS.EQUIPMENT.PREFABS.{HandKnittedSweaterConfig.ID.ToUpper()}.DESC", HandKnittedSweaterConfig.DESC);
            Strings.Add($"STRINGS.EQUIPMENT.PREFABS.{HandKnittedSweaterConfig.ID.ToUpper()}.GENERICNAME", HandKnittedSweaterConfig.GENERICNAME);
            Strings.Add($"STRINGS.EQUIPMENT.PREFABS.{HandKnittedSweaterConfig.ID.ToUpper()}.EFFECT", HandKnittedSweaterConfig.EFFECT);
            Strings.Add($"STRINGS.EQUIPMENT.PREFABS.{HandKnittedSweaterConfig.ID.ToUpper()}.RECIPE_DESC", HandKnittedSweaterConfig.RECIPE_DESC);

            #endregion
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize", new Type[0] { })]
    internal class EquipmentExtension_Db_Initialize
    {

        private static void Prefix(Db __instance)
        {
#if DEBUG
            Debug.Log("=== Equipment Extension Db [Initialize] Prefix ===");
#endif
        }
    }

    [HarmonyPatch(typeof(ClothingFabricatorConfig), "ConfigureRecipes", new Type[0] { })]
    internal class EquipmentExtension_ClothingFabricatorConfig_ConfigureRecipes
    {

        private static void Prefix()
        {
#if DEBUG
            Debug.Log("=== Equipment Extension ClothingFabricatorConfig [ConfigureRecipes] Prefix ===");
#endif

            var fabricator = ClothingFabricatorConfig.ID;

            #region Configure Recipes Hand-knitted sweater

            HandKnittedSweaterConfig.ConfigureRecipes(fabricator);

            #endregion

        }
    }

    [HarmonyPatch(typeof(SuitFabricatorConfig), "ConfigureRecipes", new Type[0] { })]
    internal class EquipmentExtension__SuitFabricatorConfig_ConfigureRecipes
    {

        private static void Prefix()
        {
#if DEBUG
            Debug.Log("=== Equipment Extension SuitFabricatorConfig [ConfigureRecipes] Prefix ===");
#endif
        }
    }

    [HarmonyPatch(typeof(GeneratedEquipment), "LoadGeneratedEquipment", new Type[0] { })]
    internal class EquipmentExtension_GeneratedEquipment_LoadGeneratedEquipment
    {

        private static void Prefix()
        {
#if DEBUG
            Debug.Log("=== Equipment Extension GeneratedEquipment [LoadGeneratedEquipment] Prefix ===");
#endif

            EquipmentConfigManager.Instance.RegisterEquipment(new HandKnittedSweaterConfig());
        }
    }
}
