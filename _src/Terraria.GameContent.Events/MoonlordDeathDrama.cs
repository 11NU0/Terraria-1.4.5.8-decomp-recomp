using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Utilities;

namespace Terraria.GameContent.Events;

public class MoonlordDeathDrama
{
	public class MoonlordPiece
	{
		private Texture2D _texture;

		private Vector2 _position;

		private Vector2 _velocity;

		private Vector2 _origin;

		private float _rotation;

		private float _rotationVelocity;

		public bool Dead
		{
			get
			{
				if (!(_position.Y > (float)(Main.maxTilesY * 16) - 480f) && !(_position.X < 480f))
				{
					return _position.X >= (float)(Main.maxTilesX * 16) - 480f;
				}
				return true;
			}
		}

		public MoonlordPiece(Texture2D pieceTexture, Vector2 textureOrigin, Vector2 centerPos, Vector2 velocity, float rot, float angularVelocity)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			_texture = pieceTexture;
			_origin = textureOrigin;
			_position = centerPos;
			_velocity = velocity;
			_rotation = rot;
			_rotationVelocity = angularVelocity;
		}

		public void Update()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			_velocity.Y += 0.3f;
			_rotation += _rotationVelocity;
			_rotationVelocity *= 0.99f;
			_position += _velocity;
		}

		public void Draw(SpriteBatch sp)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			Color light = GetLight();
			sp.Draw(_texture, _position - Main.screenPosition, (Rectangle?)null, light, _rotation, _origin, 1f, (SpriteEffects)0, 0f);
		}

		public bool InDrawRange(Rectangle playerScreen)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return playerScreen.Contains(_position.ToPoint());
		}

		public Color GetLight()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = Vector3.Zero;
			float num = 0f;
			int num2 = 5;
			Point val2 = _position.ToTileCoordinates();
			for (int i = val2.X - num2; i <= val2.X + num2; i++)
			{
				for (int j = val2.Y - num2; j <= val2.Y + num2; j++)
				{
					Vector3 val3 = val;
					Color color = Lighting.GetColor(i, j);
					val = val3 + color.ToVector3();
					num++;
				}
			}
			if (num == 0f)
			{
				return Color.White;
			}
			return new Color(val / num);
		}
	}

	public class MoonlordExplosion
	{
		private Texture2D _texture;

		private Vector2 _position;

		private Vector2 _origin;

		private Rectangle _frame;

		private int _frameCounter;

		private int _frameSpeed;

		public bool Dead
		{
			get
			{
				if (!(_position.Y > (float)(Main.maxTilesY * 16) - 480f) && !(_position.X < 480f) && !(_position.X >= (float)(Main.maxTilesX * 16) - 480f))
				{
					return _frameCounter >= _frameSpeed * 7;
				}
				return true;
			}
		}

		public MoonlordExplosion(Texture2D pieceTexture, Vector2 centerPos, int frameSpeed)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			_texture = pieceTexture;
			_position = centerPos;
			_frameSpeed = frameSpeed;
			_frameCounter = 0;
			_frame = _texture.Frame(1, 7);
			_origin = _frame.Size() / 2f;
		}

		public void Update()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_frameCounter++;
			_frame = _texture.Frame(1, 7, 0, _frameCounter / _frameSpeed);
		}

		public void Draw(SpriteBatch sp)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Color light = GetLight();
			sp.Draw(_texture, _position - Main.screenPosition, (Rectangle?)_frame, light, 0f, _origin, 1f, (SpriteEffects)0, 0f);
		}

		public bool InDrawRange(Rectangle playerScreen)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return playerScreen.Contains(_position.ToPoint());
		}

		public Color GetLight()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return new Color(255, 255, 255, 127);
		}
	}

	private static List<MoonlordPiece> _pieces = new List<MoonlordPiece>();

	private static List<MoonlordExplosion> _explosions = new List<MoonlordExplosion>();

	private static List<Vector2> _lightSources = new List<Vector2>();

	private static float whitening;

	private static float requestedLight;

	public static void Update(SceneState sceneState, SceneMetrics metrics)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _pieces.Count; i++)
		{
			MoonlordPiece moonlordPiece = _pieces[i];
			moonlordPiece.Update();
			if (moonlordPiece.Dead)
			{
				_pieces.Remove(moonlordPiece);
				i--;
			}
		}
		for (int j = 0; j < _explosions.Count; j++)
		{
			MoonlordExplosion moonlordExplosion = _explosions[j];
			moonlordExplosion.Update();
			if (moonlordExplosion.Dead)
			{
				_explosions.Remove(moonlordExplosion);
				j--;
			}
		}
		bool flag = false;
		for (int k = 0; k < _lightSources.Count; k++)
		{
			if (metrics.Center.Distance(_lightSources[k]) < 2000f)
			{
				flag = true;
				break;
			}
		}
		_lightSources.Clear();
		if (!flag)
		{
			requestedLight = 0f;
		}
		sceneState.MoveTowards(ref whitening, requestedLight, 0.02f);
		requestedLight = 0f;
	}

	public static void DrawPieces(SpriteBatch spriteBatch)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Rectangle playerScreen = Utils.CenteredRectangle(Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f, new Vector2((float)(Main.screenWidth + 1000), (float)(Main.screenHeight + 1000)));
		for (int i = 0; i < _pieces.Count; i++)
		{
			if (_pieces[i].InDrawRange(playerScreen))
			{
				_pieces[i].Draw(spriteBatch);
			}
		}
	}

	public static void DrawExplosions(SpriteBatch spriteBatch)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Rectangle playerScreen = Utils.CenteredRectangle(Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f, new Vector2((float)(Main.screenWidth + 1000), (float)(Main.screenHeight + 1000)));
		for (int i = 0; i < _explosions.Count; i++)
		{
			if (_explosions[i].InDrawRange(playerScreen))
			{
				_explosions[i].Draw(spriteBatch);
			}
		}
	}

	public static void DrawWhite(SpriteBatch spriteBatch)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (whitening != 0f)
		{
			Color val = Color.White * whitening;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), (Rectangle?)new Rectangle(0, 0, 1, 1), val);
		}
	}

	public static void ThrowPieces(Vector2 MoonlordCoreCenter, int DramaSeed)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom r = new UnifiedRandom(DramaSeed);
		Vector2 val = Vector2.UnitY.RotatedBy(r.NextFloat() * ((float)Math.PI / 2f) - (float)Math.PI / 4f + (float)Math.PI);
		_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Spine", (AssetRequestMode)1).Value, new Vector2(64f, 150f), MoonlordCoreCenter + new Vector2(0f, 50f), val * 6f, 0f, r.NextFloat() * 0.1f - 0.05f));
		val = Vector2.UnitY.RotatedBy(r.NextFloat() * ((float)Math.PI / 2f) - (float)Math.PI / 4f + (float)Math.PI);
		_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Shoulder", (AssetRequestMode)1).Value, new Vector2(40f, 120f), MoonlordCoreCenter + new Vector2(50f, -120f), val * 10f, 0f, r.NextFloat() * 0.1f - 0.05f));
		val = Vector2.UnitY.RotatedBy(r.NextFloat() * ((float)Math.PI / 2f) - (float)Math.PI / 4f + (float)Math.PI);
		_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Torso", (AssetRequestMode)1).Value, new Vector2(192f, 252f), MoonlordCoreCenter, val * 8f, 0f, r.NextFloat() * 0.1f - 0.05f));
		val = Vector2.UnitY.RotatedBy(r.NextFloat() * ((float)Math.PI / 2f) - (float)Math.PI / 4f + (float)Math.PI);
		_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Head", (AssetRequestMode)1).Value, new Vector2(138f, 185f), MoonlordCoreCenter - new Vector2(0f, 200f), val * 12f, 0f, r.NextFloat() * 0.1f - 0.05f));
	}

	public static void AddExplosion(Vector2 spot)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		_explosions.Add(new MoonlordExplosion(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Explosion", (AssetRequestMode)1).Value, spot, Main.rand.Next(2, 4)));
	}

	public static void RequestLight(float light, Vector2 spot)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		_lightSources.Add(spot);
		if (light > 1f)
		{
			light = 1f;
		}
		if (requestedLight < light)
		{
			requestedLight = light;
		}
	}
}
