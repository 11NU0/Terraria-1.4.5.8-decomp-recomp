using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIHeader : UIElement
{
	private string _text;

	public string Text
	{
		get
		{
			return _text;
		}
		set
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (_text != value)
			{
				_text = value;
				if (!Main.dedServ)
				{
					Vector2 val = FontAssets.DeathText.Value.MeasureString(Text);
					Width.Pixels = val.X;
					Height.Pixels = val.Y;
				}
				Width.Precent = 0f;
				Height.Precent = 0f;
				Recalculate();
			}
		}
	}

	public UIHeader()
	{
		Text = "";
	}

	public UIHeader(string text)
	{
		Text = text;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		float num = 1.2f;
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, Text, new Vector2(dimensions.X - num, dimensions.Y - num), Color.Black);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, Text, new Vector2(dimensions.X + num, dimensions.Y - num), Color.Black);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, Text, new Vector2(dimensions.X - num, dimensions.Y + num), Color.Black);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, Text, new Vector2(dimensions.X + num, dimensions.Y + num), Color.Black);
		if (WorldGen.tenthAnniversaryWorldGen && !Main.zenithWorld)
		{
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, Text, new Vector2(dimensions.X, dimensions.Y), Color.HotPink);
		}
		else
		{
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, Text, new Vector2(dimensions.X, dimensions.Y), Color.White);
		}
	}
}
