using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.Audio;


namespace AccessoryForPlanetaryEmpire.Weapons.New
{
    internal class TheStaff : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "NewWeapons";

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }


        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;

            Item.damage = 24;                       // 新手用法杖的基础数值
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;                          // 法杖消耗魔力

            Item.useTime = 10;
            Item.useAnimation = 45;
            Item.useLimitPerAnimation = 3;

            Item.useStyle = ItemUseStyleID.Shoot;   // 典型法杖施法姿势

            Item.noMelee = true;                    // 无近战伤害
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.White;
            Item.value = Item.buyPrice(silver: 25);

            Item.UseSound = SoundID.Item68;         // 法杖音效
            Item.autoReuse = true;

            Item.shoot = ProjectileID.DD2PhoenixBowShot; // 原版火鸟弹幕
            Item.shootSpeed = 15f;                  // 法杖射速
        }

        public override bool Shoot(Player player,
        Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            // === 生成一个随机偏移（召唤点 A） ===
            Vector2 offset = Main.rand.NextVector2Circular(60f, 60f);

            Vector2 spawnA = player.Center + offset;
            Vector2 spawnB = player.Center - offset;   // 完全相反位置（对称点）

            // === 两个出生点都朝向鼠标位置 ===
            Vector2 dirA = (Main.MouseWorld - spawnA).SafeNormalize(Vector2.UnitX);
            Vector2 dirB = (Main.MouseWorld - spawnB).SafeNormalize(Vector2.UnitX);

            Vector2 velA = dirA * Item.shootSpeed;
            Vector2 velB = dirB * Item.shootSpeed;

            // ====== 紫色魔法阵（只在 SpawnA 绘制一次即可）======
            SpawnPurpleMagicCircle(spawnA);

            // === 两发都发同样的 PROJ，往鼠标冲刺 ===
            int projType = ModContent.ProjectileType<TheStaffPROJ>();

            Projectile.NewProjectile(
                source,
                spawnA,
                velA,
                projType,
                damage,
                knockBack,
                player.whoAmI
            );

            Projectile.NewProjectile(
                source,
                spawnB,
                velB,
                projType,
                damage,
                knockBack,
                player.whoAmI
            );


         

            return false;
        }

        private void SpawnPurpleMagicCircle(Vector2 center)
        {
            // ============================
            // 🔮 紫色赛博朋克魔法阵效果
            // ============================

            // --- 1. 紫色十字 ---
            int crossLength = 36;
            Vector2[] dirs =
            {
        Vector2.UnitX,
        -Vector2.UnitX,
        Vector2.UnitY,
        -Vector2.UnitY
    };

            foreach (var cd in dirs)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 pos2 = center + cd * (i * (crossLength / 6f));
                    Dust d = Dust.NewDustPerfect(pos2, DustID.PurpleTorch);
                    d.scale = 1.3f;
                    d.velocity = cd * 0.25f;
                    d.color = Color.Lerp(Color.MediumPurple, Color.Violet, 0.6f);
                    d.noGravity = true;
                }
            }

            // --- 2. 外圈紫色圆环 ---
            int circlePoints = 40;
            for (int i = 0; i < circlePoints; i++)
            {
                float angle = MathHelper.TwoPi * i / circlePoints;
                Vector2 offset = angle.ToRotationVector2() * 20f;

                Dust d = Dust.NewDustPerfect(
                    center + offset,
                    DustID.PurpleTorch,
                    angle.ToRotationVector2() * 1.8f,
                    120,
                    Color.Lerp(Color.MediumPurple, Color.Magenta, 0.7f),
                    1.15f
                );
                d.noGravity = true;
            }

            // --- 3. 星芒 + 脉冲（紫色） ---
            int starPoints = 8;
            for (int i = 0; i < starPoints; i++)
            {
                float angle = MathHelper.TwoPi * i / starPoints;
                Vector2 dir2 = angle.ToRotationVector2();

                for (int j = 0; j < 8; j++)
                {
                    float t = j / 8f;
                    Dust d = Dust.NewDustPerfect(
                        center + dir2 * (t * 20f),
                        DustID.PurpleTorch
                    );

                    d.velocity = dir2 * MathHelper.Lerp(1f, 4f, t);
                    d.scale = MathHelper.Lerp(1.5f, 0.7f, t);
                    d.color = Color.Lerp(Color.White, Color.MediumPurple, t * 0.8f);
                    d.noGravity = true;
                }
            }
        }





        public override void AddRecipes()
        {
            //Recipe recipe = CreateRecipe();
            //recipe.AddIngredient(ItemID.Wood, 25);
            //recipe.AddIngredient(ItemID.StoneBlock, 10);
            //recipe.AddTile(TileID.WorkBenches);
            //recipe.Register();
        }





    }
}
