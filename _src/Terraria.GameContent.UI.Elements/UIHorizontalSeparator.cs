using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIHorizontalSeparator : UIElement
{
	private Asset<Texture2D> _texture;

	public Color Color;

	public int EdgeWidth;

	public UIHorizontalSeparator(int EdgeWidth = 2, bool highlightSideUp = true)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Color = Color.White;
		if (highlightSideUp)
		{
			_texture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Separator1", (AssetRequestMode)1);
		}
		else
		{
			_texture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Separator2", (AssetRequestMode)1);
		}
		Width.Set(_texture.Width(), 0f);
		Height.Set(_texture.Height(), 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Utils.DrawPanel(_texture.Value, EdgeWidth, 0, spriteBatch, dimensions.Position().Floor(), dimensions.Width, Color);
	}

	public override bool ContainsPoint(Vector2 point)
	{
		return false;
	}
}
