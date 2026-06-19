using AccessoryForPlanetaryEmpire.Global;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs;

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    public class EMP65LPlayer : ModPlayer
    {
        // 65L型激光弹幕拦截器【花后】：摁住快捷键时，以每秒三发弹幕的速度，从距离玩家最近开始清除，总清除次数上限为150次，最大半径为65格。

        public bool has65LInterceptor = false;
        public int chargeCount = 0; // 当前充能次数
        public const int maxCharge = 150; // 最大充能次数
        public const int radius = 65; // 最大半径
        public int particleTimer = 60; // 粒子特效计时器
        public bool TriggeredShrinkThisFrame = false;
        public int GetCurrentCharge() => chargeCount;

        public override void ResetEffects()
        {
            has65LInterceptor = false;
        }

        public override void PostUpdate()
        {
            if(has65LInterceptor)
            {
                // 每秒充能一次
                if (chargeCount < maxCharge)
                {
                    chargeCount++;
                }

                // 每秒生成吸收粒子特效
                if (particleTimer > 0)
                {
                    particleTimer--;
                }
                else
                {
                    particleTimer = 60; // 每秒产生一次粒子特效
                    //CreateTrackingParticles();
                }

                // 清除弹幕逻辑
                if (has65LInterceptor && KeybindSystem.ClearBulletKeybind.JustPressed && chargeCount > 0)
                {
                    ClearBullets(radius, 1); // 每次按下快捷键清除一个弹幕
                    chargeCount--; // 减少一次清理次数
                    //SoundEngine.PlaySound(SoundID.Item4, Player.position); // 播放魔力水晶音效

                    TriggeredShrinkThisFrame = true; // 💡 通知弹幕进行缩放

                    // 绘制粒子特效圆圈
                    EMPAll.CreateCooldownCircle(radius);
                }


                if (has65LInterceptor && Player.ownedProjectileCounts[ModContent.ProjectileType<EMPVis65L>()] == 0)
                {
                    Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        Player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<EMPVis65L>(),
                        0,
                        0f,
                        Player.whoAmI
                    );
                }

            }

        }

        // 清除敌方弹幕
        private void ClearBullets(int radius, int count)
        {
            List<Projectile> hostileProjectiles = new List<Projectile>();
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.hostile && !projectile.friendly)
                {
                    if (Vector2.Distance(projectile.Center, Player.Center) <= radius * 16f)
                    {
                        hostileProjectiles.Add(projectile);
                    }
                }
            }

            hostileProjectiles = hostileProjectiles.OrderBy(p => Vector2.Distance(p.Center, Player.Center)).ToList();
            for (int i = 0; i < count && i < hostileProjectiles.Count; i++)
            {
                EMPAll.FireEMPUse(Player.Center, hostileProjectiles[i].Center);
                hostileProjectiles[i].Kill();
            }
            SoundEngine.PlaySound(new SoundStyle("AccessoryForPlanetaryEmpire/Sounds/LEMP"), Player.position);

        }

      
    }
}
