//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria.DataStructures;
//using Microsoft.Xna.Framework;
//using Terraria.Audio;
//using Microsoft.Xna.Framework.Input;
//using Terraria.Localization;
//using AccessoryForPlanetaryEmpire.Players;
//using AccessoryForPlanetaryEmpire.Global;
//using AccessoryForPlanetaryEmpire.Players.Defense;

//namespace AccessoryForPlanetaryEmpire.Accessory.Defense
//{
//    public class IntelligentFlyingShieldDetector : ModItem
//    {

//        public override void SetDefaults()
//        {
//            Item.width = 20;
//            Item.height = 20;
//            Item.accessory = true;
//            Item.useTime = 45;
//            Item.useAnimation = 45;
//            Item.consumable = true;
//            Item.maxStack = 1;
//            Item.rare = ItemRarityID.Pink;
//            Item.value = Item.buyPrice(0, 35, 0, 0);
//            Item.value = Item.sellPrice(0, 35, 0, 0);
//            Item.noMelee = true;
//            Item.channel = true;
//            Item.buffTime = 1800;
//        }


//        public override void UpdateAccessory(Player player, bool hideVisual)
//        {
//            player.GetModPlayer<IntelligentFlyingShieldDetectorPlayer>().hasIntelligentFlyingShieldDetector = true;
//        }


//        public override void AddRecipes()
//        {
//            Recipe recipe = CreateRecipe();
//            recipe.AddIngredient(ItemID.FleshKnuckles, 1);
//            recipe.AddTile(TileID.WorkBenches);
//            recipe.Register();
//        }
//    }
//}




