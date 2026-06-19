using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.Movement;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Dash
{
    public class ElfEnergyCore : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Dash";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elf Energy Core");
            //Tooltip.SetDefault("Double-tap left or right to dash.\nCan also be activated with a custom keybind.");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 世纪之花~教徒：Cyan，价值25金
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (hideVisual)
            {
                player.GetModPlayer<ElfEnergyCorePlayer>().hasElfEnergyCoreAD = true;
                player.GetModPlayer<ElfEnergyCorePlayer>().hasElfEnergyCore = false;
            }
            else
            {
                player.GetModPlayer<ElfEnergyCorePlayer>().hasElfEnergyCoreAD = false;
                player.GetModPlayer<ElfEnergyCorePlayer>().hasElfEnergyCore = true;
            }

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.FairyBell);
            recipe.AddIngredient(ItemID.PixieDust, 20);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.HallowBossDye, 1);
            recipe.AddTile(TileID.MythrilAnvil); 
            recipe.Register();
        }
    }
}
