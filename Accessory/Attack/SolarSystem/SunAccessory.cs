using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem
{
    public class SunAccessory : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("太阳光环");
            //Tooltip.SetDefault("召唤太阳光环环绕玩家头顶");
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
            //var sunPlayer = player.GetModPlayer<SunPlayer>();
            //sunPlayer.hasSunAccessory = true;
            player.GetModPlayer<SunPlayer>().hasSunAccessory = true;

            if (hideVisual) // 关闭可见性
            {
                player.GetModPlayer<SunPlayer>().GuangHuan = false;
            }
            else // 开启可见性
            {
                player.GetModPlayer<SunPlayer>().GuangHuan = true;
            }
        }




        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.LavaBucket, 30); // 岩浆桶
            recipe.AddIngredient(ItemID.LivingFireBlock, 100); // 活火块
            recipe.AddIngredient(ItemID.Wire, 700); // 电线
            recipe.AddIngredient(ItemID.FragmentSolar, 50); // 太阳碎片
            //recipe.AddIngredient(ItemID.FragmentNebula, 10); // 星云碎片
            //recipe.AddIngredient(ItemID.FragmentVortex, 10); // 漩涡碎片
            //recipe.AddIngredient(ItemID.FragmentStardust, 10); // 星尘碎片
            recipe.AddTile(TileID.LunarCraftingStation); // 远古合成站
            recipe.Register();
        }






    }
}