using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;
using ReLogic.Peripherals.RGB;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    public class AedComponents : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";
        // Aed组件:
        // 每隔五秒生成一个红星在玩家周围恢复20生命值

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
            //PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();
            player.GetModPlayer<AedComponentsPlayer>().hasAEDComponent = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1); 
            recipe.AddIngredient(ItemID.DyeVat, 1); // 染缸
            recipe.AddIngredient(ItemID.HeartStatue, 1); // 爱心雕像
            //recipe.AddIngredient(ItemID.Timer5Second, 1); // 5秒计时器
            recipe.AddIngredient(ItemID.PinkGel, 5); // 粉色凝胶
            recipe.AddIngredient(ItemID.HoneyBucket, 1); // 蜂蜜桶
            recipe.AddIngredient(ItemID.SlimeGun, 1); // 史莱姆枪
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}




