using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Annihilation
{
    public class HeroDamageDevicePlayer : ModPlayer
    {
        public bool HasHeroDamageDevice = false;

        public override void ResetEffects()
        {
            HasHeroDamageDevice = false;
        }

        public override void PostUpdateEquips()
        {
            if (HasHeroDamageDevice)
            {
                float closestDistance = float.MaxValue;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                    {
                        float distance = Vector2.Distance(Player.Center, npc.Center);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                        }
                    }
                }

                ApplyHeroDamageDeviceEffects(closestDistance);
            }
        }

        private void ApplyHeroDamageDeviceEffects(float distance)
        {
            float maxBuffDistance = 10f * 16f; // 10格
            float noEffectDistance = 30f * 16f; // 30格
            float maxDebuffDistance = 60f * 16f; // 60格

            if (distance <= noEffectDistance)
            {
                if (distance <= maxBuffDistance)
                {
                    // 提升效果
                    float factor = (noEffectDistance - distance) / (noEffectDistance - maxBuffDistance);
                    Player.GetDamage(DamageClass.Melee) += 0.5f * factor; // 最高提升50%攻击力
                    Player.GetAttackSpeed(DamageClass.Melee) += 0.3f * factor; // 最高提升30%攻速
                }
                else
                {
                    // 无效果
                }
            }
            else if (distance <= maxDebuffDistance)
            {
                // 降低效果
                float factor = (distance - noEffectDistance) / (maxDebuffDistance - noEffectDistance);
                Player.GetDamage(DamageClass.Melee) -= 0.3f * factor; // 最高降低30%攻击力
                Player.GetAttackSpeed(DamageClass.Melee) -= 0.3f * factor; // 最高降低30%攻速
            }
        }
    }
}
