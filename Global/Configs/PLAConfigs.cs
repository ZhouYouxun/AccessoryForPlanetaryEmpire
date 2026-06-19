using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AccessoryForPlanetaryEmpire.Global.Configs
{
    public class PLAConfigs : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("开启玩家耐炸")]
        [Tooltip("开启后让爆破物无法对玩家造成有效伤害")]
        [DefaultValue(true)]
        public bool EnableExplosionDamageImmunity { get; set; }

        [Label("开启炸队友")]
        [Tooltip("开启后，玩家弹幕可以按精密计算后的伤害命中其他玩家")]
        [DefaultValue(false)]
        public bool EnableFriendlyFire { get; set; }

        //[Label("开启珍珠木弓更改")]
        //[Tooltip("开启后将修改珍珠木弓的属性和功能")]
        //[DefaultValue(true)]
        //public bool EnablePearlwoodBowTweaks { get; set; }

        ////[Label("开启珍珠木剑更改")]
        ////[Tooltip("开启后将修改珍珠木剑的属性和功能")]
        ////[DefaultValue(true)]
        ////public bool EnablePearlwoodSwordTweaks { get; set; }

        //[Label("开启死神镰刀更改")]
        //[Tooltip("开启后将修改死神镰刀的属性和功能")]
        //[DefaultValue(true)]
        //public bool EnableDeathSickleTweaks { get; set; }

        //[Label("开启脉冲弓更改")]
        //[Tooltip("开启后将修改脉冲弓的属性和功能")]
        //[DefaultValue(true)]
        //public bool EnablePulseBowTweaks { get; set; }

        //[Label("开启庆典大炮更改")]
        //[Tooltip("开启后将修改庆典大炮的属性和功能")]
        //[DefaultValue(true)]
        //public bool EnableFireworksLauncherTweaks { get; set; }

        //[Label("开启灰烬木剑更改")]
        //[Tooltip("开启后将修改灰烬木剑的属性和功能")]
        //[DefaultValue(true)]
        //public bool EnableAshWoodSwordTweaks { get; set; }
    }
}
