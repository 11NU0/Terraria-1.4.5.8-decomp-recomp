using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders;

public class MiscShaderData(Asset<Effect> shader, string passName) : ShaderData(shader, passName)
{
	private Vector3 _uColor = Vector3.One;

	private Vector3 _uSecondaryColor = Vector3.One;

	private float _uSaturation = 1f;

	private float _uOpacity = 1f;

	private Asset<Texture2D> _uImage0;

	private Asset<Texture2D> _uImage1;

	private Asset<Texture2D> _uImage2;

	private Texture _uImage0Tex;

	private Texture _uImage1Tex;

	private Texture _uImage2Tex;

	private bool _useProjectionMatrix;

	private Vector4 _shaderSpecificData = Vector4.Zero;

	private SamplerState _customSamplerState;

	private Matrix? _transformMatrix;

	private Effect _effect;

	private EffectParameter<Vector3> uColor;

	private EffectParameter<float> uSaturation;

	private EffectParameter<Vector3> uSecondaryColor;

	private EffectParameter<float> uTime;

	private EffectParameter<float> uOpacity;

	private EffectParameter<Vector4> uShaderSpecificData;

	private EffectParameter<Vector4> uSourceRect;

	private EffectParameter<Vector2> uDrawPosition;

	private EffectParameter<Vector2> uImageSize0;

	private EffectParameter<Vector2> uImageSize1;

	private EffectParameter<Vector2> uImageSize2;

	private EffectParameter<Matrix> MatrixTransform;

	private void CheckCachedParameters()
	{
		if (_effect == null || _effect != Shader)
		{
			_effect = Shader;
			uColor = Shader.GetParameter<Vector3>("uColor");
			uSaturation = Shader.GetParameter<float>("uSaturation");
			uSecondaryColor = Shader.GetParameter<Vector3>("uSecondaryColor");
			uTime = Shader.GetParameter<float>("uTime");
			uOpacity = Shader.GetParameter<float>("uOpacity");
			uShaderSpecificData = Shader.GetParameter<Vector4>("uShaderSpecificData");
			uSourceRect = Shader.GetParameter<Vector4>("uSourceRect");
			uDrawPosition = Shader.GetParameter<Vector2>("uDrawPosition");
			uImageSize0 = Shader.GetParameter<Vector2>("uImageSize0");
			uImageSize1 = Shader.GetParameter<Vector2>("uImageSize1");
			uImageSize2 = Shader.GetParameter<Vector2>("uImageSize2");
			MatrixTransform = Shader.GetParameter<Matrix>("MatrixTransform");
		}
	}

	public virtual void Apply(DrawData? drawData = null)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected Obj, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected Obj, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected Obj, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		CheckCachedParameters();
		uColor.SetValue(_uColor);
		uSaturation.SetValue(_uSaturation);
		uSecondaryColor.SetValue(_uSecondaryColor);
		uTime.SetValue(Main.GlobalTimeWrappedHourly);
		uOpacity.SetValue(_uOpacity);
		uShaderSpecificData.SetValue(_shaderSpecificData);
		if (drawData.HasValue)
		{
			DrawData value = drawData.Value;
			Vector4 value2 = Vector4.Zero;
			if (drawData.Value.sourceRect.HasValue)
			{
				value2 = new Vector4((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height);
			}
			uSourceRect.SetValue(value2);
			uDrawPosition.SetValue(value.position);
			uImageSize0.SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
		}
		else
		{
			uSourceRect.SetValue(new Vector4(0f, 0f, 4f, 4f));
		}
		SamplerState val = SamplerState.LinearWrap;
		if (_customSamplerState != null)
		{
			val = _customSamplerState;
		}
		Texture val2 = (Texture)((_uImage0 != null) ? ((object)_uImage0.Value) : ((object)_uImage0Tex));
		if (val2 != null)
		{
			Main.graphics.GraphicsDevice.Textures[0] = val2;
			Main.graphics.GraphicsDevice.SamplerStates[0] = val;
			if (val2 is Texture2D)
			{
				uImageSize0.SetValue(((Texture2D)val2).Size());
			}
		}
		val2 = (Texture)((_uImage1 != null) ? ((object)_uImage1.Value) : ((object)_uImage1Tex));
		if (val2 != null)
		{
			Main.graphics.GraphicsDevice.Textures[1] = val2;
			Main.graphics.GraphicsDevice.SamplerStates[1] = val;
			if (val2 is Texture2D)
			{
				uImageSize1.SetValue(((Texture2D)val2).Size());
			}
		}
		val2 = (Texture)((_uImage2 != null) ? ((object)_uImage2.Value) : ((object)_uImage2Tex));
		if (val2 != null)
		{
			Main.graphics.GraphicsDevice.Textures[2] = val2;
			Main.graphics.GraphicsDevice.SamplerStates[2] = val;
			if (val2 is Texture2D)
			{
				uImageSize2.SetValue(((Texture2D)val2).Size());
			}
		}
		if (_useProjectionMatrix)
		{
			MatrixTransform.SetValue(Main.GameViewMatrix.NormalizedTransformationMatrix);
		}
		if (_transformMatrix.HasValue)
		{
			MatrixTransform.SetValue(_transformMatrix.Value);
		}
		base.Apply();
	}

	public MiscShaderData UseColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(new Vector3(r, g, b));
	}

	public MiscShaderData UseColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(color.ToVector3());
	}

	public MiscShaderData UseColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uColor = color;
		return this;
	}

	public MiscShaderData UseSamplerState(SamplerState state)
	{
		_customSamplerState = state;
		return this;
	}

	public MiscShaderData UseImage0(string path)
	{
		if (Main.dedServ)
		{
			return this;
		}
		_uImage0Tex = null;
		_uImage0 = Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
		return this;
	}

	public MiscShaderData UseImage1(string path)
	{
		if (Main.dedServ)
		{
			return this;
		}
		_uImage1Tex = null;
		_uImage1 = Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
		return this;
	}

	public MiscShaderData UseImage2(string path)
	{
		if (Main.dedServ)
		{
			return this;
		}
		_uImage2Tex = null;
		_uImage2 = Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
		return this;
	}

	public MiscShaderData UseImage0(Texture texture)
	{
		if (Main.dedServ)
		{
			return this;
		}
		_uImage0Tex = texture;
		_uImage0 = null;
		return this;
	}

	public MiscShaderData UseImage1(Texture texture)
	{
		if (Main.dedServ)
		{
			return this;
		}
		_uImage1Tex = texture;
		_uImage1 = null;
		return this;
	}

	public MiscShaderData UseImage2(Texture texture)
	{
		if (Main.dedServ)
		{
			return this;
		}
		_uImage2Tex = texture;
		_uImage2 = null;
		return this;
	}

	private static bool IsPowerOfTwo(int n)
	{
		return (int)Math.Ceiling(Math.Log(n) / Math.Log(2.0)) == (int)Math.Floor(Math.Log(n) / Math.Log(2.0));
	}

	public MiscShaderData UseOpacity(float alpha)
	{
		_uOpacity = alpha;
		return this;
	}

	public MiscShaderData UseSecondaryColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(new Vector3(r, g, b));
	}

	public MiscShaderData UseSecondaryColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(color.ToVector3());
	}

	public MiscShaderData UseSecondaryColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uSecondaryColor = color;
		return this;
	}

	public MiscShaderData UseProjectionMatrix(bool doUse)
	{
		_useProjectionMatrix = doUse;
		return this;
	}

	public MiscShaderData UseSpriteTransformMatrix(Matrix? transform)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		if (!transform.HasValue)
		{
			_transformMatrix = null;
			return this;
		}
		Viewport viewport = Main.graphics.GraphicsDevice.Viewport;
		float num = ((viewport.Width > 0) ? (1f / (float)viewport.Width) : 0f);
		float num2 = ((viewport.Height > 0) ? (-1f / (float)viewport.Height) : 0f);
		Matrix val = new Matrix
		{
			M11 = num * 2f,
			M22 = num2 * 2f,
			M33 = 1f,
			M44 = 1f,
			M41 = -1f - num,
			M42 = 1f - num2
		};
		_transformMatrix = transform.Value * val;
		return this;
	}

	public MiscShaderData UseSaturation(float saturation)
	{
		_uSaturation = saturation;
		return this;
	}

	public virtual MiscShaderData GetSecondaryShader(Entity entity)
	{
		return this;
	}

	public MiscShaderData UseShaderSpecificData(Vector4 specificData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_shaderSpecificData = specificData;
		return this;
	}
}
