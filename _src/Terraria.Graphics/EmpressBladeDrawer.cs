using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics;

public struct EmpressBladeDrawer
{
	public const int TotalIllusions = 1;

	public const int FramesPerImportantTrail = 60;

	private static VertexStrip _vertexStrip = new VertexStrip();

	public Color ColorStart;

	public Color ColorEnd;

	public void Draw(Projectile proj)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		_ = proj.ai[1];
		MiscShaderData miscShaderData = GameShaders.Misc["EmpressBlade"];
		int num = 1;
		int num2 = 0;
		int num3 = 0;
		float num4 = 0.6f;
		miscShaderData.UseShaderSpecificData(new Vector4((float)num, (float)num2, (float)num3, num4));
		miscShaderData.Apply();
		_vertexStrip.PrepareStrip(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f, proj.oldPos.Length, includeBacksides: true);
		_vertexStrip.DrawTrail();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}

	private Color StripColors(float progressOnStrip)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		Color result = Color.Lerp(ColorStart, ColorEnd, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip, clamped: true));
		result.A /= 2;
		return result;
	}

	private float StripWidth(float progressOnStrip)
	{
		return 36f;
	}
}
