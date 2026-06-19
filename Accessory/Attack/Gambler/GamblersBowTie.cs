using AccessoryForPlanetaryEmpire.Players.Attack.Gambler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Gambler
{
    public class GamblersBowTie : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Gambler";

        public override void SetStaticDefaults()
        {

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
            player.GetModPlayer<GamblersBowTiePlayer>().hasGamblersBowTie = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.LargeAmber, 1);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(ItemID.BlackThread, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
