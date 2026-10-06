using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIImageFramed : UIElement, IColorable
{
	private Asset<Texture2D> _texture;

	private Rectangle _frame;

	public Color Color
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return field;
		}
		[CompilerGenerated]
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			field = value;
		}
	}

	public UIImageFramed(Asset<Texture2D> texture, Rectangle frame)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		_texture = texture;
		_frame = frame;
		Width.Set(_frame.Width, 0f);
		Height.Set(_frame.Height, 0f);
		Color = Color.White;
	}

	public void SetImage(Asset<Texture2D> texture, Rectangle frame)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		_texture = texture;
		_frame = frame;
		Width.Set(_frame.Width, 0f);
		Height.Set(_frame.Height, 0f);
	}

	public void SetFrame(Rectangle frame)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_frame = frame;
		Width.Set(_frame.Width, 0f);
		Height.Set(_frame.Height, 0f);
	}

	public void SetFrame(int frameCountHorizontal, int frameCountVertical, int frameX, int frameY, int sizeOffsetX, int sizeOffsetY)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		SetFrame(_texture.Frame(frameCountHorizontal, frameCountVertical, frameX, frameY).OffsetSize(sizeOffsetX, sizeOffsetY));
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		spriteBatch.Draw(_texture.Value, dimensions.Position(), (Rectangle?)_frame, Color);
	}
}
