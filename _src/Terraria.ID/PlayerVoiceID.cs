using Microsoft.Xna.Framework;

namespace Terraria.ID;

public class PlayerVoiceID
{
	public static class Sets
	{
		public static SetFactory Factory = new SetFactory(4);

		public static Color[] Colors = Factory.CreateCustomSet<Color>(Color.White, new object[6]
		{
			1,
			Color.CornflowerBlue,
			2,
			Color.HotPink,
			3,
			Color.LimeGreen
		});

		static Sets()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		}
	}

	public static int[] VariantOrder = new int[3] { 1, 2, 3 };

	public const int None = 0;

	public const int Male = 1;

	public const int Female = 2;

	public const int Other = 3;

	public const int Count = 4;
}
