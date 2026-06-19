//using AccessoryForPlanetaryEmpire.Global;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.GameInput;
//using Terraria;
//using Terraria.ModLoader;
//using AccessoryForPlanetaryEmpire.Buffs;
//using Microsoft.Xna.Framework;

//namespace AccessoryForPlanetaryEmpire.Players.Attack.JobSkill
//{
//    public class HandHeldMechanicalBucklerPlayer : ModPlayer
//    {
//        public bool hasHandHeldMechanicalBuckler;
//        private DateTime lastSkillUseTime = DateTime.MinValue; // 记录上次技能使用的时间
//        private const double skillCooldown = 30; // 冷却时间60秒

//        public override void ResetEffects()
//        {
//            hasHandHeldMechanicalBuckler = false;
//        }

//        public override void UpdateDead()
//        {
//            lastSkillUseTime = DateTime.MinValue; // 死亡时重置冷却
//        }

//        public override void ProcessTriggers(TriggersSet triggersSet)
//        {
//            if (hasHandHeldMechanicalBuckler && KeybindSystem.HHMBKeybind.JustPressed)
//            {
//                // 检查自上次使用技能以来是否已经过去了冷却时间
//                if ((DateTime.Now - lastSkillUseTime).TotalSeconds > skillCooldown)
//                {
//                    CombatText.NewText(Player.getRect(), Color.LightSkyBlue, "血管加速器的技能已触发！", dramatic: false, dot: false);
//                    Player.AddBuff(ModContent.BuffType<BloodVesselAcceleratorSkillBuff>(), 600); // 持续时间10秒
//                    lastSkillUseTime = DateTime.Now; // 更新上次技能使用的时间
//                }
//                else
//                {
//                    CombatText.NewText(Player.getRect(), Color.LightSkyBlue, "技能正在冷却！", dramatic: false, dot: false);
//                }
//            }
//        }
//    }
//}
