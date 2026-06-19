using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players;
using Humanizer;
using static System.Collections.Specialized.BitVector32;
using AccessoryForPlanetaryEmpire.Players.Attack.Annihilation;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Annihilation
{
    public class HeroDamageDevice : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Annihilation";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式前：Orange，价值5金
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.value = Item.sellPrice(0, 20, 0, 0);

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<HeroDamageDevicePlayer>().HasHeroDamageDevice = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.SharpeningStation, 1); // 利器站
            //recipe.AddIngredient(ItemID.PlatinumCrown, 1); // 王冠
            //recipe.AddIngredient(ItemID.GoldCrown, 1); // 王冠
            recipe.AddIngredient(ItemID.Sake, 5); // 清酒
            recipe.AddIngredient(ItemID.IceBlade, 1); // 冰雪刃
            recipe.AddIngredient(ItemID.Katana, 1); // 武士刀
            recipe.AddIngredient(ItemID.DyeTradersScimitar, 1);  // 染料弯刀
            recipe.AddIngredient(ItemID.FalconBlade, 1); // 猎鹰刀
            recipe.AddIngredient(ItemID.PlatinumBroadsword, 1); // 铂金宽刀
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
