using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class SolarSky : CustomSky
{
	private struct Meteor
	{
		public Vector2 Position;

		public float Depth;

		public int FrameCounter;

		public float Scale;

		public float StartX;
	}

	private UnifiedRandom _random = new UnifiedRandom();

	private Asset<Texture2D> _planetTexture;

	private Asset<Texture2D> _bgTexture;

	private Asset<Texture2D> _meteorTexture;

	private bool _isActive;

	private Meteor[] _meteors;

	private float _fadeOpacity;

	public override void OnLoad()
	{
		_planetTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Planet", (AssetRequestMode)1);
		_bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Background", (AssetRequestMode)1);
		_meteorTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Meteor", (AssetRequestMode)1);
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
		float num = 1200f;
		for (int i = 0; i < _meteors.Length; i++)
		{
			_meteors[i].Position.X -= num * (float)gameTime.ElapsedGameTime.TotalSeconds;
			_meteors[i].Position.Y += num * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if ((double)_meteors[i].Position.Y > Main.worldSurface * 16.0)
			{
				_meteors[i].Position.X = _meteors[i].StartX;
				_meteors[i].Position.Y = -10000f;
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		if (maxDepth >= float.MaxValue && minDepth < float.MaxValue)
		{
			spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * _fadeOpacity);
			spriteBatch.Draw(_bgTexture.Value, new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 2400.0) * 0.10000000149011612)), Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f * _fadeOpacity));
			Vector2 val = new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
			Vector2 val2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
			spriteBatch.Draw(_planetTexture.Value, val + new Vector2(-200f, -200f) + val2, (Rectangle?)null, Color.White * 0.9f * _fadeOpacity, 0f, new Vector2((float)(_planetTexture.Width() >> 1), (float)(_planetTexture.Height() >> 1)), 1f, (SpriteEffects)0, 0f);
		}
		int num = -1;
		int num2 = 0;
		for (int i = 0; i < _meteors.Length; i++)
		{
			float depth = _meteors[i].Depth;
			if (num == -1 && depth < maxDepth)
			{
				num = i;
			}
			if (depth <= minDepth)
			{
				break;
			}
			num2 = i;
		}
		if (num == -1)
		{
			return;
		}
		float num3 = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
		Vector2 val3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
		Rectangle val4 = new Rectangle(-1000, -1000, Main.screenWidth + 1000, Main.screenHeight + 1000);
		for (int j = num; j < num2; j++)
		{
			Vector2 val5 = new Vector2(1f / _meteors[j].Depth, 0.9f / _meteors[j].Depth);
			Vector2 val6 = (_meteors[j].Position - val3) * val5 + val3 - Main.screenPosition;
			int num4 = _meteors[j].FrameCounter / 3;
			_meteors[j].FrameCounter = (_meteors[j].FrameCounter + 1) % 12;
			if (val4.Contains((int)val6.X, (int)val6.Y))
			{
				spriteBatch.Draw(_meteorTexture.Value, val6, (Rectangle?)new Rectangle(0, num4 * (_meteorTexture.Height() / 4), _meteorTexture.Width(), _meteorTexture.Height() / 4), Color.White * num3 * _fadeOpacity, 0f, Vector2.Zero, val5.X * 5f * _meteors[j].Scale, (SpriteEffects)0, 0f);
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
		_meteors = new Meteor[150];
		for (int i = 0; i < _meteors.Length; i++)
		{
			float num = (float)i / (float)_meteors.Length;
			_meteors[i].Position.X = num * ((float)Main.maxTilesX * 16f) + _random.NextFloat() * 40f - 20f;
			_meteors[i].Position.Y = _random.NextFloat() * (0f - ((float)Main.worldSurface * 16f + 10000f)) - 10000f;
			if (_random.Next(3) != 0)
			{
				_meteors[i].Depth = _random.NextFloat() * 3f + 1.8f;
			}
			else
			{
				_meteors[i].Depth = _random.NextFloat() * 5f + 4.8f;
			}
			_meteors[i].FrameCounter = _random.Next(12);
			_meteors[i].Scale = _random.NextFloat() * 0.5f + 1f;
			_meteors[i].StartX = _meteors[i].Position.X;
		}
		Array.Sort(_meteors, SortMethod);
	}

	private int SortMethod(Meteor meteor1, Meteor meteor2)
	{
		return meteor2.Depth.CompareTo(meteor1.Depth);
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
