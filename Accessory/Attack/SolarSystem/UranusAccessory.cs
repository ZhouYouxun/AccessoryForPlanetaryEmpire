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
using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem
{
    public class UranusAccessory : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Uranus Accessory");
            // Tooltip.SetDefault("Summons Uranus that slows and freezes enemies.");
        }

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
            player.GetModPlayer<UranusPlayer>().hasUranusAccessory = true;

            if (hideVisual) // 关闭可见性
            {
                player.GetModPlayer<UranusPlayer>().GuangHuan = false;
            }
            else // 开启可见性
            {
                player.GetModPlayer<UranusPlayer>().GuangHuan = true;
            }

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.FrostHelmet, 1);
            recipe.AddIngredient(ItemID.FrostBreastplate, 1);
            recipe.AddIngredient(ItemID.FrostLeggings, 1); // 寒霜套装
            recipe.AddIngredient(ItemID.Cloud, 30); // 云块
            recipe.AddIngredient(ItemID.RainCloud, 10); // 雨云块
            recipe.AddTile(TileID.MythrilAnvil); // 铁砧
            recipe.Register();
        }

    }
}
