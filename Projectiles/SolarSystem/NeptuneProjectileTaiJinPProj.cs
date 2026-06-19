using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    internal class NeptuneProjectileTaiJinPProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Extra_89"; // 使用原版光点


        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3; // 可击中次数
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 6; // 可调节飞行平滑度
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
        }

        public override void OnSpawn(IEntitySource source)
        {
            // 弹幕生成时执行，用于初始化粒子或播放生成音效
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            // 🌟 生成银灰色粒子拖尾
            if (Main.rand.NextFloat() < 0.4f)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.SilverCoin, Vector2.Zero, 100, Color.WhiteSmoke, 1.2f);
                d.noGravity = true;
                d.fadeIn = 0.5f;
            }

            // === 🎯 转向逻辑 ===
            int time = 300 - Projectile.timeLeft;

            if (time < 150)
            {
                // 每 10 帧为一段
                int segment = (time / 10) % 3;

                float rotateAmount = 0.06f; // 每帧旋转角度
                if (segment == 0)
                    Projectile.velocity = Projectile.velocity.RotatedBy(-rotateAmount); // 左转
                else if (segment == 2)
                    Projectile.velocity = Projectile.velocity.RotatedBy(rotateAmount); // 右转
                // segment == 1 时保持直线
            }
            else
            {
                // 🧲 开始追踪敌人
                NPC target = FindClosestNPC(600f);
                if (target != null)
                {
                    Vector2 toTarget = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, toTarget * Projectile.velocity.Length(), 0.1f);
                }
            }
        }

        // 🔍 寻找最近敌人
        private NPC FindClosestNPC(float maxDistance)
        {
            NPC closest = null;
            float closestDist = maxDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this))
                {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDist)
                    {
                        closest = npc;
                        closestDist = dist;
                    }
                }
            }

            return closest;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;

            // === ✨ 拖尾绘制 ===
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float alpha = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length;
                Color color = Color.Silver * alpha;
                color.A = 0;

                Main.spriteBatch.Draw(
                    texture,
                    drawPos,
                    null,
                    color,
                    Projectile.oldRot[i],
                    drawOrigin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0f
                );
            }

            // === 💫 本体魔法绘制 ===
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 center = Projectile.Center - Main.screenPosition;
            Vector2 origin = tex.Size() * 0.5f;

            float time = Main.GlobalTimeWrappedHourly;
            float baseScale = Projectile.scale;

            for (int i = 0; i < 3; i++)
            {
                float scale = baseScale * (1f + i * 0.15f); // 每层更大
                float rot = time * (i % 2 == 0 ? 1f : -1f) * (0.4f + i * 0.2f); // 交替旋转方向

                Color color = Color.Silver * (0.4f - i * 0.1f); // 外圈更淡
                color.A = 0;

                Main.spriteBatch.Draw(
                    tex,
                    center,
                    null,
                    color,
                    Projectile.rotation + rot,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );
            }

            // 最上层是正常贴图
            //Main.spriteBatch.Draw(
            //    tex,
            //    center,
            //    null,
            //    lightColor,
            //    Projectile.rotation,
            //    origin,
            //    baseScale,
            //    SpriteEffects.None,
            //    0
            //);


            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 弹幕命中 NPC 时执行，可用于生成击中特效、播放音效、回复血量等
        }

        public override void OnKill(int timeLeft)
        {
            // 弹幕死亡（时间到或碰撞）时执行，可用于生成碎裂粒子、播放破碎音效
        }


    }
}
