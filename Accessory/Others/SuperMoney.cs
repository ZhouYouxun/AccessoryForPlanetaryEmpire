using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;


namespace AccessoryForPlanetaryEmpire.Accessory.Others
{
    public class SuperMoney : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Others";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 特殊价值物品：Expert
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(11, 45, 14, 0);
            Item.value = Item.sellPrice(11, 45, 14, 0);
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 80); // 用一些垃圾做一个很值钱的礼物盒
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", 1);
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
