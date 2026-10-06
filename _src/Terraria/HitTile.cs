using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Utilities;

namespace Terraria;

public class HitTile
{
	public struct HitTileObject
	{
		public int X;

		public int Y;

		public int damage;

		public int type;

		public int timeToLive;

		public int crackStyle;

		public int animationTimeElapsed;

		public Vector2 animationDirection;

		public void Clear()
		{
			X = 0;
			Y = 0;
			damage = 0;
			type = 0;
			timeToLive = 0;
			if (rand == null)
			{
				rand = new UnifiedRandom((int)DateTime.Now.Ticks);
			}
			for (crackStyle = rand.Next(4); crackStyle == lastCrack; crackStyle = rand.Next(4))
			{
			}
			lastCrack = crackStyle;
		}

		public void Prepare(int x, int y, int hitType)
		{
			X = x;
			Y = y;
			type = hitType;
		}

		public void SetPosition(int x, int y)
		{
			X = x;
			Y = y;
		}

		public void AddDamage(int damageAmount)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			damage += damageAmount;
			timeToLive = 60;
			animationTimeElapsed = 0;
			animationDirection = (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2() * 2f;
		}
	}

	internal const int UNUSED = 0;

	internal const int TILE = 1;

	internal const int WALL = 2;

	internal const int MAX_HITTILES = 500;

	internal const int TIMETOLIVE = 60;

	private static UnifiedRandom rand;

	private static int lastCrack = -1;

	public HitTileObject[] data;

	private int[] order;

	private int bufferLocation;

	public static void ClearAllTilesAtThisLocation(int x, int y)
	{
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].active)
			{
				Main.player[i].hitTile.ClearThisTile(x, y);
			}
		}
	}

	public void ClearThisTile(int x, int y)
	{
		for (int i = 0; i <= 500; i++)
		{
			int num = order[i];
			HitTileObject hitTileObject = data[num];
			if (hitTileObject.X == x && hitTileObject.Y == y)
			{
				Clear(i);
				Prune();
			}
		}
	}

	public HitTile()
	{
		rand = new UnifiedRandom();
		data = new HitTileObject[501];
		order = new int[501];
		for (int i = 0; i <= 500; i++)
		{
			data[i].Clear();
			order[i] = i;
		}
		bufferLocation = 0;
	}

	public int TryFinding(int x, int y, int hitType)
	{
		for (int i = 0; i <= 500; i++)
		{
			int num = order[i];
			HitTileObject hitTileObject = data[num];
			if (hitTileObject.type == hitType)
			{
				if (hitTileObject.X == x && hitTileObject.Y == y)
				{
					return num;
				}
			}
			else if (i != 0 && hitTileObject.type == 0)
			{
				break;
			}
		}
		return -1;
	}

	public void TryClearingAndPruning(int x, int y, int hitType)
	{
		int num = TryFinding(x, y, hitType);
		if (num != -1)
		{
			Clear(num);
			Prune();
		}
	}

	public int HitObject(int x, int y, int hitType)
	{
		for (int i = 0; i <= 500; i++)
		{
			int num = order[i];
			HitTileObject hitTileObject = data[num];
			if (hitTileObject.type == hitType)
			{
				if (hitTileObject.X == x && hitTileObject.Y == y)
				{
					return num;
				}
			}
			else if (i != 0 && hitTileObject.type == 0)
			{
				break;
			}
		}
		data[bufferLocation].Prepare(x, y, hitType);
		return bufferLocation;
	}

	public void UpdatePosition(int tileId, int x, int y)
	{
		if (tileId >= 0 && tileId <= 500)
		{
			data[tileId].SetPosition(x, y);
		}
	}

	public int AddDamage(int tileId, int damageAmount, bool updateAmount = true)
	{
		if (tileId < 0 || tileId > 500)
		{
			return 0;
		}
		if (tileId == bufferLocation && damageAmount == 0)
		{
			return 0;
		}
		if (!updateAmount)
		{
			return data[tileId].damage + damageAmount;
		}
		data[tileId].AddDamage(damageAmount);
		SortSlots(tileId);
		return data[tileId].damage;
	}

	private void SortSlots(int tileId)
	{
		if (tileId == bufferLocation)
		{
			bufferLocation = order[500];
			if (tileId != bufferLocation)
			{
				data[bufferLocation].Clear();
			}
			for (int num = 500; num > 0; num--)
			{
				order[num] = order[num - 1];
			}
			order[0] = bufferLocation;
		}
		else
		{
			int num;
			for (num = 0; num <= 500 && order[num] != tileId; num++)
			{
			}
			while (num > 1)
			{
				int num2 = order[num - 1];
				order[num - 1] = order[num];
				order[num] = num2;
				num--;
			}
			order[1] = tileId;
		}
	}

	public void Clear(int tileId)
	{
		if (tileId >= 0 && tileId <= 500)
		{
			data[tileId].Clear();
			int i;
			for (i = 0; i < 500 && order[i] != tileId; i++)
			{
			}
			for (; i < 500; i++)
			{
				order[i] = order[i + 1];
			}
			order[500] = tileId;
		}
	}

	public void Prune()
	{
		bool flag = false;
		for (int i = 0; i <= 500; i++)
		{
			flag |= Prune(ref data[i]);
		}
		if (!flag)
		{
			return;
		}
		int num = 1;
		while (flag)
		{
			flag = false;
			for (int j = num; j < 500; j++)
			{
				if (data[order[j]].type == 0 && data[order[j + 1]].type != 0)
				{
					int num2 = order[j];
					order[j] = order[j + 1];
					order[j + 1] = num2;
					flag = true;
				}
			}
		}
	}

	private static bool Prune(ref HitTileObject dataObject)
	{
		if (dataObject.type == 0)
		{
			return false;
		}
		Tile tile = Main.tile[dataObject.X, dataObject.Y];
		if (dataObject.timeToLive <= 1)
		{
			dataObject.Clear();
			return true;
		}
		dataObject.timeToLive--;
		if ((double)dataObject.timeToLive < 12.0)
		{
			dataObject.damage -= 10;
		}
		else if ((double)dataObject.timeToLive < 24.0)
		{
			dataObject.damage -= 7;
		}
		else if ((double)dataObject.timeToLive < 36.0)
		{
			dataObject.damage -= 5;
		}
		else if ((double)dataObject.timeToLive < 48.0)
		{
			dataObject.damage -= 2;
		}
		if (dataObject.damage < 0)
		{
			dataObject.Clear();
			return true;
		}
		if (dataObject.type == 1)
		{
			if (!tile.active())
			{
				dataObject.Clear();
				return true;
			}
		}
		else if (tile.wall == 0)
		{
			dataObject.Clear();
			return true;
		}
		return false;
	}

	public void DrawFreshAnimations(SpriteBatch spriteBatch)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < data.Length; i++)
		{
			data[i].animationTimeElapsed++;
		}
		if (!Main.SettingsEnabled_MinersWobble)
		{
			return;
		}
		int num = 1;
		Vector2 val = new Vector2((float)Main.offScreenRange);
		if (Main.drawToScreen)
		{
			val = Vector2.Zero;
		}
		val = Vector2.Zero;
		bool flag = Main.ShouldShowInvisibleBlocksAndWalls();
		for (int j = 0; j < data.Length; j++)
		{
			if (data[j].type != num)
			{
				continue;
			}
			int damage = data[j].damage;
			if (damage < 20)
			{
				continue;
			}
			int x = data[j].X;
			int y = data[j].Y;
			if (!WorldGen.InWorld(x, y))
			{
				continue;
			}
			Tile tile = Main.tile[x, y];
			bool flag2 = tile != null;
			if (flag2 && num == 1)
			{
				flag2 = flag2 && tile.active() && Main.tileSolid[Main.tile[x, y].type] && (!tile.invisibleBlock() | flag);
			}
			if (flag2 && num == 2)
			{
				flag2 = flag2 && tile.wall != 0 && (!tile.invisibleWall() | flag);
			}
			if (!flag2)
			{
				continue;
			}
			bool flag3 = false;
			bool flag4 = false;
			if (tile.type == 10)
			{
				flag3 = false;
			}
			else if (Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
			{
				flag3 = true;
			}
			else if (WorldGen.IsTreeType(tile.type))
			{
				flag4 = true;
				int num2 = tile.frameX / 22;
				int num3 = tile.frameY / 22;
				if (num3 < 9)
				{
					flag3 = ((num2 != 1 && num2 != 2) || num3 < 6 || num3 > 8) && (num2 != 3 || num3 > 2) && (num2 != 4 || num3 < 3 || num3 > 5) && ((num2 != 5 || num3 < 6 || num3 > 8) ? true : false);
				}
			}
			else if (tile.type == 72)
			{
				flag4 = true;
				if (tile.frameX <= 34)
				{
					flag3 = true;
				}
			}
			if (!flag3 || tile.slope() != 0 || tile.halfBrick())
			{
				continue;
			}
			int num4 = 0;
			if (damage >= 80)
			{
				num4 = 3;
			}
			else if (damage >= 60)
			{
				num4 = 2;
			}
			else if (damage >= 40)
			{
				num4 = 1;
			}
			else if (damage >= 20)
			{
				num4 = 0;
			}
			Rectangle value = new Rectangle(data[j].crackStyle * 18, num4 * 18, 16, 16);
			value.Inflate(-2, -2);
			if (flag4)
			{
				value.X = (4 + data[j].crackStyle / 2) * 18;
			}
			int animationTimeElapsed = data[j].animationTimeElapsed;
			if (!((float)animationTimeElapsed >= 10f))
			{
				float num5 = (float)animationTimeElapsed / 10f;
				Color val2 = Lighting.GetColor(x, y);
				float num6 = 0f;
				Vector2 zero = Vector2.Zero;
				float num7 = 0.5f;
				float num8 = num5 % num7;
				num8 *= 1f / num7;
				if ((int)(num5 / num7) % 2 == 1)
				{
					num8 = 1f - num8;
				}
				Tile tileSafely = Framing.GetTileSafely(x, y);
				Tile tile2 = tileSafely;
				Texture2D val3 = Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady(tileSafely.type, 0, tileSafely.color());
				if (val3 != null)
				{
					Vector2 val4 = new Vector2(8f);
					Vector2 val5 = new Vector2(1f);
					float num9 = num8 * 0.2f + 1f;
					float num10 = 1f - num8;
					num10 = 1f;
					val2 *= num10 * num10 * 0.8f;
					Vector2 val6 = num9 * val5;
					Vector2 val7 = (new Vector2((float)(x * 16 - (int)Main.screenPosition.X), (float)(y * 16 - (int)Main.screenPosition.Y)) + val + val4 + zero).Floor();
					spriteBatch.Draw(val3, val7, (Rectangle?)new Rectangle((int)tile2.frameX, (int)tile2.frameY, 16, 16), val2, num6, val4, val6, (SpriteEffects)0, 0f);
					val2.A = 180;
					spriteBatch.Draw(TextureAssets.TileCrack.Value, val7, (Rectangle?)value, val2, num6, val4, val6, (SpriteEffects)0, 0f);
				}
			}
		}
	}
}
