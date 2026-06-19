//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria;
//using Terraria.Audio;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
//{
//    public class DestroyBomb : ModItem
//    {
//        public override void SetDefaults()
//        {
//            Item.width = 20;
//            Item.height = 26;
//            Item.useTime = Item.useAnimation = 12;
//            Item.maxStack = 999;
//            Item.damage = 99;
//            Item.consumable = true;
//            Item.shootSpeed = 25f;
//            Item.shoot = ModContent.ProjectileType<DestroyBombProj>();
//            Item.useStyle = ItemUseStyleID.Swing;
//            Item.noMelee = true;
//            Item.noUseGraphic = true;
//            Item.UseSound = SoundID.Item1;
//            Item.value = Item.buyPrice(silver: 50);
//            Item.rare = ItemRarityID.LightRed;
//        }

//        public override void AddRecipes()
//        {
//            CreateRecipe(5)
//                .AddIngredient(ItemID.Detonator) // 原版雷管
//                .AddTile(TileID.Anvils)
//                .Register();
//        }
//    }
//}


//namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
//{
//    public class DestroyBombProj : ModProjectile
//    {
//        public override string Texture => "AccessoryForPlanetaryEmpire/LazyItems/Tool/DestroyBomb";

//        public override void SetDefaults()
//        {
//            Projectile.width = 20;
//            Projectile.height = 20;
//            Projectile.friendly = false;
//            Projectile.hostile = true;
//            Projectile.tileCollide = true;
//            Projectile.penetrate = -1;
//            Projectile.timeLeft = 580;
//            Projectile.aiStyle = 0;
//        }

//        public override void AI()
//        {
//            Projectile.velocity.Y += 0.3f; // 重力影响
//            Projectile.rotation += 0.2f * Projectile.direction;

//            if (Main.rand.NextBool(3))
//            {
//                Dust.NewDust(Projectile.Center, 0, 0, DustID.Electric, 0, 0, 150, Color.Cyan, 1.5f);
//            }

//            if (Main.rand.NextBool(4))
//            {
//                Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueCrystalShard, 0, 0, 150, Color.Blue, 1.2f);
//            }
//        }

//        public override bool OnTileCollide(Vector2 oldVelocity)
//        {
//            Projectile.Kill();
//            return false;
//        }

//        public override void OnKill(int timeLeft)
//        {
//            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

//            // 科幻风蓝色电爆粒子
//            for (int i = 0; i < 100; i++)
//            {
//                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 150, Color.Cyan, 2f);
//                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 100, Color.LightBlue, 1.8f);
//            }

//            if (Projectile.owner == Main.myPlayer)
//            {
//                Projectile.ExpandHitboxBy(300); // 超大爆炸范围
//                Projectile.Damage();
//                Projectile.ExplodeTiles(10, true); // 地形破坏范围也大
//            }
//        }

//        public override bool PreDraw(ref Color lightColor)
//        {
//            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
//            Vector2 origin = tex.Size() / 2f;
//            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
//            return false;
//        }
//    }
//}

//// === 本地扩展函数 ===
//internal static class DestroyBombUtils
//{
//    public static void ExpandHitboxBy(this Projectile projectile, int newSize)
//    {
//        projectile.position = projectile.Center;
//        projectile.width = newSize;
//        projectile.height = newSize;
//        projectile.position -= projectile.Size * 0.5f;
//    }

//    public static void ExplodeTiles(this Projectile p, int radius, bool respectVanilla = true)
//    {
//        Vector2 explosionPos = p.Center;
//        int minX = (int)(explosionPos.X / 16f) - radius;
//        int maxX = (int)(explosionPos.X / 16f) + radius;
//        int minY = (int)(explosionPos.Y / 16f) - radius;
//        int maxY = (int)(explosionPos.Y / 16f) + radius;

//        minX = Utils.Clamp(minX, 0, Main.maxTilesX);
//        maxX = Utils.Clamp(maxX, 0, Main.maxTilesX);
//        minY = Utils.Clamp(minY, 0, Main.maxTilesY);
//        maxY = Utils.Clamp(maxY, 0, Main.maxTilesY);

//        float centerX = explosionPos.X / 16f;
//        float centerY = explosionPos.Y / 16f;

//        for (int x = minX; x <= maxX; x++)
//        {
//            for (int y = minY; y <= maxY; y++)
//            {
//                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
//                if (dist > radius)
//                    continue;

//                Tile tile = Main.tile[x, y];
//                if (tile == null || !tile.HasTile)
//                    continue;

//                if (respectVanilla && (!TileLoader.CanExplode(x, y) || Main.tileDungeon[tile.TileType]))
//                    continue;

//                WorldGen.KillTile(x, y, false, false, false);

//                if (!tile.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
//                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y);
//            }
//        }
//    }
//}
