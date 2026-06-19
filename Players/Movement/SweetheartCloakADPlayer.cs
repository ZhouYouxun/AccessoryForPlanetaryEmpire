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
//    public class SweetheartCloakADPlayer : ModPlayer
//    {
//        public const int DashRight = 2;
//        public const int DashLeft = 3;

//        public const int DashCooldown = 48; // 冲刺的冷却（0.8秒）
//        public const int DashDuration = 30; // 冲刺持续时间
//        public const float DashVelocity = 11f; // 冲刺速度调整为较弱的力度

//        public bool hasSweetheartCloakAD = false;
//        public int DashDir = -1;

//        public int DashDelay = 0; // 剩余冷却时间
//        public int DashTimer = 0; // 冲刺持续时间

//        private int dashParticleTimer = 0;
//        private int invincibleTimer = 0;

//        public override void ResetEffects()
//        {
//            hasSweetheartCloakAD = false;

//            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
//            {
//                DashDir = DashRight;
//            }
//            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
//            {
//                DashDir = DashLeft;
//            }
//            else
//            {
//                DashDir = -1;
//            }
//        }

//        public override void PreUpdateMovement()
//        {
//            if (CanUseDash() && DashDir != -1 && DashDelay == 0)
//            {
//                Vector2 newVelocity = Player.velocity;

//                switch (DashDir)
//                {
//                    case DashLeft when Player.velocity.X > -DashVelocity:
//                    case DashRight when Player.velocity.X < DashVelocity:
//                        {
//                            float dashDirection = DashDir == DashRight ? 1 : -1;
//                            newVelocity.X = dashDirection * DashVelocity;
//                            break;
//                        }
//                    default:
//                        return;
//                }

//                DashDelay = DashCooldown;
//                DashTimer = DashDuration;
//                Player.velocity = newVelocity;
//                dashParticleTimer = DashDuration;
//                invincibleTimer = Main.rand.NextFloat() < 0.25f ? DashDuration : 0; // 25%概率无敌
//            }

//            if (DashDelay > 0)
//                DashDelay--;

//            if (DashTimer > 0)
//            {
//                DashTimer--;
//                CreateYellowDust();
//            }

//            if (invincibleTimer > 0)
//            {
//                invincibleTimer--;
//            }
//        }

//        private bool CanUseDash()
//        {
//            return hasSweetheartCloakAD
//                && Player.dashType == 0
//                && !Player.setSolar
//                && !Player.mount.Active;
//        }

//        private void CreateYellowDust()
//        {
//            float sizeMultiplier = 3.0f; // 粒子的大小倍数
//            float brightnessMultiplier = 6f; // 粒子的亮度倍数
//            float radius = 15f; // 粒子生成半径

//            // 在玩家周围随机生成粒子特效
//            int particleCount = 10; // 每次生成的粒子数量
//            for (int i = 0; i < particleCount; i++)
//            {
//                Vector2 randomOffset = new Vector2(Main.rand.NextFloat(-radius, radius), Main.rand.NextFloat(-radius, radius));
//                Vector2 position = Player.Center + randomOffset;
//                CreateParticle(position, sizeMultiplier, brightnessMultiplier);
//            }

//            // 生成类似 DeepDiverDash 的粒子特效
//            for (int m = 0; m < 5; m++)
//            {
//                Vector2 dustVel3 = -Player.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.03f, 0.2f);
//                Dust dust = Dust.NewDustPerfect(Player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) - (Player.velocity * 1.7f), DustID.YellowTorch, dustVel3);
//                dust.scale = Main.rand.NextFloat(0.6f, 1.8f);
//                dust.alpha = 125;
//                dust.noGravity = true;
//            }
//        }

//        private void CreateParticle(Vector2 position, float sizeMultiplier, float brightnessMultiplier)
//        {
//            Dust dust = Dust.NewDustPerfect(position, DustID.YellowTorch, null, 100, Color.White * brightnessMultiplier, 1.5f * sizeMultiplier);
//            dust.noGravity = true;
//            dust.fadeIn = 1f;
//            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
//            dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
//            dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
//        }





//        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
//        {
//            if (invincibleTimer > 0)
//            {
//                return false;
//            }
//            return true;
//        }

//        public override bool CanBeHitByProjectile(Projectile proj)
//        {
//            if (invincibleTimer > 0)
//            {
//                return false;
//            }
//            return true;
//        }

//        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
//        {
//            if (DashTimer > 0 && invincibleTimer == 0)
//            {
//                DashDelay = DashCooldown; // 设置冷却时间
//            }
//        }

//        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
//        {
//            if (DashTimer > 0 && invincibleTimer == 0)
//            {
//                DashDelay = DashCooldown; // 设置冷却时间
//            }
//        }
//    }
//}
