using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct RainbowRodDrawer
{
	private static VertexStrip _vertexStrip = new VertexStrip();

	public void Draw(Projectile proj)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
		miscShaderData.UseSaturation(-2.8f);
		miscShaderData.UseOpacity(4f);
		miscShaderData.Apply();
		_vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
		_vertexStrip.DrawTrail();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}

	private Color StripColors(float progressOnStrip)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		Color val = Main.hslToRgb((progressOnStrip * 1.6f - Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.5f);
		Color result = Color.Lerp(Color.White, val, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
		result.A = 0;
		return result;
	}

	private float StripWidth(float progressOnStrip)
	{
		float num = 1f;
		float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
		num *= 1f - (1f - lerpValue) * (1f - lerpValue);
		return MathHelper.Lerp(0f, 32f, num);
	}
}
