using System;
using System.Collections.Generic;
using AccessoryForPlanetaryEmpire.LazyItems.Tool;
using AccessoryForPlanetaryEmpire.Players.Attack.Annihilation;
using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;
using AccessoryForPlanetaryEmpire.Players.Movement;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    internal class UIOverlaySystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Add(new LegacyGameInterfaceLayer(
                "AccessoryForPlanetaryEmpire: Matrix Status HUD",
                delegate
                {
                    DrawMatrixStatusHud();
                    return true;
                },
                InterfaceScaleType.Game));
        }

        private static void DrawMatrixStatusHud()
        {
            Player player = Main.LocalPlayer;
            if (Main.gameMenu || !player.active || player.dead)
            {
                return;
            }

            List<MatrixStatusBar> bars = new();

            AddStellarCleaverBars(player, bars);
            AddPeriodicBarrierBar(player, bars);
            AddAnnihilationBars(player, bars);
            AddCirculationBars(player, bars);
            AddMovementBars(player, bars);
            AddEMPBars(player, bars);

            MatrixStatusHud.DrawBars(player, bars);
        }

        private static void AddStellarCleaverBars(Player player, List<MatrixStatusBar> bars)
        {
            StellarCleaverPlayer cleaver = player.GetModPlayer<StellarCleaverPlayer>();
            if (cleaver.HoldingStellarCleaver && cleaver.DashboardCharge > 0)
            {
                bars.Add(new MatrixStatusBar("CLEAVER", cleaver.DashboardCharge, StellarCleaverPlayer.DashboardChargeMax, new Color(86, 238, 255), "OPEN"));
            }
        }

        private static void AddPeriodicBarrierBar(Player player, List<MatrixStatusBar> bars)
        {
            PeriodicBarrierBoosterPlayer barrier = player.GetModPlayer<PeriodicBarrierBoosterPlayer>();
            if (!barrier.hasPeriodicBarrierBooster)
            {
                return;
            }

            if (barrier.IsBarrierActive)
            {
                bars.Add(new MatrixStatusBar("P-BARRIER", barrier.ActiveTimer, PeriodicBarrierBoosterPlayer.ActiveFrames, new Color(82, 196, 255), "INV"));
            }
            else
            {
                AddReadyBar(bars, "P-BARRIER", barrier.CooldownTimer, PeriodicBarrierBoosterPlayer.CooldownFrames, new Color(82, 196, 255));
            }
        }

        private static void AddAnnihilationBars(Player player, List<MatrixStatusBar> bars)
        {
            UltimateFunctionOverloadDevicePlayer overload = player.GetModPlayer<UltimateFunctionOverloadDevicePlayer>();
            if (overload.hasUltimateFunctionOverloadDevice)
            {
                AddActiveOrReadyBar(
                    bars,
                    "OVERLOAD-V",
                    overload.SkillActiveTimer,
                    UltimateFunctionOverloadDevicePlayer.SkillDurationFrames,
                    overload.SkillCooldownTimer,
                    UltimateFunctionOverloadDevicePlayer.SkillCooldownFrames,
                    new Color(128, 218, 255));
            }

            LimitResponseModulePlayer limit = player.GetModPlayer<LimitResponseModulePlayer>();
            if (limit.hasLimitResponseModule)
            {
                AddActiveOrReadyBar(
                    bars,
                    "LIMIT-R",
                    limit.BuffTimer,
                    LimitResponseModulePlayer.BuffDuration,
                    limit.LimitCooldown,
                    LimitResponseModulePlayer.CooldownDuration,
                    new Color(255, 220, 86));
            }
        }

        private static void AddCirculationBars(Player player, List<MatrixStatusBar> bars)
        {
            MechanicalHeartPlayer heart = player.GetModPlayer<MechanicalHeartPlayer>();
            if (heart.hasMechanicalHeart)
            {
                AddReadyBar(bars, "MECH-HEART", heart.DeathPreventionCooldownTimer, heart.CurrentCooldownFrames, new Color(92, 238, 255));
            }

            BloodVesselAcceleratorPlayer vessel = player.GetModPlayer<BloodVesselAcceleratorPlayer>();
            if (vessel.hasBloodVesselAccelerator)
            {
                AddActiveOrReadyBar(
                    bars,
                    "BLOOD-V",
                    vessel.SkillActiveTimer,
                    BloodVesselAcceleratorPlayer.SkillDurationFrames,
                    vessel.SkillCooldownTimer,
                    BloodVesselAcceleratorPlayer.SkillCooldownFrames,
                    new Color(255, 96, 146));
            }

            BloodPumpPlayer pump = player.GetModPlayer<BloodPumpPlayer>();
            if (pump.hasBloodPump)
            {
                AddActiveOrReadyBar(
                    bars,
                    "BLOOD-PUMP",
                    pump.ActiveTimer,
                    BloodPumpPlayer.ActiveFrames,
                    pump.CooldownTimer,
                    BloodPumpPlayer.CooldownFrames,
                    new Color(255, 82, 126));
            }
        }

        private static void AddMovementBars(Player player, List<MatrixStatusBar> bars)
        {
            TeleportationPlayer teleport = player.GetModPlayer<TeleportationPlayer>();
            if (teleport.TeleportationEquipped)
            {
                AddActiveOrReadyBar(
                    bars,
                    "WARP",
                    teleport.TSkillActive ? teleport.TSkillTime : 0,
                    TeleportationPlayer.ActiveFrames,
                    teleport.TSkillCooldown,
                    TeleportationPlayer.CooldownFrames,
                    new Color(210, 236, 255));
            }

            WindrunnerBootsPlayer wind = player.GetModPlayer<WindrunnerBootsPlayer>();
            if (wind.hasSwiftWindWalkerBoots)
            {
                AddActiveOrReadyBar(
                    bars,
                    "WINDRUN",
                    wind.SpeedBoostTimer,
                    WindrunnerBootsPlayer.BuffDuration,
                    wind.CooldownTimer,
                    WindrunnerBootsPlayer.CooldownTime,
                    new Color(90, 238, 190));
            }

            SpeedModActivePlayer speed = player.GetModPlayer<SpeedModActivePlayer>();
            if (speed.speedModActiveEquipped)
            {
                Color rainbow = Main.hslToRgb((Main.GlobalTimeWrappedHourly * 0.22f) % 1f, 0.92f, 0.68f);
                AddActiveOrReadyBar(
                    bars,
                    "SPEED-DRV",
                    speed.ActiveTimer,
                    SpeedModActivePlayer.ActiveFrames,
                    speed.CooldownTimer,
                    SpeedModActivePlayer.CooldownFrames,
                    rainbow);
            }
        }

        private static void AddEMPBars(Player player, List<MatrixStatusBar> bars)
        {
            EMPClearBulletPlayer clear = player.GetModPlayer<EMPClearBulletPlayer>();
            const int empCooldown = 45 * 60;
            AddEMPReadyBar(bars, clear.has15Interceptor, "EMP-15", clear.cooldown15Interceptor, empCooldown, Color.Purple);
            AddEMPReadyBar(bars, clear.has25IIInterceptor, "EMP-25II", clear.cooldown25IIInterceptor, empCooldown, Color.Lime);
            AddEMPReadyBar(bars, clear.has35IVInterceptor, "EMP-35IV", clear.cooldown35IVInterceptor, empCooldown, Color.Bisque);
            AddEMPReadyBar(bars, clear.has45Interceptor, "EMP-45", clear.cooldown45Interceptor, empCooldown, Color.DarkGreen);
            AddEMPReadyBar(bars, clear.has15Absorber, "EMP-ABS", clear.cooldown15Absorber, empCooldown, Color.MediumVioletRed);
            AddEMPReadyBar(bars, clear.has55IInterceptor, "EMP-55I", clear.cooldown55IInterceptor, empCooldown, Color.WhiteSmoke);
            AddEMPReadyBar(bars, clear.has55VIInterceptor, "EMP-55VI", clear.cooldown55VIInterceptor, empCooldown, Color.Cyan);
            AddEMPReadyBar(bars, clear.has55IXInterceptor, "EMP-55IX", clear.cooldown55IXInterceptor, empCooldown, Color.IndianRed);

            EMPSupportBulletPlayer support = player.GetModPlayer<EMPSupportBulletPlayer>();
            AddEMPReadyBar(bars, support.has25dBarrier, "EMP-25D", support.cooldown25dBarrier, 30 * 60, Color.DarkGray);
            AddEMPReadyBar(bars, support.has25eSubSpace, "EMP-25E", support.cooldown25fSubSpace, 30 * 60, Color.MidnightBlue);

            EMPUniversalPlayer universal = player.GetModPlayer<EMPUniversalPlayer>();
            AddEMPReadyBar(bars, universal.hasUniversalProtectionDevice, "EMP-001", universal.cooldownUniversalProtectionDevice, 60 * 60, Color.White);

            EMP65LPlayer laser = player.GetModPlayer<EMP65LPlayer>();
            if (laser.has65LInterceptor)
            {
                bars.Add(new MatrixStatusBar("EMP-65L", laser.chargeCount, EMP65LPlayer.maxCharge, new Color(80, 190, 255), "CHG"));
            }
        }

        private static void AddEMPReadyBar(List<MatrixStatusBar> bars, bool equipped, string label, int cooldown, int maxCooldown, Color color)
        {
            if (equipped)
            {
                AddReadyBar(bars, label, cooldown, maxCooldown, color);
            }
        }

        private static void AddActiveOrReadyBar(
            List<MatrixStatusBar> bars,
            string label,
            int activeTimer,
            int activeMax,
            int cooldownTimer,
            int cooldownMax,
            Color color)
        {
            if (activeTimer > 0)
            {
                bars.Add(new MatrixStatusBar(label, activeTimer, Math.Max(1, activeMax), color, "ACTIVE"));
            }
            else
            {
                AddReadyBar(bars, label, cooldownTimer, cooldownMax, color);
            }
        }

        private static void AddReadyBar(List<MatrixStatusBar> bars, string label, int cooldownTimer, int cooldownMax, Color color)
        {
            int max = Math.Max(1, cooldownMax);
            int clampedCooldown = Math.Clamp(cooldownTimer, 0, max);
            string state = clampedCooldown <= 0 ? "READY" : "CD";
            bars.Add(new MatrixStatusBar(label, max - clampedCooldown, max, color, state));
        }
    }
}
