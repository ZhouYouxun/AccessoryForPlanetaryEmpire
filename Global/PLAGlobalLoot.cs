//using AccessoryForPlanetaryEmpire.Players;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria.GameContent.ItemDropRules;

//namespace AccessoryForPlanetaryEmpire.Global
//{
//    public class PLAGlobalLoot : GlobalLoot
//    {
//        private bool hasPortableDrillingRig;

//        // 1. 检测玩家是否收藏了 PortableDrillingRig
//        public override void OnEnterWorld(Player player)
//        {
//            hasPortableDrillingRig = false;

//            // 遍历玩家背包，检查是否有收藏的 PortableDrillingRig
//            foreach (Item item in player.inventory)
//            {
//                if (item.type == ModContent.ItemType<PortableDrillingRig>() && item.favorited)
//                {
//                    hasPortableDrillingRig = true;
//                    break;
//                }
//            }
//        }

//        // 2. 根据收藏状态修改掉落率
//        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
//        {
//            if (hasPortableDrillingRig)
//            {
//                globalLoot.RemoveWhere(dropRule =>
//                {
//                    // 获取当前掉落规则的掉落率
//                    float dropRate = dropRule.Chance;

//                    if (dropRate < 0.05f)
//                    {
//                        // 如果掉落率不足5%，提升到5%
//                        dropRule.Chance = 0.05f;
//                        return true;
//                    }
//                    else if (dropRate >= 0.05f && dropRate < 0.50f)
//                    {
//                        // 如果掉落率在5%到50%之间，翻倍
//                        dropRule.Chance *= 2;
//                        return true;
//                    }
//                    return false;
//                });
//            }
//        }
//    }
//}
