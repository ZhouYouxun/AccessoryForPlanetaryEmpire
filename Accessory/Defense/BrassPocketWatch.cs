using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire.Players.Defense;


namespace AccessoryForPlanetaryEmpire.Accessory.Defense
{
    public class BrassPocketWatch : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值25金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);
        }
        
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.WarriorEmblem);
            recipe.AddIngredient(ItemID.RangerEmblem);
            recipe.AddIngredient(ItemID.SorcererEmblem);
            recipe.AddIngredient(ItemID.SummonerEmblem);
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", 25);

            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Generic) += 10f;
            player.GetDamage(DamageClass.Generic) += 0.25f;
            player.GetModPlayer<BrassPocketWatchPlayer>().hasBrassPocketWatch = true;
        }




    }
}