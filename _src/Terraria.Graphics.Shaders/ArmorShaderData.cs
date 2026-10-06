using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders;

public class ArmorShaderData(Asset<Effect> shader, string passName) : ShaderData(shader, passName)
{
	private Vector3 _uColor = Vector3.One;

	private Vector3 _uSecondaryColor = Vector3.One;

	private float _uSaturation = 1f;

	private float _uOpacity = 1f;

	private Asset<Texture2D> _uImage;

	private Vector2 _uTargetPosition = Vector2.One;

	private Effect _effect;

	private EffectParameter<Vector3> uColor;

	private EffectParameter<float> uSaturation;

	private EffectParameter<Vector3> uSecondaryColor;

	private EffectParameter<float> uTime;

	private EffectParameter<float> uOpacity;

	private EffectParameter<Vector2> uTargetPosition;

	private EffectParameter<Vector4> uSourceRect;

	private EffectParameter<Vector4> uLegacyArmorSourceRect;

	private EffectParameter<Vector2> uLegacyArmorSheetSize;

	private EffectParameter<Vector2> uDrawPosition;

	private EffectParameter<float> uRotation;

	private EffectParameter<float> uDirection;

	private EffectParameter<Vector2> uImageSize0;

	private EffectParameter<Vector2> uImageSize1;

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
			uTargetPosition = Shader.GetParameter<Vector2>("uTargetPosition");
			uSourceRect = Shader.GetParameter<Vector4>("uSourceRect");
			uLegacyArmorSourceRect = Shader.GetParameter<Vector4>("uLegacyArmorSourceRect");
			uLegacyArmorSheetSize = Shader.GetParameter<Vector2>("uLegacyArmorSheetSize");
			uDrawPosition = Shader.GetParameter<Vector2>("uDrawPosition");
			uRotation = Shader.GetParameter<float>("uRotation");
			uDirection = Shader.GetParameter<float>("uDirection");
			uImageSize0 = Shader.GetParameter<Vector2>("uImageSize0");
			uImageSize1 = Shader.GetParameter<Vector2>("uImageSize1");
		}
	}

	public virtual void Apply(Entity entity, DrawData? drawData = null)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		CheckCachedParameters();
		uColor.SetValue(_uColor);
		uSaturation.SetValue(_uSaturation);
		uSecondaryColor.SetValue(_uSecondaryColor);
		uTime.SetValue(Main.GlobalTimeWrappedHourly);
		uOpacity.SetValue(_uOpacity);
		uTargetPosition.SetValue(_uTargetPosition);
		if (drawData.HasValue)
		{
			DrawData value = drawData.Value;
			Vector4 value2 = ((!value.sourceRect.HasValue) ? new Vector4(0f, 0f, (float)value.texture.Width, (float)value.texture.Height) : new Vector4((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height));
			uSourceRect.SetValue(value2);
			uLegacyArmorSourceRect.SetValue(value2);
			uDrawPosition.SetValue(value.position);
			uImageSize0.SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
			uLegacyArmorSheetSize.SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
			uRotation.SetValue(value.rotation * ((((int)value.effect & 1) != 0) ? (-1f) : 1f));
			uDirection.SetValue((((int)value.effect & 1) == 0) ? 1 : (-1));
		}
		else
		{
			Vector4 value3 = new Vector4(0f, 0f, 4f, 4f);
			uSourceRect.SetValue(value3);
			uLegacyArmorSourceRect.SetValue(value3);
			uRotation.SetValue(0f);
		}
		if (_uImage != null)
		{
			Main.graphics.GraphicsDevice.Textures[1] = (Texture)(object)_uImage.Value;
			uImageSize1.SetValue(new Vector2((float)_uImage.Width(), (float)_uImage.Height()));
		}
		if (entity != null)
		{
			uDirection.SetValue(entity.direction);
		}
		if (entity is Player { bodyFrame: var bodyFrame })
		{
			uLegacyArmorSourceRect.SetValue(new Vector4((float)bodyFrame.X, (float)bodyFrame.Y, (float)bodyFrame.Width, (float)bodyFrame.Height));
			uLegacyArmorSheetSize.SetValue(new Vector2(40f, 1120f));
		}
		Apply();
	}

	public ArmorShaderData UseColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(new Vector3(r, g, b));
	}

	public ArmorShaderData UseColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(color.ToVector3());
	}

	public ArmorShaderData UseColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uColor = color;
		return this;
	}

	public ArmorShaderData UseImage(string path)
	{
		if (!Main.dedServ)
		{
			_uImage = Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
		}
		return this;
	}

	public ArmorShaderData UseOpacity(float alpha)
	{
		_uOpacity = alpha;
		return this;
	}

	public ArmorShaderData UseTargetPosition(Vector2 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uTargetPosition = position;
		return this;
	}

	public ArmorShaderData UseSecondaryColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(new Vector3(r, g, b));
	}

	public ArmorShaderData UseSecondaryColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(color.ToVector3());
	}

	public ArmorShaderData UseSecondaryColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uSecondaryColor = color;
		return this;
	}

	public ArmorShaderData UseSaturation(float saturation)
	{
		_uSaturation = saturation;
		return this;
	}

	public virtual ArmorShaderData GetSecondaryShader(Entity entity)
	{
		return this;
	}
}
