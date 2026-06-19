using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles;
using AccessoryForPlanetaryEmpire.Accessory.Attack.JobSkill;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Gambler
{
    public class LuckyCoinPlayer : ModPlayer
    {
        public bool hasLuckyCoin;

        public override void ResetEffects()
        {
            hasLuckyCoin = false;
        }

        public override void PostSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (hasLuckyCoin)
            {
                // 获取物品的基础卖出价格
                int sellPrice = item.value;
                float chance = Main.rand.NextFloat(); // 生成一个0到1之间的随机数

                if (chance < 0.6f) // 60%的几率随机回收一定比例的金钱
                {
                    float recoverPercentage = Main.rand.NextFloat(0.5f, 1f); // 随机生成一个0.5到1之间的百分比，增加回收金额
                    int recoverAmount = (int)(sellPrice * recoverPercentage); // 计算回收的金额
                    int totalRecoverAmount = recoverAmount; // 保存原始回收金额以便显示

                    // 给予玩家相应的金钱
                    Player.QuickSpawnItem(Player.GetSource_Loot(), ItemID.GoldCoin, recoverAmount / 10000); // 金币
                    recoverAmount %= 10000; // 剩余未能转化为金币的金额
                    Player.QuickSpawnItem(Player.GetSource_Loot(), ItemID.SilverCoin, recoverAmount / 100); // 银币
                    recoverAmount %= 100; // 剩余未能转化为银币的金额
                    Player.QuickSpawnItem(Player.GetSource_Loot(), ItemID.CopperCoin, recoverAmount); // 铜币

                    CombatText.NewText(Player.getRect(), Color.GreenYellow, $"你回收了{totalRecoverAmount}铜币!", dramatic: false, dot: false);
                }
                else // 40%的几率亏损一定比例的金钱
                {
                    float lossPercentage = Main.rand.NextFloat(0f, 0.5f); // 随机生成一个0到0.5之间的百分比，减少亏损金额
                    int lossAmount = (int)(sellPrice * lossPercentage); // 计算亏损的金额
                    int totalLossAmount = lossAmount; // 保存原始亏损金额以便显示

                    // 扣除相应的金钱
                    DeductCoinsFromPlayer(lossAmount);

                    CombatText.NewText(Player.getRect(), Color.Red, $"你亏损了{totalLossAmount}铜币!", dramatic: false, dot: false);
                }

            }
        }

        private void DeductCoinsFromPlayer(int amount)
        {
            // 从玩家身上扣除相应的金钱
            while (amount > 0)
            {
                if (amount >= 10000 && Player.CountItem(ItemID.GoldCoin) > 0)
                {
                    Player.ConsumeItem(ItemID.GoldCoin);
                    amount -= 10000;
                }
                else if (amount >= 100 && Player.CountItem(ItemID.SilverCoin) > 0)
                {
                    Player.ConsumeItem(ItemID.SilverCoin);
                    amount -= 100;
                }
                else if (amount >= 1 && Player.CountItem(ItemID.CopperCoin) > 0)
                {
                    Player.ConsumeItem(ItemID.CopperCoin);
                    amount -= 1;
                }
                else
                {
                    break; // 玩家没有足够的金钱来扣除
                }
            }
        }


        //public override void PreUpdate()
        //{
        //    if (hasLuckyCoin)
        //    {
        //        ApplyPlatinumCoinDamageBoost();
        //    }
        //}
        //public override void PostUpdate()
        //{
        //    if (hasLuckyCoin)
        //    {
        //        ApplyPlatinumCoinDamageBoost();
        //    }
        //}

        //private void ApplyPlatinumCoinDamageBoost()
        //{
        //    // 检测玩家携带的铂金币数量
        //    int platinumCoinCount = Player.CountItem(ItemID.PlatinumCoin);
        //    int maxBoostCoins = 150; // 最大提升的铂金币数量

        //    // 计算伤害提升比例
        //    float damageBoost = Math.Min(platinumCoinCount, maxBoostCoins) * 0.005f; // 每个铂金币提升0.5%的伤害，最大提升75%

        //    // 应用伤害提升
        //    Player.GetDamage(DamageClass.Generic) += damageBoost;
        //}


        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (hasLuckyCoin)
            {
                // 检测玩家携带的铂金币数量
                int platinumCoinCount = Player.CountItem(ItemID.PlatinumCoin);
                int maxBoostCoins = 150; // 最大提升的铂金币数量

                // 计算伤害提升比例
                float damageBoost = Math.Min(platinumCoinCount, maxBoostCoins) * 0.005f; // 每个铂金币提升0.5%的伤害，最大提升75%

                // 应用伤害提升
                damage += damageBoost;
            }
        }


    }
}
