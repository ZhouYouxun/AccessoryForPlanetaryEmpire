using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace AccessoryForPlanetaryEmpire.Accessory.Y_TestAcc
{
    internal sealed class TheItemKitCatalogPanel : ModProjectile, ILocalizedModType
    {
        private const int PreferredPanelWidth = 900;
        private const int PreferredPanelHeight = 560;
        private const int ScreenMargin = 12;
        private const int BorderThickness = 3;
        private const int TransitionDuration = 18;
        private const string ModName = "AccessoryForPlanetaryEmpire";

        private static readonly CatalogCategory[] CategoryOrder =
        {
            CatalogCategory.Attack,
            CatalogCategory.Defense,
            CatalogCategory.Movement,
            CatalogCategory.Dash,
            CatalogCategory.Job,
            CatalogCategory.Wings,
            CatalogCategory.Utility
        };

        private static EquipmentCatalogEntry[] cachedEntries;
        private static RasterizerState clippingRasterizer;

        private Vector2 panelTopLeft;
        private bool panelPositionInitialized;
        private int categoryIndex;
        private int entryIndex;
        private EquipmentCatalogEntry previousEntry;
        private int transitionTimer = TransitionDuration;
        private int transitionDirection = 1;
        private int lastHoveredControl = -1;

        public override string Texture => "Terraria/Images/Projectile_1";
        public new string LocalizationCategory => "Projectiles";

        private bool FadeOut
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        private static int PanelWidth => Math.Min(PreferredPanelWidth, Math.Max(620, Main.screenWidth - ScreenMargin * 2));
        private static int PanelHeight => Math.Min(PreferredPanelHeight, Math.Max(430, Main.screenHeight - ScreenMargin * 2));
        private static Rectangle MouseRectangle => new((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);

        public override void Load()
        {
            if (!Main.dedServ)
            {
                clippingRasterizer = new RasterizerState
                {
                    CullMode = CullMode.None,
                    ScissorTestEnable = true
                };
            }
        }

        public override void Unload()
        {
            clippingRasterizer?.Dispose();
            clippingRasterizer = null;
            cachedEntries = null;
        }

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

            if (owner.HeldItem.type != ModContent.ItemType<TheItem>())
            {
                FadeOut = true;
            }

            if (!panelPositionInitialized && Main.myPlayer == Projectile.owner)
            {
                panelTopLeft = GetClampedPanelTopLeft(Main.MouseScreen - new Vector2(PanelWidth, PanelHeight) * 0.5f);
                panelPositionInitialized = true;
            }

            Vector2 panelCenter = panelTopLeft + new Vector2(PanelWidth, PanelHeight) * 0.5f;
            Projectile.Center = Main.myPlayer == Projectile.owner ? Main.screenPosition + panelCenter : owner.Center;
            Projectile.timeLeft = 2;
            Projectile.Opacity = MathHelper.Clamp(Projectile.Opacity + (FadeOut ? -0.14f : 0.18f), 0f, 1f);

            if (transitionTimer < TransitionDuration)
            {
                transitionTimer++;
            }

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
            EquipmentCatalogEntry[] entries = GetEntries();
            EnsureValidSelection(entries);

            Rectangle panelArea = new((int)panelTopLeft.X, (int)panelTopLeft.Y, PanelWidth, PanelHeight);
            Rectangle categoryArea = new(panelArea.X + 24, panelArea.Y + 69, panelArea.Width - 48, 38);
            Rectangle contentArea = new(panelArea.X + 24, panelArea.Y + 119, panelArea.Width - 48, panelArea.Height - 166);
            Rectangle leftArrowArea = new(contentArea.X + 8, contentArea.Y + contentArea.Height / 2 - 34, 54, 68);
            Rectangle rightArrowArea = new(contentArea.Right - 62, contentArea.Y + contentArea.Height / 2 - 34, 54, 68);
            Rectangle cardArea = new(contentArea.X + 76, contentArea.Y + 5, contentArea.Width - 152, contentArea.Height - 10);
            bool mouseOverPanel = panelArea.Intersects(MouseRectangle);
            bool leftClickPressed = Main.mouseLeft && Main.mouseLeftRelease;
            bool rightClickPressed = Main.mouseRight && Main.mouseRightRelease;
            int hoveredControl = -1;

            DrawPanel(panelArea, Projectile.Opacity);
            DrawMatrixRain(panelArea, Projectile.Opacity);
            DrawHeader(panelArea, entries.Length, Projectile.Opacity);

            for (int i = 0; i < CategoryOrder.Length; i++)
            {
                Rectangle tabArea = GetCategoryTabArea(categoryArea, i);
                bool hovered = tabArea.Intersects(MouseRectangle);
                bool selected = i == categoryIndex;
                int count = CountEntries(entries, CategoryOrder[i]);
                DrawCategoryTab(CategoryOrder[i], count, tabArea, hovered, selected, Projectile.Opacity);

                if (hovered)
                {
                    hoveredControl = 100 + i;
                    if (leftClickPressed && Projectile.Opacity >= 0.95f && count > 0 && i != categoryIndex)
                    {
                        ChangeCategory(entries, i);
                    }
                }
            }

            EquipmentCatalogEntry[] categoryEntries = GetCategoryEntries(entries, CurrentCategory);
            EquipmentCatalogEntry currentEntry = categoryEntries.Length > 0 ? categoryEntries[entryIndex] : null;
            bool leftHovered = leftArrowArea.Intersects(MouseRectangle);
            bool rightHovered = rightArrowArea.Intersects(MouseRectangle);

            DrawContentFrame(contentArea, GetCategoryColor(CurrentCategory), Projectile.Opacity);

            if (leftHovered)
            {
                hoveredControl = 1;
                if (leftClickPressed && Projectile.Opacity >= 0.95f)
                {
                    ChangeEntry(categoryEntries, -1);
                }
            }
            else if (rightHovered)
            {
                hoveredControl = 2;
                if (leftClickPressed && Projectile.Opacity >= 0.95f)
                {
                    ChangeEntry(categoryEntries, 1);
                }
            }

            DrawAnimatedCardClipped(currentEntry, cardArea, categoryEntries.Length, Projectile.Opacity);
            DrawArrowButton(leftArrowArea, false, leftHovered, categoryEntries.Length > 1, Projectile.Opacity);
            DrawArrowButton(rightArrowArea, true, rightHovered, categoryEntries.Length > 1, Projectile.Opacity);
            DrawFooter(panelArea, Projectile.Opacity);
            PlayHoverSound(owner, hoveredControl);

            if (!mouseOverPanel && !FadeOut && Projectile.Opacity >= 0.95f && (leftClickPressed || rightClickPressed))
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

        public static void RequestClose(Projectile projectile)
        {
            if (projectile.ModProjectile is TheItemKitCatalogPanel panel)
            {
                panel.FadeOut = true;
            }
            else
            {
                projectile.ai[0] = 1f;
            }
        }

        private CatalogCategory CurrentCategory => CategoryOrder[Math.Clamp(categoryIndex, 0, CategoryOrder.Length - 1)];

        private static EquipmentCatalogEntry[] GetEntries()
        {
            if (cachedEntries is { Length: > 0 })
            {
                return cachedEntries;
            }

            cachedEntries = ModContent.GetContent<ModItem>()
                .Where(modItem => modItem.Mod.Name == ModName && modItem.Item.accessory)
                .Select(modItem => new EquipmentCatalogEntry(modItem, GetCategory(modItem)))
                .OrderBy(entry => Array.IndexOf(CategoryOrder, entry.Category))
                .ThenBy(entry => GetSubcategory(entry.ModItem))
                .ThenBy(entry => Lang.GetItemNameValue(entry.ItemType))
                .ToArray();

            return cachedEntries;
        }

        private static CatalogCategory GetCategory(ModItem modItem)
        {
            string name = modItem.GetType().FullName ?? string.Empty;

            if (name.Contains(".Accessory.Attack.", StringComparison.Ordinal))
            {
                return CatalogCategory.Attack;
            }

            if (name.Contains(".Accessory.Defense.", StringComparison.Ordinal))
            {
                return CatalogCategory.Defense;
            }

            if (name.Contains(".Accessory.Movement.", StringComparison.Ordinal))
            {
                return CatalogCategory.Movement;
            }

            if (name.Contains(".Accessory.Dash.", StringComparison.Ordinal))
            {
                return CatalogCategory.Dash;
            }

            if (name.Contains(".Accessory.JobAcc.", StringComparison.Ordinal))
            {
                return CatalogCategory.Job;
            }

            if (name.Contains(".Accessory.Wings.", StringComparison.Ordinal))
            {
                return CatalogCategory.Wings;
            }

            return CatalogCategory.Utility;
        }

        private static string GetSubcategory(ModItem modItem)
        {
            string fullName = modItem.GetType().FullName ?? string.Empty;
            const string marker = ".Accessory.";
            int markerIndex = fullName.IndexOf(marker, StringComparison.Ordinal);
            if (markerIndex < 0)
            {
                return fullName;
            }

            string path = fullName[(markerIndex + marker.Length)..];
            int lastSeparator = path.LastIndexOf('.');
            return lastSeparator > 0 ? path[..lastSeparator] : string.Empty;
        }

        private static EquipmentCatalogEntry[] GetCategoryEntries(EquipmentCatalogEntry[] entries, CatalogCategory category)
        {
            return entries.Where(entry => entry.Category == category).ToArray();
        }

        private static int CountEntries(EquipmentCatalogEntry[] entries, CatalogCategory category)
        {
            return entries.Count(entry => entry.Category == category);
        }

        private void EnsureValidSelection(EquipmentCatalogEntry[] entries)
        {
            categoryIndex = Math.Clamp(categoryIndex, 0, CategoryOrder.Length - 1);
            EquipmentCatalogEntry[] categoryEntries = GetCategoryEntries(entries, CurrentCategory);
            entryIndex = categoryEntries.Length == 0 ? 0 : Math.Clamp(entryIndex, 0, categoryEntries.Length - 1);
        }

        private void ChangeCategory(EquipmentCatalogEntry[] entries, int newCategoryIndex)
        {
            EquipmentCatalogEntry[] currentEntries = GetCategoryEntries(entries, CurrentCategory);
            previousEntry = currentEntries.Length > 0 ? currentEntries[entryIndex] : null;
            transitionDirection = newCategoryIndex > categoryIndex ? 1 : -1;
            categoryIndex = newCategoryIndex;
            entryIndex = 0;
            transitionTimer = 0;
            SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.62f, Pitch = 0.28f }, Main.LocalPlayer.Center);
        }

        private void ChangeEntry(EquipmentCatalogEntry[] entries, int direction)
        {
            if (entries.Length <= 1)
            {
                return;
            }

            previousEntry = entries[entryIndex];
            transitionDirection = direction;
            entryIndex = (entryIndex + direction + entries.Length) % entries.Length;
            transitionTimer = 0;
            SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.66f, Pitch = direction > 0 ? 0.24f : 0.06f }, Main.LocalPlayer.Center);
            SoundEngine.PlaySound(SoundID.Item4 with { Volume = 0.22f, Pitch = 0.42f }, Main.LocalPlayer.Center);
        }

        private void PlayHoverSound(Player owner, int hoveredControl)
        {
            if (hoveredControl >= 0 && hoveredControl != lastHoveredControl && Projectile.Opacity >= 0.95f)
            {
                SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.36f, Pitch = 0.16f }, owner.Center);
            }

            lastHoveredControl = hoveredControl;
        }

        private static Vector2 GetClampedPanelTopLeft(Vector2 desiredTopLeft)
        {
            float maxX = Math.Max(ScreenMargin, Main.screenWidth - PanelWidth - ScreenMargin);
            float maxY = Math.Max(ScreenMargin, Main.screenHeight - PanelHeight - ScreenMargin);

            return new Vector2(
                MathHelper.Clamp(desiredTopLeft.X, ScreenMargin, maxX),
                MathHelper.Clamp(desiredTopLeft.Y, ScreenMargin, maxY));
        }

        private static Rectangle GetCategoryTabArea(Rectangle categoryArea, int index)
        {
            int tabWidth = categoryArea.Width / CategoryOrder.Length;
            int x = categoryArea.X + index * tabWidth;
            int width = index == CategoryOrder.Length - 1 ? categoryArea.Right - x : tabWidth - 3;
            return new Rectangle(x, categoryArea.Y, width, categoryArea.Height);
        }

        private static void DrawPanel(Rectangle panelArea, float opacity)
        {
            Color back = new(5, 10, 13, 246);
            Color border = new(72, 255, 190);
            DrawRectangle(panelArea, back * opacity);
            DrawBorder(panelArea, border * opacity, BorderThickness);

            Rectangle innerArea = new(panelArea.X + 9, panelArea.Y + 9, panelArea.Width - 18, panelArea.Height - 18);
            DrawBorder(innerArea, new Color(24, 96, 82, 210) * opacity, 1);

            for (int x = panelArea.X + 24; x < panelArea.Right - 24; x += 32)
            {
                DrawRectangle(new Rectangle(x, panelArea.Y + 14, 1, panelArea.Height - 28), new Color(34, 106, 90, 64) * opacity);
            }

            for (int y = panelArea.Y + 24; y < panelArea.Bottom - 24; y += 28)
            {
                DrawRectangle(new Rectangle(panelArea.X + 14, y, panelArea.Width - 28, 1), new Color(34, 106, 90, 64) * opacity);
            }

            int scanY = panelArea.Y + 14 + (int)((Main.GlobalTimeWrappedHourly * 52f) % Math.Max(1, panelArea.Height - 28));
            DrawRectangle(new Rectangle(panelArea.X + 14, scanY, panelArea.Width - 28, 2), new Color(96, 255, 202, 48) * opacity);
        }

        private static void DrawMatrixRain(Rectangle panelArea, float opacity)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Color rainColor = new Color(56, 255, 168) * (opacity * 0.16f);
            int left = panelArea.X + 24;
            int right = panelArea.Right - 48;
            int columns = Math.Max(1, (right - left) / 34 + 1);
            float time = Main.GlobalTimeWrappedHourly * 22f;

            for (int i = 0; i < columns; i++)
            {
                int x = left + i * 34;
                if (x > right)
                {
                    break;
                }

                int y = panelArea.Y + 18 + (int)((time + i * 31) % Math.Max(1, panelArea.Height - 36));
                string glyph = (i % 4) switch
                {
                    0 => "01",
                    1 => "PLA",
                    2 => "10",
                    _ => "[]"
                };

                ChatManager.DrawColorCodedString(Main.spriteBatch, font, glyph, new Vector2(x, y), rainColor, 0f, Vector2.Zero, Vector2.One * 0.54f);
            }
        }

        private static void DrawHeader(Rectangle panelArea, int totalEntries, float opacity)
        {
            Rectangle titleArea = new(panelArea.X + 28, panelArea.Y + 19, panelArea.Width - 56, 28);
            DrawCenteredFitText(
                GetText("Title"),
                titleArea,
                new Color(224, 255, 244),
                0.92f,
                0.58f,
                opacity);

            Rectangle subtitleArea = new(panelArea.X + 30, panelArea.Y + 47, panelArea.Width - 60, 18);
            string subtitle = Language.GetTextValue("Mods.AccessoryForPlanetaryEmpire.TheItemCatalog.Subtitle", totalEntries);
            DrawCenteredFitText(subtitle, subtitleArea, new Color(112, 224, 188), 0.5f, 0.35f, opacity);
        }

        private static void DrawCategoryTab(CatalogCategory category, int count, Rectangle area, bool hovered, bool selected, float opacity)
        {
            Color accent = GetCategoryColor(category);
            Color back = Color.Lerp(new Color(9, 21, 23), accent, selected ? 0.28f : hovered ? 0.16f : 0.07f);
            Color border = Color.Lerp(new Color(42, 104, 92), accent, selected ? 0.8f : hovered ? 0.58f : 0.28f);

            DrawRectangle(area, back * (opacity * 0.94f));
            DrawBorder(area, border * opacity, selected ? 2 : 1);

            if (selected)
            {
                DrawRectangle(new Rectangle(area.X + 5, area.Bottom - 4, area.Width - 10, 2), accent * opacity);
            }

            string text = $"{GetCategoryName(category)} [{count}]";
            DrawCenteredFitText(text, new Rectangle(area.X + 4, area.Y + 4, area.Width - 8, area.Height - 8), selected ? Color.White : new Color(184, 218, 208), 0.55f, 0.31f, opacity);
        }

        private static void DrawContentFrame(Rectangle area, Color accent, float opacity)
        {
            DrawRectangle(area, new Color(4, 12, 15, 224) * opacity);
            DrawBorder(area, Color.Lerp(new Color(30, 96, 82), accent, 0.42f) * opacity, 2);

            Rectangle inner = new(area.X + 7, area.Y + 7, area.Width - 14, area.Height - 14);
            DrawBorder(inner, Color.Lerp(new Color(18, 65, 58), accent, 0.2f) * opacity, 1);
        }

        private static void DrawArrowButton(Rectangle area, bool pointsRight, bool hovered, bool enabled, float opacity)
        {
            Color accent = enabled ? new Color(86, 255, 194) : new Color(66, 92, 84);
            Color back = hovered && enabled ? new Color(18, 82, 64) : new Color(8, 31, 29);
            Color border = hovered && enabled ? Color.White : accent;
            float pulse = hovered && enabled ? 0.7f + 0.3f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 8f) : 0.74f;

            DrawRectangle(area, back * (opacity * 0.9f));
            DrawBorder(area, border * (opacity * pulse), hovered && enabled ? 2 : 1);
            DrawClipped(area, () => DrawChevron(area.Center.ToVector2(), pointsRight, accent * (opacity * pulse), hovered && enabled ? 3 : 2));
        }

        private void DrawAnimatedCardClipped(EquipmentCatalogEntry currentEntry, Rectangle cardArea, int categoryCount, float opacity)
        {
            DrawClipped(cardArea, () => DrawAnimatedCard(currentEntry, cardArea, categoryCount, opacity));
        }

        private static void DrawClipped(Rectangle area, Action drawAction)
        {
            GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;
            Rectangle previousScissorRectangle = graphicsDevice.ScissorRectangle;
            Rectangle clipRectangle = TransformToScissorRectangle(area);
            if (clipRectangle.Width <= 0 || clipRectangle.Height <= 0 || clippingRasterizer == null)
            {
                return;
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                clippingRasterizer,
                null,
                Main.GameViewMatrix.TransformationMatrix);

            graphicsDevice.ScissorRectangle = clipRectangle;
            drawAction();
            Main.spriteBatch.End();

            graphicsDevice.ScissorRectangle = previousScissorRectangle;
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.GameViewMatrix.TransformationMatrix);
        }

        private static Rectangle TransformToScissorRectangle(Rectangle area)
        {
            Matrix transform = Main.GameViewMatrix.TransformationMatrix;
            Vector2 topLeft = Vector2.Transform(new Vector2(area.Left, area.Top), transform);
            Vector2 topRight = Vector2.Transform(new Vector2(area.Right, area.Top), transform);
            Vector2 bottomLeft = Vector2.Transform(new Vector2(area.Left, area.Bottom), transform);
            Vector2 bottomRight = Vector2.Transform(new Vector2(area.Right, area.Bottom), transform);

            float minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            float minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            float maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            float maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

            Rectangle transformed = new(
                (int)Math.Floor(minX),
                (int)Math.Floor(minY),
                Math.Max(1, (int)Math.Ceiling(maxX) - (int)Math.Floor(minX)),
                Math.Max(1, (int)Math.Ceiling(maxY) - (int)Math.Floor(minY)));

            Viewport viewport = Main.graphics.GraphicsDevice.Viewport;
            return Rectangle.Intersect(transformed, new Rectangle(0, 0, viewport.Width, viewport.Height));
        }

        private void DrawAnimatedCard(EquipmentCatalogEntry currentEntry, Rectangle cardArea, int categoryCount, float opacity)
        {
            float progress = MathHelper.Clamp(transitionTimer / (float)TransitionDuration, 0f, 1f);
            float eased = 1f - (float)Math.Pow(1f - progress, 3f);

            if (previousEntry != null && progress < 1f)
            {
                float oldOffset = -transitionDirection * 92f * eased;
                DrawEntryCard(previousEntry, cardArea, categoryCount, oldOffset, 1f - eased, 1f - 0.08f * eased, opacity);
            }

            float newOffset = transitionDirection * 92f * (1f - eased);
            DrawEntryCard(currentEntry, cardArea, categoryCount, newOffset, eased, 0.92f + 0.08f * eased, opacity);
        }

        private void DrawEntryCard(EquipmentCatalogEntry entry, Rectangle cardArea, int categoryCount, float xOffset, float animationOpacity, float scale, float panelOpacity)
        {
            if (animationOpacity <= 0.01f)
            {
                return;
            }

            float opacity = panelOpacity * animationOpacity;
            Color accent = GetCategoryColor(entry?.Category ?? CurrentCategory);
            Vector2 center = cardArea.Center.ToVector2() + new Vector2(xOffset, 0f);
            Rectangle scaledArea = ScaleRectangle(cardArea, center, scale);

            DrawRectangle(scaledArea, Color.Lerp(new Color(7, 17, 20), accent, 0.08f) * (opacity * 0.96f));
            DrawBorder(scaledArea, Color.Lerp(new Color(44, 126, 108), accent, 0.58f) * opacity, 2);
            DrawCornerBrackets(scaledArea, accent * opacity);

            if (entry == null)
            {
                DrawCenteredFitText(
                    GetText("NoEntries"),
                    new Rectangle(scaledArea.X + 25, scaledArea.Y + 25, scaledArea.Width - 50, scaledArea.Height - 50),
                    new Color(190, 220, 210),
                    0.66f,
                    0.4f,
                    opacity);
                return;
            }

            int iconBoxSize = Math.Min(164, Math.Max(118, scaledArea.Height - 150));
            Rectangle iconFrame = new(scaledArea.X + 26, scaledArea.Y + 64, iconBoxSize, iconBoxSize);
            Rectangle textArea = new(iconFrame.Right + 28, scaledArea.Y + 65, scaledArea.Right - iconFrame.Right - 54, scaledArea.Height - 118);
            Rectangle categoryLabelArea = new(scaledArea.X + 24, scaledArea.Y + 18, scaledArea.Width - 48, 24);
            Rectangle nameArea = new(textArea.X, scaledArea.Y + 22, textArea.Width, 31);
            Rectangle indexArea = new(scaledArea.X + 24, scaledArea.Bottom - 35, scaledArea.Width - 48, 18);

            DrawCenteredFitText(GetCategoryName(entry.Category), categoryLabelArea, accent, 0.56f, 0.35f, opacity);
            DrawFitText(Lang.GetItemNameValue(entry.ItemType), nameArea, Color.White, 0.82f, 0.46f, opacity);

            DrawRectangle(iconFrame, Color.Lerp(new Color(3, 12, 14), accent, 0.1f) * (opacity * 0.9f));
            DrawBorder(iconFrame, Color.Lerp(accent, Color.White, 0.18f) * opacity, 2);
            DrawItemGlow(entry.ItemType, iconFrame.Center.ToVector2(), iconBoxSize * 0.7f, accent, opacity);
            DrawItemIcon(entry.ItemType, iconFrame.Center.ToVector2(), iconBoxSize * 0.61f, Color.White, opacity);

            DrawRectangle(new Rectangle(textArea.X, textArea.Y - 9, textArea.Width, 2), accent * (opacity * 0.72f));
            DrawWrappedParagraph(GetDescription(entry), textArea, new Color(220, 239, 232), 0.64f, opacity);

            string indexText = Language.GetTextValue(
                "Mods.AccessoryForPlanetaryEmpire.TheItemCatalog.EntryIndex",
                entryIndex + 1,
                categoryCount,
                GetSubcategoryLabel(entry.ModItem));
            DrawCenteredFitText(indexText, indexArea, new Color(114, 190, 166), 0.46f, 0.3f, opacity);
        }

        private static void DrawFooter(Rectangle panelArea, float opacity)
        {
            Rectangle hintArea = new(panelArea.X + 28, panelArea.Bottom - 34, panelArea.Width - 56, 18);
            DrawCenteredFitText(GetText("Controls"), hintArea, new Color(104, 176, 156), 0.44f, 0.3f, opacity);
        }

        private static string GetDescription(EquipmentCatalogEntry entry)
        {
            string tooltip = entry.ModItem.Tooltip?.Value;
            if (string.IsNullOrWhiteSpace(tooltip) || tooltip == entry.ModItem.Tooltip.Key)
            {
                return GetText("MissingDescription");
            }

            return CollapseWhitespace(StripChatTags(tooltip));
        }

        private static string GetSubcategoryLabel(ModItem modItem)
        {
            string subcategory = GetSubcategory(modItem);
            if (string.IsNullOrWhiteSpace(subcategory))
            {
                return GetCategoryName(GetCategory(modItem));
            }

            string leaf = subcategory.Split('.').Last();
            string key = $"Mods.AccessoryForPlanetaryEmpire.TheItemCatalog.Subcategories.{leaf}";
            string localized = Language.GetTextValue(key);
            return localized == key ? leaf : localized;
        }

        private static string GetCategoryName(CatalogCategory category)
        {
            return GetText($"Categories.{category}");
        }

        private static string GetText(string suffix)
        {
            return Language.GetTextValue($"Mods.AccessoryForPlanetaryEmpire.TheItemCatalog.{suffix}");
        }

        private static Color GetCategoryColor(CatalogCategory category)
        {
            return category switch
            {
                CatalogCategory.Attack => new Color(255, 94, 76),
                CatalogCategory.Defense => new Color(90, 176, 255),
                CatalogCategory.Movement => new Color(72, 234, 255),
                CatalogCategory.Dash => new Color(255, 220, 92),
                CatalogCategory.Job => new Color(196, 120, 255),
                CatalogCategory.Wings => new Color(114, 255, 160),
                _ => new Color(180, 202, 210)
            };
        }

        private static void DrawItemGlow(int itemType, Vector2 center, float maxSize, Color accent, float opacity)
        {
            Texture2D texture = TextureAssets.Item[itemType].Value;
            Rectangle source = texture.Frame();
            Vector2 sourceSize = source.Size();
            float fitScale = Math.Min(maxSize / Math.Max(1f, sourceSize.X), maxSize / Math.Max(1f, sourceSize.Y));
            float pulse = 0.72f + 0.28f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f + itemType);

            for (int i = 0; i < 8; i++)
            {
                Vector2 offset = (MathHelper.TwoPi * i / 8f).ToRotationVector2() * (5f + pulse * 3f);
                Main.EntitySpriteDraw(texture, center + offset, source, accent * (opacity * 0.16f), 0f, sourceSize * 0.5f, fitScale * (1f + pulse * 0.04f), SpriteEffects.None, 0f);
            }
        }

        private static void DrawItemIcon(int itemType, Vector2 center, float maxSize, Color color, float opacity)
        {
            Texture2D texture = TextureAssets.Item[itemType].Value;
            Rectangle source = texture.Frame();
            Vector2 sourceSize = source.Size();
            float fitScale = Math.Min(maxSize / Math.Max(1f, sourceSize.X), maxSize / Math.Max(1f, sourceSize.Y));
            float hover = 1f + 0.025f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f + itemType * 0.2f);
            Main.EntitySpriteDraw(texture, center, source, color * opacity, 0f, sourceSize * 0.5f, fitScale * hover, SpriteEffects.None, 0f);
        }

        private static Rectangle ScaleRectangle(Rectangle rectangle, Vector2 center, float scale)
        {
            int width = Math.Max(1, (int)(rectangle.Width * scale));
            int height = Math.Max(1, (int)(rectangle.Height * scale));
            return new Rectangle((int)(center.X - width * 0.5f), (int)(center.Y - height * 0.5f), width, height);
        }

        private static void DrawCornerBrackets(Rectangle area, Color color)
        {
            const int length = 17;
            const int thickness = 2;

            DrawRectangle(new Rectangle(area.X + 7, area.Y + 7, length, thickness), color);
            DrawRectangle(new Rectangle(area.X + 7, area.Y + 7, thickness, length), color);
            DrawRectangle(new Rectangle(area.Right - length - 7, area.Y + 7, length, thickness), color);
            DrawRectangle(new Rectangle(area.Right - thickness - 7, area.Y + 7, thickness, length), color);
            DrawRectangle(new Rectangle(area.X + 7, area.Bottom - thickness - 7, length, thickness), color);
            DrawRectangle(new Rectangle(area.X + 7, area.Bottom - length - 7, thickness, length), color);
            DrawRectangle(new Rectangle(area.Right - length - 7, area.Bottom - thickness - 7, length, thickness), color);
            DrawRectangle(new Rectangle(area.Right - thickness - 7, area.Bottom - length - 7, thickness, length), color);
        }

        private static void DrawChevron(Vector2 center, bool pointsRight, Color color, int thickness)
        {
            float direction = pointsRight ? 1f : -1f;
            Vector2 upperStart = center + new Vector2(-direction * 10f, -14f);
            Vector2 lowerStart = center + new Vector2(-direction * 10f, 14f);
            Vector2 tip = center + new Vector2(direction * 10f, 0f);
            DrawLine(upperStart, tip, color, thickness);
            DrawLine(lowerStart, tip, color, thickness);
        }

        private static void DrawLine(Vector2 start, Vector2 end, Color color, int thickness)
        {
            Vector2 edge = end - start;
            Main.spriteBatch.Draw(
                TextureAssets.MagicPixel.Value,
                start,
                null,
                color,
                edge.ToRotation(),
                new Vector2(0f, 0.5f),
                new Vector2(edge.Length(), thickness),
                SpriteEffects.None,
                0f);
        }

        private static void DrawWrappedParagraph(string text, Rectangle area, Color color, float scale, float opacity)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            List<string> lines = WrapText(text, area.Width, scale);
            int maxLines = Math.Max(1, (int)(area.Height / Math.Max(1f, font.LineSpacing * scale)));

            if (lines.Count > maxLines)
            {
                lines.RemoveRange(maxLines, lines.Count - maxLines);
                lines[^1] = TrimTextToFit(lines[^1], area.Width, scale, true);
            }

            float lineHeight = font.LineSpacing * scale;
            for (int i = 0; i < lines.Count; i++)
            {
                DrawTextWithShadow(lines[i], new Vector2(area.X, area.Y + i * lineHeight), color * opacity, scale);
            }
        }

        private static List<string> WrapText(string text, int width, float scale)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            List<string> lines = new();
            string currentLine = string.Empty;

            foreach (char character in text)
            {
                string candidate = currentLine + character;
                if (font.MeasureString(candidate).X * scale <= width)
                {
                    currentLine = candidate;
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(currentLine))
                {
                    lines.Add(currentLine.Trim());
                }

                currentLine = char.IsWhiteSpace(character) ? string.Empty : character.ToString();
            }

            if (!string.IsNullOrWhiteSpace(currentLine))
            {
                lines.Add(currentLine.Trim());
            }

            return lines;
        }

        private static string TrimTextToFit(string text, int width, float scale, bool forceSuffix)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            const string suffix = "...";

            if (!forceSuffix && font.MeasureString(text).X * scale <= width)
            {
                return text;
            }

            string trimmed = text;
            while (trimmed.Length > 0 && font.MeasureString(trimmed + suffix).X * scale > width)
            {
                trimmed = trimmed[..^1];
            }

            return trimmed.TrimEnd() + suffix;
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

            if (size.Y * scale > area.Height)
            {
                scale = Math.Min(scale, area.Height / size.Y);
            }

            scale = MathHelper.Clamp(scale, minScale, maxScale);
            Vector2 position = new(area.X, area.Y + Math.Max(0f, (area.Height - size.Y * scale) * 0.5f));
            DrawTextWithShadow(text, position, color * opacity, scale);
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
            DrawTextWithShadow(text, position, color * opacity, scale);
        }

        private static void DrawTextWithShadow(string text, Vector2 position, Color color, float scale)
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

        private static string StripChatTags(string text)
        {
            string output = text;
            int tagStart;

            while ((tagStart = output.IndexOf("[c/", StringComparison.Ordinal)) >= 0)
            {
                int colon = output.IndexOf(':', tagStart);
                int close = colon >= 0 ? output.IndexOf(']', colon) : -1;
                if (colon < 0 || close < 0)
                {
                    break;
                }

                string inner = output[(colon + 1)..close];
                output = output[..tagStart] + inner + output[(close + 1)..];
            }

            while ((tagStart = output.IndexOf("[i:", StringComparison.Ordinal)) >= 0)
            {
                int close = output.IndexOf(']', tagStart);
                if (close < 0)
                {
                    break;
                }

                output = output.Remove(tagStart, close - tagStart + 1);
            }

            return output;
        }

        private static string CollapseWhitespace(string text)
        {
            return string.Join(" ", text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries));
        }

        private static void DrawRectangle(Rectangle rectangle, Color color)
        {
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, rectangle, color);
        }

        private static void DrawBorder(Rectangle rectangle, Color color, int thickness)
        {
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            DrawRectangle(new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
        }
    }

    internal enum CatalogCategory
    {
        Attack,
        Defense,
        Movement,
        Dash,
        Job,
        Wings,
        Utility
    }

    internal sealed class EquipmentCatalogEntry
    {
        public EquipmentCatalogEntry(ModItem modItem, CatalogCategory category)
        {
            ModItem = modItem;
            Category = category;
        }

        public ModItem ModItem { get; }
        public CatalogCategory Category { get; }
        public int ItemType => ModItem.Type;
    }
}
