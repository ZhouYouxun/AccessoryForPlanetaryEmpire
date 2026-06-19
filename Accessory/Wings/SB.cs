using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class SB : ModItem
    {
        public override void SetStaticDefaults()
        {
            // 设置飞行时间 = 5秒 (300帧)，垂直飞行速度 = 原版6.5f ×1.5
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(100, 6.5f * 1.5f, 1.5f);
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 最终毕业装：Master，价值95金
            Item.rare = ItemRarityID.Master;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.value = Item.sellPrice(0, 95, 0, 0);

        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 启用所有原版 ExtraJump 项
            // ✅启用明确列出的 ExtraJump 项
            player.GetJumpState(ExtraJump.Flipper).Enable();
            player.GetJumpState(ExtraJump.BasiliskMount).Enable();
            player.GetJumpState(ExtraJump.GoatMount).Enable();
            player.GetJumpState(ExtraJump.SantankMount).Enable();
            player.GetJumpState(ExtraJump.UnicornMount).Enable();
            player.GetJumpState(ExtraJump.SandstormInABottle).Enable();
            player.GetJumpState(ExtraJump.BlizzardInABottle).Enable();
            player.GetJumpState(ExtraJump.FartInAJar).Enable();
            player.GetJumpState(ExtraJump.TsunamiInABottle).Enable();
            player.GetJumpState(ExtraJump.CloudInABottle).Enable();


            // 强化跳跃属性
            player.jumpSpeedBoost += 2.0f;
            player.jumpBoost = true;
            player.autoJump = true;
            player.noFallDmg = true;

            // ✨仅在完成所有跳跃后进入飞行阶段时触发粒子特效
            if (player.controlJump && player.wingTime > 0f && player.jump == 0 && player.velocity.Y != 0f && !hideVisual)
            {
                // 粒子发射线：以脚底下 y + 24 为中心线，x方向 ±16 像素
                Vector2 center = player.Center + new Vector2(0, 24f);
                float time = Main.GameUpdateCount * 0.1f;
                for (int i = 0; i < 2; i++)
                {
                    float angle = time + i * MathHelper.Pi;
                    float spiralX = (float)Math.Cos(angle) * 16f;
                    float spiralY = (float)Math.Sin(angle * 2f) * 2f;

                    Vector2 pos = center + new Vector2(spiralX, spiralY);
                    Dust dust = Dust.NewDustDirect(pos, 0, 0, DustID.Electric, 0f, 0f, 100, Color.Cyan, 1.3f);
                    dust.noGravity = true;
                    dust.velocity *= 0f;

                    Dust trail = Dust.NewDustDirect(pos, 0, 0, DustID.BlueCrystalShard, 0f, 0f, 150, Color.LightBlue, 1.1f);
                    trail.noGravity = true;
                    trail.velocity *= 0f;
                }
            }

        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.75f;
            ascentWhenRising = 0.225f;
            maxCanAscendMultiplier = 1.2f;
            maxAscentMultiplier = 3.0f;
            constantAscend = 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BundleofBalloons)//气球束
                .AddIngredient(ItemID.BalloonHorseshoeFart)//绿马掌气球
                .AddIngredient(ItemID.Flipper)//脚蹼
                .AddIngredient(ItemID.AncientHorn)//远古号角
                .AddIngredient(ItemID.WallOfFleshGoatMountItem)//黑色山羊
                .AddIngredient(ItemID.SantankMountItem)//圣诞坦克
                .AddIngredient(ItemID.BlessedApple)// 苹果
                //.AddIngredient(ItemID.SandstorminaBottle)//沙暴
                //.AddIngredient(ItemID.BlizzardinaBottle)//暴雪
                //.AddIngredient(ItemID.TsunamiInABottle)//海啸
                //.AddIngredient(ItemID.CloudinaBottle)//云朵
                .AddIngredient(ItemID.SoulofFlight, 20)
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
