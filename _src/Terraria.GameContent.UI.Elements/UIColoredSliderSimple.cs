using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIColoredSliderSimple : UIElement
{
	public float FillPercent;

	public Color FilledColor = Main.OurFavoriteColor;

	public Color EmptyColor = Color.Black;

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		DrawValueBarDynamicWidth(spriteBatch);
	}

	private void DrawValueBarDynamicWidth(SpriteBatch sb)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value = TextureAssets.ColorBar.Value;
		Rectangle val = GetDimensions().ToRectangle();
		Rectangle val2 = new Rectangle(5, 4, 4, 4);
		Utils.DrawSplicedPanel(sb, value, val.X, val.Y, val.Width, val.Height, val2.X, val2.Width, val2.Y, val2.Height, Color.White);
		Rectangle val3 = val;
		val3.X += val2.Left;
		val3.Width -= val2.Right;
		val3.Y += val2.Top;
		val3.Height -= val2.Bottom;
		Texture2D value2 = TextureAssets.MagicPixel.Value;
		Rectangle value3 = new Rectangle(0, 0, 1, 1);
		sb.Draw(value2, val3, (Rectangle?)value3, EmptyColor);
		Rectangle val4 = val3;
		val4.Width = (int)((float)val4.Width * FillPercent);
		sb.Draw(value2, val4, (Rectangle?)value3, FilledColor);
	}

	public UIColoredSliderSimple()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	}
}
