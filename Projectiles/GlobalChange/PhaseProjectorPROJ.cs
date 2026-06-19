using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;

namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class PhaseProjectorPROJ : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        // 保存原始伤害值
        private int originalDamage;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.penetrate == 1 && player.GetModPlayer<PhaseProjectorPlayer>().hasPhaseProjector) // 仅对穿透次数为一的弹幕生效，因此不用限制职业，召唤杖、长枪、炮台、悠悠球这些不会被算进来
            {
                // 设置弹幕的穿透次数为无限
                projectile.penetrate = -1;

                // 保存原始伤害值
                originalDamage = projectile.damage;

                // 穿透敌人时不造成伤害
                projectile.damage = 0;
            }
        }

        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<PhaseProjectorPlayer>().hasPhaseProjector && projectile.penetrate == -1)
            {
                // 恢复并提升伤害至两倍
                projectile.damage = originalDamage * 2;
            }
            return true; // 确保弹幕正常消失
        }





    }
}
