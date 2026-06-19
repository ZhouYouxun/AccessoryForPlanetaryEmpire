using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players;



namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Summon
{
    public class Summon0 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Summon";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式前：Orange，价值15金
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.value = Item.sellPrice(0, 15, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) *= 0f; 
            player.GetAttackSpeed(DamageClass.Summon) += 5.20f;
            player.GetModPlayer<InfSummonPlayer>().hasInfSummon = true;
            player.maxMinions += 1145140;
            player.maxTurrets += 1145140;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BabyBirdStaff, 1);
            recipe.AddIngredient(ItemID.SlimeStaff, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
