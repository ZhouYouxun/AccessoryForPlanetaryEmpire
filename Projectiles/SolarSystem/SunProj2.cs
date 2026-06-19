using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    // 太阳主题的治疗弹：优先追敌→无敌时按“直线-转弯”循环飞行；
    // 存活满 180 帧后若仍未消失，转为追踪玩家，触碰回血并自毁；
    // PreDraw 橙色脉动环，AI 中橙色尘粒；死亡时正前方扇形 Dust 扩散。
    internal class SunProj2 : ModProjectile
    {
        // 使用 Extra_89 作为光环底图
        public override string Texture => "Terraria/Images/Extra_89";

        // ——可调参数（数学节奏）——
        private const int HealAgeThreshold = 180;   // 达到此生存帧后转为追踪玩家
        private const float BaseSpeed = 16f;        // 正常飞行速度
        private const float HomingSpeed = 17.5f;    // 跟踪时速度
        private const float HomingTurnRate = 0.12f; // 跟踪时的转向插值
        private const int StraightTicks = 18;       // 直线阶段持续帧数
        private const int CurveTicks = 14;          // 转弯阶段持续帧数
       
        // ——直线→转圈 逻辑所需（每发独立，不用 localAI）——
        private bool _moveInited;
        private int _movePhase;          // 0=直线, 1=转圈
        private int _moveTimer;          // 当前阶段计时
        private int _straightTicksCur;   // 本轮直线时长
        private int _curveTicksCur;      // 本轮转圈时长（按一圈±35%换算成tick）
        private float _turnSign;          // +1=左转 / -1=右转
        private float _turnRadPerTick;    // 每帧转角(弧度) = CurveTurnDeg
        
        // === 常量区（放在 SunProj2 顶部和其他 const 一起）===
        private const float CurveTurnDeg = 2.4f; // 每帧转角（度）。2.4°/帧≈150帧转一圈


        // ——内部状态（不用 localAI）——
        private int _age;               // 生存帧
        private int _phaseTimer;        // 当前阶段计时
        private bool _curveRight = true;// 转向方向（右/左）交替
        private bool _healingPhase;     // 是否进入“追踪玩家回血”阶段

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;           // 最大生存时间
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic; // 或 Generic 均可
            Projectile.extraUpdates = 1;
        }
        public override void SetStaticDefaults()
        {
            // 记录最近 N 帧的位置到 oldPos[]；模式 0 = 按时间衰减
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14; // 可改 8~20
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            // ——同款贴图（与你本体一致：Extra_89）——
            Texture2D ringTex = Terraria.GameContent.TextureAssets.Extra[89].Value;
            Vector2 origin = ringTex.Size() * 0.5f;

            // ===== 先画拖尾（按历史朝向 oldRot 旋转）=====
            int len = Projectile.oldPos.Length;
            for (int i = len - 1; i >= 0; i--) // 先画更老的，层次更自然
            {
                // oldPos 是“左上角”，+半尺寸转成“中心点”
                Vector2 tailPos = Projectile.oldPos[i] + Projectile.Size * 0.5f
                                - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

                // 历史朝向：优先用 oldRot，缺失时用当前 rotation 兜底
                float rot = (i < Projectile.oldRot.Length && Projectile.oldRot[i] != 0f)
                    ? Projectile.oldRot[i]
                    : Projectile.rotation;

                // 太阳暖橙（不刺眼）
                float t = (i + 1f) / (float)len;                    // 旧→新：0..1
                float fade = MathHelper.Lerp(0.05f, 0.55f, t);      // 越新越亮
                Color tailCol = new Color(255, 180, 60, 0) * (fade * 0.9f);

                // 轻微呼吸
                float pulse = 1f + 0.04f * (float)Math.Sin((Main.GlobalTimeWrappedHourly + i * 0.05f) * 6f);

                // 沿飞行方向轻拉伸，方向感更强（本体仍不旋转）
                float s = MathHelper.Lerp(0.45f, 0.95f, t);
                Vector2 tailScale = new Vector2(s * 1.10f, s * 0.90f) * pulse;

                sb.Draw(ringTex, tailPos, null, tailCol, rot, origin, tailScale, SpriteEffects.None, 0f);
            }

            // ===== 再画本体（保持你原有的“四层脉动环”，不旋转）=====
            Vector2 drawPos = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
            float pulseMain = 1f + 0.08f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 5f);
            Color baseCol = new Color(255, 180, 60, 0);

            for (int i = 0; i < 4; i++)
            {
                float angle = MathHelper.TwoPi * i / 4f + Main.GlobalTimeWrappedHourly * MathHelper.TwoPi * 0.55f;
                float scale = (0.28f + 0.06f * i) * pulseMain * 2.6f;
                Color c = baseCol * (1.4f - i * 0.22f);
                Main.EntitySpriteDraw(ringTex, drawPos, null, c, angle, origin, scale, SpriteEffects.None, 0);
            }

            return false; // 手动绘制，屏蔽默认
        }


        // ——巡航随机化参数（每发不同）——
        private bool _seedInited;
        private int _behaviorId;      // 0/1/2 三种巡航分支，确保每发不同
        private float _randPhase;      // 正弦相位偏移
        private float _randAmpDeg;     // 正弦振幅（度）
        private float _randConstDeg;   // 常量转弯基础角（度）
        private float _randJitter;     // 微扰强度

        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {
            // 每发独立随机，使同批弹幕不同步
            if (_seedInited) return;
            _seedInited = true;

            // 三种分支：均参考 RevelationSpark 的“独立随机偏转 + 窗口式旋转/正弦扰动”的思想
            _behaviorId = Main.rand.Next(0, 3);                 // 0 / 1 / 2
            _randPhase = Main.rand.NextFloat(0f, MathHelper.TwoPi);
            _randAmpDeg = Main.rand.NextFloat(0.8f, 2.2f);      // 正弦振幅（度）
            _randConstDeg = Main.rand.NextFloat(1.6f, 2.8f);    // 常量转弯（度/帧）
            _randJitter = Main.rand.NextFloat(0.6f, 1.0f);      // 轻微随机幅度系数

            // ⭐ 出生瞬间降速到5%
            if (Projectile.velocity.LengthSquared() > 1e-4f)
                Projectile.velocity *= 0.05f; // 一开始将速度重置到5%
        }

        // 指数爬升：从 0.05 → 1.0（随时间指数增长）
        // mu=0 → 0.05；mu=1 → 1.0；extraUpdates 会自动放大持续时间
        private float GetSpawnRampMultiplier()
        {
            int rampFrames = 36 * (Projectile.extraUpdates + 1); // 耗时36爬到最顶端速度
            float mu = MathHelper.Clamp(_age / (float)rampFrames, 0f, 1f);
            // 0.05^(1-mu)：mu=0 → 0.05；mu=1 → 1
            return (float)Math.Pow(0.05f, 1f - mu);
        }

        // 便捷：给任意基准速度套上出生爬升系数
        private float CurrentSpeedTarget(float baseSpeed) => baseSpeed * GetSpawnRampMultiplier();

        public override void AI()
        {
            EnsureMoveInit();   // ← 必须在最前，保证直线/转圈参数就绪
            _age++;

            Player owner = Main.player[Projectile.owner];

            // ——阶段切换：生存满 HealAgeThreshold 后，改为追踪玩家回血——
            int ageThresholdFixed = HealAgeThreshold * (Projectile.extraUpdates + 1);
            if (!_healingPhase && _age >= ageThresholdFixed)
                _healingPhase = true;


            // 放在 AI() 里，每帧都执行
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.PiOver4;


            if (_healingPhase)
            {
                // 追踪玩家（用于回血）
                Vector2 want = (owner.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, want * HomingSpeed, 0.18f);
                EmitSolarSpiralDust(Projectile.Center, owner.Center - Projectile.Center);
                TryHealOwner(owner);
                return;
            }

            //// ——优先追敌：若有敌人则温和自导——
            //NPC target = FindClosestNPC(Projectile.Center, 90000); // 寻敌范围
            //if (target != null && target.active)
            //{
            //    Vector2 want = (target.Center - Projectile.Center).SafeNormalize(Projectile.velocity.SafeNormalize(Vector2.UnitX));
            //    // 控制转向“数学美感”：角度差限制 + 速度保持
            //    float turn = MathHelper.ToRadians(5.0f);
            //    float angA = Projectile.velocity.ToRotation();
            //    float angB = want.ToRotation();
            //    float angDelta = MathHelper.WrapAngle(angB - angA);
            //    angDelta = MathHelper.Clamp(angDelta, -turn, turn);

            //    float speed = Projectile.velocity.Length();
            //    float lerp = HomingTurnRate;
            //    Vector2 newVel = Vector2.Lerp(Projectile.velocity, (angA + angDelta).ToRotationVector2() * speed, lerp);
            //    Projectile.velocity = newVel.SafeNormalize(Vector2.UnitX) * MathHelper.Lerp(speed, HomingSpeed, 0.08f);

            //    EmitSolarSpiralDust(Projectile.Center, Projectile.velocity);
            //}
            //else
            {
                // ——无敌：按“直线→持续转弯→直线→持续转弯”循环飞行——
                PatternedCruise();
                EmitSolarSpiralDust(Projectile.Center, Projectile.velocity);
            }
        }

        private void EnsureMoveInit()
        {
            if (_moveInited) return;
            _moveInited = true;
            _turnRadPerTick = MathHelper.ToRadians(CurveTurnDeg); // 用你已有常量
            StartStraightPhase(); // 先从直线开始
        }

        // 随机一段直线时长（以 StraightTicks 为基准轻微浮动）
        private void StartStraightPhase()
        {
            _movePhase = 0;
            _moveTimer = 0;
            _straightTicksCur = Main.rand.Next(Math.Max(6, StraightTicks - 4), StraightTicks + 5);
        }

        // 开始转圈：随机方向，目标角度=一整圈±35%，用每帧转角换算出tick数
        private void StartCurvePhase()
        {
            _movePhase = 1;
            _moveTimer = 0;

            _turnSign = Main.rand.NextBool() ? 1f : -1f;           // 左/右随机
            float turns = Main.rand.NextFloat(0.65f, 1.35f);       // 0.65~1.35 圈
            float totalAngle = MathHelper.TwoPi * turns;           // 目标总角度
            _curveTicksCur = Math.Max(1, (int)Math.Round(totalAngle / _turnRadPerTick));
        }




        // 直线→转圈→直线→转圈 循环：
        // 直线：匀速前进 _straightTicksCur 帧
        // 转圈：以固定每帧转角 _turnRadPerTick 持续左(或右)转 _curveTicksCur 帧
        //      其中 _curveTicksCur 由 “一圈±35%” 的目标角度换算而来
        private void PatternedCruise()
        {
            float speed = BaseSpeed;

            if (_movePhase == 0)
            {
                // ——直线阶段：仅保持速度模长
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * speed;

                _moveTimer++;
                if (_moveTimer >= _straightTicksCur)
                {
                    // 切换到转圈阶段
                    StartCurvePhase();
                }
            }
            else // _movePhase == 1
            {
                // ——转圈阶段：每帧固定小角度旋转（方向固定），形成“近似圆弧”
                Projectile.velocity = Projectile.velocity.RotatedBy(_turnRadPerTick * _turnSign);
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * speed;

                _moveTimer++;
                if (_moveTimer >= _curveTicksCur)
                {
                    // 弧度达到目标（可能 <1 圈或 >1 圈），回到直线并重新随机时长与方向
                    StartStraightPhase();
                }
            }
        }



        // 橙色日耀尘粒（Fermat 螺线 + 相位扰动）：优雅且不刺眼
        private void EmitSolarSpiralDust(Vector2 center, Vector2 forward)
        {
            //// 以黄金角近似：角度相位 + 半径随时间与相位缓变
            //float t = (float)Main.GameUpdateCount * 0.12f;
            //int count = 3; // 每帧少量，避免噪点
            //for (int i = 0; i < count; i++)
            //{
            //    float k = t + i * 2.39996323f; // 约等于黄金角
            //    float r = 4f + 2.6f * (float)Math.Sin(k * 0.7f);
            //    Vector2 dir = (forward.LengthSquared() > 1e-4f ? forward : Vector2.UnitX).SafeNormalize(Vector2.UnitX);
            //    Vector2 radial = (k).ToRotationVector2();
            //    Vector2 pos = center + radial * r + dir.RotatedBy(MathHelper.PiOver2) * (float)Math.Sin(k) * 1.5f;

            //    Dust d = Dust.NewDustPerfect(pos, DustID.SolarFlare, Vector2.Zero, 160, new Color(255, 200, 80), 0.95f);
            //    d.noGravity = true;                 // ✅ 不受重力
            //    d.fadeIn = 1.0f + Main.rand.NextFloat(0.2f);
            //    d.velocity = radial * 0.6f + dir * 0.4f;
            //}
        }

        private void TryHealOwner(Player owner)
        {
            // 触碰回血（getGoodWorld 35 / 否则 10），并自杀
            if (Projectile.Hitbox.Intersects(owner.Hitbox))
            {
                int healAmount = Main.getGoodWorld ? 35 : 10;
                owner.statLife += healAmount;
                owner.HealEffect(healAmount, true);
                Projectile.Kill();
            }
        }

        private NPC FindClosestNPC(Vector2 from, float maxDist)
        {
            NPC best = null;
            float bestD = maxDist;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (!n.active) continue;
                if (n.friendly || n.dontTakeDamage) continue; // 排除友好/不可受伤
                if (!n.CanBeChasedBy(Projectile)) continue;   // 以本弹幕为攻击者的可追条件

                float d = Vector2.Distance(from, n.Center);
                if (d > bestD) continue;

                // 需要有视线（防止隔墙不追）
                if (!Collision.CanHitLine(from, 1, 1, n.Center, 1, 1))
                    continue;

                bestD = d;
                best = n;
            }
            return best;
        }

        public override void OnKill(int timeLeft)
        {
            // 朝向取当前速度方向，兜底为 +X
            Vector2 forward = Projectile.velocity.LengthSquared() > 1e-4f
                ? Vector2.Normalize(Projectile.velocity)
                : Vector2.UnitX;

            // 传入中心与朝向
            PLALightingBoltsSystem.PLA_SpawnSUNBurst(Projectile.Center, forward);

            // （若你还有 Dust 扇形/其他收尾，可以继续保留在此之后）
        }



    }
}
