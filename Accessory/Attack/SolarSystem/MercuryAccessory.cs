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
using AccessoryForPlanetaryEmpire.Buffs;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem
{
    public class MercuryAccessory : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

        public override void SetStaticDefaults()
        {
            //Tooltip.SetDefault("Grants Mercury's Blessing: 45% damage reduction. Loss of buff on hit.");
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
            player.GetModPlayer<MercuryPlayer>().hasMercuryAccessory = true;
            // player.GetModPlayer<MercuryPlayer>().hasTSSAccessory = true;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.StoneBlock, 24); // 岩石块
            recipe.AddIngredient(ItemID.StoneBlock, 39); // 岩石块
            recipe.AddIngredient(ItemID.WaterBucket, 10); // 水桶
            recipe.AddTile(TileID.Anvils); // 铁砧
            recipe.Register();
        }



    }
}
