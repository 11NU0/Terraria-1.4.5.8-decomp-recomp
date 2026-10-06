using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class VortexSky : CustomSky
{
	private struct Bolt
	{
		public Vector2 Position;

		public float Depth;

		public int Life;

		public bool IsAlive;
	}

	private UnifiedRandom _random = new UnifiedRandom();

	private Asset<Texture2D> _planetTexture;

	private Asset<Texture2D> _bgTexture;

	private Asset<Texture2D> _boltTexture;

	private Asset<Texture2D> _flashTexture;

	private bool _isActive;

	private int _ticksUntilNextBolt;

	private float _fadeOpacity;

	private Bolt[] _bolts;

	public override void OnLoad()
	{
		_planetTexture = Main.Assets.Request<Texture2D>("Images/Misc/VortexSky/Planet", (AssetRequestMode)1);
		_bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/VortexSky/Background", (AssetRequestMode)1);
		_boltTexture = Main.Assets.Request<Texture2D>("Images/Misc/VortexSky/Bolt", (AssetRequestMode)1);
		_flashTexture = Main.Assets.Request<Texture2D>("Images/Misc/VortexSky/Flash", (AssetRequestMode)1);
	}

	public override void Update(GameTime gameTime)
	{
		if (_isActive)
		{
			_fadeOpacity = Math.Min(1f, 0.01f + _fadeOpacity);
		}
		else
		{
			_fadeOpacity = Math.Max(0f, _fadeOpacity - 0.01f);
		}
		if (_ticksUntilNextBolt <= 0)
		{
			_ticksUntilNextBolt = _random.Next(1, 5);
			int i;
			for (i = 0; _bolts[i].IsAlive && i != _bolts.Length - 1; i++)
			{
			}
			_bolts[i].IsAlive = true;
			_bolts[i].Position.X = _random.NextFloat() * ((float)Main.maxTilesX * 16f + 4000f) - 2000f;
			_bolts[i].Position.Y = _random.NextFloat() * 500f;
			_bolts[i].Depth = _random.NextFloat() * 8f + 2f;
			_bolts[i].Life = 30;
		}
		_ticksUntilNextBolt--;
		for (int j = 0; j < _bolts.Length; j++)
		{
			if (_bolts[j].IsAlive)
			{
				_bolts[j].Life--;
				if (_bolts[j].Life <= 0)
				{
					_bolts[j].IsAlive = false;
				}
			}
		}
	}

	public override Color OnTileColor(Color inColor)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		return new Color(Vector4.Lerp(inColor.ToVector4(), Vector4.One, _fadeOpacity * 0.5f));
	}

	public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		if (maxDepth >= float.MaxValue && minDepth < float.MaxValue)
		{
			spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * _fadeOpacity);
			spriteBatch.Draw(_bgTexture.Value, new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 2400.0) * 0.10000000149011612)), Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f) * _fadeOpacity);
			Vector2 val = new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
			Vector2 val2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
			spriteBatch.Draw(_planetTexture.Value, val + new Vector2(-200f, -200f) + val2, (Rectangle?)null, Color.White * 0.9f * _fadeOpacity, 0f, new Vector2((float)(_planetTexture.Width() >> 1), (float)(_planetTexture.Height() >> 1)), 1f, (SpriteEffects)0, 0f);
		}
		float num = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
		Vector2 val3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
		Rectangle val4 = new Rectangle(-1000, -1000, Main.screenWidth + 1000, Main.screenHeight + 1000);
		for (int i = 0; i < _bolts.Length; i++)
		{
			if (!_bolts[i].IsAlive || !(_bolts[i].Depth > minDepth) || !(_bolts[i].Depth < maxDepth))
			{
				continue;
			}
			Vector2 val5 = new Vector2(1f / _bolts[i].Depth, 0.9f / _bolts[i].Depth);
			Vector2 val6 = (_bolts[i].Position - val3) * val5 + val3 - Main.screenPosition;
			if (val4.Contains((int)val6.X, (int)val6.Y))
			{
				Texture2D value = _boltTexture.Value;
				int life = _bolts[i].Life;
				if (life > 26 && life % 2 == 0)
				{
					value = _flashTexture.Value;
				}
				float num2 = (float)life / 30f;
				spriteBatch.Draw(value, val6, (Rectangle?)null, Color.White * num * num2 * _fadeOpacity, 0f, Vector2.Zero, val5.X * 5f, (SpriteEffects)0, 0f);
			}
		}
	}

	public override float GetCloudAlpha()
	{
		return (1f - _fadeOpacity) * 0.3f + 0.7f;
	}

	public override void Activate(Vector2 position, params object[] args)
	{
		_fadeOpacity = 0.002f;
		_isActive = true;
		_bolts = new Bolt[500];
		for (int i = 0; i < _bolts.Length; i++)
		{
			_bolts[i].IsAlive = false;
		}
	}

	public override void Deactivate(params object[] args)
	{
		_isActive = false;
	}

	public override void Reset()
	{
		_isActive = false;
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
