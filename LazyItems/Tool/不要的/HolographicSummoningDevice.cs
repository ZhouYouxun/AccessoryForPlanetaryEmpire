//using AccessoryForPlanetaryEmpire.Players;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.Audio;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;



//namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
//{
//    public class HolographicSummoningDevice : ModItem
//    {
//        private int currentBossIndex = 0; // 当前可召唤的Boss索引
//        private List<int> availableBosses; // 可召唤的Boss列表

//        public override void SetStaticDefaults()
//        {
//            //DisplayName.SetDefault("Holographic Summoning Device");
//            //Tooltip.SetDefault("A device that summons bosses you have already defeated.\nRight-click to switch between bosses.");
//        }

//        public override void SetDefaults()
//        {
//            Item.width = 28;
//            Item.height = 20;
//            Item.useTime = 10;
//            Item.useAnimation = 10;
//            Item.useStyle = ItemUseStyleID.HoldUp;
//            Item.rare = ItemRarityID.Lime;
//            Item.consumable = false; // 道具不可消耗
//            Item.autoReuse = true;   // 自动连续使用
//        }

//        public override void UpdateInventory(Player player)
//        {
//            // 更新可召唤的Boss列表
//            availableBosses = GetAvailableBosses(player);
//            currentBossIndex = 0; // 每次打开背包时重置到第一个可召唤的Boss
//        }

//        public override bool AltFunctionUse(Player player)
//        {
//            return true; // 允许右键使用
//        }

//        public override bool CanUseItem(Player player)
//        {
//            // 检查当前是否可召唤所选的Boss
//            return availableBosses.Count > 0 && !NPC.AnyNPCs(availableBosses[currentBossIndex]);
//        }

//        public override bool? UseItem(Player player)
//        {
//            if (player.altFunctionUse == 2) // 如果是右键点击
//            {
//                // 切换当前选择的Boss
//                currentBossIndex = (currentBossIndex + 1) % availableBosses.Count;
//                string bossName = GetBossName(availableBosses[currentBossIndex]);
//                Color textColor = GetBossColor(availableBosses[currentBossIndex]);

//                // 播放切换音效
//                SoundEngine.PlaySound(SoundID.Item149);

//                // 显示Boss名称提示
//                CombatText.NewText(player.getRect(), textColor, $"已切换至 {bossName}");
//            }
//            else
//            {
//                // 播放召唤音效
//                SoundEngine.PlaySound(SoundID.Roar, player.Center);

//                // 召唤当前选择的Boss
//                if (Main.netMode != NetmodeID.MultiplayerClient)
//                {
//                    NPC.SpawnOnPlayer(player.whoAmI, availableBosses[currentBossIndex]);
//                }
//                else
//                {
//                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: availableBosses[currentBossIndex]);
//                }
//            }

//            return true;
//        }

//        private List<int> GetAvailableBosses(Player player)
//        {
//            List<int> bosses = new List<int>();

//            if (NPC.downedSlimeKing) bosses.Add(NPCID.KingSlime);
//            if (NPC.downedBoss1) bosses.Add(NPCID.EyeofCthulhu);
//            if (NPC.downedBoss2)
//            {
//                bosses.Add(NPCID.EaterofWorldsHead);
//                bosses.Add(NPCID.BrainofCthulhu);
//            }
//            if (NPC.downedQueenBee) bosses.Add(NPCID.QueenBee);
//            if (NPC.downedBoss3) bosses.Add(NPCID.SkeletronHead);
//            if (Main.hardMode)
//            {
//                bosses.Add(NPCID.WallofFlesh);
//                if (NPC.downedQueenSlime) bosses.Add(NPCID.QueenSlimeBoss);
//                if (NPC.downedMechBoss1) bosses.Add(NPCID.TheDestroyer);
//                if (NPC.downedMechBoss2) bosses.Add(NPCID.SkeletronPrime);
//                if (NPC.downedMechBoss3) bosses.Add(NPCID.Retinazer);
//                if (NPC.downedPlantBoss) bosses.Add(NPCID.Plantera);
//                if (NPC.downedGolemBoss) bosses.Add(NPCID.Golem);
//                if (NPC.downedEmpressOfLight) bosses.Add(636); // 光之女皇
//                if (NPC.downedFishron) bosses.Add(NPCID.DukeFishron);
//                if (NPC.downedAncientCultist) bosses.Add(NPCID.CultistBoss);
//                if (NPC.downedMoonlord) bosses.Add(NPCID.MoonLordCore);
//            }

//            return bosses;
//        }

//        private string GetBossName(int bossID)
//        {
//            // 返回对应Boss的名称
//            switch (bossID)
//            {
//                case NPCID.KingSlime: return "史莱姆王";
//                case NPCID.EyeofCthulhu: return "克苏鲁之眼";
//                case NPCID.EaterofWorldsHead: return "世界吞噬者";
//                case NPCID.BrainofCthulhu: return "克苏鲁之脑";
//                case NPCID.QueenBee: return "蜂王";
//                case NPCID.SkeletronHead: return "骷髅王";
//                case NPCID.WallofFlesh: return "血肉之墙";
//                case NPCID.QueenSlimeBoss: return "史莱姆女王";
//                case NPCID.TheDestroyer: return "毁灭者";
//                case NPCID.SkeletronPrime: return "机械骷髅王";
//                case NPCID.Retinazer: return "双子魔眼";
//                case NPCID.Plantera: return "世纪之花";
//                case NPCID.Golem: return "石巨人";
//                case 636: return "光之女皇"; // 光之女皇的数字ID
//                case NPCID.DukeFishron: return "猪鲨公爵";
//                case NPCID.CultistBoss: return "拜月教邪教徒";
//                case NPCID.MoonLordCore: return "月亮领主";
//                default: return "未知Boss";
//            }
//        }

//        private Color GetBossColor(int bossID)
//        {
//            // 根据BossID返回对应的颜色
//            switch (bossID)
//            {
//                case NPCID.KingSlime: return Color.Blue;
//                case NPCID.EyeofCthulhu: return Color.Red;
//                case NPCID.EaterofWorldsHead: return Color.Purple;
//                case NPCID.BrainofCthulhu: return Color.Crimson;
//                case NPCID.QueenBee: return Color.Yellow;
//                case NPCID.SkeletronHead: return Color.Gray;
//                case NPCID.WallofFlesh: return Color.DarkRed;
//                case NPCID.QueenSlimeBoss: return Color.Pink;
//                case NPCID.TheDestroyer: return Color.DarkGray;
//                case NPCID.SkeletronPrime: return Color.Silver;
//                case NPCID.Retinazer: return Color.Green;
//                case NPCID.Plantera: return Color.Green;
//                case NPCID.Golem: return Color.Brown;
//                case 636: return Color.Magenta; // 光之女皇的颜色
//                case NPCID.DukeFishron: return Color.Cyan;
//                case NPCID.CultistBoss: return Color.DarkBlue;
//                case NPCID.MoonLordCore: return Color.White;
//                default: return Color.White;
//            }
//        }
//    }
//}










