using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players;


namespace AccessoryForPlanetaryEmpire.Accessory.Movement
{
    public class SpeedModActive : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Movement";
        //public override void SetStaticDefaults()
        //{
        //    DisplayName.SetDefault("冲锋战舰动力推进装置");
        //    Tooltip.SetDefault("移动速度增加350%，但是防御力减少40%" +
        //        "\n世界吞噬者和克苏鲁之脑后" +
        //        "\n这似乎是安装在小型冲锋战舰上面的速度推进装置" +
        //        "\n但不知为何，装置的体型能够让你很轻松的佩戴" +
        //        "\n但由于你的速度过快，以至于撕裂了空间，你的盔甲开始变得松散");
        //}

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
            player.statDefense -= (int)(player.statDefense * 0.8f); // 防御力减少20%
            player.GetModPlayer<SpeedModActivePlayer>().speedModActiveEquipped = true;
            player.wingTime = float.PositiveInfinity; // Grants infinite flight
            player.moveSpeed += 0.30f; // Increases movement speed by 30%
            player.maxFallSpeed *= 1.30f; // Increases fall speed by 30%
            player.jumpSpeedBoost += 0.30f; // Increases jump speed by 30%
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Magiluminescence, 1);
            recipe.AddIngredient(ItemID.EmpressFlightBooster, 1);
            //recipe.AddIngredient(ItemID.FragmentNebula, 10);
            //recipe.AddIngredient(ItemID.FragmentSolar, 10);
            //recipe.AddIngredient(ItemID.FragmentVortex, 10);
            //recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

    }
}
