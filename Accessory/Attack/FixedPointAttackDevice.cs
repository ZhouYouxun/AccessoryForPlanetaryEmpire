using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players.Attack;
using System;


namespace AccessoryForPlanetaryEmpire.Accessory.Attack
{
    public class FixedPointAttackDevice : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack";
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
            player.GetModPlayer<FixedPointAttackDevicePlayer>().hasFixedPointAttackDevice = true;

            //if (player.velocity == Vector2.Zero) // 如果玩家不移动
            //{
            //    player.GetDamage(DamageClass.Generic) += 1f; // 增加100%的攻击力
            //    player.GetAttackSpeed(DamageClass.Generic) += 0.5f; // 增加50%的攻击速度
            //    player.lifeRegen += 15; // 增加15点生命回复
            //    player.aggro -= 3000; // 降低3000点仇恨
            //                          // 生成红色粒子
            //                          // 生成红色粒子形成一个圆圈
            //    float radius = 50f; // 圆圈半径
            //    int dustCount = 20; // 粒子数量
            //    for (int i = 0; i < dustCount; i++)
            //    {
            //        float angle = 2f * (float)Math.PI / dustCount * i;
            //        float x = player.Center.X + radius * (float)Math.Cos(angle);
            //        float y = player.Center.Y + radius * (float)Math.Sin(angle);
            //        Vector2 dustPosition = new Vector2(x, y);
            //        Dust dust = Dust.NewDustPerfect(dustPosition, DustID.Torch, Vector2.Zero, 100, default(Color), 1.5f);
            //        dust.velocity *= 0.3f; // 调整粒子的速度
            //        dust.noGravity = true; // 不受重力影响
            //        dust.fadeIn = 1f;
            //        dust.scale = 1.5f;
            //    }
            //}
            //else // 如果玩家在移动
            //{
            //    player.GetDamage(DamageClass.Generic) -= 0.99f; // 降低99%的攻击力
            //    player.statDefense -= (int)(player.statDefense * 0.99f); // 降低99%的防御力
            //}
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); 
            recipe.AddIngredient(ItemID.BeetleScaleMail, 1); // 甲虫进攻甲
            recipe.AddIngredient(ItemID.WoodenChair, 10); // 椅子，太阳花
            recipe.AddIngredient(ItemID.Sunflower, 3);
            recipe.AddIngredient(ItemID.RagePotion, 5); // 三种药水
            recipe.AddIngredient(ItemID.ArcheryPotion, 5);
            recipe.AddIngredient(ItemID.WrathPotion, 5);
            recipe.AddTile(TileID.LihzahrdFurnace);
            recipe.Register();
        }

    }
}
