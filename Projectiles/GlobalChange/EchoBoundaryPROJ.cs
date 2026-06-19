using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Players;
using System.Reflection;
using Terraria.Audio;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class EchoBoundaryPROJ : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private float initialSpeed = -1; // 初始化为-1，表示未设置
        public int bounceCount = 0; // 反弹次数
        private int checkFrameCounter = 0; // 计时器，用于控制检测频率
        private int frameCounter = 0; // 记录弹幕生成后的帧数

        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            var modPlayer = player.GetModPlayer<EchoBoundaryPlayer>();
            frameCounter++; // 增加帧计数器

            // 每10帧检测一次
            if (checkFrameCounter > 0)
            {
                checkFrameCounter--;
                return;
            }


            if (modPlayer.hasEchoBoundary && IsEligibleProjectile(projectile) && frameCounter > 30) // 30帧后才允许反弹
            {
                // 如果初始速度未设置，记录初始速度
                if (initialSpeed == -1)
                {
                    initialSpeed = projectile.velocity.Length();
                }

                if (bounceCount < 5) // 最多反弹5次
                {
                    Vector2 newVelocity = projectile.velocity;

                    // 检测是否撞到屏幕边缘
                    bool hasBounced = false;

                    if (projectile.position.X <= Main.screenPosition.X || projectile.position.X + projectile.width >= Main.screenPosition.X + Main.screenWidth)
                    {
                        newVelocity.X = -projectile.velocity.X; // 反弹X轴方向
                        hasBounced = true;
                    }
                    if (projectile.position.Y <= Main.screenPosition.Y || projectile.position.Y + projectile.height >= Main.screenPosition.Y + Main.screenHeight)
                    {
                        newVelocity.Y = -projectile.velocity.Y; // 反弹Y轴方向
                        hasBounced = true;
                    }

                    // 只有在实际反弹时才会更新速度、伤害，并增加反弹计数
                    if (hasBounced && newVelocity != projectile.velocity)
                    {
                        projectile.velocity = newVelocity;
                        projectile.damage = (int)(projectile.damage * 1.05f);
                        bounceCount++;

                        // 🌌 撞墙时播放光点
                        // 💥 加入光点特效
                        Vector2 reflectDir = projectile.velocity.SafeNormalize(Vector2.UnitX);
                        //PLALightingBoltsSystem.PLA_Spawn_EchoSpiral(projectile.Center, reflectDir);
                    }
                }

                //ReflectIfNearScreenEdge(projectile, player);
            }

            checkFrameCounter = 20; // 10帧检测一次
        }

        public override void SetDefaults(Projectile projectile)
        {
            // 增加额外的更新次数，例如将更新次数设置为2次每帧
            // projectile.extraUpdates = 5;
        }


        // 放在 AI 或 PostAI 中每帧判断（不再用 checkFrameCounter）
        //public void ReflectIfNearScreenEdge(Projectile projectile, Player player)
        //{
        //    var modPlayer = player.GetModPlayer<EchoBoundaryPlayer>();

        //    if (!modPlayer.hasEchoBoundary) // 判断是否佩戴装备
        //        return;

        //    Vector2 nextPos = projectile.Center + projectile.velocity;
        //    Rectangle screen = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

        //    float buffer = 8f; // 容差（提前距离），单位像素

        //    bool reflected = false;
        //    Vector2 newVelocity = projectile.velocity;

        //    // 左右边缘
        //    if (nextPos.X < screen.Left + buffer && newVelocity.X < 0f)
        //    {
        //        newVelocity.X *= -1f;
        //        reflected = true;
        //    }
        //    else if (nextPos.X > screen.Right - buffer && newVelocity.X > 0f)
        //    {
        //        newVelocity.X *= -1f;
        //        reflected = true;
        //    }

        //    // 上下边缘
        //    if (nextPos.Y < screen.Top + buffer && newVelocity.Y < 0f)
        //    {
        //        newVelocity.Y *= -1f;
        //        reflected = true;
        //    }
        //    else if (nextPos.Y > screen.Bottom - buffer && newVelocity.Y > 0f)
        //    {
        //        newVelocity.Y *= -1f;
        //        reflected = true;
        //    }

        //    if (reflected)
        //    {
        //        projectile.velocity = newVelocity;
        //        projectile.damage = (int)(projectile.damage * 1.15f);

        //        // 💥 加入光点特效
        //        Vector2 reflectDir = projectile.velocity.SafeNormalize(Vector2.UnitX);
        //        PLALightingBoltsSystem.PLA_Spawn_EchoSpiral(projectile.Center, reflectDir);

        //        // 💡 可选：短暂无敌时间或反弹音效（比如 SoundID.Item74）
        //        SoundEngine.PlaySound(SoundID.Item74 with { Pitch = -0.2f }, projectile.Center);
        //    }
        //}



        private bool IsEligibleProjectile(Projectile projectile)
        {
            // 默认的职业类型检测
            bool isEligible = projectile.DamageType == DamageClass.Melee ||
                              projectile.DamageType == DamageClass.Ranged ||
                              projectile.DamageType == DamageClass.Magic ||
                              projectile.DamageType == DamageClass.Throwing;

            // 尝试加载 CalamityMod 模组并检查 RogueDamageClass
            Mod calamityMod;
            if (ModLoader.TryGetMod("CalamityMod", out calamityMod))
            {
                // 使用反射获取 RogueDamageClass 的实例
                var rogueType = calamityMod.Code.DefinedTypes.FirstOrDefault(t => t.Name == "RogueDamageClass");

                if (rogueType != null)
                {
                    FieldInfo rogueInstanceField = rogueType.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
                    var rogueInstance = rogueInstanceField?.GetValue(null) as DamageClass;
                    if (rogueInstance != null && projectile.DamageType == rogueInstance)
                    {
                        isEligible = true; // 允许 Rogue 类型弹幕也能享受反弹效果
                    }
                }
            }

            return isEligible;
        }



    }
}