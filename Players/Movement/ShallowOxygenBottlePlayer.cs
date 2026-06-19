using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Accessory.Dash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.Movement
{
    public class ShallowOxygenBottlePlayer : ModPlayer
    {
        public bool hasShallowOxygenBottle = false;   // 使用特殊按键冲刺
        public bool hasShallowOxygenBottleAD = false; // 使用双击方向键冲刺
        public bool hasCommandersSaw = false;         // 新增，使用特殊按键冲刺
        public bool hasCommandersSawAD = false;       // 新增，使用双击方向键冲刺
        public bool isDashing = false;
        public int dashCooldown = 0;
        private int dashParticleTimer = 0;
        private int invincibleTimer = 0;
        public int DashDir = -1; // 用于左右双击的方向检测

        public override void ResetEffects()
        {
            hasShallowOxygenBottle = false;
            hasShallowOxygenBottleAD = false;
            hasCommandersSaw = false;         // 重置CommandersSaw开关
            hasCommandersSawAD = false;       // 重置CommandersSaw双击开关

            // 双击左右键检测
            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15)
            {
                DashDir = 2; // 右
            }
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15)
            {
                DashDir = 3; // 左
            }
            else
            {
                DashDir = -1; // 重置方向
            }
        }

        public override void PostUpdate()
        {
            if (dashCooldown > 0)
            {
                dashCooldown--;
            }

            // 使用双击方向键冲刺
            if ((hasShallowOxygenBottleAD || hasCommandersSawAD) && DashDir != -1 && dashCooldown == 0)
            {
                Dash();
            }
            // 使用特殊按键冲刺
            else if ((hasShallowOxygenBottle || hasCommandersSaw) && KeybindSystem.DASHKeybind.JustPressed && dashCooldown == 0)
            {
                Dash();
            }

            // 粒子效果和无敌时间处理
            if (dashParticleTimer > 0)
            {
                dashParticleTimer--;
                if (hasShallowOxygenBottle || hasShallowOxygenBottleAD)
                {
                    CreateWaterDust(); // 绘制蓝色六芒星粒子
                }
                else if (hasCommandersSaw || hasCommandersSawAD)
                {
                    CreateRedDust(); // 绘制红色六芒星粒子
                }
            }

            if (invincibleTimer > 0)
            {
                invincibleTimer--;
                if (invincibleTimer == 0)
                {
                    isDashing = false;
                }
            }

            if (isDashing)
            {
                CheckDashCollision();
            }
        }

        // 执行冲刺功能
        private void Dash()
        {
            Vector2 dashDirection = Player.velocity;

            if (dashDirection != Vector2.Zero)
            {
                dashDirection.Normalize(); // 保持当前的移动方向
                float dashSpeed = hasCommandersSaw || hasCommandersSawAD ? 18f * 0.7f : 18f; // 如果是CommandersSaw，冲刺速度慢30%
                Player.velocity = dashDirection * dashSpeed; // 根据启用的饰品设置不同速度
                isDashing = true;
                dashCooldown = 60; // 冷却时间为1秒
                dashParticleTimer = 60; // 粒子生成时间为1秒
                invincibleTimer = 40; // 无敌时间为40帧
            }
        }

        // 创建蓝色六芒星水能粒子
        private void CreateWaterDust()
        {
            float progress = (60 - dashParticleTimer) / 60f; // 计算粒子生成的进度
            float radius = 10f * (1 - progress); // 计算当前半径
            float sizeMultiplier = 3.0f; // 粒子的大小倍数
            float brightnessMultiplier = 6f; // 粒子的亮度倍数

            // 六芒星形状的控制点
            Vector2[] points = {
                new Vector2(0, -3.5f), // 上
                new Vector2(1.5f, -2.625f), // 右上1
                new Vector2(3.03f, -1.75f), // 右上2
                new Vector2(3.03f, 1.75f), // 右下2
                new Vector2(1.5f, 2.625f), // 右下1
                new Vector2(0, 3.5f), // 下
                new Vector2(-1.5f, 2.625f), // 左下1
                new Vector2(-3.03f, 1.75f), // 左下2
                new Vector2(-3.03f, -1.75f), // 左上2
                new Vector2(-1.5f, -2.625f) // 左上1
            };

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 position = Player.Center + points[i] * radius;
                CreateParticle(position, sizeMultiplier, brightnessMultiplier, DustID.BlueTorch); // 蓝色粒子
            }
        }

        // 创建红色六芒星粒子
        private void CreateRedDust()
        {
            float progress = (60 - dashParticleTimer) / 60f; // 计算粒子生成的进度
            float radius = 10f * (1 - progress); // 计算当前半径
            float sizeMultiplier = 3.0f; // 粒子的大小倍数
            float brightnessMultiplier = 6f; // 粒子的亮度倍数

            // 六芒星形状的控制点
            Vector2[] points = {
                new Vector2(0, -3.5f), // 上
                new Vector2(1.5f, -2.625f), // 右上1
                new Vector2(3.03f, -1.75f), // 右上2
                new Vector2(3.03f, 1.75f), // 右下2
                new Vector2(1.5f, 2.625f), // 右下1
                new Vector2(0, 3.5f), // 下
                new Vector2(-1.5f, 2.625f), // 左下1
                new Vector2(-3.03f, 1.75f), // 左下2
                new Vector2(-3.03f, -1.75f), // 左上2
                new Vector2(-1.5f, -2.625f) // 左上1
            };

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 position = Player.Center + points[i] * radius;
                CreateParticle(position, sizeMultiplier, brightnessMultiplier, DustID.RedTorch); // 红色粒子
            }
        }

        private void CreateParticle(Vector2 position, float sizeMultiplier, float brightnessMultiplier, int dustType)
        {
            Dust dust = Dust.NewDustPerfect(position, dustType, null, 100, Color.White * brightnessMultiplier, 1.5f * sizeMultiplier);
            dust.noGravity = true;
            dust.fadeIn = 1f;
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
            dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
            dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
        }

        private void CheckDashCollision()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && Player.getRect().Intersects(npc.getRect()))
                {
                    npc.StrikeNPC(new NPC.HitInfo
                    {
                        Damage = 75, // 冲刺伤害
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

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            // 冲刺期间免疫敌人近战碰撞伤害
            if (isDashing)
            {
                return false;
            }
            return true;
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (isDashing)
            {
                return false;
            }
            return true;
        }
    }
}
