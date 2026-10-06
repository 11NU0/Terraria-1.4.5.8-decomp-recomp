using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures;

public struct DrawData
{
	public Texture2D texture;

	public Vector2 position;

	public Rectangle destinationRectangle;

	public Rectangle? sourceRect;

	public Color color;

	public float rotation;

	public Vector2 origin;

	public Vector2 scale;

	public SpriteEffects effect;

	public int shader;

	public bool ignorePlayerRotation;

	public readonly bool useDestinationRectangle;

	public static Rectangle? nullRectangle;

	public DrawData(Texture2D texture, Vector2 position, Color color)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		this.texture = texture;
		this.position = position;
		this.color = color;
		destinationRectangle = default;
		sourceRect = nullRectangle;
		rotation = 0f;
		origin = Vector2.Zero;
		scale = Vector2.One;
		effect = (SpriteEffects)0;
		shader = 0;
		ignorePlayerRotation = false;
		useDestinationRectangle = false;
	}

	public DrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		this.texture = texture;
		this.position = position;
		this.color = color;
		destinationRectangle = default;
		this.sourceRect = sourceRect;
		rotation = 0f;
		origin = Vector2.Zero;
		scale = Vector2.One;
		effect = (SpriteEffects)0;
		shader = 0;
		ignorePlayerRotation = false;
		useDestinationRectangle = false;
	}

	public DrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float inactiveLayerDepth = 0f)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		this.texture = texture;
		this.position = position;
		this.sourceRect = sourceRect;
		this.color = color;
		this.rotation = rotation;
		this.origin = origin;
		this.scale = new Vector2(scale, scale);
		this.effect = effect;
		destinationRectangle = default;
		shader = 0;
		ignorePlayerRotation = false;
		useDestinationRectangle = false;
	}

	public DrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float inactiveLayerDepth = 0f)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		this.texture = texture;
		this.position = position;
		this.sourceRect = sourceRect;
		this.color = color;
		this.rotation = rotation;
		this.origin = origin;
		this.scale = scale;
		this.effect = effect;
		destinationRectangle = default;
		shader = 0;
		ignorePlayerRotation = false;
		useDestinationRectangle = false;
	}

	public DrawData(Texture2D texture, Rectangle destinationRectangle, Color color)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		this.texture = texture;
		this.destinationRectangle = destinationRectangle;
		this.color = color;
		position = Vector2.Zero;
		sourceRect = nullRectangle;
		rotation = 0f;
		origin = Vector2.Zero;
		scale = Vector2.One;
		effect = (SpriteEffects)0;
		shader = 0;
		ignorePlayerRotation = false;
		useDestinationRectangle = true;
	}

	public DrawData(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRect, Color color)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		this.texture = texture;
		this.destinationRectangle = destinationRectangle;
		this.color = color;
		position = Vector2.Zero;
		this.sourceRect = sourceRect;
		rotation = 0f;
		origin = Vector2.Zero;
		scale = Vector2.One;
		effect = (SpriteEffects)0;
		shader = 0;
		ignorePlayerRotation = false;
		useDestinationRectangle = true;
	}

	public DrawData(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, SpriteEffects effect, float inactiveLayerDepth = 0f)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		this.texture = texture;
		this.destinationRectangle = destinationRectangle;
		this.sourceRect = sourceRect;
		this.color = color;
		this.rotation = rotation;
		this.origin = origin;
		this.effect = effect;
		position = Vector2.Zero;
		scale = Vector2.One;
		shader = 0;
		ignorePlayerRotation = false;
		useDestinationRectangle = true;
	}

	public void Draw(SpriteBatch sb)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (useDestinationRectangle)
		{
			sb.Draw(texture, destinationRectangle, sourceRect, color, rotation, origin, effect, 0f);
		}
		else
		{
			sb.Draw(texture, position, sourceRect, color, rotation, origin, scale, effect, 0f);
		}
	}

	public void Draw(SpriteDrawBuffer sb)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (useDestinationRectangle)
		{
			sb.Draw(texture, destinationRectangle, sourceRect, color, rotation, origin, effect);
		}
		else
		{
			sb.Draw(texture, position, sourceRect, color, rotation, origin, scale, effect);
		}
	}
}
