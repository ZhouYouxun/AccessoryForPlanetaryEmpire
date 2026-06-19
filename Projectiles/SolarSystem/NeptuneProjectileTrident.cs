using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    internal class NeptuneProjectileTrident : ModProjectile
    {
        public int HostID; // 绑定的海王星主弹幕
        //public override string Texture => "Terraria/Images/Item_683"; 

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
        }

        public override string Texture
        {
            get
            {
                int texIndex = (int)Projectile.ai[1]; // 0~3
                int[] itemIDs = new[] { 277, 1200, 683, 1445 };
                return $"Terraria/Images/Item_{itemIDs[texIndex % itemIDs.Length]}";
            }
        }

        public override void AI()
        {
            // 如果场上不存在任何海王星本体弹幕 → 直接消失
            bool hasNeptune = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active &&
                    Main.projectile[i].type == ModContent.ProjectileType<NeptuneProjectile>())
                {
                    hasNeptune = true;
                    break;
                }
            }



            int type = ModContent.ProjectileType<NeptuneProjectile>();

            // 无效、越界、inactive、类型不对 → 自杀
            if (!Main.projectile.IndexInRange(HostID) ||
                !Main.projectile[HostID].active ||
                Main.projectile[HostID].type != type)
            {
                Projectile.Kill();
                return;
            }

            Projectile host = Main.projectile[HostID];

            Player player = Main.player[host.owner];

            float baseSpeed = MathHelper.TwoPi / 120f; // 转速
            float rotation = Projectile.ai[0] + Main.GameUpdateCount * baseSpeed;

            float radius = 8 * 16f;
            Vector2 center = host.Center;
            Vector2 offset = rotation.ToRotationVector2() * radius;
            Projectile.Center = center + offset;

            {
                // 🎯 获取敌人方向（如果有）
                Vector2 aimDir = center - Projectile.Center;
                NPC target = FindClosestTarget(Projectile.Center, 5800f);

                if (target != null && target.CanBeChasedBy())
                {
                    aimDir = target.Center - Projectile.Center;
                }

                // ➕ 枪头偏移
                float targetRotation = aimDir.ToRotation();

                // 🌙 平滑非线性旋转（iOS 式动画感）
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, targetRotation, 0.1f);
            }



            // 🌀 每 60 帧发射一次弹幕
            if (++Projectile.localAI[0] > 60)
            {
                Projectile.localAI[0] = 0;

                Vector2 shootDir = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2();
                baseSpeed = 10f;
                int dmg = (int)(player.HeldItem.damage * 0.5f);
                if (dmg < 25)
                    dmg = 25;

                switch ((int)Projectile.ai[1])
                {
                    case 0: // 🌊 水流
                        {
                            int id = Projectile.NewProjectile(
                                Projectile.GetSource_FromThis(),
                                Projectile.Center,
                                shootDir * baseSpeed,
                                ProjectileID.WaterStream,
                                dmg, 0f, Projectile.owner);

                            if (id < Main.maxProjectiles)
                            {
                                var p = Main.projectile[id];
                                p.friendly = true;
                                p.hostile = false;
                                p.penetrate = -1;
                                p.usesLocalNPCImmunity = true;
                                p.localNPCHitCooldown = 60;
                            }
                            break;
                        }

                    case 1: // ⚪ 钛金
                        {
                            Projectile.NewProjectile(
                                Projectile.GetSource_FromThis(),
                                Projectile.Center,
                                shootDir * baseSpeed,
                                ModContent.ProjectileType<NeptuneProjectileTaiJinPProj>(),
                                dmg, 0f, Projectile.owner);
                            break;
                        }

                    case 2: // 💀 恶魔
                        {
                            int id = Projectile.NewProjectile(
                                Projectile.GetSource_FromThis(),
                                Projectile.Center,
                                shootDir * (baseSpeed + 2f),
                                ProjectileID.UnholyTridentFriendly,
                                dmg, 0f, Projectile.owner);

                            if (id < Main.maxProjectiles)
                            {
                                var p = Main.projectile[id];
                                p.friendly = true;
                                p.hostile = false;
                                p.penetrate = -1;
                                p.usesLocalNPCImmunity = true;
                                p.localNPCHitCooldown = 60;

                                // ✅ 👇 你可以在这里做“仅恶魔”的额外设置
                                p.extraUpdates = 1;
                            }
                            break;
                        }

                    case 3: // 🔥 地狱火
                        {
                            int id = Projectile.NewProjectile(
                                Projectile.GetSource_FromThis(),
                                Projectile.Center,
                                shootDir * (baseSpeed + 2f),
                                ProjectileID.InfernoFriendlyBolt,
                                dmg, 0f, Projectile.owner);

                            if (id < Main.maxProjectiles)
                            {
                                var p = Main.projectile[id];
                                p.friendly = true;
                                p.hostile = false;
                                p.penetrate = -1;
                                p.usesLocalNPCImmunity = true;
                                p.localNPCHitCooldown = 60;

                                // ✅ 👇 地狱火的独立逻辑
                                p.extraUpdates = 1;
                            }
                            break;
                        }
                }


            }
        }

        private NPC FindClosestTarget(Vector2 position, float maxDistance)
        {
            NPC closest = null;
            float minDist = maxDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() && !npc.friendly && npc.active)
                {
                    float dist = Vector2.Distance(position, npc.Center);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closest = npc;
                    }
                }
            }

            return closest;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D altTexture = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawOrigin = altTexture.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            // 🌈 根据三叉戟种类选择光晕颜色
            Color glowColor = Color.White;
            switch ((int)Projectile.ai[1])
            {
                case 0: glowColor = Color.Cyan; break;       // 海水三叉戟
                case 1: glowColor = Color.White; break;      // 钛金三叉戟
                case 2: glowColor = Color.Violet; break;     // 恶魔三叉戟
                case 3: glowColor = Color.OrangeRed; break;  // 地狱火三叉戟
            }

            glowColor *= 0.6f;
            glowColor.A = 0;

            // 🔄 基于 localAI[0] 生成非线性脉冲值
            float t = Projectile.localAI[0] / 60f;
            float pulse = MathF.Sin(MathHelper.Pi * t); // [0,1] 再回到 [0]
            float glowStrength = MathHelper.SmoothStep(1.0f, 5.5f, pulse); // 非线性放大 glow 效果

            // 🔆 绘制外部光晕
            for (int i = 0; i < 8; i++)
            {
                float angle = MathHelper.TwoPi * i / 8f;
                Vector2 offset = angle.ToRotationVector2() * glowStrength;
                Main.spriteBatch.Draw(
                    altTexture,
                    drawPos + offset,
                    null,
                    glowColor,
                    Projectile.rotation,
                    drawOrigin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0f
                );
            }

            // 🗡 绘制三叉戟本体
            Main.spriteBatch.Draw(
                altTexture,
                drawPos,
                null,
                Color.White,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }





    }
}