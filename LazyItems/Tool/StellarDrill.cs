//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria.DataStructures;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using Terraria.Audio;

//namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
//{
//    public class StellarDrill : ModItem
//    {
//        public override void SetStaticDefaults()
//        {
//            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
//        }

//        public override void SetDefaults()
//        {
//            Item.width = 48;
//            Item.height = 48;
//            Item.damage = CalculateDamage();
//            Item.DamageType = DamageClass.Melee;
//            Item.useTime = 4;
//            Item.useAnimation = 20;
//            Item.useStyle = ItemUseStyleID.Shoot;
//            Item.knockBack = 0f;
//            Item.value = Item.buyPrice(gold: 40);
//            Item.rare = ItemRarityID.Expert;
//            Item.UseSound = SoundID.Item23;
//            Item.autoReuse = true;
//            Item.channel = true;
//            Item.noMelee = true;
//            Item.noUseGraphic = true;
//            Item.shoot = ModContent.ProjectileType<StellarDrillProj>();
//            Item.tileBoost += 10;
//            Item.pick = 59;
//        }

//        //public override bool AltFunctionUse(Player player) => true;

//        public override bool CanUseItem(Player player)
//        {
//            //if (player.altFunctionUse == 2)
//            //{
//            //    // 右键：锤+斧模式
//            //    Item.useStyle = ItemUseStyleID.Shoot;
//            //    Item.useTime = CalculateUseTime();
//            //    Item.useAnimation = 10;
//            //    Item.damage = CalculateDamage();
//            //    Item.axe = CalculateAxePower();
//            //    Item.hammer = CalculateHammerPower();
//            //    Item.pick = 0;
//            //    Item.channel = false;
//            //    Item.noMelee = false;
//            //    Item.noUseGraphic = false;
//            //    Item.shoot = ModContent.ProjectileType<StellarDrillProj>();
//            //}
//            //else
//            {
//                // 左键：钻头发射模式[不支持锤子]
//                Item.useStyle = ItemUseStyleID.Shoot;
//                Item.useTime = CalculateUseTime();
//                Item.useAnimation = 20;
//                Item.damage = CalculateDamage();
//                Item.pick = CalculatePickPower();
//                Item.axe = CalculateAxePower();
//                Item.axe = 0;
//                Item.hammer = 0;
//                Item.channel = true;
//                Item.noMelee = true;
//                Item.noUseGraphic = true;
//                Item.shoot = ModContent.ProjectileType<StellarDrillProj>();
//            }

//            return base.CanUseItem(player);
//        }

//        public override void UpdateInventory(Player player)
//        {
//            CalculateDamage();
//            CalculateUseTime();
//            CalculatePickPower();
//            CalculateAxePower();
//            CalculateHammerPower();
//        }

//        private int CalculateDamage()
//        {
//            if (NPC.downedMoonlord) return 150;
//            if (NPC.downedGolemBoss) return 120;
//            if (NPC.downedMechBossAny) return 98;
//            if (Main.hardMode) return 67;
//            if (NPC.downedBoss3) return 34;
//            if (NPC.downedBoss2) return 22;
//            return 15;
//        }

//        private int CalculateUseTime()
//        {
//            if (NPC.downedMoonlord) return 3;
//            if (NPC.downedGolemBoss) return 3;
//            if (NPC.downedMechBossAny) return 4;
//            if (Main.hardMode) return 5;
//            if (NPC.downedBoss3) return 6;
//            return 7;
//        }

//        private int CalculatePickPower()
//        {
//            if (NPC.downedMoonlord) return 250;
//            if (NPC.downedGolemBoss) return 210;
//            if (NPC.downedMechBossAny) return 200;
//            if (Main.hardMode) return 190;
//            if (NPC.downedBoss3) return 100;
//            if (NPC.downedBoss2) return 70;
//            return 59;
//        }

//        private int CalculateAxePower()
//        {
//            if (NPC.downedMoonlord) return 250;
//            if (NPC.downedGolemBoss) return 225;
//            if (NPC.downedMechBossAny) return 200;
//            if (Main.hardMode) return 175;
//            if (NPC.downedBoss3) return 150;
//            if (NPC.downedBoss2) return 125;
//            return 100;
//        }

//        private int CalculateHammerPower()
//        {
//            if (NPC.downedMoonlord) return 500;
//            if (NPC.downedGolemBoss) return 155;
//            if (NPC.downedMechBossAny) return 120;
//            if (Main.hardMode) return 80;
//            if (NPC.downedBoss3) return 75;
//            return 70;
//        }
//    }
//}



//namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
//{
//    public class StellarDrillProj : ModProjectile
//    {
//        public override string Texture => "AccessoryForPlanetaryEmpire/LazyItems/Tool/StellarDrill";
//        private Player Owner => Main.player[Projectile.owner];

//        public override void SetDefaults()
//        {
//            Projectile.width = 22;
//            Projectile.height = 22;
//            Projectile.aiStyle = ProjAIStyleID.Drill;
//            Projectile.friendly = true;
//            Projectile.penetrate = -1;
//            Projectile.tileCollide = false;
//            Projectile.hide = true;
//            Projectile.ownerHitCheck = true;
//            Projectile.DamageType = DamageClass.Melee;
//            Projectile.scale = 1f;
//        }

//        public override bool ShouldUpdatePosition() => false;

//        public override void AI()
//        {
//            if (!Owner.channel)
//            {
//                Projectile.Kill();
//                return;
//            }

//            Projectile.timeLeft = 2;

//            if (Projectile.soundDelay <= 0)
//            {
//                SoundEngine.PlaySound(SoundID.Item23, Owner.Center);
//                Projectile.soundDelay = 30;
//            }

//            Vector2 direction = (Main.MouseWorld - Owner.MountedCenter).SafeNormalize(Vector2.One);
//            Projectile.velocity = direction;
//            Projectile.rotation = direction.ToRotation();

//            Owner.heldProj = Projectile.whoAmI;
//            Owner.ChangeDir(Math.Sign(direction.X));
//            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, direction.ToRotation() - MathHelper.PiOver2);
//            Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, direction.ToRotation() - MathHelper.PiOver2 - MathHelper.PiOver4 * 0.5f * Owner.direction);
//            Owner.SetDummyItemTime(2);

//            Projectile.Center = Owner.MountedCenter + direction * 12f;

//            if (Main.rand.NextBool(6))
//            {
//                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch, Scale: 1.4f);
//                dust.noGravity = true;
//                dust.velocity *= 0.2f;
//                dust.velocity.Y = -Main.rand.NextFloat(1f, 3f);
//            }
//        }

//        public override bool PreDraw(ref Color lightColor)
//        {
//            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
//            Vector2 origin = new Vector2(tex.Width / 2f, tex.Height / 2f);
//            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
//            return false;
//        }
//    }
//}



