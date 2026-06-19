using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;


namespace AccessoryForPlanetaryEmpire.Players.Attack.Annihilation
{
    public class AntimatterSynthesizerPlayer : ModPlayer
    {
        public bool hasAntimatterSynthesizer;
        private const float Chance = 0.05f; // 5% 概率
        private const int CooldownTime = 600; // 10秒冷却，60帧每秒，所以600帧
        private int cooldownTimer = 0; // 冷却计时器

        public override void ResetEffects()
        {
            hasAntimatterSynthesizer = false;
        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // 如果冷却计时器大于0，则减少冷却时间
            if (cooldownTimer > 0)
            {
                cooldownTimer--;
            }

            // 只有当冷却时间已结束时，才有5%的概率生成敌对弹幕
            if (hasAntimatterSynthesizer && cooldownTimer == 0 && Main.rand.NextFloat() < Chance)
            {
                CreateHostileProjectile(item, type, damage, knockback);
                cooldownTimer = CooldownTime; // 重置冷却计时器为10秒（600帧）
            }
        }

        private void CreateHostileProjectile(Item item, int type, int damage, float knockback)
        {
            // 随机决定弹幕从玩家的左侧或右侧生成
            bool fromLeft = Main.rand.NextBool(); // 随机决定从左边还是右边
            float spawnX = fromLeft ? Player.position.X - 600f : Player.position.X + 600f; // 在玩家左侧或右侧生成
            float spawnY = Player.position.Y - Main.rand.Next(100, 200); // 生成在玩家的上方一定高度

            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            // 计算朝向玩家的方向
            Vector2 direction = Player.Center - spawnPosition;
            direction.Normalize();

            // 添加弹幕随机性
            float speedX = direction.X * 10f + Main.rand.Next(-120, 121) * 0.01f;
            float speedY = direction.Y * 10f + Main.rand.Next(-120, 121) * 0.01f;

            // 生成敌对弹幕
            int hostileProjectile = Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPosition, new Vector2(speedX, speedY), type, damage, knockback, Main.myPlayer);

            // 转换弹幕阵营为敌对
            Main.projectile[hostileProjectile].hostile = true;
            Main.projectile[hostileProjectile].friendly = false;
            Main.projectile[hostileProjectile].owner = 255; // 255 表示敌方阵营
        }







    }
}