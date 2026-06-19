using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players.Defense;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyEMPs.OtherEMP
{
    public class PeriodicBarrierBooster : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyEMPs.OtherEMP";
        //public override void SetStaticDefaults()
        //{
        //    DisplayName.SetDefault("周期性屏障生成器");
        //    Tooltip.SetDefault("每隔20秒钟提供5秒钟的无敌，并播放磨刀台的声音" +
        //        "\n任意Boss前" +
        //        "\n由帝国冲锋军所装备的制式防御性装备，但是在使用期间可能会让使用者出现神志不清的副作用");
        //}

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式前：Orange，价值15金
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.value = Item.sellPrice(0, 15, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 8;
            player.GetModPlayer<PeriodicBarrierBoosterPlayer>().hasPeriodicBarrierBooster = true;



        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Shackle, 1); // 镣铐
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 10); // 铁定
            recipe.AddIngredient(ItemID.Chain, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

