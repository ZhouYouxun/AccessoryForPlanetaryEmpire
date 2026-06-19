using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem
{
    public class EarthAccessory : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jupiter Accessory");
            // Tooltip.SetDefault("Summons Jupiter that deals collision damage and explodes.");
        }

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
            player.GetModPlayer<EarthPlayer>().hasEarthAccessory = true;
        }




        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.DontHurtCrittersBook, 1); // 小动物保护指南
            recipe.AddIngredient(ItemID.Moonglow, 3); // 月光草
            recipe.AddIngredient(ItemID.Daybloom, 3); // 日光花
            recipe.AddIngredient(ItemID.Vine, 3); // 藤蔓
            recipe.AddIngredient(ItemID.JungleSpores, 8); // 丛林孢子
            recipe.AddTile(TileID.Anvils); // 砧
            recipe.Register();
        }







    }
}
