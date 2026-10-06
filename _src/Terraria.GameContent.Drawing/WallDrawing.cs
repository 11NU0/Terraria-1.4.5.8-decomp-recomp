using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Testing;

namespace Terraria.GameContent.Drawing;

public class WallDrawing : TileDrawingBase
{
	public static bool QuickPaintLookup = true;

	private static VertexColors _glowPaintColors = new VertexColors(Color.White);

	private Tile[,] _tileArray;

	private TilePaintSystemV2 _paintSystem;

	private bool _shouldShowInvisibleWalls;

	private DrawBlackHelper drawBlackHelper;

	private TilePaintSystemV2.WallVariationKey _lastPaintLookupKey;

	private Texture2D _lastPaintLookupTexture;

	public void LerpVertexColorsWithColor(ref VertexColors colors, Color lerpColor, float percent)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		colors.TopLeftColor = Color.Lerp(colors.TopLeftColor, lerpColor, percent);
		colors.TopRightColor = Color.Lerp(colors.TopRightColor, lerpColor, percent);
		colors.BottomLeftColor = Color.Lerp(colors.BottomLeftColor, lerpColor, percent);
		colors.BottomRightColor = Color.Lerp(colors.BottomRightColor, lerpColor, percent);
	}

	public WallDrawing(TilePaintSystemV2 paintSystem)
	{
		_paintSystem = paintSystem;
	}

	public void Update()
	{
		if (!Main.dedServ)
		{
			_shouldShowInvisibleWalls = Main.ShouldShowInvisibleBlocksAndWalls();
		}
	}

	public static void DrawOutline(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Main.spriteBatch.Draw(texture, position, (Rectangle?)sourceRectangle, color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
	}

	public void DrawWalls()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		FlushLogData = TimeLogger.FlushWallTiles;
		DrawCallLogData = TimeLogger.WallDrawCalls;
		if (DebugOptions.hideWalls)
		{
			return;
		}
		float gfxQuality = Main.gfxQuality;
		Vector2 screenPosition = Main.screenPosition;
		int[] wallBlend = Main.wallBlend;
		_tileArray = Main.tile;
		int num = (int)(120f * (1f - gfxQuality) + 40f * gfxQuality);
		if (DebugOptions.devLightTilesCheat)
		{
			num = 1000;
		}
		int num2 = (int)((float)num * 0.4f);
		int num3 = (int)((float)num * 0.35f);
		int num4 = (int)((float)num * 0.3f);
		TileDrawing.GetScreenDrawArea(!Main.drawToScreen, out var drawOffSet, out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
		VertexColors vertices = default;
		Rectangle sourceRectangle = new Rectangle(0, 0, 32, 32);
		int underworldLayer = Main.UnderworldLayer;
		drawBlackHelper = new DrawBlackHelper(0u, drawOffSet);
		_lastPaintLookupKey = default;
		for (int i = firstTileY; i < lastTileY; i++)
		{
			for (int j = firstTileX; j < lastTileX; j++)
			{
				Tile tile = _tileArray[j, i];
				if (tile == null)
				{
					tile = new Tile();
					_tileArray[j, i] = tile;
				}
				ushort wall = tile.wall;
				if (wall <= 0 || FullTile(j, i) || (wall == 318 && !_shouldShowInvisibleWalls) || (tile.invisibleWall() && !_shouldShowInvisibleWalls))
				{
					continue;
				}
				Color val = Lighting.GetColor(j, i);
				if (tile.fullbrightWall())
				{
					val = Color.White;
				}
				if (wall == 318)
				{
					val = Color.White;
				}
				if (TileDrawingBase.DrawOwnBlacks)
				{
					if (val.R == 0 && val.G == 0 && val.B == 0)
					{
						drawBlackHelper.DrawBlack(j, i);
						continue;
					}
				}
				else if (val.R == 0 && val.G == 0 && val.B == 0 && i < underworldLayer)
				{
					continue;
				}
				Main.instance.LoadWall(wall);
				Texture2D wallDrawTexture = GetWallDrawTexture(tile);
				Main.tileBatch.SetLayer((uint)(((tile.active() ? 1 : 0) << 21) | (tile.wallColor() << 16) | wall), 0);
				sourceRectangle.X = tile.wallFrameX();
				sourceRectangle.Y = tile.wallFrameY() + Main.wallFrame[wall] * 180;
				ushort wall2 = tile.wall;
				if ((uint)(wall2 - 242) <= 1u)
				{
					int num5 = 20;
					int num6 = (Main.wallFrameCounter[wall] + j * 11 + i * 27) % (num5 * 8);
					sourceRectangle.Y = tile.wallFrameY() + 180 * (num6 / num5);
				}
				if (Lighting.NotRetro && !Main.wallLight[wall] && tile.wall != 241 && (tile.wall < 88 || tile.wall > 93) && !WorldGen.SolidTile(tile))
				{
					if (tile.wall == 346)
					{
						vertices.TopRightColor = (vertices.TopLeftColor = (vertices.BottomRightColor = (vertices.BottomLeftColor = new Color((int)(byte)Main.DiscoR, (int)(byte)Main.DiscoG, (int)(byte)Main.DiscoB))));
					}
					else if (tile.wall == 44)
					{
						vertices.TopRightColor = (vertices.TopLeftColor = (vertices.BottomRightColor = (vertices.BottomLeftColor = new Color((int)(byte)Main.DiscoR, (int)(byte)Main.DiscoG, (int)(byte)Main.DiscoB))));
					}
					else
					{
						Lighting.GetCornerColors(j, i, out vertices);
						wall2 = tile.wall;
						if ((uint)(wall2 - 341) <= 4u)
						{
							LerpVertexColorsWithColor(ref vertices, Color.White, 0.5f);
						}
						if (tile.fullbrightWall())
						{
							vertices = _glowPaintColors;
						}
					}
					Main.tileBatch.Draw(wallDrawTexture, new Vector2((float)(j * 16 - (int)screenPosition.X - 8), (float)(i * 16 - (int)screenPosition.Y - 8)) + drawOffSet, sourceRectangle, vertices, Vector2.Zero, 1f, (SpriteEffects)0);
					if (tile.wall == 347)
					{
						Texture2D value = TextureAssets.GlowMask[361].Value;
						LiquidRenderer.SetShimmerVertexColors_Sparkle(ref vertices, 0.7f, j, i, top: true);
						Main.tileBatch.Draw(value, new Vector2((float)(j * 16 - (int)screenPosition.X - 8), (float)(i * 16 - (int)screenPosition.Y - 8)) + drawOffSet, sourceRectangle, vertices, Vector2.Zero, 1f, (SpriteEffects)0);
					}
				}
				else
				{
					Color val2 = val;
					if (wall == 44 || wall == 346)
					{
						val2 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
					}
					if ((uint)(wall - 341) <= 4u)
					{
						val2 = Color.Lerp(val2, Color.White, 0.5f);
					}
					Main.tileBatch.Draw(wallDrawTexture, new Vector2((float)(j * 16 - (int)screenPosition.X - 8), (float)(i * 16 - (int)screenPosition.Y - 8)) + drawOffSet, sourceRectangle, val2, Vector2.Zero, 1f, (SpriteEffects)0);
					if (tile.wall == 347)
					{
						Texture2D value2 = TextureAssets.GlowMask[361].Value;
						Color val3 = LiquidRenderer.GetShimmerGlitterColor(top: true, j, i) * 0.7f;
						Main.tileBatch.Draw(value2, new Vector2((float)(j * 16 - (int)screenPosition.X - 8), (float)(i * 16 - (int)screenPosition.Y - 8)) + drawOffSet, sourceRectangle, val3, Vector2.Zero, 1f, (SpriteEffects)0);
					}
				}
				if (val.R > num2 || val.G > num3 || val.B > num4)
				{
					bool num7 = _tileArray[j - 1, i].wall > 0 && wallBlend[_tileArray[j - 1, i].wall] != wallBlend[tile.wall];
					bool flag = _tileArray[j + 1, i].wall > 0 && wallBlend[_tileArray[j + 1, i].wall] != wallBlend[tile.wall];
					bool flag2 = _tileArray[j, i - 1].wall > 0 && wallBlend[_tileArray[j, i - 1].wall] != wallBlend[tile.wall];
					bool flag3 = _tileArray[j, i + 1].wall > 0 && wallBlend[_tileArray[j, i + 1].wall] != wallBlend[tile.wall];
					if (num7)
					{
						DrawOutline(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X), (float)(i * 16 - (int)screenPosition.Y)) + drawOffSet, new Rectangle(0, 0, 2, 16), val);
					}
					if (flag)
					{
						DrawOutline(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X + 14), (float)(i * 16 - (int)screenPosition.Y)) + drawOffSet, new Rectangle(14, 0, 2, 16), val);
					}
					if (flag2)
					{
						DrawOutline(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X), (float)(i * 16 - (int)screenPosition.Y)) + drawOffSet, new Rectangle(0, 0, 16, 2), val);
					}
					if (flag3)
					{
						DrawOutline(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X), (float)(i * 16 - (int)screenPosition.Y + 14)) + drawOffSet, new Rectangle(0, 14, 16, 2), val);
					}
				}
			}
		}
		drawBlackHelper.EndStrip();
		RestartLayeredBatch();
		Main.instance.DrawTileCracks(2, Main.LocalPlayer.hitReplace);
		Main.instance.DrawTileCracks(2, Main.LocalPlayer.hitTile);
	}

	public Texture2D GetWallDrawTexture(Tile tile)
	{
		return GetWallDrawTexture(tile.wall, tile.wallColor());
	}

	public Texture2D GetWallDrawTexture(int wallType, int paintColor)
	{
		TilePaintSystemV2.WallVariationKey wallVariationKey = new TilePaintSystemV2.WallVariationKey
		{
			WallType = wallType,
			PaintColor = paintColor
		};
		if (_lastPaintLookupKey == wallVariationKey)
		{
			return _lastPaintLookupTexture;
		}
		_lastPaintLookupKey = wallVariationKey;
		_lastPaintLookupTexture = LookupWallDrawTexture(wallVariationKey);
		return _lastPaintLookupTexture;
	}

	private Texture2D LookupWallDrawTexture(TilePaintSystemV2.WallVariationKey key)
	{
		if (key.PaintColor != 0)
		{
			Texture2D val = _paintSystem.TryGetWallAndRequestIfNotReady(key.WallType, key.PaintColor);
			if (val != null)
			{
				return val;
			}
		}
		return TextureAssets.Wall[key.WallType].Value;
	}

	protected bool FullTile(int x, int y)
	{
		if (_tileArray[x - 1, y] == null || _tileArray[x - 1, y].blockType() != 0 || _tileArray[x + 1, y] == null || _tileArray[x + 1, y].blockType() != 0)
		{
			return false;
		}
		Tile tile = _tileArray[x, y];
		if (tile == null)
		{
			return false;
		}
		if (tile.active())
		{
			if (Main.tileFrameImportant[tile.type] || TileID.Sets.DrawsWalls[tile.type])
			{
				return false;
			}
			if (tile.invisibleBlock() && !_shouldShowInvisibleWalls)
			{
				return false;
			}
			if (DebugOptions.ShowUnbreakableWall && tile.wall == 350)
			{
				return false;
			}
			if (tile.type == 740)
			{
				short frameX = tile.frameX;
				short frameY = tile.frameY;
				if ((frameX == 180 || frameX == 198) && frameY >= 0 && frameY <= 36)
				{
					return false;
				}
				if (frameX >= 108 && frameX <= 144 && (frameY == 18 || frameY == 36))
				{
					return false;
				}
			}
			if (Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
			{
				int frameX2 = tile.frameX;
				int frameY2 = tile.frameY;
				if (Main.tileLargeFrames[tile.type] > 0)
				{
					if (frameY2 == 18 || frameY2 == 108)
					{
						if (frameX2 >= 18 && frameX2 <= 54)
						{
							return true;
						}
						if (frameX2 >= 108 && frameX2 <= 144)
						{
							return true;
						}
					}
				}
				else
				{
					switch (frameY2)
					{
					case 0:
						if (frameX2 >= 180 && frameX2 <= 198)
						{
							return true;
						}
						break;
					case 18:
						if (frameX2 >= 18 && frameX2 <= 54)
						{
							return true;
						}
						if (frameX2 >= 108 && frameX2 <= 144)
						{
							return true;
						}
						if (frameX2 >= 180 && frameX2 <= 198)
						{
							return true;
						}
						break;
					case 36:
						if (frameX2 >= 108 && frameX2 <= 144)
						{
							return true;
						}
						if (frameX2 >= 180 && frameX2 <= 198)
						{
							return true;
						}
						break;
					case 90:
					case 91:
					case 92:
					case 93:
					case 94:
					case 95:
					case 96:
					case 97:
					case 98:
					case 99:
					case 100:
					case 101:
					case 102:
					case 103:
					case 104:
					case 105:
					case 106:
					case 107:
					case 108:
					case 109:
					case 110:
					case 111:
					case 112:
					case 113:
					case 114:
					case 115:
					case 116:
					case 117:
					case 118:
					case 119:
					case 120:
					case 121:
					case 122:
					case 123:
					case 124:
					case 125:
					case 126:
					case 127:
					case 128:
					case 129:
					case 130:
					case 131:
					case 132:
					case 133:
					case 134:
					case 135:
					case 136:
					case 137:
					case 138:
					case 139:
					case 140:
					case 141:
					case 142:
					case 143:
					case 144:
					case 145:
					case 146:
					case 147:
					case 148:
					case 149:
					case 150:
					case 151:
					case 152:
					case 153:
					case 154:
					case 155:
					case 156:
					case 157:
					case 158:
					case 159:
					case 160:
					case 161:
					case 162:
					case 163:
					case 164:
					case 165:
					case 166:
					case 167:
					case 168:
					case 169:
					case 170:
					case 171:
					case 172:
					case 173:
					case 174:
					case 175:
					case 176:
					case 177:
					case 178:
					case 179:
					case 180:
						if (frameX2 <= 54)
						{
							return true;
						}
						if (frameX2 >= 144 && frameX2 <= 216)
						{
							return true;
						}
						break;
					default:
						if (frameY2 == 198 && frameX2 >= 108 && frameX2 <= 144)
						{
							return true;
						}
						break;
					}
				}
			}
		}
		return false;
	}

	static WallDrawing()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	}
}
