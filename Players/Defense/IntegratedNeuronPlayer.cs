using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense
{
    public class IntegratedNeuronPlayer : ModPlayer
    {
        public bool hasIntegratedNeuron = false;
        private int potionSicknessCounter;
        private int manaSicknessCounter;
        private int generalDebuffCounter;

        // 使用反射获取的黑名单Buff类型
        private int? adrenalineModeBuffType = null;
        private int? rageModeBuffType = null;

        public override void ResetEffects()
        {
            hasIntegratedNeuron = false;
        }

        public override void UpdateDead()
        {
            hasIntegratedNeuron = false;
        }





        public override void UpdateEquips()
        {
            // 如果装备了Integrated Neuron
            if (hasIntegratedNeuron)
            {
                // 如果启用了CalamityMod
                Mod calamityMod;
                if (ModLoader.TryGetMod("CalamityMod", out calamityMod))
                {
                    // 初始化 AdrenalineMode 和 RageMode Buff 类型
                    if (adrenalineModeBuffType == null)
                    {
                        var adrenalineBuff = calamityMod.Code.DefinedTypes.FirstOrDefault(t => t.Name == "AdrenalineMode");
                        adrenalineModeBuffType = adrenalineBuff?.GetField("Type", BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as int?;
                    }

                    if (rageModeBuffType == null)
                    {
                        var rageBuff = calamityMod.Code.DefinedTypes.FirstOrDefault(t => t.Name == "RageMode");
                        rageModeBuffType = rageBuff?.GetField("Type", BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as int?;
                    }
                }

                // 减少药水病时间
                if (++potionSicknessCounter >= 300)
                {
                    potionSicknessCounter = 0;
                    if (Player.HasBuff(BuffID.PotionSickness))
                    {
                        Player.buffTime[Player.FindBuffIndex(BuffID.PotionSickness)] = (int)(Player.buffTime[Player.FindBuffIndex(BuffID.PotionSickness)] * 0.95f);
                    }
                }

                // 减少魔力病时间
                if (++manaSicknessCounter >= 60)
                {
                    manaSicknessCounter = 0;
                    if (Player.HasBuff(BuffID.ManaSickness))
                    {
                        Player.buffTime[Player.FindBuffIndex(BuffID.ManaSickness)] = (int)(Player.buffTime[Player.FindBuffIndex(BuffID.ManaSickness)] * 0.75f);
                    }
                }

                // 减少一般 Debuff 时间，但排除黑名单中的 Buff
                if (++generalDebuffCounter >= 60)
                {
                    generalDebuffCounter = 0;
                    for (int i = 0; i < Player.buffTime.Length; i++)
                    {
                        int buffType = Player.buffType[i];
                        // 仅当这个 Buff 是 Debuff 时，才进行时间减少的逻辑，并排除黑名单中的 Buff
                        if (buffType > 0 && Player.buffTime[i] > 0 && Main.debuff[buffType] && !IsBuffInBlacklist(buffType))  // 调用 IsBuffInBlacklist
                        {
                            Player.buffTime[i] = (int)(Player.buffTime[i] * 0.7f);  // 减少非黑名单 Buff 的时间
                        }
                    }
                }
            }
        }




        private bool IsBuffInBlacklist(int buffType)
        {
            // 检查当前Buff是否在黑名单中
            //return buffType == adrenalineModeBuffType || buffType == rageModeBuffType;
            if (buffType == adrenalineModeBuffType || buffType == rageModeBuffType)
            {
                return true;
            }
            return false;
        }








    }
}