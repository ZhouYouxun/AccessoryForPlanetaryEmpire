using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Global;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using System;
using System;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    public class PeriodicBarrierBoosterPlayer : ModPlayer
    {
        // 周期性屏障装置player文件
        public bool hasPeriodicBarrierBooster = false;
        private int shieldProjectileId = -1;
        public const int CooldownFrames = 2700; // 45秒 (60帧 * 45秒)
        public const int ActiveFrames = 300; // 5秒无敌
        public int CooldownTimer = CooldownFrames;
        public int ActiveTimer;
        public bool IsBarrierActive => ActiveTimer > 0;

        public override void ResetEffects()
        {
            hasPeriodicBarrierBooster = false;
        }

        public override void PostUpdate()
        {
            if (hasPeriodicBarrierBooster)
            {
                if (ActiveTimer > 0)
                {
                    if (Player.HasBuff(ModContent.BuffType<INV>()))
                    {
                        ActiveTimer--;
                        if (ActiveTimer <= 0)
                        {
                            CooldownTimer = CooldownFrames;
                        }
                    }
                    else
                    {
                        ActiveTimer = 0;
                        CooldownTimer = CooldownFrames;
                    }

                    // 确保 BlueRoundShield 弹幕始终跟随玩家
                    if (shieldProjectileId == -1 || !Main.projectile[shieldProjectileId].active || Main.projectile[shieldProjectileId].type != ModContent.ProjectileType<BlueRoundShield>())
                    {
                        shieldProjectileId = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<BlueRoundShield>(), 0, 0, Player.whoAmI);
                    }
                }
                else
                {
                    if (CooldownTimer > 0)
                    {
                        CooldownTimer--;
                    }

                    if (CooldownTimer <= 0)
                    {
                        Player.AddBuff(ModContent.BuffType<INV>(), ActiveFrames);
                        ActiveTimer = ActiveFrames;
                        CombatText.NewText(Player.getRect(), Color.Brown, "周期屏障装置已开启！", dramatic: false, dot: false);

                        if (shieldProjectileId == -1 || !Main.projectile[shieldProjectileId].active || Main.projectile[shieldProjectileId].type != ModContent.ProjectileType<BlueRoundShield>())
                        {
                            shieldProjectileId = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<BlueRoundShield>(), 0, 0, Player.whoAmI);
                        }
                    }
                }
            }
            else
            {
                shieldProjectileId = -1; // 禁用时重置 shieldProjectileId
            }
        }









    }
}
