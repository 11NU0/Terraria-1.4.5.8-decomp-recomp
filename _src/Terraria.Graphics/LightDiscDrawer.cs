using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct LightDiscDrawer
{
	private static VertexStrip _vertexStrip = new VertexStrip();

	public void Draw(Projectile proj)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
		miscShaderData.UseSaturation(-2.8f);
		miscShaderData.UseOpacity(2f);
		miscShaderData.Apply();
		_vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
		_vertexStrip.DrawTrail();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}

	private Color StripColors(float progressOnStrip)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f - progressOnStrip;
		Color result = new Color(48, 63, 150) * (num * num * num * num) * 0.5f;
		result.A = 0;
		return result;
	}

	private float StripWidth(float progressOnStrip)
	{
		return 16f;
	}
}
