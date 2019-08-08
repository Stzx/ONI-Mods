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
