using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent.UI;

public class ItemRarity
{
	private static Dictionary<int, Color> _rarities = new Dictionary<int, Color>();

	public static void Initialize()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		_rarities.Clear();
		_rarities.Add(-11, Colors.RarityAmber);
		_rarities.Add(-1, Colors.RarityTrash);
		_rarities.Add(1, Colors.RarityBlue);
		_rarities.Add(2, Colors.RarityGreen);
		_rarities.Add(3, Colors.RarityOrange);
		_rarities.Add(4, Colors.RarityRed);
		_rarities.Add(5, Colors.RarityPink);
		_rarities.Add(6, Colors.RarityPurple);
		_rarities.Add(7, Colors.RarityLime);
		_rarities.Add(8, Colors.RarityYellow);
		_rarities.Add(9, Colors.RarityCyan);
	}

	public static Color GetColor(int rarity)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		Color result = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		if (_rarities.ContainsKey(rarity))
		{
			return _rarities[rarity];
		}
		return result;
	}
}
