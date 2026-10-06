using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders;

public class SandstormShaderData(string passName) : ScreenShaderData(passName)
{
	private Vector2 _texturePosition = Vector2.Zero;

	public override void Update(GameTime gameTime)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2(0f - Main.windSpeedCurrent, -1f) * new Vector2(20f, 0.1f);
		val.Normalize();
		val *= new Vector2(2f, 0.2f);
		if (FocusHelper.UpdateVisualEffects)
		{
			_texturePosition += val * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}
		_texturePosition.X %= 10f;
		_texturePosition.Y %= 10f;
		UseDirection(val);
		base.Update(gameTime);
	}

	public override void Apply()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		UseTargetPosition(_texturePosition);
		base.Apply();
	}
}
