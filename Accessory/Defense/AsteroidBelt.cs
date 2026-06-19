using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using AccessoryForPlanetaryEmpire.Projectiles;
using AccessoryForPlanetaryEmpire.Players.Attack.JobSkill;
using AccessoryForPlanetaryEmpire.Players.Defense;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense
{
    public class AsteroidBelt : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense";

        private const int Cooldown = 30 * 60; // 30秒冷却时间
        private int timer = 0;

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值25金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AsteroidBeltPlayer>().hasAsteroidBelt = true;

            // 检查是否已经有一个活跃的AsteroidBeltPROJ弹幕
            bool asteroidExists = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<AsteroidBeltPROJ>())
                {
                    asteroidExists = true;
                    break;
                }
            }

            // 如果没有找到活跃的AsteroidBeltPROJ弹幕，则在5秒后生成一个新的
            if (!asteroidExists)
            {
                player.GetModPlayer<AsteroidBeltPlayer>().asteroidSpawnTimer++;
                if (player.GetModPlayer<AsteroidBeltPlayer>().asteroidSpawnTimer >= 600) // 10秒 (60帧/秒)
                {
                    Vector2 positionAbove = player.Center + new Vector2(0, -20 * 16); // 20格距离上方（1格=16像素）
                    Projectile.NewProjectile(player.GetSource_Accessory(Item), positionAbove, Vector2.Zero, ModContent.ProjectileType<AsteroidBeltPROJ>(), 0, 0, player.whoAmI);
                    player.GetModPlayer<AsteroidBeltPlayer>().asteroidSpawnTimer = 0; // 重置计时器
                }
            }
            else
            {
                player.GetModPlayer<AsteroidBeltPlayer>().asteroidSpawnTimer = 0; // 如果小行星存在，重置计时器
            }

        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.MeteoriteBar, 25); // 25个陨石锭
            recipe.AddIngredient(ItemID.CrystalShard, 15); // 15个水晶碎块
            recipe.AddIngredient(ItemID.FallenStar, 50); // 50个陨落之星
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
