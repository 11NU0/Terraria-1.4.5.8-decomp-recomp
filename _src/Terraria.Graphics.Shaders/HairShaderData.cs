using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders;

public class HairShaderData : ShaderData
{
	protected Vector3 _uColor = Vector3.One;

	protected Vector3 _uSecondaryColor = Vector3.One;

	protected float _uSaturation = 1f;

	protected float _uOpacity = 1f;

	protected Asset<Texture2D> _uImage;

	protected bool _shaderDisabled;

	private Vector2 _uTargetPosition = Vector2.One;

	private Effect _effect;

	private EffectParameter<Vector3> uColor;

	private EffectParameter<float> uSaturation;

	private EffectParameter<Vector3> uSecondaryColor;

	private EffectParameter<float> uTime;

	private EffectParameter<float> uOpacity;

	private EffectParameter<float> uDirection;

	private EffectParameter<Vector4> uSourceRect;

	private EffectParameter<Vector2> uDrawPosition;

	private EffectParameter<Vector2> uTargetPosition;

	private EffectParameter<Vector2> uImageSize0;

	private EffectParameter<Vector2> uImageSize1;

	public bool ShaderDisabled => _shaderDisabled;

	public HairShaderData(Asset<Effect> shader, string passName)
		: base(shader, passName)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
	}

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
			uDirection = Shader.GetParameter<float>("uDirection");
			uSourceRect = Shader.GetParameter<Vector4>("uSourceRect");
			uDrawPosition = Shader.GetParameter<Vector2>("uDrawPosition");
			uTargetPosition = Shader.GetParameter<Vector2>("uTargetPosition");
			uImageSize0 = Shader.GetParameter<Vector2>("uImageSize0");
			uImageSize1 = Shader.GetParameter<Vector2>("uImageSize1");
		}
	}

	public virtual void Apply(Player player, DrawData? drawData = null)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		if (!_shaderDisabled)
		{
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
				Vector4 value2 = new Vector4((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height);
				uSourceRect.SetValue(value2);
				uDrawPosition.SetValue(value.position);
				uImageSize0.SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
			}
			else
			{
				uSourceRect.SetValue(new Vector4(0f, 0f, 4f, 4f));
			}
			if (_uImage != null)
			{
				Main.graphics.GraphicsDevice.Textures[1] = (Texture)(object)_uImage.Value;
				uImageSize1.SetValue(new Vector2((float)_uImage.Width(), (float)_uImage.Height()));
			}
			if (player != null)
			{
				uDirection.SetValue(player.direction);
			}
			Apply();
		}
	}

	public virtual Color GetColor(Player player, Color lightColor)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return new Color(lightColor.ToVector4() * player.hairColor.ToVector4());
	}

	public HairShaderData UseColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(new Vector3(r, g, b));
	}

	public HairShaderData UseColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(color.ToVector3());
	}

	public HairShaderData UseColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uColor = color;
		return this;
	}

	public HairShaderData UseImage(string path)
	{
		if (!Main.dedServ)
		{
			_uImage = Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
		}
		return this;
	}

	public HairShaderData UseOpacity(float alpha)
	{
		_uOpacity = alpha;
		return this;
	}

	public HairShaderData UseSecondaryColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(new Vector3(r, g, b));
	}

	public HairShaderData UseSecondaryColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(color.ToVector3());
	}

	public HairShaderData UseSecondaryColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uSecondaryColor = color;
		return this;
	}

	public HairShaderData UseSaturation(float saturation)
	{
		_uSaturation = saturation;
		return this;
	}

	public HairShaderData UseTargetPosition(Vector2 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uTargetPosition = position;
		return this;
	}
}
