using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class TamingWhipRingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Taming Whip Ring Buff");
            //Description.SetDefault("Increases whip damage and speed");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // 每次触发增加50%的伤害和15%的攻速，最高350%伤害和105%攻速
            int maxStacks = 7;
            float damageIncreasePerStack = 0.50f; // 每次叠加增加50%伤害
            float speedIncreasePerStack = 0.15f;  // 每次叠加增加15%攻速
            int buffDuration = 60; // 每层持续1秒（60 ticks）

            // 计算当前叠加层数
            int currentStacks = player.buffTime[buffIndex] / buffDuration;
            if (currentStacks > maxStacks)
            {
                currentStacks = maxStacks;
            }

            // 计算叠加的伤害和攻速加成
            player.GetDamage<SummonMeleeSpeedDamageClass>() += damageIncreasePerStack * currentStacks;
            player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += speedIncreasePerStack * currentStacks;

            // 限制最大叠加层数为7次
            if (player.buffTime[buffIndex] > maxStacks * buffDuration)
            {
                player.buffTime[buffIndex] = maxStacks * buffDuration;
            }
        }






    }
}