using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures; // 如有需要
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj
{
    public class BreakingDrillPlayer : ModPlayer
    {
        public bool hasBreakingDrill;

        public override void ResetEffects()
        {
            hasBreakingDrill = false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasBreakingDrill)
            {
                ApplyBreakingDrillEffect(target, ref modifiers);
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasBreakingDrill && proj.friendly && !proj.hostile)
            {
                ApplyBreakingDrillEffect(target, ref modifiers);
            }
        }

        private void ApplyBreakingDrillEffect(NPC target, ref NPC.HitModifiers modifiers)
        {
            int defense = target.defense;

            // 计算额外伤害增幅
            float defenseMultiplier = 1 + defense * 0.02f;

            // 应用额外伤害增幅
            modifiers.SourceDamage *= defenseMultiplier;
        }




    }
}