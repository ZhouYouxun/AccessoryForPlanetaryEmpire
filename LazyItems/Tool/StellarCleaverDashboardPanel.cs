using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
{
    internal sealed class StellarCleaverDashboardPanel : ModProjectile
    {
        private const int PreferredPanelWidth = 620;
        private const int PreferredPanelHeight = 360;
        private const int ScreenMargin = 12;
        private const int BorderThickness = 3;

        private Vector2 panelTopLeft;
        private bool panelPositionInitialized;
        private int lastHoveredControl = -1;

        public override string Texture => "Terraria/Images/Projectile_1";

        private bool FadeOut
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        private static int PanelWidth => Math.Min(PreferredPanelWidth, Math.Max(520, Main.screenWidth - ScreenMargin * 2));
        private static int PanelHeight => Math.Min(PreferredPanelHeight, Math.Max(300, Main.screenHeight - ScreenMargin * 2));
        private static Rectangle MouseRectangle => new((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 9999999;
        }

        public override void SetDefaults()
        {
            Projectile.width = PreferredPanelWidth;
            Projectile.height = PreferredPanelHeight;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0f;
        }

        public override bool ShouldUpdatePosition() => false;
        public override bool? CanDamage() => false;

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead)
            {
                Projectile.Kill();
                return;
            }

            if (owner.HeldItem.type != ModContent.ItemType<StellarCleaver>())
            {
                FadeOut = true;
            }

            if (!panelPositionInitialized && Main.myPlayer == Projectile.owner)
            {
                Vector2 playerScreenCenter = owner.Center - Main.screenPosition;
                panelTopLeft = GetClampedPanelTopLeft(playerScreenCenter - new Vector2(PanelWidth, PanelHeight) * 0.5f);
                panelPositionInitialized = true;
            }

            Vector2 panelCenter = panelTopLeft + new Vector2(PanelWidth, PanelHeight) * 0.5f;
            Projectile.Center = Main.myPlayer == Projectile.owner ? Main.screenPosition + panelCenter : owner.Center;
            Projectile.timeLeft = 2;
            Projectile.Opacity = MathHelper.Clamp(Projectile.Opacity + (FadeOut ? -0.14f : 0.18f), 0f, 1f);

            if (FadeOut && Projectile.Opacity <= 0f)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.myPlayer != Projectile.owner)
            {
                return false;
            }

            Player owner = Main.player[Projectile.owner];
            StellarCleaverPlayer cleaverPlayer = owner.GetModPlayer<StellarCleaverPlayer>();
            int maxBoost = StellarCleaver.GetCurrentMaxTileBoost();
            cleaverPlayer.CurrentMaxTileBoost = maxBoost;
            cleaverPlayer.ClampSettings();

            Rectangle panelArea = new((int)panelTopLeft.X, (int)panelTopLeft.Y, PanelWidth, PanelHeight);
            Rectangle sliderArea = new(panelArea.X + 54, panelArea.Y + 142, panelArea.Width - 108, 58);
            Rectangle trackArea = new(sliderArea.X + 18, sliderArea.Y + 32, sliderArea.Width - 36, 8);
            Rectangle lightButton = new(panelArea.X + 54, panelArea.Y + 228, (panelArea.Width - 126) / 2, 42);
            Rectangle autoButton = new(lightButton.Right + 18, lightButton.Y, lightButton.Width, lightButton.Height);
            bool mouseOverPanel = panelArea.Intersects(MouseRectangle);
            bool leftClickPressed = Main.mouseLeft && Main.mouseLeftRelease;
            int hoveredControl = -1;

            DrawPanel(panelArea, Projectile.Opacity);
            DrawMatrixRain(panelArea, Projectile.Opacity);
            DrawHeader(panelArea, Projectile.Opacity);
            DrawRangeSlider(sliderArea, trackArea, cleaverPlayer, maxBoost, Projectile.Opacity);

            if ((trackArea.Intersects(MouseRectangle) || cleaverPlayer.RangeSliderDragging) && Projectile.Opacity >= 0.95f)
            {
                hoveredControl = 1;
                if (Main.mouseLeft)
                {
                    cleaverPlayer.RangeSliderDragging = true;
                    cleaverPlayer.AutoMaxRange = false;
                    float ratio = MathHelper.Clamp((Main.MouseScreen.X - trackArea.X) / Math.Max(1f, trackArea.Width), 0f, 1f);
                    cleaverPlayer.ManualTileBoost = (int)MathF.Round(ratio * maxBoost);
                    cleaverPlayer.ClampSettings();
                }
            }

            if (!Main.mouseLeft)
            {
                cleaverPlayer.RangeSliderDragging = false;
            }

            bool lightHovered = lightButton.Intersects(MouseRectangle);
            bool autoHovered = autoButton.Intersects(MouseRectangle);
            if (lightHovered)
            {
                hoveredControl = 2;
                if (leftClickPressed && Projectile.Opacity >= 0.95f)
                {
                    cleaverPlayer.LightingParticlesEnabled = !cleaverPlayer.LightingParticlesEnabled;
                    SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.5f, Pitch = cleaverPlayer.LightingParticlesEnabled ? 0.22f : -0.08f }, owner.Center);
                }
            }
            else if (autoHovered)
            {
                hoveredControl = 3;
                if (leftClickPressed && Projectile.Opacity >= 0.95f)
                {
                    cleaverPlayer.AutoMaxRange = !cleaverPlayer.AutoMaxRange;
                    if (cleaverPlayer.AutoMaxRange)
                    {
                        cleaverPlayer.ManualTileBoost = maxBoost;
                    }

                    SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.5f, Pitch = cleaverPlayer.AutoMaxRange ? 0.24f : 0f }, owner.Center);
                }
            }

            DrawToggleButton(lightButton, "LIGHT FX", cleaverPlayer.LightingParticlesEnabled, lightHovered, Projectile.Opacity);
            DrawToggleButton(autoButton, "AUTO MAX", cleaverPlayer.AutoMaxRange, autoHovered, Projectile.Opacity);
            DrawFooter(panelArea, Projectile.Opacity);
            PlayHoverSound(owner, hoveredControl);

            if (!mouseOverPanel && !FadeOut && Projectile.Opacity >= 0.95f && (leftClickPressed || (Main.mouseRight && Main.mouseRightRelease)))
            {
                FadeOut = true;
                SoundEngine.PlaySound(SoundID.MenuClose with { Volume = 0.58f, Pitch = 0.05f }, owner.Center);
            }

            if (mouseOverPanel)
            {
                Main.blockMouse = true;
                owner.mouseInterface = true;
            }

            return false;
        }

        public static bool HasPanel(Player player)
        {
            int panelType = ModContent.ProjectileType<StellarCleaverDashboardPanel>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.owner == player.whoAmI && projectile.type == panelType)
                {
                    return true;
                }
            }

            return false;
        }

        public static void Open(Player player, IEntitySource source)
        {
            if (HasPanel(player))
            {
                return;
            }

            Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<StellarCleaverDashboardPanel>(), 0, 0f, player.whoAmI);
            SoundEngine.PlaySound(SoundID.MenuOpen with { Volume = 0.68f, Pitch = 0.08f }, player.Center);
        }

        private static Vector2 GetClampedPanelTopLeft(Vector2 desiredTopLeft)
        {
            float maxX = Math.Max(ScreenMargin, Main.screenWidth - PanelWidth - ScreenMargin);
            float maxY = Math.Max(ScreenMargin, Main.screenHeight - PanelHeight - ScreenMargin);

            return new Vector2(
                MathHelper.Clamp(desiredTopLeft.X, ScreenMargin, maxX),
                MathHelper.Clamp(desiredTopLeft.Y, ScreenMargin, maxY));
        }

        private void PlayHoverSound(Player owner, int hoveredControl)
        {
            if (hoveredControl >= 0 && hoveredControl != lastHoveredControl && Projectile.Opacity >= 0.95f)
            {
                SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.28f, Pitch = 0.12f }, owner.Center);
            }

            lastHoveredControl = hoveredControl;
        }

        private static void DrawPanel(Rectangle panelArea, float opacity)
        {
            Color back = new(4, 10, 13, 242);
            Color border = new(86, 238, 255);
            DrawRectangle(panelArea, back * opacity);
            DrawBorder(panelArea, border * opacity, BorderThickness);

            Rectangle innerArea = new(panelArea.X + 9, panelArea.Y + 9, panelArea.Width - 18, panelArea.Height - 18);
            DrawBorder(innerArea, new Color(32, 100, 112, 210) * opacity, 1);

            for (int x = panelArea.X + 26; x < panelArea.Right - 26; x += 34)
            {
                DrawRectangle(new Rectangle(x, panelArea.Y + 16, 1, panelArea.Height - 32), new Color(48, 138, 150, 52) * opacity);
            }

            for (int y = panelArea.Y + 26; y < panelArea.Bottom - 26; y += 28)
            {
                DrawRectangle(new Rectangle(panelArea.X + 16, y, panelArea.Width - 32, 1), new Color(48, 138, 150, 52) * opacity);
            }

            int scanY = panelArea.Y + 14 + (int)((Main.GlobalTimeWrappedHourly * 58f) % Math.Max(1, panelArea.Height - 28));
            DrawRectangle(new Rectangle(panelArea.X + 14, scanY, panelArea.Width - 28, 2), new Color(110, 244, 255, 44) * opacity);
        }

        private static void DrawMatrixRain(Rectangle panelArea, float opacity)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Color rainColor = new Color(78, 238, 255) * (opacity * 0.15f);
            int columns = Math.Max(12, panelArea.Width / 34);
            float time = Main.GlobalTimeWrappedHourly * 24f;

            for (int i = 0; i < columns; i++)
            {
                int x = panelArea.X + 24 + i * 34;
                int y = panelArea.Y + 18 + (int)((time + i * 29) % Math.Max(1, panelArea.Height - 36));
                string glyph = (i % 5) switch
                {
                    0 => "SC",
                    1 => "01",
                    2 => "10",
                    3 => "DIG",
                    _ => "SYS"
                };

                ChatManager.DrawColorCodedString(Main.spriteBatch, font, glyph, new Vector2(x, y), rainColor, 0f, Vector2.Zero, Vector2.One * 0.48f);
            }
        }

        private static void DrawHeader(Rectangle panelArea, float opacity)
        {
            DrawCenteredFitText("STELLAR CLEAVER MATRIX", new Rectangle(panelArea.X + 34, panelArea.Y + 26, panelArea.Width - 68, 34), new Color(230, 255, 255), 0.92f, 0.56f, opacity);
            DrawCenteredFitText("RANGE VECTOR / PARTICLE EMITTER / AUTO PROFILE", new Rectangle(panelArea.X + 34, panelArea.Y + 62, panelArea.Width - 68, 22), new Color(112, 234, 255), 0.45f, 0.32f, opacity);
            DrawRectangle(new Rectangle(panelArea.X + 54, panelArea.Y + 102, panelArea.Width - 108, 2), new Color(92, 238, 255) * (opacity * 0.78f));
        }

        private static void DrawRangeSlider(Rectangle area, Rectangle trackArea, StellarCleaverPlayer cleaverPlayer, int maxBoost, float opacity)
        {
            int appliedBoost = cleaverPlayer.GetAppliedTileBoost(maxBoost);
            float ratio = maxBoost <= 0 ? 0f : MathHelper.Clamp(appliedBoost / (float)maxBoost, 0f, 1f);
            Color accent = new(86, 238, 255);
            Color dim = new(34, 92, 106);

            DrawRectangle(area, new Color(3, 16, 19, 206) * opacity);
            DrawBorder(area, dim * opacity, 1);
            DrawFitText("MINING RANGE EXTENSION", new Rectangle(area.X + 16, area.Y + 7, area.Width - 32, 20), new Color(206, 248, 252), 0.54f, 0.36f, opacity);
            DrawCenteredFitText($"+{appliedBoost} / +{maxBoost}", new Rectangle(area.Right - 118, area.Y + 7, 100, 20), accent, 0.48f, 0.32f, opacity);

            DrawRectangle(trackArea, new Color(0, 0, 0, 168) * opacity);
            DrawBorder(trackArea, accent * (opacity * 0.42f), 1);

            if (ratio > 0f)
            {
                DrawRectangle(new Rectangle(trackArea.X, trackArea.Y, (int)(trackArea.Width * ratio), trackArea.Height), accent * (opacity * 0.84f));
            }

            Vector2 knobCenter = new(trackArea.X + trackArea.Width * ratio, trackArea.Center.Y);
            DrawDiamond(knobCenter, 11f, Color.White * (opacity * 0.82f));
            DrawDiamond(knobCenter, 7f, accent * opacity);
        }

        private static void DrawToggleButton(Rectangle area, string label, bool enabled, bool hovered, float opacity)
        {
            Color accent = enabled ? new Color(92, 255, 196) : new Color(255, 100, 92);
            Color fill = Color.Lerp(new Color(5, 18, 22), accent, enabled ? 0.12f : 0.08f);
            Color border = hovered ? Color.White : accent;

            DrawRectangle(area, fill * (opacity * 0.92f));
            DrawBorder(area, border * opacity, hovered ? 2 : 1);
            DrawCenteredFitText(label, new Rectangle(area.X + 10, area.Y + 7, area.Width - 20, 16), Color.White, 0.48f, 0.32f, opacity);
            DrawCenteredFitText(enabled ? "ON" : "OFF", new Rectangle(area.X + 10, area.Y + 23, area.Width - 20, 14), accent, 0.42f, 0.28f, opacity);
        }

        private static void DrawFooter(Rectangle panelArea, float opacity)
        {
            DrawCenteredFitText("LMB DRAG SLIDER // CLICK OUTSIDE TO CLOSE", new Rectangle(panelArea.X + 36, panelArea.Bottom - 38, panelArea.Width - 72, 18), new Color(118, 190, 198), 0.42f, 0.3f, opacity);
        }

        private static void DrawDiamond(Vector2 center, float size, Color color)
        {
            Main.spriteBatch.Draw(
                TextureAssets.MagicPixel.Value,
                center,
                new Rectangle(0, 0, 1, 1),
                color,
                MathHelper.PiOver4,
                new Vector2(0.5f),
                new Vector2(size),
                SpriteEffects.None,
                0f);
        }

        private static void DrawFitText(string text, Rectangle area, Color color, float maxScale, float minScale, float opacity)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 size = font.MeasureString(text);
            if (size.X <= 0f || size.Y <= 0f)
            {
                return;
            }

            float scale = maxScale;
            if (size.X * scale > area.Width)
            {
                scale = area.Width / size.X;
            }

            scale = MathHelper.Clamp(scale, minScale, maxScale);
            DrawText(text, new Vector2(area.X, area.Y + Math.Max(0f, (area.Height - size.Y * scale) * 0.5f)), color * opacity, scale);
        }

        private static void DrawCenteredFitText(string text, Rectangle area, Color color, float maxScale, float minScale, float opacity)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 size = font.MeasureString(text);
            if (size.X <= 0f || size.Y <= 0f)
            {
                return;
            }

            float scale = maxScale;
            if (size.X * scale > area.Width)
            {
                scale = area.Width / size.X;
            }

            if (size.Y * scale > area.Height)
            {
                scale = Math.Min(scale, area.Height / size.Y);
            }

            scale = MathHelper.Clamp(scale, minScale, maxScale);
            Vector2 position = new(
                area.X + Math.Max(0f, (area.Width - size.X * scale) * 0.5f),
                area.Y + Math.Max(0f, (area.Height - size.Y * scale) * 0.5f));
            DrawText(text, position, color * opacity, scale);
        }

        private static void DrawText(string text, Vector2 position, Color color, float scale)
        {
            ChatManager.DrawColorCodedStringWithShadow(
                Main.spriteBatch,
                FontAssets.MouseText.Value,
                text,
                position,
                color,
                0f,
                Vector2.Zero,
                Vector2.One * scale);
        }

        private static void DrawBorder(Rectangle rectangle, Color color, int thickness)
        {
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            DrawRectangle(new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
        }

        private static void DrawRectangle(Rectangle rectangle, Color color)
        {
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, rectangle, color);
        }
    }
}
