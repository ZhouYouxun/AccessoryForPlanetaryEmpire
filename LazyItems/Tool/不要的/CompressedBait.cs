//using AccessoryForPlanetaryEmpire.Players;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
//{
//    internal class CompressedBait : ModItem
//    {
//        public override void SetStaticDefaults()
//        {
//            Item.ResearchUnlockCount = 5; // 玩家需要研究5个此物品以解锁无限制作
//        }

//        public override void SetDefaults()
//        {
//            Item.width = 12;
//            Item.height = 12;
//            Item.maxStack = 1; // 1 最大堆叠
//            Item.rare = ItemRarityID.LightRed; // 稀有度为LightRed
//            Item.value = Item.sellPrice(silver: 30); // 售价为30银币
//            Item.bait = 65; // 鱼饵的钓鱼力
//            Item.consumable = false;
//        }

//        // 阻止消耗鱼饵
//        public override bool? CanConsumeBait(Player player)
//        {
//            // 返回 false 以阻止消耗鱼饵
//            return false;
//        }

//        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
//        {
//            itemGroup = ContentSamples.CreativeHelper.ItemGroup.FishingBait;
//        }
//    }
//}