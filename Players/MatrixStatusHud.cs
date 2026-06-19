using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace AccessoryForPlanetaryEmpire.Players
{
    public readonly struct MatrixStatusBar
    {
        public MatrixStatusBar(string label, float current, float max, Color theme, string state)
        {
            Label = label;
            Current = current;
            Max = Math.Max(1f, max);
            Theme = theme;
            State = state;
        }

        public string Label { get; }
        public float Current { get; }
        public float Max { get; }
        public Color Theme { get; }
        public string State { get; }
        public float Fill => MathHelper.Clamp(Current / Max, 0f, 1f);
    }

    public static class MatrixStatusHud
    {
        private const int BarWidth = 116;
        private const int BarHeight = 22;
        private const int GapX = 8;
        private const int GapY = 5;

        private static readonly string[] Glyphs =
        {
            "0", "1", "01", "10", "PLA", "SYS", "RUN", "EMP", "::", "[]"
        };

        public static void DrawBars(Player player, IReadOnlyList<MatrixStatusBar> bars)
        {
            if (bars.Count == 0)
            {
                return;
            }

            int columns = bars.Count <= 3 ? 1 : bars.Count <= 10 ? 2 : 3;
            int rows = (bars.Count + columns - 1) / columns;
            float gridWidth = columns * BarWidth + (columns - 1) * GapX;
            float gridHeight = rows * BarHeight + (rows - 1) * GapY;
            Vector2 anchor = player.Top - Main.screenPosition + new Vector2(0f, player.gfxOffY - 42f);
            Vector2 topLeft = new(anchor.X - gridWidth * 0.5f, anchor.Y - gridHeight);

            topLeft.X = MathHelper.Clamp(topLeft.X, 8f, Math.Max(8f, Main.screenWidth - gridWidth - 8f));
            topLeft.Y = MathHelper.Clamp(topLeft.Y, 8f, Math.Max(8f, Main.screenHeight - gridHeight - 8f));

            for (int i = 0; i < bars.Count; i++)
            {
                int column = i % columns;
                int row = i / columns;
                Rectangle area = new(
                    (int)(topLeft.X + column * (BarWidth + GapX)),
                    (int)(topLeft.Y + row * (BarHeight + GapY)),
                    BarWidth,
                    BarHeight);

                DrawSingleBar(area, bars[i], i);
            }
        }

        private static void DrawSingleBar(Rectangle area, MatrixStatusBar bar, int index)
        {
            float time = Main.GlobalTimeWrappedHourly;
            float pulse = 0.72f + 0.28f * (float)Math.Sin(time * 6.2f + index * 0.73f);
            Color theme = bar.Theme;
            Color border = Color.Lerp(theme, Color.White, 0.16f + pulse * 0.16f);
            Rectangle track = new(area.X + 8, area.Y + 13, area.Width - 16, 5);
            int fillWidth = (int)(track.Width * bar.Fill);

            DrawRectangle(area, new Color(2, 8, 10, 210));
            DrawRectangle(new Rectangle(area.X + 1, area.Y + 1, area.Width - 2, area.Height - 2), Color.Lerp(new Color(4, 18, 18), theme, 0.08f) * 0.82f);
            DrawMatrixNoise(area, theme, index);
            DrawBorder(area, border * (0.68f + pulse * 0.22f), 1);
            DrawCornerBrackets(area, border * (0.88f + pulse * 0.12f));

            DrawRectangle(track, new Color(0, 0, 0, 166));
            if (fillWidth > 0)
            {
                Rectangle fill = new(track.X, track.Y, fillWidth, track.Height);
                DrawRectangle(fill, Color.Lerp(theme, Color.White, 0.18f) * 0.86f);
                DrawRectangle(new Rectangle(fill.Right - 1, track.Y - 1, 2, track.Height + 2), Color.White * 0.7f);
            }

            DrawBorder(track, theme * 0.58f, 1);

            Rectangle labelArea = new(area.X + 7, area.Y + 2, area.Width - 43, 10);
            Rectangle stateArea = new(area.Right - 38, area.Y + 2, 33, 10);
            DrawFitText(bar.Label, labelArea, Color.Lerp(new Color(180, 236, 224), Color.White, pulse * 0.18f), 0.36f, 0.22f);
            DrawCenteredFitText(bar.State, stateArea, theme, 0.33f, 0.2f);

            int scanX = area.X + 3 + (int)((time * 34f + index * 17f) % Math.Max(1, area.Width - 6));
            DrawRectangle(new Rectangle(scanX, area.Y + 3, 1, area.Height - 6), theme * 0.18f);
        }

        private static void DrawMatrixNoise(Rectangle area, Color theme, int index)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            float time = Main.GlobalTimeWrappedHourly;
            int columns = 4;

            for (int i = 0; i < columns; i++)
            {
                int glyphIndex = ((int)(time * 8f) + index * 5 + i * 3) % Glyphs.Length;
                float x = area.X + 10 + i * 26;
                float y = area.Y + 3 + (time * (10f + i * 1.7f) + index * 9 + i * 12) % 12f;
                ChatManager.DrawColorCodedString(
                    Main.spriteBatch,
                    font,
                    Glyphs[glyphIndex],
                    new Vector2(x, y),
                    theme * 0.16f,
                    0f,
                    Vector2.Zero,
                    Vector2.One * 0.29f);
            }
        }

        private static void DrawCornerBrackets(Rectangle area, Color color)
        {
            const int length = 8;
            DrawRectangle(new Rectangle(area.X, area.Y, length, 1), color);
            DrawRectangle(new Rectangle(area.X, area.Y, 1, length), color);
            DrawRectangle(new Rectangle(area.Right - length, area.Y, length, 1), color);
            DrawRectangle(new Rectangle(area.Right - 1, area.Y, 1, length), color);
            DrawRectangle(new Rectangle(area.X, area.Bottom - 1, length, 1), color * 0.72f);
            DrawRectangle(new Rectangle(area.X, area.Bottom - length, 1, length), color * 0.72f);
            DrawRectangle(new Rectangle(area.Right - length, area.Bottom - 1, length, 1), color * 0.72f);
            DrawRectangle(new Rectangle(area.Right - 1, area.Bottom - length, 1, length), color * 0.72f);
        }

        private static void DrawFitText(string text, Rectangle area, Color color, float maxScale, float minScale)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 size = font.MeasureString(text);
            if (size.X <= 0f || size.Y <= 0f)
            {
                return;
            }

            float scale = MathHelper.Clamp(Math.Min(maxScale, area.Width / size.X), minScale, maxScale);
            Vector2 position = new(area.X, area.Y + Math.Max(0f, (area.Height - size.Y * scale) * 0.5f));
            DrawText(text, position, color, scale);
        }

        private static void DrawCenteredFitText(string text, Rectangle area, Color color, float maxScale, float minScale)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 size = font.MeasureString(text);
            if (size.X <= 0f || size.Y <= 0f)
            {
                return;
            }

            float scale = MathHelper.Clamp(Math.Min(maxScale, area.Width / size.X), minScale, maxScale);
            Vector2 position = new(
                area.X + Math.Max(0f, (area.Width - size.X * scale) * 0.5f),
                area.Y + Math.Max(0f, (area.Height - size.Y * scale) * 0.5f));
            DrawText(text, position, color, scale);
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
