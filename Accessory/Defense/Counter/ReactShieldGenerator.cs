using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using Humanizer;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players.Defense.Counter;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.Counter
{
    public class ReactShieldGenerator : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.Counter";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 世纪之花~教徒：Cyan，价值35金
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 35, 0, 0);
            Item.value = Item.sellPrice(0, 35, 0, 0);

        }



        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ReactShieldGeneratorPlayer>().hasReactShieldGenerator = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.MasterNinjaGear, 1); // 忍者大师设备
            recipe.AddIngredient(ItemID.SilverDye, 5); // 银色和棕色的两种染料
            recipe.AddIngredient(ItemID.BrownDye, 5);
            recipe.AddIngredient(ItemID.Ectoplasm, 5); // 灵气
            recipe.AddIngredient(ItemID.SoulofNight, 10); // 暗影之魂
            recipe.AddIngredient(ItemID.DarkShard, 2); // 暗影碎片
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }


    }
}




