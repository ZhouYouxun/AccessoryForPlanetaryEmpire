//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria;


//public class ShieldWhenDeployedPlayer : ModPlayer
//{
//    private const int invincibilityDuration = 10 * 60; // 无敌持续时间为10秒，以游戏帧为单位（每秒60帧）
//    private bool hasShieldWhenDeployed;

//    public override void OnRespawn(Player player)
//    {

//        if (hasShieldWhenDeployed)
//        {
//            player.immune = true; // 设置玩家为无敌状态
//            player.immuneTime = invincibilityDuration; // 无敌状态持续时间
//                                                       // 如果存在视觉效果，比如闪烁，可以调整下面的值
//            player.immuneAlpha = 0;
//        }

//    }



//}
