using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers;

public class LittleFlyingCritterParticle : IPooledParticle, IParticle, IParticleRepel
{
	public enum FlyType
	{
		RegularFly,
		ButterFly
	}

	private int _lifeTimeCounted;

	private int _lifeTimeTotal;

	private Vector2 _spawnPosition;

	private Vector2 _localPosition;

	private Vector2 _velocity;

	private float _neverGoBelowThis;

	private Vector2 _addedVelocity;

	private int _repelLifetimeDecay;

	private Color _overrideColor;

	private FlyType _type;

	private int _variantRow;

	private int _variantColumn;

	public bool IsRestingInPool { get; private set; }

	public bool ShouldBeRemovedFromRenderer { get; private set; }

	public void Prepare(FlyType type, Vector2 position, int duration, Color overrideColor = default(Color), int repelLifetimeDecay = 0)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		_type = type;
		_variantRow = Main.rand.Next(8);
		_variantColumn = ((Main.rand.Next(5) == 0) ? 1 : 0);
		_spawnPosition = position;
		_localPosition = position + Main.rand.NextVector2Circular(4f, 8f);
		_neverGoBelowThis = position.Y + 8f;
		RandomizeVelocity();
		_lifeTimeCounted = 0;
		_lifeTimeTotal = 300 + Main.rand.Next(6) * 60;
		_overrideColor = overrideColor;
		_repelLifetimeDecay = repelLifetimeDecay;
	}

	private void RandomizeVelocity()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		_velocity = Main.rand.NextVector2Circular(1f, 1f);
	}

	public void RestInPool()
	{
		IsRestingInPool = true;
	}

	public virtual void FetchFromPool()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		IsRestingInPool = false;
		ShouldBeRemovedFromRenderer = false;
		_addedVelocity = Vector2.Zero;
	}

	public void Update(ref ParticleRendererSettings settings)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		if (++_lifeTimeCounted >= _lifeTimeTotal)
		{
			ShouldBeRemovedFromRenderer = true;
		}
		float num = 0.02f;
		int num2 = 30;
		if (_type == FlyType.ButterFly)
		{
			num = 0.01f;
			num2 = 600;
		}
		_velocity += new Vector2((float)Math.Sign(_spawnPosition.X - _localPosition.X) * num, (float)Math.Sign(_spawnPosition.Y - _localPosition.Y) * num);
		if (_lifeTimeCounted % num2 == 0 && Main.rand.Next(2) == 0)
		{
			RandomizeVelocity();
			if (Main.rand.Next(2) == 0)
			{
				_velocity /= 2f;
			}
		}
		_addedVelocity *= 0.98f;
		if (_addedVelocity.Length() < 0.01f)
		{
			_addedVelocity = new Vector2(0f, 0f);
		}
		_localPosition += _velocity + _addedVelocity;
		if (_localPosition.Y > _neverGoBelowThis)
		{
			_localPosition.Y = _neverGoBelowThis;
			if (_velocity.Y > 0f)
			{
				_velocity.Y *= -1f;
			}
			if (_addedVelocity.Y > 0f)
			{
				_addedVelocity.Y *= -1f;
			}
		}
	}

	public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = settings.AnchorPosition + _localPosition;
		if (val.X < -10f || val.X > (float)(Main.screenWidth + 10) || val.Y < -10f || val.Y > (float)(Main.screenHeight + 10))
		{
			ShouldBeRemovedFromRenderer = true;
			return;
		}
		switch (_type)
		{
		case FlyType.RegularFly:
			Draw_Fly(ref settings, spritebatch);
			break;
		case FlyType.ButterFly:
			Draw_ButterFly(ref settings, spritebatch);
			break;
		}
	}

	private void Draw_ButterFly(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = _velocity + _addedVelocity;
		Texture2D value = TextureAssets.Extra[281].Value;
		int num = _lifeTimeCounted % 10 / 5;
		int variantRow = _variantRow;
		bool flag = _variantColumn == 1;
		Rectangle val2 = new Rectangle(flag ? 10 : 0, (variantRow * 2 + num) * 10, flag ? 14 : 8, 8);
		Vector2 val3 = val2.Size() / 2f;
		float num2 = Utils.Remap(_lifeTimeCounted, 0f, 90f, 0f, 1f) * Utils.Remap(_lifeTimeCounted, _lifeTimeTotal - 90, _lifeTimeTotal, 1f, 0f);
		Color color = Lighting.GetColor(_localPosition.ToTileCoordinates());
		_overrideColor = Color.White;
		Vector4 val4 = _overrideColor.ToVector4() * color.ToVector4();
		Color val5 = new Color(val4);
		float num3 = 0.75f;
		spritebatch.Draw(value, settings.AnchorPosition + _localPosition, (Rectangle?)val2, val5 * num2, 0f, val3, num3, (val.X < 0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
	}

	private void Draw_Fly(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = _velocity + _addedVelocity;
		Texture2D value = TextureAssets.Extra[262].Value;
		int frameY = _lifeTimeCounted % 6 / 3;
		Rectangle value2 = value.Frame(1, 6, 0, frameY);
		Vector2 val2 = new Vector2((float)((!(val.X > 0f)) ? 1 : 3), 3f);
		float num = Utils.Remap(_lifeTimeCounted, 0f, 90f, 0f, 1f) * Utils.Remap(_lifeTimeCounted, _lifeTimeTotal - 90, _lifeTimeTotal, 1f, 0f);
		Color color = Lighting.GetColor(_localPosition.ToTileCoordinates());
		if (_overrideColor == default(Color))
		{
			spritebatch.Draw(value, settings.AnchorPosition + _localPosition, (Rectangle?)value2, color * num, 0f, val2, 1f, (val.X > 0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
			return;
		}
		Vector4 val3 = _overrideColor.ToVector4() * color.ToVector4();
		Color val4 = new Color(val3);
		value2.Offset(0, 12);
		spritebatch.Draw(value, settings.AnchorPosition + _localPosition, (Rectangle?)value2, val4 * num, 0f, val2, 1f, (val.X > 0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
		value2.Offset(0, 12);
		spritebatch.Draw(value, settings.AnchorPosition + _localPosition, (Rectangle?)value2, color * num, 0f, val2, 1f, (val.X > 0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
	}

	public void BeRepelled(ref ParticleRepelDetails details)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		float num = Utils.Remap(_localPosition.Distance(details.SourcePosition) - details.Radius, 0f, 100f, 1f, 0f);
		if (!(num <= 0f))
		{
			Vector2 val = _localPosition.DirectionFrom(details.SourcePosition).SafeNormalize(-Vector2.UnitY).RotatedByRandom(0.5235987901687622);
			_addedVelocity = val * 3.5f * num;
			_lifeTimeCounted += _repelLifetimeDecay;
		}
	}
}
