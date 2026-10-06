using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class AuroraSky : CustomSky
{
	private delegate void ScriptMethodSignature(VertexStrip vertexStrip, float skyOpacity, ref Color lastSkyColor);

	private UnifiedRandom _random = new UnifiedRandom();

	private bool _isActive;

	private bool _isLeaving;

	private float _opacity;

	private VertexStrip vertexStrip = new VertexStrip();

	private Color _lastSkyColor;

	public override void OnLoad()
	{
	}

	public override void Update(GameTime gameTime)
	{
		if (FocusHelper.PauseSkies)
		{
			return;
		}
		if (_isLeaving)
		{
			_opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
			if (_opacity < 0f)
			{
				_isActive = false;
				_opacity = 0f;
			}
		}
		else
		{
			_opacity += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.3f;
			if (_opacity > 1f)
			{
				_opacity = 1f;
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		if (maxDepth == float.MaxValue)
		{
			DrawAuroraSky(vertexStrip, _opacity, ref _lastSkyColor);
		}
	}

	private static void DrawAuroraSky(VertexStrip vertexStrip, float skyOpacity, ref Color lastSkyColor)
	{
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e44: Unknown result type (might be due to invalid IL or missing references)
		MiscShaderData miscShaderData = GameShaders.Misc["Aurora"];
		float num = (Main.dayTime ? 54000f : 32400f);
		float fromValue = (float)Main.time;
		skyOpacity *= Utils.Remap(fromValue, 0f, 180f, 0f, 1f) * Utils.Remap(fromValue, num - 180f, num, 1f, 0f);
		if (skyOpacity <= 0.01f || Main.dayTime)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		int num2 = 1;
		float num3 = 1f;
		float num4 = 1f;
		bool flag5 = false;
		float saturation = 1f;
		switch (Main.GetMoonPhase())
		{
		case MoonPhase.Full:
			flag = true;
			num2 = 3;
			break;
		case MoonPhase.ThreeQuartersAtLeft:
			num2 = 2;
			flag5 = true;
			break;
		case MoonPhase.HalfAtLeft:
			flag2 = true;
			flag3 = true;
			num2 = 3;
			flag4 = true;
			num4 *= 0.5f;
			break;
		case MoonPhase.QuarterAtLeft:
			return;
		case MoonPhase.Empty:
			flag2 = true;
			num2 = 3;
			break;
		case MoonPhase.QuarterAtRight:
			num2 = 2;
			flag5 = true;
			saturation = 0.5f;
			break;
		case MoonPhase.HalfAtRight:
			return;
		case MoonPhase.ThreeQuartersAtRight:
			flag2 = true;
			flag3 = true;
			num2 = 3;
			flag4 = true;
			num4 *= 0.5f;
			saturation = 0.5f;
			break;
		}
		PlayerInput.SetZoom_Background();
		Main.spriteBatch.End();
		Vector2 val = new Vector2(1920f, 1080f);
		float num5 = (float)Main.ScreenSize.X / val.X;
		miscShaderData.UseSpriteTransformMatrix(Main.LatestSurfaceBackgroundBeginner.transformMatrix * Matrix.CreateScale(num5));
		Vector2 lastCelestialBodyPosition = Main.LastCelestialBodyPosition;
		lastCelestialBodyPosition.Y *= val.X / val.Y / ((float)Main.ScreenSize.X / (float)Main.ScreenSize.Y);
		float num6 = Main.GlobalTimeWrappedHourly / 60f;
		for (int i = 0; i < num2; i++)
		{
			vertexStrip.Reset();
			int num7 = 140;
			float num8 = 2.5f;
			float num9 = 0f;
			float luminosity = 1f;
			Vector4 specificData = new Vector4(0f, 0f, 0f, 0f);
			if (i == 0)
			{
				specificData.Y = 0f;
			}
			if (i == 1)
			{
				specificData.Y = 0.7f;
			}
			if (i == 2)
			{
				specificData.Y = 0.8f;
			}
			if (flag4)
			{
				luminosity = 1f;
				specificData.X = 0.3f;
			}
			if (flag2)
			{
				num8 = 1f;
				num9 += 0.33f;
				if (i != 0)
				{
					specificData.Y = 0.4f + (float)i * 0.2f;
				}
				if (!flag3)
				{
					specificData.Z = 0.2f;
				}
			}
			if (flag && i != 0)
			{
				specificData.Y = 0.4f;
			}
			if (flag5 && i == 0)
			{
				specificData.Y = 0.3f;
			}
			if (flag5 && i == 1)
			{
				specificData.Y = 0.5f;
			}
			if (flag && i == 0)
			{
				specificData.Y = 0.5f;
			}
			if (flag2 && i == 0)
			{
				specificData.Y = 0.7f;
			}
			miscShaderData.UseShaderSpecificData(specificData);
			for (int num10 = num7; num10 >= 0; num10--)
			{
				float num11 = (float)num10 / (float)num7;
				float num12 = num11;
				if (flag5 && i == 1)
				{
					num11 = Utils.Remap(num11, 0f, 1f, 50f / (float)num7, 90f / (float)num7);
				}
				float num13 = num11;
				if (!flag)
				{
					num13 = 1f - num11;
				}
				float num14 = MathHelper.Lerp(0.4f, 0.1f, num11);
				float num15 = 0.4f + num6;
				float num16 = 3f;
				float num17 = 0.5f + (float)Math.Cos((double)num11 * Math.PI * (double)num16 + (double)num15) * 0.4f * MathHelper.Lerp(1f, 0.3f, num13);
				float num18 = Utils.Remap(Math.Abs((float)Math.Sin((double)num11 * Math.PI * (double)num16 + (double)num15)), 0f, 0.98f, 0f, 1f);
				float num19 = MathHelper.Lerp(0.2f, 0.05f, num13) * num3;
				float num20 = 0.5f - 0.5f * (float)Math.Cos(num11 * ((float)Math.PI * 2f));
				float num21 = num6;
				if (flag5)
				{
					float num22 = num6 * 0.16f;
					if (i == 1)
					{
						Utils.Remap(num11, 0f, 1f, 50f / (float)num7, 90f / (float)num7);
					}
					num14 += (1f - num11) * 0.05f;
					num19 += 0.05f;
					if (i == 1)
					{
						num14 = 0.5f + (float)Math.Cos(num22 * ((float)Math.PI * 2f) * 0.15f + num11 * 60f) * 0.03f;
						float num23 = num11 + num22;
						num17 = 0.5f + (float)Math.Cos((double)num23 * Math.PI * 2.0) * 1.4f * MathHelper.Lerp(1f, 0.3f, num11);
						num17 += (float)Math.Sin(num22 * ((float)Math.PI * 2f)) * MathHelper.Lerp(0.4f, 0.13f, num11);
						num14 -= (float)Math.Cos(num22 * ((float)Math.PI * 2f) * 3f + num11 * 5f) * 0.06f;
						num19 += 0.15f;
						num17 = num12 * 1.1f;
						num18 = 1f - (float)Math.Sin(num12 * ((float)Math.PI * 2f) * 2f + (float)Math.PI / 2f) * 0.35f - 0.35f;
						num14 = (float)Math.Sin(num12 * ((float)Math.PI * 2f) * 2f + (float)Math.PI / 2f) * 0.0125f + 0.55f;
						num19 = 0.16f * num3 + 0.05f + (float)Math.Sin(num12 * ((float)Math.PI * 2f) * 2f) * 0.025f;
						num19 += 0.2f;
					}
					if (i == 0)
					{
						float num24 = Utils.Remap(num11, 0f, 0.3f, 0f, 1f);
						num20 *= num24 * num24 * num24;
						num19 -= 0.1f;
						num19 += 0.8f * num11 * num11;
					}
				}
				if (flag && i == 0)
				{
					float num25 = num6 * 0.16f;
					num14 = 0.5f + (float)Math.Cos(num25 * ((float)Math.PI * 2f) * 0.15f + num11 * 60f) * 0.03f;
					float num26 = num11 + num25;
					num17 = 0.5f + (float)Math.Cos((double)num26 * Math.PI * 2.0) * 1.4f * MathHelper.Lerp(1f, 0.3f, num11);
					num17 += (float)Math.Sin(num25 * ((float)Math.PI * 2f)) * MathHelper.Lerp(0.4f, 0.13f, num11);
					num19 += (float)(Math.Sin(num25 * ((float)Math.PI * 2f)) + 1.0) * MathHelper.Lerp(0.24f, 0.15f, num11) * num3;
					num14 -= (float)Math.Cos(num25 * ((float)Math.PI * 2f) * 3f + num11 * 5f) * 0.06f;
					num17 = num12 * 1.1f;
					num14 = (float)Math.Sin(num12 * ((float)Math.PI * 2f) * 2f + (float)Math.PI / 2f + num6 * 2f + (float)Math.PI) * 0.025f + 0.55f;
					num19 = 0.16f * num3 + 0.05f + (float)Math.Sin(num12 * ((float)Math.PI * 2f) * 2f + num6 * 2f) * 0.02f;
					num18 = 1f - (float)Math.Sin(num12 * ((float)Math.PI * 2f) * 2f + (float)Math.PI / 2f) * 0.35f - 0.35f;
				}
				if (flag2)
				{
					float num27 = num6 * 0.16f;
					if (i == 0)
					{
						num14 = 0.5f + (float)Math.Cos(num27 * ((float)Math.PI * 2f) * 0.15f + num11 * 60f) * 0.03f;
						float num28 = num11 + num27;
						num17 = 0.5f + (float)Math.Cos((double)num28 * Math.PI * 2.0) * 1.4f * MathHelper.Lerp(1f, 0.3f, num11);
						num17 += (float)Math.Sin(num27 * ((float)Math.PI * 2f)) * MathHelper.Lerp(0.4f, 0.13f, num11);
						num14 -= (float)Math.Cos(num27 * ((float)Math.PI * 2f) * 3f + num11 * 5f) * 0.06f;
						num19 += 0.15f;
						num17 = num12 * 1.1f;
						num18 = 1f - (float)Math.Sin(num12 * ((float)Math.PI * 2f) * 2f + (float)Math.PI / 2f) * 0.35f - 0.35f;
						num14 = (float)Math.Sin(num12 * ((float)Math.PI * 2f) * 2f + (float)Math.PI / 2f) * 0.025f + 0.55f;
						num19 = 0.16f * num3 + 0.05f + (float)Math.Sin(num12 * ((float)Math.PI * 2f) * 2f) * 0.05f;
					}
					else
					{
						_ = 1;
						_ = 1;
						_ = 2;
						if (i == 1 || i == 2)
						{
							num14 = MathHelper.Lerp(0.3f, 0.3f, num11);
							Math.Sin(num6 * ((float)Math.PI * 2f));
							float value = (float)Math.Cos(num6 * ((float)Math.PI * 2f));
							if (i == 1)
							{
								num19 += 0.5f * num11;
							}
							num14 -= (float)Math.Cos(num11 * ((float)Math.PI * 2f) + num6 * 2f) * 0.07f;
							num18 = Utils.Remap(Math.Abs(value), 0f, 0.98f, 0f, 1f);
							num18 = 1f;
							num17 = num11;
							num21 += 0.35f;
							if (!flag3)
							{
								num21 -= 0.35f;
							}
							num20 *= 0.55f;
							if (i == 2)
							{
								Math.Sin(num6 * ((float)Math.PI * 2f));
								Math.Cos(num6 * ((float)Math.PI * 2f));
								num14 -= (float)Math.Cos(num6 * ((float)Math.PI * 2f) * 0.35f + num11 * 13.73f) * 0.04f * (1f - num11) + 0.04f;
								num14 -= 0.03f;
							}
						}
						else
						{
							switch (i)
							{
							case 1:
							{
								num14 = MathHelper.Lerp(0.4f, 0.1f, num11);
								Math.Sin(num6 * ((float)Math.PI * 2f));
								float value3 = (float)Math.Cos(num6 * ((float)Math.PI * 2f));
								num14 -= (float)Math.Cos(num11 * ((float)Math.PI * 2f) + num6 * 2f) * 0.07f;
								num18 = Utils.Remap(Math.Abs(value3), 0f, 0.98f, 0f, 1f);
								num18 = 1f;
								num17 = num11;
								num21 += 0.35f;
								num20 *= 0.55f;
								break;
							}
							case 2:
							{
								num14 = MathHelper.Lerp(0.1f, 0.4f, num11);
								Math.Sin(num6 * ((float)Math.PI * 2f));
								float value2 = (float)Math.Cos(num6 * ((float)Math.PI * 2f));
								num14 -= (float)Math.Cos(num6 * ((float)Math.PI * 2f) * 0.35f) * 0.15f * (1f - num11);
								num21 += 0.35f;
								num18 = Utils.Remap(Math.Abs(value2), 0f, 0.98f, 0f, 1f);
								num18 = 1f;
								num17 = num11;
								break;
							}
							}
						}
					}
				}
				if (flag3)
				{
					num21 = num6 + (float)i * 0.05f;
					num8 = 0.5f;
					num9 = 0.02f;
				}
				if (flag2 && !flag3)
				{
					luminosity = 1f;
					num9 = 0.45f;
				}
				if (flag && i != 0)
				{
					num20 = Math.Max(num20 * 2f, num11);
					if (num20 > 1f)
					{
						num20 = 1f;
					}
					num17 = MathHelper.Lerp(num17, lastCelestialBodyPosition.X, num11);
					num14 += 0.05f;
					num14 = MathHelper.Lerp(num14, lastCelestialBodyPosition.Y + 0.025f, num11);
					num20 *= 0.5f;
				}
				Vector2 val2 = val * new Vector2(num17, num14);
				Vector2 val3 = val * new Vector2(num17, num14 - num19);
				if (!flag)
				{
					float num29 = Main.GlobalTimeWrappedHourly * 0.1f;
					val2 += ((num29 + 0.3f) * ((float)Math.PI * 2f)).ToRotationVector2() * 2f;
					val3 += ((num29 * 0.8f + 0.67f) * ((float)Math.PI * 2f)).ToRotationVector2() * 2f;
					val3.Y += (float)Math.Sin((num29 + num11) * ((float)Math.PI * 2f) * 3f) * 15f - 15f;
					val2.Y += (float)Math.Sin((num29 + num11) * ((float)Math.PI * 2f) * 0.5f) * 1f;
					val3.Y += (float)Math.Sin((num29 + num11) * ((float)Math.PI * 2f) * 0.5f) * 1f;
					val2.X += (float)Math.Sin((num29 + num11) * ((float)Math.PI * 2f) * 1f) * 3f;
					val3.X += (float)Math.Sin((num29 + num11) * ((float)Math.PI * 2f) * 0.75f) * 3f;
				}
				Color val4 = Main.hslToRgb((float)((double)num21 + Math.Cos(num11 * ((float)Math.PI * 2f) * num8) * 0.1) % 1f, saturation, 0.5f);
				Color val5 = Main.hslToRgb((float)((double)num21 + Math.Cos(num11 * ((float)Math.PI * 2f) * num8) * 0.1 + (double)num9) % 1f, saturation, luminosity);
				if (i == 0 && num10 == 19)
				{
					lastSkyColor = val4;
				}
				float num30 = num18 * skyOpacity * num20 * num4;
				if (flag)
				{
					float fromValue2 = (val * new Vector2(num17, num14 - num19 * 0.25f)).Distance(val * lastCelestialBodyPosition);
					num30 *= Utils.Remap(fromValue2, 29f, 60f, 0f, 1f);
					float num31 = 505f;
					float num32 = 1f - num11;
					num32 *= num32 * num32;
					if (i == 1)
					{
						val2.X -= num31 * num32;
						val3.X -= num31 * num32;
						num30 -= num11 * num11 * 0.36f;
					}
					if (i == 2)
					{
						val2.X += num31 * num32;
						val3.X += num31 * num32;
						num30 -= num11 * num11 * 0.36f;
					}
				}
				vertexStrip.AddVertexPair(val2, val3, num11, val4 * num30, val5 * num30);
			}
			miscShaderData.Apply();
			vertexStrip.PrepareIndices(includeBacksides: true);
			vertexStrip.DrawTrail();
		}
		Main.LatestSurfaceBackgroundBeginner.Begin(Main.spriteBatch);
	}

	public static void ModifyTileColor(ref Color tileColor, float intensity)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		if (SkyManager.Instance["Aurora"] is AuroraSky { _opacity: var opacity } auroraSky && !(opacity <= 0f))
		{
			MoonPhase moonPhase = Main.GetMoonPhase();
			if (moonPhase != MoonPhase.QuarterAtLeft)
			{
				Color lastSkyColor = auroraSky._lastSkyColor;
				lastSkyColor.A = byte.MaxValue;
				tileColor = Color.Lerp(tileColor, lastSkyColor, opacity * intensity);
			}
		}
	}

	public override void Activate(Vector2 position, params object[] args)
	{
		_isActive = true;
		_isLeaving = false;
	}

	public override void Deactivate(params object[] args)
	{
		_isLeaving = true;
	}

	public override void Reset()
	{
		_opacity = 0f;
		_isActive = false;
	}

	public override bool IsActive()
	{
		return _isActive;
	}
}
