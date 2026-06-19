using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using System.Linq;
using AccessoryForPlanetaryEmpire.Players;
using Terraria.GameContent;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class JupiterProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jupiter");
        }

        public override void SetDefaults()
        {
            Projectile.width = 55;
            Projectile.height = 55;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1.5f;
            Projectile.damage = 800; // 初始伤害
            Projectile.DamageType = DamageClass.Melee; // 设置为近战伤害类型
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.GetModPlayer<JupiterPlayer>().hasJupiterAccessory)
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 10;

            // 设置公转初始速度
            float baseRotatingSpeed = MathHelper.TwoPi / 1200f; // 默认公转速度
            float rotatingSpeed = baseRotatingSpeed;

            // 初始化设置，第一次运行时触发
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[1] = 10f; // 初始速度倍率为10倍
            }

            // 每一帧减速，直到达到正常速度
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], 1f, 0.02f); // 指数减速
            rotatingSpeed *= Math.Max(Projectile.localAI[1], 1f); // 确保旋转速度不低于正常速度

            Projectile.localAI[0]++; // 递增计时器

            // 根据减速后的旋转速度调整角度增量
            Projectile.ai[0] += (float)(rotatingSpeed / baseRotatingSpeed * 2f);
            double angle = Projectile.ai[0] * baseRotatingSpeed;
            Vector2 offset = new Vector2(-1, -1) * 25 * 16; // 公转半径，木星位于左上角
            offset = offset.RotatedBy(angle);
            Projectile.Center = player.Center + offset;

            // 添加黄色光效（木星主题色）
            Lighting.AddLight(Projectile.Center, 0.9f, 0.8f, 0.3f); // 添加黄色光效

            // 添加拖尾效果
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Copper);
            Main.dust[dustId].noGravity = true;
            Main.dust[dustId].scale = 1.5f;
            Main.dust[dustId].velocity *= 0.5f;

            // 添加常态光环
            for (int i = 0; i < 5; i++)
            {
                Vector2 circularOffset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(5f, 10f);
                int ringDustId = Dust.NewDust(Projectile.Center + circularOffset, 1, 1, DustID.GoldFlame);
                Main.dust[ringDustId].noGravity = true;
                Main.dust[ringDustId].scale = 0.5f;
            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Explode(target);
        }

        private void Explode(NPC target)
        {
            // 释放爆炸弹幕
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.DD2ExplosiveTrapT3Explosion, Projectile.damage, Projectile.knockBack, Projectile.owner);

            // 🌋 木星风暴爆发：三层爆破 Dust 风暴
            for (int layer = 0; layer < 3; layer++)
            {
                int count = 30 + layer * 10;
                float baseSpeed = 2f + layer * 1.5f;
                float scaleBase = 1.2f - layer * 0.2f;

                for (int i = 0; i < count; i++)
                {
                    float angle = MathHelper.TwoPi * i / count + Main.rand.NextFloat(-0.1f, 0.1f);
                    Vector2 velocity = angle.ToRotationVector2() * Main.rand.NextFloat(baseSpeed * 0.6f, baseSpeed * 1.4f);

                    // 多种火焰/爆破系 Dust 混合
                    int dustType = Main.rand.NextBool(3) ? DustID.Torch : Main.rand.NextBool(2) ? DustID.GoldFlame : DustID.SolarFlare;

                    Dust d = Dust.NewDustPerfect(Projectile.Center, dustType, velocity, 80,
                        Color.Lerp(Color.OrangeRed, Color.Yellow, Main.rand.NextFloat(0.3f, 0.7f)),
                        scaleBase * Main.rand.NextFloat(0.8f, 1.6f));

                    d.noGravity = Main.rand.NextBool(3) ? true : false; // 混合部分无重力火焰
                    d.fadeIn = Main.rand.NextFloat(0.5f, 1.2f); // 更猛烈的膨胀感
                }
            }


            // 确保爆炸只发生一次
            // Projectile.Kill();
        }

        public override bool? CanHitNPC(NPC target)
        {
            // 检查是否能碰撞敌人
            if (target.immune[Projectile.owner] == 0)
            {
                return base.CanHitNPC(target);
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // 使目标有短暂无敌帧
            target.immune[Projectile.owner] = 10;
        }

        public override bool PreDraw(ref Color lightColor) // 确保贴图的中心点为绘制的中心点
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            spriteBatch.Draw(texture, drawPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }




    }
}
