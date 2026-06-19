using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Players.Movement;


namespace AccessoryForPlanetaryEmpire.Accessory.Dash
{
    public class PowerJetPack : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Dash";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("电能喷气背包");
            // Tooltip.SetDefault("按下特殊绑定键来进行大冲刺\n冷却时间为45秒");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 世纪之花~教徒：Cyan，价值25金
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PowerJetPackPlayer>().hasPowerJetPack = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.LaserDrill, 1); // 使用激光钻头
            recipe.AddIngredient(ItemID.CrystalShard, 100); // 与电相关的材料
            recipe.AddIngredient(ItemID.MartianConduitPlating, 200);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
