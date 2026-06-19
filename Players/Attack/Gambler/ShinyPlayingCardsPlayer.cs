using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Gambler
{
    public class ShinyPlayingCardsPlayer : ModPlayer
    {
        public bool hasShinyPlayingCards;

        public override void ResetEffects()
        {
            hasShinyPlayingCards = false;
        }

        public override void PostUpdate()
        {
            if (hasShinyPlayingCards)
            {
                RemoveInvincibilityFrames();
            }
        }

        private void RemoveInvincibilityFrames()
        {
            if (!Main.getGoodWorld)
            {
                Player.immuneTime = 0; // 移除玩家无敌帧
                Player.hurtCooldowns[0] = 0;
                Player.hurtCooldowns[1] = 0;
            }


            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    npc.immune[Player.whoAmI] = 0; // 移除敌人无敌帧
                }
            }
        }
    }
}
