using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria.GameContent;

public class AmbientWindSystem
{
	private UnifiedRandom _random = new UnifiedRandom();

	private List<Point> _spotsForAirboneWind = new List<Point>();

	private int _updatesCounter;

	public void Update()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.LocalPlayer.ZoneGraveyard)
		{
			return;
		}
		_updatesCounter++;
		Rectangle tileWorkSpace = GetTileWorkSpace();
		int num = tileWorkSpace.X + tileWorkSpace.Width;
		int num2 = tileWorkSpace.Y + tileWorkSpace.Height;
		for (int i = tileWorkSpace.X; i < num; i++)
		{
			for (int j = tileWorkSpace.Y; j < num2; j++)
			{
				TrySpawningWind(i, j);
			}
		}
		if (_updatesCounter % 30 == 0)
		{
			SpawnAirborneWind();
		}
	}

	private void SpawnAirborneWind()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		foreach (Point item in _spotsForAirboneWind)
		{
			SpawnAirborneCloud(item.X, item.Y);
		}
		_spotsForAirboneWind.Clear();
	}

	private Rectangle GetTileWorkSpace()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Point val = Main.LocalPlayer.Center.ToTileCoordinates();
		int num = 120;
		int num2 = 30;
		return new Rectangle(val.X - num / 2, val.Y - num2 / 2, num, num2);
	}

	private void TrySpawningWind(int x, int y)
	{
		if (!WorldGen.InWorld(x, y, 10) || Main.tile[x, y] == null)
		{
			return;
		}
		TestAirCloud(x, y);
		Tile tile = Main.tile[x, y];
		if (!tile.active() || tile.slope() > 0 || tile.halfBrick() || !Main.tileSolid[tile.type])
		{
			return;
		}
		tile = Main.tile[x, y - 1];
		if (!WorldGen.SolidTile(tile) && _random.Next(120) == 0)
		{
			SpawnFloorCloud(x, y);
			if (_random.Next(3) == 0)
			{
				SpawnFloorCloud(x, y - 1);
			}
		}
	}

	private void SpawnAirborneCloud(int x, int y)
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		int num = _random.Next(2, 6);
		float num2 = 1.1f;
		float num3 = 2.2f;
		float num4 = 0.023561945f * _random.NextFloatDirection();
		float num5 = 0.023561945f * _random.NextFloatDirection();
		while (num5 > -0.011780973f && num5 < 0.011780973f)
		{
			num5 = 0.023561945f * _random.NextFloatDirection();
		}
		if (_random.Next(4) == 0)
		{
			num = _random.Next(9, 16);
			num2 = 1.1f;
			num3 = 1.2f;
		}
		else if (_random.Next(4) == 0)
		{
			num = _random.Next(9, 16);
			num2 = 1.1f;
			num3 = 0.2f;
		}
		Vector2 val = new Vector2(-10f, 0f);
		Vector2 val2 = new Point(x, y).ToWorldCoordinates();
		num4 -= num5 * (float)num * 0.5f;
		float num6 = num4;
		for (int i = 0; i < num; i++)
		{
			if (Main.rand.Next(10) == 0)
			{
				num5 *= _random.NextFloatDirection();
			}
			Vector2 val3 = _random.NextVector2Circular(4f, 4f);
			int type = 1091 + _random.Next(2) * 2;
			float num7 = 1.4f;
			float num8 = num2 + _random.NextFloat() * num3;
			float num9 = num6 + num5;
			Vector2 val4 = Vector2.UnitX.RotatedBy(num9) * num7;
			Gore.NewGorePerfect(val2 + val3 - val, val4 * Main.WindForVisuals, type, num8);
			val2 += val4 * 6.5f * num8;
			num6 = num9;
		}
	}

	private void SpawnFloorCloud(int x, int y)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = new Point(x, y - 1).ToWorldCoordinates();
		int type = _random.Next(1087, 1090);
		float num = 16f * _random.NextFloat();
		position.Y -= num;
		if (num < 4f)
		{
			type = 1090;
		}
		float num2 = 0.4f;
		float scale = 0.8f + _random.NextFloat() * 0.2f;
		Gore.NewGorePerfect(position, Vector2.UnitX * num2 * Main.WindForVisuals, type, scale);
	}

	private void TestAirCloud(int x, int y)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		if (_random.Next(120000) != 0)
		{
			return;
		}
		for (int i = -2; i <= 2; i++)
		{
			if (i != 0)
			{
				Tile t = Main.tile[x + i, y];
				if (!DoesTileAllowWind(t))
				{
					return;
				}
				t = Main.tile[x, y + i];
				if (!DoesTileAllowWind(t))
				{
					return;
				}
			}
		}
		_spotsForAirboneWind.Add(new Point(x, y));
	}

	private bool DoesTileAllowWind(Tile t)
	{
		if (t.active())
		{
			return !Main.tileSolid[t.type];
		}
		return true;
	}
}
