using AccessoryForPlanetaryEmpire.Accessory.Defense.Counter;
using AccessoryForPlanetaryEmpire.Players.Attack.Gambler;
using AccessoryForPlanetaryEmpire.Players.Defense.Counter;
using Microsoft.Xna.Framework;
using System;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.Counter
{
    public class ControllableStellarEnergy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.Counter";
        public override void SetStaticDefaults()
        {


        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 最终毕业装：Master，价值95金
            Item.rare = ItemRarityID.Master;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.value = Item.sellPrice(0, 95, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ChargedBulletNestPlayer>().hasChargedBulletNest = true;
            player.GetModPlayer<DarkSideOfTheMoonPlayer>().DarkSideOfTheMoonEquipped = true;
            player.GetModPlayer<FirepowerExoskeletonDevicePlayer>().firepowerExoskeleton = true;
            player.GetModPlayer<NervousSystemDisorderPlayer>().hasNervousSystemDisorderDevice = true;
            player.GetModPlayer<ReactShieldGeneratorPlayer>().hasReactShieldGenerator = true;
            player.GetModPlayer<ScarabPlayer>().hasScarab = true;
            player.GetModPlayer<ShinySlimeGunPlayer>().hasShinySlimeGun = true;
            player.GetModPlayer<SlimeGunPlayer>().hasSlimeGun = true;


            if (hideVisual) // 隐藏特效
            {
                player.GetModPlayer<ControllableStellarEnergyPlayer>().ControllableStellarEnergyEquipped = false;
            }
            else // 释放特效
            {
                player.GetModPlayer<ControllableStellarEnergyPlayer>().ControllableStellarEnergyEquipped = true;
            }

        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ChargedBulletNest>(), 1);
            recipe.AddIngredient(ModContent.ItemType<DarkSideOfTheMoon>(), 1);
            recipe.AddIngredient(ModContent.ItemType<FirepowerExoskeletonDevice>(), 1);
            recipe.AddIngredient(ModContent.ItemType<MagicSeeker>(), 1);
            recipe.AddIngredient(ModContent.ItemType<NervousSystemDisorderDevice>(), 1);
            recipe.AddIngredient(ModContent.ItemType<ReactShieldGenerator>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Scarab>(), 1);
            recipe.AddIngredient(ModContent.ItemType<ShinySlimeGun>(), 1);
            recipe.AddIngredient(ModContent.ItemType<SlimeGun>(), 1);

            // 判断是否启用了 CalamityMod
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                recipe.AddIngredient(calamityMod.Find<ModItem>("DazzlingStabberStaff").Type, 1);
                recipe.AddIngredient(calamityMod.Find<ModItem>("ViridVanguard").Type, 1);
                recipe.AddIngredient(calamityMod.Find<ModItem>("SnakeEyes").Type, 1);
            }

            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}

namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
{
    public class ControllableStellarEnergyPlayer : ModPlayer
    {
        public bool ControllableStellarEnergyEquipped;

        public override void ResetEffects()
        {
            ControllableStellarEnergyEquipped = false;
        }
        public override void PostUpdate()
        {
            // ===== 未佩戴：立刻清理弹幕 =====
            if (!ControllableStellarEnergyEquipped)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active &&
                        p.owner == Player.whoAmI &&
                        p.type == ModContent.ProjectileType<ControllableStellarEnergyProjection>())
                    {
                        p.Kill();
                    }
                }
                return;
            }

            // ===== 佩戴：保证身上只有一个 =====
            bool exists = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active &&
                    p.owner == Player.whoAmI &&
                    p.type == ModContent.ProjectileType<ControllableStellarEnergyProjection>())
                {
                    exists = true;
                    break;
                }
            }

            if (!exists && Player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<ControllableStellarEnergyProjection>(),
                    0,
                    0f,
                    Player.whoAmI
                );
            }
        }




    }
}