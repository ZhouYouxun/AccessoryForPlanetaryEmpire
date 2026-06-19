using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire.Accessory.Y_TestAcc;
using AccessoryForPlanetaryEmpire.Accessory.Attack.Annihilation;
using AccessoryForPlanetaryEmpire.Accessory.Attack.SummonOnly;
using AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Accessory.Attack.Gambler;
using AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyEMPs;
using AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC;
using AccessoryForPlanetaryEmpire.Accessory.Defense.Counter;
using AccessoryForPlanetaryEmpire.Accessory.Wings;

namespace AccessoryForPlanetaryEmpire.Global
{
    internal class PLGlobalBuyItem : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Mechanic)
            {
                // 使用你本文件内部实现的AddWithCustomValue
                AddWithCustomValue(
                    shop,
                    ModContent.ItemType<TheItem>(),
                    Item.buyPrice(gold: 5)
                );
            }


            if (shop.NpcType == NPCID.Cyborg)
            {
                // ★ 创建“击败月球领主”条件
                Condition downedMoonlordCondition = new Condition(
                    "DownedMoonlord",
                    () => NPC.downedMoonlord
                );

                // ★ 添加：需要击败月球领主 + 自定义价格
                AddWithCustomValue(
                    shop,
                    ModContent.ItemType<CrownofOblivion>(),
                    Item.buyPrice(gold: 1500),
                    downedMoonlordCondition
                );
                AddWithCustomValue(
                    shop,
                    ModContent.ItemType<CelestialDominion>(),
                    Item.buyPrice(gold: 1500),
                    downedMoonlordCondition
                );
                AddWithCustomValue(
                    shop,
                    ModContent.ItemType<BulletScreenManagementTerminal>(),
                    Item.buyPrice(gold: 1500),
                    downedMoonlordCondition
                );
                AddWithCustomValue(
                    shop,
                    ModContent.ItemType<Z_GamblersKit>(),
                    Item.buyPrice(gold: 1500),
                    downedMoonlordCondition
                );
                AddWithCustomValue(
                    shop,
                    ModContent.ItemType<UniversalProtectionDevice001>(),
                    Item.buyPrice(gold: 1500),
                    downedMoonlordCondition
                );
                AddWithCustomValue(
                    shop,
                    ModContent.ItemType<Z_CirculationSystemRetrofitKit>(),
                    Item.buyPrice(gold: 1500),
                    downedMoonlordCondition
                );
                AddWithCustomValue(
                    shop,
                    ModContent.ItemType<ControllableStellarEnergy>(),
                    Item.buyPrice(gold: 1500),
                    downedMoonlordCondition
                );
                AddWithCustomValue(
                    shop,
                    ModContent.ItemType<IMPWing>(),
                    Item.buyPrice(gold: 1500),
                    downedMoonlordCondition
                );
            }










        }

        // =============================
        // ★ 你本地版 AddWithCustomValue
        // =============================
        private NPCShop AddWithCustomValue(NPCShop shop, int itemType, int customValue, params Condition[] conditions)
        {
            Item item = new Item();
            item.SetDefaults(itemType);
            item.shopCustomPrice = customValue;

            shop.Add(item, conditions);
            return shop;
        }

        // 泛型版本（如果你想用）
        private NPCShop AddWithCustomValue<T>(NPCShop shop, int customValue, params Condition[] conditions) where T : ModItem
        {
            return AddWithCustomValue(shop, ModContent.ItemType<T>(), customValue, conditions);
        }
    }
}