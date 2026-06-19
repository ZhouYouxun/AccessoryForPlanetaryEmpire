using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire
{
    internal class DynamicRarity : ModRarity
    {
        public override Color RarityColor
        {
            get
            {
                // 使用时间做动态颜色
                float t = (float)(Main.GlobalTimeWrappedHourly % 1.0);

                // 颜色：蓝色 → 紫色
                Color c1 = new Color(80, 140, 255);
                Color c2 = new Color(200, 80, 255);

                // 线性插值（做渐变）
                return Color.Lerp(c1, c2, (float)(0.5 + 0.5 * Math.Sin(t * 6f)));
            }
        }
    }

    //然后这么调用:    Item.rare = ModContent.RarityType<DynamicRarity>();

}