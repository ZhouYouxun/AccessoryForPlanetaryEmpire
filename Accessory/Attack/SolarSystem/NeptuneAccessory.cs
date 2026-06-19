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
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using Terraria.Audio;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem
{
    public class NeptuneAccessory : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

        private int fireCooldown = 0;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Neptune Accessory");
            // Tooltip.SetDefault("Harness the power of Neptune.");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 世纪之花~教徒：Cyan，价值35金
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 35, 0, 0);
            Item.value = Item.sellPrice(0, 35, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<NeptunePlayer>().hasNeptuneAccessory = true;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.Coral, 10); // 珊瑚
            recipe.AddIngredient(ItemID.Trident, 1); // 三叉戟
            recipe.AddIngredient(ItemID.UnholyTrident, 1); // 邪恶三叉戟
            recipe.AddIngredient(ItemID.TitaniumTrident, 1); // 钛金三叉戟
            recipe.AddIngredient(ItemID.InfernoFork, 1); // 地狱火三叉戟
            recipe.AddTile(TileID.MythrilAnvil); // 秘银砧
            recipe.Register();
        }

    }
}
