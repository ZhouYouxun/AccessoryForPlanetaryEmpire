using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire
{
    internal class PLARecipes : ModSystem
    {
        public override void AddRecipeGroups()
        {
            // ----------------------------------------------------------
            // ⚙️ 规则说明：
            // RecipeGroup 用来创建 “任意XX锭” 这种可替换材料组
            // 用法示例： AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyCopperBar", 5);
            // ----------------------------------------------------------

            RecipeGroup group;

            // ==========================
            // 铜锭 + 锡锭
            // ==========================
            group = new RecipeGroup(
                () => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBar)}",
                new int[]
                {
                    ItemID.CopperBar,
                    ItemID.TinBar
                }
            );
            RecipeGroup.RegisterGroup("AccessoryForPlanetaryEmpire:AnyCopperBar", group);


            // ==========================
            // 铁锭 + 铅锭
            // ==========================
            group = new RecipeGroup(
                () => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.IronBar)}",
                new int[]
                {
                    ItemID.IronBar,
                    ItemID.LeadBar
                }
            );
            RecipeGroup.RegisterGroup("AccessoryForPlanetaryEmpire:AnyIronBar", group);


            // ==========================
            // 银锭 + 钨锭
            // ==========================
            group = new RecipeGroup(
                () => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SilverBar)}",
                new int[]
                {
                    ItemID.SilverBar,
                    ItemID.TungstenBar
                }
            );
            RecipeGroup.RegisterGroup("AccessoryForPlanetaryEmpire:AnySilverBar", group);

            // ==========================
            // 金锭 + 铂金锭
            // ==========================
            group = new RecipeGroup(
                () => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldBar)}",
                new int[]
                {
                    ItemID.GoldBar,
                    ItemID.PlatinumBar
                }
            );
            RecipeGroup.RegisterGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", group);

            // ==========================
            // 魔矿锭 + 血腥锭（恶魔/猩红）
            // ==========================
            group = new RecipeGroup(
                () => $"{Language.GetTextValue("LegacyMisc.37")} 恶魔 / 猩红锭",
                new int[]
                {
                    ItemID.DemoniteBar,
                    ItemID.CrimtaneBar
                }
            );
            RecipeGroup.RegisterGroup("AccessoryForPlanetaryEmpire:AnyEvilBar", group);

            // ==========================
            // 钴锭 + 钯金锭
            // ==========================
            group = new RecipeGroup(
                () => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CobaltBar)}",
                new int[]
                {
                    ItemID.CobaltBar,
                    ItemID.PalladiumBar
                }
            );
            RecipeGroup.RegisterGroup("AccessoryForPlanetaryEmpire:AnyCobaltBar", group);

            // ==========================
            // 秘银锭 + 山铜锭
            // ==========================
            group = new RecipeGroup(
                () => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.MythrilBar)}",
                new int[]
                {
                    ItemID.MythrilBar,
                    ItemID.OrichalcumBar
                }
            );
            RecipeGroup.RegisterGroup("AccessoryForPlanetaryEmpire:AnyMythrilBar", group);

            // ==========================
            // 精金锭 + 钛金锭
            // ==========================
            group = new RecipeGroup(
                () => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.AdamantiteBar)}",
                new int[]
                {
                    ItemID.AdamantiteBar,
                    ItemID.TitaniumBar
                }
            );
            RecipeGroup.RegisterGroup("AccessoryForPlanetaryEmpire:AnyAdamantiteBar", group);
        }
    }
}
