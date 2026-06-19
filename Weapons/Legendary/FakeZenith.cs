using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players;
using Terraria.Audio;


namespace AccessoryForPlanetaryEmpire.Weapons.Legendary
{
    internal class FakeZenith : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 200; // 设置基础伤害
            Item.DamageType = DamageClass.Melee; // 设定为近战类型
            Item.width = 145; // 武器的宽度
            Item.height = 145; // 武器的高度
            Item.useTime = Item.useAnimation = 35; // 使用动画时间
            Item.useStyle = ItemUseStyleID.Swing; // 使用风格为挥舞
            Item.knockBack = 6f; // 击退力
            Item.value = Item.buyPrice(gold: 1000); // 价值设定
            Item.rare = ItemRarityID.Red; // 稀有度设定
            Item.UseSound = SoundID.Item1; // 使用音效
            Item.autoReuse = true; // 自动挥舞
            Item.shoot = ProjectileID.EnchantedBeam; // 设置默认弹幕
            Item.shootSpeed = 19f; // 弹幕速度
            Item.scale = 1.5f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CopperShortsword, 1);
            recipe.AddIngredient(ItemID.Starfury, 1);
            recipe.AddIngredient(ItemID.EnchantedSword, 1);
            recipe.AddIngredient(ItemID.BeeKeeper, 1);
            recipe.AddIngredient(ItemID.Seedler, 1);
            recipe.AddIngredient(ItemID.TheHorsemansBlade, 1);
            recipe.AddIngredient(ItemID.InfluxWaver, 1);
            recipe.AddIngredient(ItemID.StarWrath, 1);
            recipe.AddIngredient(ItemID.Meowmere, 1);
            recipe.AddIngredient(ItemID.TerraBlade, 1);
            recipe.AddTile(TileID.LunarCraftingStation); // 合成站
            recipe.Register();
        }


        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // 右键
            {
                Item.damage = 1150; // 设置伤害值
                Item.useTime = Item.useAnimation = 20;
                Item.shootSpeed = 15f; // 更改使用时的武器弹幕飞行速度
                Item.UseSound = SoundID.Item1;
            }
            else // 左键
            {
                Item.damage = 114; // 设置伤害值
                Item.useTime = Item.useAnimation = 18;
                Item.shootSpeed = 15f; // 更改使用时的武器弹幕飞行速度
                Item.UseSound = SoundID.Item1;
            }
            return base.CanUseItem(player);
        }



        private static int leftClickAttackCounter = 0;

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float adjustedItemScale = player.GetAdjustedItemScale(Item); // 获取玩家缩放

            if (player.altFunctionUse == 2) // 右键时
            {
                {
                    // 三层叠加斩击波（不同缩放 + 不同颜色）
                    (float scale, Color color)[] configs = new[]
                    {
                    (1.0f, Color.White),
                    (1.15f, Color.WhiteSmoke),
                    (1.3f, Color.LightGray)
                };

                    foreach (var (scale, color) in configs)
                    {
                        int p = Projectile.NewProjectile(
                            source,
                            player.MountedCenter,
                            new Vector2(player.direction, 0f),
                            ModContent.ProjectileType<SwordBarrage>(),
                            damage,
                            knockback,
                            player.whoAmI,
                            player.direction * player.gravDir,
                            player.itemAnimationMax,
                            adjustedItemScale * scale
                        );

                        if (p >= 0 && p < Main.maxProjectiles)
                        {
                            Projectile proj = Main.projectile[p];
                            proj.localAI[0] = color.R / 255f;
                            proj.localAI[1] = color.G / 255f;
                            proj.localAI[2] = color.B / 255f;
                        }
                    }
                }
            }
            else
            {
                // 左键

                {
                    // 三层叠加斩击波（不同缩放 + 不同颜色）
                    (float scale, Color color)[] configs = new[]
                    {
                    (1.0f, Color.White),
                    (1.15f, new Color(200, 200, 200)),        // 浅灰色
                    (1.3f, new Color(180, 100, 255))          // 淡紫色
                };

                    foreach (var (scale, color) in configs)
                    {
                        int p = Projectile.NewProjectile(
                            source,
                            player.MountedCenter,
                            new Vector2(player.direction, 0f),
                            ModContent.ProjectileType<SwordBarrage>(),
                            damage,
                            knockback,
                            player.whoAmI,
                            player.direction * player.gravDir,
                            player.itemAnimationMax,
                            adjustedItemScale * scale
                        );

                        if (p >= 0 && p < Main.maxProjectiles)
                        {
                            Projectile proj = Main.projectile[p];
                            proj.localAI[0] = color.R / 255f;
                            proj.localAI[1] = color.G / 255f;
                            proj.localAI[2] = color.B / 255f;
                        }
                    }
                }


                leftClickAttackCounter++;
                if (leftClickAttackCounter >= 2)
                {
                    leftClickAttackCounter = 0; // 重置

                    // 释放 TheSuperSlash
                    int superSlash = Projectile.NewProjectile(
                        source,
                        player.Center,
                        velocity.SafeNormalize(Vector2.UnitX * player.direction) * 5f,
                        ModContent.ProjectileType<TheSuperSlash>(),
                        (int)(damage * 1.5f),
                        knockback,
                        player.whoAmI
                    );
                }


                int dir = player.direction;
                Vector2 baseVelocity = velocity.SafeNormalize(Vector2.UnitX * dir);
                Vector2 perpendicular = baseVelocity.RotatedBy(MathHelper.PiOver2); // ⊥偏移方向

                // === 1️⃣ 四发并排攻击：中间 TerraBeam，两侧 InfluxWaver（偏移沿垂直方向） ===
                for (int i = -2; i <= 1; i++)
                {
                    Vector2 offset = perpendicular * i * 16f;
                    Vector2 spawnPos = position + offset;

                    int projType = (i == -1) ? ProjectileID.TerraBeam : ProjectileID.InfluxWaver;
                    int p = Projectile.NewProjectile(source, spawnPos, baseVelocity * 14f, projType, damage, knockback, player.whoAmI);
                    if (p >= 0 && p < Main.maxProjectiles)
                    {
                        Projectile proj = Main.projectile[p];
                        proj.extraUpdates = 5;
                    }
                }

                // === 2️⃣ 玩家为圆心，在圆周上挑两个对立点释放 Light/NightBeam 各两发 ===
                float beamRadius = 5f * 16f;
                float baseAngle = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始角度
                Vector2 center = player.Center;

                for (int i = 0; i < 4; i++) // X个角度（180°间隔）
                {
                    float angle = baseAngle + i * MathHelper.Pi; // 0°, 180°
                    Vector2 spawnPos = center + angle.ToRotationVector2() * beamRadius;
                    Vector2 shootDir = (Main.MouseWorld - spawnPos).SafeNormalize(Vector2.UnitY) * 13f;

                    // LightBeam
                    int l = Projectile.NewProjectile(source, spawnPos, shootDir, ProjectileID.LightBeam, damage, knockback, player.whoAmI);
                    if (l >= 0 && l < Main.maxProjectiles)
                    {
                        Projectile proj = Main.projectile[l];
                        proj.extraUpdates = 5;
                    }

                    // NightBeam
                    int n = Projectile.NewProjectile(source, spawnPos, shootDir, ProjectileID.NightBeam, damage, knockback, player.whoAmI);
                    if (n >= 0 && n < Main.maxProjectiles)
                    {
                        Projectile proj = Main.projectile[n];
                        proj.extraUpdates = 5;
                    }
                }


                // === 2️⃣ Starfury + StarWrath 从天而降 ===
                Vector2 playerCenter = player.Center;
                Vector2 mouseWorld = Main.MouseWorld;
                Vector2 projCenter = (mouseWorld + playerCenter) * 0.5f;
                float shiftX = (playerCenter.X - projCenter.X) * 0.25f;
                float finalX = projCenter.X + shiftX;

                for (int i = 0; i < 6; i++)
                {
                    Vector2 spawnPos = new Vector2(finalX + Main.rand.NextFloat(-80f, 80f), playerCenter.Y - 70f * 16f + Main.rand.NextFloat(-24f, 24f));
                    Vector2 shootDir = (Main.MouseWorld - spawnPos).SafeNormalize(Vector2.UnitY).RotatedByRandom(MathHelper.ToRadians(6f));
                    int projType = Main.rand.NextBool() ? ProjectileID.Starfury : ProjectileID.StarWrath;

                    int p = Projectile.NewProjectile(source, spawnPos, shootDir * 16f, projType, damage, knockback, player.whoAmI);
                    if (p >= 0 && p < Main.maxProjectiles)
                    {
                        Projectile proj = Main.projectile[p];
                        proj.friendly = true;
                        proj.hostile = false;
                        proj.penetrate = 1;
                        proj.localNPCHitCooldown = 60;
                        proj.usesLocalNPCImmunity = true;
                        proj.extraUpdates = 2;
                    }
                }

                // === 3️⃣ SeedlerNut 当前角度发射 + 散射 ===
                float nutSpeed = 9.5f;
                Vector2 nutVelocity = velocity.SafeNormalize(Vector2.UnitX * player.direction)
                    .RotatedByRandom(MathHelper.ToRadians(15f)) * nutSpeed;

                int nut = Projectile.NewProjectile(source, position, nutVelocity, ProjectileID.SeedlerNut, damage, knockback, player.whoAmI);
                if (nut >= 0 && nut < Main.maxProjectiles)
                {
                    Projectile proj = Main.projectile[nut];
                    proj.friendly = true;
                    proj.hostile = false;
                    proj.penetrate = -1;
                    proj.localNPCHitCooldown = 60;
                    proj.usesLocalNPCImmunity = true;
                }

                // === 4️⃣ 找到最近敌人并围绕释放 FlamingJack ===
                NPC target = FindClosestEnemy(player.Center, 2500f);
                if (target != null && target.CanBeChasedBy())
                {
                    int jackCount = Main.rand.Next(5, 11); // 5~10发
                    for (int i = 0; i < jackCount; i++)
                    {
                        float radius = Main.rand.NextFloat(15f * 16f, 35f * 16f); // 半径范围
                        float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                        Vector2 spawnPos = target.Center + angle.ToRotationVector2() * radius;

                        Vector2 shootVel = (target.Center - spawnPos).SafeNormalize(Vector2.Zero) * 15f;

                        int jack = Projectile.NewProjectile(source, spawnPos, shootVel, ProjectileID.FlamingJack, damage, knockback, player.whoAmI);
                        if (jack >= 0 && jack < Main.maxProjectiles)
                        {
                            Projectile proj = Main.projectile[jack];
                            //proj.friendly = true;
                            //proj.hostile = false;
                            //proj.penetrate = -1;
                            //proj.localNPCHitCooldown = 60;
                            //proj.usesLocalNPCImmunity = true;
                            proj.extraUpdates = 2;
                        }
                    }
                }







            }

            return false; // 不发射默认弹幕
        }
        public static NPC FindClosestEnemy(Vector2 position, float maxDistance)
        {
            NPC closest = null;
            float closestDistance = maxDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy())
                {
                    float dist = Vector2.Distance(position, npc.Center);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        closest = npc;
                    }
                }
            }

            return closest;
        }



        //public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        //{
        //    float adjustedItemScale = player.GetAdjustedItemScale(Item); // 计算武器和玩家的整体缩放
        //    Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ModContent.ProjectileType<SwordBarrage>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);




        //// 173号EnchantedBeam
        //Vector2 targetPosition = Main.MouseWorld; // 获取鼠标位置
        //Vector2 direction = (player.Center - targetPosition).SafeNormalize(Vector2.Zero) * 30f; // 计算方向并三倍速度
        //// 在鼠标位置生成，弹幕朝向玩家位置
        //Projectile.NewProjectile(source, targetPosition, direction, ProjectileID.EnchantedBeam, damage, knockback, player.whoAmI);


        //// 9号Starfury
        //int numberOfStarfury = Main.rand.Next(3, 6); // 随机生成3到5个弹幕
        //for (int i = 0; i < numberOfStarfury; i++)
        //{
        //    Vector2 spawnPosition = new Vector2(Main.MouseWorld.X + Main.rand.Next(-100, 100), Main.screenPosition.Y - Main.rand.Next(50, 100)); // 从屏幕上方随机位置生成
        //    float speedMultiplier = Main.rand.NextFloat(5f, 6f); // 弹幕速度的5~6倍
        //    Vector2 projectileVelocity = (Main.MouseWorld - spawnPosition).SafeNormalize(Vector2.Zero) * speedMultiplier * 10f; // 计算速度并乘以速度倍数
        //    Projectile.NewProjectile(source, spawnPosition, projectileVelocity, ProjectileID.Starfury, damage, knockback, player.whoAmI);
        //}


        //// 503号StarWrath
        //for (int i = 0; i < 5; i++)
        //{
        //    Vector2 starWrathVelocity = new Vector2(velocity.X, velocity.Y + Main.rand.NextFloat(-5f, 25f));
        //    Projectile.NewProjectile(source, position, starWrathVelocity, ProjectileID.StarWrath, damage, knockback, player.whoAmI);
        //}


        //// 451号InfluxWaver
        //int numberOfProjectiles = Main.rand.Next(4, 7); // 随机生成4到6个弹幕
        //for (int i = 0; i < numberOfProjectiles; i++)
        //{
        //    // 在鼠标下方一段距离的位置生成弹幕，X方向有一定的随机性
        //    Vector2 influxPosition = new Vector2(Main.MouseWorld.X + Main.rand.Next(-100, 100), Main.MouseWorld.Y + Main.screenHeight / 2);

        //    // 增加速度至原来的三倍
        //    Vector2 projectileVelocity = new Vector2(0, -30f); // 速度为原来的三倍

        //    Projectile.NewProjectile(source, influxPosition, projectileVelocity, ProjectileID.InfluxWaver, damage, knockback, player.whoAmI);
        //}




        //// 483号SeedlerNut
        //Vector2 seedlerVelocity = new Vector2(0, -10f).RotatedByRandom(MathHelper.ToRadians(5));
        //Projectile.NewProjectile(source, player.Center, seedlerVelocity, ProjectileID.SeedlerNut, damage, knockback, player.whoAmI);

        //// 985号TerraBlade2Shot
        //Projectile.NewProjectile(source, player.Center, velocity, ProjectileID.TerraBlade2Shot, damage, knockback, player.whoAmI);

        //// 984号TerraBlade2
        //// Projectile.NewProjectile(source, player.Center, Vector2.Zero, ProjectileID.TerraBlade2, damage, knockback, player.whoAmI);

        //// 502号Meowmere
        //for (int i = -10; i <= 10; i += 10)
        //{
        //    Vector2 meowmereVelocity = velocity.RotatedBy(MathHelper.ToRadians(i));
        //    Projectile.NewProjectile(source, player.Center, meowmereVelocity, ProjectileID.Meowmere, damage, knockback, player.whoAmI);
        //}

        //    return false; // 返回false以防止自动发射默认弹幕
        //}



        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //var source = player.GetSource_ItemUse(Item);
            //SoundEngine.PlaySound(SoundID.Item73, player.Center);


        }


        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.SilverCoin, 0, 0, 0, default, 2);
            base.MeleeEffects(player, hitbox);
        }



        // 调整手持位置
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-25, -25);
        }







    }
}