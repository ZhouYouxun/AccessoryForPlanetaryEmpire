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
    public class PlutoAccessory : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pluto Accessory");
            // Tooltip.SetDefault("Summons Pluto that blocks enemy projectiles.");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 教徒后：Red，价值55金
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 55, 0, 0);
            Item.value = Item.sellPrice(0, 55, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PlutoPlayer>().hasPlutoAccessory = true;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ModContent.ItemType<Accessory.Defense.AsteroidBelt>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.LihzahrdBrick, 50); // 神庙砖块
            recipe.AddIngredient(ItemID.FragmentSolar, 10); // 日耀碎片
            recipe.AddTile(TileID.MythrilAnvil); // 秘银砧
            recipe.Register();
        }

    }
}
