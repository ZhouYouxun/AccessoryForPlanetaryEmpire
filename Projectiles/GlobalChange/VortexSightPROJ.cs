using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.Achievements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using System.Reflection;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Players.Attack.Annihilation;

namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class VortexSightPROJ : GlobalProjectile
    {

        public override bool InstancePerEntity => true;


        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (Main.player[projectile.owner].GetModPlayer<AntimatterSynthesizerPlayer>().hasAntimatterSynthesizer && DamageClassUtils.IsEligibleProjectile(projectile))
            {
                // 提升弹幕飞行速度
                projectile.velocity *= 1.5f;
                // 仅在穿透次数不等于 -1 和 0 的情况下增加穿透次数
                if (projectile.penetrate > 1)
                {
                    //// 检查弹幕的存在时间，如果超过1200帧（20秒），则将其设置为1200帧
                    //if (projectile.timeLeft > 1200)
                    //{
                    //    projectile.timeLeft = 1200;
                    //}
                    projectile.penetrate += 10;
                }

            }
        }




    }
}
