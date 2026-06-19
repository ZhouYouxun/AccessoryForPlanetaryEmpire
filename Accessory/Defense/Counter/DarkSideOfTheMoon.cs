using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static AccessoryForPlanetaryEmpire.Global.PLGlobalItem;
using AccessoryForPlanetaryEmpire.Players.Defense.Counter;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.Counter
{
    public class DarkSideOfTheMoon : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.Counter";

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 教徒后：Red，价值35金
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 35, 0, 0);
            Item.value = Item.sellPrice(0, 35, 0, 0);

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.PortalGun);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DarkSideOfTheMoonPlayer>().DarkSideOfTheMoonEquipped = true;
        }






    }
}
