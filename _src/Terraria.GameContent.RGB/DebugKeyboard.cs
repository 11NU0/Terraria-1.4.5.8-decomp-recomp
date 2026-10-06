using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

internal class DebugKeyboard : RgbDevice
{
	private DebugKeyboard(Fragment fragment)
		: base((RgbDeviceVendor)4, (RgbDeviceType)6, fragment, new DeviceColorProfile())
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected Obj, but got Unknown
	}

	public static DebugKeyboard Create()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		int num = 400;
		int num2 = 100;
		Point[] array = new Point[num * num2];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				array[i * num + j] = new Point(j / 10, i / 10);
			}
		}
		Vector2[] array2 = new Vector2[num * num2];
		for (int k = 0; k < num2; k++)
		{
			for (int l = 0; l < num; l++)
			{
				array2[k * num + l] = new Vector2((float)l / (float)num2, (float)k / (float)num2);
			}
		}
		return new DebugKeyboard(Fragment.FromCustom(array, array2));
	}

	public override void Present()
	{
	}

	public override void DebugDraw(IDebugDrawer drawer, Vector2 position, float scale)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < ((RgbDevice)this).LedCount; i++)
		{
			Vector2 ledCanvasPosition = ((RgbDevice)this).GetLedCanvasPosition(i);
			drawer.DrawSquare(new Vector4(ledCanvasPosition * scale + position, scale / 100f, scale / 100f), new Color(base.GetUnprocessedLedColor(i)));
		}
	}
}
