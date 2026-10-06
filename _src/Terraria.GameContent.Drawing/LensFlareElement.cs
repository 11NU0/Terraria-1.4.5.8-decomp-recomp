using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Drawing;

public struct LensFlareElement
{
	public Asset<Texture2D> Texture;

	public int RepeatTimes;

	public float ScaleStart;

	public float ScaleOverIndex;

	public float DistanceStart;

	public float DistanceAlongIndex;

	public Color Color;

	public float IntensityOverIndex;

	public float Rotation;

	public void Draw(SpriteBatch spriteBatch, Vector2 sunPosition, Vector2 screenCenterPosition, float intensity)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		if (intensity == 0f)
		{
			return;
		}
		Player localPlayer = Main.LocalPlayer;
		int availableAdvancedShadowsCount = localPlayer.availableAdvancedShadowsCount;
		Vector2 v = localPlayer.GetAdvancedShadow(0).Position - localPlayer.GetAdvancedShadow(Math.Min(4, availableAdvancedShadowsCount - 1)).Position;
		float num = Vector2.Dot(v.SafeNormalize(Vector2.UnitX), (sunPosition - screenCenterPosition).SafeNormalize(-Vector2.UnitY)) * v.Length();
		for (int i = 0; i < RepeatTimes; i++)
		{
			float num2 = ScaleStart + ScaleOverIndex * (float)i;
			Color val = Color * (1f + IntensityOverIndex * (float)i) * intensity;
			float num3 = DistanceStart + DistanceAlongIndex * (float)i;
			num3 += num * -0.0002f;
			num3 %= 1f;
			Vector2 val2 = Vector2.Lerp(sunPosition, screenCenterPosition, num3 * 2f);
			float num4 = (screenCenterPosition - sunPosition).ToRotation() + Rotation;
			if (Rotation == 0f)
			{
				num4 += Main.screenPosition.Y * 0.001f;
			}
			spriteBatch.Draw(Texture.Value, val2, (Rectangle?)null, val, num4, Texture.Size() / 2f, num2, (SpriteEffects)0, 0f);
		}
	}
}
