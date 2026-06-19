using AccessoryForPlanetaryEmpire.Players.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Dash
{
    public class SweetheartCloak : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Dash";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sweetheart Cloak");
            // Tooltip.SetDefault("Double-tap a direction to dash.\nChance to become invincible while dashing.");
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
            if (hideVisual)
            {
                player.GetModPlayer<SweetheartCloakPlayer>().hasSweetheartCloakAD = true;
                player.GetModPlayer<SweetheartCloakPlayer>().hasSweetheartCloak = false;
            }
            else
            {
                player.GetModPlayer<SweetheartCloakPlayer>().hasSweetheartCloakAD = false;
                player.GetModPlayer<SweetheartCloakPlayer>().hasSweetheartCloak = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.EoCShield, 1);
            recipe.AddIngredient(ItemID.BeeWax, 10);
            recipe.AddIngredient(ItemID.BottledHoney, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}