//using AccessoryForPlanetaryEmpire.Players;
//using AccessoryForPlanetaryEmpire.Global;
//using AccessoryForPlanetaryEmpire.Accessory.Dash;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;


//namespace AccessoryForPlanetaryEmpire.Players.Movement
//{
//    public class ElfEnergyCoreADPlayer : ModPlayer
//    {
//        public const int DashRight = 2;
//        public const int DashLeft = 3;

//        public const int DashCooldown = 60; // 冲刺的冷却
//        public const int DashDuration = 60; // 冲刺持续时间

//        public const float DashVelocity = 12f;

//        public bool hasElfEnergyCoreAD = false;
//        public int DashDir = -1;

//        public int DashDelay = 0; // frames remaining till we can dash again
//        public int DashTimer = 0; // frames remaining in the dash

//        private int dashParticleTimer = 0;
//        private int invincibleTimer = 0;

//        public override void ResetEffects()
//        {
//            hasElfEnergyCoreAD = false;

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
//                            float dashDirection = DashDir == DashRight ? 1 : -1.3f;
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
//                invincibleTimer = 30; // 30 frames of invincibility
//            }

//            if (DashDelay > 0)
//                DashDelay--;

//            if (DashTimer > 0)
//            {
//                Player.eocDash = DashTimer;
//                Player.armorEffectDrawShadowEOCShield = true;
//                DashTimer--;
//            }

//            if (dashParticleTimer > 0)
//            {
//                dashParticleTimer--;
//                CreateElfDust();
//            }

//            if (invincibleTimer > 0)
//            {
//                invincibleTimer--;
//            }
//        }

//        private bool CanUseDash()
//        {
//            return hasElfEnergyCoreAD
//                && Player.dashType == 0
//                && !Player.setSolar
//                && !Player.mount.Active;
//        }

//        private void CreateElfDust()
//        {
//            float progress = (DashDuration - dashParticleTimer) / (float)DashDuration;
//            float radius = 10f * (1 - progress);
//            float sizeMultiplier = 2.5f; // 粒子的大小倍数
//            float brightnessMultiplier = 6f; // 粒子的亮度倍数

//            // 生成正方形形状的粒子特效
//            int particlesPerSide = 5;
//            for (int i = 0; i < particlesPerSide; i++)
//            {
//                float offset = i - (particlesPerSide - 1) / 2f;

//                // 上边
//                CreateParticle(new Vector2(offset, -3.5f) * radius, sizeMultiplier, brightnessMultiplier);
//                // 下边
//                CreateParticle(new Vector2(offset, 3.5f) * radius, sizeMultiplier, brightnessMultiplier);
//                // 左边
//                CreateParticle(new Vector2(-3.5f, offset) * radius, sizeMultiplier, brightnessMultiplier);
//                // 右边
//                CreateParticle(new Vector2(3.5f, offset) * radius, sizeMultiplier, brightnessMultiplier);
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

//        private void CreateParticle(Vector2 positionOffset, float sizeMultiplier, float brightnessMultiplier)
//        {
//            Vector2 dustPosition = Player.Center + positionOffset;
//            Dust dust = Dust.NewDustPerfect(dustPosition, DustID.YellowTorch, null, 100, Color.White * brightnessMultiplier, 1.5f * sizeMultiplier);
//            dust.noGravity = true;
//            dust.fadeIn = 1f;
//            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
//            dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
//            dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
//        }


//        public override bool CanBeHitByProjectile(Projectile proj)
//        {
//            if (invincibleTimer > 0)
//            {
//                if (Main.rand.NextFloat() < 0.2f)
//                {
//                    proj.Kill();
//                }
//                return false;
//            }
//            return true;
//        }

//        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
//        {
//            if (invincibleTimer > 0)
//            {
//                return true;
//            }
//            return true;
//        }
//    }
//}