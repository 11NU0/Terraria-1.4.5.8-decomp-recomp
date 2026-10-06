using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria.DataStructures;

public class RichRoomCheckFeedback : IRoomCheckFeedback, IRoomCheckFeedback_Spread, IRoomCheckFeedback_Scoring
{
	private enum Reason
	{
		BlockedWall,
		UnsafeWall,
		OpenAir,
		Good,
		Hazard
	}

	private struct ParticlePreparation
	{
		public Reason type;

		public int x;

		public int y;

		public int iteration;

		public bool consumed;
	}

	private struct ScorePreparation
	{
		public int x;

		public int y;

		public int score;
	}

	public static RichRoomCheckFeedback Instance = new RichRoomCheckFeedback();

	private static ParticlePool<RoomCheckParticle> _particlePool = new ParticlePool<RoomCheckParticle>(100, GetNewParticle);

	private ParticlePreparation[] _space = new ParticlePreparation[128];

	private int _spaceCount;

	private int _highestIteration;

	private ScorePreparation[] _score = new ScorePreparation[128];

	private int _scoreCount;

	private ScorePreparation _bestScore;

	private int _originX;

	private int _originY;

	public bool StopOnFail => false;

	public bool DisplayText => false;

	private static RoomCheckParticle GetNewParticle()
	{
		return new RoomCheckParticle();
	}

	private void Add(int x, int y, int iteration, Reason type)
	{
		if (_spaceCount >= _space.Length)
		{
			Array.Resize(ref _space, _space.Length * 2);
		}
		if (_highestIteration < iteration)
		{
			_highestIteration = iteration;
		}
		_space[_spaceCount++] = new ParticlePreparation
		{
			type = type,
			x = x,
			y = y,
			iteration = iteration
		};
	}

	public void BeginSpread(int x, int y)
	{
		_spaceCount = 0;
		_highestIteration = 0;
		_originX = x;
		_originY = y;
	}

	public void StartedInASolidTile(int x, int y)
	{
		Add(x, y, 0, Reason.BlockedWall);
	}

	public void TooCloseToWorldEdge(int x, int y, int iteration)
	{
		Add(x, y, iteration, Reason.OpenAir);
	}

	public void AnyBlockScannedHere(int x, int y, int iteration)
	{
		Add(x, y, iteration, Reason.Good);
	}

	public void RoomTooBig(int x, int y, int iteration)
	{
		Add(x, y, iteration, Reason.OpenAir);
	}

	public void BlockingWall(int x, int y, int iteration)
	{
		Add(x, y, iteration, Reason.BlockedWall);
	}

	public void BlockingOpenGate(int x, int y, int iteration)
	{
		Add(x, y, iteration, Reason.BlockedWall);
	}

	public void Stinkbug(int x, int y, int iteration)
	{
		Add(x, y, iteration, Reason.Hazard);
	}

	public void EchoStinkbug(int x, int y, int iteration)
	{
		Add(x, y, iteration, Reason.Hazard);
	}

	public void MissingAWall(int x, int y, int iteration)
	{
		Add(x, y, iteration, Reason.OpenAir);
	}

	public void UnsafeWall(int x, int y, int iteration)
	{
		Add(x, y, iteration, Reason.UnsafeWall);
	}

	public void EndSpread()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		Vector2 origin = new Vector2((float)_originX, (float)_originY);
		for (int i = 0; i < _spaceCount; i++)
		{
			ParticlePreparation particlePreparation = _space[i];
			for (int j = 0; j < _spaceCount; j++)
			{
				ParticlePreparation particlePreparation2 = _space[j];
				if (particlePreparation.x == particlePreparation2.x && particlePreparation.y == particlePreparation2.y && particlePreparation.type == Reason.Good && particlePreparation2.type != Reason.Good)
				{
					particlePreparation.consumed = true;
				}
			}
			_space[i] = particlePreparation;
		}
		float highestDistanceFromOrigin = GetHighestDistanceFromOrigin(ref origin);
		float num = 3f * highestDistanceFromOrigin + 60f;
		for (int k = 0; k < _spaceCount; k++)
		{
			ParticlePreparation particlePreparation3 = _space[k];
			if (particlePreparation3.consumed)
			{
				continue;
			}
			ushort type = Main.tile[particlePreparation3.x, particlePreparation3.y].type;
			bool flag = TileID.Sets.RoomNeeds.CountsAsTable[type] || TileID.Sets.RoomNeeds.CountsAsChair[type] || TileID.Sets.RoomNeeds.CountsAsTorch[type] || TileID.Sets.RoomNeeds.CountsAsDoor[type];
			Asset<Texture2D> textureAsset = TextureAssets.Extra[293];
			Color colorTint = Color.Cyan * 0.7f;
			colorTint.A /= 2;
			Vector2 val = new Vector2((float)particlePreparation3.x, (float)particlePreparation3.y);
			float num2 = 1f;
			switch (particlePreparation3.type)
			{
			case Reason.BlockedWall:
				textureAsset = TextureAssets.Extra[292];
				colorTint = new Color(80, 255, 255) * 0.7f;
				colorTint.A /= 2;
				continue;
			case Reason.OpenAir:
			case Reason.Hazard:
				textureAsset = TextureAssets.Extra[292];
				colorTint = new Color(255, 40, 40, 255);
				break;
			case Reason.UnsafeWall:
				textureAsset = TextureAssets.Extra[298];
				colorTint = new Color(255, 40, 40, 255);
				break;
			default:
				num2 = 1.5f;
				if (flag)
				{
					continue;
				}
				break;
			}
			RoomCheckParticle roomCheckParticle = _particlePool.RequestParticle();
			roomCheckParticle.SetBasicInfo(textureAsset, null, Vector2.Zero, new Vector2((float)(particlePreparation3.x * 16 + 8), (float)(particlePreparation3.y * 16 + 8)));
			roomCheckParticle.Delay = (int)(3f * Vector2.Distance(origin, val));
			float num3 = num - (float)roomCheckParticle.Delay;
			roomCheckParticle.SetTypeInfo(num3 * num2);
			roomCheckParticle.FadeInNormalizedTime = Utils.Remap(num - 24f, roomCheckParticle.Delay, num, 0f, 1f);
			roomCheckParticle.FadeOutNormalizedTime = Utils.Remap(num - 6f, roomCheckParticle.Delay, num, 0f, 1f);
			roomCheckParticle.ColorTint = colorTint;
			roomCheckParticle.Scale = Vector2.One;
			Main.ParticleSystem_World_OverPlayers.Add(roomCheckParticle);
		}
		for (int l = 0; l < _spaceCount; l++)
		{
			ParticlePreparation particlePreparation4 = _space[l];
			if (particlePreparation4.consumed)
			{
				continue;
			}
			ushort type2 = Main.tile[particlePreparation4.x, particlePreparation4.y].type;
			if (!TileID.Sets.RoomNeeds.CountsAsTable[type2] && !TileID.Sets.RoomNeeds.CountsAsChair[type2] && !TileID.Sets.RoomNeeds.CountsAsTorch[type2] && !TileID.Sets.RoomNeeds.CountsAsDoor[type2])
			{
				continue;
			}
			TileObjectData.TryGetTileBounds(particlePreparation4.x, particlePreparation4.y, out var bounds);
			for (int m = 0; m < _spaceCount; m++)
			{
				if (m != l)
				{
					ParticlePreparation particlePreparation5 = _space[m];
					if (particlePreparation5.x >= bounds.Left && particlePreparation5.x < bounds.Right && particlePreparation5.y >= bounds.Top && particlePreparation5.y < bounds.Bottom)
					{
						_space[m].consumed = true;
					}
				}
			}
		}
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		for (int n = 0; n < _spaceCount; n++)
		{
			ParticlePreparation particlePreparation6 = _space[n];
			if (particlePreparation6.consumed)
			{
				continue;
			}
			ushort type3 = Main.tile[particlePreparation6.x, particlePreparation6.y].type;
			bool flag6 = TileID.Sets.RoomNeeds.CountsAsTable[type3];
			bool flag7 = TileID.Sets.RoomNeeds.CountsAsChair[type3];
			bool flag8 = TileID.Sets.RoomNeeds.CountsAsTorch[type3];
			bool flag9 = TileID.Sets.RoomNeeds.CountsAsDoor[type3];
			if (!(flag6 | flag7 | flag9 | flag8))
			{
				continue;
			}
			Asset<Texture2D> val2 = TextureAssets.Extra[293];
			if (flag6)
			{
				if (flag3)
				{
					continue;
				}
				flag3 = true;
				val2 = TextureAssets.Extra[297];
			}
			if (flag7)
			{
				if (flag2)
				{
					continue;
				}
				flag2 = true;
				val2 = TextureAssets.Extra[295];
			}
			if (flag9)
			{
				if (flag5)
				{
					continue;
				}
				flag5 = true;
				val2 = TextureAssets.Extra[296];
			}
			if (flag8)
			{
				if (flag4)
				{
					continue;
				}
				flag4 = true;
				val2 = TextureAssets.Extra[294];
			}
			TileObjectData.TryGetTileBounds(particlePreparation6.x, particlePreparation6.y, out var bounds2);
			Color val3 = Color.LimeGreen * 0.8f;
			val3.A /= 2;
			Vector2 val4 = new Vector2((float)particlePreparation6.x, (float)particlePreparation6.y);
			RoomCheckParticle roomCheckParticle2 = _particlePool.RequestParticle();
			roomCheckParticle2.SetBasicInfo(initialLocalPosition: new Vector2((float)(bounds2.Left + bounds2.Right) / 2f, MathHelper.Min((float)bounds2.Top / 2f + (float)bounds2.Bottom / 2f, (float)(bounds2.Top + 1))) * 16f + new Vector2(0f, (float)(-val2.Height() / 2)), textureAsset: val2, frame: null, initialVelocity: Vector2.Zero);
			roomCheckParticle2.Delay = (int)(3f * Vector2.Distance(origin, val4));
			float num4 = num - (float)roomCheckParticle2.Delay;
			roomCheckParticle2.SetTypeInfo(num4);
			roomCheckParticle2.FadeInNormalizedTime = Utils.Remap(num - 24f, roomCheckParticle2.Delay, num, 0f, 1f);
			roomCheckParticle2.FadeOutNormalizedTime = Utils.Remap(num - 6f, roomCheckParticle2.Delay, num, 0f, 1f);
			roomCheckParticle2.Scale = Vector2.One;
			int num5 = 32;
			roomCheckParticle2.LocalPosition.Y -= num5;
			roomCheckParticle2.Velocity = new Vector2(0f, (float)num5 * 2.5f) / num4;
			roomCheckParticle2.AccelerationPerFrame = -roomCheckParticle2.Velocity * 1.25f / num4;
			Main.ParticleSystem_World_OverPlayers.Add(roomCheckParticle2);
		}
	}

	private float GetHighestDistanceFromOrigin(ref Vector2 origin)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		for (int i = 0; i < _spaceCount; i++)
		{
			ParticlePreparation particlePreparation = _space[i];
			Vector2 val = new Vector2((float)particlePreparation.x, (float)particlePreparation.y);
			float num2 = Vector2.Distance(origin, val);
			if (num < num2)
			{
				num = num2;
			}
		}
		return num;
	}

	public void BeginScoring()
	{
		_bestScore = default;
		_scoreCount = 0;
	}

	public void ReportScore(int x, int y, int score)
	{
		if (_scoreCount >= _score.Length)
		{
			Array.Resize(ref _score, _score.Length * 2);
		}
		_score[_scoreCount++] = new ScorePreparation
		{
			x = x,
			y = y,
			score = score
		};
	}

	public void SetAsHighScore(int x, int y, int score)
	{
		_bestScore = new ScorePreparation
		{
			x = x,
			y = y,
			score = score
		};
	}

	public void EndScoring()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)_originX, (float)_originY);
		float num = 0f;
		for (int i = 0; i < _spaceCount; i++)
		{
			ParticlePreparation particlePreparation = _space[i];
			Vector2 val2 = new Vector2((float)particlePreparation.x, (float)particlePreparation.y);
			float num2 = Vector2.Distance(val, val2);
			if (num < num2)
			{
				num = num2;
			}
		}
		float num3 = 3f * num + 90f;
		int score = _bestScore.score;
		if (score == 0)
		{
			return;
		}
		for (int j = 0; j < _scoreCount; j++)
		{
			ScorePreparation scorePreparation = _score[j];
			if (scorePreparation.score != 0 && (scorePreparation.x != _bestScore.x || scorePreparation.y != _bestScore.y))
			{
				Asset<Texture2D> textureAsset = TextureAssets.Extra[293];
				RoomCheckParticle roomCheckParticle = _particlePool.RequestParticle();
				roomCheckParticle.SetBasicInfo(textureAsset, null, Vector2.Zero, new Vector2((float)(scorePreparation.x * 16 + 8), (float)(scorePreparation.y * 16 + 8 - 16)));
				Vector2 val3 = new Vector2((float)scorePreparation.x, (float)scorePreparation.y);
				roomCheckParticle.Delay = (int)(3f * Vector2.Distance(val, val3));
				float timeToLive = num3 - (float)roomCheckParticle.Delay;
				roomCheckParticle.SetTypeInfo(timeToLive);
				roomCheckParticle.FadeInNormalizedTime = Utils.Remap(num3 - 24f, roomCheckParticle.Delay, num3, 0f, 1f);
				roomCheckParticle.FadeOutNormalizedTime = Utils.Remap(num3 - 6f, roomCheckParticle.Delay, num3, 0f, 1f);
				if (scorePreparation.score > 0)
				{
					roomCheckParticle.ColorTint = Color.LimeGreen * (0.5f + 0.5f * (float)scorePreparation.score / (float)score);
				}
				else
				{
					roomCheckParticle.ColorTint = Color.Red * ((float)scorePreparation.score / (float)(-score));
				}
				roomCheckParticle.Scale = Vector2.One * 2f;
				Main.ParticleSystem_World_OverPlayers.Add(roomCheckParticle);
			}
		}
		for (int k = 0; k < 1; k++)
		{
			ScorePreparation bestScore = _bestScore;
			if (bestScore.score != 0)
			{
				Asset<Texture2D> textureAsset2 = TextureAssets.Extra[293];
				RoomCheckParticle roomCheckParticle2 = _particlePool.RequestParticle();
				roomCheckParticle2.SetBasicInfo(textureAsset2, null, Vector2.Zero, new Vector2((float)(bestScore.x * 16 + 8), (float)(bestScore.y * 16 + 8 - 16)));
				Vector2 val4 = new Vector2((float)bestScore.x, (float)bestScore.y);
				roomCheckParticle2.Delay = (int)(3f * Vector2.Distance(val, val4));
				float timeToLive2 = num3 - (float)roomCheckParticle2.Delay;
				roomCheckParticle2.SetTypeInfo(timeToLive2);
				roomCheckParticle2.FadeInNormalizedTime = Utils.Remap(num3 - 24f, roomCheckParticle2.Delay, num3, 0f, 1f);
				roomCheckParticle2.FadeOutNormalizedTime = Utils.Remap(num3 - 6f, roomCheckParticle2.Delay, num3, 0f, 1f);
				roomCheckParticle2.ColorTint = Main.OurFavoriteColor;
				roomCheckParticle2.Scale = Vector2.One * 3f;
				Main.ParticleSystem_World_OverPlayers.Add(roomCheckParticle2);
			}
		}
	}
}
