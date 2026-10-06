using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.Graphics.Light;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria.GameContent.Shaders;

public class WaterShaderData : ScreenShaderData
{
	private struct Ripple
	{
		private static readonly Rectangle[] RIPPLE_SHAPE_SOURCE_RECTS = new Rectangle[3]
		{
			new Rectangle(0, 0, 0, 0),
			new Rectangle(1, 1, 62, 62),
			new Rectangle(1, 65, 62, 62)
		};

		public readonly Vector2 Position;

		public readonly Color WaveData;

		public readonly Vector2 Size;

		public readonly RippleShape Shape;

		public readonly float Rotation;

		public Rectangle SourceRectangle
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return RIPPLE_SHAPE_SOURCE_RECTS[(int)Shape];
			}
		}

		public Ripple(Vector2 position, Color waveData, Vector2 size, RippleShape shape, float rotation)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Position = position;
			WaveData = waveData;
			Size = size;
			Shape = shape;
			Rotation = rotation;
		}

		static Ripple()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		}
	}

	private const float DISTORTION_BUFFER_SCALE = 0.25f;

	private const float WAVE_FRAMERATE = 1f / 60f;

	private const int MAX_RIPPLES_QUEUED = 200;

	public bool DrawRipples = true;

	public bool _useViscosityFilter = true;

	private RenderTarget2D _distortionTarget;

	private RenderTarget2D _distortionTargetSwap;

	private Texture2D _noDistortionTexture;

	private bool _usingRenderTargets;

	private Vector2 _lastDistortionDrawOffset = Vector2.Zero;

	private float _progress;

	private Ripple[] _rippleQueue = new Ripple[200];

	private int _rippleQueueCount;

	private int _lastScreenWidth;

	private int _lastScreenHeight;

	public bool _useProjectileWaves = true;

	private bool _useNPCWaves = true;

	private bool _usePlayerWaves = true;

	private bool _useRippleWaves = true;

	private bool _useCustomWaves = true;

	private bool _clearNextFrame = true;

	private Texture2D[] _viscosityMaskChain = new Texture2D[3];

	private int _activeViscosityMask;

	private Asset<Texture2D> _rippleShapeTexture;

	private bool _isWaveBufferDirty = true;

	private int _queuedSteps;

	private const int MAX_QUEUED_STEPS = 2;

	public event Action<TileBatch> OnWaveDraw;

	public WaterShaderData(string passName)
		: base(passName)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Main.OnRenderTargetsInitialized += InitRenderTargets;
		Main.OnRenderTargetsReleased += ReleaseRenderTargets;
		_rippleShapeTexture = Main.Assets.Request<Texture2D>("Images/Misc/Ripples", (AssetRequestMode)1);
		Main.OnPreDraw += PreDraw;
	}

	public override void Update(GameTime gameTime)
	{
		_useViscosityFilter = Main.WaveQuality >= 3;
		_useProjectileWaves = Main.WaveQuality >= 3;
		_usePlayerWaves = Main.WaveQuality >= 2;
		_useRippleWaves = Main.WaveQuality >= 2;
		_useCustomWaves = Main.WaveQuality >= 2;
		if (!FocusHelper.PauseLiquidRenderer)
		{
			_progress += (float)gameTime.ElapsedGameTime.TotalSeconds * Intensity * 0.75f;
			_progress %= 86400f;
			if (_useProjectileWaves || _useRippleWaves || _useCustomWaves || _usePlayerWaves)
			{
				_queuedSteps++;
			}
			base.Update(gameTime);
		}
	}

	private void StepLiquids()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		_isWaveBufferDirty = true;
		Vector2 val = (Vector2)(Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange));
		Vector2 val2 = val - Main.screenPosition;
		TileBatch tileBatch = Main.tileBatch;
		GraphicsDevice graphicsDevice = ((Game)Main.instance).GraphicsDevice;
		graphicsDevice.SetRenderTarget(_distortionTarget);
		if (_clearNextFrame)
		{
			graphicsDevice.Clear(new Color(0.5f, 0.5f, 0f, 1f));
			_clearNextFrame = false;
		}
		DrawWaves();
		graphicsDevice.SetRenderTarget(_distortionTargetSwap);
		graphicsDevice.Clear(new Color(0.5f, 0.5f, 0.5f, 1f));
		Main.tileBatch.Begin();
		val2 *= 0.25f;
		val2.X = (float)Math.Floor(val2.X);
		val2.Y = (float)Math.Floor(val2.Y);
		Vector2 val3 = val2 - _lastDistortionDrawOffset;
		_lastDistortionDrawOffset = val2;
		tileBatch.Draw((Texture2D)(object)_distortionTarget, new Vector4(val3.X, val3.Y, (float)((Texture2D)_distortionTarget).Width, (float)((Texture2D)_distortionTarget).Height), new VertexColors(Color.White));
		GameShaders.Misc["WaterProcessor"].Apply(new DrawData((Texture2D)(object)_distortionTarget, Vector2.Zero, Color.White));
		tileBatch.End();
		RenderTarget2D distortionTarget = _distortionTarget;
		_distortionTarget = _distortionTargetSwap;
		_distortionTargetSwap = distortionTarget;
		if (_useViscosityFilter)
		{
			LiquidRenderer.Instance.SetWaveMaskData(ref _viscosityMaskChain[_activeViscosityMask]);
			tileBatch.Begin();
			Rectangle cachedDrawArea = LiquidRenderer.Instance.GetCachedDrawArea();
			Rectangle val4 = new Rectangle(0, 0, cachedDrawArea.Height, cachedDrawArea.Width);
			Vector4 val5 = new Vector4((float)(cachedDrawArea.X + cachedDrawArea.Width), (float)cachedDrawArea.Y, (float)cachedDrawArea.Height, (float)cachedDrawArea.Width);
			val5 *= 16f;
			val5.X -= val.X;
			val5.Y -= val.Y;
			val5 *= 0.25f;
			val5.X += val2.X;
			val5.Y += val2.Y;
			graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			tileBatch.Draw(_viscosityMaskChain[_activeViscosityMask], val5, val4, new VertexColors(Color.White), val4.Size(), (SpriteEffects)1, -(float)Math.PI / 2f);
			tileBatch.End();
			_activeViscosityMask++;
			_activeViscosityMask %= _viscosityMaskChain.Length;
		}
		graphicsDevice.SetRenderTarget((RenderTarget2D)null);
	}

	private void DrawWaves()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0765: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Unknown result type (might be due to invalid IL or missing references)
		//IL_082d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		Vector2 screenPosition = Main.screenPosition;
		Vector2 val = (Vector2)(Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange));
		Vector2 val2 = -_lastDistortionDrawOffset / 0.25f + val;
		TileBatch tileBatch = Main.tileBatch;
		_ = ((Game)Main.instance).GraphicsDevice;
		Vector2 dimensions = new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
		Vector2 val3 = new Vector2(16f, 16f);
		tileBatch.Begin();
		GameShaders.Misc["WaterDistortionObject"].Apply();
		if (_useNPCWaves)
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i] == null || !Main.npc[i].active || (!Main.npc[i].wet && Main.npc[i].wetCount == 0) || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, Main.npc[i].position - val3, Main.npc[i].Size + val3))
				{
					continue;
				}
				NPC nPC = Main.npc[i];
				Vector2 val4 = nPC.Center - val2;
				Vector2 velocity = nPC.velocity;
				double radians = 0f - nPC.rotation;
				Vector2 center = default;
				Vector2 val5 = velocity.RotatedBy(radians, center) / new Vector2((float)nPC.height, (float)nPC.width);
				float num = val5.LengthSquared();
				num = num * 0.3f + 0.7f * num * (1024f / (float)(nPC.height * nPC.width));
				num = Math.Min(num, 0.08f);
				float num2 = num;
				center = nPC.velocity - nPC.oldVelocity;
				num = num2 + center.Length() * 0.5f;
				val5.Normalize();
				Vector2 velocity2 = nPC.velocity;
				velocity2.Normalize();
				val4 -= velocity2 * 10f;
				if (!_useViscosityFilter && (nPC.honeyWet || nPC.lavaWet))
				{
					num *= 0.3f;
				}
				if (nPC.wet)
				{
					tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(val4.X, val4.Y, (float)nPC.width * 2f, (float)nPC.height * 2f) * 0.25f, new VertexColors(new Color(val5.X * 0.5f + 0.5f, val5.Y * 0.5f + 0.5f, 0.5f * num)), new Vector2((float)TextureAssets.MagicPixel.Width() / 2f, (float)TextureAssets.MagicPixel.Height() / 2f), (SpriteEffects)0, nPC.rotation);
				}
				if (nPC.wetCount != 0)
				{
					num = nPC.velocity.Length();
					num = 0.195f * (float)Math.Sqrt(num);
					float num3 = 5f;
					if (!nPC.wet)
					{
						num3 = -20f;
					}
					QueueRipple(nPC.Center + velocity2 * num3, new Color(0.5f, (nPC.wet ? num : (0f - num)) * 0.5f + 0.5f, 0f, 1f) * 0.5f, new Vector2((float)nPC.width, (float)nPC.height * ((float)(int)nPC.wetCount / 9f)) * MathHelper.Clamp(num * 10f, 0f, 1f), RippleShape.Circle);
				}
			}
		}
		if (_usePlayerWaves)
		{
			for (int j = 0; j < 255; j++)
			{
				if (Main.player[j] == null || !Main.player[j].active || (!Main.player[j].wet && Main.player[j].wetCount == 0) || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, Main.player[j].position - val3, Main.player[j].Size + val3))
				{
					continue;
				}
				Player player = Main.player[j];
				Vector2 val6 = player.Center - val2;
				float num4 = player.velocity.Length();
				num4 = 0.05f * (float)Math.Sqrt(num4);
				Vector2 velocity3 = player.velocity;
				velocity3.Normalize();
				val6 -= velocity3 * 10f;
				if (!_useViscosityFilter && (player.honeyWet || player.lavaWet))
				{
					num4 *= 0.3f;
				}
				if (player.wet)
				{
					tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(val6.X - (float)player.width * 2f * 0.5f, val6.Y - (float)player.height * 2f * 0.5f, (float)player.width * 2f, (float)player.height * 2f) * 0.25f, new VertexColors(new Color(velocity3.X * 0.5f + 0.5f, velocity3.Y * 0.5f + 0.5f, 0.5f * num4)));
				}
				if (player.wetCount != 0)
				{
					float num5 = 5f;
					if (!player.wet)
					{
						num5 = -20f;
					}
					num4 *= 3f;
					QueueRipple(player.Center + velocity3 * num5, player.wet ? num4 : (0f - num4), new Vector2((float)player.width, (float)player.height * ((float)(int)player.wetCount / 9f)) * MathHelper.Clamp(num4 * 10f, 0f, 1f), RippleShape.Circle);
				}
			}
		}
		if (_useProjectileWaves)
		{
			for (int k = 0; k < 1000; k++)
			{
				Projectile projectile = Main.projectile[k];
				if (projectile.wet && !projectile.lavaWet)
				{
					_ = !projectile.honeyWet;
				}
				else
					_ = 0;
				bool flag = projectile.lavaWet;
				bool flag2 = projectile.honeyWet;
				bool flag3 = projectile.wet;
				if (projectile.ignoreWater)
				{
					flag3 = true;
				}
				if (!((projectile != null && projectile.active && ProjectileID.Sets.CanDistortWater[projectile.type]) & flag3) || ProjectileID.Sets.NoLiquidDistortion[projectile.type] || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, projectile.position - val3, projectile.Size + val3))
				{
					continue;
				}
				if (projectile.ignoreWater)
				{
					bool flag4 = Collision.LavaCollision(projectile.position, projectile.width, projectile.height);
					flag = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
					flag2 = Collision.honey;
					if (!(flag4 | flag | flag2))
					{
						continue;
					}
				}
				Vector2 val7 = projectile.Center - val2;
				float num6 = projectile.velocity.Length();
				num6 = 2f * (float)Math.Sqrt(0.05f * num6);
				Vector2 velocity4 = projectile.velocity;
				velocity4.Normalize();
				if (!_useViscosityFilter && (flag2 | flag))
				{
					num6 *= 0.3f;
				}
				float num7 = Math.Max(12f, (float)projectile.width * 0.75f);
				float num8 = Math.Max(12f, (float)projectile.height * 0.75f);
				tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(val7.X - num7 * 0.5f, val7.Y - num8 * 0.5f, num7, num8) * 0.25f, new VertexColors(new Color(velocity4.X * 0.5f + 0.5f, velocity4.Y * 0.5f + 0.5f, num6 * 0.5f)));
			}
		}
		tileBatch.End();
		if (_useRippleWaves)
		{
			tileBatch.Begin();
			for (int l = 0; l < _rippleQueueCount; l++)
			{
				Vector2 val8 = _rippleQueue[l].Position - val2;
				Vector2 size = _rippleQueue[l].Size;
				Rectangle sourceRectangle = _rippleQueue[l].SourceRectangle;
				Texture2D value = _rippleShapeTexture.Value;
				tileBatch.Draw(value, new Vector4(val8.X, val8.Y, size.X, size.Y) * 0.25f, sourceRectangle, new VertexColors(_rippleQueue[l].WaveData), new Vector2((float)(sourceRectangle.Width / 2), (float)(sourceRectangle.Height / 2)), (SpriteEffects)0, _rippleQueue[l].Rotation);
			}
			tileBatch.End();
		}
		_rippleQueueCount = 0;
		if (_useCustomWaves && OnWaveDraw != null)
		{
			tileBatch.Begin();
			OnWaveDraw(tileBatch);
			tileBatch.End();
		}
	}

	private void PreDraw(GameTime gameTime)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		ValidateRenderTargets();
		if (!_usingRenderTargets || !Main.IsGraphicsDeviceAvailable)
		{
			return;
		}
		if (_useProjectileWaves || _useRippleWaves || _useCustomWaves || _usePlayerWaves)
		{
			for (int i = 0; i < Math.Min(_queuedSteps, 2); i++)
			{
				StepLiquids();
			}
		}
		else if (_isWaveBufferDirty || _clearNextFrame)
		{
			GraphicsDevice graphicsDevice = ((Game)Main.instance).GraphicsDevice;
			graphicsDevice.SetRenderTarget(_distortionTarget);
			graphicsDevice.Clear(new Color(0.5f, 0.5f, 0f, 1f));
			_clearNextFrame = false;
			_isWaveBufferDirty = false;
			graphicsDevice.SetRenderTarget((RenderTarget2D)null);
		}
		_queuedSteps = 0;
	}

	public override void Apply()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		if (_usingRenderTargets && Main.IsGraphicsDeviceAvailable)
		{
			UseProgress(_progress);
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			Vector2 val = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
			Vector2 unscaledScreenPosition = ScreenShaderData.UnscaledScreenPosition;
			Vector2 val2 = (Main.drawToScreen ? Vector2.Zero : val) - unscaledScreenPosition;
			UseImage((Texture2D)(DrawRipples ? ((object)_distortionTarget) : ((object)_noDistortionTexture)), 1);
			UseImage(Main.waterTarget.Texture, 2, SamplerState.PointClamp);
			UseTargetPosition(unscaledScreenPosition + val - Main.waterTarget.Position);
			UseImageOffset(-(val2 - _lastDistortionDrawOffset / 0.25f));
			base.Apply();
		}
	}

	private void ValidateRenderTargets()
	{
		int backBufferWidth = ((Game)Main.instance).GraphicsDevice.PresentationParameters.BackBufferWidth;
		int backBufferHeight = ((Game)Main.instance).GraphicsDevice.PresentationParameters.BackBufferHeight;
		bool flag = !Main.drawToScreen;
		if (_usingRenderTargets && !flag)
		{
			ReleaseRenderTargets();
		}
		else if (!_usingRenderTargets & flag)
		{
			InitRenderTargets(backBufferWidth, backBufferHeight);
		}
		else if ((_usingRenderTargets & flag) && (_distortionTarget.IsContentLost || _distortionTargetSwap.IsContentLost))
		{
			_clearNextFrame = true;
		}
	}

	private void InitRenderTargets(int width, int height)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected Obj, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected Obj, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected Obj, but got Unknown
		_lastScreenWidth = width;
		_lastScreenHeight = height;
		width = (int)((float)width * 0.25f);
		height = (int)((float)height * 0.25f);
		try
		{
			_noDistortionTexture = new Texture2D(((Game)Main.instance).GraphicsDevice, 1, 1, false, (SurfaceFormat)0);
			_noDistortionTexture.SetData<Color>(new Color[1]
			{
				new Color(0.5f, 0.5f, 0f, 1f)
			});
			_distortionTarget = new RenderTarget2D(((Game)Main.instance).GraphicsDevice, width, height, false, (SurfaceFormat)0, (DepthFormat)0, 0, (RenderTargetUsage)1);
			_distortionTargetSwap = new RenderTarget2D(((Game)Main.instance).GraphicsDevice, width, height, false, (SurfaceFormat)0, (DepthFormat)0, 0, (RenderTargetUsage)1);
			_usingRenderTargets = true;
			_clearNextFrame = true;
		}
		catch (Exception ex)
		{
			Lighting.Mode = LightMode.Retro;
			_usingRenderTargets = false;
			Console.WriteLine("Failed to create water distortion render targets. " + ex);
		}
	}

	private void ReleaseRenderTargets()
	{
		try
		{
			if (_distortionTarget != null)
			{
				((GraphicsResource)_distortionTarget).Dispose();
			}
			if (_distortionTargetSwap != null)
			{
				((GraphicsResource)_distortionTargetSwap).Dispose();
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error disposing of water distortion render targets. " + ex);
		}
		_distortionTarget = null;
		_distortionTargetSwap = null;
		_usingRenderTargets = false;
	}

	public void QueueRipple(Vector2 position, float strength = 1f, RippleShape shape = RippleShape.Square, float rotation = 0f)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		float num = strength * 0.5f + 0.5f;
		float num2 = Math.Min(Math.Abs(strength), 1f);
		QueueRipple(position, new Color(0.5f, num, 0f, 1f) * num2, new Vector2(4f * Math.Max(Math.Abs(strength), 1f)), shape, rotation);
	}

	public void QueueRipple(Vector2 position, float strength, Vector2 size, RippleShape shape = RippleShape.Square, float rotation = 0f)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		float num = strength * 0.5f + 0.5f;
		float num2 = Math.Min(Math.Abs(strength), 1f);
		QueueRipple(position, new Color(0.5f, num, 0f, 1f) * num2, size, shape, rotation);
	}

	public void QueueRipple(Vector2 position, Color waveData, Vector2 size, RippleShape shape = RippleShape.Square, float rotation = 0f)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (!_useRippleWaves || Main.drawToScreen)
		{
			_rippleQueueCount = 0;
		}
		else if (_rippleQueueCount < _rippleQueue.Length)
		{
			_rippleQueue[_rippleQueueCount++] = new Ripple(position, waveData, size, shape, rotation);
		}
	}
}
