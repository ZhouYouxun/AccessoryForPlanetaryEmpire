using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
{
    public sealed class StellarCleaverPlayer : ModPlayer
    {
        public const int DashboardChargeMax = 60;

        public bool HoldingStellarCleaver;
        public int DashboardCharge;
        public int CurrentMaxTileBoost;
        public int ManualTileBoost;
        public bool AutoMaxRange = true;
        public bool LightingParticlesEnabled = true;
        internal bool RangeSliderDragging;

        public override void ResetEffects()
        {
            HoldingStellarCleaver = false;
            CurrentMaxTileBoost = 0;
        }

        public override void PostUpdate()
        {
            if (Main.myPlayer != Player.whoAmI || Main.gameMenu)
            {
                return;
            }

            ClampSettings();

            if (!HoldingStellarCleaver)
            {
                DashboardCharge = 0;
                RangeSliderDragging = false;
                return;
            }

            if (StellarCleaverDashboardPanel.HasPanel(Player))
            {
                DashboardCharge = 0;
                return;
            }

            bool charging = Main.mouseLeft &&
                Main.mouseRight &&
                !Main.mapFullscreen &&
                !Main.playerInventory &&
                !Main.blockMouse &&
                !Player.mouseInterface;

            if (charging)
            {
                DashboardCharge = Math.Clamp(DashboardCharge + 1, 0, DashboardChargeMax);
                if (DashboardCharge >= DashboardChargeMax)
                {
                    StellarCleaverDashboardPanel.Open(Player, Player.GetSource_FromThis());
                    SoundEngine.PlaySound(SoundID.Item4 with { Volume = 0.38f, Pitch = 0.18f }, Player.Center);
                    DashboardCharge = 0;
                }
            }
            else if (DashboardCharge > 0)
            {
                DashboardCharge = Math.Max(0, DashboardCharge - 3);
            }
        }

        public int GetAppliedTileBoost(int maxTileBoost)
        {
            CurrentMaxTileBoost = Math.Max(0, maxTileBoost);
            ClampSettings();
            return AutoMaxRange ? CurrentMaxTileBoost : ManualTileBoost;
        }

        public void ClampSettings()
        {
            CurrentMaxTileBoost = Math.Max(0, CurrentMaxTileBoost);
            ManualTileBoost = Math.Clamp(ManualTileBoost, 0, CurrentMaxTileBoost);
        }

        public override void SaveData(TagCompound tag)
        {
            tag["StellarCleaverManualTileBoost"] = ManualTileBoost;
            tag["StellarCleaverAutoMaxRange"] = AutoMaxRange;
            tag["StellarCleaverLightingParticles"] = LightingParticlesEnabled;
        }

        public override void LoadData(TagCompound tag)
        {
            ManualTileBoost = tag.GetInt("StellarCleaverManualTileBoost");
            AutoMaxRange = !tag.ContainsKey("StellarCleaverAutoMaxRange") || tag.GetBool("StellarCleaverAutoMaxRange");
            LightingParticlesEnabled = !tag.ContainsKey("StellarCleaverLightingParticles") || tag.GetBool("StellarCleaverLightingParticles");
        }
    }
}
