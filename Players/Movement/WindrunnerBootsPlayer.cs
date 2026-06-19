using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;


namespace AccessoryForPlanetaryEmpire.Players.Movement
{
    public class WindrunnerBootsPlayer : ModPlayer
    {

        public bool hasSwiftWindWalkerBoots;
        public int CooldownTimer = 0;
        public int SpeedBoostTimer = 0;
        public const int CooldownTime = 45 * 60; // 45 seconds cooldown in frames (60 frames per second)
        public const int BuffDuration = 5 * 60; // 5 seconds buff duration in frames

        public override void ResetEffects()
        {
            hasSwiftWindWalkerBoots = false;
        }

        public override void PreUpdate()
        {
            if (hasSwiftWindWalkerBoots)
            {
                if (CooldownTimer > 0)
                {
                    CooldownTimer--;
                }

                if (KeybindSystem.WBOKeybind.JustPressed && CooldownTimer <= 0)
                {
                    ActivateSwiftWindWalkerEffect();
                    CooldownTimer = CooldownTime;
                }

                if (SpeedBoostTimer > 0)
                {
                    if (Player.HasBuff(ModContent.BuffType<WindrunnerBootsBuff>()))
                    {
                        SpeedBoostTimer--;
                        Player.moveSpeed += 15.25f; 
                        Player.runAcceleration *= 2.25f;
                        Player.maxRunSpeed *= 3.25f;
                    }
                    else
                    {
                        SpeedBoostTimer = 0;
                    }
                }
            }
        }

        private void ActivateSwiftWindWalkerEffect()
        {
            Player.AddBuff(ModContent.BuffType<WindrunnerBootsBuff>(), BuffDuration);
            SpeedBoostTimer = BuffDuration; // 设置速度提升的持续时间
        }

    }
}
