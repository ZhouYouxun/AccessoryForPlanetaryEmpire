using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;

namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class UranusPlayer : ModPlayer
    {
        public bool hasUranusAccessory = false;
        public bool hasTSSAccessory = false;
        public bool GuangHuan = false;
        private int debuffCooldown = 60; // 一秒钟的冷却时间

        public override void ResetEffects()
        {
            hasUranusAccessory = false;
            hasTSSAccessory = false;
            GuangHuan = false;
        }

        public override void PreUpdate()
        {
            if (hasUranusAccessory)
            {
                ManageUranusAccessory();
                if (debuffCooldown > 0)
                {
                    debuffCooldown--;
                }
                else
                {
                    ApplyDebuffs();
                    debuffCooldown = 60; // 重置冷却时间
                }
            }

            if (hasTSSAccessory)
            {
                ManageUranusAccessory();
                if (debuffCooldown > 0)
                {
                    debuffCooldown--;
                }
                else
                {
                    ApplyDebuffsTSS();
                    debuffCooldown = 60; // 重置冷却时间
                }
            }
        }

        private void ApplyDebuffs()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && Vector2.Distance(Player.Center, npc.Center) <= 125 * 16)
                {
                    npc.AddBuff(ModContent.BuffType<Uranium238Debuff>(), 300); // 5秒

                    if (Main.rand.NextFloat() < 0.2f)
                    {
                        npc.AddBuff(ModContent.BuffType<Uranium235Debuff>(), 60); // 0.5秒
                    }
                }
            }
        }

        private void ApplyDebuffsTSS()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && Vector2.Distance(Player.Center, npc.Center) <= 175 * 16) // 升级至太阳系后，它的半径提升至175
                {
                    npc.AddBuff(ModContent.BuffType<Uranium238Debuff>(), 300); // 5秒

                    if (Main.rand.NextFloat() < 0.2f)
                    {
                        npc.AddBuff(ModContent.BuffType<Uranium235Debuff>(), 60); // 0.5秒
                    }
                }
            }
        }

        private void ManageUranusAccessory()
        {
            bool uranusExists = false;
            Vector2 uranusPosition = Vector2.Zero;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<UranusProjectile>())
                {
                    uranusExists = true;
                    uranusPosition = proj.Center;
                    break;
                }
            }

            if (!uranusExists)
            {
                double angle = Main.rand.NextDouble() * 2 * Math.PI;
                uranusPosition = Player.Center + new Vector2((float)(Math.Cos(angle) * 35 * 16), (float)(Math.Sin(angle) * 35 * 16)); // 公转半径35格
                Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), uranusPosition, Vector2.Zero, ModContent.ProjectileType<UranusProjectile>(), 200, 0, Player.whoAmI);
            }
        }

    }
}
