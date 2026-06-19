using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Players;
using System;

namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory
{
    public class LifeSubSpacePROJ : ModProjectile
    {
        public override string Texture => "AccessoryForPlanetaryEmpire/Projectiles/Accessory/INV"; // 使用透明弹幕贴图

        public override void SetDefaults()
        {
            Projectile.width = 1000; // 设置光环的宽度
            Projectile.height = 1000; // 设置光环的高度
            Projectile.friendly = true; // 弹幕对玩家友好
            Projectile.penetrate = -1; // 弹幕可以无限穿透
            Projectile.timeLeft = 900; // 弹幕持续时间，这里设置为900帧15秒
            Projectile.tileCollide = false; // 弹幕不与瓷砖碰撞
            Projectile.ignoreWater = true; // 弹幕忽略水的影响
            Projectile.alpha = 255; // 初始时弹幕是完全透明的
        }

        public override void AI()
        {
            // 弹幕的逻辑会每帧调用一次
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && player.Hitbox.Intersects(Projectile.Hitbox))
                {
                    // 如果玩家活跃且没有死亡，并且与弹幕的碰撞框相交
                    player.AddBuff(ModContent.BuffType<LifeSubSpaceBuff>(), 60, true); // 给玩家添加LifeSubSpaceBuff效果，持续1秒（60帧）

                    // 清除玩家的所有负面buff
                    for (int j = 0; j < player.buffType.Length; j++)
                    {
                        if (Main.debuff[player.buffType[j]])
                        {
                            player.DelBuff(j);
                        }
                    }
                }
            }

            if (Main.rand.NextBool(3)) // 每 3 帧触发一次
            {
                PLALightingBoltsSystem.PLA_Spawn_BloodRingWhiteSparkles(Projectile.Center, 500f); // ? 像素环绕
            }

            // 生成粒子效果
            float time = (float)Main.GameUpdateCount / 60f;

            for (int i = 0; i < 20; i++)
            {
                // 🔵 控制粒子角度 + 波动半径
                float angle = MathHelper.TwoPi * i / 20f + Main.rand.NextFloat(-0.2f, 0.2f);
                float radius = 500f + (float)Math.Sin(time * 2f + i) * 30f; // 波动扩散

                Vector2 offset = angle.ToRotationVector2() * radius;
                Vector2 position = Projectile.Center + offset;

                // 💨 速度朝向中心，略带扰动
                Vector2 velocity = -offset.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.4f, 3f);
                velocity += offset.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(-0.4f, 0.4f);

                // 🧪 粒子
                Dust dust = Dust.NewDustPerfect(position, DustID.MagicMirror, velocity, 80, Color.Lerp(Color.Green, Color.White, Main.rand.NextFloat(0.2f)), 1.6f);
                dust.noGravity = true;
                dust.fadeIn = 0.9f;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }





        }

        public override bool? CanCutTiles()
        {
            return false; // 光环不会切割瓷砖
        }

        public override bool? CanDamage()
        {
            return false; // 光环不会造成伤害
        }
    }
}
