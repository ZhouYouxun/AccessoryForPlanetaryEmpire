using System;
using AccessoryForPlanetaryEmpire.Global.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.FriendlyFire
{
    public sealed class FriendlyFireGlobalProjectile : GlobalProjectile
    {
        private const int OwnerSpawnProtectionFrames = 15;
        private const int TrackingWindow = 10;
        private const float HoldoutDistance = 6f * 16f;

        private int age;
        private int holdoutFrames;
        private int trackingSamples;
        private float trackingTurnSum;
        private Vector2 lastVelocity;
        private bool originalHostile;
        private bool friendlyFireEnabled;

        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, Terraria.DataStructures.IEntitySource source) {
            age = 0;
            holdoutFrames = 0;
            trackingSamples = 0;
            trackingTurnSum = 0f;
            lastVelocity = projectile.velocity;
            originalHostile = projectile.hostile;
            friendlyFireEnabled = false;
        }

        public override void PostAI(Projectile projectile) {
            age++;
            UpdateTracking(projectile);
            UpdateHoldoutDetection(projectile);

            if (CanEnableFriendlyFire(projectile)) {
                friendlyFireEnabled = true;
            }

            if (friendlyFireEnabled && !IsManagedFriendlyFire(projectile)) {
                friendlyFireEnabled = false;

                if (!originalHostile) {
                    projectile.hostile = false;
                }
            }

            if (friendlyFireEnabled) {
                projectile.hostile = true;
            }
        }

        public override bool CanHitPlayer(Projectile projectile, Player target) {
            if (!IsFriendlyFireActive(projectile)) {
                return true;
            }

            return CanDamageTarget(projectile, target);
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) {
            if (!IsFriendlyFireActive(projectile)) {
                return;
            }

            if (!CanDamageTarget(projectile, target) || !CanPassCooldown(projectile, target)) {
                modifiers.Cancel();
                return;
            }

            int damage = CalculateFriendlyFireDamage(projectile, target);
            modifiers.SourceDamage *= 0f;
            modifiers.FinalDamage.Base += damage;
            modifiers.SetMaxDamage(damage);
            modifiers = modifiers with { CooldownCounter = ImmunityCooldownID.WrongBugNet };
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info) {
            if (!IsFriendlyFireActive(projectile)) {
                return;
            }

            int cooldown = GetFriendlyFireCooldown(projectile);
            target.GetModPlayer<FriendlyFirePlayer>().SetProjectileCooldown(projectile.type, cooldown);

            if (info.CooldownCounter >= 0 && info.CooldownCounter < target.hurtCooldowns.Length) {
                target.hurtCooldowns[info.CooldownCounter] = 0;
            }
        }

        public bool CanPassCooldown(Projectile projectile, Player target) {
            return target.GetModPlayer<FriendlyFirePlayer>().CanTakeHitFrom(projectile.type);
        }

        public bool IsFriendlyFireActive(Projectile projectile) {
            return IsFriendlyFireConfigEnabled() && friendlyFireEnabled && IsManagedFriendlyFire(projectile);
        }

        public bool CanDamageTarget(Projectile projectile, Player target) {
            if (!target.active || target.dead) {
                return false;
            }

            if (target.whoAmI == projectile.owner && age < OwnerSpawnProtectionFrames) {
                return false;
            }

            return true;
        }

        public int GetFriendlyFireCooldown(Projectile projectile) {
            int cooldown = 20;

            if (projectile.usesIDStaticNPCImmunity) {
                cooldown = projectile.idStaticNPCHitCooldown;
            }
            else if (projectile.usesLocalNPCImmunity) {
                cooldown = projectile.localNPCHitCooldown;
            }

            return Math.Clamp(cooldown, 1, 60);
        }

        private static bool IsFriendlyFireConfigEnabled() {
            return ModContent.GetInstance<PLAConfigs>().EnableFriendlyFire;
        }

        private bool CanEnableFriendlyFire(Projectile projectile) {
            return IsFriendlyFireConfigEnabled() && projectile.friendly && IsManagedFriendlyFire(projectile);
        }

        private bool IsManagedFriendlyFire(Projectile projectile) {
            if (!IsFriendlyFireConfigEnabled()) {
                return false;
            }

            if (projectile.damage <= 0 || projectile.owner < 0 || projectile.owner >= Main.maxPlayers) {
                return false;
            }

            if (projectile.npcProj || projectile.trap || projectile.minion || projectile.sentry) {
                return false;
            }

            if (IsHoldout(projectile)) {
                return false;
            }

            return true;
        }

        private int CalculateFriendlyFireDamage(Projectile projectile, Player target) {
            int baseDamage = Math.Max(projectile.damage, 1);
            int maxLife = Math.Max(target.statLifeMax2, 1);
            int cooldown = GetFriendlyFireCooldown(projectile);

            float rFinal = baseDamage * 60f * GetPierceMultiplier(projectile) * GetAreaMultiplier(projectile) *
                GetLifetimeMultiplier(projectile) * GetTrackingMultiplier(projectile) * GetHighFrequencyMultiplier(cooldown) /
                (cooldown * maxLife);

            float damageMultiplier = Math.Max(0.02f, 0.25f / (1f + 0.8f * rFinal));
            float impact = baseDamage / (float)maxLife;

            if (impact >= 2f) {
                damageMultiplier = Math.Max(damageMultiplier, 0.12f);
            }
            else if (impact >= 1f) {
                damageMultiplier = Math.Max(damageMultiplier, 0.08f);
            }

            return Math.Max(1, (int)MathF.Round(baseDamage * damageMultiplier));
        }

        private static float GetPierceMultiplier(Projectile projectile) {
            if (projectile.penetrate == -1) {
                return 1.6f;
            }

            if (projectile.penetrate >= 5) {
                return 1.3f;
            }

            if (projectile.penetrate >= 2) {
                return 1.15f;
            }

            return 1f;
        }

        private static float GetAreaMultiplier(Projectile projectile) {
            int area = projectile.width * projectile.height;
            float multiplier = 1f;

            if (area >= 240 * 240) {
                multiplier = 1.5f;
            }
            else if (area >= 120 * 120) {
                multiplier = 1.25f;
            }
            else if (area >= 64 * 64) {
                multiplier = 1.1f;
            }

            if (Math.Max(projectile.width, projectile.height) >= 20 * 16) {
                multiplier *= 1.2f;
            }

            return multiplier;
        }

        private static float GetLifetimeMultiplier(Projectile projectile) {
            if (projectile.timeLeft > 600) {
                return 1.4f;
            }

            if (projectile.timeLeft > 180) {
                return 1.25f;
            }

            if (projectile.timeLeft > 60) {
                return 1.1f;
            }

            return 1f;
        }

        private float GetTrackingMultiplier(Projectile projectile) {
            if (trackingSamples <= 0) {
                return 1f;
            }

            float averageTurn = trackingTurnSum / trackingSamples;
            float speed = projectile.velocity.Length();

            if (speed >= 12f && averageTurn > 0.08f) {
                return 1.6f;
            }

            if (averageTurn > 0.06f) {
                return 1.35f;
            }

            if (averageTurn > 0.03f) {
                return 1.15f;
            }

            return 1f;
        }

        private static float GetHighFrequencyMultiplier(int cooldown) {
            if (cooldown <= 5) {
                return 1.5f;
            }

            if (cooldown <= 10) {
                return 1.25f;
            }

            if (cooldown <= 20) {
                return 1.1f;
            }

            return 1f;
        }

        private void UpdateTracking(Projectile projectile) {
            if (projectile.velocity.LengthSquared() < 0.01f || lastVelocity.LengthSquared() < 0.01f) {
                lastVelocity = projectile.velocity;
                return;
            }

            float turn = Math.Abs(MathHelper.WrapAngle(projectile.velocity.ToRotation() - lastVelocity.ToRotation()));
            trackingTurnSum += turn;
            trackingSamples++;

            if (trackingSamples > TrackingWindow) {
                trackingTurnSum *= (TrackingWindow - 1f) / TrackingWindow;
                trackingSamples = TrackingWindow;
            }

            lastVelocity = projectile.velocity;
        }

        private void UpdateHoldoutDetection(Projectile projectile) {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers) {
                return;
            }

            Player owner = Main.player[projectile.owner];
            float distance = Vector2.Distance(projectile.Center, owner.MountedCenter);

            if (distance < HoldoutDistance && (projectile.timeLeft <= 2 || projectile.velocity.LengthSquared() < 0.25f)) {
                holdoutFrames++;
            }
            else {
                holdoutFrames = 0;
            }
        }

        private bool IsHoldout(Projectile projectile) {
            if (holdoutFrames >= 6) {
                return true;
            }

            string name = GetProjectileName(projectile);
            return name.Contains("HoldOut", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("Hold", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetProjectileName(Projectile projectile) {
            ModProjectile modProjectile = ModContent.GetModProjectile(projectile.type);

            if (modProjectile != null) {
                return modProjectile.FullName;
            }

            return ProjectileID.Search.GetName(projectile.type);
        }
    }
}
