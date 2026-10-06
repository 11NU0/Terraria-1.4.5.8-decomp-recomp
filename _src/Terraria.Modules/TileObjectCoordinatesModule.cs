using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Modules;

public class TileObjectCoordinatesModule
{
	public int width;

	public int[] heights;

	public int padding;

	public Point16 paddingFix;

	public int styleWidth;

	public int styleHeight;

	public bool calculated;

	public int drawStyleOffset;

	public Rectangle[,] drawFrameOffsets;

	public TileObjectCoordinatesModule(TileObjectCoordinatesModule copyFrom = null, int[] drawHeight = null, Rectangle[,] drawFrameOffs = null)
	{
		if (copyFrom == null)
		{
			width = 0;
			padding = 0;
			paddingFix = Point16.Zero;
			styleWidth = 0;
			drawStyleOffset = 0;
			styleHeight = 0;
			calculated = false;
			heights = drawHeight;
			drawFrameOffsets = drawFrameOffs;
			return;
		}
		width = copyFrom.width;
		padding = copyFrom.padding;
		paddingFix = copyFrom.paddingFix;
		drawStyleOffset = copyFrom.drawStyleOffset;
		styleWidth = copyFrom.styleWidth;
		styleHeight = copyFrom.styleHeight;
		calculated = copyFrom.calculated;
		if (drawHeight == null)
		{
			if (copyFrom.heights == null)
			{
				heights = null;
			}
			else
			{
				heights = new int[copyFrom.heights.Length];
				Array.Copy(copyFrom.heights, heights, heights.Length);
			}
		}
		else
		{
			heights = drawHeight;
		}
		if (drawFrameOffs == null)
		{
			if (copyFrom.drawFrameOffsets == null)
			{
				drawFrameOffsets = null;
				return;
			}
			drawFrameOffsets = new Rectangle[copyFrom.drawFrameOffsets.GetLength(0), copyFrom.drawFrameOffsets.GetLength(1)];
			Array.Copy(copyFrom.drawFrameOffsets, drawFrameOffsets, drawFrameOffsets.Length);
		}
		else
		{
			drawFrameOffsets = drawFrameOffs;
		}
	}
}
