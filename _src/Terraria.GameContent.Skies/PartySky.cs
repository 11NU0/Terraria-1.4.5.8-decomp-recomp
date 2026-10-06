using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class PartySky : CustomSky
{
	private struct Balloon
	{
		private const int MAX_FRAMES_X = 3;

		private const int MAX_FRAMES_Y = 3;

		private const int FRAME_RATE = 14;

		public int Variant;

		private Texture2D _texture;

		public Vector2 Position;

		public float Depth;

		public int FrameHeight;

		public int FrameWidth;

		public float Speed;

		public bool Active;

		private int _frameCounter;

		public Texture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				_texture = value;
				FrameWidth = value.Width / 3;
				FrameHeight = value.Height / 3;
			}
		}

		public int Frame
		{
			get
			{
				return _frameCounter;
			}
			set
			{
				_frameCounter = value % 42;
			}
		}

		public Rectangle GetSourceRectangle()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle(FrameWidth * Variant, _frameCounter / 14 * FrameHeight, FrameWidth, FrameHeight);
		}
	}

	public static bool MultipleSkyWorkaroundFix;

	private bool _active;

	private bool _leaving;

	private Asset<Texture2D>[] _textures;

	private Balloon[] _balloons;

	private UnifiedRandom _random = new UnifiedRandom();

	private int _balloonsDrawing;

	public override void OnLoad()
	{
		_textures = new Asset<Texture2D>[3];
		for (int i = 0; i < _textures.Length; i++)
		{
			_textures[i] = TextureAssets.Extra[69 + i];
		}
		GenerateBalloons(onlyMissing: false);
	}

	private void GenerateBalloons(bool onlyMissing)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		if (!onlyMissing)
		{
			_balloons = new Balloon[Main.maxTilesY / 4];
		}
		for (int i = 0; i < _balloons.Length; i++)
		{
			if (!onlyMissing || !_balloons[i].Active)
			{
				int num = (int)((double)Main.screenPosition.Y * 0.7 - (double)Main.screenHeight);
				int minValue = (int)((double)num - Main.worldSurface * 16.0);
				_balloons[i].Position = new Vector2((float)(_random.Next(0, Main.maxTilesX) * 16), (float)_random.Next(minValue, num));
				ResetBalloon(i);
				_balloons[i].Active = true;
			}
		}
		_balloonsDrawing = _balloons.Length;
	}

	public void ResetBalloon(int i)
	{
		_balloons[i].Depth = (float)i / (float)_balloons.Length * 1.75f + 1.6f;
		_balloons[i].Speed = -1.5f - 2.5f * (float)_random.NextDouble();
		_balloons[i].Texture = _textures[_random.Next(2)].Value;
		_balloons[i].Variant = _random.Next(3);
		if (_random.Next(30) == 0)
		{
			_balloons[i].Texture = _textures[2].Value;
		}
	}

	public override void Update(GameTime gameTime)
	{
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if (!MultipleSkyWorkaroundFix && Main.dayRate == 0)
		{
			return;
		}
		MultipleSkyWorkaroundFix = false;
		if (FocusHelper.PauseSkies)
		{
			return;
		}
		for (int i = 0; i < _balloons.Length; i++)
		{
			if (!_balloons[i].Active)
			{
				continue;
			}
			_balloons[i].Frame++;
			_balloons[i].Position.Y += _balloons[i].Speed;
			_balloons[i].Position.X += Main.windSpeedCurrent * (3f - _balloons[i].Speed);
			if (!(_balloons[i].Position.Y < 300f))
			{
				continue;
			}
			if (!_leaving)
			{
				ResetBalloon(i);
				_balloons[i].Position = new Vector2((float)(_random.Next(0, Main.maxTilesX) * 16), (float)Main.worldSurface * 16f + 1600f);
				if (_random.Next(30) == 0)
				{
					_balloons[i].Texture = _textures[2].Value;
				}
			}
			else
			{
				_balloons[i].Active = false;
				_balloonsDrawing--;
			}
		}
		if (_balloonsDrawing == 0)
		{
			_active = false;
		}
		_active = true;
	}

	public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu && _active)
		{
			_active = false;
			_leaving = false;
			for (int i = 0; i < _balloons.Length; i++)
			{
				_balloons[i].Active = false;
			}
		}
		if ((double)Main.screenPosition.Y > Main.worldSurface * 16.0 || Main.gameMenu || Opacity <= 0f)
		{
			return;
		}
		int num = -1;
		int num2 = 0;
		for (int j = 0; j < _balloons.Length; j++)
		{
			float depth = _balloons[j].Depth;
			if (num == -1 && depth < maxDepth)
			{
				num = j;
			}
			if (depth <= minDepth)
			{
				break;
			}
			num2 = j;
		}
		if (num == -1)
		{
			return;
		}
		Vector2 val = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
		Rectangle val2 = new Rectangle(-1000, -1000, Main.screenWidth + 1000, Main.screenHeight + 1000);
		for (int k = num; k < num2; k++)
		{
			if (_balloons[k].Active)
			{
				Color val3 = new Color(Main.ColorOfTheSkies.ToVector4() * 0.9f + new Vector4(0.1f)) * 0.8f;
				float num3 = 1f;
				if (_balloons[k].Depth > 3f)
				{
					num3 = 0.6f;
				}
				else if ((double)_balloons[k].Depth > 2.5)
				{
					num3 = 0.7f;
				}
				else if (_balloons[k].Depth > 2f)
				{
					num3 = 0.8f;
				}
				else if ((double)_balloons[k].Depth > 1.5)
				{
					num3 = 0.9f;
				}
				num3 *= 0.9f;
				val3 = new Color((int)((float)(int)val3.R * num3), (int)((float)(int)val3.G * num3), (int)((float)(int)val3.B * num3), (int)((float)(int)val3.A * num3));
				Vector2 val4 = new Vector2(1f / _balloons[k].Depth, 0.9f / _balloons[k].Depth);
				Vector2 position = _balloons[k].Position;
				position = (position - val) * val4 + val - Main.screenPosition;
				position.X = (position.X + 500f) % 4000f;
				if (position.X < 0f)
				{
					position.X += 4000f;
				}
				position.X -= 500f;
				if (val2.Contains((int)position.X, (int)position.Y))
				{
					spriteBatch.Draw(_balloons[k].Texture, position, (Rectangle?)_balloons[k].GetSourceRectangle(), val3 * Opacity, 0f, Vector2.Zero, val4.X * 2f, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	public override void Activate(Vector2 position, params object[] args)
	{
		if (_active)
		{
			_leaving = false;
			GenerateBalloons(onlyMissing: true);
		}
		else
		{
			GenerateBalloons(onlyMissing: false);
			_active = true;
			_leaving = false;
		}
	}

	public override void Deactivate(params object[] args)
	{
		_leaving = true;
	}

	public override bool IsActive()
	{
		return _active;
	}

	public override void Reset()
	{
		_active = false;
	}
}
