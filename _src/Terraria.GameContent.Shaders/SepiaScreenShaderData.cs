using Microsoft.Xna.Framework;
using Terraria.Enums;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders;

public class SepiaScreenShaderData : ScreenShaderData
{
	public SepiaScreenShaderData(string passName)
		: base(passName)
	{
	}

	public override void Update(GameTime gameTime)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		float x = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
		float num = 1f - Utils.SmoothStep((float)Main.worldSurface, (float)Main.worldSurface + 30f, x);
		Vector3 val2;
		Vector3 val = (val2 = new Vector3(0.191f, -0.054f, -0.221f));
		Vector3 val3 = val * 0.5f;
		Vector3 val4 = new Vector3(0f, -0.03f, 0.15f);
		Vector3 val5 = new Vector3(-0.11f, 0.01f, 0.16f);
		float cloudAlpha = Main.cloudAlpha;
		GetDaylightPowers(out var nightlightPower, out var daylightPower, out var moonPower, out var dawnPower);
		float num2 = nightlightPower * 0.13f;
		if (Main.starGame)
		{
			float num3 = (float)Main.starGameMath() - 1f;
			nightlightPower = num3;
			daylightPower = 1f - num3;
			moonPower = num3;
			dawnPower = 1f - num3;
			num2 = nightlightPower * 0.13f;
		}
		else if (!Main.dayTime)
		{
			if (Main.GetMoonPhase() == MoonPhase.Full)
			{
				val2 = new Vector3(-0.19f, 0.01f, 0.22f);
				num2 += 0.07f * moonPower;
			}
			if (Main.bloodMoon)
			{
				val2 = new Vector3(0.2f, -0.1f, -0.221f);
				num2 = 0.2f;
			}
		}
		nightlightPower *= num;
		daylightPower *= num;
		moonPower *= num;
		dawnPower *= num;
		UseOpacity(1f);
		UseIntensity(1.4f - daylightPower * 0.2f);
		float num4 = 0.3f - num2 * nightlightPower;
		num4 = MathHelper.Lerp(num4, 0.1f, cloudAlpha);
		float num5 = 0.2f;
		num4 = MathHelper.Lerp(num4, num5, 1f - num);
		UseProgress(num4);
		Vector3 val6 = Vector3.Lerp(val, val2, moonPower);
		val6 = Vector3.Lerp(val6, val4, dawnPower);
		val6 = Vector3.Lerp(val6, val5, cloudAlpha);
		val6 = Vector3.Lerp(val6, val3, 1f - num);
		UseColor(val6);
	}

	private static void GetDaylightPowers(out float nightlightPower, out float daylightPower, out float moonPower, out float dawnPower)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		nightlightPower = 0f;
		daylightPower = 0f;
		moonPower = 0f;
		Vector2 dayTimeAsDirectionIn24HClock = Utils.GetDayTimeAsDirectionIn24HClock();
		Vector2 dayTimeAsDirectionIn24HClock2 = Utils.GetDayTimeAsDirectionIn24HClock(4.5f);
		Vector2 dayTimeAsDirectionIn24HClock3 = Utils.GetDayTimeAsDirectionIn24HClock(0f);
		float fromValue = Vector2.Dot(dayTimeAsDirectionIn24HClock, dayTimeAsDirectionIn24HClock3);
		float fromValue2 = Vector2.Dot(dayTimeAsDirectionIn24HClock, dayTimeAsDirectionIn24HClock2);
		nightlightPower = Utils.Remap(fromValue, -0.2f, 0.1f, 0f, 1f);
		daylightPower = Utils.Remap(fromValue, 0.1f, -1f, 0f, 1f);
		dawnPower = Utils.Remap(fromValue2, 0.66f, 1f, 0f, 1f);
		if (!Main.dayTime)
		{
			float num = (float)(Main.time / 32400.0) * 2f;
			if (num > 1f)
			{
				num = 2f - num;
			}
			moonPower = Utils.Remap(num, 0f, 0.25f, 0f, 1f);
		}
	}
}
