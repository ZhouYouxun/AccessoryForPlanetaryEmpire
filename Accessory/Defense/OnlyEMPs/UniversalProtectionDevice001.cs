using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.Defense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Accessory;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyEMPs.ClearBullet;



namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyEMPs
{
    public class UniversalProtectionDevice001 : ModItem, ILocalizedModType, IEMPExclusive
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            // EMP 饰品互斥：两个中任意一个是 EMPExclusive 都禁止共存
            bool equippedIsEMP = ModContent.GetModItem(equippedItem.type) is IEMPExclusive;
            bool incomingIsEMP = ModContent.GetModItem(incomingItem.type) is IEMPExclusive;

            return !(equippedIsEMP && incomingIsEMP);
        }

        public new string LocalizationCategory => "Accessory.Defense.OnlyEMPs";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("001型通用舰载全方位防护装置");
            //Tooltip.SetDefault("按下快捷键时清除半径250格范围内的敌方弹幕（冷却时间为60秒），"
            //    + "弹幕被清除时会被引爆，造成10倍原弹幕伤害；"
            //    + "将所有被清除的敌方弹幕叛变为我方弹幕，伤害提升至原始弹幕的5倍，再次造成一次针对敌方的伤害；"
            //    + "每一发被清除的弹幕都能转化为2点防御力和2%伤害提升，持续15秒；"
            //    + "在原地留下一个半径100格的屏障，屏障可以为内部的玩家提供无敌并清除接触到的地方弹幕。");
        }

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
        private int energyShieldCatapultTimer; // 声明计时器变量

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EMPUniversalPlayer>().hasUniversalProtectionDevice = true;
            player.GetModPlayer<PeriodicBarrierBoosterPlayer>().hasPeriodicBarrierBooster = true;
            // 计时器逻辑
            if (energyShieldCatapultTimer > 0)
            {
                energyShieldCatapultTimer--; // 每帧减少计时器
            }
            else
            {
                // 获取鼠标位置
                Vector2 mousePosition = Main.MouseWorld;
                // 获取发射方向
                Vector2 direction = mousePosition - player.Center;
                direction.Normalize();
                direction *= 4f; // 设置弹幕速度

                // 发射弹幕
                Projectile.NewProjectile(
                    player.GetSource_Accessory(Item),
                    player.Center,
                    direction,
                    ModContent.ProjectileType<EnergyShieldCatapultPROJ>(),
                    0,
                    0,
                    player.whoAmI
                );

                // 生成粒子特效
                CreateExplosionEffect(player.Center);

                // 重置计时器为5秒（300帧）
                energyShieldCatapultTimer = 300;
            }
        }

        // 创建爆炸粒子特效的方法
        private void CreateExplosionEffect(Vector2 position)
        {
            int particleCount = 100;
            for (int i = 0; i < particleCount; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(5f, 10f);
                Dust dust = Dust.NewDustPerfect(position, DustID.BlueTorch, velocity);
                dust.noGravity = true;
                dust.scale = 7.2f;
                dust.fadeIn = 1f;
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.OtherEMP.PeriodicBarrierBooster>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.ClearBullet.Interceptor15>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.ClearBullet.Interceptor25II>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.ClearBullet.Interceptor35IV>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.OtherEMP.EnergyShieldCatapult>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.SupportBullet.Barrier25d>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.SupportBullet.SubSpace25e>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.Interceptor65L>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.ClearBullet.Interceptor45>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.ClearBullet.Absorber15>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.ClearBullet.Interceptor55I>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.ClearBullet.Interceptor55IX>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Defense.OnlyEMPs.ClearBullet.Interceptor55VI>(), 1);

            // 判断是否启用了 CalamityMod
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                recipe.AddIngredient(calamityMod.Find<ModItem>("TeslaCannon").Type, 1);
                recipe.AddIngredient(calamityMod.Find<ModItem>("Omicron").Type, 1);
            }

            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}