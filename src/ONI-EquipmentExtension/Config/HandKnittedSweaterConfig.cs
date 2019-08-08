using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Klei.AI;

using STRINGS;

using UnityEngine;

namespace ONI_EquipmentExtension.Config
{
    internal class HandKnittedSweaterConfig : IEquipmentConfig
    {
        public const string ID = "Hand_Knitted_Sweater";

        #region STRINGS

        // <link=""HANDKNITTEDSWEATER"">手织毛衣</link>
        public static string NAME        = UI.FormatAsLink("手织毛衣", ID.ToUpper());

        public static string DESC        = "妈妈织的毛衣，很暖和。";

        public static string GENERICNAME = "服装";

        public static string EFFECT      = @"增加保暖效果，让穿戴者在<link=""heat"">寒冷</link>环境中保持体温。";

        public static string RECIPE_DESC = EFFECT;

        #endregion

        #region Def

        public static float FABTIME                                     = 45;

        public static string SLOT                                       = TUNING.EQUIPMENT.CLOTHING.SLOT;

        public static int MASS                                          = 4;

        public static string ANIM                                       = TUNING.EQUIPMENT.VESTS.WARM_VEST_ICON0;

        public static string SNAP_ON                                    = TUNING.EQUIPMENT.VESTS.SNAPON0;

        public static string BUILD_OVERRIDE                             = TUNING.EQUIPMENT.VESTS.WARM_VEST_ANIM0;

        public static string SNAP_ON1                                   = TUNING.EQUIPMENT.VESTS.SNAPON1;

        public static readonly ClothingWearer.ClothingInfo clothingInfo = new ClothingWearer.ClothingInfo(NAME, 12, 1f, -1.25f);

        #endregion

        public static ComplexRecipe recipe;

        [CompilerGenerated]
        private static Action<Equippable> ActionEq;

        public EquipmentDef CreateEquipmentDef()
        {
            var attributeModifiers = new List<AttributeModifier>();

            #region Def Show Effect

            var temperature        = $"{DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(clothingInfo.conductivityMod)}";
            var temperatureTooltip = temperature;

            var decor              = $"{DUPLICANTS.ATTRIBUTES.DECOR.NAME}: {clothingInfo.decorMod}";
            var decorTooltip       = decor;

            #endregion

            Descriptor temperatureDesc = new Descriptor(temperature, temperatureTooltip, Descriptor.DescriptorType.Effect, false);
            Descriptor decorDesc = new Descriptor(decor, decorTooltip, Descriptor.DescriptorType.Effect, false);

            var def = EquipmentTemplates.CreateEquipmentDef(ID, SLOT, SimHashes.Carbon, MASS, ANIM, SNAP_ON, BUILD_OVERRIDE, 4, attributeModifiers, SNAP_ON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, null, null);
                def.additionalDescriptors.Add(temperatureDesc);
                def.additionalDescriptors.Add(decorDesc);
                def.OnEquipCallBack = delegate (Equippable eq) { OnEquip(eq, clothingInfo); };
                def.OnUnequipCallBack = ActionEq ?? (ActionEq = new Action<Equippable>(OnUnequip));
                def.RecipeDescription = RECIPE_DESC;

            return def;
        }

        public static void OnEquip(Equippable eq, ClothingWearer.ClothingInfo clothingInfo)
        {
            CoolVestConfig.OnEquipVest(eq, clothingInfo);
        }

        public static void OnUnequip(Equippable eq)
        {
            CoolVestConfig.OnUnequipVest(eq);
        }

        public void DoPostConfigure(GameObject go)
        {
            Setup(go);

            go.GetComponent<KPrefabID>().AddTag(GameTags.PedestalDisplayable, false);
        }

        public static void Setup(GameObject go)
        {
            var equippable = go.GetComponent<Equippable>() ?? go.AddComponent<Equippable>();
            equippable.SetQuality(QualityLevel.Good);

            go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
            go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
        }

        public static void ConfigureRecipes(string fabricator)
        {
            var inputs = new ComplexRecipe.RecipeElement[]
            {
                new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 3f),
            };
            var outputs = new ComplexRecipe.RecipeElement[]
            {
                new ComplexRecipe.RecipeElement(ID.ToTag(), 1f),
            };

            var id = ComplexRecipeManager.MakeRecipeID(fabricator, inputs, outputs);

            recipe = new ComplexRecipe(id, inputs, outputs)
            {
                time         = FABTIME,
                description  = RECIPE_DESC,
                nameDisplay  = ComplexRecipe.RecipeNameDisplay.Result,
                fabricators  = new List<Tag> { fabricator },
                sortOrder    = 1,
                requiredTech = Db.Get().TechItems.suitsOverlay.parentTech.Id
            };
        }
    }
}
