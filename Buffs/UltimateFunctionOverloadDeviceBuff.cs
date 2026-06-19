using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class UltimateFunctionOverloadDeviceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false; // 这个Buff不是一个减益效果
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen = 0;
            player.endurance -= 0.7f;
            player.statLifeMax2 = (int)(player.statLifeMax2 * 0.5f);

            // 降低防御力和移动速度
            player.statDefense /= 4;
            player.moveSpeed /= 2f;

            // 提升仇恨值
            player.aggro += 3000;

            // 提升伤害
            player.GetDamage(DamageClass.Generic) += 3f;

            // 创建叉叉形粒子特效
            CreateCrossEffect(player);
        }

        // 创建叉叉形粒子特效的方法
        private void CreateCrossEffect(Player player)
        {
            int dustID = DustID.Lava;
            float duration = 1f; // 粒子的持续时间
            float sizeMultiplier = 3f; // 粒子的大小倍数
            float brightnessMultiplier = 3f; // 粒子的亮度倍数
            float amplitude = 150f; // 振幅
            int particleCount = 3; // 每帧生成的粒子数

            for (int i = 0; i < particleCount; i++)
            {
                // 随机生成 t 值，范围在 [0, 2 * pi)
                float t = Main.rand.NextFloat(MathHelper.TwoPi);

                // 叉叉形的参数方程
                float x = amplitude * (float)Math.Cos(t);
                float y = amplitude * (float)Math.Sin(t);

                // 创建粒子特效
                Vector2 position1 = player.Center + new Vector2(x, y);
                Vector2 position2 = player.Center + new Vector2(-x, y);
                Dust dust1 = Dust.NewDustPerfect(position1, dustID, Vector2.Zero, 100, Color.Red * brightnessMultiplier, 1.5f * sizeMultiplier);
                Dust dust2 = Dust.NewDustPerfect(position2, dustID, Vector2.Zero, 100, Color.Red * brightnessMultiplier, 1.5f * sizeMultiplier);
                dust1.noGravity = true;
                dust2.noGravity = true;
                dust1.fadeIn = duration;
                dust2.fadeIn = duration;
                dust1.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                dust2.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                dust1.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
                dust2.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
                dust1.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
                dust2.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
            }
        }


    }
}


