using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class MercuryCurseingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance = 0f; // 将伤害减免设置为0
            player.statDefense *= 0.1f;


            // 检查是否已经生成了粒子特效，如果没有，则生成一次
            if (!player.HasBuff(ModContent.BuffType<MercuryCurseingBuff>()))
            {
                // 生成一次性银色特效粒子
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(player.position, player.width, player.height, DustID.SilverCoin);
                }
            }

            // 移除水星弹幕
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<MercuryProjectile>())
                {
                    proj.Kill();
                }
            }
        }
    }
}
