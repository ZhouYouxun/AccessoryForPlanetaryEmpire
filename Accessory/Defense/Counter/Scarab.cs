using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players.Defense.Counter;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.Counter
{
    public class Scarab : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.Counter";


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
            player.GetModPlayer<ScarabPlayer>().hasScarab = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.HerculesBeetle, 1); // 大力士甲虫
            recipe.AddIngredient(ItemID.ScarabBomb, 150); // 甲虫炸弹
            recipe.AddIngredient(ItemID.MagicMirror, 1); // 魔镜
            recipe.AddIngredient(ItemID.SoulofNight, 10); // 暗影之魂
            recipe.AddIngredient(ItemID.DarkShard, 2); // 暗影碎片
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}




