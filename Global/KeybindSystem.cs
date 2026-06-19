using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players;


namespace AccessoryForPlanetaryEmpire.Global
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind ClearBulletKeybind { get; private set; }
        public static ModKeybind UFODKeybind { get; private set; }
        public static ModKeybind TKeybind { get; private set; }
        public static ModKeybind BLKeybind { get; private set; }
        public static ModKeybind NSDDKeybind { get; private set; }
        public static ModKeybind HHMBKeybind { get; private set; }
        public static ModKeybind UMBKeybind { get; private set; }
        public static ModKeybind SMAKeybind { get; private set; }
        public static ModKeybind SJCKeybind { get; private set; }
        public static ModKeybind WBOKeybind { get; private set; }
        public static ModKeybind DASHKeybind { get; private set; }
        public static ModKeybind BSMTKeybind { get; private set; }
        public static ModKeybind SRKeybind { get; private set; }






        public override void Load()
        {
            // Registers a new keybind
            ClearBulletKeybind = KeybindLoader.RegisterKeybind(Mod, "通用电磁脉冲", "P");
            UFODKeybind = KeybindLoader.RegisterKeybind(Mod, "超载装置", "P");
            TKeybind = KeybindLoader.RegisterKeybind(Mod, "瞬间移动", "P");
            BLKeybind = KeybindLoader.RegisterKeybind(Mod, "活血泵", "P");
            HHMBKeybind = KeybindLoader.RegisterKeybind(Mod, "血管加速器", "P");
            SMAKeybind = KeybindLoader.RegisterKeybind(Mod, "动力加速装置", "P");
            WBOKeybind = KeybindLoader.RegisterKeybind(Mod, "疾风行者靴", "P");
            DASHKeybind = KeybindLoader.RegisterKeybind(Mod, "单键冲刺", "P");
            BSMTKeybind = KeybindLoader.RegisterKeybind(Mod, "弹幕管理终端", "P");
            SRKeybind = KeybindLoader.RegisterKeybind(Mod, "音波环", "P");
        }

        public override void Unload()
        {
            ClearBulletKeybind = null;
            UFODKeybind = null;
            TKeybind = null;           
            BLKeybind = null;
            NSDDKeybind = null;
            HHMBKeybind = null;
            UMBKeybind = null;
            SMAKeybind = null;
            SJCKeybind = null;
            WBOKeybind = null;
            DASHKeybind = null;
            BSMTKeybind = null;
            SRKeybind = null;
        }
    }
}