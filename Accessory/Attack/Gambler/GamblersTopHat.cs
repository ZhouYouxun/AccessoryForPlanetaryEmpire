using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players.Attack.Gambler;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Gambler
{
    public class GamblersTopHat : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Gambler";
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

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f; // 加0.1倍伤害,也就是10%
            //player.GetCritChance(DamageClass.Generic) = 0f;
            player.GetModPlayer<GamblersTopHatPlayer>().hasGamblersTopHat = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.LargeSapphire, 1);
            recipe.AddIngredient(ItemID.TopHat, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.DarkShard, 1);
            recipe.AddIngredient(ItemID.LightShard, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
