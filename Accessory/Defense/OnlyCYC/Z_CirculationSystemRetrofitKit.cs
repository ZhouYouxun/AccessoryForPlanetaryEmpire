using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.Attack.JobSkill;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj;
using AccessoryForPlanetaryEmpire.Players.Defense.Counter;
using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    public class Z_CirculationSystemRetrofitKit : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";


        // 循环系统改造套件：
        // 活血泵，机械主义心脏，疼痛编辑器，心脏起搏器，AED组件，血管加速器6合1，拥有他们6个的全部效果（一股far味）

        private const int Cooldown = 45 * 60; // 45秒冷却时间
        private int timer = 0;
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 最终毕业装：Master，价值95金
            Item.rare = ItemRarityID.Master;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.value = Item.sellPrice(0, 95, 0, 0);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            if (hideVisual) // 隐藏特效
            {
                player.GetModPlayer<Z_CirculationSystemPlayer>().circulationAuraActive = false;
            }
            else // 释放特效
            {
                player.GetModPlayer<Z_CirculationSystemPlayer>().circulationAuraActive = true;
            }

            player.aggro += 5000; // 仇恨值超大幅度提升
            player.statDefense += 25; // 提供25点防御力

            player.GetModPlayer<TungstenCorePlayer>().hasTungstenCore = true;
            player.GetModPlayer<AedComponentsPlayer>().hasAEDComponent = true;
            player.GetModPlayer<BloodPumpPlayer>().hasBloodPump = true;
            player.GetModPlayer<BloodVesselAcceleratorPlayer>().hasBloodVesselAccelerator = true;
            player.GetModPlayer<HeartPacemakerPlayer>().hasHeartPacemaker = true;
            player.GetModPlayer<MechanicalHeartPlayer>().hasMechanicalHeart = true;
            player.GetModPlayer<PLPlayer>().HasPainEditor = true;
            player.GetModPlayer<AorticThrusterPlayer>().hasAorticThruster = true;
            player.GetModPlayer<AttackIndicatorPlayer>().hasDefenseIndicator = true;
            player.GetModPlayer<SwarmConstructPlayer>().hasSwarmConstruct = true;
            //player.GetModPlayer<HandHeldMechanicalBucklerPlayer>().hasHandHeldMechanicalBuckler = true;


            if (timer > 0)
            {
                timer--; // 每帧减少冷却时间
            }

            // 当玩家血量低于20%且冷却时间结束时触发效果
            if (timer <= 0 && player.statLife <= player.statLifeMax2 * 0.2f)
            {
                // 创建LifeSubSpacePROJ弹幕
                Projectile.NewProjectile(
                    player.GetSource_Accessory(Item), // 来源于此配饰
                    player.Center,                    // 在玩家中心生成
                    Vector2.Zero,                     // 没有速度，静止不动
                    ModContent.ProjectileType<LifeSubSpacePROJ>(), // 弹幕类型
                    0,                                 // 无伤害
                    0,                                 // 无击退
                    player.whoAmI                      // 这个弹幕属于当前玩家
                );
                timer = Cooldown; // 重置冷却时间
            }



            // 检查玩家是否浸没在液体中
            if (player.wet)
            {
                // 增加20%的最大生命值
                player.statLifeMax2 += (int)(player.statLifeMax2 * 0.2f);

                // 增加15点生命回复
                player.lifeRegen += 15;

                // 增加25点防御力
                player.statDefense += 25;

                // 增加20%的移动速度
                player.moveSpeed += 0.2f;

                // 增加20%的飞行时间
                player.wingTimeMax = (int)(player.wingTimeMax * 1.2f);
            }
            else
            {
                // 当玩家不在液体中时，没有任何惩罚
            }
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.AedComponents>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.BloodPump>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.BloodVesselAccelerator>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.HeartPacemaker>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.MechanicalHeart>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.PainEditor>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.LifeSubSpace>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.AorticThruster>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.SedativeRefrigerator>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.SwarmConstruct>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.CO2Switch>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyCYC.TungstenCore>(), 1);
            recipe.AddIngredient(ItemID.LunarBar, 5);

            // 判断是否启用了 CalamityMod
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                recipe.AddIngredient(calamityMod.Find<ModItem>("BloodstoneCore").Type, 5);
            }

            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}




namespace AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC
{
    public class Z_CirculationSystemPlayer : ModPlayer
    {
        // 是否启用心跳光晕
        public bool circulationAuraActive;

        public override void ResetEffects()
        {
            circulationAuraActive = false;
        }
    }
}





namespace AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC
{
    internal class Z_CirculationHeartbeatAuraLayer : PlayerDrawLayer
    {
        // 放在所有原版绘制之后，作为“能量层”
        public override Position GetDefaultPosition()
            => new AfterParent(PlayerDrawLayers.LastVanillaLayer);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.GetModPlayer<Players.Defense.OnlyCYC.Z_CirculationSystemPlayer>().circulationAuraActive
                   && !player.dead
                   && drawInfo.shadow == 0f;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            List<DrawData> drawCache = drawInfo.DrawDataCache;

            // ===============================
            // ① 心跳函数（非正弦，接近生理心跳）
            // ===============================
            float t = Main.GlobalTimeWrappedHourly;

            float heartbeat =
                (float)Math.Pow(Math.Abs(Math.Sin(t * 2.2f)), 4f) +
                0.35f * (float)Math.Pow(Math.Abs(Math.Sin(t * 4.4f)), 6f);

            heartbeat = MathHelper.Clamp(heartbeat, 0f, 1f);

            // ===============================
            // ② 参数区（你以后调的地方）
            // ===============================
            int ringCount = 12;                 // 光晕分身数量
            float baseRadius = 2.5f;            // 基础偏移半径
            float pulseRadius = 6.5f * heartbeat;
            float minOpacity = 0.05f;
            float maxOpacity = 0.28f;
            float scaleBoost = 0.03f + 0.06f * heartbeat;

            Color baseColor = new Color(200, 40, 40); // 深红
            Color hotColor = new Color(255, 80, 80); // 心跳峰值红

            Color auraColor = Color.Lerp(baseColor, hotColor, heartbeat);
            float auraOpacity = MathHelper.Lerp(minOpacity, maxOpacity, heartbeat);

            // ===============================
            // ③ 核心技巧：复制已有 DrawData
            // ===============================
            List<DrawData> afterimages = new List<DrawData>();

            for (int i = 0; i < ringCount; i++)
            {
                float angle = MathHelper.TwoPi * i / ringCount;
                Vector2 offset =
                    angle.ToRotationVector2() *
                    (baseRadius + pulseRadius);

                foreach (DrawData original in drawCache)
                {
                    DrawData copy = original;

                    // —— 位移到光晕位置 ——
                    copy.position += offset;

                    // —— 颜色 & 透明度 ——
                    copy.color = auraColor * auraOpacity;

                    // —— 微缩放，避免完全重叠 ——
                    copy.scale *= 1f + scaleBoost;

                    afterimages.Add(copy);
                }
            }

            // 插入到最底层（在玩家本体后）
            drawInfo.DrawDataCache.InsertRange(0, afterimages);
        }
    }
}




