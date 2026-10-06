using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class MoonLordSky : CustomSky
{
	private UnifiedRandom _random = new UnifiedRandom();

	private bool _isActive;

	private bool _forPlayer;

	private float _fadeOpacity;

	public MoonLordSky(bool forPlayer)
	{
		_forPlayer = forPlayer;
	}

	public override void OnLoad()
	{
	}

	public override void Update(GameTime gameTime)
	{
		if (_forPlayer)
		{
			if (_isActive)
			{
				_fadeOpacity = Math.Min(1f, 0.01f + _fadeOpacity);
			}
			else
			{
				_fadeOpacity = Math.Max(0f, _fadeOpacity - 0.01f);
			}
		}
	}

	private float GetIntensity()
	{
		if (_forPlayer)
		{
			return _fadeOpacity;
		}
		float? moonLordSkyIntensity = Main.SceneMetrics.MoonLordSkyIntensity;
		if (moonLordSkyIntensity.HasValue)
		{
			return moonLordSkyIntensity.Value;
		}
		return 0f;
	}

	public override Color OnTileColor(Color inColor)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		float intensity = GetIntensity();
		return new Color(Vector4.Lerp(new Vector4(0.5f, 0.8f, 1f, 1f), inColor.ToVector4(), 1f - intensity));
	}

	public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (maxDepth >= 0f && minDepth < 0f)
		{
			float intensity = GetIntensity();
			spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
		}
	}

	public override float GetCloudAlpha()
	{
		return 1f - _fadeOpacity;
	}

	public override void Activate(Vector2 position, params object[] args)
	{
		_isActive = true;
		if (_forPlayer)
		{
			_fadeOpacity = 0.002f;
		}
		else
		{
			_fadeOpacity = 1f;
		}
	}

	public override void Deactivate(params object[] args)
	{
		_isActive = false;
		if (!_forPlayer)
		{
			_fadeOpacity = 0f;
		}
	}

	public override void Reset()
	{
		_isActive = false;
		_fadeOpacity = 0f;
	}

	public override bool IsActive()
	{
		if (!_isActive)
		{
			return _fadeOpacity > 0.001f;
		}
		return true;
	}
}
