using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using System.IO.Pipelines;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj
{
    public class AITrackingDevice : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.ModProj";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值15金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.value = Item.sellPrice(0, 15, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AITrackingDevicePlayer>().hasAITrackingDevice = true;
        }



        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.MechanicalWheelPiece, 1); // 机械boss的专家掉落
            recipe.AddIngredient(ItemID.MechanicalWagonPiece, 1);
            recipe.AddIngredient(ItemID.MechanicalBatteryPiece, 1);
            recipe.AddIngredient(ItemID.MechanicalLens, 1); // 机械晶状体
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
