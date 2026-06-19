using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players.Defense;


namespace AccessoryForPlanetaryEmpire.Accessory.Defense
{
    public class ShieldGeneratorOnDeployment : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense";

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
            player.statDefense += 8;
            player.GetModPlayer<ShieldGeneratorOnDeploymentPlayer>().hasShieldGeneratorOnDeployment = true;
            player.noKnockback = true;
            player.fireWalk = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.ObsidianShield, 1);
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

    }
}
