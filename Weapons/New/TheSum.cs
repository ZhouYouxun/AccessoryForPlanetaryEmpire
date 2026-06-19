using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Weapons.New
{
    internal class TheSum : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "NewWeapons";

        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<TheSumWhip>(), 20, 2, 4);

            Item.shootSpeed = 4;
            Item.damage = 30;
            Item.knockBack = 2;
            Item.DamageType = DamageClass.SummonMeleeSpeed;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 50, 0);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 30;
            Item.useTime = 30;

            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override bool MeleePrefix()
        {
            return true;
        }

        public override void AddRecipes()
        {
            //Recipe recipe = CreateRecipe();
            //recipe.AddIngredient(ItemID.Wood, 25);
            //recipe.AddIngredient(ItemID.StoneBlock, 10);
            //recipe.AddTile(TileID.WorkBenches);
            //recipe.Register();
        }

    }
}
