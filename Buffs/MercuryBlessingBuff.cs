using AccessoryForPlanetaryEmpire.Players;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class MercuryBlessingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
        //    DisplayName.SetDefault("Mercury's Blessing");
        //    Description.SetDefault("45% damage reduction.");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.85f;
        }
    }
}
