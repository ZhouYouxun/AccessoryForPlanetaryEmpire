using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.NPCs.BossAIs.TheALTK
{
    public class TheALTK : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // Main.npcFrameCount[NPC.type] = 1; // 帧数，如果有动画可以改动
        }

        public override void SetDefaults()
        {
            NPC.width = 180; // 碰撞箱宽度
            NPC.height = 200; // 碰撞箱高度
            NPC.damage = 127; // 基础伤害
            NPC.defense = 44; // 防御力
            NPC.lifeMax = 100000; // 血量
            NPC.knockBackResist = 0f; // 击退抗性
            NPC.aiStyle = -1; // AI使用自定义的AI文件
            NPC.boss = true; // 设置为boss
            NPC.noTileCollide = true; // 不与地形碰撞
            NPC.noGravity = true; // 不受重力影响
            NPC.lavaImmune = true; // 免疫岩浆
            //NPC.HitSound = SoundID.NPCHit1; // 击中声音
            //NPC.DeathSound = SoundID.NPCDeath14; // 死亡声音
            NPC.value = Item.buyPrice(591, 60, 15, 3); // 掉落的货币价值
            Music = MusicID.Boss3; // 设置战斗音乐
            NPC.netAlways = true; // 确保boss在网络上同步

            AIType = -1;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }

        public override void AI()
        {
            TheALTKAI.AI(NPC); // 调用AI文件中的逻辑
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion; // 掉落物
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.LunarBar, 114); // 掉落月亮锭
        }
    }
}