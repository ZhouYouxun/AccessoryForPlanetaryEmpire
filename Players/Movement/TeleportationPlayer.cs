using AccessoryForPlanetaryEmpire.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Global;

namespace AccessoryForPlanetaryEmpire.Players.Movement
{
    public class TeleportationPlayer : ModPlayer
    {
        public const int CooldownFrames = 15 * 60;
        public const int ActiveFrames = 5;

        public bool TSkillActive = false;
        public int TSkillTime = 0;
        public int TSkillCooldown = 0;
        public bool TSkillEffectsApplied = false;

        public bool TeleportationEquipped;


        public override void ResetEffects()
        {
            // 重置效果
            TeleportationEquipped = false;
        }

        public override void PostUpdateMiscEffects()
        {
            if (TeleportationEquipped)
            {
                if (Player.whoAmI == Main.myPlayer && TSkillCooldown <= 0 && !TSkillActive && CheckTeleportKeybind())
                {
                    ActivateTeleport();
                }

                // 检查技能冷却时间
                if (TSkillCooldown > 0)
                {
                    TSkillCooldown--; // 冷却时间减少
                }

                // 检查技能是否激活
                if (TSkillActive)
                {
                    // 检查技能持续时间
                    if (TSkillTime > 0)
                    {
                        if (!TSkillEffectsApplied)
                        {
                            // 瞬移到鼠标位置
                            Vector2 mousePosition = Main.MouseWorld;
                            Player.Teleport(mousePosition, -1);
                            Player.velocity = Vector2.Zero;

                            // 清除以自身为半径15格以内的所有敌方弹幕
                            for (int i = 0; i < Main.projectile.Length; i++)
                            {
                                Projectile projectile = Main.projectile[i];
                                if (projectile.active && projectile.hostile)
                                {
                                    float distance = Vector2.Distance(Player.Center, projectile.Center);
                                    if (distance <= 15 * 16) // 15格半径，每格16像素
                                    {
                                        projectile.active = false;
                                    }
                                }
                            }

                            // 创建魔法阵粒子特效
                            CreateMagicCircleEffect(Player.Center);

                            TSkillEffectsApplied = true; // 标记效果已应用
                        }

                        // 提升3000%伤害
                        //Player.GetDamage(DamageClass.Generic) += 30f;

                        // 持续时间减少
                        TSkillTime--;
                    }
                    else
                    {
                        // 设置冷却时间
                        TSkillCooldown = CooldownFrames;

                        //Player.GetDamage(DamageClass.Generic) -= 10f;

                        // 清除标记
                        TSkillActive = false;
                        TSkillEffectsApplied = false;
                    }
                }


            }
  
            
        }

        private bool CheckTeleportKeybind()
        {
            // 这里假设已经在 KeyPlayers.cs 和 KeybindSystem.cs 中定义了相应的按键
            // 返回按键是否被按下的布尔值
            return KeybindSystem.TKeybind.JustPressed;
        }

        private void ActivateTeleport()
        {
            // 设置技能持续时间和标记
            TSkillTime = ActiveFrames;
            TSkillActive = true;
        }

        // 创建魔法阵粒子特效的方法
        private void CreateMagicCircleEffect(Vector2 position)
        {
            int dustID = DustID.WhiteTorch;
            float duration = 2f; // 粒子的持续时间
            float sizeMultiplier = 4f; // 粒子的大小倍数
            float brightnessMultiplier = 5f; // 粒子的亮度倍数
            float radius = 15 * 16; // 15格半径，每格16像素
            int particleCount = 100; // 粒子数量

            // 创建圆形
            for (int i = 0; i < particleCount; i++)
            {
                float angle = i * (MathHelper.TwoPi / particleCount);
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;

                Vector2 particlePosition = position + velocity;
                Dust dust = Dust.NewDustPerfect(particlePosition, dustID, Vector2.Zero, 100, Color.White * brightnessMultiplier, 2f * sizeMultiplier);
                dust.noGravity = true;
                dust.fadeIn = duration;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
                dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
            }
        }

    }
}
