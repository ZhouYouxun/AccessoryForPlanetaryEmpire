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
//using Terraria.ID;
//using Microsoft.Xna.Framework;

//namespace AccessoryForPlanetaryEmpire.Players.Attack.JobSkill
//{
//    public class UnlimitedManaBackpackPlayer : ModPlayer
//    {
//        public bool hasUnlimitedManaBackpack;
//        private DateTime lastSkillUseTime; // 上次技能使用时间

//        public override void ResetEffects()
//        {
//            hasUnlimitedManaBackpack = false;
//            if (lastSkillUseTime == DateTime.MinValue)
//            {
//                // 初始化lastSkillUseTime为足够早以确保立即可用
//                lastSkillUseTime = DateTime.Now.AddSeconds(-60);
//            }
//        }

//        public override void ProcessTriggers(TriggersSet triggersSet)
//        {
//            if (hasUnlimitedManaBackpack && KeybindSystem.UMBKeybind.JustPressed)
//            {
//                // 检查当前时间是否超过了上次使用时间+冷却时间
//                if ((DateTime.Now - lastSkillUseTime).TotalSeconds >= 60)
//                {
//                    CombatText.NewText(Player.getRect(), Color.MediumPurple, "无限魔力背包技能已触发！", dramatic: false, dot: false);
//                    Player.AddBuff(ModContent.BuffType<UnlimitedManaBackpackPlayerSkillBuff>(), 600); // 持续时间10秒
//                    Player.AddBuff(BuffID.Obstructed, 600); // 添加Obstructed buff，持续10秒
//                    // 生成粒子特效
//                    CreateInfinitySymbolEffect(Player.Center);
//                    lastSkillUseTime = DateTime.Now; // 重置上次技能使用时间
//                }
//                else
//                {
//                    CombatText.NewText(Player.getRect(), Color.MediumPurple, "技能正在冷却！", dramatic: false, dot: false);
//                }
//            }
//        }

//        // 创建无穷符号粒子特效的方法
//        private void CreateInfinitySymbolEffect(Vector2 position)
//        {
//            int dustID = DustID.PurpleTorch;
//            float duration = 1.5f; // 粒子的持续时间
//            float sizeMultiplier = 4f; // 粒子的大小倍数
//            float brightnessMultiplier = 6f; // 粒子的亮度倍数
//            int particleCount = 50; // 粒子数量

//            for (int i = 0; i < particleCount; i++)
//            {
//                float t = i / (float)particleCount * MathHelper.TwoPi;
//                float x = 16 * (float)Math.Sin(t);
//                float y = 8 * (float)Math.Sin(2 * t);

//                Vector2 offset = new Vector2(x, y);
//                Dust dust = Dust.NewDustPerfect(position + offset, dustID, Vector2.Zero, 100, Color.Purple * brightnessMultiplier, 1.5f * sizeMultiplier);
//                dust.noGravity = true;
//                dust.fadeIn = duration;
//                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
//                dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
//                dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
//            }
//        }


//    }
//}
