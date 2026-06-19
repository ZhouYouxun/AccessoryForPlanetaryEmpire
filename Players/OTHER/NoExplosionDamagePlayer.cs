using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Global.Configs;

namespace AccessoryForPlanetaryEmpire.Players.OTHER
{
    public class NoExplosionDamagePlayer : ModPlayer
    {
        //public override void ResetEffects()
        //{
        //    Player.noKnockback = true; // 防止击退
        //}

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (ModContent.GetInstance<PLAConfigs>().EnableExplosionDamageImmunity)
            {
                if (proj.aiStyle == 16) // 16 is the aiStyle for grenades/explosives
                {
                    modifiers.FinalDamage *= 0f;
                }
            }
        }

        //public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        //{
        //    // 如果NPC是自爆型，则忽略伤害
        //    if (npc.type == NPCID.Bomb || npc.type == NPCID.StickyBomb || npc.type == NPCID.BouncyBomb)
        //    {
        //        damage = 0;
        //    }
        //}
    }
}
