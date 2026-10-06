using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Skies;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Drawing;

public class NextHorizonRenderer : IHorizonRenderer
{
	private static Asset<Texture2D>[] _sunriseTextures;

	private static Asset<Texture2D>[] _sunsetTextures;

	private static Asset<Texture2D> _sunflareGradientTexture;

	private static Asset<Texture2D> _sunflareGradientDitherTexture;

	private static Asset<Texture2D> _sunflarePointBlurryTexture;

	private static Asset<Texture2D> _sunflarePointSharpTexture;

	private static Asset<Texture2D> _bokehTexture;

	private static Asset<Texture2D> _spectraTexture;

	private static Asset<Texture2D> _sunflare1Texture;

	private static Asset<Texture2D> _sunflare2Texture;

	private List<DrawData> _drawData = new List<DrawData>(200);

	private void LoadTextures()
	{
		if (_sunriseTextures == null)
		{
			_sunriseTextures = new Asset<Texture2D>[4]
			{
				Main.Assets.Request<Texture2D>("Images/Misc/Sunrise/Sunrise_Blue", (AssetRequestMode)1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunrise/Sunrise_Violet", (AssetRequestMode)1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunrise/Sunrise_Yellow", (AssetRequestMode)1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunrise/Sunrise_Aluminum", (AssetRequestMode)1)
			};
			_sunsetTextures = new Asset<Texture2D>[4]
			{
				Main.Assets.Request<Texture2D>("Images/Misc/Sunset/Sunset_Blue", (AssetRequestMode)1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunset/Sunset_Dark", (AssetRequestMode)1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunset/Sunset_Pink", (AssetRequestMode)1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunset/Sunset_Red", (AssetRequestMode)1)
			};
			_sunflareGradientTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/colorgradient", (AssetRequestMode)1);
			_sunflareGradientDitherTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/colorgradientdither", (AssetRequestMode)1);
			_sunflarePointBlurryTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/Lens/PointBlurry", (AssetRequestMode)1);
			_sunflarePointSharpTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/Lens/PointSharp", (AssetRequestMode)1);
			_sunflare1Texture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/flare1", (AssetRequestMode)1);
			_sunflare2Texture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/flare2", (AssetRequestMode)1);
			_bokehTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/Lens/Flare1", (AssetRequestMode)1);
			_spectraTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/Lens/Flare2", (AssetRequestMode)1);
		}
	}

	private static Rectangle GetGradientRect()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		int num = 400;
		int val = (int)((1.0 - Utils.GetLerpValue(40.0, Main.worldSurface, Main.screenPosition.Y / 16f)) * (double)num);
		int num2 = Math.Max(0, val) - num;
		return new Rectangle(0, num2, Main.screenWidth, Main.screenHeight + num);
	}

	public void DrawHorizon()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.ShouldDrawSurfaceBackground())
		{
			return;
		}
		LoadTextures();
		int sunriseSunsetTextureIndex = GetSunriseSunsetTextureIndex();
		Asset<Texture2D> val = _sunriseTextures[sunriseSunsetTextureIndex % _sunriseTextures.Length];
		Asset<Texture2D> val2 = _sunsetTextures[sunriseSunsetTextureIndex % _sunsetTextures.Length];
		GetVisibilities(out var sunsetVisibility, out var sunriseVisibility, out var _);
		SpriteBatch spriteBatch = Main.spriteBatch;
		Rectangle gradientRect = GetGradientRect();
		foreach (BackgroundGradientDrawer backgroundDrawer in SunGradients.BackgroundDrawers)
		{
			backgroundDrawer.Draw();
		}
		if (sunriseVisibility != 0f)
		{
			spriteBatch.Draw(val.Value, gradientRect, Color.White * sunriseVisibility);
		}
		if (sunsetVisibility != 0f)
		{
			spriteBatch.Draw(val2.Value, gradientRect, Color.White * sunsetVisibility);
		}
	}

	public float GetMoonStrength()
	{
		return Utils.Remap(Math.Abs(4 - Main.moonPhase), 0f, 4f, 0f, 1f);
	}

	public void DrawSurfaceLayer(int layerIndex)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		if (Main.ShouldDrawSurfaceBackground())
		{
			LoadTextures();
			SpriteBatch spriteBatch = Main.spriteBatch;
			Rectangle gradientRect = GetGradientRect();
			GetVisibilities(out var sunsetVisibility, out var sunriseVisibility, out var _);
			int sunriseSunsetTextureIndex = GetSunriseSunsetTextureIndex();
			List<Color[]> sunrises = SunGradients.Sunrises;
			Color[] array = sunrises[sunriseSunsetTextureIndex % sunrises.Count];
			List<Color[]> sunsets = SunGradients.Sunsets;
			Color[] array2 = sunsets[sunriseSunsetTextureIndex % sunsets.Count];
			Color color = Color.Transparent;
			BlendColor(ref color, array2[0], sunsetVisibility);
			BlendColor(ref color, array[0], sunriseVisibility);
			float num = 1f;
			switch (layerIndex)
			{
			case 0:
				num = 1f;
				break;
			case 1:
				num = 0.75f;
				break;
			case 2:
				num = 0.5f;
				break;
			case 3:
				num = 0.5f;
				break;
			}
			_ = _sunriseTextures[sunriseSunsetTextureIndex % _sunriseTextures.Length];
			_ = _sunsetTextures[sunriseSunsetTextureIndex % _sunsetTextures.Length];
			_ = Main.tileBatch;
			if (layerIndex == 3)
			{
				float num2 = 0.6f;
				num = 1f;
				spriteBatch.Draw(_sunflareGradientTexture.Value, gradientRect, (Rectangle?)null, array[0] * num * sunriseVisibility * num2, 0f, Vector2.Zero, (SpriteEffects)1, 0f);
				spriteBatch.Draw(_sunflareGradientTexture.Value, gradientRect, (Rectangle?)null, array2[0] * num * sunsetVisibility * num2, 0f, Vector2.Zero, (SpriteEffects)0, 0f);
			}
		}
	}

	private int GetSunriseSunsetTextureIndex()
	{
		return Main.HorizonPhase;
	}

	public void ModifyHorizonLight(ref Color color)
	{
		if (Main.ShouldDrawSurfaceBackground())
		{
			GetVisibilities(out var sunsetVisibility, out var sunriseVisibility, out var _);
			int sunriseSunsetTextureIndex = GetSunriseSunsetTextureIndex();
			List<Color[]> sunrises = SunGradients.Sunrises;
			Color[] gradient = sunrises[sunriseSunsetTextureIndex % sunrises.Count];
			List<Color[]> sunsets = SunGradients.Sunsets;
			Color[] gradient2 = sunsets[sunriseSunsetTextureIndex % sunsets.Count];
			BlendColor(ref color, gradient2, sunsetVisibility);
			BlendColor(ref color, gradient, sunriseVisibility);
		}
	}

	public void DrawSun(Vector2 sunPosition)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		GetVisibilities(out var sunsetVisibility, out var sunriseVisibility, out var celestialVisibility);
		sunsetVisibility *= celestialVisibility;
		sunriseVisibility *= celestialVisibility;
		LoadTextures();
		Color val = new Color(255, 255, 255, 0);
		SpriteBatch spriteBatch = Main.spriteBatch;
		spriteBatch.Draw(_sunflare1Texture.Value, sunPosition, (Rectangle?)null, val * sunsetVisibility * 0.75f, 0f, _sunflare1Texture.Size() / 2f, 3f, (SpriteEffects)0, 0f);
		spriteBatch.Draw(_sunflare1Texture.Value, sunPosition, (Rectangle?)null, val * sunsetVisibility * 0.35f, 0f, _sunflare1Texture.Size() / 2f, 2f, (SpriteEffects)0, 0f);
		spriteBatch.Draw(_sunflare2Texture.Value, sunPosition, (Rectangle?)null, val * sunriseVisibility * 0.7f * 0.5f, 0f, _sunflare2Texture.Size() / 2f, 2f, (SpriteEffects)0, 0f);
		spriteBatch.Draw(_sunflare2Texture.Value, sunPosition, (Rectangle?)null, val * sunriseVisibility * 0.3f * 0.5f, 0f, _sunflare2Texture.Size() / 2f, 1.5f, (SpriteEffects)0, 0f);
		spriteBatch.Draw(_sunflare2Texture.Value, sunPosition, (Rectangle?)null, val * sunriseVisibility * 0.2f * 0.5f, 0f, _sunflare2Texture.Size() / 2f, 1f, (SpriteEffects)0, 0f);
	}

	private void BlendColor(ref Color color, Color[] gradient, float opacity)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		BlendColor(ref color, gradient[gradient.Length / 2], opacity);
	}

	private void BlendColor(ref Color color, Color colorToChoose, float opacity)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (!(opacity <= 0f))
		{
			Color val = new Color((int)Math.Max(color.R, colorToChoose.R), (int)Math.Max(color.G, colorToChoose.G), (int)Math.Max(color.B, colorToChoose.B), (int)Math.Max(color.A, colorToChoose.A));
			color = Color.Lerp(color, val, opacity);
		}
	}

	private static void GetVisibilities(out float sunsetVisibility, out float sunriseVisibility, out float celestialVisibility)
	{
		sunsetVisibility = 1f;
		sunriseVisibility = 1f;
		celestialVisibility = GetCelestialEffectPower();
		float num = 1f;
		num *= Main.atmo;
		float num2 = 1f - Main.cloudAlpha;
		num *= num2 * num2;
		num *= 1f - Main.SmoothedMushroomLightInfluence;
		sunriseVisibility *= num;
		sunsetVisibility *= num;
		double time = Main.time;
		double num3 = 54000.0;
		if (Main.dayTime)
		{
			float fromMin = 3600f;
			int num4 = 2700;
			float fromMax = 10800f;
			float num5 = -10800f;
			float num6 = -3600f;
			sunriseVisibility *= Utils.Remap((float)time, 0f, num4, 0f, 1f) * Utils.Remap((float)time, fromMin, fromMax, 1f, 0f);
			float num7 = Utils.Remap((float)time, (float)num3 + num5, (float)num3 + num6, 0f, 1f);
			float num8 = Utils.Remap((float)time, (float)num3 + num6, (float)num3, 1f, 0f);
			sunsetVisibility *= num7 * num8 * num8;
			if (Main.eclipse)
			{
				sunsetVisibility = 0f;
				sunriseVisibility = 0f;
			}
		}
		else
		{
			sunriseVisibility = 0f;
			sunsetVisibility = 0f;
		}
		if (Main.gameMenu && WorldGen.drunkWorldGen)
		{
			sunsetVisibility = (sunriseVisibility = 0f);
		}
	}

	public void CloudsStart()
	{
		_drawData.Clear();
	}

	public void DrawCloud(float globalCloudAlpha, Cloud theCloud, int cloudPass, float cY)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> val = TextureAssets.Cloud[theCloud.type];
		Vector2 position = new Vector2(theCloud.position.X, cY) + val.Size() / 2f;
		Color cloudColor = theCloud.cloudColor(Main.ColorOfTheSkies);
		OriginalColorsForCloud(theCloud, cloudPass, ref cloudColor);
		if (Main.atmo < 1f)
		{
			cloudColor *= Main.atmo;
		}
		_drawData.Add(new DrawData(val.Value, position, null, cloudColor * globalCloudAlpha, theCloud.rotation, val.Size() / 2f, theCloud.scale, theCloud.spriteDir));
	}

	private void OriginalColorsForCloud(Cloud theCloud, int cloudPass, ref Color cloudColor)
	{
		if (cloudPass == 1)
		{
			float num = theCloud.scale * 0.8f;
			float num2 = (theCloud.scale + 1f) / 2f * 0.9f;
			cloudColor.R = (byte)((float)(int)cloudColor.R * num);
			cloudColor.G = (byte)((float)(int)cloudColor.G * num2);
		}
	}

	private void BetterColorsForClouds(Cloud theCloud, int cloudPass, ref Vector2 cloudDrawPosition, ref Color cloudColor)
	{
		float num = 0f;
		switch (cloudPass)
		{
		case 1:
			num = 0.7f;
			break;
		case 2:
			num = 0.35f;
			break;
		}
		if (Main.keyState.IsKeyDown((Keys)160))
		{
			num = 0f;
		}
		if (num > 0f)
		{
			GetVisibilities(out var sunsetVisibility, out var sunriseVisibility, out var _);
			int sunriseSunsetTextureIndex = GetSunriseSunsetTextureIndex();
			List<Color[]> sunrises = SunGradients.Sunrises;
			Color[] gradient = sunrises[sunriseSunsetTextureIndex % sunrises.Count];
			List<Color[]> sunsets = SunGradients.Sunsets;
			Color[] gradient2 = sunsets[sunriseSunsetTextureIndex % sunsets.Count];
			float normalizedScreenHeight = cloudDrawPosition.Y / (float)Main.screenHeight;
			float alpha = theCloud.Alpha;
			BlendColorAlongGradientBasedOnHeight(ref cloudColor, sunsetVisibility, normalizedScreenHeight, gradient2, alpha);
			BlendColorAlongGradientBasedOnHeight(ref cloudColor, sunriseVisibility, normalizedScreenHeight, gradient, alpha);
		}
	}

	private void BlendColorAlongGradientBasedOnHeight(ref Color color, float visibility, float normalizedScreenHeight, Color[] gradient, float opacity)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		float num = MathHelper.Clamp(normalizedScreenHeight * (float)gradient.Length, 0f, (float)(gradient.Length - 1));
		float num2 = num % 1f;
		int num3 = (int)Math.Floor(num);
		if (num2 == 0f || num3 == gradient.Length - 1)
		{
			BlendColor(ref color, gradient[num3] * opacity, visibility);
			return;
		}
		Color colorToChoose = Color.Lerp(gradient[num3], gradient[num3 + 1], num2) * opacity;
		BlendColor(ref color, colorToChoose, visibility);
	}

	private static float GetCelestialEffectPower()
	{
		float num = 1800f;
		float num2 = 1800f;
		float toMax = 0f;
		if (Main.dayTime)
		{
			return Utils.Remap((float)Main.time, 0f, num * 2f, 0f, 1f) * Utils.Remap((float)Main.time, 54000f - num, 54000f, 1f, toMax);
		}
		return Utils.Remap((float)Main.time, 0f, num2 * 2f, 0f, 1f) * Utils.Remap((float)Main.time, 32400f - num2, 32400f, 1f, 0f);
	}

	public void CloudsEnd()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		if (_drawData.Count == 0)
		{
			return;
		}
		Main.spriteBatch.End();
		SpriteDrawBuffer spriteBuffer = Main.spriteBuffer;
		foreach (DrawData drawDatum in _drawData)
		{
			drawDatum.Draw(spriteBuffer);
		}
		MiscShaderData miscShaderData = GameShaders.Misc["HorizonClouds"];
		miscShaderData.UseSpriteTransformMatrix(Main.LatestSurfaceBackgroundBeginner.transformMatrix);
		HorizonHelper.GetCelestialBodyColors(out var sunColor, out var moonColor);
		Color tileColor = (Main.dayTime ? sunColor : moonColor);
		AuroraSky.ModifyTileColor(ref tileColor, 1f);
		miscShaderData.UseColor(tileColor);
		Vector2 celestialBodyPosition = GetCelestialBodyPosition();
		GetVisibilities(out var sunsetVisibility, out var sunriseVisibility, out var celestialVisibility);
		float num = Math.Max(sunsetVisibility, sunriseVisibility) * celestialVisibility;
		if (!Main.dayTime)
		{
			num = Math.Max(num, celestialVisibility * 0.15f);
		}
		num *= Utils.Clamp(1f - Main.cloudBGAlpha, 0f, 1f);
		miscShaderData.UseShaderSpecificData(new Vector4(celestialBodyPosition.X, celestialBodyPosition.Y, num, 0f));
		for (int i = 0; i < _drawData.Count; i++)
		{
			miscShaderData.Apply(_drawData[i]);
			spriteBuffer.DrawSingle(i);
		}
		spriteBuffer.Unbind();
		Main.LatestSurfaceBackgroundBeginner.Begin(Main.spriteBatch);
	}

	private static Vector2 GetCelestialBodyPosition()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return Main.LastCelestialBodyPosition * Main.ScreenSize.ToVector2();
	}

	public void DrawLensFlare()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		if (Main.ShouldDrawSurfaceBackground() && Main.HorizonHelper.SunVisibilityEnabled)
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			Vector2 celestialBodyPosition = GetCelestialBodyPosition();
			Vector2 screenCenter = Main.ScreenSize.ToVector2() / 2f;
			GetVisibilities(out var sunsetVisibility, out var sunriseVisibility, out var celestialVisibility);
			float num = AdjustIntensity(sunriseVisibility, celestialVisibility);
			float num2 = AdjustIntensity(sunsetVisibility, celestialVisibility);
			if (!((double)num <= 0.01) || !((double)num2 <= 0.01))
			{
				Main.LatestSurfaceBackgroundBeginner.Begin(spriteBatch, (SpriteSortMode)1);
				EffectPass val = Main.pixelShader.CurrentTechnique.Passes[0];
				MiscShaderData miscShaderData = GameShaders.Misc["LensFlare"];
				miscShaderData.UseImage1((Texture)(object)Main.HorizonHelper.SunVisibilityPixelTexture);
				miscShaderData.Apply();
				DrawSunriseFlare(spriteBatch, celestialBodyPosition, screenCenter, num);
				DrawSunsetFlare(spriteBatch, celestialBodyPosition, screenCenter, num2);
				spriteBatch.End();
				val.Apply();
			}
		}
	}

	private float AdjustIntensity(float temporalIntensity, float celestialVisibility)
	{
		float num = temporalIntensity;
		num *= celestialVisibility;
		num *= num * num;
		int sunScorchCounter = Main.SceneMetrics.PerspectivePlayer.sunScorchCounter;
		if (sunScorchCounter > 0)
		{
			float lerpValue = Utils.GetLerpValue(0f, 300f, sunScorchCounter, clamped: true);
			lerpValue = 1f - lerpValue;
			num = 1f - lerpValue * lerpValue;
			num *= celestialVisibility;
			num *= 5f;
		}
		return num;
	}

	private void DrawSunsetFlare(SpriteBatch spriteBatch, Vector2 sunPosition, Vector2 screenCenter, float intensity)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		if (!(intensity <= 0.01f))
		{
			LoadTextures();
			LensFlareElement lensFlareElement = default;
			lensFlareElement.Texture = _sunflarePointBlurryTexture;
			lensFlareElement.RepeatTimes = 3;
			lensFlareElement.DistanceStart = 0.33f;
			lensFlareElement.DistanceAlongIndex = 0.05f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.ScaleOverIndex = -0.04f;
			lensFlareElement.Color = new Color(43, 32, 0, 0) * (120f / 255f);
			lensFlareElement.IntensityOverIndex = -0.125f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _sunflarePointSharpTexture;
			lensFlareElement.RepeatTimes = 3;
			lensFlareElement.DistanceStart = 0.03f;
			lensFlareElement.DistanceAlongIndex = 0.05f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.ScaleOverIndex = 0.04f;
			lensFlareElement.Color = new Color(43, 32, 0, 0) * (120f / 255f);
			lensFlareElement.IntensityOverIndex = -0.125f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _sunflarePointBlurryTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.41f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.Color = new Color(255, 0, 65, 0) * (30f / 255f);
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _bokehTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.475f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * (40f / 255f);
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _bokehTexture;
			lensFlareElement.RepeatTimes = 6;
			lensFlareElement.DistanceStart = 0.225f;
			lensFlareElement.DistanceAlongIndex = 0.04f;
			lensFlareElement.ScaleStart = 0.24f;
			lensFlareElement.ScaleOverIndex = -0.04f;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * (20f / 255f);
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _sunflarePointBlurryTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.6f;
			lensFlareElement.ScaleStart = 1f;
			lensFlareElement.Color = new Color(255, 157, 0, 0) * (40f / 255f);
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _spectraTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.65f;
			lensFlareElement.ScaleStart = 0.4f;
			lensFlareElement.Rotation = (float)Math.PI;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * (10f / 255f);
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
		}
	}

	private void DrawSunriseFlare(SpriteBatch spriteBatch, Vector2 sunPosition, Vector2 screenCenter, float intensity)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		if (!(intensity <= 0.01f))
		{
			LoadTextures();
			LensFlareElement lensFlareElement = default;
			lensFlareElement.Texture = _sunflarePointSharpTexture;
			lensFlareElement.RepeatTimes = 3;
			lensFlareElement.DistanceStart = 0.33f;
			lensFlareElement.DistanceAlongIndex = 0.05f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.ScaleOverIndex = -0.04f;
			lensFlareElement.Color = new Color(0, 32, 43, 0) * (120f / 255f);
			lensFlareElement.IntensityOverIndex = -0.125f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _sunflarePointSharpTexture;
			lensFlareElement.RepeatTimes = 3;
			lensFlareElement.DistanceStart = 0.03f;
			lensFlareElement.DistanceAlongIndex = 0.05f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.ScaleOverIndex = 0.04f;
			lensFlareElement.Color = new Color(0, 32, 43, 0) * (120f / 255f);
			lensFlareElement.IntensityOverIndex = -0.125f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _sunflarePointBlurryTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.41f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.Color = new Color(65, 0, 255, 0) * (30f / 255f);
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _bokehTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.525f;
			lensFlareElement.Rotation = 0.01f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * (40f / 255f);
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _bokehTexture;
			lensFlareElement.RepeatTimes = 6;
			lensFlareElement.DistanceStart = 0.225f;
			lensFlareElement.DistanceAlongIndex = 0.04f;
			lensFlareElement.ScaleStart = 0.24f;
			lensFlareElement.ScaleOverIndex = -0.04f;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * (20f / 255f);
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _sunflarePointBlurryTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.6f;
			lensFlareElement.ScaleStart = 1f;
			lensFlareElement.Color = new Color(0, 157, 255, 0) * (40f / 255f);
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default;
			lensFlareElement.Texture = _spectraTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.65f;
			lensFlareElement.ScaleStart = 0.38f;
			lensFlareElement.Rotation = (float)Math.PI;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * (10f / 255f);
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
		}
	}
}
