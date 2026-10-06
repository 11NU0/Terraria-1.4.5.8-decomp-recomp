using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics;

public class Camera
{
	public Vector2 UnscaledPosition
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return Main.screenPosition;
		}
	}

	public Vector2 UnscaledSize
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
		}
	}

	public Vector2 ScaledPosition
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return UnscaledPosition + GameViewMatrix.Translation;
		}
	}

	public Vector2 ScaledSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			return UnscaledSize - GameViewMatrix.Translation * 2f;
		}
	}

	public float BiggerScaledAxis
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Vector2 scaledSize = ScaledSize;
			if (!(scaledSize.X > scaledSize.Y))
			{
				return scaledSize.Y;
			}
			return scaledSize.X;
		}
	}

	public float SmallerScaledAxis
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Vector2 scaledSize = ScaledSize;
			if (!(scaledSize.X < scaledSize.Y))
			{
				return scaledSize.Y;
			}
			return scaledSize.X;
		}
	}

	public RasterizerState Rasterizer => Main.Rasterizer;

	public SamplerState Sampler => Main.DefaultSamplerState;

	public SpriteViewMatrix GameViewMatrix => Main.GameViewMatrix;

	public SpriteBatch SpriteBatch => Main.spriteBatch;

	public Vector2 Center
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return UnscaledPosition + UnscaledSize * 0.5f;
		}
	}
}
