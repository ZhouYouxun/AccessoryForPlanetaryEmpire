using AccessoryForPlanetaryEmpire.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace AccessoryForPlanetaryEmpire.Players.Movement
{
    public class PowerJetPackPlayer : ModPlayer
    {
        public bool hasPowerJetPack = false;    // 普通冲刺
        public bool hasMultipleTorpedoTubes = false;  // 新增，提供特殊冲刺效果
        public static ModKeybind HXRKeybind;
        private Vector2 originalVelocity;
        private int dashCooldown = 0;
        private int dashDuration = 40; // 冲刺持续时间40帧
        private int dashTimer = 0;

        public override void Load()
        {
            // 注册自定义快捷键
            HXRKeybind = KeybindLoader.RegisterKeybind(Mod, "大冲刺", "P");
        }

        public override void ResetEffects()
        {
            hasPowerJetPack = false;
            hasMultipleTorpedoTubes = false;  // 重置MultipleTorpedoTubes开关
        }

        public override void PostUpdate()
        {
            if (dashCooldown > 0)
            {
                dashCooldown--;
            }

            if ((hasPowerJetPack || hasMultipleTorpedoTubes) && HXRKeybind.JustPressed && dashCooldown == 0)
            {
                Dash();
            }

            if (dashTimer > 0)
            {
                dashTimer--;
                CreateJetParticles();
                if (hasMultipleTorpedoTubes)
                {
                    LaunchTorpedoes();  // 如果启用了MultipleTorpedoTubes，则发射导弹
                }
                else
                {
                    CheckDashCollision();  // 否则检测接触伤害
                }
                if (dashTimer == 0)
                {
                    EndDash();
                }
            }
        }

        private void Dash()
        {
            originalVelocity = Player.velocity;
            Vector2 dashDirection = Vector2.Zero;

            // 根据玩家当前方向设置冲刺方向
            if (Player.controlLeft)
            {
                dashDirection = new Vector2(-1f, 0f);
            }
            else if (Player.controlRight)
            {
                dashDirection = new Vector2(1f, 0f);
            }

            if (dashDirection != Vector2.Zero)
            {
                dashDirection.Normalize();
                Player.velocity = dashDirection * 35f; // 设置更快的冲刺速度
                dashTimer = dashDuration;
                dashCooldown = 20 * 60; // 冷却时间为45秒
                Player.AddBuff(ModContent.BuffType<PowerJetPackCoolDown>(), 20 * 60); // 添加冷却Buff
            }
        }

        private void EndDash()
        {
            Player.velocity = originalVelocity; // 恢复原始速度
        }

        private void CreateJetParticles()
        {
            int dustType = hasMultipleTorpedoTubes ? DustID.Smoke : DustID.Electric;
            Color dustColor = hasMultipleTorpedoTubes ? Color.Gray : default(Color);

            for (int i = 0; i < 15; i++)
            {
                Vector2 position = Player.Center + new Vector2(Main.rand.NextFloat(-20f, 20f), Main.rand.NextFloat(-20f, 20f));
                Dust dust = Dust.NewDustPerfect(position, dustType, null, 150, dustColor, 2f);
                dust.velocity = Player.velocity * 0.5f;
                dust.noGravity = true;
                dust.scale = 1.5f * Main.rand.NextFloat(0.7f, 1.3f);
            }

            for (int i = 0; i < 30; i++)
            {
                Vector2 position = Player.Center + new Vector2(Main.rand.NextFloat(-40f, 40f), Main.rand.NextFloat(-40f, 40f));
                Dust dust = Dust.NewDustPerfect(position, dustType, null, 150, dustColor, 1.2f);
                dust.velocity = Player.velocity * 0.3f;
                dust.noGravity = false;
                dust.scale = 1.2f * Main.rand.NextFloat(0.5f, 1.0f);
            }

            // 半圆形的冲刺轨迹
            for (int i = 0; i < 20; i++)
            {
                float angle = MathHelper.ToRadians(i * 9); // 0-180度
                Vector2 position = Player.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 40f;
                Dust dust = Dust.NewDustPerfect(position, DustID.GoldFlame, null, 150, dustColor, 1.5f);
                dust.velocity = Player.velocity * 0.5f;
                dust.noGravity = true;
                dust.scale = 2f * Main.rand.NextFloat(0.7f, 1.3f);
            }
        }

        private void LaunchTorpedoes()
        {
            // 在冲刺期间，每帧随机发射ClusterSnowmanRocketI(803) 或 MiniNukeSnowmanRocketI(808)
            int[] projectileChoices = { ProjectileID.ClusterSnowmanRocketI, ProjectileID.MiniNukeSnowmanRocketI };
            int selectedProjectile = Main.rand.Next(projectileChoices);

            // 发射方向为玩家中心的随机方向
            Vector2 launchDirection = Main.rand.NextVector2Unit() * 8f; // 随机发射方向

            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, launchDirection, selectedProjectile, 100, 0, Player.whoAmI);
        }

        private void CheckDashCollision()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && Player.getRect().Intersects(npc.getRect()))
                {
                    npc.StrikeNPC(new NPC.HitInfo
                    {
                        Damage = 300, // 冲刺伤害
                        Knockback = 10f,
                        HitDirection = Player.direction
                    });

                    // 在敌人位置产生Dust
                    for (int i = 0; i < 10; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.UltraBrightTorch, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 150, default(Color), 1.5f);
                    }
                }
            }
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (dashTimer > 0)
            {
                proj.Kill();
                return false;
            }
            return true;
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            return dashTimer <= 0;
        }
    }
}
