using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class BloodVesselAcceleratorSkillBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }


        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 90;
            player.endurance = 99f;
            CreateEnergyAbsorptionEffect(player);
            if (Main.rand.NextBool(3))
                PLALightingBoltsSystem.PLA_Spawn_BloodAbsorption(player.Center);

        }

        // 创建能量吸收粒子特效的方法
        private void CreateEnergyAbsorptionEffect(Player player)
        {
            float time = Main.GlobalTimeWrappedHourly;
            int dustID = DustID.Blood; // 红色血液粒子
            int count = 5; // 每帧生成数量
            float wave = (float)Math.Sin(time * 6f) * 0.5f + 1f; // 心跳节奏（0.5~1.5）

            for (int i = 0; i < count; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(160f, 160f) * wave; // 随心跳缩放范围
                Vector2 pos = player.Center + offset;
                Vector2 vel = (player.Center - pos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.5f, 3.5f) * wave;

                Dust d = Dust.NewDustPerfect(pos, dustID, vel, 80, Color.Red * 2f, Main.rand.NextFloat(1.6f, 2.4f));
                d.noGravity = true;
                d.fadeIn = 1.2f;
                d.scale *= wave;
                d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }
        }




    }
}


