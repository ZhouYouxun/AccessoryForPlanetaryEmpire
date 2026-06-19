using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using Humanizer;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Global;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    public class PainEditor : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";
        public bool HasPainEditor { get; private set; }

        // 疼痛编辑器:
        // 将单次受到的伤害限制在(当前)最大生命值的20%

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值25金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLPlayer>().HasPainEditor = true;
            // Main.NewText($"已经佩戴了疼痛编辑器，HasPainEditor为： {HasPainEditor}");
        }





        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.GlassKiln, 1); // 玻璃窑
            recipe.AddIngredient(ItemID.Wire, 150); // 电线
            recipe.AddIngredient(ItemID.WirePipe, 5); // 集线盒
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 3); // 沙暴核心
            recipe.AddIngredient(ItemID.CrossNecklace, 1); // 十字项链
            //recipe.AddIngredient(ItemID.SoulofLight, 10); // 光明之魂
            //recipe.AddIngredient(ItemID.LightShard, 5); // 光明碎片
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
