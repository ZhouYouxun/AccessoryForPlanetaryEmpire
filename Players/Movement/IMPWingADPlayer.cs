//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;

//namespace AccessoryForPlanetaryEmpire.Players.Movement
//{
//    public class IMPWingADPlayer : ModPlayer
//    {
//        public bool hasIMPWingAD;
//        private bool isDashing = false;
//        private int dashCooldown = 0;
//        private int dashDuration = 15; // 冲刺持续时间
//        private int dashSpeed = 18; // 冲刺初始速度，与 ShallowOxygenBottlePlayer 一致
//        private int invincibilityFrames = 60; // 冲刺期间无敌60帧

//        public override void ResetEffects()
//        {
//            hasIMPWingAD = false;
//        }

//        public override void PreUpdateMovement()
//        {
//            // 通过双击左右键触发冲刺
//            if (hasIMPWingAD && dashCooldown == 0 && (Player.controlRight && Player.doubleTapCardinalTimer[2] > 0 || Player.controlLeft && Player.doubleTapCardinalTimer[3] > 0))
//            {
//                StartDash();
//            }
//            else if (isDashing)
//            {
//                PerformDash();
//            }

//            // 处理冷却时间
//            if (dashCooldown > 0)
//            {
//                dashCooldown--;
//            }
//        }

//        private void StartDash()
//        {
//            isDashing = true;
//            dashCooldown = 60; // 冲刺冷却时间与 ShallowOxygenBottlePlayer 一致
//            Player.immuneTime = invincibilityFrames; // 设置无敌时间
//            Player.velocity = GetDashDirection() * dashSpeed; // 设置冲刺速度
//        }

//        private Vector2 GetDashDirection()
//        {
//            // 根据双击方向键决定冲刺方向
//            if (Player.controlLeft)
//                return Vector2.UnitX * -1;
//            else
//                return Vector2.UnitX; // 默认向右冲刺
//        }

//        private void PerformDash()
//        {
//            if (dashDuration > 0)
//            {
//                Player.velocity *= 0.95f; // 冲刺期间速度逐渐减少
//                dashDuration--;

//                CreateDashEffect(); // 生成粒子特效
//                CheckDashCollision(); // 检测冲刺碰撞
//            }
//            else
//            {
//                isDashing = false;
//                dashDuration = 15; // 重置冲刺持续时间
//            }
//        }

//        private void CreateDashEffect()
//        {
//            // 创建来自 ElfEnergyCorePlayer 和 ShallowOxygenBottlePlayer 的粒子特效
//            Vector2 position = Player.Center;

//            // ElfEnergyCorePlayer 的粒子特效
//            for (int i = 0; i < 5; i++)
//            {
//                Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
//                Dust dust = Dust.NewDustPerfect(position, DustID.YellowTorch, velocity);
//                dust.noGravity = true;
//                dust.scale = 2f;
//                dust.fadeIn = 1f;
//            }

//            // ShallowOxygenBottlePlayer 的粒子特效
//            for (int i = 0; i < 6; i++)
//            {
//                Vector2 velocity = new Vector2((float)Math.Cos(Main.rand.NextFloat(0, MathHelper.TwoPi)), (float)Math.Sin(Main.rand.NextFloat(0, MathHelper.TwoPi))) * 1.5f;
//                Dust dust = Dust.NewDustPerfect(position, DustID.Water, velocity);
//                dust.noGravity = true;
//                dust.scale = 1.5f;
//            }
//        }

//        private void CheckDashCollision()
//        {
//            foreach (NPC npc in Main.npc)
//            {
//                if (npc.active && !npc.friendly && npc.lifeMax > 5 && Player.getRect().Intersects(npc.getRect()))
//                {
//                    npc.StrikeNPC(new NPC.HitInfo
//                    {
//                        Damage = 200, // 冲刺伤害
//                        Knockback = 10f,
//                        HitDirection = Player.direction
//                    });

//                    for (int i = 0; i < 10; i++)
//                    {
//                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldFlame, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 150, default(Color), 1.5f);
//                    }
//                }
//            }
//        }

//        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
//        {
//            return !isDashing; // 冲刺期间免疫敌人伤害
//        }

//        public override bool CanBeHitByProjectile(Projectile proj)
//        {
//            if (isDashing && Main.rand.NextFloat() < 0.5f)
//            {
//                proj.Kill(); // 50% 概率清除弹幕
//                return false;
//            }
//            return !isDashing;
//        }
//    }
//}