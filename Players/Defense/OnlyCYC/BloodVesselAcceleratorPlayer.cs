using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria;
using Terraria.ModLoader;
using System;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Accessory.Defense;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using Terraria.Audio;

namespace AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC
{
    public class BloodVesselAcceleratorPlayer : ModPlayer
    {
        public bool hasBloodVesselAccelerator;
        public const int SkillCooldownFrames = 30 * 60;
        public const int SkillDurationFrames = 10 * 60;
        public int SkillCooldownTimer;
        public int SkillActiveTimer;

        public override void ResetEffects()
        {
            hasBloodVesselAccelerator = false;
        }

        public override void UpdateLifeRegen()
        {
            if (hasBloodVesselAccelerator)
            {
                if (Player.statLife <= Player.statLifeMax2 * 0.15f) // 生命值低于15%
                {
                    Player.lifeRegen += 100;
                }
                else if (Player.statLife <= Player.statLifeMax2 * 0.5f) // 生命值低于50%
                {
                    Player.lifeRegen += 25;
                }
                else if (Player.statLife > Player.statLifeMax2 * 0.75f) // 生命值高于75%
                {
                    Player.lifeRegen += 0;
                }
            }
        }




        public override void UpdateDead()
        {
            SkillActiveTimer = 0;
        }

        public override void PostUpdate()
        {
            if (SkillCooldownTimer > 0)
            {
                SkillCooldownTimer--;
            }

            if (SkillActiveTimer > 0)
            {
                if (Player.HasBuff(ModContent.BuffType<BloodVesselAcceleratorSkillBuff>()))
                {
                    SkillActiveTimer--;
                }
                else
                {
                    SkillActiveTimer = 0;
                }
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (hasBloodVesselAccelerator && KeybindSystem.HHMBKeybind.JustPressed)
            {
                // 检查自上次使用技能以来是否已经过去了冷却时间
                if (SkillCooldownTimer <= 0)
                {
                    CombatText.NewText(Player.getRect(), Color.LightSkyBlue, "血管加速器的技能已触发！", dramatic: false, dot: false);
                    Player.AddBuff(ModContent.BuffType<BloodVesselAcceleratorSkillBuff>(), SkillDurationFrames); // 持续时间10秒
                    SoundEngine.PlaySound(SoundID.Item68, Player.Center);
                    SkillActiveTimer = SkillDurationFrames;
                    SkillCooldownTimer = SkillCooldownFrames;
                }
                else
                {
                    CombatText.NewText(Player.getRect(), Color.LightSkyBlue, "技能正在冷却！", dramatic: false, dot: false);
                }
            }
        }



    }
}
