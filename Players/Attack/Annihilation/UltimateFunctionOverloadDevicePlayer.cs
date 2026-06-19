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

namespace AccessoryForPlanetaryEmpire.Players.Attack.Annihilation
{
    public class UltimateFunctionOverloadDevicePlayer : ModPlayer
    {
        public const int SkillCooldownFrames = 60 * 60;
        public const int SkillDurationFrames = 15 * 60;
        public int SkillCooldownTimer;
        public int SkillActiveTimer;
        public bool hasUltimateFunctionOverloadDevice;

        public override void ResetEffects()
        {
            hasUltimateFunctionOverloadDevice = false;
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
                if (Player.HasBuff(ModContent.BuffType<UltimateFunctionOverloadDeviceBuff>()))
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
            if (hasUltimateFunctionOverloadDevice && KeybindSystem.UFODKeybind.JustPressed)
            {
                // 检查自上次使用技能以来是否已经过去了冷却时间
                if (SkillCooldownTimer <= 0)
                {
                    CombatText.NewText(Player.getRect(), Color.LightBlue, "超频V型的技能已触发！", dramatic: false, dot: false);
                    Player.AddBuff(ModContent.BuffType<UltimateFunctionOverloadDeviceBuff>(), SkillDurationFrames); // 持续时间15秒

                    // 如果不是getGoodWorld模式，才施加Obstructed buff
                    if (!Main.getGoodWorld)
                    {
                        Player.AddBuff(BuffID.Obstructed, SkillDurationFrames); // 添加Obstructed buff，持续15秒
                    }

                    SkillActiveTimer = SkillDurationFrames;
                    SkillCooldownTimer = SkillCooldownFrames;
                }
                else
                {
                    CombatText.NewText(Player.getRect(), Color.LightBlue, "技能正在冷却！", dramatic: false, dot: false);
                }
            }
        }


    }
}
