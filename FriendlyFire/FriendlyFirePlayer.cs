using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.FriendlyFire
{
    public sealed class FriendlyFirePlayer : ModPlayer
    {
        private readonly Dictionary<int, int> projectileCooldowns = new();

        public override void PostUpdate() {
            if (projectileCooldowns.Count == 0) {
                return;
            }

            List<int> projectileTypes = new(projectileCooldowns.Keys);
            List<int> expiredTypes = null;

            foreach (int projectileType in projectileTypes) {
                int remaining = projectileCooldowns[projectileType];
                int next = remaining - 1;

                if (next <= 0) {
                    expiredTypes ??= new List<int>();
                    expiredTypes.Add(projectileType);
                }
                else {
                    projectileCooldowns[projectileType] = next;
                }
            }

            if (expiredTypes == null) {
                return;
            }

            foreach (int projectileType in expiredTypes) {
                projectileCooldowns.Remove(projectileType);
            }
        }

        public override bool CanBeHitByProjectile(Projectile proj) {
            if (!proj.TryGetGlobalProjectile(out FriendlyFireGlobalProjectile friendlyFire)) {
                return true;
            }

            if (!friendlyFire.IsFriendlyFireActive(proj)) {
                return true;
            }

            if (!friendlyFire.CanDamageTarget(proj, Player)) {
                return false;
            }

            return friendlyFire.CanPassCooldown(proj, Player);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
            if (!proj.TryGetGlobalProjectile(out FriendlyFireGlobalProjectile friendlyFire)) {
                return;
            }

            if (!friendlyFire.IsFriendlyFireActive(proj)) {
                return;
            }

            SetProjectileCooldown(proj.type, friendlyFire.GetFriendlyFireCooldown(proj));
        }

        public bool CanTakeHitFrom(int projectileType) {
            return !projectileCooldowns.TryGetValue(projectileType, out int remaining) || remaining <= 0;
        }

        public void SetProjectileCooldown(int projectileType, int cooldown) {
            if (cooldown <= 0) {
                projectileCooldowns.Remove(projectileType);
                return;
            }

            projectileCooldowns[projectileType] = cooldown;
        }
    }
}
