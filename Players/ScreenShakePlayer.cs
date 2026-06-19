using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs;
using AccessoryForPlanetaryEmpire.Buffs;
using System.Collections;

namespace AccessoryForPlanetaryEmpire.Players
{
    public class ScreenShakePlayer : ModPlayer
    {

        // 屏幕震动强度属性
        public float GeneralScreenShakePower;

        // 在每帧更新屏幕位置时调用
        public override void ModifyScreenPosition()
        {
            // 如果 GeneralScreenShakePower 大于0，则进行屏幕震动操作
            if (GeneralScreenShakePower > 0f)
            {
                // 使用随机的向量来偏移屏幕位置，实现屏幕震动效果
                Main.screenPosition += Main.rand.NextVector2Circular(GeneralScreenShakePower, GeneralScreenShakePower);

                // 逐渐减少 GeneralScreenShakePower 的值，使震动效果逐渐减弱
                GeneralScreenShakePower = MathHelper.Clamp(GeneralScreenShakePower - 0.185f, 0f, 20f);
            }
        }

    }
}
