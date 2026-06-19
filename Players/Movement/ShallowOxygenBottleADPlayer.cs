//using System;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;

//namespace AccessoryForPlanetaryEmpire.Players.Movement
//{
//    public class ShallowOxygenBottleADPlayer : ModPlayer
//    {
//        public const int DashRight = 2;
//        public const int DashLeft = 3;

//        public const int DashCooldown = 60; // 冲刺的冷却
//        public const int DashDuration = 60; // 冲刺持续时间

//        public const float DashVelocity = 15f;

//        public bool hasShallowOxygenBottleAD = false;
//        public bool isDashing = false;
//        public int dashCooldown = 0;
//        private int dashParticleTimer = 0;
//        private int invincibleTimer = 0;
//        public int DashDir = -1;

//        public int DashDelay = 0; // frames remaining till we can dash again
//        public int DashTimer = 0; // frames remaining in the dash

//        public override void ResetEffects()
//        {
//            hasShallowOxygenBottleAD = false;

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

//        public override void PostUpdate()
//        {
//            if (dashCooldown > 0)
//            {
//                dashCooldown--;
//            }

//            if (hasShallowOxygenBottleAD)
//            {
//                // 检测是否双击方向键
//                if (DashDir != -1 && dashCooldown == 0)
//                {
//                    Dash();
//                }

//                // 如果粒子计时器大于0，则继续生成粒子
//                if (dashParticleTimer > 0)
//                {
//                    dashParticleTimer--;
//                    CreateWaterDust();
//                }

//                // 如果无敌计时器大于0，则继续减小计时器
//                if (invincibleTimer > 0)
//                {
//                    invincibleTimer--;
//                    if (invincibleTimer == 0)
//                    {
//                        isDashing = false; // 无敌时间结束
//                    }
//                }
//            }

//            // 检测冲刺状态下的碰撞
//            if (isDashing)
//            {
//                CheckDashCollision();
//            }
//        }

//        // 执行冲刺功能
//        private void Dash()
//        {
//            Vector2 dashDirection = Player.velocity;

//            // 如果玩家当前速度为零，不执行冲刺
//            if (dashDirection != Vector2.Zero)
//            {
//                dashDirection.Normalize(); // 保持当前的移动方向
//                Player.velocity = dashDirection * 18f; // 只加速，不改变方向
//                isDashing = true;
//                dashCooldown = 60; // 冷却时间为1秒
//                dashParticleTimer = 60; // 粒子生成时间为1秒
//                invincibleTimer = 40; // 无敌时间为40帧
//            }
//        }

//        // 创建水能粒子dust
//        private void CreateWaterDust()
//        {
//            float progress = (60 - dashParticleTimer) / 60f; // 计算粒子生成的进度
//            float radius = 10f * (1 - progress); // 计算当前半径
//            float sizeMultiplier = 3.0f; // 粒子的大小倍数
//            float brightnessMultiplier = 6f; // 粒子的亮度倍数
                                            
//            // 六芒星形状的控制点
//            Vector2[] points = {
//        new Vector2(0, -3.5f), // 上
//        new Vector2(1.5f, -2.625f), // 右上1
//        new Vector2(3.03f, -1.75f), // 右上2
//        new Vector2(3.03f, 1.75f), // 右下2
//        new Vector2(1.5f, 2.625f), // 右下1
//        new Vector2(0, 3.5f), // 下
//        new Vector2(-1.5f, 2.625f), // 左下1
//        new Vector2(-3.03f, 1.75f), // 左下2
//        new Vector2(-3.03f, -1.75f), // 左上2
//        new Vector2(-1.5f, -2.625f) // 左上1
//    };

//            for (int i = 0; i < points.Length; i++)
//            {
//                Vector2 position = Player.Center + points[i] * radius;
//                CreateParticle(position, sizeMultiplier, brightnessMultiplier);
//            }

//            // 生成类似 DeepDiverDash 的粒子特效
//            for (int m = 0; m < 5; m++)
//            {
//                Vector2 dustVel3 = -Player.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.03f, 0.2f);
//                Dust dust = Dust.NewDustPerfect(Player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) - (Player.velocity * 1.7f), DustID.BlueTorch, dustVel3);
//                dust.scale = Main.rand.NextFloat(0.6f, 1.8f);
//                dust.alpha = 125;
//                dust.noGravity = true;
//            }
//        }

//        private void CreateParticle(Vector2 position, float sizeMultiplier, float brightnessMultiplier)
//        {
//            Dust dust = Dust.NewDustPerfect(position, DustID.BlueTorch, null, 100, Color.White * brightnessMultiplier, 1.5f * sizeMultiplier);
//            dust.noGravity = true;
//            dust.fadeIn = 1f;
//            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
//            dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
//            dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
//        }

//        private void CheckDashCollision()
//        {
//            foreach (NPC npc in Main.npc)
//            {
//                if (npc.active && !npc.friendly && npc.lifeMax > 5 && Player.getRect().Intersects(npc.getRect()))
//                {
//                    npc.StrikeNPC(new NPC.HitInfo
//                    {
//                        Damage = 75, // 冲刺伤害
//                        Knockback = 10f,
//                        HitDirection = Player.direction
//                    });

//                    // 在敌人位置产生Dust
//                    for (int i = 0; i < 10; i++)
//                    {
//                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.UltraBrightTorch, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 150, default(Color), 1.5f);
//                    }
//                    // 结束冲刺
//                    // isDashing = false;
//                    // invincibleTimer = 0;
//                    // dashParticleTimer = 0;
//                    // Player.velocity = Vector2.Zero; // 停止玩家的冲刺速度
//                    // break;
//                }
//            }
//        }

//        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
//        {
//            // 冲刺期间免疫敌人近战碰撞伤害
//            if (isDashing)
//            {
//                return false;
//            }
//            return true;
//        }

//        public override bool CanBeHitByProjectile(Projectile proj)
//        {
//            if (isDashing)
//            {
//                return false;
//            }
//            return true;
//        }
//    }
//}