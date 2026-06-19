using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.Localization; // 这可以允许代码里出现中文
using AccessoryForPlanetaryEmpire.Accessory;
using AccessoryForPlanetaryEmpire.Global;

namespace AccessoryForPlanetaryEmpire.Players
{
    // See Common/Systems/KeybindSystem for keybind registration.
    public class KeyPlayers : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            PlanetaryPlayer modPlayer = Main.LocalPlayer.GetModPlayer<PlanetaryPlayer>();

            //// 检查技能是否激活
            //if (!modPlayer.UFODSkillActive && KeybindSystem.UFODKeybind.JustPressed)
            //{
            //    if (modPlayer.UFODSkillCooldown > 0)
            //    {
            //        Main.NewText($"∫∫∫∫∫错误：无法启用极限机能超载装置，冷却时间还未完成∫∫∫∫∫");
            //    }
            //    else
            //    {
            //        Main.NewText($"∫∫∫∫∫极限机能超载装置已开启，持续时间为15秒，请注意自身安全∫∫∫∫∫");
            //    }
            //}


            //// 检查技能是否激活
            //if (!modPlayer.PBBHSkillActive && KeybindSystem.PBBHKeybind.JustPressed)
            //{
            //    if (modPlayer.PBBHSkillCooldown > 0)
            //    {
            //        Main.NewText($"∫∫∫∫∫错误：无法启用周期性屏障生成器（手动版），冷却时间还未完成∫∫∫∫∫");
            //    }
            //    else
            //    {
            //        Main.NewText($"∫∫∫∫∫周期性屏障生成器已经开启，持续时间为5秒，请尽情输出∫∫∫∫∫");
            //    }
            //}



            //// 检查技能是否激活
            //if (!modPlayer.RAPHSkillActive && KeybindSystem.RAPHKeybind.JustPressed)
            //{
            //    if (modPlayer.RAPHSkillCooldown > 0)
            //    {
            //        Main.NewText($"∫∫∫∫∫错误：无法启用反射电弧脉冲（手动版），冷却时间还未完成∫∫∫∫∫");
            //    }
            //    else
            //    {
            //        Main.NewText($"∫∫∫∫∫反射电弧脉冲激活成功，敌方弹幕已被清除∫∫∫∫∫");
            //    }
            //}


            //// 检查技能是否激活
            //if (!modPlayer.TSkillActive && KeybindSystem.TKeybind.JustPressed)
            //{
            //    if (modPlayer.TSkillCooldown > 0)
            //    {
            //        Main.NewText($"∫∫∫∫∫错误：无法启用传送装置，冷却时间还未完成∫∫∫∫∫");
            //    }
            //    else
            //    {
            //        Main.NewText($"∫∫∫∫∫传送装置已启用成功，敌方弹幕已被清除，自身已被加强∫∫∫∫∫");
            //    }
            //}



            //if (!modPlayer.RBSkillActive && KeybindSystem.RBKeybind.JustPressed)
            //{
            //    if (modPlayer.RBSkillCooldown > 0)
            //    {
            //        Main.NewText($"∫∫∫∫∫错误：无法启用团队屏障，冷却时间还未完成∫∫∫∫∫");
            //    }
            //    else
            //    {
            //        Main.NewText($"∫∫∫∫∫团队屏障已启用成功，持续时间为7秒，请尽情输出∫∫∫∫∫");
            //    }
            //}



            //if (!modPlayer.SEMPSkillActive && KeybindSystem.SEMPKeybind.JustPressed)
            //{
            //    if (modPlayer.SEMPSkillCooldown > 0)
            //    {
            //        Main.NewText($"∫∫∫∫∫错误：无法启用眩晕脉冲，冷却时间还未完成∫∫∫∫∫");
            //    }
            //    else
            //    {
            //        Main.NewText($"∫∫∫∫∫眩晕脉冲已启用成功，持续时间为5秒，请尽情输出∫∫∫∫∫");
            //    }



            //}



            //if (!modPlayer.BBSkillActive && KeybindSystem.BBKeybind.JustPressed)
            //{
            //    if (modPlayer.BBSkillCooldown > 0)
            //    {
            //        Main.NewText($"∫∫∫∫∫错误：无法启用子弹雷管脉冲，冷却时间还未完成∫∫∫∫∫");
            //    }
            //    else
            //    {
            //        Main.NewText($"∫∫∫∫∫子弹雷管已启用成功∫∫∫∫∫");
            //    }



            // }




            //if (!modPlayer.TCLSkillActive && KeybindSystem.TCLKeybind.JustPressed)
            //{
            //    if (modPlayer.TCLSkillCooldown > 0)
            //    {
            //        Main.NewText($"∫∫∫∫∫错误：无法启用克隆装置，冷却时间还未完成∫∫∫∫∫");
            //    }
            //    else
            //    {
            //        Main.NewText($"∫∫∫∫∫克隆装置已启用成功∫∫∫∫∫");
            //    }



            //}




        }

    }
}