using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire.Global;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    public class EMPSupportBulletPlayer : ModPlayer
    {
        // 25d型屏障生成装置【3王后】：按下快捷键时在原地放置一个半径35的屏障，屏障阻挡一定数量的弹幕后消失，冷却时间为30秒
        // 25f型超时空场生成装置【3王后】：按下快捷键时在原地放置一个半径35的屏障，屏障可以为内部的所有玩家提供持续的无敌，离开这个范围不再提供，冷却时间为30秒

        // 定义所有装置的标志变量和冷却时间变量
        public bool has25dBarrier = false;
        public bool has25eSubSpace = false;

        public int cooldown25dBarrier = 0;
        public int cooldown25fSubSpace = 0;

        public int particleTimer = 60; // 粒子特效计时器

        public override void ResetEffects()
        {
            has25dBarrier = false;
            has25eSubSpace = false;
        }

        public override void PostUpdate()
        {
            // -----------------------------------------
            // 35d型屏障生成装置【3王后】
            // -----------------------------------------
            if (has25dBarrier && cooldown25dBarrier > 0)
            {
                cooldown25dBarrier--;
                if (cooldown25dBarrier == 0)
                {
                    CombatText.NewText(Player.getRect(), Color.Cyan, "25d型屏障生成装置冷却成功！");
                    SoundEngine.PlaySound(SoundID.Item4, Player.position); // 播放冷却完成音效（魔力水晶）
                }
            }
            if (has25dBarrier && KeybindSystem.ClearBulletKeybind.JustPressed && cooldown25dBarrier == 0)
            {
                CreateBarrier(Player.Center, ModContent.ProjectileType<BarrierProjectile>());
                EMPAll.ShootInterceptEffect(Player.Center, Color.DarkGray);
                cooldown25dBarrier = 30 * 60; // 30秒冷却时间
                SoundEngine.PlaySound(new SoundStyle("AccessoryForPlanetaryEmpire/Sounds/EMP"), Player.position);
            }

            // -----------------------------------------
            // 25f型超时空场生成装置【3王后】
            // -----------------------------------------
            if (has25eSubSpace && cooldown25fSubSpace > 0)
            {
                cooldown25fSubSpace--;
                if (cooldown25fSubSpace == 0)
                {
                    CombatText.NewText(Player.getRect(), Color.Cyan, "25f型超时空场生成装置冷却成功！");
                    SoundEngine.PlaySound(SoundID.Item4, Player.position); // 播放冷却完成音效（魔力水晶）
                }
            }
            if (has25eSubSpace && KeybindSystem.ClearBulletKeybind.JustPressed && cooldown25fSubSpace == 0)
            {
                CreateBarrier(Player.Center, ModContent.ProjectileType<SubSpaceProjectile>());
                EMPAll.ShootInterceptEffect(Player.Center, Color.MidnightBlue);
                cooldown25fSubSpace = 30 * 60; // 30秒冷却时间
                SoundEngine.PlaySound(new SoundStyle("AccessoryForPlanetaryEmpire/Sounds/EMP"), Player.position);
            }



            if ((has25dBarrier || has25eSubSpace) && Player.ownedProjectileCounts[ModContent.ProjectileType<EMPVis2>()] == 0)
            {
                Projectile.NewProjectile(
                    Player.GetSource_Accessory(null),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<EMPVis2>(),
                    0,
                    0,
                    Player.whoAmI
                );
            }
        }

        // 创建屏障
        private void CreateBarrier(Vector2 position, int type)
        {
            Projectile.NewProjectile(Player.GetSource_Accessory(null), position, Vector2.Zero, type, 0, 0, Player.whoAmI);
        }




    }
}