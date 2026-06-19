using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using AccessoryForPlanetaryEmpire.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Buffs;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem
{
    public class VenusAccessory : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

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
            player.GetModPlayer<VenusPlayer>().hasVenusAccessory = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.Cloud, 60); // 云
            recipe.AddIngredient(ItemID.SunplateBlock, 52); // 日光板
            recipe.AddIngredient(ItemID.LavaBucket, 5); // 熔岩桶
            recipe.AddIngredient(ItemID.FallenStar, 5); // 5个陨落之星
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", 10);
            recipe.AddIngredient(ItemID.GuideVoodooDoll, 1);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }


    }
}