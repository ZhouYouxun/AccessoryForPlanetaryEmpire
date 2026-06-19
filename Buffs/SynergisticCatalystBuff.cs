using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class SynergisticCatalystBuff : ModBuff
    {
        public float damageIncrease;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Synergistic Catalyst Buff");
            //Description.SetDefault("Minion damage randomly fluctuates between 80% and 160%");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            SynergisticCatalystPlayer modPlayer = player.GetModPlayer<SynergisticCatalystPlayer>();

            // 在Buff持续期间，随机调整召唤物的伤害在80%到160%之间浮动
            float fluctuation = Main.rand.NextFloat(0.8f, 1.6f);
            player.GetDamage<SummonDamageClass>() *= fluctuation + modPlayer.damageIncrease;
        }


    }
}