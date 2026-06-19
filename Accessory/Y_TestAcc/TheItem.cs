using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players;
using System;
using Terraria.GameContent;


namespace AccessoryForPlanetaryEmpire.Accessory.Y_TestAcc
{
    public class TheItem : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Y_TestAcc";

        private static int PanelType => ModContent.ProjectileType<TheItemKitCatalogPanel>();

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.shoot = PanelType;
            Item.shootSpeed = 0f;
            Item.UseSound = null;
            // 困难模式前：Orange，价值2金
            //Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.maxStack = 9999;
            Item.rare = ModContent.RarityType<DynamicRarity>();
        }

        public override bool CanUseItem(Player player)
        {
            return Main.myPlayer == player.whoAmI &&
                !Main.mapFullscreen &&
                !Main.blockMouse &&
                !player.mouseInterface &&
                !(Main.playerInventory && Main.HoverItem.type == Type);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (TryCloseExistingPanel(player))
            {
                SoundEngine.PlaySound(SoundID.MenuClose with { Volume = 0.58f, Pitch = 0.05f }, player.Center);
                return false;
            }

            Projectile.NewProjectile(
                source,
                player.Center,
                Vector2.Zero,
                PanelType,
                0,
                0f,
                player.whoAmI);

            SoundEngine.PlaySound(SoundID.MenuOpen with { Volume = 0.68f, Pitch = 0.08f }, player.Center);
            SoundEngine.PlaySound(SoundID.Item4 with { Volume = 0.42f, Pitch = 0.2f }, player.Center);
            return false;
        }

        private static bool TryCloseExistingPanel(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (!projectile.active || projectile.owner != player.whoAmI || projectile.type != PanelType)
                {
                    continue;
                }

                TheItemKitCatalogPanel.RequestClose(projectile);
                return true;
            }

            return false;
        }

        public void DrawBackAfterimage(SpriteBatch spriteBatch, Vector2 baseDrawPosition, Rectangle frame, float baseScale)
        {
            // 控制脉冲变化，让光有跳动感
            float pulse = (float)Math.Cos(1.618f * Main.GlobalTimeWrappedHourly * 2f)
                        + (float)Math.Cos(Math.E * Main.GlobalTimeWrappedHourly * 1.7f);
            pulse = pulse * 0.25f + 0.5f;
            pulse = (float)Math.Pow(pulse, 3); // 锐化脉冲

            // 光圈颜色（黄色 → 橙色之间变化）
            Color drawColor = Color.Lerp(new Color(255, 218, 99), new Color(249, 134, 44), pulse);
            drawColor *= MathHelper.Lerp(0.35f, 0.67f, pulse);
            drawColor.A = 25;

            // 环绕的 8 个光点
            float offset = MathHelper.Lerp(-0.3f, 1.2f, pulse) * baseScale * 16f; // 最后这个数字变大，可以增加它的夸张程度
            for (int i = 0; i < 8; i++)
            {
                Vector2 drawPosition =
                    baseDrawPosition + (MathHelper.TwoPi * i / 8f).ToRotationVector2() * offset;

                spriteBatch.Draw(TextureAssets.Item[Item.type].Value,
                                 drawPosition, frame, drawColor,
                                 0f, Vector2.Zero, baseScale,
                                 SpriteEffects.None, 0f);
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame,
    Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            // 调整坐标到中心
            //Vector2 center = position + frame.Size() * 0f;
            Vector2 center = position + new Vector2(-12f, -13f);
            DrawBackAfterimage(spriteBatch, center, frame, scale);
            return true;
        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
    ref float rotation, ref float scale, int whoAmI)
        {
            Rectangle frame = TextureAssets.Item[Item.type].Value.Frame();

            // Item.position 是左上角，需要改为中心点
            //Vector2 center = Item.Center - Main.screenPosition;
            Vector2 center = Item.Center + new Vector2(-12f, -13f);

            DrawBackAfterimage(spriteBatch, center, frame, scale);
            return true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", 2);
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 3);
            recipe.AddIngredient(ItemID.Chain, 15);
            recipe.AddIngredient(ItemID.Wood, 70);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
