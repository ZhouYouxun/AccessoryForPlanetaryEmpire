using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;


namespace AccessoryForPlanetaryEmpire.Weapons.New
{
    internal class TheKnife : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "NewWeapons";


        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override bool AltFunctionUse(Player player) => true;


        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;

            Item.damage = 15;                   // 左键基础伤害（你后面会改）
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;                  // 左键挥动时间
            Item.useAnimation = 15;

            Item.useTurn = true;
            Item.autoReuse = true;

            Item.rare = ItemRarityID.White;
            Item.value = Item.buyPrice(silver: 20);

            Item.UseSound = SoundID.Item1;

            Item.noMelee = false;                // 大剑有近战 hurtbox
            Item.shoot = ProjectileID.None;      // 默认不射弹（左键逻辑里手动射）
            Item.shootSpeed = 10f;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // 右键：更快挥砍，不射弹
                Item.useTime = 14;
                Item.useAnimation = 14;
                Item.damage = 30;
                Item.shoot = ProjectileID.None;
                Item.noMelee = false;
            }
            else
            {
                // 左键：保持双连射结构
                Item.useTime = 30;
                Item.useAnimation = 30;
                Item.useLimitPerAnimation = 1;
                Item.damage = 20;
                Item.shoot = ModContent.ProjectileType<TheKnifePROJ>();
            }

            return base.CanUseItem(player);
        }

        // ================================
        // 双连射 + 自定义弹幕
        // ================================
        public override bool Shoot(Player player,
            Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            if (player.altFunctionUse == 2)
                return false; // 右键不射弹

            // 判断当前是第几发（使用动画计数）
            int anim = player.itemAnimation;
            int totalTime = Item.useAnimation;   // 50
            int singleTime = Item.useTime;       // 25

            bool isFirstShot = anim > singleTime; // 前半段动画
            bool isSecondShot = anim <= singleTime;

            if (!isFirstShot && !isSecondShot)
                return false;

            // 你自己的弹幕
            int projType = ModContent.ProjectileType<TheKnifePROJ>();

            // 不做偏移，纯直射
            Projectile.NewProjectile(
                source,
                position,
                velocity * 0.5f,
                projType,
                damage,
                knockBack,
                player.whoAmI
            );

            return false;
        }

        // ================================
        // 特技：大剑挥砍时的特效
        // ================================
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            // 轻量特效：剑风粒子 + 蓝白能量火花
            for (int i = 0; i < 2; i++)
            {
                Vector2 pos = new Vector2(
                    Main.rand.Next(hitbox.Left, hitbox.Right),
                    Main.rand.Next(hitbox.Top, hitbox.Bottom)
                );

                Dust d = Dust.NewDustPerfect(pos, DustID.SilverFlame);
                d.noGravity = true;
                d.scale = 1f + Main.rand.NextFloat(0.2f, 0.5f);
                d.velocity *= 0.2f;
            }

            // 再加一点剑光轨迹（更有刀光感）
            if (Main.rand.NextBool(3))
            {
                Dust d2 = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Electric);
                d2.noGravity = true;
                d2.scale = 0.8f;
                d2.velocity *= 0.1f;
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
