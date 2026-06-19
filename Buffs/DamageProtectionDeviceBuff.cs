using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class DamageProtectionDeviceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {

            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            //Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense *= 0.1f;
            player.endurance = 0f;
        }



        //public override void SetStaticDefaults()
        //{
        //    // DisplayName.SetDefault("My Debuff");
        //    // Description.SetDefault("This debuff reduces your defenses to zero.");
        //    Main.debuff[Type] = true; // Makes sure Terraria treats this as a debuff
        //    Main.buffNoSave[Type] = true; // Prevents the buff from being saved
        //    Main.buffNoTimeDisplay[Type] = false; // Shows the debuff's timer

        //    // 下面4个是灾厄里的"谷酒"所拥有的4种属性
        //    Main.debuff[Type] = true;
        //    Main.pvpBuff[Type] = true;
        //    Main.buffNoSave[Type] = false;
        //    BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        //}

        //public override bool RightClick(int buffIndex)
        //{
        //    return base.RightClick(buffIndex);
        //}
        // () => false


        //public override void Update(Player player, ref int buffIndex)
        //{
        //    var modPlayer = player.GetModPlayer<MyModPlayer>();
        //    if (modPlayer.originalDefense == -1) // Check if we've already saved the original defense
        //    {
        //        // Save the original defense and endurance values
        //        modPlayer.originalDefense = player.statDefense;
        //        modPlayer.originalEndurance = player.endurance;

        //        // Set defense and endurance to zero
        //        player.statDefense = 0;
        //        player.endurance = 0f;
        //    }
        //}




    }


    //public class MyModPlayer : ModPlayer
    //{
    //    public int originalDefense = -1;
    //    public float originalEndurance = -1f;

    //    public override void ResetEffects()
    //    {
    //        if (Player.HasBuff(ModContent.BuffType<MyDebuff>()))
    //        {
    //            // Debuff is active, do nothing here
    //        }
    //        else if (originalDefense != -1 || originalEndurance != -1f)
    //        {
    //            // Debuff has ended, restore original values
    //            Player.statDefense = originalDefense;
    //            Player.endurance = originalEndurance;

    //            // Reset the stored values
    //            originalDefense = -1;
    //            originalEndurance = -1f;
    //        }
    //    }
    //}



}


