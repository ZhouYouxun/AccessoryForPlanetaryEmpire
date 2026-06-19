using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Buffs;

namespace AccessoryForPlanetaryEmpire
{
    public class SunLightSystem : ModSystem
    {
        // 定义一个静态开关变量，控制是否启用随机 RGB 颜色效果
        public static bool IsPsychedelicActive = false;

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            // 检查玩家是否持有 ElectronicPsychedelicDrugsBuff，如果没有持有，关闭炫光效果
            if (Main.LocalPlayer != null && !Main.LocalPlayer.HasBuff(ModContent.BuffType<ElectronicPsychedelicDrugsBuff>()))
            {
                IsPsychedelicActive = false;
            }

            // 只有当开关被打开时，才会启用随机颜色效果
            if (IsPsychedelicActive)
            {
                // 随机生成 RGB 颜色
                tileColor = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256));
                backgroundColor = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256));
            }
        }

        public override void OnWorldUnload()
        {
            // 当玩家退出世界时，关闭炫光效果
            IsPsychedelicActive = false;
        }


    }
}
