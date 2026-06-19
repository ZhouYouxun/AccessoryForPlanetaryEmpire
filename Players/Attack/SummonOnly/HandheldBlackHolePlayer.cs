using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly
{
    public class HandheldBlackHolePlayer : ModPlayer
    {
        public bool hasHandheldBlackHole;

        public override void ResetEffects()
        {
            hasHandheldBlackHole = false;
        }


        //public override void PostUpdateEquips()
        //{
        //    if (hasHandheldBlackHole)
        //    {
        //        List<Projectile> turrets = new List<Projectile>();

        //        // 收集当前玩家的所有哨兵炮台弹幕
        //        foreach (Projectile proj in Main.projectile)
        //        {
        //            if (proj.active && proj.sentry && proj.owner == Player.whoAmI)
        //            {
        //                turrets.Add(proj);
        //                proj.tileCollide = false; // 无视地形
        //            }
        //        }

        //        int count = turrets.Count;
        //        if (count == 0)
        //            return;

        //        float radius = 75f; // 半径为 5×15
        //        float rotationSpeed = 0.02f; // 转动速度，越大越快
        //        float baseAngle = Main.GameUpdateCount * rotationSpeed; // 随时间旋转

        //        for (int i = 0; i < count; i++)
        //        {
        //            Projectile proj = turrets[i];

        //            // 每个炮台平分角度
        //            float angle = baseAngle + MathHelper.TwoPi * i / count;
        //            Vector2 targetOffset = radius * angle.ToRotationVector2();
        //            Vector2 targetPos = Player.Center + targetOffset;

        //            // 平滑移动到目标位置
        //            Vector2 toTarget = targetPos - proj.Center;
        //            float followSpeed = 10f;
        //            proj.velocity = Vector2.Lerp(proj.velocity, toTarget, 0.1f);
        //        }
        //    }
        //}









    }
}


namespace AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly
{
    public class HandheldBlackHoleGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;


        public override void AI(Projectile projectile)
        {
            Player owner = Main.player[projectile.owner];

            if (!owner.active || owner.dead)
                return;

            if (!owner.GetModPlayer<HandheldBlackHolePlayer>().hasHandheldBlackHole)
                return;

            if (!projectile.active || !projectile.sentry || projectile.minion)
                return;

            // ✅ 极快速度追踪玩家中心，解决远距离无法回归的问题
            Vector2 toPlayer = owner.Center - projectile.Center;
            float speed = 114514f;
            Vector2 desiredVelocity = toPlayer.SafeNormalize(Vector2.Zero) * speed;
            projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 0.5f); // 快速贴合

            // ✅ 禁止与地形碰撞（避免被卡）
            projectile.tileCollide = false;
        }




    }
}




