using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Movement
{
    public class ArtisansToken : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack";
        public override void SetStaticDefaults()
        {


        }

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
            if (Item.prefix == PrefixID.Warding) // Warding (护佑) 增加4点防御
            {
                player.statDefense += 4 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Menacing) // Menacing (险恶) 增加4%伤害
            {
                player.GetDamage(DamageClass.Generic) += 0.04f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Quick) // Quick (迅捷) 增加4%移动速度
            {
                player.moveSpeed += 0.04f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Arcane) // Arcane (奥术) 增加20点魔力
            {
                player.statManaMax2 += 20 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Hard) // Hard (坚韧) 增加1点防御力
            {
                player.statDefense += 1 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Guarding) // Guarding (守护) 增加2点防御力
            {
                player.statDefense += 2 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Armored) // Armored (装甲) 增加3点防御力
            {
                player.statDefense += 3 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Precise) // Precise (精准) 增加2%暴击率
            {
                player.GetCritChance(DamageClass.Generic) += 2 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Lucky) // Lucky (幸运) 增加4%暴击率
            {
                player.GetCritChance(DamageClass.Generic) += 4 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Jagged) // Jagged (锯齿) 增加1%攻击力
            {
                player.GetDamage(DamageClass.Generic) += 0.01f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Spiked) // Spiked (尖刺) 增加2%攻击力
            {
                player.GetDamage(DamageClass.Generic) += 0.02f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Angry) // Angry (愤怒) 增加3%攻击力
            {
                player.GetDamage(DamageClass.Generic) += 0.03f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Menacing) // Menacing (险恶) 增加4%攻击力
            {
                player.GetDamage(DamageClass.Generic) += 0.04f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Brisk) // Brisk (活泼) 增加1%移动速度
            {
                player.moveSpeed += 0.01f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Fleeting) // Fleeting (转瞬) 增加2%移动速度
            {
                player.moveSpeed += 0.02f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Hasty) // Hasty (匆忙) 增加3%移动速度
            {
                player.moveSpeed += 0.03f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Quick) // Quick (迅捷) 增加4%移动速度
            {
                player.moveSpeed += 0.04f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Wild) // Wild (狂野) 增加1%近战攻速
            {
                player.GetAttackSpeed(DamageClass.Melee) += 0.01f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Rash) // Rash (轻率) 增加2%近战攻速
            {
                player.GetAttackSpeed(DamageClass.Melee) += 0.02f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Intrepid) // Intrepid (无畏) 增加3%近战攻速
            {
                player.GetAttackSpeed(DamageClass.Melee) += 0.03f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Violent) // Violent (暴力) 增加4%近战攻速
            {
                player.GetAttackSpeed(DamageClass.Melee) += 0.04f * 6; // 乘以6倍效果
            }

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.SpikyBall, 150);
            recipe.AddIngredient(ItemID.Ruler, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }



    }
}
