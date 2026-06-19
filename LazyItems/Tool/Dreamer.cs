using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using AccessoryForPlanetaryEmpire.Players;

namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
{
    internal class Dreamer : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanFishInLava[Item.type] = true; // 允许在岩浆中钓鱼
        }

        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 70;
            Item.useAnimation = 8;
            Item.useTime = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.fishingPole = 40;
            Item.shootSpeed = 20f;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<DreamerBobber>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ReinforcedFishingPole, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override void HoldItem(Player player)
        {
            // 计算钓鱼力和鱼钩数量
            int fishingPower = 40;
            int bobberCount = 10;

            if (NPC.downedBoss3) // Skeletron
            {
                fishingPower = 60;
                bobberCount = 20;
            }
            if (Main.hardMode) // Wall of Flesh
            {
                fishingPower = 80;
                bobberCount = 50;
            }
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) // Mech Bosses
            {
                fishingPower = 100;
                bobberCount = 70;
            }
            if (NPC.downedGolemBoss) // Golem
            {
                fishingPower = 130;
                bobberCount = 90;
            }
            if (NPC.downedMoonlord) // Moon Lord
            {
                fishingPower = 170;
                bobberCount = 150;
            }

            Item.fishingPole = fishingPower;

            // 设置使用时的鱼钩数量
            player.GetModPlayer<DreamerPlayer>().BobberCount = bobberCount;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int bobberCount = player.GetModPlayer<DreamerPlayer>().BobberCount;

            for (int i = 0; i < bobberCount; i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(18f)), type, 0, 0f, player.whoAmI, ai2: Main.rand.Next(2));
            }
            return false;
        }

        // 一个错误的函数，它会直接给你随机的物品（挺好玩的，记下了）
        //public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        //{
        //    lineOriginOffset = new Vector2(53f, -33f);

        //    // 在鱼钩投射物中存储一个随机的颜色，如果尚未设置
        //    if (bobber.localAI[0] == 0f)
        //    {
        //        bobber.localAI[0] = 1f; // 标记已经初始化过颜色
        //        bobber.ai[0] = Main.rand.Next(256); // R
        //        bobber.ai[1] = Main.rand.Next(256); // G
        //        bobber.ai[2] = Main.rand.Next(256); // B
        //    }

        //    // 使用存储的颜色
        //    lineColor = new Color((int)bobber.ai[0], (int)bobber.ai[1], (int)bobber.ai[2], 100);
        //}




        public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            lineOriginOffset = new Vector2(69f, -59f); // 这里调整顶端的位置

            // 将鱼线颜色固定设置为天蓝色
            lineColor = new Color(135, 206, 235, 100); // 天蓝色 (R: 135, G: 206, B: 235)
        }




        // 每时每刻都更改钓鱼线的颜色（疯狂变换）
        //public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        //{
        //    lineOriginOffset = new Vector2(53f, -33f);
        //    // 随机选择RGB颜色
        //    lineColor = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256), 100);
        //}




        //// 存储当前的RGB值，作为状态变量
        //private int currentR = 128; // 初始为中间值
        //private int currentG = 128;
        //private int currentB = 128;

        //// 每时每刻都更改钓鱼线的颜色（一起进行变化）
        //public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        //{
        //    lineOriginOffset = new Vector2(53f, -33f);

        //    // 定义RGB颜色变化的步长，使颜色平缓过渡
        //    int colorStep = 5;

        //    // 随机决定颜色增减
        //    currentR += Main.rand.Next(-colorStep, colorStep + 1);
        //    currentG += Main.rand.Next(-colorStep, colorStep + 1);
        //    currentB += Main.rand.Next(-colorStep, colorStep + 1);

        //    // 限制RGB值在0到255之间，防止溢出
        //    currentR = Math.Clamp(currentR, 0, 255);
        //    currentG = Math.Clamp(currentG, 0, 255);
        //    currentB = Math.Clamp(currentB, 0, 255);

        //    // 设置钓鱼线的颜色
        //    lineColor = new Color(currentR, currentG, currentB, 100);

        //    // 减慢颜色变化的速度，通过减少调用频率
        //    if (Main.rand.NextBool(2)) // 每次调用只有50%的几率实际改变颜色
        //    {
        //        return;
        //    }
        //}

        public override bool? CanConsumeBait(Player player)
        {
            // 返回 false 以阻止消耗鱼饵
            return false;
        }

        //public override bool ConsumeItem(Player player)
        //{
        //    // 返回 false 以阻止消耗鱼饵
        //    return false;
        //}

    }

    internal class DreamerPlayer : ModPlayer
    {
        public int BobberCount = 10; // 默认鱼钩数量
    }

    internal class DreamerBobber : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 28;
            Projectile.aiStyle = ProjAIStyleID.Bobber;
            Projectile.bobber = true;
        }

        //public override void PostDraw(Color lightColor)
        //{
        //    Texture2D glowmask = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/LazyItems/Tool/DreamerBobber_Glow").Value;
        //    float xOffset = (glowmask.Width - Projectile.width) * 0.5f + Projectile.width * 0.5f;
        //    Vector2 drawPos = Projectile.position - Main.screenPosition;
        //    drawPos.X += xOffset;
        //    drawPos.Y += Projectile.height / 2f + Projectile.gfxOffY;
        //    Rectangle frame = new(0, 0, glowmask.Width, glowmask.Height);
        //    Vector2 origin = new Vector2(xOffset, Projectile.height / 2f);
        //    SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        //    if (Projectile.ai[0] <= 1f)
        //    {
        //        Main.spriteBatch.Draw(glowmask, drawPos, frame, Color.White, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
        //    }
        //}

        public override bool PreDrawExtras()
        {
            Lighting.AddLight(Projectile.Center, 0.35f, 0f, 0.25f);
            return true;
        }
    }
}
