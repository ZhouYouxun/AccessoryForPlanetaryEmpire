using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.IO.Pipelines;
using System.Collections;
using AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SummonOnly
{
    public class GiftofLcarus : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SummonOnly";
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
            player.GetAttackSpeed(DamageClass.Summon) += 0.5f;
            // player.GetCritChance(DamageClass.Melee) -= 20;
            player.GetDamage(DamageClass.Summon) += 0.35f;
            player.maxTurrets += 1;
            player.maxMinions += 3;
            player.GetModPlayer<GiftofLcarusPlayer>().hasGiftofLcarus = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.SummonerEmblem, 1); // 召唤师徽章
            recipe.AddIngredient(ItemID.BewitchingTable, 1); // 施法桌
            recipe.AddIngredient(ItemID.Glass, 50); // 玻璃
            recipe.AddIngredient(ItemID.Sapphire, 5); // 蓝玉
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
    }
}
