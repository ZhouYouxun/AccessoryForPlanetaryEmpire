using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    public class BrassPocketWatchPlayer : ModPlayer
    {
        public bool hasBrassPocketWatch;

        public override void ResetEffects()
        {
            hasBrassPocketWatch = false;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            base.Kill(damage, hitDirection, pvp, damageSource);

            if (hasBrassPocketWatch && Main.rand.NextFloat() < 0.15f)
            {
                // 清除所有敌人
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.type != NPCID.TargetDummy)
                    {
                        npc.StrikeNPC(new NPC.HitInfo()
                        {
                            Damage = npc.lifeMax * 10,
                            Knockback = 999f,
                            HitDirection = 0,
                            Crit = true,
                            InstantKill = true
                        });
                    }
                }

                // 清除所有弹幕
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active && proj.type < ProjectileID.Count)
                    {
                        proj.Kill();
                    }
                }

                // 清除所有玩家（除了自己，除非你想一起带走😈）
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player other = Main.player[i];
                    if (other.active && other.whoAmI != Player.whoAmI && !other.dead)
                    {
                        other.KillMe(PlayerDeathReason.ByCustomReason($"{other.name} 被时空坍缩吞没了。"), 999999, 0);
                    }
                }

                // （可选）特效、音效、闪光、提示文本等
                //CombatText.NewText(Player.getRect(), Color.Gold, "时空崩溃！");
                SoundEngine.PlaySound(SoundID.Item122, Player.Center); // 例：星星爆炸声
            }
        }
    }

}
