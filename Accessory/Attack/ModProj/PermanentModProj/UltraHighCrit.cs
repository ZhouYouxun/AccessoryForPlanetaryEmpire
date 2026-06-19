using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using System;


namespace AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj.PermanentModProj
{
    public class UltraHighCrit : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.ModProj.PermanentModProj";

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 世纪之花~教徒：Cyan，价值25金
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense -= (int)(player.statDefense * 0.40f);
            // GetCritChance是所有伤害类型的暴击率意思
            player.GetCritChance(DamageClass.Generic) += 100f;
            if (!hideVisual) // 这个是饰品右上角的小眼睛的隐藏功能
            {
                // 当不隐藏该视频的时候将会生效：
                player.GetModPlayer<PlanetaryPlayer>().UltraHighCrit = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.EyeoftheGolem, 3); // 石巨人之眼，要三个
            recipe.AddIngredient(ItemID.LargeAmethyst, 1); // 6种大宝石
            recipe.AddIngredient(ItemID.LargeTopaz, 1);
            recipe.AddIngredient(ItemID.LargeSapphire, 1);
            recipe.AddIngredient(ItemID.LargeEmerald, 1);
            recipe.AddIngredient(ItemID.LargeRuby, 1);
            recipe.AddIngredient(ItemID.LargeDiamond, 1);
            recipe.AddTile(TileID.LihzahrdFurnace); // 丛林蜥蜴熔炉
            recipe.Register();
        }
    }
}
