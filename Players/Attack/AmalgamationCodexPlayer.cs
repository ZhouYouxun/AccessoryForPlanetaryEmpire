using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Accessory.Attack;
using System.Reflection;

namespace AccessoryForPlanetaryEmpire.Players.Attack
{
    public class AmalgamationCodexPlayer : ModPlayer
    {
        public bool hasAmalgamationCodex;
        public bool downedBoss2; // 第一个邪恶boss被打败的开关
        public bool hardMode; // 困难模式开启的开关
        public bool downedGolemBoss; // 石巨人被打败的开关

        public override void ResetEffects()
        {
            hasAmalgamationCodex = false;
        }
        private int? GetRandomDebuffFromAllMods(NPC target)
        {
            List<int> validDebuffs = new();

            foreach (Mod mod in ModLoader.Mods)
            {
                foreach (ModBuff buff in mod.GetContent<ModBuff>())
                {
                    int buffType = buff.Type;

                    // ✅ 条件筛选：不能免疫、不能已有、必须是 debuff
                    //if (!target.buffImmune[buffType] &&
                    //    !target.HasBuff(buffType) &&
                    //    Main.debuff[buffType] &&
                    //    !buff.Name.ToLower().Contains("summon"))
                    if (!target.buffImmune[buffType] &&
                        !target.HasBuff(buffType) &&
                        Main.debuff[buffType])
                    {
                        // 添加额外限制
                        string name = buff.Name;

                        if (
                            (name == "HolyFlames" || name == "GodSlayerInferno" || name == "ElementalMix" || name == "Dragonfire") &&
                            !NPC.downedPlantBoss)
                            continue;

                        if (
                            (name == "MarkedforDeath" || name == "KamiFlu" || name == "WhisperingDeath" || name == "TimeDistortion" || name == "GlacialState") &&
                            !Main.hardMode)
                            continue;

                        // 通过所有条件，添加
                        validDebuffs.Add(buffType);
                    }
                }
            }

            if (validDebuffs.Count > 0)
                return validDebuffs[Main.rand.Next(validDebuffs.Count)];

            return null; // 没找到合适的
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasAmalgamationCodex)
            {
                // 添加一个几率触发
                if (Main.rand.NextFloat() < 0.35f)
                {
                    int? randomDebuff = GetRandomDebuffFromAllMods(target);
                    if (randomDebuff.HasValue)
                    {
                        int duration = Main.getGoodWorld ? 1500 : 300;
                        target.AddBuff(randomDebuff.Value, duration);
                    }
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasAmalgamationCodex)
            {
                //List<int> debuffs = new List<int>()
                //{
                //    BuffID.OnFire,           // 着火
                //    BuffID.Poisoned,         // 中毒
                //    BuffID.Confused,         // 困惑
                //    BuffID.Frostburn,        // 冻伤
                //    BuffID.Ichor,            // 灵液
                //    BuffID.CursedInferno,    // 诅咒地狱火
                //    BuffID.ShadowFlame,      // 暗影火
                //    BuffID.Daybreak,         // 黎明碎片
                //    BuffID.BetsysCurse,      // 贝希的诅咒
                //    BuffID.Midas,            // 迈达斯
                //    BuffID.Wet,              // 湿润
                //    BuffID.Suffocation,      // 窒息
                //    BuffID.Burning,          // 燃烧
                //    BuffID.Bleeding,         // 流血
                //    BuffID.Weak,             // 虚弱
                //    BuffID.WitheredArmor,    // 枯萎的盔甲
                //    BuffID.WitheredWeapon,   // 枯萎的武器
                //    BuffID.Silenced,         // 沉默
                //    BuffID.BrokenArmor,      // 盔甲破损
                //    BuffID.Slimed,           // 粘液
                //    BuffID.Slow,             // 减速
                //    BuffID.Stoned,           // 石化
                //    BuffID.Venom,            // 剧毒
                //    BuffID.Dazed,            // 眩晕
                //    BuffID.ObsidianSkin,     // 黑曜石皮肤
                //    BuffID.OgreSpit          // 食人魔的唾液
                //};

                //// 检查是否加载了 CalamityMod
                //Mod calamityMod;
                //if (ModLoader.TryGetMod("CalamityMod", out calamityMod))
                //{
                //    // 根据游戏进度添加不同的减益效果

                //    // 如果启用了 Main.getGoodWorld 模式，直接添加所有阶段性减益到默认池
                //    if (Main.getGoodWorld)
                //    {
                //        // 石巨人阶段的减益直接加入池
                //        debuffs.Add(calamityMod.Find<ModBuff>("MiracleBlight").Type);      // 超位崩解：每秒失去1250生命值
                //        debuffs.Add(calamityMod.Find<ModBuff>("VulnerabilityHex").Type);   // 孱弱巫咒：每秒失去1332点生命值，极大幅度降低移速

                //        // 困难模式的减益直接加入池
                //        debuffs.Add(calamityMod.Find<ModBuff>("MarkedforDeath").Type);     // 死亡标记：伤害减免与防御力归零
                //        debuffs.Add(calamityMod.Find<ModBuff>("KamiFlu").Type);            // 卡米之疫：降低20%敌人的伤害减免并每秒损失125点生命值
                //        debuffs.Add(calamityMod.Find<ModBuff>("WhisperingDeath").Type);    // 死亡低语：减少10%接触伤害
                //        debuffs.Add(calamityMod.Find<ModBuff>("TimeDistortion").Type);     // 时间扭曲：降低15%的移动速度
                //        debuffs.Add(calamityMod.Find<ModBuff>("GlacialState").Type);       // 冰河时代：瞬间完全静止

                //        // 第一个邪恶boss阶段的减益直接加入池
                //        debuffs.Add(calamityMod.Find<ModBuff>("HolyFlames").Type);         // 神圣火焰：每秒失去100生命值
                //        debuffs.Add(calamityMod.Find<ModBuff>("GodSlayerInferno").Type);   // 弑神怒焰：每秒失去125生命值
                //        debuffs.Add(calamityMod.Find<ModBuff>("ElementalMix").Type);       // 元素谐鸣：每秒失去200生命值
                //        debuffs.Add(calamityMod.Find<ModBuff>("Dragonfire").Type);         // 巨龙之焰：每秒失去480生命值
                //    }
                //    else
                //    {
                //        // 如果启用了 Main.getGoodWorld 模式，直接添加所有阶段性减益到默认池
                //        // 如果没有启用 Main.getGoodWorld 模式，则按正常的游戏进度检测来添加减益效果

                //        // 如果击败石巨人，则添加这些减益效果
                //        if (downedGolemBoss)
                //        {
                //            debuffs.Add(calamityMod.Find<ModBuff>("MiracleBlight").Type);      // 超位崩解：每秒失去1250生命值
                //            debuffs.Add(calamityMod.Find<ModBuff>("VulnerabilityHex").Type);   // 孱弱巫咒：每秒失去1332点生命值，极大幅度降低移速
                //        }

                //        // 如果进入困难模式，则添加这些减益效果
                //        if (hardMode)
                //        {
                //            debuffs.Add(calamityMod.Find<ModBuff>("MarkedforDeath").Type);     // 死亡标记：伤害减免与防御力归零
                //            debuffs.Add(calamityMod.Find<ModBuff>("KamiFlu").Type);            // 卡米之疫：降低20%敌人的伤害减免并每秒损失125点生命值
                //            debuffs.Add(calamityMod.Find<ModBuff>("WhisperingDeath").Type);    // 死亡低语：减少10%接触伤害
                //            debuffs.Add(calamityMod.Find<ModBuff>("TimeDistortion").Type);     // 时间扭曲：降低15%的移动速度
                //            debuffs.Add(calamityMod.Find<ModBuff>("GlacialState").Type);       // 冰河时代：瞬间完全静止
                //        }

                //        // 如果击败第一个邪恶Boss，则添加这些减益效果
                //        if (downedBoss2)
                //        {
                //            debuffs.Add(calamityMod.Find<ModBuff>("HolyFlames").Type);         // 神圣火焰：每秒失去100生命值
                //            debuffs.Add(calamityMod.Find<ModBuff>("GodSlayerInferno").Type);   // 弑神怒焰：每秒失去125生命值
                //            debuffs.Add(calamityMod.Find<ModBuff>("ElementalMix").Type);       // 元素谐鸣：每秒失去200生命值
                //            debuffs.Add(calamityMod.Find<ModBuff>("Dragonfire").Type);         // 巨龙之焰：每秒失去480生命值
                //        }
                //    }

                //    // 不检测阶段也能触发的效果，使用共享概率池
                //    debuffs.Add(calamityMod.Find<ModBuff>("AstralInfectionDebuff").Type);  // 星体感染：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("BanishingFire").Type);          // 驱逐之火：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("BrimstoneFlames").Type);        // 硫磺火焰：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("BurningBlood").Type);           // 燃烧之血：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("CrushDepth").Type);             // 深海压力：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("Nightwither").Type);            // 夜枯萎：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("Plague").Type);                 // 瘟疫：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("RancorBurn").Type);             // 怨恨之火：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("RiptideDebuff").Type);          // 激流减益：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("SagePoison").Type);             // 圣贤之毒：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("Shadowflame").Type);            // 暗影火：持续伤害
                //    debuffs.Add(calamityMod.Find<ModBuff>("Vaporfied").Type);              // 蒸汽波：每秒造成15点伤害并减少10%的移速
                //    debuffs.Add(calamityMod.Find<ModBuff>("GalvanicCorrosion").Type);      // 电偶腐蚀：瞬间完全停止不动
                //    debuffs.Add(calamityMod.Find<ModBuff>("Eutrophication").Type);         // 富营养化：瞬间完全静止
                //    debuffs.Add(calamityMod.Find<ModBuff>("TemporalSadness").Type);        // 莫名的悲伤：降低敌人50%的接触伤害并使敌人水平速度-90%
                //}

                //int randomDebuff = debuffs[Main.rand.Next(debuffs.Count)];
                //if (Main.getGoodWorld)
                //{
                //    target.AddBuff(randomDebuff, 1500); // 持续时间为25秒（1500帧）
                //}
                //else
                //{
                //    target.AddBuff(randomDebuff, 300); // 持续时间为5秒（300帧）
                //}

                int? randomDebuff = GetRandomDebuffFromAllMods(target);
                if (randomDebuff.HasValue)
                {
                    int duration = Main.getGoodWorld ? 1500 : 300;
                    target.AddBuff(randomDebuff.Value, duration);
                }


            }
        }








    }
}
