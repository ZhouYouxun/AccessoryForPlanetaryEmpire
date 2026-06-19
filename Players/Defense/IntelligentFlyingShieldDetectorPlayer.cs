//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.DataStructures;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria;
//using Terraria;
//using Terraria.ModLoader;
//using System;
//using AccessoryForPlanetaryEmpire.Global;
//using AccessoryForPlanetaryEmpire.Buffs;
//using AccessoryForPlanetaryEmpire.NPCs;
//using AccessoryForPlanetaryEmpire.Accessory.Defense;
//using AccessoryForPlanetaryEmpire.Cheat.Buffs;
//using Microsoft.Xna.Framework;
//using AccessoryForPlanetaryEmpire.NPCs.AccNPCs;

//namespace AccessoryForPlanetaryEmpire.Players.Defense
//{
//    public class IntelligentFlyingShieldDetectorPlayer : ModPlayer
//    {
//        public bool hasIntelligentFlyingShieldDetector;
//        public override void ResetEffects()
//        {
//            hasIntelligentFlyingShieldDetector = false;
//        }
//        public override void UpdateDead()
//        {
//            hasIntelligentFlyingShieldDetector = false;
//        }

//        private int chargeTime = 300; // 5秒，每帧60次更新
//        private int timer = 0;

//        public override void PostUpdate()
//        {
//            if (!hasIntelligentFlyingShieldDetector) return;

//            if (timer++ >= chargeTime)
//            {
//                timer = 0;
//                // 检查现有飞盾数量
//                int shieldCount = 0;
//                for (int i = 0; i < Main.maxNPCs; i++)
//                {
//                    if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<FlyingShieldNPC>() && Main.npc[i].owner == this.Player.whoAmI)
//                    {
//                        shieldCount++;
//                    }
//                }

//                if (shieldCount < 4)
//                {
//                    // 生成飞盾
//                    Vector2 spawnPosition = this.Player.Center; // 这里可以调整生成位置
//                    NPC.NewNPC((int)spawnPosition.X, (int)spawnPosition.Y, ModContent.NPCType<FlyingShieldNPC>(), 0, 0, 0, 0, this.Player.whoAmI);
//                }
//            }
//        }
//    }


//}