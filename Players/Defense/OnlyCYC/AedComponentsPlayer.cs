using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader;
using System;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Accessory.Defense;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC
{
    public class AedComponentsPlayer : ModPlayer
    {
        public bool hasAEDComponent;
        private int heartSpawnTimer;

        public override void ResetEffects()
        {
            hasAEDComponent = false;
        }

        public override void PreUpdate()
        {
            if (hasAEDComponent && Player.statLife < Player.statLifeMax2 * 0.9f) // 只有当玩家血量低于90%时
            {
                heartSpawnTimer++;
                if (heartSpawnTimer >= 210) // 210帧 = 3.5秒
                {
                    heartSpawnTimer = 0;

                    // 生成红心
                    Vector2 spawnPosition = Player.Center + new Vector2(Main.rand.NextFloat(-15 * 16, 15 * 16), Main.rand.NextFloat(-15 * 16, 15 * 16));
                    Item.NewItem(Player.GetSource_Misc("AEDComponent"), spawnPosition, ItemID.Heart);

                    // 💥强化版生命水晶粒子特效（DustID 12）
                    // 横向范围扩大、厚度增加、速度更快、粒子更大
                    for (int i = -24; i <= 24; i += 3) // 扩展宽度，间隔缩小
                    {
                        float verticalOffset = Main.rand.NextFloat(-6f, 2f); // 增加纵向厚度
                        Vector2 dustPos = Player.Bottom + new Vector2(i, verticalOffset);

                        Vector2 velocity = new Vector2(Main.rand.NextFloat(-0.4f, 0.4f), -Main.rand.NextFloat(1.8f, 3f)); // 更强上冲
                        Dust d = Dust.NewDustPerfect(dustPos, DustID.HeartCrystal, velocity);
                        d.noGravity = true;
                        d.scale = Main.rand.NextFloat(1.4f, 2.1f); // 更大
                    }

                    // 💥播放医疗触发音效（Item9）
                    SoundEngine.PlaySound(SoundID.Item9, Player.Center);


                    //CombatText.NewText(Player.getRect(), Color.OrangeRed, "红心不知道从哪里蹦出来了一个！", dramatic: false, dot: false);
                }
            }
        }







    }
}
