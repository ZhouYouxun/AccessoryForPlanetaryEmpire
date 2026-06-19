//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;
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



//namespace AccessoryForPlanetaryEmpire.NPCs.AccNPCs
//{
//    public class HPnpc : ModNPC
//    {

//        public override void SetDefaults()
//        {
//            NPC.width = 18;
//            NPC.height = 40;
//            NPC.damage = 0; // 设置为0，因为它不会对敌人造成伤害
//            NPC.defense = 10000; // 非常高的防御力以减轻伤害
//            NPC.lifeMax = 999999; // 极高的生命值
//            NPC.HitSound = SoundID.NPCHit1;
//            NPC.DeathSound = SoundID.NPCDeath1;
//            NPC.value = 0f;
//            NPC.knockBackResist = 0f; // 无法被击退
//            NPC.aiStyle = -1; // 无AI，你可以根据需要设置为其他值
//            NPC.immortal = true; // 使NPC不会死亡
//            NPC.friendly = false; // 设置为非友方，以防止被玩家伤害
//        }

//        public override void AI()
//        {
//            NPC.TargetClosest(true); // 自动目标最近的敌人
//            // NPC.taunt = true; // 增加仇恨值，吸引敌人攻击
//        }

//        public override bool CheckActive()
//        {
//            return true; // NPC始终活跃，永远不会因为玩家远离而消失
//        }

//        public override void UpdateLifeRegen(ref int damage)
//        {
//            NPC.lifeRegen = int.MaxValue; // 非常高的生命恢复速度，确保它实际上不会死
//        }

//        public override bool? CanBeHitByItem(Player player, Item item)
//        {
//            return false; // 不能被玩家的物品击中
//        }

//        public override bool? CanBeHitByProjectile(Projectile projectile)
//        {
//            return !projectile.friendly; // 只能被敌方的弹幕击中
//        }

//        // 如果需要，可以添加更多的自定义逻辑



//        public override bool CheckDead()
//        {
//            if (NPC.life <= 0)
//            {
//                NPC.life = 1; // 阻止NPC死亡
//                return false;
//            }
//            return true;
//        }


//    }
//}
