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
    public class MarsAccessory : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

        private int fireCooldown = 180; // 180帧即3秒

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mars Accessory");
            // Tooltip.SetDefault("Every 3 seconds, fire three Flamelash projectiles.");
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
            player.GetModPlayer<MarsPlayer>().hasMarsAccessory = true;

        }





        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.Hellstone, 33); // 狱岩锭
            recipe.AddIngredient(ItemID.AshBlock, 96); // 灰烬块
            recipe.AddIngredient(ItemID.FieryGreatsword, 1); // 火山大剑
            recipe.AddIngredient(ItemID.MoltenFury, 1); // 熔火之怒弓
            recipe.AddTile(TileID.Anvils); // 铁砧
            recipe.Register();
        }



    }
}
