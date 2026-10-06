using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Drawing;

public class EmptyHorizonRenderer : IHorizonRenderer
{
	public void DrawHorizon()
	{
		if (!Main.ShouldDrawSurfaceBackground())
		{
			return;
		}
		foreach (BackgroundGradientDrawer backgroundDrawer in SunGradients.BackgroundDrawers)
		{
			backgroundDrawer.Draw();
		}
	}

	public void DrawLensFlare()
	{
	}

	public void ModifyHorizonLight(ref Color color)
	{
	}

	public void DrawSun(Vector2 sunPosition)
	{
	}

	public void DrawSurfaceLayer(int layerIndex)
	{
	}

	public void CloudsStart()
	{
	}

	public void DrawCloud(float globalCloudAlpha, Cloud theCloud, int cloudPass, float cY)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> val = TextureAssets.Cloud[theCloud.type];
		Color val2 = theCloud.cloudColor(Main.ColorOfTheSkies);
		if (cloudPass == 1)
		{
			float num = theCloud.scale * 0.8f;
			float num2 = (theCloud.scale + 1f) / 2f * 0.9f;
			val2.R = (byte)((float)(int)val2.R * num);
			val2.G = (byte)((float)(int)val2.G * num2);
		}
		if (Main.atmo < 1f)
		{
			val2 *= Main.atmo;
		}
		Main.spriteBatch.Draw(val.Value, new Vector2(theCloud.position.X, cY) + val.Size() / 2f, (Rectangle?)null, val2 * globalCloudAlpha, theCloud.rotation, val.Size() / 2f, theCloud.scale, theCloud.spriteDir, 0f);
	}

	public void CloudsEnd()
	{
	}
}
