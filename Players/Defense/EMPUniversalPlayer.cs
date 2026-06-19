using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using System.Linq;
using AccessoryForPlanetaryEmpire.Buffs;

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    public class EMPUniversalPlayer : ModPlayer
    {
        public bool hasUniversalProtectionDevice = false;
        public int cooldownUniversalProtectionDevice = 10 * 60;
        public int particleTimer = 60; // 粒子特效计时器
        public float empDamageIncrease = 0f; // 存储计算好的伤害增幅值

        public override void ResetEffects()
        {
            hasUniversalProtectionDevice = false;
        }

        public override void PostUpdate()
        {
            if (hasUniversalProtectionDevice && cooldownUniversalProtectionDevice > 0)
            {
                cooldownUniversalProtectionDevice--;
                if (cooldownUniversalProtectionDevice == 0)
                {
                    CombatText.NewText(Player.getRect(), Color.Cyan, "001型通用舰载全方位防护装置冷却成功！");
                }
            }

            if (hasUniversalProtectionDevice && KeybindSystem.ClearBulletKeybind.JustPressed && cooldownUniversalProtectionDevice == 0)
            {
                ActivateUniversalProtectionDevice();
                CombatText.NewText(Player.getRect(), Color.Cyan, "001型通用舰载全方位防护装置释放成功！");
                EMPAll.TriggerScreenShake(100f);
                cooldownUniversalProtectionDevice = 60 * 60; // 60秒冷却时间
                EMPAll.CreateCooldownCircle(250);
                Projectile.NewProjectile(Player.GetSource_Accessory(null), Player.Center, Vector2.Zero, ModContent.ProjectileType<UniversalBarrierProjectile>(), 0, 0, Player.whoAmI);
                EMPAll.ShootInterceptEffect(Player.Center, Color.White);
                SoundEngine.PlaySound(new SoundStyle("AccessoryForPlanetaryEmpire/Sounds/EMP"), Player.position);
            }



            foreach (Dust dust in Main.dust)
            {
                if (dust.active && dust.customData is float rotationSpeed)
                {
                    dust.rotation += rotationSpeed; // 旋转粒子
                    dust.scale *= 0.95f; // 更快地缩小粒子
                    dust.alpha += 10; // 更快地增加透明度
                }
            }



            if (hasUniversalProtectionDevice && Player.ownedProjectileCounts[ModContent.ProjectileType<EMPVis2>()] == 0)
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



      

        private void ActivateUniversalProtectionDevice()
        {
            int clearedCount = 0;
            float damageIncrease = 2f; // 每个清除的弹幕增加的伤害百分比
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.hostile && !projectile.friendly)
                {
                    if (Vector2.Distance(projectile.Center, Player.Center) <= 250 * 16f)
                    {
                        EMPAll.FireEMPUse(Player.Center, projectile.Center);
                        ExplodeProjectile(projectile);
                        ConvertProjectile(projectile);
                        clearedCount++;
                        projectile.Kill();
                    }
                }
            }

            Player.statDefense += clearedCount * 2;
            empDamageIncrease = damageIncrease * clearedCount / 100f;
            Player.AddBuff(ModContent.BuffType<EMPUniversalBuff>(), 15 * 60); // 15秒
        }

        private void ExplodeProjectile(Projectile projectile)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, projectile.Center) <= 5 * 16f)
                {
                    int damage = (int)(projectile.damage * 10f);
                    //npc.StrikeNPC(damage, 0f, 0, true);
                    npc.SimpleStrikeNPC(damage, 0, true, 0f, DamageClass.Generic, false);
                    npc.AddBuff(ModContent.BuffType<Uranium235Debuff>(), 600); // 10秒
                    EMPAll.FireEMPUse(projectile.Center, npc.Center); // 绘制粒子线
                }
            }
        }

        private void ConvertProjectile(Projectile projectile)
        {
            Projectile newProj = Projectile.NewProjectileDirect(Player.GetSource_Accessory(null), projectile.position, -projectile.velocity, projectile.type, projectile.damage * 5, projectile.knockBack, Player.whoAmI);
            newProj.hostile = false;
            newProj.friendly = true;
        }



    }
}