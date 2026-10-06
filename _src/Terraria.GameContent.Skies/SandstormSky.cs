using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Events;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class SandstormSky : CustomSky
{
	private UnifiedRandom _random = new UnifiedRandom();

	private bool _isActive;

	private bool _isLeaving;

	private float _opacity;

	public override void OnLoad()
	{
	}

	public override void Update(GameTime gameTime)
	{
		if (FocusHelper.PauseSkies)
		{
			return;
		}
		if (_isLeaving)
		{
			_opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (_opacity < 0f)
			{
				_isActive = false;
				_opacity = 0f;
			}
		}
		else
		{
			_opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (_opacity > 1f)
			{
				_opacity = 1f;
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		if (minDepth < 1f || maxDepth == float.MaxValue)
		{
			float num = Math.Min(1f, Sandstorm.Severity * 1.5f);
			Color val = new Color(new Vector4(0.85f, 0.66f, 0.33f, 1f) * 0.8f * Main.ColorOfTheSkies.ToVector4()) * _opacity * num;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), val);
		}
	}

	public override void Activate(Vector2 position, params object[] args)
	{
		_isActive = true;
		_isLeaving = false;
	}

	public override void Deactivate(params object[] args)
	{
		_isLeaving = true;
	}

	public override void Reset()
	{
		_opacity = 0f;
		_isActive = false;
	}

	public override bool IsActive()
	{
		return _isActive;
	}
}
