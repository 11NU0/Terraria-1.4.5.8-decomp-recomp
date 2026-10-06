using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders;

public class BlizzardShaderData(string passName) : ScreenShaderData(passName)
{
	private Vector2 _texturePosition = Vector2.Zero;

	private float windSpeed = 0.1f;

	public override void Update(GameTime gameTime)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.windSpeedCurrent;
		if (num >= 0f && num <= 0.1f)
		{
			num = 0.1f;
		}
		else if (num <= 0f && num >= -0.1f)
		{
			num = -0.1f;
		}
		windSpeed = num * 0.05f + windSpeed * 0.95f;
		Vector2 val = new Vector2(0f - windSpeed, -1f) * new Vector2(10f, 2f);
		val.Normalize();
		val *= new Vector2(0.8f, 0.6f);
		if (FocusHelper.UpdateVisualEffects)
		{
			_texturePosition += val * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}
		_texturePosition.X %= 10f;
		_texturePosition.Y %= 10f;
		UseDirection(val);
		UseTargetPosition(_texturePosition);
		base.Update(gameTime);
	}

	public override void Apply()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		UseTargetPosition(_texturePosition);
		base.Apply();
	}
}
