using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using static System.Collections.Specialized.BitVector32;
using AccessoryForPlanetaryEmpire.Players.Attack.Annihilation;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Annihilation
{
    public class DamageProtectionDevice : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Annihilation";

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
            player.GetDamage(DamageClass.Melee) += 0.45f; // 提升45%的近战攻击力
            player.GetAttackSpeed(DamageClass.Melee) += 0.45f; // 提升45%的近战攻击速度
            player.GetModPlayer<DamageProtectionPlayer>().hasDamageProtection = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.HallowedBar, 15); // 神圣定
            recipe.AddIngredient(ItemID.SoulofFright, 5); // 恐惧之魂
            recipe.AddIngredient(ItemID.SharpeningStation, 1); // 利器站
            recipe.AddIngredient(ItemID.BreakerBlade, 1); // 毁灭刃
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
