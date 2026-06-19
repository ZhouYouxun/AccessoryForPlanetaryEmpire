using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj
{
    public class GlitchCore : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.ModProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glitch Core");
            //Tooltip.SetDefault("Causes projectiles to malfunction upon hitting an enemy.");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 10);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlitchCorePlayer>().hasGlitchCore = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.ZapinatorGray, 1); // 灰冲击枪
            recipe.AddIngredient(ItemID.ZapinatorOrange, 1); // 橙冲击枪
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}