using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class SplittingFlamesProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Splitting Flames");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; // 设置拖尾的长度
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // 设置拖尾模式
        }

        public override void SetDefaults()
        {
            Projectile.width = 20; // 增大弹幕宽度
            Projectile.height = 20; // 增大弹幕高度
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300; // 增加弹幕存在时间
            Projectile.scale = 1.5f; // 增大弹幕尺寸
            Projectile.damage = (int)(Projectile.damage * 2.5f); // 增加伤害倍率
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        public override void OnSpawn(IEntitySource source)
        {
            // 🔥 爆燃式喷射火焰
            for (int i = 0; i < 16; i++)
            {
                Vector2 dir = Main.rand.NextVector2CircularEdge(1f, 1f);
                Vector2 velocity = dir * Main.rand.NextFloat(3f, 6f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, velocity, 100, Color.OrangeRed, Main.rand.NextFloat(1.5f, 2.2f));
                d.noGravity = true;
                d.fadeIn = 1f;
            }

            // 🌋 爆发感的地火特效（使用原版粒子系统）
            ParticleOrchestrator.RequestParticleSpawn(
                clientOnly: false,
                ParticleOrchestraType.FlameWaders,
                new ParticleOrchestraSettings { PositionInWorld = Projectile.Center },
                Projectile.owner
            );
        }

        public override void AI()
        {
            // 增加飞行速度
            if (Projectile.velocity.Length() < 30)
            {
                Projectile.velocity *= 1.05f; // 提高速度
            }

            if (Projectile.timeLeft < 1190)
            {
                Projectile.alpha -= 10;
            }

            Projectile.rotation += 0.2f * (float)Projectile.direction;

            // 粒子效果，增加粒子生成量
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6); // 火焰粒子
                Main.dust[dust].velocity /= 2f;
            }
            {
                // 🔥 火焰拖尾（每帧生成）
                for (int i = 0; i < 3; i++)
                {
                    Vector2 offset = Main.rand.NextVector2Circular(8f, 8f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center + offset, DustID.SolarFlare, -Projectile.velocity * 0.1f, 100,
                        Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat()), Main.rand.NextFloat(1.2f, 1.8f));
                    d.noGravity = true;
                    d.fadeIn = Main.rand.NextFloat(0.6f, 1.2f);
                }

                // 🌋 随机帧触发地火震荡（不频繁）
                if (Main.rand.NextBool(6))
                {
                    ParticleOrchestrator.RequestParticleSpawn(
                        clientOnly: false,
                        ParticleOrchestraType.FlameWaders,
                        new ParticleOrchestraSettings { PositionInWorld = Projectile.Center },
                        Projectile.owner
                    );
                }

            }

            // 追踪最近的敌人
            NPC target = FindClosestNPC(500f); // 追踪范围为500像素
            if (target != null)
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 30, 0.1f); // 平滑追踪，增加速度
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SplitProjectile();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SplitProjectile();
            return true;
        }

        private void SplitProjectile()
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 newVelocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(45 * i)) * 0.5f; // 分裂的角度，降低速度
                int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity, ProjectileID.Flames, (int)(Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner); // 降低分裂后的伤害倍率
                Main.projectile[projID].scale = 1.2f; // 增大分裂后的弹幕尺寸
            }
        }

        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float closestDistance = maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this))
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D lightTexture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/SolarSystem/SplittingFlamesProjectile").Value;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 drawOrigin = new Vector2(lightTexture.Width / 2, lightTexture.Height / 2);

            // 绘制拖尾效果
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 32f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;
                Color color = Color.Lerp(Color.Orange, Color.OrangeRed, colorInterpolation) * 0.4f;
                color.A = 0;
                Vector2 drawPosition = Projectile.oldPos[i] + drawOrigin - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                float rotation = Projectile.velocity.ToRotation() + MathHelper.Pi; // 旋转90度

                float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);
                intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);
                if (Projectile.timeLeft <= 60)
                {
                    intensity *= Projectile.timeLeft / 60f;
                }
                Vector2 outerScale = new Vector2(8f) * intensity;
                Vector2 innerScale = new Vector2(8f) * intensity * 0.7f;
                Color outerColor = color * intensity;
                Color innerColor = color * 0.5f * intensity;

                Main.EntitySpriteDraw(lightTexture, drawPosition, null, outerColor, rotation, drawOrigin, outerScale * 0.6f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, innerColor, rotation, drawOrigin, innerScale * 0.6f, SpriteEffects.None, 0);
            }

            return false; // 不绘制贴图本体
        }






    }
}
