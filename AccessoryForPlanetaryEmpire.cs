using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire 
{
    public class AccessoryForPlanetaryEmpire : Mod
    {
        public static object Instance { get; internal set; }
        //public static int ImperialArmoredAircraftNPC { get; internal set; }
        //public static ModHotKey UltimateFunctionOverloadDeviceTrigger { get; internal set; }
        //public static List<Texture2D> HuskyDashEffects { get; private set; }



        // 这段是顶点绘制的核心
        /*
         public static void ProjectileDrawTail(Projectile Projectile, Texture2D Tail, Vector2 DrawOrigin, Color TailColor1, Color TailColor2, float Width, bool Lerp)
        {
            Vector2 drawOrigin = DrawOrigin;
            List<CustomVertexInfo> bars = new List<CustomVertexInfo>();
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) break;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                float scale = Projectile.scale * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                float width = Width * scale;
                if (Lerp)
                {
                    width = Width * scale*(1-scale)*scale;
                }
                var factor = i / (float)Projectile.oldPos.Length;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                var color = Color.Lerp(TailColor1, TailColor2, factor);
                color.A = 0;
                Vector2 offset = Vector2.Zero;
                bars.Add(new CustomVertexInfo(Projectile.oldPos[i] + normalDir * width + drawOrigin - Main.screenPosition + offset, color, new Vector3(factor, 1, w)));
                bars.Add(new CustomVertexInfo(Projectile.oldPos[i] + normalDir * -width + drawOrigin - Main.screenPosition + offset, color, new Vector3(factor, 0, w)));
            }
            List<CustomVertexInfo> Vx = new List<CustomVertexInfo>();
            if (bars.Count > 2)
            {
                Vx.Add(bars[0]);
                Vx.Add(bars[1]);
                Vx.Add(bars[2]);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    Vx.Add(bars[i]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 1]);

                    Vx.Add(bars[i + 1]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 3]);
                }
            }
            Main.graphics.GraphicsDevice.Textures[0] = Tail;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
        }
         
         */


        public override void Load()
        {
            ////ConfigManager.Load();
            //HuskyDashEffects = new List<Texture2D>(); 
            //for (int i = 0; i < 21; i++)
            //{
            //    HuskyDashEffects.Add(ModContent.Request<Texture2D>($"AccessoryForPlanetaryEmpire/Players/Movement/HuskyDashGIF ({i})").Value);
            //}


            //if (ModLoader.HasMod("CalamityMod"))
            //{
            //    Mod calamityMod = ModLoader.GetMod("CalamityMod");
            //    if (calamityMod != null)
            //    {
            //        // 这里你可以添加你的联动代码
            //        // 例如：检测玩家是否为Rogue职业
            //    }
            //}

        }



        public override void Unload()
        {
            //ConfigManager.Unload();
            //HuskyDashEffects = null; // Don't forget to clean up in Unload
        }

    }
    public class PlanetaryPlayer : ModPlayer
    {
        internal bool usedUltimateOverload;
        internal int overloadDamageTime;
        internal float allDamageMult;
        internal float allDamage;
        internal int dodgeCooldownTime;
        internal int reactionShieldCooldown;
        internal bool reactionShieldActive;
        internal int timer;
        internal bool gravityReduce;
        internal int reflectCooldownTime;
        internal bool disableProjectiles;
        internal float rangedDamageMult;
        internal float meleeDamageMult;
        internal float meleeSpeedMult;
        internal float magicDamageMult;
        internal int pressureMeter;
        internal int RAPTimer;
        internal bool disableProjectile;
        internal int infernoTimer;
        internal bool reactShieldActive;
        internal int reactShieldBuffTimer;
        internal int attackTimerMax;
        internal bool UFODSkillActive;
        internal int UFODSkillTime;
        internal bool UFODSkillEffectsApplied;
        internal bool MBPSkillActive;
        internal bool MBPSkillEffectsApplied;
        internal bool RAPHSkillActive;
        internal bool RAPHSkillEffectsApplied;
        internal bool RSGSkillActive;
        internal bool RSGSkillEffectsApplied;
        internal int RSGSkillTime;
        internal bool RSGCooldown;
        internal int RSGCooldownTime;
        internal int invulnerabilityCooldown;
        internal int FYHDimmuneCooldownTimer;
        internal int RAPHSkillTime;
        internal int PBBHSkillCooldown;
        internal bool PBBHSkillActive;
        internal int PBBHSkillTime;
        internal bool PBBHSkillEffectsApplied;
        internal int MSSkillCooldown;
        internal bool MSSkillActive;
        internal int MSSkillTime;
        internal bool MSSkillEffectsApplied;
        //internal int TSkillCooldown;
        internal bool TSkillActive;
        internal int TSkillTime;
        internal bool TSkillEffectsApplied;
        //internal int RBSkillCooldown;
        internal bool RBSkillActive;
        internal int RBSkillTime;
        internal bool RBSkillEffectsApplied;
        internal bool SEMPSkillActive;
        internal int SEMPSkillTime;
        internal bool SEMPSkillEffectsApplied; 
        internal bool BBSkillActive;
        internal int BBSkillTime;
        internal bool BBSkillEffectsApplied;
        internal bool PlayerRevived;
        public bool HasPainEditor; // 疼痛编辑器: 限制每次受到伤害在最大生命值的20%

        public int UFODSkillCooldown { get; set; }

        public bool dodgeCooldown { get; internal set; }
        public bool UltraHighCrit { get; internal set; }
        public bool HeroDamageDevice { get; internal set; }
        public bool HasHeroDamageDevice { get; internal set; }
        public int MBPSkillCooldown { get; internal set; }
        public int RAPHSkillCooldown { get; internal set; }
        public int RSGHSkillCooldown { get; internal set; }
        public int TSkillCooldown { get; internal set; }
        public int RBSkillCooldown { get; internal set; }
        public int SEMPSkillCooldown { get; internal set; }
        public int BBSkillCooldown { get; internal set; }
        public int MagicSeekerCooldown { get; internal set; }

    }
}