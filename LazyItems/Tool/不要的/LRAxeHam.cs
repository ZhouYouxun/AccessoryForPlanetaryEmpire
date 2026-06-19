//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
//{
//    public class LRAxeHam : ModItem
//    {

//        //public override void SetStaticDefaults()
//        //{
//        //    DisplayName.SetDefault("行星崩解之锤斧");
//        //    Tooltip.SetDefault("这把锤斧蕴含着无穷无尽的行星之力");
//        //}


//        public override void SetDefaults()
//        {
//            Item.damage = 10;
//            Item.DamageType = DamageClass.Melee;
//            Item.width = 49;
//            Item.height = 49;
//            Item.useTime = 5;
//            Item.useAnimation = 5;
//            Item.useStyle = ItemUseStyleID.Swing;
//            Item.knockBack = 6;
//            Item.value = Item.buyPrice(gold: 40); // Buy this item for one gold - change gold to any coin and change the value to any number <= 100
//            Item.rare = ItemRarityID.Green;
//            Item.UseSound = SoundID.Item1;
//            Item.autoReuse = true;
//            Item.scale = 2;
//            Item.axe = 100;
//            Item.hammer = 220;
//            Item.tileBoost += 12;
//        }


//        public override void AddRecipes()
//        {
//            Recipe recipe = CreateRecipe();
//            recipe.AddIngredient(ItemID.CopperAxe, 1);
//            recipe.AddTile(TileID.WorkBenches);
//            recipe.Register();
//        }
//    }
//}