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

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    public class ShieldGeneratorOnDeploymentPlayer : ModPlayer
    {


        public bool hasShieldGeneratorOnDeployment = false;


        public override void ResetEffects()
        {
            // 确保每帧重置
            hasShieldGeneratorOnDeployment = false;
        }

        private bool justRespawned = false;

        public override void PreUpdate()
        {
            // 如果玩家之前死亡，现在生命值大于0，可以认为玩家刚刚重生
            if (justRespawned && Player.statLife > 0)
            {
                // 玩家刚刚重生，执行重生逻辑
                // 只有在玩家装备了特定物品时才应用Buff
                if (hasShieldGeneratorOnDeployment)
                {
                    CombatText.NewText(Player.getRect(), Color.White, "  你 复 活 辣  ", dramatic: false, dot: false);
                    Player.AddBuff(ModContent.BuffType<INV>(), 600, true);
                    Player.immune = true;
                    Player.immuneTime = 600; // 10秒无敌时间
                }
                justRespawned = false;
            }

            // 如果玩家死亡，设置标志以便在下一帧检测到重生
            if (Player.dead)
            {
                justRespawned = true;
            }
        }



    }
}

