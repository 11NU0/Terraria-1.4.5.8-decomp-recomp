using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent.Ambience;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class AmbientSky : CustomSky
{
	private abstract class SkyEntity
	{
		public Vector2 Position;

		public Asset<Texture2D> Texture;

		public SpriteFrame Frame;

		public float Depth;

		public SpriteEffects Effects;

		public bool IsActive = true;

		public float Rotation;

		public Rectangle SourceRectangle
		{
			get
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				return Frame.GetSourceRectangle(Texture.Value);
			}
		}

		protected void NextFrame()
		{
			Frame.CurrentRow = (byte)((Frame.CurrentRow + 1) % Frame.RowCount);
		}

		public abstract Color GetColor(Color backgroundColor);

		public abstract void Update(int frameCount);

		protected void SetPositionInWorldBasedOnScreenSpace(Vector2 actualWorldSpace)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = actualWorldSpace - Main.Camera.Center;
			Vector2 position = Main.Camera.Center + val * (Depth / 3f);
			Position = position;
		}

		public abstract Vector2 GetDrawPosition();

		public virtual void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
		{
			CommonDraw(spriteBatch, depthScale, minDepth, maxDepth);
		}

		public void CommonDraw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if (!(Depth <= minDepth) && !(Depth > maxDepth))
			{
				Vector2 drawPositionByDepth = GetDrawPositionByDepth();
				Color val = GetColor(Main.ColorOfTheSkies) * Main.atmo;
				Vector2 val2 = SourceRectangle.Size() / 2f;
				float num = depthScale / Depth;
				spriteBatch.Draw(Texture.Value, drawPositionByDepth - Main.Camera.UnscaledPosition, (Rectangle?)SourceRectangle, val, Rotation, val2, num, Effects, 0f);
			}
		}

		internal Vector2 GetDrawPositionByDepth()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			return (GetDrawPosition() - Main.Camera.Center) * new Vector2(1f / Depth, 0.9f / Depth) + Main.Camera.Center;
		}

		internal float Helper_GetOpacityWithAccountingForOceanWaterLine()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = GetDrawPositionByDepth() - Main.Camera.UnscaledPosition;
			int num = SourceRectangle.Height / 2;
			float t = val.Y + (float)num;
			float yScreenPosition = AmbientSkyDrawCache.Instance.OceanLineInfo.YScreenPosition;
			float lerpValue = Utils.GetLerpValue(yScreenPosition - 10f, yScreenPosition - 2f, t, clamped: true);
			lerpValue *= AmbientSkyDrawCache.Instance.OceanLineInfo.OceanOpacity;
			return 1f - lerpValue;
		}
	}

	private class FadingSkyEntity : SkyEntity
	{
		protected int LifeTime;

		protected Vector2 Velocity;

		protected int FramingSpeed;

		protected int TimeEntitySpawnedIn;

		protected float Opacity;

		protected float BrightnessLerper;

		protected float FinalOpacityMultiplier;

		protected float OpacityNormalizedTimeToFadeIn;

		protected float OpacityNormalizedTimeToFadeOut;

		protected int FrameOffset;

		public FadingSkyEntity()
		{
			Opacity = 0f;
			TimeEntitySpawnedIn = -1;
			BrightnessLerper = 0f;
			FinalOpacityMultiplier = 1f;
			OpacityNormalizedTimeToFadeIn = 0.1f;
			OpacityNormalizedTimeToFadeOut = 0.9f;
		}

		public override void Update(int frameCount)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (!IsMovementDone(frameCount))
			{
				UpdateOpacity(frameCount);
				if ((frameCount + FrameOffset) % FramingSpeed == 0)
				{
					NextFrame();
				}
				UpdateVelocity(frameCount);
				Position += Velocity;
			}
		}

		public virtual void UpdateVelocity(int frameCount)
		{
		}

		private void UpdateOpacity(int frameCount)
		{
			int num = frameCount - TimeEntitySpawnedIn;
			if ((float)num >= (float)LifeTime * OpacityNormalizedTimeToFadeOut)
			{
				Opacity = Utils.GetLerpValue(LifeTime, (float)LifeTime * OpacityNormalizedTimeToFadeOut, num, clamped: true);
			}
			else
			{
				Opacity = Utils.GetLerpValue(0f, (float)LifeTime * OpacityNormalizedTimeToFadeIn, num, clamped: true);
			}
		}

		private bool IsMovementDone(int frameCount)
		{
			if (TimeEntitySpawnedIn == -1)
			{
				TimeEntitySpawnedIn = frameCount;
			}
			if (frameCount - TimeEntitySpawnedIn >= LifeTime)
			{
				IsActive = false;
				return true;
			}
			return false;
		}

		public override Color GetColor(Color backgroundColor)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			return Color.Lerp(backgroundColor, Color.White, BrightnessLerper) * Opacity * FinalOpacityMultiplier * Helper_GetOpacityWithAccountingForOceanWaterLine();
		}

		public void StartFadingOut(int currentFrameCount)
		{
			int num = (int)((float)LifeTime * OpacityNormalizedTimeToFadeOut);
			int num2 = currentFrameCount - num;
			if (num2 < TimeEntitySpawnedIn)
			{
				TimeEntitySpawnedIn = num2;
			}
		}

		public override Vector2 GetDrawPosition()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Position;
		}
	}

	private class ButterfliesSkyEntity : FadingSkyEntity
	{
		public ButterfliesSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 4000f) + 4000f;
			Depth = random.NextFloat() * 3f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			int num2 = random.Next(2) + 1;
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/ButterflySwarm" + num2, (AssetRequestMode)1);
			Frame = new SpriteFrame(1, (byte)((num2 == 2) ? 19u : 17u));
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.15f;
			OpacityNormalizedTimeToFadeOut = 0.85f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 5;
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			float num = 0.1f + Math.Abs(Main.WindForVisuals) * 0.05f;
			Velocity = new Vector2(num * (float)(((int)Effects != 1) ? 1 : (-1)), 0f);
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
			if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
			{
				StartFadingOut(frameCount);
			}
		}
	}

	private class LostKiteSkyEntity : FadingSkyEntity
	{
		public LostKiteSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			Depth = random.NextFloat() * 3f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/LostKite", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 42);
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.15f;
			OpacityNormalizedTimeToFadeOut = 0.85f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 6;
			int num2 = random.Next(Frame.RowCount);
			for (int i = 0; i < num2; i++)
			{
				NextFrame();
			}
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Invalid comparison between Unknown and I4
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			float num = 1.2f + Math.Abs(Main.WindForVisuals) * 3f;
			if (Main.IsItStorming)
			{
				num *= 1.5f;
			}
			Velocity = new Vector2(num * (float)(((int)Effects != 1) ? 1 : (-1)), 0f);
		}

		public override void Update(int frameCount)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (Main.IsItStorming)
			{
				FramingSpeed = 4;
			}
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			base.Update(frameCount);
			if (!Main.dayTime || Main.eclipse)
			{
				StartFadingOut(frameCount);
			}
		}
	}

	private class PegasusSkyEntity : FadingSkyEntity
	{
		public PegasusSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			Depth = random.NextFloat() * 3f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Pegasus", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 11);
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.15f;
			OpacityNormalizedTimeToFadeOut = 0.85f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 5;
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			float num = 1.5f + Math.Abs(Main.WindForVisuals) * 0.6f;
			Velocity = new Vector2(num * (float)(((int)Effects != 1) ? 1 : (-1)), 0f);
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
			if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
			{
				StartFadingOut(frameCount);
			}
		}

		public override Color GetColor(Color backgroundColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[6];
		}
	}

	private class VultureSkyEntity : FadingSkyEntity
	{
		public VultureSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			Depth = random.NextFloat() * 3f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Vulture", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 10);
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.15f;
			OpacityNormalizedTimeToFadeOut = 0.85f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 5;
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			float num = 3f + Math.Abs(Main.WindForVisuals) * 0.8f;
			Velocity = new Vector2(num * (float)(((int)Effects != 1) ? 1 : (-1)), 0f);
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
			if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
			{
				StartFadingOut(frameCount);
			}
		}

		public override Color GetColor(Color backgroundColor)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			float val = Math.Max(Main.bgAlphaFrontLayer[5], Main.bgAlphaFrontLayer[14]);
			val = Math.Max(val, Main.bgAlphaFrontLayer[13]);
			return base.GetColor(backgroundColor) * Math.Max(Main.bgAlphaFrontLayer[2], val);
		}
	}

	private class PixiePosseSkyEntity : FadingSkyEntity
	{
		private int pixieType = 1;

		public PixiePosseSkyEntity(Player player, FastRandom random)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Invalid comparison between Unknown and I4
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 4000f) + 4000f;
			Depth = random.NextFloat() * 3f + 2f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			if (!Main.dayTime)
			{
				pixieType = 2;
			}
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/PixiePosse" + pixieType, (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 25);
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.15f;
			OpacityNormalizedTimeToFadeOut = 0.85f;
			BrightnessLerper = 0.6f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 5;
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			float num = 0.12f + Math.Abs(Main.WindForVisuals) * 0.08f;
			Velocity = new Vector2(num * (float)(((int)Effects != 1) ? 1 : (-1)), 0f);
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
			if ((pixieType == 1 && !Main.dayTime) || (pixieType == 2 && Main.dayTime) || Main.IsItRaining || Main.eclipse || Main.bloodMoon || Main.pumpkinMoon || Main.snowMoon)
			{
				StartFadingOut(frameCount);
			}
		}

		public override void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
		{
			CommonDraw(spriteBatch, depthScale - 0.1f, minDepth, maxDepth);
		}
	}

	private class BirdsPackSkyEntity : FadingSkyEntity
	{
		public BirdsPackSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			Depth = random.NextFloat() * 3f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/BirdsVShape", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 4);
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.15f;
			OpacityNormalizedTimeToFadeOut = 0.85f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 5;
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			float num = 3f + Math.Abs(Main.WindForVisuals) * 0.8f;
			Velocity = new Vector2(num * (float)(((int)Effects != 1) ? 1 : (-1)), 0f);
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
			if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
			{
				StartFadingOut(frameCount);
			}
		}
	}

	private class SeagullsGroupSkyEntity : FadingSkyEntity
	{
		private Vector2 _magnetAccelerations;

		private Vector2 _magnetPointTarget;

		private Vector2 _positionVsMagnet;

		private Vector2 _velocityVsMagnet;

		public SeagullsGroupSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			Depth = random.NextFloat() * 3f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Seagull", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 9);
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.1f;
			OpacityNormalizedTimeToFadeOut = 0.9f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 4;
			FrameOffset = random.Next(0, Frame.RowCount);
			int num2 = random.Next(Frame.RowCount);
			for (int i = 0; i < num2; i++)
			{
				NextFrame();
			}
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Invalid comparison between Unknown and I4
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = _magnetAccelerations * new Vector2((float)Math.Sign(_magnetPointTarget.X - _positionVsMagnet.X), (float)Math.Sign(_magnetPointTarget.Y - _positionVsMagnet.Y));
			_velocityVsMagnet += val;
			_positionVsMagnet += _velocityVsMagnet;
			float num = 4f * (float)(((int)Effects != 1) ? 1 : (-1));
			Velocity = new Vector2(num, 0f) + _velocityVsMagnet;
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
			if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
			{
				StartFadingOut(frameCount);
			}
		}

		public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_magnetAccelerations = accelerations;
			_magnetPointTarget = targetOffset;
		}

		public override Color GetColor(Color backgroundColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[4];
		}

		public override void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
		{
			CommonDraw(spriteBatch, depthScale - 1.5f, minDepth, maxDepth);
		}

		public static List<SeagullsGroupSkyEntity> CreateGroup(Player player, FastRandom random)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Invalid comparison between Unknown and I4
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			List<SeagullsGroupSkyEntity> list = new List<SeagullsGroupSkyEntity>();
			int num = 100;
			int num2 = random.Next(5, 9);
			float num3 = 100f;
			VirtualCamera virtualCamera = new VirtualCamera(player);
			SpriteEffects val = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Vector2 val2 = default;
			if ((int)val == 1)
			{
				val2.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				val2.X = virtualCamera.Position.X - (float)num;
			}
			val2.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			float num4 = random.NextFloat() * 2f + 1f;
			int num5 = random.Next(30, 61) * 60;
			Vector2 val3 = new Vector2(random.NextFloat() * 0.5f + 0.5f, random.NextFloat() * 0.5f + 0.5f);
			Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * num3;
			for (int i = 0; i < num2; i++)
			{
				SeagullsGroupSkyEntity seagullsGroupSkyEntity = new SeagullsGroupSkyEntity(player, random);
				seagullsGroupSkyEntity.Depth = num4 + random.NextFloat() * 0.5f;
				seagullsGroupSkyEntity.Position = val2 + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 50f;
				seagullsGroupSkyEntity.Effects = val;
				seagullsGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(seagullsGroupSkyEntity.Position);
				seagullsGroupSkyEntity.LifeTime = num5 + random.Next(301);
				seagullsGroupSkyEntity.SetMagnetization(val3 * (random.NextFloat() * 0.3f + 0.85f) * 0.05f, targetOffset);
				list.Add(seagullsGroupSkyEntity);
			}
			return list;
		}
	}

	private class GastropodGroupSkyEntity : FadingSkyEntity
	{
		private Vector2 _magnetAccelerations;

		private Vector2 _magnetPointTarget;

		private Vector2 _positionVsMagnet;

		private Vector2 _velocityVsMagnet;

		public GastropodGroupSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 3200f) + 3200f;
			Depth = random.NextFloat() * 3f + 2f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Gastropod", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 1);
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.1f;
			OpacityNormalizedTimeToFadeOut = 0.9f;
			BrightnessLerper = 0.75f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = int.MaxValue;
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Invalid comparison between Unknown and I4
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = _magnetAccelerations * new Vector2((float)Math.Sign(_magnetPointTarget.X - _positionVsMagnet.X), (float)Math.Sign(_magnetPointTarget.Y - _positionVsMagnet.Y));
			_velocityVsMagnet += val;
			_positionVsMagnet += _velocityVsMagnet;
			float num = (1.5f + Math.Abs(Main.WindForVisuals) * 0.2f) * (float)(((int)Effects != 1) ? 1 : (-1));
			Velocity = new Vector2(num, 0f) + _velocityVsMagnet;
			Rotation = Velocity.X * 0.1f;
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
			if (Main.IsItRaining || Main.dayTime || Main.bloodMoon || Main.pumpkinMoon || Main.snowMoon)
			{
				StartFadingOut(frameCount);
			}
		}

		public override Color GetColor(Color backgroundColor)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			return Color.Lerp(backgroundColor, Colors.AmbientNPCGastropodLight, BrightnessLerper) * Opacity * FinalOpacityMultiplier;
		}

		public override void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
		{
			CommonDraw(spriteBatch, depthScale - 0.1f, minDepth, maxDepth);
		}

		public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_magnetAccelerations = accelerations;
			_magnetPointTarget = targetOffset;
		}

		public static List<GastropodGroupSkyEntity> CreateGroup(Player player, FastRandom random)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Invalid comparison between Unknown and I4
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			List<GastropodGroupSkyEntity> list = new List<GastropodGroupSkyEntity>();
			int num = 100;
			int num2 = random.Next(3, 8);
			VirtualCamera virtualCamera = new VirtualCamera(player);
			SpriteEffects val = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Vector2 val2 = default;
			if ((int)val == 1)
			{
				val2.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				val2.X = virtualCamera.Position.X - (float)num;
			}
			val2.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 3200f) + 3200f;
			float num3 = random.NextFloat() * 3f + 2f;
			int num4 = random.Next(30, 61) * 60;
			Vector2 val3 = new Vector2(random.NextFloat() * 0.1f + 0.1f, random.NextFloat() * 0.3f + 0.3f);
			Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 120f;
			for (int i = 0; i < num2; i++)
			{
				GastropodGroupSkyEntity gastropodGroupSkyEntity = new GastropodGroupSkyEntity(player, random);
				gastropodGroupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
				gastropodGroupSkyEntity.Position = val2 + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 60f;
				gastropodGroupSkyEntity.Effects = val;
				gastropodGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(gastropodGroupSkyEntity.Position);
				gastropodGroupSkyEntity.LifeTime = num4 + random.Next(301);
				gastropodGroupSkyEntity.SetMagnetization(val3 * (random.NextFloat() * 0.5f) * 0.05f, targetOffset);
				list.Add(gastropodGroupSkyEntity);
			}
			return list;
		}
	}

	private class SlimeBalloonGroupSkyEntity : FadingSkyEntity
	{
		private Vector2 _magnetAccelerations;

		private Vector2 _magnetPointTarget;

		private Vector2 _positionVsMagnet;

		private Vector2 _velocityVsMagnet;

		public SlimeBalloonGroupSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 4000f) + 4000f;
			Depth = random.NextFloat() * 3f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/SlimeBalloons", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 7);
			Frame.CurrentRow = (byte)random.Next(7);
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.025f;
			OpacityNormalizedTimeToFadeOut = 0.975f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = int.MaxValue;
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Invalid comparison between Unknown and I4
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = _magnetAccelerations * new Vector2((float)Math.Sign(_magnetPointTarget.X - _positionVsMagnet.X), (float)Math.Sign(_magnetPointTarget.Y - _positionVsMagnet.Y));
			_velocityVsMagnet += val;
			_positionVsMagnet += _velocityVsMagnet;
			float num = (1f + Math.Abs(Main.WindForVisuals) * 1f) * (float)(((int)Effects != 1) ? 1 : (-1));
			Velocity = new Vector2(num, -0.01f) + _velocityVsMagnet;
			Rotation = Velocity.X * 0.1f;
		}

		public override void Update(int frameCount)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			base.Update(frameCount);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			if (!Main.IsItAHappyWindyDay || Main.IsItRaining || !Main.dayTime || Main.eclipse)
			{
				StartFadingOut(frameCount);
			}
		}

		public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_magnetAccelerations = accelerations;
			_magnetPointTarget = targetOffset;
		}

		public static List<SlimeBalloonGroupSkyEntity> CreateGroup(Player player, FastRandom random)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Invalid comparison between Unknown and I4
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			List<SlimeBalloonGroupSkyEntity> list = new List<SlimeBalloonGroupSkyEntity>();
			int num = 100;
			int num2 = random.Next(5, 10);
			VirtualCamera virtualCamera = new VirtualCamera(player);
			SpriteEffects val = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Vector2 val2 = default;
			if ((int)val == 1)
			{
				val2.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				val2.X = virtualCamera.Position.X - (float)num;
			}
			val2.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			float num3 = random.NextFloat() * 3f + 3f;
			int num4 = random.Next(80, 121) * 60;
			Vector2 val3 = new Vector2(random.NextFloat() * 0.1f + 0.1f, random.NextFloat() * 0.1f + 0.1f);
			Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 150f;
			for (int i = 0; i < num2; i++)
			{
				SlimeBalloonGroupSkyEntity slimeBalloonGroupSkyEntity = new SlimeBalloonGroupSkyEntity(player, random);
				slimeBalloonGroupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
				slimeBalloonGroupSkyEntity.Position = val2 + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 80f;
				slimeBalloonGroupSkyEntity.Effects = val;
				slimeBalloonGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(slimeBalloonGroupSkyEntity.Position);
				slimeBalloonGroupSkyEntity.LifeTime = num4 + random.Next(301);
				slimeBalloonGroupSkyEntity.SetMagnetization(val3 * (random.NextFloat() * 0.2f) * 0.05f, targetOffset);
				list.Add(slimeBalloonGroupSkyEntity);
			}
			return list;
		}
	}

	private class HellBatsGoupSkyEntity : FadingSkyEntity
	{
		private Vector2 _magnetAccelerations;

		private Vector2 _magnetPointTarget;

		private Vector2 _positionVsMagnet;

		private Vector2 _velocityVsMagnet;

		public HellBatsGoupSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * 400f + (float)(Main.UnderworldLayer * 16);
			Depth = random.NextFloat() * 5f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/HellBat" + random.Next(1, 3), (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 10);
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.1f;
			OpacityNormalizedTimeToFadeOut = 0.9f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 4;
			FrameOffset = random.Next(0, Frame.RowCount);
			int num2 = random.Next(Frame.RowCount);
			for (int i = 0; i < num2; i++)
			{
				NextFrame();
			}
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Invalid comparison between Unknown and I4
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = _magnetAccelerations * new Vector2((float)Math.Sign(_magnetPointTarget.X - _positionVsMagnet.X), (float)Math.Sign(_magnetPointTarget.Y - _positionVsMagnet.Y));
			_velocityVsMagnet += val;
			_positionVsMagnet += _velocityVsMagnet;
			float num = (3f + Math.Abs(Main.WindForVisuals) * 0.8f) * (float)(((int)Effects != 1) ? 1 : (-1));
			Velocity = new Vector2(num, 0f) + _velocityVsMagnet;
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
		}

		public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_magnetAccelerations = accelerations;
			_magnetPointTarget = targetOffset;
		}

		public override Color GetColor(Color backgroundColor)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			return Color.Lerp(Color.White, Color.Gray, Depth / 15f) * Opacity * FinalOpacityMultiplier * Helper_GetOpacityWithAccountingForBackgroundsOff();
		}

		public static List<HellBatsGoupSkyEntity> CreateGroup(Player player, FastRandom random)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Invalid comparison between Unknown and I4
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			List<HellBatsGoupSkyEntity> list = new List<HellBatsGoupSkyEntity>();
			int num = 100;
			int num2 = random.Next(20, 40);
			VirtualCamera virtualCamera = new VirtualCamera(player);
			SpriteEffects val = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Vector2 val2 = default;
			if ((int)val == 1)
			{
				val2.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				val2.X = virtualCamera.Position.X - (float)num;
			}
			val2.Y = random.NextFloat() * 800f + (float)(Main.UnderworldLayer * 16);
			float num3 = random.NextFloat() * 5f + 3f;
			int num4 = random.Next(30, 61) * 60;
			Vector2 val3 = new Vector2(random.NextFloat() * 0.5f + 0.5f, random.NextFloat() * 0.5f + 0.5f);
			Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 100f;
			for (int i = 0; i < num2; i++)
			{
				HellBatsGoupSkyEntity hellBatsGoupSkyEntity = new HellBatsGoupSkyEntity(player, random);
				hellBatsGoupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
				hellBatsGoupSkyEntity.Position = val2 + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 50f;
				hellBatsGoupSkyEntity.Effects = val;
				hellBatsGoupSkyEntity.SetPositionInWorldBasedOnScreenSpace(hellBatsGoupSkyEntity.Position);
				hellBatsGoupSkyEntity.LifeTime = num4 + random.Next(301);
				hellBatsGoupSkyEntity.SetMagnetization(val3 * (random.NextFloat() * 0.3f + 0.85f) * 0.05f, targetOffset);
				list.Add(hellBatsGoupSkyEntity);
			}
			return list;
		}

		internal float Helper_GetOpacityWithAccountingForBackgroundsOff()
		{
			if (Main.netMode == 2 || Main.BackgroundEnabled)
			{
				return 1f;
			}
			return 0f;
		}
	}

	private class BatsGroupSkyEntity : FadingSkyEntity
	{
		private Vector2 _magnetAccelerations;

		private Vector2 _magnetPointTarget;

		private Vector2 _positionVsMagnet;

		private Vector2 _velocityVsMagnet;

		public BatsGroupSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			Depth = random.NextFloat() * 3f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Bat" + random.Next(1, 4), (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 10);
			LifeTime = random.Next(60, 121) * 60;
			OpacityNormalizedTimeToFadeIn = 0.1f;
			OpacityNormalizedTimeToFadeOut = 0.9f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 4;
			FrameOffset = random.Next(0, Frame.RowCount);
			int num2 = random.Next(Frame.RowCount);
			for (int i = 0; i < num2; i++)
			{
				NextFrame();
			}
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Invalid comparison between Unknown and I4
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = _magnetAccelerations * new Vector2((float)Math.Sign(_magnetPointTarget.X - _positionVsMagnet.X), (float)Math.Sign(_magnetPointTarget.Y - _positionVsMagnet.Y));
			_velocityVsMagnet += val;
			_positionVsMagnet += _velocityVsMagnet;
			float num = (3f + Math.Abs(Main.WindForVisuals) * 0.8f) * (float)(((int)Effects != 1) ? 1 : (-1));
			Velocity = new Vector2(num, 0f) + _velocityVsMagnet;
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
			if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
			{
				StartFadingOut(frameCount);
			}
		}

		public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_magnetAccelerations = accelerations;
			_magnetPointTarget = targetOffset;
		}

		public override Color GetColor(Color backgroundColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			return base.GetColor(backgroundColor) * Utils.Max<float>(Main.bgAlphaFrontLayer[3], Main.bgAlphaFrontLayer[0], Main.bgAlphaFrontLayer[10], Main.bgAlphaFrontLayer[11], Main.bgAlphaFrontLayer[12]);
		}

		public static List<BatsGroupSkyEntity> CreateGroup(Player player, FastRandom random)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Invalid comparison between Unknown and I4
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			List<BatsGroupSkyEntity> list = new List<BatsGroupSkyEntity>();
			int num = 100;
			int num2 = random.Next(20, 40);
			VirtualCamera virtualCamera = new VirtualCamera(player);
			SpriteEffects val = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Vector2 val2 = default;
			if ((int)val == 1)
			{
				val2.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				val2.X = virtualCamera.Position.X - (float)num;
			}
			val2.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			float num3 = random.NextFloat() * 3f + 3f;
			int num4 = random.Next(30, 61) * 60;
			Vector2 val3 = new Vector2(random.NextFloat() * 0.5f + 0.5f, random.NextFloat() * 0.5f + 0.5f);
			Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 100f;
			for (int i = 0; i < num2; i++)
			{
				BatsGroupSkyEntity batsGroupSkyEntity = new BatsGroupSkyEntity(player, random);
				batsGroupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
				batsGroupSkyEntity.Position = val2 + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 50f;
				batsGroupSkyEntity.Effects = val;
				batsGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(batsGroupSkyEntity.Position);
				batsGroupSkyEntity.LifeTime = num4 + random.Next(301);
				batsGroupSkyEntity.SetMagnetization(val3 * (random.NextFloat() * 0.3f + 0.85f) * 0.05f, targetOffset);
				list.Add(batsGroupSkyEntity);
			}
			return list;
		}
	}

	private class WyvernSkyEntity : FadingSkyEntity
	{
		public WyvernSkyEntity(Player player, FastRandom random)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (!(Main.WindForVisuals > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			Depth = random.NextFloat() * 3f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Wyvern", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 5);
			LifeTime = random.Next(40, 71) * 60;
			OpacityNormalizedTimeToFadeIn = 0.15f;
			OpacityNormalizedTimeToFadeOut = 0.85f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 4;
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			float num = 3f + Math.Abs(Main.WindForVisuals) * 0.8f;
			Velocity = new Vector2(num * (float)(((int)Effects != 1) ? 1 : (-1)), 0f);
		}
	}

	private class NormalizedBackgroundLayerSpaceSkyEntity : SkyEntity
	{
		public override Color GetColor(Color backgroundColor)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return Color.Lerp(backgroundColor, Color.White, 0.3f);
		}

		public override Vector2 GetDrawPosition()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Position;
		}

		public override void Update(int frameCount)
		{
		}
	}

	private class BoneSerpentSkyEntity : NormalizedBackgroundLayerSpaceSkyEntity
	{
	}

	private class AirshipSkyEntity : FadingSkyEntity
	{
		public AirshipSkyEntity(Player player, FastRandom random)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Invalid comparison between Unknown and I4
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera virtualCamera = new VirtualCamera(player);
			Effects = (random.Next(2) != 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = 100;
			if ((int)Effects == 1)
			{
				Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
			}
			else
			{
				Position.X = virtualCamera.Position.X - (float)num;
			}
			Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
			Depth = random.NextFloat() * 3f + 3f;
			SetPositionInWorldBasedOnScreenSpace(Position);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/FlyingShip", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 4);
			LifeTime = random.Next(40, 71) * 60;
			OpacityNormalizedTimeToFadeIn = 0.05f;
			OpacityNormalizedTimeToFadeOut = 0.95f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 4;
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			float num = 6f + Math.Abs(Main.WindForVisuals) * 1.6f;
			Velocity = new Vector2(num * (float)(((int)Effects != 1) ? 1 : (-1)), 0f);
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
			if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
			{
				StartFadingOut(frameCount);
			}
		}
	}

	private class AirBalloonSkyEntity : FadingSkyEntity
	{
		private const int RANDOM_TILE_SPAWN_RANGE = 100;

		public AirBalloonSkyEntity(Player player, FastRandom random)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			new VirtualCamera(player);
			int x = player.Center.ToTileCoordinates().X;
			Effects = (random.Next(2) != 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Position.X = ((float)x + 100f * (random.NextFloat() * 2f - 1f)) * 16f;
			Position.Y = (float)Main.worldSurface * 16f - (float)random.Next(50, 81) * 16f;
			Depth = random.NextFloat() * 3f + 3f;
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/AirBalloons_" + ((random.Next(2) == 0) ? "Large" : "Small"), (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 5);
			Frame.CurrentRow = (byte)random.Next(5);
			LifeTime = random.Next(20, 51) * 60;
			OpacityNormalizedTimeToFadeIn = 0.05f;
			OpacityNormalizedTimeToFadeOut = 0.95f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = int.MaxValue;
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			float num = Main.WindForVisuals * 4f;
			float num2 = 3f + Math.Abs(Main.WindForVisuals) * 1f;
			if ((double)Position.Y < Main.worldSurface * 12.0)
			{
				num2 *= 0.5f;
			}
			if ((double)Position.Y < Main.worldSurface * 8.0)
			{
				num2 *= 0.5f;
			}
			if ((double)Position.Y < Main.worldSurface * 4.0)
			{
				num2 *= 0.5f;
			}
			Velocity = new Vector2(num, 0f - num2);
		}

		public override void Update(int frameCount)
		{
			base.Update(frameCount);
			if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
			{
				StartFadingOut(frameCount);
			}
		}
	}

	private class CrimeraSkyEntity : EOCSkyEntity
	{
		public CrimeraSkyEntity(Player player, FastRandom random)
			: base(player, random)
		{
			int num = 3;
			if (Depth <= 6f)
			{
				num = 2;
			}
			if (Depth <= 5f)
			{
				num = 1;
			}
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Crimera" + num, (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 3);
		}

		public override Color GetColor(Color backgroundColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[8];
		}
	}

	private class EOSSkyEntity : EOCSkyEntity
	{
		public EOSSkyEntity(Player player, FastRandom random)
			: base(player, random)
		{
			int num = 3;
			if (Depth <= 6f)
			{
				num = 2;
			}
			if (Depth <= 5f)
			{
				num = 1;
			}
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/EOS" + num, (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 4);
		}

		public override Color GetColor(Color backgroundColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[1];
		}
	}

	private class EOCSkyEntity : FadingSkyEntity
	{
		private const int STATE_ZIGZAG = 1;

		private const int STATE_GOOVERPLAYER = 2;

		private int _state;

		private int _direction;

		private float _waviness;

		public EOCSkyEntity(Player player, FastRandom random)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			VirtualCamera camera = new VirtualCamera(player);
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/EOC", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 3);
			Depth = random.NextFloat() * 3f + 4.5f;
			if (random.Next(4) != 0)
			{
				BeginZigZag(ref random, camera, (random.Next(2) == 1) ? 1 : (-1));
			}
			else
			{
				BeginChasingPlayer(ref random, camera);
			}
			SetPositionInWorldBasedOnScreenSpace(Position);
			OpacityNormalizedTimeToFadeIn = 0.1f;
			OpacityNormalizedTimeToFadeOut = 0.9f;
			BrightnessLerper = 0.2f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 5;
		}

		private void BeginZigZag(ref FastRandom random, VirtualCamera camera, int direction)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			_state = 1;
			LifeTime = random.Next(18, 31) * 60;
			_direction = direction;
			_waviness = random.NextFloat() * 1f + 1f;
			Position.Y = camera.Position.Y;
			int num = 100;
			if (_direction == 1)
			{
				Position.X = camera.Position.X - (float)num;
			}
			else
			{
				Position.X = camera.Position.X + camera.Size.X + (float)num;
			}
		}

		private void BeginChasingPlayer(ref FastRandom random, VirtualCamera camera)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			_state = 2;
			LifeTime = random.Next(18, 31) * 60;
			Position = camera.Position + camera.Size * new Vector2(random.NextFloat(), random.NextFloat());
		}

		public override void UpdateVelocity(int frameCount)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			switch (_state)
			{
			case 1:
				ZigzagMove(frameCount);
				break;
			case 2:
				ChasePlayerTop(frameCount);
				break;
			}
			Rotation = Velocity.ToRotation();
		}

		private void ZigzagMove(int frameCount)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			Velocity = new Vector2((float)(_direction * 3), (float)Math.Cos((float)frameCount / 1200f * ((float)Math.PI * 2f)) * _waviness);
		}

		private void ChasePlayerTop(int frameCount)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = Main.LocalPlayer.Center + new Vector2(0f, -500f) - Position;
			if (val.Length() >= 100f)
			{
				Velocity.X += 0.1f * (float)Math.Sign(val.X);
				Velocity.Y += 0.1f * (float)Math.Sign(val.Y);
				Velocity = Vector2.Clamp(Velocity, new Vector2(-18f), new Vector2(18f));
			}
		}
	}

	private class MeteorSkyEntity : FadingSkyEntity
	{
		public MeteorSkyEntity(Player player, FastRandom random)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			new VirtualCamera(player);
			Effects = (random.Next(2) != 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Depth = random.NextFloat() * 3f + 3f;
			Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Meteor", (AssetRequestMode)1);
			Frame = new SpriteFrame(1, 4);
			Vector2 val = ((float)Math.PI / 4f + random.NextFloat() * ((float)Math.PI / 2f)).ToRotationVector2();
			float num = (float)(Main.worldSurface * 16.0 - 0.0) / val.Y;
			float num2 = 1200f;
			float num3 = num / num2;
			Vector2 velocity = val * num3;
			Velocity = velocity;
			int num4 = 100;
			Vector2 position = player.Center + new Vector2((float)random.Next(-num4, num4 + 1), (float)random.Next(-num4, num4 + 1)) - Velocity * num2 * 0.5f;
			Position = position;
			LifeTime = (int)num2;
			OpacityNormalizedTimeToFadeIn = 0.05f;
			OpacityNormalizedTimeToFadeOut = 0.95f;
			BrightnessLerper = 0.5f;
			FinalOpacityMultiplier = 1f;
			FramingSpeed = 5;
			Rotation = Velocity.ToRotation() + (float)Math.PI / 2f;
		}
	}

	private delegate SkyEntity EntityFactoryMethod(Player player, int seed);

	private bool _isActive;

	private readonly SlotVector<SkyEntity> _entities = new SlotVector<SkyEntity>(500);

	private int _frameCounter;

	public override void Activate(Vector2 position, params object[] args)
	{
		_isActive = true;
	}

	public override void Deactivate(params object[] args)
	{
		_isActive = false;
	}

	private bool AnActiveSkyConflictsWithAmbience()
	{
		if (!SkyManager.Instance["MonolithMoonLord"].IsActive())
		{
			return SkyManager.Instance["MoonLord"].IsActive();
		}
		return true;
	}

	public override void Update(GameTime gameTime)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gamePaused)
		{
			return;
		}
		_frameCounter++;
		if (Main.netMode != 2 && AnActiveSkyConflictsWithAmbience() && SkyManager.Instance["Ambience"].IsActive())
		{
			SkyManager.Instance.Deactivate("Ambience");
		}
		foreach (var item in _entities)
		{
			SkyEntity value = item.Value;
			value.Update(_frameCounter);
			if (!value.IsActive)
			{
				_entities.Remove(item.Id);
				if (Main.netMode != 2 && _entities.Count == 0 && SkyManager.Instance["Ambience"].IsActive())
				{
					SkyManager.Instance.Deactivate("Ambience");
				}
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu && Main.netMode == 0 && SkyManager.Instance["Ambience"].IsActive())
		{
			_entities.Clear();
			SkyManager.Instance.Deactivate("Ambience");
		}
		foreach (var item in _entities)
		{
			item.Value.Draw(spriteBatch, 3f, minDepth, maxDepth);
		}
	}

	public override bool IsActive()
	{
		return _isActive;
	}

	public override void Reset()
	{
	}

	public void Spawn(Player player, SkyEntityType type, int seed)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		FastRandom random = new FastRandom(seed);
		switch (type)
		{
		case SkyEntityType.AirBalloon:
			_entities.Add((SkyEntity)new AirBalloonSkyEntity(player, random));
			break;
		case SkyEntityType.Airship:
			_entities.Add((SkyEntity)new AirshipSkyEntity(player, random));
			break;
		case SkyEntityType.BirdsV:
			_entities.Add((SkyEntity)new BirdsPackSkyEntity(player, random));
			break;
		case SkyEntityType.Eyeball:
			_entities.Add((SkyEntity)new EOCSkyEntity(player, random));
			break;
		case SkyEntityType.Meteor:
			_entities.Add((SkyEntity)new MeteorSkyEntity(player, random));
			break;
		case SkyEntityType.Wyvern:
			_entities.Add((SkyEntity)new WyvernSkyEntity(player, random));
			break;
		case SkyEntityType.Bats:
		{
			List<BatsGroupSkyEntity> list5 = BatsGroupSkyEntity.CreateGroup(player, random);
			for (int m = 0; m < list5.Count; m++)
			{
				_entities.Add((SkyEntity)list5[m]);
			}
			break;
		}
		case SkyEntityType.Butterflies:
			_entities.Add((SkyEntity)new ButterfliesSkyEntity(player, random));
			break;
		case SkyEntityType.LostKite:
			_entities.Add((SkyEntity)new LostKiteSkyEntity(player, random));
			break;
		case SkyEntityType.Vulture:
			_entities.Add((SkyEntity)new VultureSkyEntity(player, random));
			break;
		case SkyEntityType.PixiePosse:
			_entities.Add((SkyEntity)new PixiePosseSkyEntity(player, random));
			break;
		case SkyEntityType.Seagulls:
		{
			List<SeagullsGroupSkyEntity> list4 = SeagullsGroupSkyEntity.CreateGroup(player, random);
			for (int l = 0; l < list4.Count; l++)
			{
				_entities.Add((SkyEntity)list4[l]);
			}
			break;
		}
		case SkyEntityType.SlimeBalloons:
		{
			List<SlimeBalloonGroupSkyEntity> list3 = SlimeBalloonGroupSkyEntity.CreateGroup(player, random);
			for (int k = 0; k < list3.Count; k++)
			{
				_entities.Add((SkyEntity)list3[k]);
			}
			break;
		}
		case SkyEntityType.Gastropods:
		{
			List<GastropodGroupSkyEntity> list2 = GastropodGroupSkyEntity.CreateGroup(player, random);
			for (int j = 0; j < list2.Count; j++)
			{
				_entities.Add((SkyEntity)list2[j]);
			}
			break;
		}
		case SkyEntityType.Pegasus:
			_entities.Add((SkyEntity)new PegasusSkyEntity(player, random));
			break;
		case SkyEntityType.EaterOfSouls:
			_entities.Add((SkyEntity)new EOSSkyEntity(player, random));
			break;
		case SkyEntityType.Crimera:
			_entities.Add((SkyEntity)new CrimeraSkyEntity(player, random));
			break;
		case SkyEntityType.Hellbats:
		{
			List<HellBatsGoupSkyEntity> list = HellBatsGoupSkyEntity.CreateGroup(player, random);
			for (int i = 0; i < list.Count; i++)
			{
				_entities.Add((SkyEntity)list[i]);
			}
			break;
		}
		}
		if (Main.netMode != 2 && !AnActiveSkyConflictsWithAmbience() && !SkyManager.Instance["Ambience"].IsActive())
		{
			SkyManager.Instance.Activate("Ambience", default);
		}
	}
}
