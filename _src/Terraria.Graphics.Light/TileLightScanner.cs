using System;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.Graphics.Light;

public class TileLightScanner
{
	private FastRandom _random = FastRandom.CreateWithRandomSeed();

	private bool _drawInvisibleWalls;

	public void ExportTo(Rectangle area, LightMap outputMap, TileLightScannerOptions options)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected Obj, but got Unknown
		_drawInvisibleWalls = options.DrawInvisibleWalls;
		FastParallel.For(area.Left, area.Right, (ParallelForAction)((int start, int end, object context) =>
		{
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			for (int i = start; i < end; i++)
			{
				for (int j = area.Top; j < area.Bottom; j++)
				{
					if (IsTileNullOrTouchingNull(i, j))
					{
						outputMap.SetMaskAt(i - area.X, j - area.Y, LightMaskMode.None);
						outputMap[i - area.X, j - area.Y] = Vector3.Zero;
					}
					else
					{
						LightMaskMode tileMask = GetTileMask(Main.tile[i, j]);
						outputMap.SetMaskAt(i - area.X, j - area.Y, tileMask);
						GetTileLight(i, j, out var outputColor);
						outputMap[i - area.X, j - area.Y] = outputColor;
					}
				}
			}
		}), (object)null);
	}

	private bool IsTileNullOrTouchingNull(int x, int y)
	{
		if (WorldGen.InWorld(x, y, 1))
		{
			if (Main.tile[x, y] != null && Main.tile[x + 1, y] != null && Main.tile[x - 1, y] != null && Main.tile[x, y - 1] != null)
			{
				return Main.tile[x, y + 1] == null;
			}
			return true;
		}
		return true;
	}

	public void Update()
	{
		_random.NextSeed();
	}

	public LightMaskMode GetMaskMode(int x, int y)
	{
		return GetTileMask(Main.tile[x, y]);
	}

	private LightMaskMode GetTileMask(Tile tile)
	{
		if (LightIsBlocked(tile) && tile.type != 131 && !tile.inActive() && tile.slope() == 0)
		{
			if (TileID.Sets.CrackedBricks[tile.type])
			{
				return LightMaskMode.CrackedBricks;
			}
			return LightMaskMode.Solid;
		}
		if (!tile.lava() && tile.liquid > 128)
		{
			if (!tile.honey())
			{
				return LightMaskMode.Water;
			}
			return LightMaskMode.Honey;
		}
		return LightMaskMode.None;
	}

	public void GetTileLight(int x, int y, out Vector3 outputColor)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		outputColor = Vector3.Zero;
		Tile tile = Main.tile[x, y];
		FastRandom localRandom = _random.WithModifier(x, y);
		if (y <= (int)Main.worldSurface)
		{
			ApplySurfaceLight(tile, x, y, ref outputColor);
		}
		else if (y > Main.UnderworldLayer)
		{
			ApplyHellLight(tile, x, y, ref outputColor);
		}
		ApplyWallLight(tile, x, y, ref localRandom, ref outputColor);
		if (tile.active())
		{
			ApplyTileLight(tile, x, y, ref localRandom, ref outputColor);
		}
		ApplyLiquidLight(tile, ref outputColor);
	}

	private void ApplyLiquidLight(Tile tile, ref Vector3 lightColor)
	{
		if (tile.liquid <= 0)
		{
			return;
		}
		if (tile.lava())
		{
			float num = 0.55f;
			num += (float)(270 - Main.mouseTextColor) / 900f;
			if (lightColor.X < num)
			{
				lightColor.X = num;
			}
			if (lightColor.Y < num)
			{
				lightColor.Y = num * 0.6f;
			}
			if (lightColor.Z < num)
			{
				lightColor.Z = num * 0.2f;
			}
		}
		else if (tile.shimmer())
		{
			float num2 = 0.7f;
			float num3 = 0.7f;
			num2 += (float)(270 - Main.mouseTextColor) / 900f;
			num3 += (float)(270 - Main.mouseTextColor) / 125f;
			if (lightColor.X < num2)
			{
				lightColor.X = num2 * 0.6f;
			}
			if (lightColor.Y < num3)
			{
				lightColor.Y = num3 * 0.25f;
			}
			if (lightColor.Z < num2)
			{
				lightColor.Z = num2 * 0.9f;
			}
		}
	}

	private bool LightIsBlocked(Tile tile)
	{
		if (tile.active() && Main.tileBlockLight[tile.type])
		{
			if (tile.invisibleBlock())
			{
				return _drawInvisibleWalls;
			}
			return true;
		}
		return false;
	}

	private void ApplyWallLight(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
	{
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		bool flag = false;
		switch (tile.wall)
		{
		case 357:
			if (!LightIsBlocked(tile))
			{
				num = 0.15f;
				num2 = 0.27f;
				num3 = 0.3f;
				flag = true;
			}
			break;
		case 182:
			if (!LightIsBlocked(tile))
			{
				num = 0.24f;
				num2 = 0.12f;
				num3 = 0.089999996f;
			}
			break;
		case 33:
			if (!LightIsBlocked(tile))
			{
				num = 0.089999996f;
				num2 = 0.052500002f;
				num3 = 0.24f;
			}
			break;
		case 174:
			if (!LightIsBlocked(tile))
			{
				num = 0.2975f;
			}
			break;
		case 175:
			if (!LightIsBlocked(tile))
			{
				if (tile.wallColor() == 0)
				{
					num = 0.075f;
					num2 = 0.15f;
					num3 = 0.4f;
				}
				else
				{
					flag = true;
				}
			}
			break;
		case 176:
			if (!LightIsBlocked(tile))
			{
				num = 0.1f;
				num2 = 0.1f;
				num3 = 0.1f;
			}
			break;
		case 137:
			if (!LightIsBlocked(tile))
			{
				float num4 = 0.4f;
				num4 += (float)(270 - Main.mouseTextColor) / 1500f;
				num4 += (float)localRandom.Next(0, 50) * 0.0005f;
				num = 1f * num4;
				num2 = 0.5f * num4;
				num3 = 0.1f * num4;
			}
			break;
		case 44:
			if (!LightIsBlocked(tile))
			{
				num = (float)Main.DiscoR / 255f * 0.15f;
				num2 = (float)Main.DiscoG / 255f * 0.15f;
				num3 = (float)Main.DiscoB / 255f * 0.15f;
			}
			break;
		case 154:
			num = 0.6f;
			num3 = 0.6f;
			break;
		case 166:
			num = 0.6f;
			num2 = 0.6f;
			break;
		case 165:
			num3 = 0.6f;
			break;
		case 156:
			num2 = 0.6f;
			break;
		case 164:
			num = 0.6f;
			break;
		case 155:
			num = 0.6f;
			num2 = 0.6f;
			num3 = 0.6f;
			break;
		case 153:
			num = 0.6f;
			num2 = 0.3f;
			break;
		case 341:
			if (!LightIsBlocked(tile))
			{
				num = 0.25f;
				num2 = 0.1f;
				num3 = 0f;
			}
			break;
		case 343:
			if (!LightIsBlocked(tile))
			{
				num = 0f;
				num2 = 0.25f;
				num3 = 0f;
			}
			break;
		case 344:
			if (!LightIsBlocked(tile))
			{
				num = 0f;
				num2 = 0.16f;
				num3 = 0.34f;
			}
			break;
		case 342:
			if (!LightIsBlocked(tile))
			{
				num = 0.3f;
				num2 = 0f;
				num3 = 0.17f;
			}
			break;
		case 345:
			if (!LightIsBlocked(tile))
			{
				num = 0.3f;
				num2 = 0f;
				num3 = 0.35f;
			}
			break;
		case 346:
			if (!LightIsBlocked(tile))
			{
				num = (float)Main.DiscoR / 255f * 0.25f;
				num2 = (float)Main.DiscoG / 255f * 0.25f;
				num3 = (float)Main.DiscoB / 255f * 0.25f;
			}
			break;
		}
		if (flag && tile.wallColor() != 0)
		{
			Color val = WorldGen.paintColor(tile.wallColor());
			num = (float)(int)val.R / 765f;
			num2 = (float)(int)val.G / 765f;
			num3 = (float)(int)val.B / 765f;
		}
		if (lightColor.X < num)
		{
			lightColor.X = num;
		}
		if (lightColor.Y < num2)
		{
			lightColor.Y = num2;
		}
		if (lightColor.Z < num3)
		{
			lightColor.Z = num3;
		}
	}

	private void ApplyTileLight(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
	{
		//IL_45b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_45be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cff: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_3da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3daf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_2499: Unknown result type (might be due to invalid IL or missing references)
		//IL_249e: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25da: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_32aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_32b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_32bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_33d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_33fc: Unknown result type (might be due to invalid IL or missing references)
		float R = 0f;
		float G = 0f;
		float B = 0f;
		bool flag = false;
		if (Main.tileLighted[tile.type])
		{
			Color val3;
			switch (tile.type)
			{
			case 658:
				if (!tile.invisibleBlock())
				{
					TorchID.TorchColor(23, out R, out G, out B);
					switch (tile.frameY / 54)
					{
					default:
						R *= 0.2f;
						G *= 0.2f;
						B *= 0.2f;
						break;
					case 1:
						R *= 0.3f;
						G *= 0.3f;
						B *= 0.3f;
						break;
					case 2:
						R *= 0.1f;
						G *= 0.1f;
						B *= 0.1f;
						break;
					}
				}
				break;
			case 356:
				if (Main.sundialCooldown == 0)
				{
					R = 0.45f;
					G = 0.25f;
					B = 0f;
				}
				break;
			case 663:
				if (Main.moondialCooldown == 0)
				{
					R = 0f;
					G = 0.25f;
					B = 0.45f;
				}
				break;
			case 656:
				R = 0.2f;
				G = 0.55f;
				B = 0.5f;
				break;
			case 20:
			{
				int num27 = tile.frameX / 18;
				if (num27 >= 30 && num27 <= 32)
				{
					R = 0.325f;
					G = 0.15f;
					B = 0.05f;
				}
				break;
			}
			case 634:
				R = 0.65f;
				G = 0.3f;
				B = 0.1f;
				break;
			case 633:
			case 637:
			case 638:
				R = 0.325f;
				G = 0.15f;
				B = 0.05f;
				break;
			case 463:
				R = 0.2f;
				G = 0.4f;
				B = 0.8f;
				break;
			case 491:
				R = 0.5f;
				G = 0.4f;
				B = 0.7f;
				break;
			case 209:
				if (tile.frameX == 234 || tile.frameX == 252)
				{
					val3 = PortalHelper.GetPortalColor(Main.myPlayer, 0);
					Vector3 val13 = val3.ToVector3() * 0.65f;
					R = val13.X;
					G = val13.Y;
					B = val13.Z;
				}
				else if (tile.frameX == 306 || tile.frameX == 324)
				{
					val3 = PortalHelper.GetPortalColor(Main.myPlayer, 1);
					Vector3 val14 = val3.ToVector3() * 0.65f;
					R = val14.X;
					G = val14.Y;
					B = val14.Z;
				}
				break;
			case 415:
				R = 0.7f;
				G = 0.5f;
				B = 0.1f;
				break;
			case 500:
				R = 0.525f;
				G = 0.375f;
				B = 0.075f;
				break;
			case 416:
				R = 0f;
				G = 0.6f;
				B = 0.7f;
				break;
			case 501:
				R = 0f;
				G = 0.45f;
				B = 0.525f;
				break;
			case 417:
				R = 0.6f;
				G = 0.2f;
				B = 0.6f;
				break;
			case 502:
				R = 0.45f;
				G = 0.15f;
				B = 0.45f;
				break;
			case 418:
				R = 0.6f;
				G = 0.6f;
				B = 0.9f;
				break;
			case 503:
				R = 0.45f;
				G = 0.45f;
				B = 0.675f;
				break;
			case 390:
				R = 0.4f;
				G = 0.2f;
				B = 0.1f;
				break;
			case 597:
				switch (tile.frameX / 54)
				{
				case 0:
					R = 0.05f;
					G = 0.8f;
					B = 0.3f;
					break;
				case 1:
					R = 0.7f;
					G = 0.8f;
					B = 0.05f;
					break;
				case 2:
					R = 0.7f;
					G = 0.5f;
					B = 0.9f;
					break;
				case 3:
					R = 0.6f;
					G = 0.6f;
					B = 0.8f;
					break;
				case 4:
					R = 0.4f;
					G = 0.4f;
					B = 1.15f;
					break;
				case 5:
					R = 0.85f;
					G = 0.45f;
					B = 0.1f;
					break;
				case 6:
					R = 0.8f;
					G = 0.8f;
					B = 1f;
					break;
				case 7:
					R = 0.5f;
					G = 0.8f;
					B = 1.2f;
					break;
				}
				R *= 0.75f;
				G *= 0.75f;
				B *= 0.75f;
				break;
			case 564:
				if (tile.frameX < 36)
				{
					R = 0.05f;
					G = 0.3f;
					B = 0.55f;
				}
				break;
			case 568:
				R = 1f;
				G = 0.61f;
				B = 0.65f;
				break;
			case 569:
				R = 0.12f;
				G = 1f;
				B = 0.66f;
				break;
			case 570:
				R = 0.57f;
				G = 0.57f;
				B = 1f;
				break;
			case 580:
				R = 0.7f;
				G = 0.3f;
				B = 0.2f;
				break;
			case 391:
				R = 0.3f;
				G = 0.1f;
				B = 0.25f;
				break;
			case 381:
			case 517:
			case 687:
				R = 0.25f;
				G = 0.1f;
				B = 0f;
				break;
			case 534:
			case 535:
			case 689:
				R = 0f;
				G = 0.25f;
				B = 0f;
				break;
			case 536:
			case 537:
			case 690:
				R = 0f;
				G = 0.16f;
				B = 0.34f;
				break;
			case 539:
			case 540:
			case 688:
				R = 0.3f;
				G = 0f;
				B = 0.17f;
				break;
			case 625:
			case 626:
			case 691:
				R = 0.3f;
				G = 0f;
				B = 0.35f;
				break;
			case 627:
			case 628:
			case 692:
				R = (float)Main.DiscoR / 255f * 0.25f;
				G = (float)Main.DiscoG / 255f * 0.25f;
				B = (float)Main.DiscoB / 255f * 0.25f;
				break;
			case 184:
				if (tile.frameX == 110)
				{
					R = 0.25f;
					G = 0.1f;
					B = 0f;
				}
				if (tile.frameX == 132)
				{
					R = 0f;
					G = 0.25f;
					B = 0f;
				}
				if (tile.frameX == 154)
				{
					R = 0f;
					G = 0.16f;
					B = 0.34f;
				}
				if (tile.frameX == 176)
				{
					R = 0.3f;
					G = 0f;
					B = 0.17f;
				}
				if (tile.frameX == 198)
				{
					R = 0.3f;
					G = 0f;
					B = 0.35f;
				}
				if (tile.frameX == 220)
				{
					R = (float)Main.DiscoR / 255f * 0.25f;
					G = (float)Main.DiscoG / 255f * 0.25f;
					B = (float)Main.DiscoB / 255f * 0.25f;
				}
				break;
			case 370:
				R = 0.32f;
				G = 0.16f;
				B = 0.12f;
				break;
			case 659:
			case 667:
			case 708:
			{
				Vector4 shimmerBaseColor = LiquidRenderer.GetShimmerBaseColor(x, y);
				R = shimmerBaseColor.X;
				G = shimmerBaseColor.Y;
				B = shimmerBaseColor.Z;
				break;
			}
			case 711:
				R = 0.01f;
				G = 0.01f;
				B = 0.01f;
				break;
			case 27:
				if (tile.frameY < 36)
				{
					R = 0.3f;
					G = 0.27f;
				}
				break;
			case 336:
				R = 0.85f;
				G = 0.5f;
				B = 0.3f;
				break;
			case 340:
				R = 0.45f;
				G = 1f;
				B = 0.45f;
				break;
			case 341:
				R = 0.4f * Main.demonTorch + 0.6f * (1f - Main.demonTorch);
				G = 0.35f;
				B = 1f * Main.demonTorch + 0.6f * (1f - Main.demonTorch);
				break;
			case 342:
				R = 0.5f;
				G = 0.5f;
				B = 1.1f;
				break;
			case 343:
				R = 0.85f;
				G = 0.85f;
				B = 0.3f;
				break;
			case 344:
				R = 0.6f;
				G = 1.026f;
				B = 0.96000004f;
				break;
			case 327:
			{
				float num12 = 0.5f;
				num12 += (float)(270 - Main.mouseTextColor) / 1500f;
				num12 += (float)localRandom.Next(0, 50) * 0.0005f;
				R = 1f * num12;
				G = 0.5f * num12;
				B = 0.1f * num12;
				break;
			}
			case 316:
			case 317:
			case 318:
			{
				int num18 = x - tile.frameX / 18;
				int num19 = y - tile.frameY / 18;
				int num20 = num18 / 3 * (num19 / 3);
				num20 %= Main.cageFrames;
				int num21 = tile.type - 316;
				bool flag6 = Main.jellyfishCageMode[num21, num20] == 2;
				if (tile.type == 316)
				{
					if (flag6)
					{
						R = 0.2f;
						G = 0.3f;
						B = 0.8f;
					}
					else
					{
						R = 0.1f;
						G = 0.2f;
						B = 0.5f;
					}
				}
				if (tile.type == 317)
				{
					if (flag6)
					{
						R = 0.2f;
						G = 0.7f;
						B = 0.3f;
					}
					else
					{
						R = 0.05f;
						G = 0.45f;
						B = 0.1f;
					}
				}
				if (tile.type == 318)
				{
					if (flag6)
					{
						R = 0.7f;
						G = 0.2f;
						B = 0.5f;
					}
					else
					{
						R = 0.4f;
						G = 0.1f;
						B = 0.25f;
					}
				}
				break;
			}
			case 719:
			{
				int num14 = (x + y + (int)(Main.GlobalTimeWrappedHourly * 15f)) % 14;
				float num15 = 0f;
				float num16 = 0f;
				float num17 = 0f;
				switch (num14)
				{
				case 0:
					num15 = 255f;
					num16 = 171f;
					num17 = 183f;
					break;
				case 1:
					num15 = 255f;
					num16 = 170f;
					num17 = 220f;
					break;
				case 2:
					num15 = 252f;
					num16 = 171f;
					num17 = 255f;
					break;
				case 3:
					num15 = 224f;
					num16 = 171f;
					num17 = 255f;
					break;
				case 4:
					num15 = 192f;
					num16 = 171f;
					num17 = 255f;
					break;
				case 5:
					num15 = 174f;
					num16 = 178f;
					num17 = 255f;
					break;
				case 6:
					num15 = 168f;
					num16 = 195f;
					num17 = 255f;
					break;
				case 7:
					num15 = 167f;
					num16 = 224f;
					num17 = 255f;
					break;
				case 8:
					num15 = 168f;
					num16 = 255f;
					num17 = 252f;
					break;
				case 9:
					num15 = 162f;
					num16 = 255f;
					num17 = 233f;
					break;
				case 10:
					num15 = 158f;
					num16 = 255f;
					num17 = 198f;
					break;
				case 11:
					num15 = 207f;
					num16 = 255f;
					num17 = 173f;
					break;
				case 12:
					num15 = 255f;
					num16 = 213f;
					num17 = 186f;
					break;
				case 13:
					num15 = 255f;
					num16 = 192f;
					num17 = 182f;
					break;
				}
				R = num15 / 255f;
				G = num16 / 255f;
				B = num17 / 255f;
				break;
			}
			case 718:
				if (!Main.dayTime && !WorldGen.SolidTile3(x, y - 1))
				{
					R = localRandom.NextFloat() * 0.04f + 0.1f + (float)Main.DiscoR / 800f;
					G = localRandom.NextFloat() * 0.04f + 0.1f + (float)Main.DiscoG / 800f;
					B = localRandom.NextFloat() * 0.04f + 0.1f + (float)Main.DiscoB / 800f;
				}
				break;
			case 717:
			{
				float num12 = 0.55f;
				num12 += (float)(270 - Main.mouseTextColor) / 800f;
				num12 += localRandom.NextFloat() * 0.03f;
				num12 *= 0.5f;
				R = num12 * 1.1f;
				G = num12 * 0.4f;
				B = num12 * 0.1f;
				break;
			}
			case 429:
			{
				int num8 = tile.frameX / 18;
				bool flag2 = num8 % 2 >= 1;
				bool flag3 = num8 % 4 >= 2;
				bool flag4 = num8 % 8 >= 4;
				bool flag5 = num8 % 16 >= 8;
				if (flag2)
				{
					R += 0.5f;
				}
				if (flag3)
				{
					G += 0.5f;
				}
				if (flag4)
				{
					B += 0.5f;
				}
				if (flag5)
				{
					R += 0.2f;
					G += 0.2f;
				}
				break;
			}
			case 286:
			case 619:
				R = 0.1f;
				G = 0.2f;
				B = 0.7f;
				break;
			case 620:
			{
				Color val = new Color(230, 230, 230, 0).MultiplyRGBA(Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.5f % 1f, 1f, 0.5f));
				val *= 0.4f;
				R = (float)(int)val.R / 255f;
				G = (float)(int)val.G / 255f;
				B = (float)(int)val.B / 255f;
				break;
			}
			case 582:
			case 598:
				R = 0.7f;
				G = 0.2f;
				B = 0.1f;
				break;
			case 270:
				R = 0.73f;
				G = 1f;
				B = 0.41f;
				break;
			case 271:
				R = 0.45f;
				G = 0.95f;
				B = 1f;
				break;
			case 581:
				R = 1f;
				G = 0.75f;
				B = 0.5f;
				break;
			case 660:
				TorchID.TorchColor(23, out R, out G, out B);
				break;
			case 572:
				switch (tile.frameY / 36)
				{
				case 0:
					R = 0.9f;
					G = 0.5f;
					B = 0.7f;
					break;
				case 1:
					R = 0.7f;
					G = 0.55f;
					B = 0.96f;
					break;
				case 2:
					R = 0.45f;
					G = 0.96f;
					B = 0.95f;
					break;
				case 3:
					R = 0.5f;
					G = 0.96f;
					B = 0.62f;
					break;
				case 4:
					R = 0.47f;
					G = 0.69f;
					B = 0.95f;
					break;
				case 5:
					R = 0.92f;
					G = 0.57f;
					B = 0.51f;
					break;
				}
				break;
			case 262:
				R = 0.75f;
				B = 0.75f;
				break;
			case 263:
				R = 0.75f;
				G = 0.75f;
				break;
			case 264:
				B = 0.75f;
				break;
			case 265:
				G = 0.75f;
				break;
			case 266:
				R = 0.75f;
				break;
			case 267:
				R = 0.75f;
				G = 0.75f;
				B = 0.75f;
				break;
			case 268:
				R = 0.75f;
				G = 0.375f;
				break;
			case 237:
				R = 0.1f;
				G = 0.1f;
				break;
			case 238:
				if ((double)lightColor.X < 0.5)
				{
					lightColor.X = 0.5f;
				}
				if ((double)lightColor.Z < 0.5)
				{
					lightColor.Z = 0.5f;
				}
				break;
			case 235:
				if ((double)lightColor.X < 0.6)
				{
					lightColor.X = 0.6f;
				}
				if ((double)lightColor.Y < 0.6)
				{
					lightColor.Y = 0.6f;
				}
				break;
			case 405:
				if (tile.frameX < 54)
				{
					float num26 = (float)localRandom.Next(28, 42) * 0.005f;
					num26 += (float)(270 - Main.mouseTextColor) / 700f;
					switch (tile.frameX / 54)
					{
					case 1:
						R = 0.7f;
						G = 1f;
						B = 0.5f;
						break;
					case 2:
						R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
						G = 0.3f;
						B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
						break;
					case 3:
						R = 0.45f;
						G = 0.75f;
						B = 1f;
						break;
					case 4:
						R = 1.15f;
						G = 1.15f;
						B = 0.5f;
						break;
					case 5:
						R = (float)Main.DiscoR / 255f;
						G = (float)Main.DiscoG / 255f;
						B = (float)Main.DiscoB / 255f;
						break;
					default:
						R = 0.9f;
						G = 0.3f;
						B = 0.1f;
						break;
					}
					R += num26;
					G += num26;
					B += num26;
				}
				break;
			case 215:
				if (tile.frameY < 36)
				{
					float num25 = (float)localRandom.Next(28, 42) * 0.005f;
					num25 += (float)(270 - Main.mouseTextColor) / 700f;
					switch (tile.frameX / 54)
					{
					case 1:
						R = 0.7f;
						G = 1f;
						B = 0.5f;
						break;
					case 2:
						R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
						G = 0.3f;
						B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
						break;
					case 3:
						R = 0.45f;
						G = 0.75f;
						B = 1f;
						break;
					case 4:
						R = 1.15f;
						G = 1.15f;
						B = 0.5f;
						break;
					case 5:
						R = (float)Main.DiscoR / 255f;
						G = (float)Main.DiscoG / 255f;
						B = (float)Main.DiscoB / 255f;
						break;
					case 6:
						R = 0.75f;
						G = 1.2824999f;
						B = 1.2f;
						break;
					case 7:
						R = 0.95f;
						G = 0.65f;
						B = 1.3f;
						break;
					case 8:
						R = 1.4f;
						G = 0.85f;
						B = 0.55f;
						break;
					case 9:
						R = 0.25f;
						G = 1.3f;
						B = 0.8f;
						break;
					case 10:
						R = 0.95f;
						G = 0.4f;
						B = 1.4f;
						break;
					case 11:
						R = 1.4f;
						G = 0.7f;
						B = 0.5f;
						break;
					case 12:
						R = 1.25f;
						G = 0.6f;
						B = 1.2f;
						break;
					case 13:
						R = 0.75f;
						G = 1.45f;
						B = 0.9f;
						break;
					case 14:
						R = 0.25f;
						G = 0.65f;
						B = 1f;
						break;
					case 15:
						TorchID.TorchColor(23, out R, out G, out B);
						break;
					default:
						R = 0.9f;
						G = 0.3f;
						B = 0.1f;
						break;
					}
					R += num25;
					G += num25;
					B += num25;
				}
				break;
			case 92:
				if (tile.frameY <= 18 && tile.frameX == 0)
				{
					R = 1f;
					G = 1f;
					B = 1f;
				}
				break;
			case 592:
				if (tile.frameY > 0)
				{
					float num24 = (float)localRandom.Next(28, 42) * 0.005f;
					num24 += (float)(270 - Main.mouseTextColor) / 700f;
					R = 1.35f;
					G = 0.45f;
					B = 0.15f;
					R += num24;
					G += num24;
					B += num24;
				}
				break;
			case 593:
				if (tile.frameX < 18)
				{
					R = 0.8f;
					G = 0.3f;
					B = 0.1f;
				}
				break;
			case 594:
				if (tile.frameX < 36)
				{
					R = 0.8f;
					G = 0.3f;
					B = 0.1f;
				}
				break;
			case 548:
				if (tile.frameX / 54 >= 7)
				{
					R = 0.7f;
					G = 0.3f;
					B = 0.2f;
				}
				break;
			case 613:
			case 614:
				R = 0.7f;
				G = 0.3f;
				B = 0.2f;
				break;
			case 93:
				if (tile.frameX != 0)
				{
					break;
				}
				switch (tile.frameY / 54)
				{
				case 1:
					R = 0.95f;
					G = 0.95f;
					B = 0.5f;
					break;
				case 2:
					R = 0.85f;
					G = 0.6f;
					B = 1f;
					break;
				case 3:
					R = 0.75f;
					G = 1f;
					B = 0.6f;
					break;
				case 4:
				case 5:
					R = 0.75f;
					G = 0.85f;
					B = 1f;
					break;
				case 6:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 7:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				case 9:
					R = 1f;
					G = 1f;
					B = 0.7f;
					break;
				case 10:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 12:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 13:
					R = 1f;
					G = 1f;
					B = 0.6f;
					break;
				case 14:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 18:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 19:
					if (tile.color() == 0)
					{
						R = 0.37f;
						G = 0.8f;
						B = 1f;
					}
					else
					{
						flag = true;
					}
					break;
				case 20:
					R = 0f;
					G = 0.9f;
					B = 1f;
					break;
				case 21:
					R = 0.25f;
					G = 0.7f;
					B = 1f;
					break;
				case 23:
					R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
					G = 0.3f;
					B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
					break;
				case 24:
					R = 0.35f;
					G = 0.5f;
					B = 0.3f;
					break;
				case 25:
					R = 0.34f;
					G = 0.4f;
					B = 0.31f;
					break;
				case 26:
					R = 0.25f;
					G = 0.32f;
					B = 0.5f;
					break;
				case 29:
					R = 0.9f;
					G = 0.75f;
					B = 1f;
					break;
				case 30:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 31:
				{
					val3 = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f);
					Vector3 val12 = val3.ToVector3() * 1.2f;
					R = val12.X;
					G = val12.Y;
					B = val12.Z;
					break;
				}
				case 32:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				case 33:
					R = 0.55f;
					G = 0.45f;
					B = 0.95f;
					break;
				case 34:
					R = 1f;
					G = 0.6f;
					B = 0.1f;
					break;
				case 35:
					R = 0.3f;
					G = 0.75f;
					B = 0.55f;
					break;
				case 36:
					R = 0.9f;
					G = 0.55f;
					B = 0.7f;
					break;
				case 37:
					R = 0.55f;
					G = 0.85f;
					B = 1f;
					break;
				case 38:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 39:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 40:
					R = 0.4f;
					G = 0.8f;
					B = 0.9f;
					break;
				case 41:
					R = 1f;
					G = 1f;
					B = 1f;
					break;
				case 42:
					R = 0.95f;
					G = 0.5f;
					B = 0.4f;
					break;
				case 43:
				{
					Vector4 val11 = LiquidRenderer.GetShimmerBaseColor(x, y) * 1.5f;
					R = MathHelper.Clamp(val11.X, 0f, 1f);
					G = MathHelper.Clamp(val11.Y, 0f, 1f);
					B = MathHelper.Clamp(val11.Z, 0f, 1f);
					break;
				}
				case 44:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 45:
					R = 1f;
					G = 2f / 3f;
					B = 198f / 255f;
					break;
				case 46:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 47:
					R = 243f / 255f;
					G = 231f / 255f;
					B = 92f / 255f;
					break;
				case 48:
					R = 162f / 255f;
					G = 128f / 255f;
					B = 1f;
					break;
				case 49:
					R = 1f;
					G = 100f / 255f;
					B = 100f / 255f;
					break;
				case 50:
					R = 190f / 255f;
					G = 190f / 255f;
					B = 1f;
					break;
				case 51:
					R = 2f / 3f;
					G = 180f / 255f;
					B = 1f;
					break;
				case 52:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 53:
					R = 1f;
					G = 0.95f;
					B = 0.75f;
					break;
				case 54:
					R = 1f;
					G = 0.85499996f;
					B = 0.585f;
					break;
				case 55:
					R = 0.5f;
					G = 0.9f;
					B = 1f;
					flag = true;
					break;
				case 56:
					R = 1f;
					G = 0.9f;
					B = 0.9f;
					break;
				case 57:
					R = 180f / 255f;
					G = 230f / 255f;
					B = 1f;
					break;
				case 58:
					R = 150f / 255f;
					G = 235f / 255f;
					B = 245f / 255f;
					break;
				case 59:
					R = 2f / 3f;
					G = 245f / 255f;
					B = 1f;
					break;
				case 60:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 61:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 62:
					R = 235f / 255f;
					G = 105f / 255f;
					B = 1f;
					break;
				case 63:
					R = 190f / 255f;
					G = 190f / 255f;
					B = 1f;
					break;
				case 64:
					R = 215f / 255f;
					G = 175f / 255f;
					B = 245f / 255f;
					break;
				default:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				}
				break;
			case 96:
				if (tile.frameX >= 36)
				{
					R = 0.5f;
					G = 0.35f;
					B = 0.1f;
				}
				break;
			case 98:
				if (tile.frameY == 0)
				{
					R = 1f;
					G = 0.97f;
					B = 0.85f;
				}
				break;
			case 4:
				if (tile.frameX < 66)
				{
					TorchID.TorchColor(tile.frameY / 22, out R, out G, out B);
				}
				break;
			case 372:
				if (tile.frameX == 0)
				{
					R = 0.9f;
					G = 0.1f;
					B = 0.75f;
				}
				break;
			case 646:
				if (tile.frameX == 0)
				{
					R = 0.2f;
					G = 0.3f;
					B = 0.32f;
				}
				break;
			case 33:
				if (tile.frameX != 0)
				{
					break;
				}
				switch (tile.frameY / 22)
				{
				case 0:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 1:
					R = 0.55f;
					G = 0.85f;
					B = 0.35f;
					break;
				case 2:
					R = 0.65f;
					G = 0.95f;
					B = 0.5f;
					break;
				case 3:
					R = 0.2f;
					G = 0.75f;
					B = 1f;
					break;
				case 5:
					R = 0.85f;
					G = 0.6f;
					B = 1f;
					break;
				case 7:
				case 8:
					R = 0.75f;
					G = 0.85f;
					B = 1f;
					break;
				case 9:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 10:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				case 14:
					R = 1f;
					G = 1f;
					B = 0.6f;
					break;
				case 15:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 18:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 19:
					if (tile.color() == 0)
					{
						R = 0.37f;
						G = 0.8f;
						B = 1f;
					}
					else
					{
						flag = true;
					}
					break;
				case 20:
					R = 0f;
					G = 0.9f;
					B = 1f;
					break;
				case 21:
					R = 0.25f;
					G = 0.7f;
					B = 1f;
					break;
				case 23:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 24:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 25:
					R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
					G = 0.3f;
					B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
					break;
				case 28:
					R = 0.9f;
					G = 0.75f;
					B = 1f;
					break;
				case 29:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 30:
				{
					val3 = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f);
					Vector3 val10 = val3.ToVector3() * 1.2f;
					R = val10.X;
					G = val10.Y;
					B = val10.Z;
					break;
				}
				case 31:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				case 32:
					R = 0.55f;
					G = 0.45f;
					B = 0.95f;
					break;
				case 33:
					R = 1f;
					G = 0.6f;
					B = 0.1f;
					break;
				case 34:
					R = 0.3f;
					G = 0.75f;
					B = 0.55f;
					break;
				case 35:
					R = 0.9f;
					G = 0.55f;
					B = 0.7f;
					break;
				case 36:
					R = 0.55f;
					G = 0.85f;
					B = 1f;
					break;
				case 37:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 38:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 39:
					R = 0.4f;
					G = 0.8f;
					B = 0.9f;
					break;
				case 40:
					R = 1f;
					G = 1f;
					B = 1f;
					break;
				case 41:
					R = 0.95f;
					G = 0.5f;
					B = 0.4f;
					break;
				case 42:
				{
					Vector4 val9 = LiquidRenderer.GetShimmerBaseColor(x, y) * 1.5f;
					R = MathHelper.Clamp(val9.X, 0f, 1f);
					G = MathHelper.Clamp(val9.Y, 0f, 1f);
					B = MathHelper.Clamp(val9.Z, 0f, 1f);
					break;
				}
				case 43:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 44:
					R = 1f;
					G = 2f / 3f;
					B = 198f / 255f;
					break;
				case 45:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 46:
					R = 243f / 255f;
					G = 231f / 255f;
					B = 92f / 255f;
					break;
				case 47:
					R = 162f / 255f;
					G = 128f / 255f;
					B = 1f;
					break;
				case 48:
					R = 1f;
					G = 100f / 255f;
					B = 100f / 255f;
					break;
				case 49:
					R = 190f / 255f;
					G = 190f / 255f;
					B = 1f;
					break;
				case 50:
					R = 2f / 3f;
					G = 180f / 255f;
					B = 1f;
					break;
				case 51:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 52:
					R = 1f;
					G = 0.95f;
					B = 0.75f;
					break;
				case 53:
					R = 1f;
					G = 0.85499996f;
					B = 0.585f;
					break;
				case 54:
					R = 0.5f;
					G = 0.9f;
					B = 1f;
					flag = true;
					break;
				case 55:
					R = 1f;
					G = 0.9f;
					B = 0.9f;
					break;
				case 56:
					R = 180f / 255f;
					G = 230f / 255f;
					B = 1f;
					break;
				case 57:
					R = 150f / 255f;
					G = 235f / 255f;
					B = 245f / 255f;
					break;
				case 58:
					R = 2f / 3f;
					G = 245f / 255f;
					B = 1f;
					break;
				case 59:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 60:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 61:
					R = 235f / 255f;
					G = 105f / 255f;
					B = 1f;
					break;
				case 62:
					R = 190f / 255f;
					G = 190f / 255f;
					B = 1f;
					break;
				case 63:
					R = 215f / 255f;
					G = 175f / 255f;
					B = 245f / 255f;
					break;
				default:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				}
				break;
			case 174:
				if (tile.frameX == 0)
				{
					R = 1f;
					G = 0.95f;
					B = 0.65f;
				}
				break;
			case 100:
			case 173:
				if (tile.frameX >= 36)
				{
					break;
				}
				switch (tile.frameY / 36)
				{
				case 1:
					R = 0.95f;
					G = 0.95f;
					B = 0.5f;
					break;
				case 2:
					R = 0.85f;
					G = 0.6f;
					B = 1f;
					break;
				case 3:
					R = 1f;
					G = 0.6f;
					B = 0.6f;
					break;
				case 5:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 6:
				case 7:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 8:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				case 9:
					R = 0.75f;
					G = 0.85f;
					B = 1f;
					break;
				case 11:
					R = 1f;
					G = 1f;
					B = 0.7f;
					break;
				case 12:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 13:
					R = 1f;
					G = 1f;
					B = 0.6f;
					break;
				case 14:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 18:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 19:
					if (tile.color() == 0)
					{
						R = 0.37f;
						G = 0.8f;
						B = 1f;
					}
					else
					{
						flag = true;
					}
					break;
				case 20:
					R = 0f;
					G = 0.9f;
					B = 1f;
					break;
				case 21:
					R = 0.25f;
					G = 0.7f;
					B = 1f;
					break;
				case 25:
					R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
					G = 0.3f;
					B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
					break;
				case 22:
					R = 0.35f;
					G = 0.5f;
					B = 0.3f;
					break;
				case 23:
					R = 0.34f;
					G = 0.4f;
					B = 0.31f;
					break;
				case 24:
					R = 0.25f;
					G = 0.32f;
					B = 0.5f;
					break;
				case 29:
					R = 0.9f;
					G = 0.75f;
					B = 1f;
					break;
				case 30:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 31:
				{
					val3 = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f);
					Vector3 val8 = val3.ToVector3() * 1.2f;
					R = val8.X;
					G = val8.Y;
					B = val8.Z;
					break;
				}
				case 32:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				case 33:
					R = 0.55f;
					G = 0.45f;
					B = 0.95f;
					break;
				case 34:
					R = 1f;
					G = 0.6f;
					B = 0.1f;
					break;
				case 35:
					R = 0.3f;
					G = 0.75f;
					B = 0.55f;
					break;
				case 36:
					R = 0.9f;
					G = 0.55f;
					B = 0.7f;
					break;
				case 37:
					R = 0.55f;
					G = 0.85f;
					B = 1f;
					break;
				case 38:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 39:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 40:
					R = 0.4f;
					G = 0.8f;
					B = 0.9f;
					break;
				case 41:
					R = 1f;
					G = 1f;
					B = 1f;
					break;
				case 42:
					R = 0.95f;
					G = 0.5f;
					B = 0.4f;
					break;
				case 43:
				{
					Vector4 val7 = LiquidRenderer.GetShimmerBaseColor(x, y) * 1.5f;
					R = MathHelper.Clamp(val7.X, 0f, 1f);
					G = MathHelper.Clamp(val7.Y, 0f, 1f);
					B = MathHelper.Clamp(val7.Z, 0f, 1f);
					break;
				}
				case 44:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 45:
					R = 1f;
					G = 2f / 3f;
					B = 198f / 255f;
					break;
				case 46:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 47:
					R = 243f / 255f;
					G = 231f / 255f;
					B = 92f / 255f;
					break;
				case 48:
					R = 162f / 255f;
					G = 128f / 255f;
					B = 1f;
					break;
				case 49:
					R = 1f;
					G = 100f / 255f;
					B = 100f / 255f;
					break;
				case 50:
					R = 190f / 255f;
					G = 190f / 255f;
					B = 1f;
					break;
				case 51:
					R = 2f / 3f;
					G = 180f / 255f;
					B = 1f;
					break;
				case 52:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 53:
					R = 1f;
					G = 0.95f;
					B = 0.75f;
					break;
				case 54:
					R = 1f;
					G = 0.85499996f;
					B = 0.585f;
					break;
				case 55:
					R = 0.5f;
					G = 0.9f;
					B = 1f;
					flag = true;
					break;
				case 56:
					R = 1f;
					G = 0.9f;
					B = 0.9f;
					break;
				case 57:
					R = 180f / 255f;
					G = 230f / 255f;
					B = 1f;
					break;
				case 58:
					R = 150f / 255f;
					G = 235f / 255f;
					B = 245f / 255f;
					break;
				case 59:
					R = 2f / 3f;
					G = 245f / 255f;
					B = 1f;
					break;
				case 60:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 61:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 62:
					R = 235f / 255f;
					G = 105f / 255f;
					B = 1f;
					break;
				case 63:
					R = 190f / 255f;
					G = 190f / 255f;
					B = 1f;
					break;
				case 64:
					R = 215f / 255f;
					G = 175f / 255f;
					B = 245f / 255f;
					break;
				default:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				}
				break;
			case 34:
			{
				if (tile.frameX % 108 >= 54)
				{
					break;
				}
				int num23 = tile.frameY / 54;
				switch (num23 + 37 * (tile.frameX / 108))
				{
				case 7:
					R = 0.95f;
					G = 0.95f;
					B = 0.5f;
					break;
				case 8:
					R = 0.85f;
					G = 0.6f;
					B = 1f;
					break;
				case 9:
					R = 1f;
					G = 0.6f;
					B = 0.6f;
					break;
				case 12:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 13:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				case 11:
				case 17:
					R = 0.75f;
					G = 0.85f;
					B = 1f;
					break;
				case 15:
					R = 1f;
					G = 1f;
					B = 0.7f;
					break;
				case 16:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 18:
					R = 1f;
					G = 1f;
					B = 0.6f;
					break;
				case 19:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 23:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 24:
					if (tile.color() == 0)
					{
						R = 0.37f;
						G = 0.8f;
						B = 1f;
					}
					else
					{
						flag = true;
					}
					break;
				case 25:
					R = 0f;
					G = 0.9f;
					B = 1f;
					break;
				case 26:
					R = 0.25f;
					G = 0.7f;
					B = 1f;
					break;
				case 27:
					R = 0.55f;
					G = 0.85f;
					B = 0.35f;
					break;
				case 28:
					R = 0.65f;
					G = 0.95f;
					B = 0.5f;
					break;
				case 29:
					R = 0.2f;
					G = 0.75f;
					B = 1f;
					break;
				case 30:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 32:
					R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
					G = 0.3f;
					B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
					break;
				case 35:
					R = 0.9f;
					G = 0.75f;
					B = 1f;
					break;
				case 36:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 37:
				{
					val3 = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f);
					Vector3 val6 = val3.ToVector3() * 1.2f;
					R = val6.X;
					G = val6.Y;
					B = val6.Z;
					break;
				}
				case 38:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				case 39:
					R = 0.55f;
					G = 0.45f;
					B = 0.95f;
					break;
				case 40:
					R = 1f;
					G = 0.6f;
					B = 0.1f;
					break;
				case 41:
					R = 0.3f;
					G = 0.75f;
					B = 0.55f;
					break;
				case 42:
					R = 0.9f;
					G = 0.55f;
					B = 0.7f;
					break;
				case 43:
					R = 0.55f;
					G = 0.85f;
					B = 1f;
					break;
				case 44:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 45:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 46:
					R = 0.4f;
					G = 0.8f;
					B = 0.9f;
					break;
				case 47:
					R = 1f;
					G = 1f;
					B = 1f;
					break;
				case 48:
					R = 0.95f;
					G = 0.5f;
					B = 0.4f;
					break;
				case 49:
				{
					Vector4 val5 = LiquidRenderer.GetShimmerBaseColor(x, y) * 1.5f;
					R = MathHelper.Clamp(val5.X, 0f, 1f);
					G = MathHelper.Clamp(val5.Y, 0f, 1f);
					B = MathHelper.Clamp(val5.Z, 0f, 1f);
					break;
				}
				case 50:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 51:
					R = 1f;
					G = 2f / 3f;
					B = 198f / 255f;
					break;
				case 52:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 53:
					R = 243f / 255f;
					G = 231f / 255f;
					B = 92f / 255f;
					break;
				case 54:
					R = 162f / 255f;
					G = 128f / 255f;
					B = 1f;
					break;
				case 55:
					R = 1f;
					G = 100f / 255f;
					B = 100f / 255f;
					break;
				case 56:
					R = 190f / 255f;
					G = 190f / 255f;
					B = 1f;
					break;
				case 57:
					R = 2f / 3f;
					G = 180f / 255f;
					B = 1f;
					break;
				case 58:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 59:
					R = 1f;
					G = 0.95f;
					B = 0.75f;
					break;
				case 60:
					R = 1f;
					G = 0.85499996f;
					B = 0.585f;
					break;
				case 61:
					R = 0.5f;
					G = 0.9f;
					B = 1f;
					flag = true;
					break;
				case 62:
					R = 1f;
					G = 0.9f;
					B = 0.9f;
					break;
				case 63:
					R = 180f / 255f;
					G = 230f / 255f;
					B = 1f;
					break;
				case 64:
					R = 150f / 255f;
					G = 235f / 255f;
					B = 245f / 255f;
					break;
				case 65:
					R = 2f / 3f;
					G = 245f / 255f;
					B = 1f;
					break;
				case 66:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 67:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 68:
					R = 235f / 255f;
					G = 105f / 255f;
					B = 1f;
					break;
				case 69:
					R = 190f / 255f;
					G = 190f / 255f;
					B = 1f;
					break;
				case 70:
					R = 215f / 255f;
					G = 175f / 255f;
					B = 245f / 255f;
					break;
				default:
					R = 1f;
					G = 0.95f;
					B = 0.8f;
					break;
				}
				break;
			}
			case 35:
				if (tile.frameX < 36)
				{
					R = 0.75f;
					G = 0.6f;
					B = 0.3f;
				}
				break;
			case 95:
				if (tile.frameX < 36)
				{
					R = 1f;
					G = 0.95f;
					B = 0.8f;
				}
				break;
			case 17:
			case 133:
			case 302:
				R = 0.83f;
				G = 0.6f;
				B = 0.5f;
				break;
			case 77:
				R = 0.75f;
				G = 0.45f;
				B = 0.25f;
				break;
			case 37:
			{
				float num22 = (float)localRandom.Next(95, 106) * 0.01f;
				R = 0.56f * num22;
				G = 0.43f * num22;
				B = 0.15f * num22;
				break;
			}
			case 22:
			case 140:
				if (tile.color() != 27 && tile.color() != 26)
				{
					R = 0.12f;
				}
				G = 0.07f;
				B = 0.32f;
				break;
			case 171:
				if (tile.frameX < 10)
				{
					x -= tile.frameX;
					y -= tile.frameY;
				}
				switch ((Main.tile[x, y].frameY & 0x3C00) >> 10)
				{
				case 1:
					R = 0.1f;
					G = 0.1f;
					B = 0.1f;
					break;
				case 2:
					R = 0.2f;
					break;
				case 3:
					G = 0.2f;
					break;
				case 4:
					B = 0.2f;
					break;
				case 5:
					R = 0.125f;
					G = 0.125f;
					break;
				case 6:
					R = 0.2f;
					G = 0.1f;
					break;
				case 7:
					R = 0.125f;
					G = 0.125f;
					break;
				case 8:
					R = 0.08f;
					G = 0.175f;
					break;
				case 9:
					G = 0.125f;
					B = 0.125f;
					break;
				case 10:
					R = 0.125f;
					B = 0.125f;
					break;
				case 11:
					R = 0.1f;
					G = 0.1f;
					B = 0.2f;
					break;
				default:
					R = (G = (B = 0f));
					break;
				}
				R *= 0.5f;
				G *= 0.5f;
				B *= 0.5f;
				break;
			case 204:
			case 347:
				if (tile.color() != 27 && tile.color() != 26)
				{
					R = 0.35f;
				}
				break;
			case 42:
				if (tile.frameX != 0)
				{
					break;
				}
				switch (tile.frameY / 36)
				{
				case 0:
					R = 0.7f;
					G = 0.65f;
					B = 0.55f;
					break;
				case 1:
					R = 0.9f;
					G = 0.75f;
					B = 0.6f;
					break;
				case 2:
					R = 0.8f;
					G = 0.6f;
					B = 0.6f;
					break;
				case 3:
					R = 0.65f;
					G = 0.5f;
					B = 0.2f;
					break;
				case 4:
					R = 0.5f;
					G = 0.7f;
					B = 0.4f;
					break;
				case 5:
					R = 0.9f;
					G = 0.4f;
					B = 0.2f;
					break;
				case 6:
					R = 0.7f;
					G = 0.75f;
					B = 0.3f;
					break;
				case 7:
				{
					float num13 = Main.demonTorch * 0.2f;
					R = 0.9f - num13;
					G = 0.9f - num13;
					B = 0.7f + num13;
					break;
				}
				case 8:
					R = 0.75f;
					G = 0.6f;
					B = 0.3f;
					break;
				case 9:
					R = 1f;
					G = 0.3f;
					B = 0.5f;
					B += Main.demonTorch * 0.2f;
					R -= Main.demonTorch * 0.1f;
					G -= Main.demonTorch * 0.2f;
					break;
				case 11:
					R = 0.85f;
					G = 0.6f;
					B = 1f;
					break;
				case 14:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 15:
				case 16:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 17:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				case 18:
					R = 0.75f;
					G = 0.85f;
					B = 1f;
					break;
				case 21:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 22:
					R = 1f;
					G = 1f;
					B = 0.6f;
					break;
				case 23:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 27:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 28:
					if (tile.color() == 0)
					{
						R = 0.37f;
						G = 0.8f;
						B = 1f;
					}
					else
					{
						flag = true;
					}
					break;
				case 29:
					R = 0f;
					G = 0.9f;
					B = 1f;
					break;
				case 30:
					R = 0.25f;
					G = 0.7f;
					B = 1f;
					break;
				case 32:
					R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
					G = 0.3f;
					B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
					break;
				case 35:
					R = 0.7f;
					G = 0.6f;
					B = 0.9f;
					break;
				case 36:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 37:
				{
					val3 = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f);
					Vector3 val4 = val3.ToVector3() * 1.2f;
					R = val4.X;
					G = val4.Y;
					B = val4.Z;
					break;
				}
				case 38:
					R = 1f;
					G = 0.97f;
					B = 0.85f;
					break;
				case 39:
					R = 0.55f;
					G = 0.45f;
					B = 0.95f;
					break;
				case 40:
					R = 1f;
					G = 0.6f;
					B = 0.1f;
					break;
				case 41:
					R = 0.3f;
					G = 0.75f;
					B = 0.55f;
					break;
				case 42:
					R = 0.9f;
					G = 0.55f;
					B = 0.7f;
					break;
				case 43:
					R = 0.55f;
					G = 0.85f;
					B = 1f;
					break;
				case 44:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 45:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 46:
					R = 0.4f;
					G = 0.8f;
					B = 0.9f;
					break;
				case 47:
					R = 1f;
					G = 1f;
					B = 1f;
					break;
				case 48:
					R = 0.95f;
					G = 0.5f;
					B = 0.4f;
					break;
				case 49:
				{
					Vector4 val2 = LiquidRenderer.GetShimmerBaseColor(x, y) * 1.5f;
					R = MathHelper.Clamp(val2.X, 0f, 1f);
					G = MathHelper.Clamp(val2.Y, 0f, 1f);
					B = MathHelper.Clamp(val2.Z, 0f, 1f);
					break;
				}
				case 50:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 51:
					R = 1f;
					G = 2f / 3f;
					B = 198f / 255f;
					break;
				case 52:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 53:
					R = 243f / 255f;
					G = 231f / 255f;
					B = 92f / 255f;
					break;
				case 54:
					R = 162f / 255f;
					G = 128f / 255f;
					B = 1f;
					break;
				case 55:
					R = 1f;
					G = 100f / 255f;
					B = 100f / 255f;
					break;
				case 56:
					R = 190f / 255f;
					G = 190f / 255f;
					B = 1f;
					break;
				case 57:
					R = 2f / 3f;
					G = 180f / 255f;
					B = 1f;
					break;
				case 58:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 59:
					R = 1f;
					G = 0.95f;
					B = 0.75f;
					break;
				case 60:
					R = 1f;
					G = 0.85499996f;
					B = 0.585f;
					break;
				case 61:
					R = 0.5f;
					G = 0.9f;
					B = 1f;
					flag = true;
					break;
				case 62:
					R = 1f;
					G = 0.9f;
					B = 0.9f;
					break;
				case 63:
					R = 180f / 255f;
					G = 230f / 255f;
					B = 1f;
					break;
				case 64:
					R = 150f / 255f;
					G = 235f / 255f;
					B = 245f / 255f;
					break;
				case 65:
					R = 2f / 3f;
					G = 245f / 255f;
					B = 1f;
					break;
				case 66:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 67:
					R = 1f;
					G = 0.95f;
					B = 0.65f;
					break;
				case 68:
					R = 235f / 255f;
					G = 105f / 255f;
					B = 1f;
					break;
				case 69:
					R = 190f / 255f;
					G = 190f / 255f;
					B = 1f;
					break;
				case 70:
					R = 215f / 255f;
					G = 175f / 255f;
					B = 245f / 255f;
					break;
				default:
					R = 1f;
					G = 1f;
					B = 1f;
					break;
				}
				break;
			case 49:
				if (tile.frameX == 0)
				{
					R = 0f;
					G = 0.35f;
					B = 0.8f;
				}
				break;
			case 519:
				if (tile.frameY == 90)
				{
					if (tile.color() == 0)
					{
						float num11 = (float)localRandom.Next(28, 42) * 0.005f;
						num11 += (float)(270 - Main.mouseTextColor) / 1000f;
						R = 0.1f;
						G = 0.2f + num11 / 2f;
						B = 0.7f + num11;
					}
					else
					{
						flag = true;
					}
				}
				break;
			case 70:
			case 71:
			case 72:
			case 190:
			case 348:
			case 349:
			case 528:
			case 578:
				if (tile.type != 349 || tile.frameX >= 36)
				{
					float num10 = (float)localRandom.Next(28, 42) * 0.005f;
					num10 += (float)(270 - Main.mouseTextColor) / 1000f;
					if (tile.color() == 0)
					{
						R = 0f;
						G = 0.2f + num10 / 2f;
						B = 1f;
					}
					else
					{
						flag = true;
					}
				}
				break;
			case 739:
				R = 0.35f;
				G = 0.63f;
				B = 0.7f;
				flag = true;
				break;
			case 350:
			{
				double num9 = Main.timeForVisualEffects * 0.08;
				B = (G = (R = (float)((0.0 - Math.Cos(((int)(num9 / 6.283) % 3 == 1) ? num9 : 0.0)) * 0.1 + 0.1)));
				break;
			}
			case 61:
			case 703:
				if (tile.frameX == 144)
				{
					float num6 = 1f + (float)(270 - Main.mouseTextColor) / 400f;
					float num7 = 0.8f - (float)(270 - Main.mouseTextColor) / 400f;
					R = 0.42f * num7;
					G = 0.81f * num6;
					B = 0.52f * num7;
				}
				break;
			case 26:
			case 31:
			case 695:
			case 696:
				if (((tile.type == 31 || tile.type == 696) && tile.frameX >= 36) || ((tile.type == 26 || tile.type == 695) && tile.frameX >= 54))
				{
					float num4 = (float)localRandom.Next(-5, 6) * 0.0025f;
					R = 0.5f + num4 * 2f;
					G = 0.2f + num4;
					B = 0.1f;
				}
				else
				{
					float num5 = (float)localRandom.Next(-5, 6) * 0.0025f;
					R = 0.31f + num5;
					G = 0.1f;
					B = 0.44f + num5 * 2f;
				}
				break;
			case 699:
				R = 0.4f;
				G = 0.2f;
				B = 0.15f;
				break;
			case 84:
			{
				int num2 = tile.frameX / 18;
				float num3 = 0f;
				switch (num2)
				{
				case 2:
					num3 = (float)(270 - Main.mouseTextColor) / 400f;
					if (num3 > 1f)
					{
						num3 = 1f;
					}
					else if (num3 < 0f)
					{
						num3 = 0f;
					}
					R = num3 * 1.4f;
					G = num3 * 1.2f;
					B = num3 / 2f;
					break;
				case 5:
					num3 = 0.9f;
					R = num3;
					G = num3 * 0.8f;
					B = num3 * 0.2f;
					break;
				case 6:
					num3 = 0.08f;
					G = num3 * 0.8f;
					B = num3;
					break;
				}
				break;
			}
			case 83:
				if (tile.frameX == 18 && !Main.dayTime)
				{
					R = 0.1f;
					G = 0.4f;
					B = 0.6f;
				}
				if (tile.frameX == 90 && !Main.raining && Main.time > 40500.0)
				{
					R = 0.9f;
					G = 0.72f;
					B = 0.18f;
				}
				break;
			case 126:
				if (tile.frameX < 36)
				{
					R = (float)Main.DiscoR / 255f;
					G = (float)Main.DiscoG / 255f;
					B = (float)Main.DiscoB / 255f;
				}
				break;
			case 125:
			{
				float num = (float)localRandom.Next(28, 42) * 0.01f;
				num += (float)(270 - Main.mouseTextColor) / 800f;
				G = (lightColor.Y = 0.3f * num);
				B = (lightColor.Z = 0.6f * num);
				break;
			}
			case 129:
				switch (tile.frameX / 18 % 3)
				{
				case 0:
					R = 0f;
					G = 0.05f;
					B = 0.25f;
					break;
				case 1:
					R = 0.2f;
					G = 0f;
					B = 0.15f;
					break;
				case 2:
					R = 0.1f;
					G = 0f;
					B = 0.2f;
					break;
				}
				break;
			case 149:
				if (tile.frameX <= 36)
				{
					switch (tile.frameX / 18)
					{
					case 0:
						R = 0.1f;
						G = 0.2f;
						B = 0.5f;
						break;
					case 1:
						R = 0.5f;
						G = 0.1f;
						B = 0.1f;
						break;
					case 2:
						R = 0.2f;
						G = 0.5f;
						B = 0.1f;
						break;
					}
					R *= (float)localRandom.Next(970, 1031) * 0.001f;
					G *= (float)localRandom.Next(970, 1031) * 0.001f;
					B *= (float)localRandom.Next(970, 1031) * 0.001f;
				}
				break;
			case 160:
				R = (float)Main.DiscoR / 255f * 0.25f;
				G = (float)Main.DiscoG / 255f * 0.25f;
				B = (float)Main.DiscoB / 255f * 0.25f;
				break;
			case 354:
				R = 0.65f;
				G = 0.35f;
				B = 0.15f;
				break;
			}
		}
		if (flag && tile.color() != 0)
		{
			Color val15 = WorldGen.paintColor(tile.color());
			R = (float)(int)val15.R / 255f;
			G = (float)(int)val15.G / 255f;
			B = (float)(int)val15.B / 255f;
		}
		if (lightColor.X < R)
		{
			lightColor.X = R;
		}
		if (lightColor.Y < G)
		{
			lightColor.Y = G;
		}
		if (lightColor.Z < B)
		{
			lightColor.Z = B;
		}
	}

	private void ApplySurfaceLight(Tile tile, int x, int y, ref Vector3 lightColor)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = (float)(int)Main.tileColor.R / 255f;
		float num5 = (float)(int)Main.tileColor.G / 255f;
		float num6 = (float)(int)Main.tileColor.B / 255f;
		float num7 = (num4 + num5 + num6) / 3f;
		if (tile.active() && TileID.Sets.AllowLightInWater[tile.type])
		{
			if (lightColor.X < num7 && (Main.wallLight[tile.wall] || tile.wall == 73 || tile.wall == 227 || (tile.invisibleWall() && !_drawInvisibleWalls)))
			{
				num = num4;
				num2 = num5;
				num3 = num6;
			}
		}
		else if ((!tile.active() || !Main.tileNoSunLight[tile.type] || ((tile.slope() != 0 || tile.halfBrick() || (tile.invisibleBlock() && !_drawInvisibleWalls)) && Main.tile[x, y - 1].liquid == 0 && Main.tile[x, y + 1].liquid == 0 && Main.tile[x - 1, y].liquid == 0 && Main.tile[x + 1, y].liquid == 0)) && lightColor.X < num7 && (Main.wallLight[tile.wall] || tile.wall == 73 || tile.wall == 227 || (tile.invisibleWall() && !_drawInvisibleWalls)))
		{
			if (tile.liquid < 200)
			{
				if (!tile.halfBrick() || Main.tile[x, y - 1].liquid < 200)
				{
					num = num4;
					num2 = num5;
					num3 = num6;
				}
			}
			else if (Main.liquidAlpha[13] > 0f)
			{
				if (Main.rand == null)
				{
					Main.rand = new UnifiedRandom();
				}
				num3 = num6 * 0.175f * (1f + Main.rand.NextFloat() * 0.13f) * Main.liquidAlpha[13];
			}
		}
		if ((!tile.active() || tile.halfBrick() || !Main.tileNoSunLight[tile.type]) && ((tile.wall >= 88 && tile.wall <= 93) || tile.wall == 241) && tile.liquid < byte.MaxValue)
		{
			num = num4;
			num2 = num5;
			num3 = num6;
			int num8 = tile.wall - 88;
			if (tile.wall == 241)
			{
				num8 = 6;
			}
			switch (num8)
			{
			case 0:
				num *= 0.9f;
				num2 *= 0.15f;
				num3 *= 0.9f;
				break;
			case 1:
				num *= 0.9f;
				num2 *= 0.9f;
				num3 *= 0.15f;
				break;
			case 2:
				num *= 0.15f;
				num2 *= 0.15f;
				num3 *= 0.9f;
				break;
			case 3:
				num *= 0.15f;
				num2 *= 0.9f;
				num3 *= 0.15f;
				break;
			case 4:
				num *= 0.9f;
				num2 *= 0.15f;
				num3 *= 0.15f;
				break;
			case 5:
			{
				float num9 = 0.2f;
				float num10 = 0.7f - num9;
				num *= num10 + (float)Main.DiscoR / 255f * num9;
				num2 *= num10 + (float)Main.DiscoG / 255f * num9;
				num3 *= num10 + (float)Main.DiscoB / 255f * num9;
				break;
			}
			case 6:
				num *= 0.9f;
				num2 *= 0.5f;
				num3 *= 0f;
				break;
			}
		}
		float num11 = 1f - Main.shimmerDarken;
		num *= num11;
		num2 *= num11;
		num3 *= num11;
		if (lightColor.X < num)
		{
			lightColor.X = num;
		}
		if (lightColor.Y < num2)
		{
			lightColor.Y = num2;
		}
		if (lightColor.Z < num3)
		{
			lightColor.Z = num3;
		}
	}

	private void ApplyHellLight(Tile tile, int x, int y, ref Vector3 lightColor)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.08f;
		if ((!tile.active() || !Main.tileNoSunLight[tile.type] || ((tile.slope() != 0 || tile.halfBrick()) && Main.tile[x, y - 1].liquid == 0 && Main.tile[x, y + 1].liquid == 0 && Main.tile[x - 1, y].liquid == 0 && Main.tile[x + 1, y].liquid == 0)) && lightColor.X < num4 && (Main.wallLight[tile.wall] || tile.wall == 73 || tile.wall == 227 || (tile.invisibleWall() && !_drawInvisibleWalls)) && tile.liquid < 200 && (!tile.halfBrick() || Main.tile[x, y - 1].liquid < 200))
		{
			num = num4;
			num2 = num4 * 0.6f;
			num3 = num4 * 0.2f;
		}
		if ((!tile.active() || tile.halfBrick() || !Main.tileNoSunLight[tile.type]) && ((tile.wall >= 88 && tile.wall <= 93) || tile.wall == 241) && tile.liquid < byte.MaxValue)
		{
			num = num4;
			num2 = num4 * 0.6f;
			num3 = num4 * 0.2f;
			int num5 = tile.wall - 88;
			if (tile.wall == 241)
			{
				num5 = 6;
			}
			switch (num5)
			{
			case 0:
				num *= 0.9f;
				num2 *= 0.15f;
				num3 *= 0.9f;
				break;
			case 1:
				num *= 0.9f;
				num2 *= 0.9f;
				num3 *= 0.15f;
				break;
			case 2:
				num *= 0.15f;
				num2 *= 0.15f;
				num3 *= 0.9f;
				break;
			case 3:
				num *= 0.15f;
				num2 *= 0.9f;
				num3 *= 0.15f;
				break;
			case 4:
				num *= 0.9f;
				num2 *= 0.15f;
				num3 *= 0.15f;
				break;
			case 5:
			{
				float num6 = 0.2f;
				float num7 = 0.7f - num6;
				num *= num7 + (float)Main.DiscoR / 255f * num6;
				num2 *= num7 + (float)Main.DiscoG / 255f * num6;
				num3 *= num7 + (float)Main.DiscoB / 255f * num6;
				break;
			}
			case 6:
				num *= 0.9f;
				num2 *= 0.5f;
				num3 *= 0f;
				break;
			}
		}
		if (lightColor.X < num)
		{
			lightColor.X = num;
		}
		if (lightColor.Y < num2)
		{
			lightColor.Y = num2;
		}
		if (lightColor.Z < num3)
		{
			lightColor.Z = num3;
		}
	}
}
