using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Shaders;

public class ScreenShaderData : ShaderData
{
	private Vector3 _uColor = Vector3.One;

	private Vector3 _uSecondaryColor = Vector3.One;

	private float _uOpacity = 1f;

	private float _globalOpacity = 1f;

	private float _uIntensity = 1f;

	private Vector2 _uTargetPosition = Vector2.One;

	private Vector2 _uDirection = new Vector2(0f, 1f);

	private float _uProgress;

	private Vector2 _uImageOffset = Vector2.Zero;

	private Vector2 _uSceneSize;

	private Vector2 _uSceneOffset;

	private Vector2 _uImageSize0;

	private Asset<Texture2D>[] _uAssetImages = new Asset<Texture2D>[3];

	private Texture2D[] _uCustomImages = new Texture2D[3];

	private SamplerState[] _samplerStates = new SamplerState[3];

	private Vector2[] _imageScales = new Vector2[3]
	{
		Vector2.One,
		Vector2.One,
		Vector2.One
	};

	public static bool MultiChunkCapture;

	private Effect _effect;

	private EffectParameter<Vector3> uColor;

	private EffectParameter<float> uOpacity;

	private EffectParameter<Vector3> uSecondaryColor;

	private EffectParameter<float> uTime;

	private EffectParameter<Vector2> uScreenResolution;

	private EffectParameter<Vector2> uScreenPosition;

	private EffectParameter<Vector2> uTargetPosition;

	private EffectParameter<Vector2> uImageOffset;

	private EffectParameter<Vector2> uSceneSize;

	private EffectParameter<Vector2> uSceneOffset;

	private EffectParameter<float> uIntensity;

	private EffectParameter<float> uProgress;

	private EffectParameter<Vector2> uDirection;

	private EffectParameter<Vector2> uZoom;

	private EffectParameter<Vector2>[] uImageSize = new EffectParameter<Vector2>[4];

	private EffectParameter<bool> uMultiChunkScene;

	public float Intensity => _uIntensity;

	public float CombinedOpacity => _uOpacity * _globalOpacity;

	public static Vector2 UnscaledScreenPosition
	{
		get
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			Matrix effectMatrix = Main.GameViewMatrix.EffectMatrix;
			Matrix transformationMatrix = Main.GameViewMatrix.TransformationMatrix;
			return Main.screenPosition + new Vector2(effectMatrix.M41 - transformationMatrix.M41, effectMatrix.M42 - transformationMatrix.M42) / new Vector2(transformationMatrix.M11, transformationMatrix.M22);
		}
	}

	public static Vector2 UnscaledScreenSize
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / Main.GameViewMatrix.RenderZoom;
		}
	}

	public ScreenShaderData(string passName)
		: base(Main.ScreenShaderRef, passName)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
	}

	public ScreenShaderData(Asset<Effect> shader, string passName)
		: base(shader, passName)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
	}

	public virtual void Update(GameTime gameTime)
	{
	}

	private void CheckCachedParameters()
	{
		if (_effect == null || _effect != Shader)
		{
			_effect = Shader;
			uColor = Shader.GetParameter<Vector3>("uColor");
			uOpacity = Shader.GetParameter<float>("uOpacity");
			uSecondaryColor = Shader.GetParameter<Vector3>("uSecondaryColor");
			uTime = Shader.GetParameter<float>("uTime");
			uScreenResolution = Shader.GetParameter<Vector2>("uScreenResolution");
			uScreenPosition = Shader.GetParameter<Vector2>("uScreenPosition");
			uTargetPosition = Shader.GetParameter<Vector2>("uTargetPosition");
			uImageOffset = Shader.GetParameter<Vector2>("uImageOffset");
			uSceneSize = Shader.GetParameter<Vector2>("uSceneSize");
			uSceneOffset = Shader.GetParameter<Vector2>("uSceneOffset");
			uIntensity = Shader.GetParameter<float>("uIntensity");
			uProgress = Shader.GetParameter<float>("uProgress");
			uDirection = Shader.GetParameter<Vector2>("uDirection");
			uZoom = Shader.GetParameter<Vector2>("uZoom");
			uMultiChunkScene = Shader.GetParameter<bool>("uMultiChunkScene");
			for (int i = 0; i < uImageSize.Length; i++)
			{
				uImageSize[i] = Shader.GetParameter<Vector2>("uImageSize" + i);
			}
		}
	}

	public override void Apply()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		CheckCachedParameters();
		Vector2 val = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
		uColor.SetValue(_uColor);
		uOpacity.SetValue(CombinedOpacity);
		uSecondaryColor.SetValue(_uSecondaryColor);
		uTime.SetValue(Main.GlobalTimeWrappedHourly);
		uScreenResolution.SetValue(UnscaledScreenSize);
		uScreenPosition.SetValue(UnscaledScreenPosition - val);
		uTargetPosition.SetValue(_uTargetPosition - val);
		uImageOffset.SetValue(_uImageOffset);
		uSceneSize.SetValue(_uSceneSize);
		uSceneOffset.SetValue(_uSceneOffset);
		uIntensity.SetValue(_uIntensity);
		uProgress.SetValue(_uProgress);
		uDirection.SetValue(_uDirection);
		uZoom.SetValue(Main.GameViewMatrix.RenderZoom);
		uMultiChunkScene.SetValue(MultiChunkCapture);
		uImageSize[0].SetValue(_uImageSize0);
		for (int i = 0; i < _uAssetImages.Length; i++)
		{
			Texture2D val2 = _uCustomImages[i];
			if (_uAssetImages[i] != null && _uAssetImages[i].IsLoaded)
			{
				val2 = _uAssetImages[i].Value;
			}
			if (val2 != null)
			{
				Main.graphics.GraphicsDevice.Textures[i + 1] = (Texture)(object)val2;
				int width = val2.Width;
				int height = val2.Height;
				if (_samplerStates[i] != null)
				{
					Main.graphics.GraphicsDevice.SamplerStates[i + 1] = _samplerStates[i];
				}
				else if (Utils.IsPowerOfTwo(width) && Utils.IsPowerOfTwo(height))
				{
					Main.graphics.GraphicsDevice.SamplerStates[i + 1] = SamplerState.LinearWrap;
				}
				else
				{
					Main.graphics.GraphicsDevice.SamplerStates[i + 1] = SamplerState.AnisotropicClamp;
				}
				uImageSize[i + 1].SetValue(new Vector2((float)width, (float)height) * _imageScales[i]);
			}
		}
		base.Apply();
	}

	public ScreenShaderData UseImageOffset(Vector2 offset)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uImageOffset = offset;
		return this;
	}

	public ScreenShaderData UseIntensity(float intensity)
	{
		_uIntensity = intensity;
		return this;
	}

	public ScreenShaderData UseColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(new Vector3(r, g, b));
	}

	public ScreenShaderData UseProgress(float progress)
	{
		_uProgress = progress;
		return this;
	}

	public ScreenShaderData UseImage(Texture2D image, int index = 0, SamplerState samplerState = null)
	{
		_samplerStates[index] = samplerState;
		_uAssetImages[index] = null;
		_uCustomImages[index] = image;
		return this;
	}

	public ScreenShaderData UseImage(string path, int index = 0, SamplerState samplerState = null)
	{
		_uAssetImages[index] = Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
		_uCustomImages[index] = null;
		_samplerStates[index] = samplerState;
		return this;
	}

	public ScreenShaderData UseSceneSize(Vector2 size)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uSceneSize = size;
		return this;
	}

	public ScreenShaderData UseSceneOffset(Vector2 size)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uSceneOffset = size;
		return this;
	}

	public ScreenShaderData UseImageSize0(Vector2 size)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uImageSize0 = size;
		return this;
	}

	public ScreenShaderData UseColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(color.ToVector3());
	}

	public ScreenShaderData UseColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uColor = color;
		return this;
	}

	public ScreenShaderData UseDirection(Vector2 direction)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uDirection = direction;
		return this;
	}

	public ScreenShaderData UseGlobalOpacity(float opacity)
	{
		_globalOpacity = opacity;
		return this;
	}

	public ScreenShaderData UseTargetPosition(Vector2 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uTargetPosition = position;
		return this;
	}

	public ScreenShaderData UseSecondaryColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(new Vector3(r, g, b));
	}

	public ScreenShaderData UseSecondaryColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(color.ToVector3());
	}

	public ScreenShaderData UseSecondaryColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uSecondaryColor = color;
		return this;
	}

	public ScreenShaderData UseOpacity(float opacity)
	{
		_uOpacity = opacity;
		return this;
	}

	public ScreenShaderData UseImageScale(Vector2 scale, int index = 0)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_imageScales[index] = scale;
		return this;
	}

	public virtual ScreenShaderData GetSecondaryShader(Player player)
	{
		return this;
	}
}
