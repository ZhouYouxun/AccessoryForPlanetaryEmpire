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
using AccessoryForPlanetaryEmpire.Players;
using Terraria.GameInput;


namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem
{
    public class SaturnAccessory : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

        private int fireCooldown = 180; // 180帧即3秒

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Saturn Accessory");
            // Tooltip.SetDefault("Summons Saturn that throws returning rings to enemies.");
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
            player.GetModPlayer<SaturnPlayer>().hasSaturnAccessory = true;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.DirtBlock, 58); // 泥土
            recipe.AddIngredient(ItemID.MudBlock, 232); // 淤泥
            recipe.AddIngredient(ItemID.WeatherPain, 1); // 天候棒
            recipe.AddIngredient(ItemID.LightDisc, 1); // 光辉飞盘
            recipe.AddIngredient(ItemID.BouncingShield, 1); // 中士联盾
            recipe.AddTile(TileID.MythrilAnvil); // 秘银砧
            recipe.Register();
        }








    }
}