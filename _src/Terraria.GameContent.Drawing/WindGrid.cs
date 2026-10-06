using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Drawing;

public class WindGrid
{
	private struct WindCoord
	{
		public int Time;

		public int DirectionX;

		public int DirectionY;
	}

	private WindCoord[,] _grid = new WindCoord[1, 1];

	private int _width = 1;

	private int _height = 1;

	private int _gameTime;

	public void SetSize(int targetWidth, int targetHeight)
	{
		_width = Math.Max(_width, targetWidth);
		_height = Math.Max(_height, targetHeight);
		ResizeGrid();
	}

	public void Update()
	{
		_gameTime++;
		if (Main.SettingsEnabled_TilesSwayInWind)
		{
			ScanPlayers();
		}
	}

	public void GetWindTime(int tileX, int tileY, int timeThreshold, out int windTimeLeft, out int directionX, out int directionY)
	{
		WindCoord windCoord = _grid[tileX % _width, tileY % _height];
		directionX = windCoord.DirectionX;
		directionY = windCoord.DirectionY;
		if (windCoord.Time + timeThreshold < _gameTime)
		{
			windTimeLeft = 0;
		}
		else
		{
			windTimeLeft = _gameTime - windCoord.Time;
		}
	}

	private void ResizeGrid()
	{
		if (_width > _grid.GetLength(0) || _height > _grid.GetLength(1))
		{
			_grid = new WindCoord[_width, _height];
		}
	}

	private void SetWindTime(int tileX, int tileY, int directionX, int directionY)
	{
		int num = tileX % _width;
		int num2 = tileY % _height;
		_grid[num, num2].Time = _gameTime;
		_grid[num, num2].DirectionX = directionX;
		_grid[num, num2].DirectionY = directionY;
	}

	private void ScanPlayers()
	{
		if (Main.netMode == 0)
		{
			ScanPlayer(Main.myPlayer);
		}
		else if (Main.netMode == 1)
		{
			for (int i = 0; i < 255; i++)
			{
				ScanPlayer(i);
			}
		}
	}

	private void ScanPlayer(int i)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[i];
		if (!player.active || player.dead || (player.velocity.X == 0f && player.velocity.Y == 0f))
		{
			return;
		}
		Rectangle val = Utils.CenteredRectangle(Main.Camera.Center, Main.Camera.UnscaledSize);
		if (!val.Intersects(player.Hitbox) || player.velocity.HasNaNs())
		{
			return;
		}
		int directionX = Math.Sign(player.velocity.X);
		int directionY = Math.Sign(player.velocity.Y);
		foreach (Point item in Collision.GetTilesIn(player.TopLeft, player.BottomRight))
		{
			SetWindTime(item.X, item.Y, directionX, directionY);
		}
	}
}
