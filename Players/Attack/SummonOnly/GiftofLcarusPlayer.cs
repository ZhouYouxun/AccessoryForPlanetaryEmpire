using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;


namespace AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly
{
    public class GiftofLcarusPlayer : ModPlayer
    {
        //private const int TriggerTime = 300; // 5秒，以帧计算
        //private int shootingTimer = 0;
        private int shootingTimer = 0;

        public bool hasGiftofLcarus = false;
        public override void ResetEffects()
        {
            hasGiftofLcarus = false;
            if (shootingTimer > 0)
            {
                shootingTimer--;
            }
        }


        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // CombatText.NewText(Player.getRect(), Color.LightGreen, "OnHitNPCWithProj", dramatic: false, dot: false);
            // 检查弹幕是否是召唤物弹幕并且确保玩家有装备提供的特效
            if (hasGiftofLcarus && Main.rand.NextFloat() < 0.40f && shootingTimer <= 0) // 40% 的概率 (&& Main.rand.NextFloat() < 0.20f)
            {
                // CombatText.NewText(Player.getRect(), Color.LightGreen, "hasGiftofLcarus", dramatic: false, dot: false);
                // 目前到这一步是没问题
                // if (proj.minion)
                // 只有当它属于召唤伤害，同时不属于鞭子伤害才会触发
                if (proj.DamageType == DamageClass.Summon && proj.DamageType != DamageClass.SummonMeleeSpeed)
                {
                    // 获取玩家当前手持的武器
                    Item heldItem = Player.HeldItem;
                    if (heldItem.DamageType != DamageClass.Summon && heldItem.DamageType != DamageClass.SummonMeleeSpeed)
                    {

                        // CombatText.NewText(Player.getRect(), Color.Red, "以触发伊卡鲁斯之礼", dramatic: false, dot: false);
                        // 获取鼠标位置并转换为世界坐标
                        Vector2 mousePosition = Main.MouseWorld;

                        // 计算从召唤物到鼠标位置的方向向量
                        Vector2 direction = Vector2.Normalize(mousePosition - proj.Center) * 10f; // 设置速度

                        // 获取弹幕的射击属性
                        int shootType = heldItem.shoot;
                        float shootSpeed = heldItem.shootSpeed;
                        //int damage = heldItem.damage;
                        float knockBack = heldItem.knockBack;

                        int damage = (int)(proj.damage * 0.5f);  // 造成召唤物伤害的50%


                        //// 计算发射方向
                        //Vector2 shootDirection;
                        //if (proj.spriteDirection == -1) // 召唤物面向左边
                        //{
                        //    shootDirection = new Vector2(-1, 0); // 向左发射
                        //}
                        //else // 召唤物面向右边
                        //{
                        //    shootDirection = new Vector2(1, 0); // 向右发射
                        //}

                        // 计算发射方向，随机选取一个角度
                        float randomAngle = Main.rand.NextFloat(0, 360); // 生成0到360度之间的随机角度
                        Vector2 shootDirection = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(randomAngle)); // 将角度转换为方向向量


                        // 发射弹幕
                        Projectile.NewProjectile(
                            proj.GetSource_FromThis(),
                            proj.Center,
                            shootDirection * shootSpeed,
                            shootType,
                            damage,
                            knockBack,
                            Player.whoAmI
                        );
                        // 设置冷却时间为7帧
                        shootingTimer = 60;
                    }
                }

            }
        }











    }
}
