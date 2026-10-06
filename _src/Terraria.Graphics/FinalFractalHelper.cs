using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct FinalFractalHelper
{
	public delegate void SpawnDustMethod(Vector2 centerPosition, float rotation, Vector2 velocity);

	public struct FinalFractalProfile
	{
		public float trailWidth;

		public Color trailColor;

		public SpawnDustMethod dustMethod;

		public VertexStrip.StripColorFunction colorMethod;

		public VertexStrip.StripHalfWidthFunction widthMethod;

		public FinalFractalProfile(float fullBladeLength, Color color)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			trailWidth = fullBladeLength / 2f;
			trailColor = color;
			widthMethod = null;
			colorMethod = null;
			dustMethod = null;
			widthMethod = StripWidth;
			colorMethod = StripColors;
			dustMethod = StripDust;
		}

		private void StripDust(Vector2 centerPosition, float rotation, Vector2 velocity)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			if (Main.rand.Next(9) == 0)
			{
				int num = Main.rand.Next(1, 4);
				for (int i = 0; i < num; i++)
				{
					Dust dust = Dust.NewDustPerfect(centerPosition, 278, null, 100, Color.Lerp(trailColor, Color.White, Main.rand.NextFloat() * 0.3f));
					dust.scale = 0.4f;
					dust.fadeIn = 0.4f + Main.rand.NextFloat() * 0.3f;
					dust.noGravity = true;
					dust.velocity += rotation.ToRotationVector2() * (3f + Main.rand.NextFloat() * 4f);
				}
			}
		}

		private Color StripColors(float progressOnStrip)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			Color result = trailColor * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A /= 2;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			return trailWidth;
		}
	}

	public const int TotalIllusions = 4;

	public const int FramesPerImportantTrail = 15;

	private static VertexStrip _vertexStrip = new VertexStrip();

	private static Dictionary<int, FinalFractalProfile> _fractalProfiles = new Dictionary<int, FinalFractalProfile>
	{
		{
			65,
			new FinalFractalProfile(60f, new Color(236, 62, 192))
		},
		{
			1123,
			new FinalFractalProfile(75f, Main.OurFavoriteColor)
		},
		{
			46,
			new FinalFractalProfile(65f, new Color(122, 66, 191))
		},
		{
			121,
			new FinalFractalProfile(110f, new Color(254, 158, 35))
		},
		{
			190,
			new FinalFractalProfile(105f, new Color(107, 203, 0))
		},
		{
			368,
			new FinalFractalProfile(75f, new Color(236, 200, 19))
		},
		{
			674,
			new FinalFractalProfile(75f, new Color(236, 200, 19))
		},
		{
			273,
			new FinalFractalProfile(70f, new Color(179, 54, 201))
		},
		{
			675,
			new FinalFractalProfile(70f, new Color(179, 54, 201))
		},
		{
			2880,
			new FinalFractalProfile(70f, new Color(84, 234, 245))
		},
		{
			989,
			new FinalFractalProfile(48f, new Color(91, 158, 232))
		},
		{
			1826,
			new FinalFractalProfile(76f, new Color(252, 95, 4))
		},
		{
			3063,
			new FinalFractalProfile(76f, new Color(254, 194, 250))
		},
		{
			3065,
			new FinalFractalProfile(70f, new Color(237, 63, 133))
		},
		{
			757,
			new FinalFractalProfile(70f, new Color(80, 222, 122))
		},
		{
			155,
			new FinalFractalProfile(70f, new Color(56, 78, 210))
		},
		{
			795,
			new FinalFractalProfile(72f, new Color(237, 28, 36))
		},
		{
			3018,
			new FinalFractalProfile(75f, new Color(143, 215, 29))
		},
		{
			4144,
			new FinalFractalProfile(45f, new Color(178, 255, 180))
		},
		{
			3507,
			new FinalFractalProfile(45f, new Color(235, 166, 135))
		},
		{
			4956,
			new FinalFractalProfile(86f, new Color(178, 255, 180))
		}
	};

	private static FinalFractalProfile _defaultProfile = new FinalFractalProfile(50f, Color.White);

	public static int GetRandomProfileIndex()
	{
		List<int> list = _fractalProfiles.Keys.ToList();
		int index = Main.rand.Next(list.Count);
		if (list[index] == 4956)
		{
			list.RemoveAt(index);
			index = Main.rand.Next(list.Count);
		}
		return list[index];
	}

	public void Draw(Projectile proj)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		FinalFractalProfile finalFractalProfile = GetFinalFractalProfile((int)proj.ai[1]);
		MiscShaderData miscShaderData = GameShaders.Misc["FinalFractal"];
		int num = 4;
		int num2 = 0;
		int num3 = 0;
		int num4 = 4;
		miscShaderData.UseShaderSpecificData(new Vector4((float)num, (float)num2, (float)num3, (float)num4));
		miscShaderData.UseImage0("Images/Extra_" + (short)201);
		miscShaderData.UseImage1("Images/Extra_" + (short)193);
		miscShaderData.Apply();
		_vertexStrip.PrepareStrip(proj.oldPos, proj.oldRot, finalFractalProfile.colorMethod, finalFractalProfile.widthMethod, -Main.screenPosition + proj.Size / 2f, proj.oldPos.Length, includeBacksides: true);
		_vertexStrip.DrawTrail();
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}

	public static FinalFractalProfile GetFinalFractalProfile(int usedSwordId)
	{
		if (!_fractalProfiles.TryGetValue(usedSwordId, out var value))
		{
			return _defaultProfile;
		}
		return value;
	}

	private Color StripColors(float progressOnStrip)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		Color result = Color.Lerp(Color.White, Color.Violet, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
		result.A /= 2;
		return result;
	}

	private float StripWidth(float progressOnStrip)
	{
		return 50f;
	}

	static FinalFractalHelper()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
	}
}
