using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    public class FixedPointDefenseDevicePlayer : ModPlayer
    {
        public bool hasFixedPointDefenseDevice = false;

        public override void ResetEffects()
        {
            hasFixedPointDefenseDevice = false;
        }

        public override void PostUpdateEquips()
        {
            if (hasFixedPointDefenseDevice)
            {
                ApplyDefenseDeviceEffects();
            }
        }

        private void ApplyDefenseDeviceEffects()
        {
            float speed = Player.velocity.Length();
            float maxSpeed = Player.accRunSpeed; // 玩家最大移速
            float speedFactor = MathHelper.Clamp(1f - speed / maxSpeed, -1.0f, 0.75f);// 调整负值范围 -1.0意味着静止不动时的翻倍防御  0.75代表全速移动时降低的防御

            if (speed == 0)
            {
                CreateParticles();
            }

            int originalDefense = Player.statDefense;
            int defenseAdjustment = (int)(originalDefense * 0.99f * speedFactor);
            Player.statDefense += defenseAdjustment; // 调整防御力
        }



        private void CreateParticles()
        {
            float radius = 50f; // 圆圈半径
            int dustCount = 20; // 粒子数量
            for (int i = 0; i < dustCount; i++)
            {
                float angle = 2f * (float)Math.PI / dustCount * i;
                float x = Player.Center.X + radius * (float)Math.Cos(angle);
                float y = Player.Center.Y + radius * (float)Math.Sin(angle);
                Vector2 dustPosition = new Vector2(x, y);
                Dust dust = Dust.NewDustPerfect(dustPosition, DustID.UltraBrightTorch, Vector2.Zero, 100, default(Color), 1.5f);
                dust.velocity *= 0.3f; // 调整粒子的速度
                dust.noGravity = true; // 不受重力影响
                dust.fadeIn = 1f;
                dust.scale = 1.5f;
            }
        }
    }
}