using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace Terraria.ID;

public static class TorchID
{
	public class Sets
	{
		public static SetFactory Factory = new SetFactory(Count);

		public static bool[] IsABiomeTorch = Factory.CreateBoolSet(false, 0, 18, 19, 20, 21, 23, 13, 7, 9, 22, 16);
	}

	private interface ITorchLightProvider
	{
		void GetRGB(out float r, out float g, out float b);
	}

	private struct ConstantTorchLight(float Red, float Green, float Blue) : ITorchLightProvider
	{
		public float R = Red;

		public float G = Green;

		public float B = Blue;

		public void GetRGB(out float r, out float g, out float b)
		{
			r = R;
			g = G;
			b = B;
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct DemonTorchLight : ITorchLightProvider
	{
		public void GetRGB(out float r, out float g, out float b)
		{
			r = 0.5f * Main.demonTorch + (1f - Main.demonTorch);
			g = 0.3f;
			b = Main.demonTorch + 0.5f * (1f - Main.demonTorch);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct ShimmerTorchLight : ITorchLightProvider
	{
		public void GetRGB(out float r, out float g, out float b)
		{
			float num = 0.9f;
			float num2 = 0.9f;
			num += (float)(270 - Main.mouseTextColor) / 900f;
			num2 += (float)(270 - Main.mouseTextColor) / 125f;
			num = MathHelper.Clamp(num, 0f, 1f);
			num2 = MathHelper.Clamp(num2, 0f, 1f);
			r = num * 0.9f;
			g = num2 * 0.55f;
			b = num * 1.2f;
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct DiscoTorchLight : ITorchLightProvider
	{
		public void GetRGB(out float r, out float g, out float b)
		{
			r = (float)Main.DiscoR / 255f;
			g = (float)Main.DiscoG / 255f;
			b = (float)Main.DiscoB / 255f;
		}
	}

	public static int[] Dust = new int[24]
	{
		6, 59, 60, 61, 62, 63, 64, 65, 75, 135,
		158, 169, 156, 234, 66, 242, 293, 294, 295, 296,
		297, 298, 307, 310
	};

	private static ITorchLightProvider[] _lights;

	public const short Torch = 0;

	public const short Blue = 1;

	public const short Red = 2;

	public const short Green = 3;

	public const short Purple = 4;

	public const short White = 5;

	public const short Yellow = 6;

	public const short Demon = 7;

	public const short Cursed = 8;

	public const short Ice = 9;

	public const short Orange = 10;

	public const short Ichor = 11;

	public const short UltraBright = 12;

	public const short Bone = 13;

	public const short Rainbow = 14;

	public const short Pink = 15;

	public const short Desert = 16;

	public const short Coral = 17;

	public const short Corrupt = 18;

	public const short Crimson = 19;

	public const short Hallowed = 20;

	public const short Jungle = 21;

	public const short Mushroom = 22;

	public const short Shimmer = 23;

	public static readonly short Count = 24;

	public static void Initialize()
	{
		ITorchLightProvider[] array = new ITorchLightProvider[Count];
		array[0] = new ConstantTorchLight(1f, 0.95f, 0.8f);
		array[1] = new ConstantTorchLight(0f, 0.1f, 1.3f);
		array[2] = new ConstantTorchLight(1f, 0.1f, 0.1f);
		array[3] = new ConstantTorchLight(0f, 1f, 0.1f);
		array[4] = new ConstantTorchLight(0.9f, 0f, 0.9f);
		array[5] = new ConstantTorchLight(1.4f, 1.4f, 1.4f);
		array[6] = new ConstantTorchLight(0.9f, 0.9f, 0f);
		array[7] = default(DemonTorchLight);
		array[8] = new ConstantTorchLight(1f, 1.6f, 0.5f);
		array[9] = new ConstantTorchLight(0.75f, 0.85f, 1.4f);
		array[10] = new ConstantTorchLight(1f, 0.5f, 0f);
		array[11] = new ConstantTorchLight(1.4f, 1.4f, 0.7f);
		array[12] = new ConstantTorchLight(0.75f, 1.3499999f, 1.5f);
		array[13] = new ConstantTorchLight(0.95f, 0.75f, 1.3f);
		array[14] = default(DiscoTorchLight);
		array[15] = new ConstantTorchLight(1f, 0f, 1f);
		array[16] = new ConstantTorchLight(1.4f, 0.85f, 0.55f);
		array[17] = new ConstantTorchLight(0.25f, 1.3f, 0.8f);
		array[18] = new ConstantTorchLight(0.95f, 0.4f, 1.4f);
		array[19] = new ConstantTorchLight(1.4f, 0.7f, 0.5f);
		array[20] = new ConstantTorchLight(1.25f, 0.6f, 1.2f);
		array[21] = new ConstantTorchLight(0.75f, 1.45f, 0.9f);
		array[22] = new ConstantTorchLight(0.3f, 0.78f, 1.2f);
		array[23] = default(ShimmerTorchLight);
		_lights = array;
	}

	public static void TorchColor(int torchID, out float R, out float G, out float B)
	{
		if (torchID < 0 || torchID >= _lights.Length)
		{
			R = (G = (B = 0f));
		}
		else
		{
			_lights[torchID].GetRGB(out R, out G, out B);
		}
	}
}
