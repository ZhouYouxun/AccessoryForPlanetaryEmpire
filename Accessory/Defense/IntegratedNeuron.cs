using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Players.Defense;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense
{
    public class IntegratedNeuron : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense";
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
            player.GetModPlayer<IntegratedNeuronPlayer>().hasIntegratedNeuron = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", 7);
            recipe.AddIngredient(ItemID.Wire, 7);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
