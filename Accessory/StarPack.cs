using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyEMPs.OtherEMP;
using AccessoryForPlanetaryEmpire.Accessory.Attack.Gambler;
using AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem;
using AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC;
using AccessoryForPlanetaryEmpire.Weapons.New;


namespace AccessoryForPlanetaryEmpire.Accessory
{
    public class StarPack : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star Pack");
            //Tooltip.SetDefault("A starter pack full of useful items.");
            Item.ResearchUnlockCount = 0;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            // 添加要在背包中开出的物品
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.IronHelmet); // 铁头盔
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.IronChainmail); // 铁链甲
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.IronGreaves); // 铁护胫

            // 初始四武准备更换
            //player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.PlatinumBroadsword); // 铂金剑
            //player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.PainterPaintballGun); // 彩弹枪
            //player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.DiamondStaff); // 钻石法杖
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.SlimeStaff); // 史莱姆法杖

            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<TheKnife>(), 1);
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<TheBow>(), 1);
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<TheStaff>(), 1);
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<TheSum>(), 1);

            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.Bomb, 20); // 20个炸弹
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.Glowstick, 20); // 20个荧光棒

            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.Torch, 200); // 200个火把
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.Wood, 800); // 800个木板
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.Rope, 200); // 200根绳子
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.Chest, 10); // 10个宝箱

            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.ManaCrystal, 3); // 3个魔力水晶
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.LifeCrystal, 3); // 3个生命水晶
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.GoldCoin, 15); // 15个金币

            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 3); // 3个科技结晶
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<PeriodicBarrierBooster>(), 1); // 1个周期性屏障
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.MasterBait, 5); // 5个大师鱼饵

            // 在三个模组装备中随机抽取一个给予玩家
            int[] extraRewards = new int[]
            {
                ModContent.ItemType<CyberChip>(),
                ModContent.ItemType<CO2Switch>(),
                ModContent.ItemType<MercuryAccessory>()
            };
            int choice = Main.rand.Next(extraRewards.Length); // 随机 0~2
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), extraRewards[choice]); // 给玩家其中一个

            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.Bed, 1); // 给一张床


            // 尝试加载 CalamityMod 并给予玩家 Crystalline 武器
            Mod calamityMod;
            if (ModLoader.TryGetMod("CalamityMod", out calamityMod))
            {
                // 获取 Crystalline 的物品类型 ID
                int crystallineItemType = calamityMod.Find<ModItem>("Crystalline").Type;
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), crystallineItemType); // 给玩家 Crystalline 武器
            }

            // 尝试加载 MagicStorage 并给予玩家 StorageUnit、StorageHeart 和 CraftingAccess
            Mod magicStorageMod;
            if (ModLoader.TryGetMod("MagicStorage", out magicStorageMod))
            {
                int storageUnitType = magicStorageMod.Find<ModItem>("StorageUnit").Type;
                int storageHeartType = magicStorageMod.Find<ModItem>("StorageHeart").Type;
                int craftingAccessType = magicStorageMod.Find<ModItem>("CraftingAccess").Type;

                // 给玩家 32 个 StorageUnit
                for (int i = 0; i < 32; i++)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), storageUnitType);
                }

                // 给玩家 1 个 StorageHeart
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), storageHeartType);

                // 给玩家 1 个 CraftingAccess
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), craftingAccessType);
            }
        }







    }
}