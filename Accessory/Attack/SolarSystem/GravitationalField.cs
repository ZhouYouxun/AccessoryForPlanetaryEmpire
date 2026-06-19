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
using AccessoryForPlanetaryEmpire.Projectiles;


namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem
{
    public class GravitationalField : ModItem, ILocalizedModType
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
            player.GetModPlayer<GravitationalFieldPlayer>().hasGravitationalField = true;  
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.IceBlock, 50); // 冰块
            recipe.AddIngredient(ItemID.Actuator, 10); // 引爆器
            recipe.AddTile(TileID.MythrilAnvil); // 秘银砧
            recipe.Register();
        }

    }
}