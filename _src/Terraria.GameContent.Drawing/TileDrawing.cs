using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.Liquid;
using Terraria.GameContent.Tile_Entities;
using Terraria.Graphics;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Testing;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria.GameContent.Drawing;

public class TileDrawing : TileDrawingBase
{
	private enum TileCounterType
	{
		Tree,
		WindyGrass,
		MultiTileGrass,
		MultiTileVine,
		Vine,
		BiomeGrass,
		VoidLens,
		ReverseVine,
		TeleportationPylon,
		MasterTrophy,
		AnyDirectionalGrass,
		Count
	}

	private struct TileFlameData
	{
		public Texture2D flameTexture;

		public ulong flameSeed;

		public int flameCount;

		public Color flameColor;

		public int flameRangeXMin;

		public int flameRangeXMax;

		public int flameRangeYMin;

		public int flameRangeYMax;

		public float flameRangeMultX;

		public float flameRangeMultY;
	}

	public static readonly uint Layer_LiquidBehindTiles;

	public static readonly uint Layer_BehindTiles;

	public static readonly uint Layer_Tiles;

	public static readonly uint Layer_OverTiles;

	private const int MAX_SPECIALS = 9000;

	private const int MAX_SPECIALS_LEGACY = 1000;

	private const float FORCE_FOR_MIN_WIND = 0.08f;

	private const float FORCE_FOR_MAX_WIND = 1.2f;

	private int _leafFrequency = 100000;

	private int[] _specialsCount = new int[11];

	private Point[][] _specialPositions = new Point[11][];

	private Dictionary<Point, int> _displayDollTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _hatRackTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _trainingDummyTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _itemFrameTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _deadCellsDisplayJarTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _foodPlatterTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _weaponRackTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _chestPositions = new Dictionary<Point, int>();

	private int _specialTilesCount;

	private int[] _specialTileX = new int[1000];

	private int[] _specialTileY = new int[1000];

	private UnifiedRandom _rand;

	private double _treeWindCounter;

	private double _grassWindCounter;

	private double _sunflowerWindCounter;

	private double _vineWindCounter;

	private WindGrid _windGrid = new WindGrid();

	private bool _shouldShowInvisibleBlocks;

	private bool _shouldShowInvisibleBlocks_LastFrame;

	private List<Point> _vineRootsPositions = new List<Point>();

	private List<Point> _reverseVineRootsPositions = new List<Point>();

	private TilePaintSystemV2 _paintSystem;

	private INatureRenderer _natureRenderer = new NextNatureRenderer();

	private Color _martianGlow = new Color(0, 0, 0, 0);

	private Color _meteorGlow = new Color(100, 100, 100, 0);

	private Color _lavaMossGlow = new Color(150, 100, 50, 0);

	private Color _kryptonMossGlow = new Color(0, 200, 0, 0);

	private Color _xenonMossGlow = new Color(0, 180, 250, 0);

	private Color _argonMossGlow = new Color(225, 0, 125, 0);

	private Color _violetMossGlow = new Color(150, 0, 250, 0);

	private bool _isActiveAndNotPaused;

	private Player _perspectivePlayer = new Player();

	private Color _highQualityLightingRequirement;

	private Color _mediumQualityLightingRequirement;

	private static readonly Vector2 _zero;

	private DrawBlackHelper drawBlackHelper;

	private static float[] noise;

	private TilePaintSystemV2.TileVariationkey _lastPaintLookupKey;

	private Texture2D _lastPaintLookupTexture;

	private Vector3[] _glowPaintColorSlices = new Vector3[9]
	{
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One
	};

	private List<DrawData> _voidLensData = new List<DrawData>();

	private bool[] _tileSolid => Main.tileSolid;

	private bool[] _tileSolidTop => Main.tileSolidTop;

	private Dust[] _dust => Main.dust;

	private Gore[] _gore => Main.gore;

	private void AddSpecialPoint(int x, int y, TileCounterType type)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		_specialPositions[(int)type][_specialsCount[(int)type]++] = new Point(x, y);
	}

	public TileDrawing(TilePaintSystemV2 paintSystem)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		_paintSystem = paintSystem;
		_rand = new UnifiedRandom();
		for (int i = 0; i < _specialPositions.Length; i++)
		{
			_specialPositions[i] = new Point[9000];
		}
	}

	public void PreparePaintForTilesOnScreen()
	{
		if (Main.GameUpdateCount % 6 == 0)
		{
			GetScreenDrawArea(!Main.drawToScreen, out var _, out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
			PrepareForAreaDrawing(firstTileX, lastTileX, firstTileY, lastTileY, prepareLazily: true);
		}
	}

	public void PrepareForAreaDrawing(int firstTileX, int lastTileX, int firstTileY, int lastTileY, bool prepareLazily)
	{
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		TilePaintSystemV2.TileVariationkey lookupKey = default;
		TilePaintSystemV2.WallVariationKey lookupKey2 = default;
		for (int i = firstTileY; i < lastTileY + 4; i++)
		{
			for (int j = firstTileX - 2; j < lastTileX + 2; j++)
			{
				Tile tile = Main.tile[j, i];
				if (tile == null)
				{
					continue;
				}
				if (tile.active())
				{
					Main.instance.LoadTiles(tile.type);
					lookupKey.TileType = tile.type;
					lookupKey.PaintColor = tile.color();
					int tileStyle = 0;
					switch (tile.type)
					{
					case 5:
						tileStyle = GetTreeBiome(j, i, tile.frameX, tile.frameY);
						break;
					case 323:
						tileStyle = GetPalmTreeBiome(j, i);
						break;
					}
					lookupKey.TileStyle = tileStyle;
					if (lookupKey.PaintColor != 0)
					{
						_paintSystem.RequestTile(ref lookupKey);
					}
				}
				if (tile.wall != 0)
				{
					Main.instance.LoadWall(tile.wall);
					lookupKey2.WallType = tile.wall;
					lookupKey2.PaintColor = tile.wallColor();
					if (lookupKey2.PaintColor != 0)
					{
						_paintSystem.RequestWall(ref lookupKey2);
					}
				}
				if (!prepareLazily)
				{
					MakeExtraPreparations(tile, j, i);
				}
			}
		}
		TimeLogger.FindPaintedTiles.AddTime(fromTimestamp);
	}

	private void MakeExtraPreparations(Tile tile, int x, int y)
	{
		switch (tile.type)
		{
		case 5:
		{
			int treeFrame2 = 0;
			int floorY2 = 0;
			int topTextureFrameWidth2 = 0;
			int topTextureFrameHeight2 = 0;
			int treeStyle2 = 0;
			int xoffset2 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
			if (WorldGen.GetCommonTreeFoliageData(x, y, xoffset2, ref treeFrame2, ref treeStyle2, out floorY2, out topTextureFrameWidth2, out topTextureFrameHeight2))
			{
				TilePaintSystemV2.TreeFoliageVariantKey lookupKey3 = new TilePaintSystemV2.TreeFoliageVariantKey
				{
					TextureIndex = treeStyle2,
					PaintColor = tile.color()
				};
				_paintSystem.RequestTreeTop(ref lookupKey3);
				_paintSystem.RequestTreeBranch(ref lookupKey3);
			}
			break;
		}
		case 583:
		case 584:
		case 585:
		case 586:
		case 587:
		case 588:
		case 589:
		{
			int treeFrame3 = 0;
			int floorY3 = 0;
			int topTextureFrameWidth3 = 0;
			int topTextureFrameHeight3 = 0;
			int treeStyle3 = 0;
			int xoffset3 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
			if (WorldGen.GetGemTreeFoliageData(x, y, xoffset3, ref treeFrame3, ref treeStyle3, out floorY3, out topTextureFrameWidth3, out topTextureFrameHeight3))
			{
				TilePaintSystemV2.TreeFoliageVariantKey lookupKey4 = new TilePaintSystemV2.TreeFoliageVariantKey
				{
					TextureIndex = treeStyle3,
					PaintColor = tile.color()
				};
				_paintSystem.RequestTreeTop(ref lookupKey4);
				_paintSystem.RequestTreeBranch(ref lookupKey4);
			}
			break;
		}
		case 596:
		case 616:
		{
			int treeFrame = 0;
			int floorY = 0;
			int topTextureFrameWidth = 0;
			int topTextureFrameHeight = 0;
			int treeStyle = 0;
			int xoffset = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
			if (WorldGen.GetVanityTreeFoliageData(x, y, xoffset, ref treeFrame, ref treeStyle, out floorY, out topTextureFrameWidth, out topTextureFrameHeight))
			{
				TilePaintSystemV2.TreeFoliageVariantKey lookupKey2 = new TilePaintSystemV2.TreeFoliageVariantKey
				{
					TextureIndex = treeStyle,
					PaintColor = tile.color()
				};
				_paintSystem.RequestTreeTop(ref lookupKey2);
				_paintSystem.RequestTreeBranch(ref lookupKey2);
			}
			break;
		}
		case 634:
		{
			int treeFrame4 = 0;
			int floorY4 = 0;
			int topTextureFrameWidth4 = 0;
			int topTextureFrameHeight4 = 0;
			int treeStyle4 = 0;
			int xoffset4 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
			if (WorldGen.GetAshTreeFoliageData(x, y, xoffset4, ref treeFrame4, ref treeStyle4, out floorY4, out topTextureFrameWidth4, out topTextureFrameHeight4))
			{
				TilePaintSystemV2.TreeFoliageVariantKey lookupKey5 = new TilePaintSystemV2.TreeFoliageVariantKey
				{
					TextureIndex = treeStyle4,
					PaintColor = tile.color()
				};
				_paintSystem.RequestTreeTop(ref lookupKey5);
				_paintSystem.RequestTreeBranch(ref lookupKey5);
			}
			break;
		}
		case 323:
		{
			int textureIndex = 15;
			if (x >= WorldGen.beachDistance && x <= Main.maxTilesX - WorldGen.beachDistance)
			{
				textureIndex = 21;
			}
			TilePaintSystemV2.TreeFoliageVariantKey lookupKey = new TilePaintSystemV2.TreeFoliageVariantKey
			{
				TextureIndex = textureIndex,
				PaintColor = tile.color()
			};
			_paintSystem.RequestTreeTop(ref lookupKey);
			_paintSystem.RequestTreeBranch(ref lookupKey);
			break;
		}
		}
	}

	public void Update()
	{
		if (!Main.dedServ)
		{
			double num = Math.Abs(Main.WindForVisuals);
			num = Utils.GetLerpValue(0.08f, 1.2f, (float)num, clamped: true);
			_treeWindCounter += 1.0 / 240.0 + 1.0 / 240.0 * num * 2.0;
			_grassWindCounter += 1.0 / 180.0 + 1.0 / 180.0 * num * 4.0;
			_sunflowerWindCounter += 1.0 / 420.0 + 1.0 / 420.0 * num * 5.0;
			_vineWindCounter += 1.0 / 120.0 + 1.0 / 120.0 * num * 0.4000000059604645;
			UpdateLeafFrequency();
			EnsureWindGridSize();
			_windGrid.Update();
			_shouldShowInvisibleBlocks = Main.ShouldShowInvisibleBlocksAndWalls();
			if (_shouldShowInvisibleBlocks_LastFrame != _shouldShowInvisibleBlocks)
			{
				_shouldShowInvisibleBlocks_LastFrame = _shouldShowInvisibleBlocks;
				Main.sectionManager.SetAllFramedSectionsAsNeedingRefresh();
			}
		}
	}

	public void ClearSpecialBlockCounts()
	{
		_vineRootsPositions.Clear();
		_reverseVineRootsPositions.Clear();
		_specialsCount[3] = 0;
		_specialsCount[2] = 0;
		_specialsCount[6] = 0;
		_specialsCount[4] = 0;
		_specialsCount[1] = 0;
		_specialsCount[10] = 0;
		_specialsCount[0] = 0;
		_specialsCount[7] = 0;
		_specialsCount[8] = 0;
		_specialsCount[9] = 0;
	}

	private void DrawNature(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, SideFlags seams = SideFlags.None)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_natureRenderer.DrawNature(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth, seams);
	}

	private void DrawNatureGlowmask(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_natureRenderer.DrawGlowmask(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
	}

	public void PostDrawTiles(bool solidLayer)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (!solidLayer)
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			SpriteBatchBeginner beginner = new SpriteBatchBeginner((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
			beginner.Begin(Main.spriteBatch);
			DrawMultiTileVines();
			DrawMultiTileGrass();
			DrawVoidLenses();
			DrawTeleportationPylons();
			DrawMasterTrophies();
			DrawGrass();
			DrawAnyDirectionalGrass();
			DrawTrees();
			DrawVines();
			DrawReverseVines();
			Main.spriteBatch.End();
			TimeLogger.TileExtras.AddTime(fromTimestamp);
			_natureRenderer.DrawAfterAllObjects(beginner);
		}
		if (solidLayer)
		{
			TimeLogger.StartTimestamp fromTimestamp2 = TimeLogger.Start();
			DrawEntities_HatRacks();
			DrawEntities_DisplayDolls();
			TimeLogger.ClothingRacks.AddTime(fromTimestamp2);
		}
	}

	public void DrawLiquidBehindTiles(int waterStyleOverride = -1)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		Main.tileBatch.Restart();
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		GetScreenDrawArea(!Main.drawToScreen, out var drawOffSet, out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
		for (int i = firstTileY; i < lastTileY + 4; i++)
		{
			for (int j = firstTileX - 2; j < lastTileX + 2; j++)
			{
				Tile tile = Main.tile[j, i];
				if (tile != null)
				{
					Main.tileBatch.SetLayer(0u, 0);
					DrawTile_LiquidBehindTile(solidLayer: false, waterStyleOverride, unscaledPosition, drawOffSet, j, i, tile);
				}
			}
		}
		int value = Main.tileBatch.End();
		TimeLogger.LiquidBackgroundDrawCalls.Add(value);
	}

	public void Draw(bool solidLayer, bool intoRenderTargets, int waterStyleOverride = -1)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Unknown result type (might be due to invalid IL or missing references)
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		FlushLogData = (solidLayer ? TimeLogger.FlushSolidTiles : TimeLogger.FlushNonSolidTiles);
		DrawCallLogData = (solidLayer ? TimeLogger.SolidDrawCalls : TimeLogger.NonSolidDrawCalls);
		_isActiveAndNotPaused = FocusHelper.AllowTileDrawingToEmitEffects;
		_perspectivePlayer = Main.SceneMetrics.PerspectivePlayer;
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		if (!solidLayer)
		{
			Main.critterCage = false;
		}
		EnsureWindGridSize();
		ClearLegacyCachedDraws();
		ClearCachedTileDraws(solidLayer);
		float num = 255f * (1f - Main.gfxQuality) + 30f * Main.gfxQuality;
		_highQualityLightingRequirement.R = (byte)num;
		_highQualityLightingRequirement.G = (byte)((double)num * 1.1);
		_highQualityLightingRequirement.B = (byte)((double)num * 1.2);
		float num2 = 50f * (1f - Main.gfxQuality) + 2f * Main.gfxQuality;
		_mediumQualityLightingRequirement.R = (byte)num2;
		_mediumQualityLightingRequirement.G = (byte)((double)num2 * 1.1);
		_mediumQualityLightingRequirement.B = (byte)((double)num2 * 1.2);
		if (DebugOptions.devLightTilesCheat)
		{
			_highQualityLightingRequirement.R = byte.MaxValue;
			_highQualityLightingRequirement.G = byte.MaxValue;
			_highQualityLightingRequirement.B = byte.MaxValue;
			_mediumQualityLightingRequirement.R = byte.MaxValue;
			_mediumQualityLightingRequirement.G = byte.MaxValue;
			_mediumQualityLightingRequirement.B = byte.MaxValue;
		}
		GetScreenDrawArea(!Main.drawToScreen, out var drawOffSet, out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
		drawBlackHelper = new DrawBlackHelper(Layer_Tiles, drawOffSet);
		byte b = (byte)(100f + 150f * Main.martianLight);
		_martianGlow = new Color((int)b, (int)b, (int)b, 0);
		_lastPaintLookupKey = new TilePaintSystemV2.TileVariationkey
		{
			TileType = -1
		};
		for (int i = firstTileY; i < lastTileY + 4; i++)
		{
			for (int j = firstTileX - 2; j < lastTileX + 2; j++)
			{
				Tile tile = Main.tile[j, i];
				if (tile == null)
				{
					tile = new Tile();
					Main.tile[j, i] = tile;
					Main.mapTime += 60;
				}
				else
				{
					if (!tile.active() || IsTileDrawLayerSolid(tile.type) != solidLayer || (DebugOptions.ShowUnbreakableWall && tile.wall == 350))
					{
						continue;
					}
					if (solidLayer)
					{
						Main.tileBatch.SetLayer(Layer_LiquidBehindTiles, 0);
						DrawTile_LiquidBehindTile(solidLayer, waterStyleOverride, unscaledPosition, drawOffSet, j, i, tile);
					}
					Main.tileBatch.SetLayer(Layer_Tiles, 0);
					ushort type = tile.type;
					short frameX = tile.frameX;
					short frameY = tile.frameY;
					if (!TextureAssets.Tile[type].IsLoaded)
					{
						Main.instance.LoadTiles(type);
					}
					switch (type)
					{
					case 52:
					case 62:
					case 115:
					case 205:
					case 382:
					case 528:
					case 636:
					case 638:
						CrawlToTopOfVineAndAddSpecialPoint(i, j);
						continue;
					case 549:
						CrawlToBottomOfReverseVineAndAddSpecialPoint(i, j);
						continue;
					case 34:
						if (frameX % 54 == 0 && frameY % 54 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
						}
						continue;
					case 698:
						if (frameX % 18 == 0 && frameY == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
						}
						continue;
					case 454:
						if (frameX % 72 == 0 && frameY % 54 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
						}
						continue;
					case 42:
					case 270:
					case 271:
					case 572:
					case 581:
					case 660:
						if (frameX % 18 == 0 && frameY % 36 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
						}
						continue;
					case 91:
						if (frameX % 18 == 0 && frameY % 54 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
						}
						continue;
					case 95:
					case 126:
					case 444:
						if (frameX % 36 == 0 && frameY % 36 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
						}
						continue;
					case 465:
					case 591:
					case 592:
						if (frameX % 36 == 0 && frameY % 54 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
						}
						continue;
					case 27:
						if (frameX % 36 == 0 && frameY == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
						}
						continue;
					case 236:
					case 238:
					case 702:
						if (frameX % 36 == 0 && frameY == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
						}
						continue;
					case 233:
						if (frameY == 0 && frameX % 54 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
						}
						if (frameY == 36 && frameX % 36 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
						}
						continue;
					case 652:
						if (frameX % 36 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
						}
						continue;
					case 651:
						if (frameX % 54 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
						}
						continue;
					case 530:
						if (frameX < 270)
						{
							if (frameX % 54 == 0 && frameY == 0)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						}
						break;
					case 705:
						if (frameX % 486 < 270)
						{
							if (frameX % 54 == 0 && frameY % 36 == 0)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						}
						break;
					case 485:
					case 489:
					case 490:
						if (frameY == 0 && frameX % 36 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
						}
						continue;
					case 521:
					case 522:
					case 523:
					case 524:
					case 525:
					case 526:
					case 527:
						if (frameY == 0 && frameX % 36 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
						}
						continue;
					case 493:
						if (frameY == 0 && frameX % 18 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
						}
						continue;
					case 519:
						if (frameX / 18 <= 4)
						{
							AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
						}
						continue;
					case 373:
					case 374:
					case 375:
					case 461:
					case 709:
						EmitLiquidDrops(i, j, tile, type);
						continue;
					case 491:
						if (frameX == 18 && frameY == 18)
						{
							AddSpecialPoint(j, i, TileCounterType.VoidLens);
						}
						break;
					case 597:
						if (frameX % 54 == 0 && frameY == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.TeleportationPylon);
						}
						break;
					case 617:
						if (frameX % 54 == 0 && frameY % 72 == 0)
						{
							AddSpecialPoint(j, i, TileCounterType.MasterTrophy);
						}
						break;
					case 184:
						AddSpecialPoint(j, i, TileCounterType.AnyDirectionalGrass);
						continue;
					default:
						if (ShouldSwayInWind(j, i, tile))
						{
							AddSpecialPoint(j, i, TileCounterType.WindyGrass);
							continue;
						}
						break;
					}
					DrawSingleTile(unscaledPosition, drawOffSet, j, i);
				}
			}
		}
		drawBlackHelper.EndStrip();
		RestartLayeredBatch();
		if (solidLayer)
		{
			Main.instance.DrawTileCracks(1, Main.player[Main.myPlayer].hitReplace);
			Main.instance.DrawTileCracks(1, Main.player[Main.myPlayer].hitTile);
			RestartSpriteBatch();
		}
		DrawSpecialTilesLegacy(unscaledPosition, drawOffSet);
		if (TileObject.objectPreview.Active && Main.LocalPlayer.cursorItemIconEnabled && Main.placementPreview && !CaptureManager.Instance.Active)
		{
			Main.instance.LoadTiles(TileObject.objectPreview.Type);
			float placementPreviewOpacity = Main.LocalPlayer.GetPlacementPreviewOpacity();
			TileObject.DrawPreview(Main.spriteBatch, TileObject.objectPreview, unscaledPosition - drawOffSet, placementPreviewOpacity);
		}
	}

	private void CrawlToTopOfVineAndAddSpecialPoint(int j, int i)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		int num = j;
		for (int num2 = j - 1; num2 > 0; num2--)
		{
			Tile tile = Main.tile[i, num2];
			if (WorldGen.BottomEdgeCanBeAttachedTo(i, num2) || !tile.active())
			{
				num = num2 + 1;
				break;
			}
		}
		Point item = new Point(i, num);
		if (!_vineRootsPositions.Contains(item))
		{
			_vineRootsPositions.Add(item);
			AddSpecialPoint(i, num, TileCounterType.Vine);
		}
	}

	private void CrawlToBottomOfReverseVineAndAddSpecialPoint(int j, int i)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		int num = j;
		for (int k = j; k < Main.maxTilesY; k++)
		{
			Tile tile = Main.tile[i, k];
			if (WorldGen.TopEdgeCanBeAttachedTo(i, k) || !tile.active())
			{
				num = k - 1;
				break;
			}
		}
		Point item = new Point(i, num);
		if (!_reverseVineRootsPositions.Contains(item))
		{
			_reverseVineRootsPositions.Add(item);
			AddSpecialPoint(i, num, TileCounterType.ReverseVine);
		}
	}

	private static float SmoothStep(float x)
	{
		return x * x * (3f - 2f * x);
	}

	static TileDrawing()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Layer_LiquidBehindTiles = 0u;
		Layer_BehindTiles = 1u;
		Layer_Tiles = 2u;
		Layer_OverTiles = 3u;
		_zero = default;
		noise = new float[256];
		Random random = new Random(0);
		for (int i = 0; i < noise.Length; i++)
		{
			noise[i] = (float)(random.NextDouble() * 2.0 - 1.0);
		}
	}

	private static float LinearNoise(float x)
	{
		int num = (int)x;
		if (x < 0f)
		{
			num--;
		}
		int num2 = num + 1;
		float num3 = x - (float)num;
		float num4 = noise[num & 0xFF];
		float num5 = noise[num2 & 0xFF];
		float num6 = num3;
		return num4 * (1f - num6) + num5 * num6;
	}

	private static uint Hash(uint x)
	{
		x ^= x >> 16;
		x *= 2146121005;
		x ^= x >> 15;
		x *= 2221713035u;
		x ^= x >> 16;
		return x;
	}

	private static uint Hash2(uint x)
	{
		x ^= x >> 15;
		x *= 3513297581u;
		x ^= x >> 15;
		x *= 2943497623u;
		x ^= x >> 15;
		return x;
	}

	private static float DistToWanderingCircle(Point pos, int gridSize, float wanderDist, float cyclesPerTick, uint seed)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		Point val = new Point(pos.X / gridSize, pos.Y / gridSize);
		Vector2 val2 = new Vector2(((float)val.X + 0.5f) * (float)gridSize, ((float)val.Y + 0.5f) * (float)gridSize);
		float num = (float)Main.timeForVisualEffects;
		uint num2 = (Hash((uint)val.X) ^ Hash2((uint)val.Y) ^ seed) & 0xFFFFFF;
		uint num3 = (Hash2((uint)val.X) ^ Hash((uint)val.Y) ^ seed) & 0xFFFFFF;
		val2.X += LinearNoise(((float)num2 + num) * cyclesPerTick) * wanderDist;
		val2.Y += LinearNoise(((float)num3 + num) * cyclesPerTick) * wanderDist;
		return Vector2.Distance(pos.ToVector2(), val2);
	}

	private static float LavaLightA(int tileX, int tileY)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		float val = DistToWanderingCircle(new Point(tileX, tileY), 7, 2f, 0.025f, 2221713035u);
		float val2 = DistToWanderingCircle(new Point(tileX + 3, tileY), 7, 2f, 0.025f, 657044585u);
		float val3 = DistToWanderingCircle(new Point(tileX, tileY + 3), 7, 2f, 0.025f, 741521833u);
		float t = Math.Min(val2: Math.Min(val3, DistToWanderingCircle(new Point(tileX + 3, tileY + 3), 7, 2f, 0.025f, 56936621u)), val1: Math.Min(val, val2));
		t = SmoothStep(1f - Utils.GetLerpValue(0f, 3.5f, t, clamped: true));
		float toMin = 0f;
		if (!WorldGen.SolidTile(tileX, tileY - 1))
		{
			toMin = 0.8f;
		}
		return Utils.Remap(t, 0.7f, 1f, toMin, 1f);
	}

	private void DrawSingleTile(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1707: Unknown result type (might be due to invalid IL or missing references)
		//IL_1717: Unknown result type (might be due to invalid IL or missing references)
		//IL_171c: Unknown result type (might be due to invalid IL or missing references)
		//IL_171d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1722: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_160e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1613: Unknown result type (might be due to invalid IL or missing references)
		//IL_162b: Unknown result type (might be due to invalid IL or missing references)
		//IL_164d: Unknown result type (might be due to invalid IL or missing references)
		//IL_165d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1662: Unknown result type (might be due to invalid IL or missing references)
		//IL_1663: Unknown result type (might be due to invalid IL or missing references)
		//IL_1668: Unknown result type (might be due to invalid IL or missing references)
		//IL_1675: Unknown result type (might be due to invalid IL or missing references)
		//IL_1677: Unknown result type (might be due to invalid IL or missing references)
		//IL_167a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1687: Unknown result type (might be due to invalid IL or missing references)
		//IL_1691: Unknown result type (might be due to invalid IL or missing references)
		//IL_169c: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_16bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1751: Unknown result type (might be due to invalid IL or missing references)
		//IL_1754: Unknown result type (might be due to invalid IL or missing references)
		//IL_175a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1764: Unknown result type (might be due to invalid IL or missing references)
		//IL_176f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1732: Unknown result type (might be due to invalid IL or missing references)
		//IL_1734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_103a: Unknown result type (might be due to invalid IL or missing references)
		//IL_103f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1061: Unknown result type (might be due to invalid IL or missing references)
		//IL_1066: Unknown result type (might be due to invalid IL or missing references)
		//IL_1047: Unknown result type (might be due to invalid IL or missing references)
		//IL_104c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1054: Unknown result type (might be due to invalid IL or missing references)
		//IL_1059: Unknown result type (might be due to invalid IL or missing references)
		//IL_106e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Unknown result type (might be due to invalid IL or missing references)
		//IL_108b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1187: Unknown result type (might be due to invalid IL or missing references)
		//IL_118c: Unknown result type (might be due to invalid IL or missing references)
		//IL_119e: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_102a: Unknown result type (might be due to invalid IL or missing references)
		//IL_102f: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1354: Unknown result type (might be due to invalid IL or missing references)
		//IL_137c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1381: Unknown result type (might be due to invalid IL or missing references)
		//IL_1388: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1205: Unknown result type (might be due to invalid IL or missing references)
		//IL_120a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1211: Unknown result type (might be due to invalid IL or missing references)
		//IL_1247: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1413: Unknown result type (might be due to invalid IL or missing references)
		//IL_1418: Unknown result type (might be due to invalid IL or missing references)
		//IL_141f: Unknown result type (might be due to invalid IL or missing references)
		//IL_142a: Unknown result type (might be due to invalid IL or missing references)
		//IL_128e: Unknown result type (might be due to invalid IL or missing references)
		//IL_129a: Unknown result type (might be due to invalid IL or missing references)
		//IL_129f: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_186d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1872: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1306: Unknown result type (might be due to invalid IL or missing references)
		//IL_130b: Unknown result type (might be due to invalid IL or missing references)
		//IL_130f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1314: Unknown result type (might be due to invalid IL or missing references)
		//IL_131e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1992: Unknown result type (might be due to invalid IL or missing references)
		//IL_1997: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14de: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_150c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1511: Unknown result type (might be due to invalid IL or missing references)
		//IL_1518: Unknown result type (might be due to invalid IL or missing references)
		//IL_1523: Unknown result type (might be due to invalid IL or missing references)
		//IL_1552: Unknown result type (might be due to invalid IL or missing references)
		//IL_155c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1561: Unknown result type (might be due to invalid IL or missing references)
		//IL_1586: Unknown result type (might be due to invalid IL or missing references)
		//IL_158b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1592: Unknown result type (might be due to invalid IL or missing references)
		//IL_159d: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18da: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a14: Unknown result type (might be due to invalid IL or missing references)
		TileDrawInfo tileDrawInfo = new TileDrawInfo();
		tileDrawInfo.tileCache = Main.tile[tileX, tileY];
		tileDrawInfo.typeCache = tileDrawInfo.tileCache.type;
		tileDrawInfo.tileFrameX = tileDrawInfo.tileCache.frameX;
		tileDrawInfo.tileFrameY = tileDrawInfo.tileCache.frameY;
		tileDrawInfo.tileLight = Lighting.GetColor(tileX, tileY);
		if (tileDrawInfo.tileCache.liquid > 0 && tileDrawInfo.tileCache.type == 518)
		{
			return;
		}
		GetTileDrawData(tileX, tileY, tileDrawInfo.tileCache, tileDrawInfo.typeCache, ref tileDrawInfo.tileFrameX, ref tileDrawInfo.tileFrameY, out tileDrawInfo.tileWidth, out tileDrawInfo.tileHeight, out tileDrawInfo.tileTop, out tileDrawInfo.halfBrickHeight, out tileDrawInfo.addFrX, out tileDrawInfo.addFrY, out tileDrawInfo.tileSpriteEffect, out tileDrawInfo.glowTexture, out tileDrawInfo.glowSourceRect, out tileDrawInfo.glowColor);
		if (tileDrawInfo.tileTop < 0)
		{
			Main.tileBatch.SetLayer(Layer_OverTiles, 0);
		}
		else if (tileDrawInfo.tileTop + tileDrawInfo.tileHeight <= 16)
		{
			Main.tileBatch.SetLayer(Layer_Tiles, 0);
		}
		else
		{
			Main.tileBatch.SetLayer(Layer_BehindTiles, 0);
		}
		tileDrawInfo.drawTexture = GetTileDrawTexture(tileDrawInfo.tileCache, tileX, tileY);
		Texture2D highlightTexture = null;
		Rectangle empty = Rectangle.Empty;
		Color highlightColor = Color.Transparent;
		if (TileID.Sets.HasOutlines[tileDrawInfo.typeCache])
		{
			GetTileOutlineInfo(tileX, tileY, tileDrawInfo.typeCache, ref tileDrawInfo.tileLight, ref highlightTexture, ref highlightColor);
		}
		if (_perspectivePlayer.dangerSense && IsTileDangerous(_perspectivePlayer, tileX, tileY, tileDrawInfo.tileCache, tileDrawInfo.typeCache))
		{
			if (tileDrawInfo.tileLight.R < byte.MaxValue)
			{
				tileDrawInfo.tileLight.R = byte.MaxValue;
			}
			if (tileDrawInfo.tileLight.G < 50)
			{
				tileDrawInfo.tileLight.G = 50;
			}
			if (tileDrawInfo.tileLight.B < 50)
			{
				tileDrawInfo.tileLight.B = 50;
			}
			if (_isActiveAndNotPaused && _rand.Next(30) == 0)
			{
				int num = Dust.NewDust(new Vector2((float)(tileX * 16), (float)(tileY * 16)), 16, 16, 60, 0f, 0f, 100, default, 0.3f);
				_dust[num].fadeIn = 1f;
				Dust obj = _dust[num];
				obj.velocity *= 0.1f;
				_dust[num].noLight = true;
				_dust[num].noGravity = true;
			}
		}
		if (_perspectivePlayer.findTreasure && Main.IsTileSpelunkable(tileDrawInfo.typeCache, tileDrawInfo.tileFrameX, tileDrawInfo.tileFrameY))
		{
			if (tileDrawInfo.tileLight.R < 200)
			{
				tileDrawInfo.tileLight.R = 200;
			}
			if (tileDrawInfo.tileLight.G < 170)
			{
				tileDrawInfo.tileLight.G = 170;
			}
			if (_isActiveAndNotPaused && _rand.Next(60) == 0)
			{
				int num2 = Dust.NewDust(new Vector2((float)(tileX * 16), (float)(tileY * 16)), 16, 16, 204, 0f, 0f, 150, default, 0.3f);
				_dust[num2].fadeIn = 1f;
				Dust obj2 = _dust[num2];
				obj2.velocity *= 0.1f;
				_dust[num2].noLight = true;
			}
		}
		if (_perspectivePlayer.biomeSight)
		{
			Color sightColor = Color.White;
			if (Main.IsTileBiomeSightable(tileDrawInfo.typeCache, tileDrawInfo.tileFrameX, tileDrawInfo.tileFrameY, ref sightColor))
			{
				if (tileDrawInfo.tileLight.R < sightColor.R)
				{
					tileDrawInfo.tileLight.R = sightColor.R;
				}
				if (tileDrawInfo.tileLight.G < sightColor.G)
				{
					tileDrawInfo.tileLight.G = sightColor.G;
				}
				if (tileDrawInfo.tileLight.B < sightColor.B)
				{
					tileDrawInfo.tileLight.B = sightColor.B;
				}
				if (_isActiveAndNotPaused && _rand.Next(480) == 0)
				{
					Color newColor = sightColor;
					int num3 = Dust.NewDust(new Vector2((float)(tileX * 16), (float)(tileY * 16)), 16, 16, 267, 0f, 0f, 150, newColor, 0.3f);
					_dust[num3].noGravity = true;
					_dust[num3].fadeIn = 1f;
					Dust obj3 = _dust[num3];
					obj3.velocity *= 0.1f;
					_dust[num3].noLightEmittance = true;
				}
			}
		}
		if (_isActiveAndNotPaused && (!Lighting.UpdateEveryFrame || new FastRandom(Main.TileFrameSeed).WithModifier(tileX, tileY).Next(4) == 0))
		{
			DrawTiles_EmitParticles(tileY, tileX, tileDrawInfo.tileCache, tileDrawInfo.typeCache, tileDrawInfo.tileFrameX, tileDrawInfo.tileFrameY, tileDrawInfo.tileLight);
		}
		tileDrawInfo.tileLight = DrawTiles_GetLightOverride(tileY, tileX, tileDrawInfo.tileCache, tileDrawInfo.typeCache, tileDrawInfo.tileFrameX, tileDrawInfo.tileFrameY, tileDrawInfo.tileLight);
		bool flag = false;
		if (tileDrawInfo.glowTexture != null || Main.tileGlowMask[tileDrawInfo.typeCache] != -1 || Main.tileFlame[tileDrawInfo.typeCache])
		{
			flag = true;
		}
		if (tileDrawInfo.tileLight.R >= 1 || tileDrawInfo.tileLight.G >= 1 || tileDrawInfo.tileLight.B >= 1 || TileID.Sets.IgnoreDrawLightConditions[tileDrawInfo.typeCache])
		{
			flag = true;
		}
		if (tileDrawInfo.tileCache.wall > 0 && (tileDrawInfo.tileCache.wall == 318 || tileDrawInfo.tileCache.fullbrightWall()))
		{
			flag = true;
		}
		bool flag2 = IsVisible(tileDrawInfo.tileCache);
		if (!flag2)
		{
			flag = false;
		}
		if ((!flag & flag2) && TileDrawingBase.DrawOwnBlacks)
		{
			drawBlackHelper.DrawBlack(tileX, tileY);
		}
		CacheSpecialDraws_Part1(tileX, tileY, tileDrawInfo.typeCache, tileDrawInfo.tileFrameX, tileDrawInfo.tileFrameY, !flag);
		CacheSpecialDraws_Part2(tileX, tileY, tileDrawInfo);
		if (tileDrawInfo.typeCache == 72 && tileDrawInfo.tileFrameX >= 36)
		{
			int num4 = 0;
			if (tileDrawInfo.tileFrameY == 18)
			{
				num4 = 1;
			}
			else if (tileDrawInfo.tileFrameY == 36)
			{
				num4 = 2;
			}
			Main.tileBatch.Draw(TextureAssets.ShroomCap.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X - 22), (float)(tileY * 16 - (int)screenPosition.Y - 26)) + screenOffset, new Rectangle(num4 * 62, 0, 60, 42), Lighting.GetColor(tileX, tileY), _zero, 1f, tileDrawInfo.tileSpriteEffect);
		}
		Rectangle val = new Rectangle(tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, tileDrawInfo.tileFrameY + tileDrawInfo.addFrY, tileDrawInfo.tileWidth, tileDrawInfo.tileHeight - tileDrawInfo.halfBrickHeight);
		float num5 = ((float)tileDrawInfo.tileWidth - 16f) / 2f;
		if (tileDrawInfo.typeCache >= 0 && TileID.Sets.DoNotAdjustDrawPositionBasedOnTileWidth[tileDrawInfo.typeCache])
		{
			num5 = 0f;
		}
		Vector2 val2 = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - num5, (float)(tileY * 16 - (int)screenPosition.Y + tileDrawInfo.tileTop + tileDrawInfo.halfBrickHeight)) + screenOffset;
		if (!flag)
		{
			return;
		}
		tileDrawInfo.colorTint = Color.White;
		tileDrawInfo.finalColor = GetFinalLight(tileDrawInfo.tileCache, tileDrawInfo.typeCache, tileDrawInfo.tileLight, tileDrawInfo.colorTint);
		switch (tileDrawInfo.typeCache)
		{
		case 751:
			if (tileDrawInfo.tileFrameX != 0 || tileDrawInfo.tileCache.frameY != 0)
			{
				return;
			}
			val2.X += 11f;
			val2.Y -= 8f;
			break;
		case 752:
			if (tileDrawInfo.tileFrameX != 0 || tileDrawInfo.tileFrameY != 0)
			{
				return;
			}
			val2.X += 8f;
			break;
		case 136:
			switch (tileDrawInfo.tileFrameX / 18)
			{
			case 1:
				val2.X += -2f;
				break;
			case 2:
				val2.X += 2f;
				break;
			}
			break;
		case 442:
		{
			int num7 = tileDrawInfo.tileFrameX / 22;
			if (num7 == 3)
			{
				val2.X += 2f;
			}
			break;
		}
		case 726:
			val2.X -= 2f;
			switch (tileDrawInfo.tileCache.blockType())
			{
			case 3:
				val2.X -= 6f;
				val2.Y += 2f;
				break;
			case 2:
				val2.X += 6f;
				val2.Y += 2f;
				break;
			case 5:
				val2.X -= 6f;
				break;
			case 4:
				val2.X += 6f;
				break;
			}
			break;
		case 51:
		case 697:
			tileDrawInfo.finalColor = tileDrawInfo.tileLight * 0.5f;
			break;
		case 160:
		case 692:
		{
			Color val3 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255);
			if (tileDrawInfo.tileCache.inActive())
			{
				val3 = tileDrawInfo.tileCache.actColor(val3);
			}
			tileDrawInfo.finalColor = val3;
			break;
		}
		case 129:
		{
			tileDrawInfo.finalColor = new Color(255, 255, 255, 100);
			int num6 = 2;
			if (tileDrawInfo.tileFrameX >= 324)
			{
				tileDrawInfo.finalColor = Color.Transparent;
			}
			if (tileDrawInfo.tileFrameY < 36)
			{
				val2.Y += num6 * (tileDrawInfo.tileFrameY == 0).ToDirectionInt();
			}
			else
			{
				val2.X += num6 * (tileDrawInfo.tileFrameY == 36).ToDirectionInt();
			}
			break;
		}
		case 723:
		case 724:
			switch (tileDrawInfo.tileFrameX / 18)
			{
			case 0:
				val2 += new Vector2(0f, 2f);
				break;
			case 1:
				val2 += new Vector2(0f, -2f);
				break;
			case 2:
				val2 += new Vector2(-2f, 0f);
				break;
			case 3:
				val2 += new Vector2(2f, 0f);
				break;
			}
			break;
		case 272:
		{
			int num8 = Main.tileFrame[tileDrawInfo.typeCache];
			num8 += tileX % 2;
			num8 += tileY % 2;
			num8 += tileX % 3;
			num8 += tileY % 3;
			num8 %= 2;
			num8 *= 90;
			tileDrawInfo.addFrY += num8;
			val.Y += num8;
			break;
		}
		case 80:
		{
			WorldGen.GetCactusType(tileX, tileY, tileDrawInfo.tileFrameX, tileDrawInfo.tileFrameY, out var evil, out var good, out var crimson);
			if (evil)
			{
				val.Y += 54;
			}
			if (good)
			{
				val.Y += 108;
			}
			if (crimson)
			{
				val.Y += 162;
			}
			break;
		}
		case 83:
			tileDrawInfo.drawTexture = GetTileDrawTexture(tileDrawInfo.tileCache, tileX, tileY);
			break;
		case 323:
			if (tileDrawInfo.tileCache.frameX <= 132 && tileDrawInfo.tileCache.frameX >= 88)
			{
				return;
			}
			val2.X += tileDrawInfo.tileCache.frameY;
			break;
		case 114:
			if (tileDrawInfo.tileFrameY > 0)
			{
				val.Height += 2;
			}
			break;
		}
		if (tileDrawInfo.typeCache == 314)
		{
			DrawTile_MinecartTrack(screenPosition, screenOffset, tileX, tileY, tileDrawInfo);
		}
		else if (tileDrawInfo.typeCache == 171)
		{
			DrawXmasTree(screenPosition, screenOffset, tileX, tileY, tileDrawInfo);
		}
		else
		{
			DrawBasicTile(screenPosition, screenOffset, tileX, tileY, tileDrawInfo, val, val2);
		}
		if (Main.tileGlowMask[tileDrawInfo.tileCache.type] != -1)
		{
			short num9 = Main.tileGlowMask[tileDrawInfo.tileCache.type];
			if (TextureAssets.GlowMask.IndexInRange(num9))
			{
				tileDrawInfo.drawTexture = TextureAssets.GlowMask[num9].Value;
			}
			double num10 = Main.timeForVisualEffects * 0.08;
			Color val4 = Color.White;
			bool flag3 = false;
			switch (tileDrawInfo.tileCache.type)
			{
			case 718:
				val4 = new Color(0, 0, 0, 0);
				break;
			case 717:
			{
				float num13 = LavaLightA(tileX, tileY);
				val4 = new Color(num13, num13, num13, num13 / 2f);
				break;
			}
			case 633:
				val4 = Color.Lerp(Color.White, tileDrawInfo.finalColor, 0.75f);
				break;
			case 659:
			case 667:
			case 708:
				val4 = LiquidRenderer.GetShimmerGlitterColor(top: true, tileX, tileY);
				break;
			case 350:
				val4 = new Color(new Vector4((float)((0.0 - Math.Cos(((int)(num10 / 6.283) % 3 == 1) ? num10 : 0.0)) * 0.2 + 0.2)));
				break;
			case 381:
			case 517:
			case 687:
				val4 = _lavaMossGlow;
				break;
			case 534:
			case 535:
			case 689:
				val4 = _kryptonMossGlow;
				break;
			case 536:
			case 537:
			case 690:
				val4 = _xenonMossGlow;
				break;
			case 539:
			case 540:
			case 688:
				val4 = _argonMossGlow;
				break;
			case 625:
			case 626:
			case 691:
				val4 = _violetMossGlow;
				break;
			case 627:
			case 628:
			case 692:
				val4 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				break;
			case 699:
				val4 = Color.White;
				break;
			case 370:
			case 390:
				val4 = _meteorGlow;
				break;
			case 391:
				val4 = new Color(250, 250, 250, 200);
				break;
			case 209:
				val4 = PortalHelper.GetPortalColor(Main.myPlayer, (tileDrawInfo.tileCache.frameX >= 288) ? 1 : 0);
				break;
			case 429:
			case 445:
				tileDrawInfo.drawTexture = GetTileDrawTexture(tileDrawInfo.tileCache, tileX, tileY);
				tileDrawInfo.addFrY = 18;
				break;
			case 129:
			{
				if (tileDrawInfo.tileFrameX < 324)
				{
					flag3 = true;
					break;
				}
				tileDrawInfo.drawTexture = GetTileDrawTexture(tileDrawInfo.tileCache, tileX, tileY);
				val4 = Main.hslToRgb(0.7f + (float)Math.Sin((float)Math.PI * 2f * Main.GlobalTimeWrappedHourly * 0.16f + (float)tileX * 0.3f + (float)tileY * 0.7f) * 0.16f, 1f, 0.5f);
				val4.A /= 2;
				val4 *= 0.3f;
				int num11 = 72;
				for (float num12 = 0f; num12 < (float)Math.PI * 2f; num12 += (float)Math.PI / 2f)
				{
					Main.tileBatch.Draw(tileDrawInfo.drawTexture, val2 + num12.ToRotationVector2() * 2f, new Rectangle(tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, tileDrawInfo.tileFrameY + tileDrawInfo.addFrY + num11, tileDrawInfo.tileWidth, tileDrawInfo.tileHeight), val4, Vector2.Zero, 1f, (SpriteEffects)0);
				}
				val4 = new Color(255, 255, 255, 100);
				break;
			}
			case 725:
			{
				float opacity = Filters.Scene["Noir"].Opacity;
				if (opacity > 0f && tileDrawInfo.tileFrameX % 36 == 0 && tileDrawInfo.tileFrameY == 54)
				{
					Vector2 position = val2 + new Vector2(16f, 24f);
					SpriteEffects effects = (tileDrawInfo.tileFrameX >= 36 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
					val4 = new Color(255, 255, 255, 100) * opacity;
					TileBatch tileBatch = Main.tileBatch;
					Texture2D drawTexture = tileDrawInfo.drawTexture;
					Rectangle sourceRectangle = tileDrawInfo.drawTexture.Frame();
					VertexColors colors = val4;
					Rectangle val5 = tileDrawInfo.drawTexture.Frame();
					tileBatch.Draw(drawTexture, position, sourceRectangle, colors, val5.Center.ToVector2(), 1f, effects);
				}
				flag3 = true;
				break;
			}
			}
			if (!flag3)
			{
				if (tileDrawInfo.tileCache.slope() == 0 && !tileDrawInfo.tileCache.halfBrick())
				{
					Main.tileBatch.Draw(tileDrawInfo.drawTexture, val2, new Rectangle(tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, tileDrawInfo.tileFrameY + tileDrawInfo.addFrY, tileDrawInfo.tileWidth, tileDrawInfo.tileHeight), val4, Vector2.Zero, 1f, (SpriteEffects)0);
				}
				else if (tileDrawInfo.tileCache.halfBrick())
				{
					Main.tileBatch.Draw(tileDrawInfo.drawTexture, val2, val, val4, _zero, 1f, (SpriteEffects)0);
				}
				else if (TileID.Sets.HasSlopeFrames[tileDrawInfo.tileCache.type])
				{
					Main.tileBatch.Draw(tileDrawInfo.drawTexture, val2, new Rectangle(tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, tileDrawInfo.tileFrameY + tileDrawInfo.addFrY, 16, 16), val4, _zero, 1f, tileDrawInfo.tileSpriteEffect);
				}
				else
				{
					int num14 = tileDrawInfo.tileCache.slope();
					int num15 = 2;
					for (int i = 0; i < 8; i++)
					{
						int num16 = i * -2;
						int num17 = 16 - i * 2;
						int num18 = 16 - num17;
						int num19;
						switch (num14)
						{
						case 1:
							num16 = 0;
							num19 = i * 2;
							num17 = 14 - i * 2;
							num18 = 0;
							break;
						case 2:
							num16 = 0;
							num19 = 16 - i * 2 - 2;
							num17 = 14 - i * 2;
							num18 = 0;
							break;
						case 3:
							num19 = i * 2;
							break;
						default:
							num19 = 16 - i * 2 - 2;
							break;
						}
						Main.tileBatch.Draw(tileDrawInfo.drawTexture, val2 + new Vector2((float)num19, (float)(i * num15 + num16)), new Rectangle(tileDrawInfo.tileFrameX + tileDrawInfo.addFrX + num19, tileDrawInfo.tileFrameY + tileDrawInfo.addFrY + num18, num15, num17), val4, _zero, 1f, tileDrawInfo.tileSpriteEffect);
					}
					int num20 = ((num14 <= 2) ? 14 : 0);
					Main.tileBatch.Draw(tileDrawInfo.drawTexture, val2 + new Vector2(0f, (float)num20), new Rectangle(tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, tileDrawInfo.tileFrameY + tileDrawInfo.addFrY + num20, 16, 2), val4, _zero, 1f, tileDrawInfo.tileSpriteEffect);
				}
			}
		}
		if (tileDrawInfo.glowTexture != null)
		{
			if (tileDrawInfo.typeCache == 412)
			{
				int num21 = Main.tileFrame[tileDrawInfo.typeCache] / 60;
				int num22 = (num21 + 1) % 4;
				float num23 = (float)(Main.tileFrame[tileDrawInfo.typeCache] % 60) / 60f;
				Rectangle glowSourceRect = tileDrawInfo.glowSourceRect;
				glowSourceRect.Y += num21 * 18 * 3;
				Rectangle glowSourceRect2 = tileDrawInfo.glowSourceRect;
				glowSourceRect2.Y += num22 * 18 * 3;
				Vector2 position2 = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)tileDrawInfo.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + tileDrawInfo.tileTop)) + screenOffset;
				Main.tileBatch.Draw(tileDrawInfo.glowTexture, position2, glowSourceRect, tileDrawInfo.glowColor * (1f - num23), _zero, 1f, tileDrawInfo.tileSpriteEffect);
				Main.tileBatch.Draw(tileDrawInfo.glowTexture, position2, glowSourceRect2, tileDrawInfo.glowColor * num23, _zero, 1f, tileDrawInfo.tileSpriteEffect);
			}
			else
			{
				Vector2 val6 = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)tileDrawInfo.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + tileDrawInfo.tileTop)) + screenOffset;
				if (TileID.Sets.Platforms[tileDrawInfo.typeCache])
				{
					val6 = val2;
				}
				Main.tileBatch.SetLayer(Layer_Tiles, 1);
				Main.tileBatch.Draw(tileDrawInfo.glowTexture, val6, tileDrawInfo.glowSourceRect, tileDrawInfo.glowColor, _zero, 1f, tileDrawInfo.tileSpriteEffect);
				if (TileID.Sets.Platforms[tileDrawInfo.typeCache] && tileDrawInfo.tileCache.slope() != 0)
				{
					Tile tile = Main.tile[tileX, tileY + 1];
					Tile tile2 = Main.tile[tileX - 1, tileY + 1];
					Tile tile3 = Main.tile[tileX + 1, tileY + 1];
					bool shouldShowInvisibleBlocks = _shouldShowInvisibleBlocks;
					if (tileDrawInfo.tileCache.slope() == 1 && tile3.active() && (shouldShowInvisibleBlocks || !tile3.invisibleBlock()) && Main.tileSolid[tile3.type] && tile3.slope() != 2 && !tile3.halfBrick() && (!tile.active() || (!shouldShowInvisibleBlocks && tile.invisibleBlock()) || (tile.blockType() != 0 && tile.blockType() != 5) || !TileID.Sets.BlocksStairs[tile.type]))
					{
						Rectangle glowSourceRect3 = tileDrawInfo.glowSourceRect;
						if (TileID.Sets.Platforms[tile3.type] && tile3.slope() == 0)
						{
							glowSourceRect3.X = 324;
						}
						else
						{
							glowSourceRect3.X = 198;
						}
						Main.tileBatch.SetLayer(Layer_BehindTiles, 1);
						Main.tileBatch.Draw(tileDrawInfo.glowTexture, val6 + new Vector2(0f, 16f), glowSourceRect3, tileDrawInfo.glowColor, _zero, 1f, tileDrawInfo.tileSpriteEffect);
					}
					else if (tileDrawInfo.tileCache.slope() == 2 && tile2.active() && (shouldShowInvisibleBlocks || !tile2.invisibleBlock()) && Main.tileSolid[tile2.type] && tile2.slope() != 1 && !tile2.halfBrick() && (!tile.active() || (!shouldShowInvisibleBlocks && tile.invisibleBlock()) || (tile.blockType() != 0 && tile.blockType() != 4) || !TileID.Sets.BlocksStairs[tile.type]))
					{
						Rectangle glowSourceRect4 = tileDrawInfo.glowSourceRect;
						if (TileID.Sets.Platforms[tile2.type] && tile2.slope() == 0)
						{
							glowSourceRect4.X = 306;
						}
						else
						{
							glowSourceRect4.X = 162;
						}
						Main.tileBatch.SetLayer(Layer_BehindTiles, 1);
						Main.tileBatch.Draw(tileDrawInfo.glowTexture, val6 + new Vector2(0f, 16f), glowSourceRect4, tileDrawInfo.glowColor, _zero, 1f, tileDrawInfo.tileSpriteEffect);
					}
				}
			}
		}
		if (highlightTexture != null)
		{
			empty = new Rectangle(tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, tileDrawInfo.tileFrameY + tileDrawInfo.addFrY, tileDrawInfo.tileWidth, tileDrawInfo.tileHeight);
			int num24 = 0;
			int num25 = 0;
			Main.tileBatch.Draw(highlightTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)tileDrawInfo.tileWidth - 16f) / 2f + (float)num24, (float)(tileY * 16 - (int)screenPosition.Y + tileDrawInfo.tileTop + num25)) + screenOffset, empty, highlightColor, _zero, 1f, tileDrawInfo.tileSpriteEffect);
		}
	}

	private bool IsVisible(Tile tile)
	{
		bool flag = tile.invisibleBlock();
		switch (tile.type)
		{
		case 19:
			if (tile.frameY / 18 == 48)
			{
				flag = true;
			}
			break;
		case 541:
		case 631:
			flag = true;
			break;
		}
		if (flag)
		{
			return _shouldShowInvisibleBlocks;
		}
		return true;
	}

	public Texture2D GetTileDrawTexture(Tile tile, int tileX, int tileY)
	{
		TilePaintSystemV2.TileVariationkey key = new TilePaintSystemV2.TileVariationkey
		{
			TileType = tile.type,
			TileStyle = 0,
			PaintColor = tile.color()
		};
		switch (tile.type)
		{
		case 5:
			key.TileStyle = GetTreeBiome(tileX, tileY, tile.frameX, tile.frameY);
			break;
		case 323:
			key.TileStyle = GetPalmTreeBiome(tileX, tileY);
			break;
		case 83:
			if (WorldGen.IsAlchemyPlantHarvestable(tile.frameX / 18, tileY))
			{
				key.TileType = 84;
			}
			break;
		}
		return GetTileDrawTexture(key);
	}

	public Texture2D GetTileDrawTexture(int tileType, int paintColor)
	{
		return GetTileDrawTexture(new TilePaintSystemV2.TileVariationkey
		{
			TileType = tileType,
			PaintColor = paintColor
		});
	}

	public Texture2D GetTileDrawTexture(TilePaintSystemV2.TileVariationkey key)
	{
		if (_lastPaintLookupKey == key)
		{
			return _lastPaintLookupTexture;
		}
		_lastPaintLookupKey = key;
		_lastPaintLookupTexture = LookupTileDrawTexture(key);
		return _lastPaintLookupTexture;
	}

	private Texture2D LookupTileDrawTexture(TilePaintSystemV2.TileVariationkey key)
	{
		Main.instance.LoadTiles(key.TileType);
		if (key.PaintColor != 0 || key.TileStyle != 0)
		{
			Texture2D val = _paintSystem.TryGetTileAndRequestIfNotReady(key.TileType, key.TileStyle, key.PaintColor);
			if (val != null)
			{
				return val;
			}
		}
		return TextureAssets.Tile[key.TileType].Value;
	}

	private Texture2D LookupCageTopDrawTexture(TilePaintSystemV2.CageTopVariationkey key)
	{
		if (key.PaintColor != 0)
		{
			Texture2D val = _paintSystem.TryGetCageTopAndRequestIfNotReady(key.CageStyle, key.PaintColor);
			if (val != null)
			{
				return val;
			}
		}
		return TextureAssets.CageTop[key.CageStyle].Value;
	}

	private void DrawBasicTile(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData, Rectangle normalTileRect, Vector2 normalTilePosition)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a68: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f07: Unknown result type (might be due to invalid IL or missing references)
		bool flag = !TileID.Sets.DontDrawTileSliced[drawData.tileCache.type];
		bool flag2 = !TileID.Sets.DontDrawTileSlopes[drawData.tileCache.type];
		if (drawData.typeCache == 380 || TileID.Sets.Platforms[drawData.typeCache])
		{
			DrawTile_BackRope(screenPosition, screenOffset, tileX, tileY, drawData);
		}
		if (flag2 && drawData.tileCache.slope() > 0)
		{
			if (TileID.Sets.Platforms[drawData.tileCache.type])
			{
				Tile tile = Main.tile[tileX, tileY + 1];
				Tile tile2 = Main.tile[tileX - 1, tileY + 1];
				Tile tile3 = Main.tile[tileX + 1, tileY + 1];
				bool shouldShowInvisibleBlocks = _shouldShowInvisibleBlocks;
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, normalTileRect, drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				if (drawData.tileCache.slope() == 1 && tile3.active() && (shouldShowInvisibleBlocks || !tile3.invisibleBlock()) && Main.tileSolid[tile3.type] && tile3.slope() != 2 && !tile3.halfBrick() && (!tile.active() || (!shouldShowInvisibleBlocks && tile.invisibleBlock()) || (tile.blockType() != 0 && tile.blockType() != 5) || !TileID.Sets.BlocksStairs[tile.type]))
				{
					Main.tileBatch.SetLayer(Layer_BehindTiles, 0);
					Rectangle sourceRectangle = new Rectangle(198, (int)drawData.tileFrameY, 16, 16);
					if (TileID.Sets.Platforms[tile3.type] && tile3.slope() == 0)
					{
						sourceRectangle.X = 324;
					}
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 16f), sourceRectangle, drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				}
				else if (drawData.tileCache.slope() == 2 && tile2.active() && (shouldShowInvisibleBlocks || !tile2.invisibleBlock()) && Main.tileSolid[tile2.type] && tile2.slope() != 1 && !tile2.halfBrick() && (!tile.active() || (!shouldShowInvisibleBlocks && tile.invisibleBlock()) || (tile.blockType() != 0 && tile.blockType() != 4) || !TileID.Sets.BlocksStairs[tile.type]))
				{
					Main.tileBatch.SetLayer(Layer_BehindTiles, 0);
					Rectangle sourceRectangle2 = new Rectangle(162, (int)drawData.tileFrameY, 16, 16);
					if (TileID.Sets.Platforms[tile2.type] && tile2.slope() == 0)
					{
						sourceRectangle2.X = 306;
					}
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 16f), sourceRectangle2, drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				}
				return;
			}
			if (TileID.Sets.HasSlopeFrames[drawData.tileCache.type])
			{
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, 16, 16), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				return;
			}
			int num = drawData.tileCache.slope();
			int num2 = 2;
			for (int i = 0; i < 8; i++)
			{
				int num3 = i * -2;
				int num4 = 16 - i * 2;
				int num5 = 16 - num4;
				int num6;
				switch (num)
				{
				case 1:
					num3 = 0;
					num6 = i * 2;
					num4 = 14 - i * 2;
					num5 = 0;
					break;
				case 2:
					num3 = 0;
					num6 = 16 - i * 2 - 2;
					num4 = 14 - i * 2;
					num5 = 0;
					break;
				case 3:
					num6 = i * 2;
					break;
				default:
					num6 = 16 - i * 2 - 2;
					break;
				}
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2((float)num6, (float)(i * num2 + num3)), new Rectangle(drawData.tileFrameX + drawData.addFrX + num6, drawData.tileFrameY + drawData.addFrY + num5, num2, num4), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
			}
			int num7 = ((num <= 2) ? 14 : 0);
			Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, (float)num7), new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY + num7, 16, 2), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
			return;
		}
		if (flag2 && !TileID.Sets.Platforms[drawData.typeCache] && !TileID.Sets.IgnoresNearbyHalfbricksWhenDrawn[drawData.typeCache] && _tileSolid[drawData.typeCache] && !TileID.Sets.NotReallySolid[drawData.typeCache] && !drawData.tileCache.halfBrick() && (Main.tile[tileX - 1, tileY].halfBrick() || Main.tile[tileX + 1, tileY].halfBrick()))
		{
			if (Main.tile[tileX - 1, tileY].halfBrick() && Main.tile[tileX + 1, tileY].halfBrick())
			{
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY + 8, drawData.tileWidth, 8), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				Rectangle sourceRectangle3 = new Rectangle(126 + drawData.addFrX, drawData.addFrY, 16, 8);
				if (Main.tile[tileX, tileY - 1].active() && !Main.tile[tileX, tileY - 1].bottomSlope() && Main.tile[tileX, tileY - 1].type == drawData.typeCache)
				{
					sourceRectangle3 = new Rectangle(90 + drawData.addFrX, drawData.addFrY, 16, 8);
				}
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, sourceRectangle3, drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
			}
			else if (Main.tile[tileX - 1, tileY].halfBrick())
			{
				int num8 = 4;
				if (TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[drawData.typeCache])
				{
					num8 = 2;
				}
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY + 8, drawData.tileWidth, 8), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2((float)num8, 0f), new Rectangle(drawData.tileFrameX + num8 + drawData.addFrX, drawData.addFrY + drawData.tileFrameY, drawData.tileWidth - num8, drawData.tileHeight), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle(144 + drawData.addFrX, drawData.addFrY, num8, 8), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				if (num8 == 2)
				{
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle(148 + drawData.addFrX, drawData.addFrY, 2, 2), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				}
			}
			else if (Main.tile[tileX + 1, tileY].halfBrick())
			{
				int num9 = 4;
				if (TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[drawData.typeCache])
				{
					num9 = 2;
				}
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY + 8, drawData.tileWidth, 8), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY, drawData.tileWidth - num9, drawData.tileHeight), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2((float)(16 - num9), 0f), new Rectangle(144 + (16 - num9), 0, num9, 8), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				if (num9 == 2)
				{
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(14f, 0f), new Rectangle(156, 0, 2, 2), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				}
			}
			return;
		}
		if (flag && Lighting.NotRetro && _tileSolid[drawData.typeCache] && !drawData.tileCache.halfBrick())
		{
			DrawSingleTile_SlicedBlock(normalTilePosition, tileX, tileY, drawData);
			return;
		}
		if (drawData.halfBrickHeight == 8 && (!Main.tile[tileX, tileY + 1].active() || !_tileSolid[Main.tile[tileX, tileY + 1].type] || Main.tile[tileX, tileY + 1].halfBrick()))
		{
			if (TileID.Sets.Platforms[drawData.typeCache])
			{
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, normalTileRect, drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
			}
			else
			{
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, normalTileRect.Modified(0, 0, 0, -4), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 4f), new Rectangle(144 + drawData.addFrX, 66 + drawData.addFrY, drawData.tileWidth, 4), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
			}
		}
		else if (TileID.Sets.CritterCageLidStyle[drawData.typeCache] >= 0)
		{
			int num10 = TileID.Sets.CritterCageLidStyle[drawData.typeCache];
			if ((num10 < 3 && normalTileRect.Y % 54 == 0) || (num10 >= 3 && normalTileRect.Y % 36 == 0))
			{
				Vector2 position = normalTilePosition;
				position.Y += 8f;
				Rectangle sourceRectangle4 = normalTileRect;
				sourceRectangle4.Y += 8;
				sourceRectangle4.Height -= 8;
				Main.tileBatch.Draw(drawData.drawTexture, position, sourceRectangle4, drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
				position = normalTilePosition;
				position.Y -= 2f;
				sourceRectangle4 = normalTileRect;
				if (num10 == 0)
				{
					sourceRectangle4.X = normalTileRect.X % 108;
				}
				sourceRectangle4.Y = 0;
				sourceRectangle4.Height = 10;
				Texture2D texture = LookupCageTopDrawTexture(new TilePaintSystemV2.CageTopVariationkey
				{
					CageStyle = num10,
					PaintColor = drawData.tileCache.color()
				});
				Main.tileBatch.Draw(texture, position, sourceRectangle4, drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
			}
			else
			{
				Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, normalTileRect, drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
			}
		}
		else if (drawData.typeCache == 711)
		{
			Rectangle val = new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight);
			if (normalTileRect.X == 0 && normalTileRect.Y == 0)
			{
				Rectangle val2 = val;
				val2.X += 38;
				for (float num11 = 0f; num11 < 1f; num11 += 1f / 3f)
				{
					float num12 = Main.GlobalTimeWrappedHourly % 2f / 2f;
					Color val3 = Main.hslToRgb((num12 + num11) % 1f, 1f, 0.5f);
					val3.A = 0;
					val3 *= 0.3f;
					for (int j = 0; j < 2; j++)
					{
						if (j == 1)
						{
							val2.Width = val.Width + 2;
						}
						else
						{
							val2.Width = val.Width;
						}
						for (int k = 0; k < 2; k++)
						{
							if (k == 1)
							{
								val2.Height = val.Height + 2;
							}
							else
							{
								val2.Height = val.Height;
							}
							Main.tileBatch.Draw(drawData.drawTexture, (normalTilePosition + new Vector2((float)(j * 16), (float)(k * 16)) + ((num12 + num11) * ((float)Math.PI * 2f)).ToRotationVector2() * 4f).Floor(), new Rectangle(val2.X + j * 18, val2.Y + k * 18, val2.Width, val2.Height), val3, _zero, 1f, drawData.tileSpriteEffect);
						}
					}
				}
			}
			Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, val, drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
		}
		else
		{
			Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, normalTileRect, drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
		}
		DrawSingleTile_Flames(screenPosition, screenOffset, tileX, tileY, drawData);
	}

	private int GetPalmTreeBiome(int tileX, int tileY)
	{
		int i;
		for (i = tileY; Main.tile[tileX, i].active() && Main.tile[tileX, i].type == 323; i++)
		{
		}
		return GetPalmTreeVariant(tileX, i);
	}

	private static int GetTreeBiome(int tileX, int tileY, int tileFrameX, int tileFrameY)
	{
		int num = tileX;
		int i = tileY;
		int type = Main.tile[num, i].type;
		if (tileFrameX == 66 && tileFrameY <= 45)
		{
			num++;
		}
		if (tileFrameX == 88 && tileFrameY >= 66 && tileFrameY <= 110)
		{
			num--;
		}
		if (tileFrameY >= 198)
		{
			switch (tileFrameX)
			{
			case 66:
				num--;
				break;
			case 44:
				num++;
				break;
			}
		}
		else if (tileFrameY >= 132)
		{
			switch (tileFrameX)
			{
			case 22:
				num--;
				break;
			case 44:
				num++;
				break;
			}
		}
		for (; Main.tile[num, i].active() && Main.tile[num, i].type == type; i++)
		{
		}
		return GetTreeVariant(num, i);
	}

	public static int GetTreeVariant(int x, int y)
	{
		if (Main.tile[x, y] == null || !Main.tile[x, y].active())
		{
			return -1;
		}
		switch ((int)Main.tile[x, y].type)
		{
		case 23:
		case 661:
			return 0;
		case 60:
			if (!((double)y > Main.worldSurface))
			{
				return 1;
			}
			return 5;
		case 70:
			return 6;
		case 109:
		case 492:
			return 2;
		case 147:
			return 3;
		case 199:
		case 662:
			return 4;
		default:
			return -1;
		}
	}

	private Color GetFallenStarFurnitureFlameColor()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		float num = Utils.WrappedLerp(0.5f, 1f, Main.GlobalTimeWrappedHourly % 2f / 2f);
		int num2 = (int)(150f * num);
		return new Color(150, num2, num2, 50);
	}

	private Color GetHallowedFurnitureFlameColor()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		float num = Utils.WrappedLerp(0.5f, 1f, Main.GlobalTimeWrappedHourly % 2f / 2f);
		int num2 = (int)(170f * num);
		return new Color(170, num2, num2, 75);
	}

	private Color GetCloudFurnitureFlameColor()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		return GetWrappedFurnitureFlameColor(new Color(255, 255, 255, 0));
	}

	private Color GetLibrarianFurnitureFlameColor()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		return GetWrappedFurnitureFlameColor(new Color(255, 255, 255, 0), 0.25f);
	}

	private Color GetForbiddenFurnitureFlameColor()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		return GetWrappedFurnitureFlameColor(new Color(255, 255, 255, 0), 0.25f);
	}

	private Color GetBoulderFurnitureFlameColor()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		return GetWrappedFurnitureFlameColor(new Color(255, 255, 255, 0), 0.25f);
	}

	private Color GetWrappedFurnitureFlameColor(Color baseColor, float min = 0.75f, float max = 1f)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		float num = Utils.WrappedLerp(min, max, Main.GlobalTimeWrappedHourly % 2f / 2f);
		return baseColor * num;
	}

	private TileFlameData GetTileFlameData(int tileX, int tileY, int type, int tileFrameY)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1428: Unknown result type (might be due to invalid IL or missing references)
		//IL_142d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1484: Unknown result type (might be due to invalid IL or missing references)
		//IL_1489: Unknown result type (might be due to invalid IL or missing references)
		//IL_14df: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1890: Unknown result type (might be due to invalid IL or missing references)
		//IL_1895: Unknown result type (might be due to invalid IL or missing references)
		//IL_153b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1540: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1596: Unknown result type (might be due to invalid IL or missing references)
		//IL_159b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0940: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e77: Unknown result type (might be due to invalid IL or missing references)
		//IL_127b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe2: Unknown result type (might be due to invalid IL or missing references)
		//IL_164e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1653: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_173e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1743: Unknown result type (might be due to invalid IL or missing references)
		//IL_1770: Unknown result type (might be due to invalid IL or missing references)
		//IL_1775: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1819: Unknown result type (might be due to invalid IL or missing references)
		//IL_181e: Unknown result type (might be due to invalid IL or missing references)
		//IL_184b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1850: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ceb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1039: Unknown result type (might be due to invalid IL or missing references)
		//IL_103e: Unknown result type (might be due to invalid IL or missing references)
		//IL_108f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1094: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1129: Unknown result type (might be due to invalid IL or missing references)
		//IL_112e: Unknown result type (might be due to invalid IL or missing references)
		//IL_115b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1160: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1204: Unknown result type (might be due to invalid IL or missing references)
		//IL_1209: Unknown result type (might be due to invalid IL or missing references)
		//IL_1236: Unknown result type (might be due to invalid IL or missing references)
		//IL_123b: Unknown result type (might be due to invalid IL or missing references)
		switch (type)
		{
		case 270:
			return new TileFlameData
			{
				flameTexture = TextureAssets.FireflyJar.Value,
				flameColor = new Color(200, 200, 200, 0),
				flameCount = 1
			};
		case 271:
			return new TileFlameData
			{
				flameTexture = TextureAssets.LightningbugJar.Value,
				flameColor = new Color(200, 200, 200, 0),
				flameCount = 1
			};
		case 581:
			return new TileFlameData
			{
				flameTexture = TextureAssets.GlowMask[291].Value,
				flameColor = new Color(200, 100, 100, 0),
				flameCount = 1
			};
		default:
		{
			if (!Main.tileFlame[type])
			{
				return default;
			}
			ulong flameSeed = Main.TileFrameSeed ^ (ulong)(((long)tileX << 32) | (uint)tileY);
			int num = 0;
			switch (type)
			{
			case 4:
				num = 0;
				break;
			case 33:
			case 174:
				num = 1;
				break;
			case 100:
			case 173:
				num = 2;
				break;
			case 34:
				num = 3;
				break;
			case 93:
				num = 4;
				break;
			case 49:
				num = 5;
				break;
			case 372:
				num = 16;
				break;
			case 646:
				num = 17;
				break;
			case 98:
				num = 6;
				break;
			case 35:
				num = 7;
				break;
			case 42:
				num = 13;
				break;
			}
			TileFlameData result = new TileFlameData
			{
				flameTexture = TextureAssets.Flames[num].Value,
				flameSeed = flameSeed
			};
			switch (num)
			{
			case 7:
				result.flameCount = 4;
				result.flameColor = new Color(50, 50, 50, 0);
				result.flameRangeXMin = -10;
				result.flameRangeXMax = 11;
				result.flameRangeYMin = -10;
				result.flameRangeYMax = 10;
				result.flameRangeMultX = 0f;
				result.flameRangeMultY = 0f;
				break;
			case 1:
				switch (Main.tile[tileX, tileY].frameY / 22)
				{
				case 5:
				case 6:
				case 7:
				case 10:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					break;
				case 8:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					break;
				case 12:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					break;
				case 14:
					result.flameCount = 8;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					break;
				case 16:
					result.flameCount = 4;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.15f;
					break;
				case 27:
				case 28:
					result.flameCount = 1;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 43:
					result.flameCount = 1;
					result.flameColor = GetFallenStarFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 44:
					result.flameCount = 3;
					result.flameColor = new Color(200, 200, 200, 150);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				case 45:
					result.flameCount = 1;
					result.flameColor = GetHallowedFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 56:
					result.flameCount = 1;
					result.flameColor = GetCloudFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 57:
				case 60:
					result.flameCount = 1;
					result.flameColor = new Color(200, 200, 200, 150);
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 58:
					result.flameCount = 1;
					result.flameColor = GetLibrarianFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 61:
					result.flameCount = 1;
					result.flameColor = GetForbiddenFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 63:
					result.flameCount = 1;
					result.flameColor = GetBoulderFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 47:
				case 48:
				case 49:
				case 51:
				case 52:
				case 54:
					result.flameCount = 0;
					break;
				default:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				}
				break;
			case 2:
				switch (Main.tile[tileX, tileY].frameY / 36)
				{
				case 3:
					result.flameCount = 3;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.05f;
					result.flameRangeMultY = 0.15f;
					break;
				case 6:
					result.flameCount = 5;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.15f;
					break;
				case 9:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					break;
				case 11:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					break;
				case 13:
					result.flameCount = 8;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					break;
				case 28:
				case 29:
					result.flameCount = 1;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 44:
					result.flameCount = 1;
					result.flameColor = GetFallenStarFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 45:
					result.flameCount = 3;
					result.flameColor = new Color(200, 200, 200, 150);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				case 46:
					result.flameCount = 1;
					result.flameColor = GetHallowedFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 57:
					result.flameCount = 1;
					result.flameColor = GetCloudFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 58:
				case 61:
					result.flameCount = 1;
					result.flameColor = new Color(200, 200, 200, 150);
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 59:
					result.flameCount = 1;
					result.flameColor = GetLibrarianFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 62:
					result.flameCount = 1;
					result.flameColor = GetForbiddenFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 64:
					result.flameCount = 1;
					result.flameColor = GetBoulderFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 48:
				case 49:
				case 50:
				case 52:
				case 53:
				case 55:
					result.flameCount = 0;
					break;
				default:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				}
				break;
			case 3:
			{
				int num2 = Main.tile[tileX, tileY].frameY / 54;
				if (Main.tile[tileX, tileY].frameX >= 108)
				{
					num2 += 37 * (Main.tile[tileX, tileY].frameX / 108);
				}
				switch (num2)
				{
				case 8:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					break;
				case 9:
					result.flameCount = 3;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -1;
					result.flameRangeXMax = 1;
					result.flameRangeYMin = -1;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 2f;
					result.flameRangeMultY = 2f;
					break;
				case 11:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					break;
				case 15:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					break;
				case 17:
				case 20:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					break;
				case 18:
					result.flameCount = 8;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					break;
				case 34:
				case 35:
					result.flameCount = 1;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 50:
					result.flameCount = 1;
					result.flameColor = GetFallenStarFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 51:
					result.flameCount = 3;
					result.flameColor = new Color(200, 200, 200, 150);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				case 52:
					result.flameCount = 1;
					result.flameColor = GetHallowedFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 63:
					result.flameCount = 1;
					result.flameColor = GetCloudFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 64:
				case 67:
					result.flameCount = 1;
					result.flameColor = new Color(200, 200, 200, 150);
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 65:
					result.flameCount = 1;
					result.flameColor = GetLibrarianFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 68:
					result.flameCount = 1;
					result.flameColor = GetForbiddenFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 70:
					result.flameCount = 1;
					result.flameColor = GetBoulderFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 54:
				case 55:
				case 56:
				case 58:
				case 59:
				case 61:
					result.flameCount = 0;
					break;
				default:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				}
				break;
			}
			case 4:
				switch (Main.tile[tileX, tileY].frameY / 54)
				{
				case 1:
					result.flameCount = 3;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.15f;
					break;
				case 2:
				case 4:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					break;
				case 3:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -20;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.2f;
					result.flameRangeMultY = 0.35f;
					break;
				case 5:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					break;
				case 9:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					break;
				case 13:
					result.flameCount = 8;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					break;
				case 12:
					result.flameCount = 1;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.01f;
					result.flameRangeMultY = 0.01f;
					break;
				case 28:
				case 29:
					result.flameCount = 1;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 44:
					result.flameCount = 1;
					result.flameColor = GetFallenStarFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 45:
					result.flameCount = 3;
					result.flameColor = new Color(200, 200, 200, 150);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				case 46:
					result.flameCount = 1;
					result.flameColor = GetHallowedFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 57:
					result.flameCount = 1;
					result.flameColor = GetCloudFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 58:
				case 61:
					result.flameCount = 1;
					result.flameColor = new Color(200, 200, 200, 150);
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 59:
					result.flameCount = 1;
					result.flameColor = GetLibrarianFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 62:
					result.flameCount = 1;
					result.flameColor = GetForbiddenFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 64:
					result.flameCount = 1;
					result.flameColor = GetBoulderFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 48:
				case 49:
				case 50:
				case 52:
				case 53:
				case 55:
					result.flameCount = 0;
					break;
				default:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				}
				break;
			case 13:
				switch (tileFrameY / 36)
				{
				case 1:
				case 3:
				case 6:
				case 8:
				case 19:
				case 27:
				case 29:
				case 30:
				case 31:
				case 32:
				case 36:
				case 39:
				case 53:
				case 57:
				case 60:
				case 62:
				case 66:
				case 69:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				case 2:
				case 16:
				case 25:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.1f;
					break;
				case 11:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					break;
				case 34:
				case 35:
					result.flameCount = 1;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 44:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				case 50:
					result.flameCount = 1;
					result.flameColor = GetFallenStarFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 51:
					result.flameCount = 3;
					result.flameColor = new Color(200, 200, 200, 150);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				case 52:
					result.flameCount = 1;
					result.flameColor = GetHallowedFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 63:
					result.flameCount = 1;
					result.flameColor = GetCloudFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 64:
				case 67:
					result.flameCount = 1;
					result.flameColor = new Color(200, 200, 200, 150);
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 65:
					result.flameCount = 1;
					result.flameColor = GetLibrarianFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 68:
					result.flameCount = 1;
					result.flameColor = GetForbiddenFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 70:
					result.flameCount = 1;
					result.flameColor = GetBoulderFurnitureFlameColor();
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 54:
				case 55:
				case 56:
				case 58:
				case 59:
				case 61:
					result.flameCount = 0;
					break;
				default:
					result.flameCount = 0;
					break;
				}
				break;
			default:
				result.flameCount = 7;
				result.flameColor = new Color(100, 100, 100, 0);
				if (tileFrameY / 22 == 14)
				{
					result.flameColor = new Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
				}
				result.flameRangeXMin = -10;
				result.flameRangeXMax = 11;
				result.flameRangeYMin = -10;
				result.flameRangeYMax = 1;
				result.flameRangeMultX = 0.15f;
				result.flameRangeMultY = 0.35f;
				break;
			}
			return result;
		}
		}
	}

	private void DrawSingleTile_Flames(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
	{
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_096a: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0980: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1001: Unknown result type (might be due to invalid IL or missing references)
		//IL_100b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1017: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_110f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1124: Unknown result type (might be due to invalid IL or missing references)
		//IL_112e: Unknown result type (might be due to invalid IL or missing references)
		//IL_113a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4240: Unknown result type (might be due to invalid IL or missing references)
		//IL_4263: Unknown result type (might be due to invalid IL or missing references)
		//IL_4274: Unknown result type (might be due to invalid IL or missing references)
		//IL_4279: Unknown result type (might be due to invalid IL or missing references)
		//IL_427a: Unknown result type (might be due to invalid IL or missing references)
		//IL_429b: Unknown result type (might be due to invalid IL or missing references)
		//IL_42b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_42ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_42f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4317: Unknown result type (might be due to invalid IL or missing references)
		//IL_4328: Unknown result type (might be due to invalid IL or missing references)
		//IL_432d: Unknown result type (might be due to invalid IL or missing references)
		//IL_432e: Unknown result type (might be due to invalid IL or missing references)
		//IL_434f: Unknown result type (might be due to invalid IL or missing references)
		//IL_436a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4374: Unknown result type (might be due to invalid IL or missing references)
		//IL_4380: Unknown result type (might be due to invalid IL or missing references)
		//IL_1268: Unknown result type (might be due to invalid IL or missing references)
		//IL_128e: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12df: Unknown result type (might be due to invalid IL or missing references)
		//IL_12eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_43eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_43f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4411: Unknown result type (might be due to invalid IL or missing references)
		//IL_4422: Unknown result type (might be due to invalid IL or missing references)
		//IL_4427: Unknown result type (might be due to invalid IL or missing references)
		//IL_4428: Unknown result type (might be due to invalid IL or missing references)
		//IL_442d: Unknown result type (might be due to invalid IL or missing references)
		//IL_442f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4434: Unknown result type (might be due to invalid IL or missing references)
		//IL_443e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4443: Unknown result type (might be due to invalid IL or missing references)
		//IL_4457: Unknown result type (might be due to invalid IL or missing references)
		//IL_4475: Unknown result type (might be due to invalid IL or missing references)
		//IL_4477: Unknown result type (might be due to invalid IL or missing references)
		//IL_4479: Unknown result type (might be due to invalid IL or missing references)
		//IL_4480: Unknown result type (might be due to invalid IL or missing references)
		//IL_448c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4460: Unknown result type (might be due to invalid IL or missing references)
		//IL_4467: Unknown result type (might be due to invalid IL or missing references)
		//IL_446c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1390: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1407: Unknown result type (might be due to invalid IL or missing references)
		//IL_1413: Unknown result type (might be due to invalid IL or missing references)
		//IL_4087: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d80: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ddd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4141: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ffe: Unknown result type (might be due to invalid IL or missing references)
		//IL_300a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3014: Unknown result type (might be due to invalid IL or missing references)
		//IL_3020: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_25eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_260c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2618: Unknown result type (might be due to invalid IL or missing references)
		//IL_2622: Unknown result type (might be due to invalid IL or missing references)
		//IL_262e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e89: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ecc: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_3090: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_30cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_30d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_30fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3107: Unknown result type (might be due to invalid IL or missing references)
		//IL_3113: Unknown result type (might be due to invalid IL or missing references)
		//IL_269a: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_26d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_26da: Unknown result type (might be due to invalid IL or missing references)
		//IL_26fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2707: Unknown result type (might be due to invalid IL or missing references)
		//IL_2711: Unknown result type (might be due to invalid IL or missing references)
		//IL_271d: Unknown result type (might be due to invalid IL or missing references)
		//IL_20eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_210e: Unknown result type (might be due to invalid IL or missing references)
		//IL_211f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2124: Unknown result type (might be due to invalid IL or missing references)
		//IL_2125: Unknown result type (might be due to invalid IL or missing references)
		//IL_2146: Unknown result type (might be due to invalid IL or missing references)
		//IL_2152: Unknown result type (might be due to invalid IL or missing references)
		//IL_215c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2168: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_147e: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_14be: Unknown result type (might be due to invalid IL or missing references)
		//IL_14df: Unknown result type (might be due to invalid IL or missing references)
		//IL_14eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1501: Unknown result type (might be due to invalid IL or missing references)
		//IL_418b: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_41c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_41cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_41f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_41f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4204: Unknown result type (might be due to invalid IL or missing references)
		//IL_411c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2789: Unknown result type (might be due to invalid IL or missing references)
		//IL_27af: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_27f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2800: Unknown result type (might be due to invalid IL or missing references)
		//IL_280c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2026: Unknown result type (might be due to invalid IL or missing references)
		//IL_204c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2060: Unknown result type (might be due to invalid IL or missing references)
		//IL_2065: Unknown result type (might be due to invalid IL or missing references)
		//IL_2066: Unknown result type (might be due to invalid IL or missing references)
		//IL_2087: Unknown result type (might be due to invalid IL or missing references)
		//IL_2093: Unknown result type (might be due to invalid IL or missing references)
		//IL_209d: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_156c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1592: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_317d: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_31bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_31bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_31de: Unknown result type (might be due to invalid IL or missing references)
		//IL_31ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3200: Unknown result type (might be due to invalid IL or missing references)
		//IL_2877: Unknown result type (might be due to invalid IL or missing references)
		//IL_289d: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_28d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_28e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_28fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_180f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1832: Unknown result type (might be due to invalid IL or missing references)
		//IL_1843: Unknown result type (might be due to invalid IL or missing references)
		//IL_1848: Unknown result type (might be due to invalid IL or missing references)
		//IL_1849: Unknown result type (might be due to invalid IL or missing references)
		//IL_186a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1876: Unknown result type (might be due to invalid IL or missing references)
		//IL_1880: Unknown result type (might be due to invalid IL or missing references)
		//IL_188c: Unknown result type (might be due to invalid IL or missing references)
		//IL_165b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1681: Unknown result type (might be due to invalid IL or missing references)
		//IL_1695: Unknown result type (might be due to invalid IL or missing references)
		//IL_169a: Unknown result type (might be due to invalid IL or missing references)
		//IL_169b: Unknown result type (might be due to invalid IL or missing references)
		//IL_16bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16de: Unknown result type (might be due to invalid IL or missing references)
		//IL_326b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3291: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_32aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_32cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_32d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_296c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2992: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_29cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_174a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1770: Unknown result type (might be due to invalid IL or missing references)
		//IL_1784: Unknown result type (might be due to invalid IL or missing references)
		//IL_1789: Unknown result type (might be due to invalid IL or missing references)
		//IL_178a: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_352f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3555: Unknown result type (might be due to invalid IL or missing references)
		//IL_3569: Unknown result type (might be due to invalid IL or missing references)
		//IL_356e: Unknown result type (might be due to invalid IL or missing references)
		//IL_356f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3590: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_35c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_35cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3359: Unknown result type (might be due to invalid IL or missing references)
		//IL_337f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3393: Unknown result type (might be due to invalid IL or missing references)
		//IL_3398: Unknown result type (might be due to invalid IL or missing references)
		//IL_3399: Unknown result type (might be due to invalid IL or missing references)
		//IL_33ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_33c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_33d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_33dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b54: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b91: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3601: Unknown result type (might be due to invalid IL or missing references)
		//IL_3624: Unknown result type (might be due to invalid IL or missing references)
		//IL_3635: Unknown result type (might be due to invalid IL or missing references)
		//IL_363a: Unknown result type (might be due to invalid IL or missing references)
		//IL_363b: Unknown result type (might be due to invalid IL or missing references)
		//IL_365c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3668: Unknown result type (might be due to invalid IL or missing references)
		//IL_3672: Unknown result type (might be due to invalid IL or missing references)
		//IL_367e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3448: Unknown result type (might be due to invalid IL or missing references)
		//IL_346e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3482: Unknown result type (might be due to invalid IL or missing references)
		//IL_3487: Unknown result type (might be due to invalid IL or missing references)
		//IL_3488: Unknown result type (might be due to invalid IL or missing references)
		//IL_34a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_34b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_34bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_34cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_2450: Unknown result type (might be due to invalid IL or missing references)
		//IL_2476: Unknown result type (might be due to invalid IL or missing references)
		//IL_248a: Unknown result type (might be due to invalid IL or missing references)
		//IL_248f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2490: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_24d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2211: Unknown result type (might be due to invalid IL or missing references)
		//IL_2237: Unknown result type (might be due to invalid IL or missing references)
		//IL_224b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2250: Unknown result type (might be due to invalid IL or missing references)
		//IL_2251: Unknown result type (might be due to invalid IL or missing references)
		//IL_2282: Unknown result type (might be due to invalid IL or missing references)
		//IL_2289: Unknown result type (might be due to invalid IL or missing references)
		//IL_2293: Unknown result type (might be due to invalid IL or missing references)
		//IL_229f: Unknown result type (might be due to invalid IL or missing references)
		//IL_235a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2380: Unknown result type (might be due to invalid IL or missing references)
		//IL_2394: Unknown result type (might be due to invalid IL or missing references)
		//IL_2399: Unknown result type (might be due to invalid IL or missing references)
		//IL_239a: Unknown result type (might be due to invalid IL or missing references)
		//IL_23cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_23dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_23e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1935: Unknown result type (might be due to invalid IL or missing references)
		//IL_195b: Unknown result type (might be due to invalid IL or missing references)
		//IL_196f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1974: Unknown result type (might be due to invalid IL or missing references)
		//IL_1975: Unknown result type (might be due to invalid IL or missing references)
		//IL_1996: Unknown result type (might be due to invalid IL or missing references)
		//IL_199d: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e75: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ee6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3966: Unknown result type (might be due to invalid IL or missing references)
		//IL_398c: Unknown result type (might be due to invalid IL or missing references)
		//IL_39a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_39a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_39a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_39d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_39e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_39ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3727: Unknown result type (might be due to invalid IL or missing references)
		//IL_374d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3761: Unknown result type (might be due to invalid IL or missing references)
		//IL_3766: Unknown result type (might be due to invalid IL or missing references)
		//IL_3767: Unknown result type (might be due to invalid IL or missing references)
		//IL_3798: Unknown result type (might be due to invalid IL or missing references)
		//IL_379f: Unknown result type (might be due to invalid IL or missing references)
		//IL_37a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dce: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3870: Unknown result type (might be due to invalid IL or missing references)
		//IL_3896: Unknown result type (might be due to invalid IL or missing references)
		//IL_38aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_38af: Unknown result type (might be due to invalid IL or missing references)
		//IL_38b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_38e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_38e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_38fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_3da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3da7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ce9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e94: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ed3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ed4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4003: Unknown result type (might be due to invalid IL or missing references)
		//IL_4017: Unknown result type (might be due to invalid IL or missing references)
		//IL_401c: Unknown result type (might be due to invalid IL or missing references)
		//IL_401d: Unknown result type (might be due to invalid IL or missing references)
		//IL_403e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4045: Unknown result type (might be due to invalid IL or missing references)
		//IL_404f: Unknown result type (might be due to invalid IL or missing references)
		//IL_405b: Unknown result type (might be due to invalid IL or missing references)
		if (drawData.typeCache == 548 && drawData.tileFrameX / 54 > 6)
		{
			Main.tileBatch.Draw(TextureAssets.GlowMask[297].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 613)
		{
			Main.tileBatch.Draw(TextureAssets.GlowMask[298].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 614)
		{
			Main.tileBatch.Draw(TextureAssets.GlowMask[299].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 593)
		{
			Main.tileBatch.Draw(TextureAssets.GlowMask[295].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 594)
		{
			Main.tileBatch.Draw(TextureAssets.GlowMask[296].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 215 && drawData.tileFrameY < 36)
		{
			int num = 15;
			Color val = new Color(255, 255, 255, 0);
			switch (drawData.tileFrameX / 54)
			{
			case 5:
				val = new Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
				break;
			case 14:
				val = new Color(50, 50, 100, 20);
				break;
			case 15:
				val = new Color(255, 255, 255, 200);
				break;
			}
			Main.tileBatch.Draw(TextureAssets.Flames[num].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle((int)drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), val, _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 85)
		{
			float graveyardVisualIntensity = Main.GraveyardVisualIntensity;
			if (graveyardVisualIntensity > 0f)
			{
				ulong num2 = Main.TileFrameSeed ^ (ulong)(((long)tileX << 32) | (uint)tileY);
				TileFlameData tileFlameData = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
				if (num2 == 0L)
				{
					num2 = tileFlameData.flameSeed;
				}
				tileFlameData.flameSeed = num2;
				Vector2 val2 = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset;
				Rectangle sourceRectangle = new Rectangle(drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight);
				for (int i = 0; i < tileFlameData.flameCount; i++)
				{
					Color val3 = tileFlameData.flameColor * graveyardVisualIntensity;
					float num3 = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeXMin, tileFlameData.flameRangeXMax) * tileFlameData.flameRangeMultX;
					float num4 = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeYMin, tileFlameData.flameRangeYMax) * tileFlameData.flameRangeMultY;
					for (float num5 = 0f; num5 < 1f; num5 += 0.25f)
					{
						Main.tileBatch.Draw(tileFlameData.flameTexture, val2 + new Vector2(num3, num4) + Vector2.UnitX.RotatedBy(num5 * ((float)Math.PI * 2f)) * 2f, sourceRectangle, val3, _zero, 1f, drawData.tileSpriteEffect);
					}
					Main.tileBatch.Draw(tileFlameData.flameTexture, val2, sourceRectangle, Color.White * graveyardVisualIntensity, _zero, 1f, drawData.tileSpriteEffect);
				}
			}
		}
		if (drawData.typeCache == 356 && Main.sundialCooldown == 0)
		{
			Texture2D value = TextureAssets.GlowMask[325].Value;
			Rectangle sourceRectangle2 = new Rectangle((int)drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
			Color val4 = new Color(100, 100, 100, 0);
			int num6 = tileX - drawData.tileFrameX / 18;
			int num7 = tileY - drawData.tileFrameY / 18;
			ulong seed = Main.TileFrameSeed ^ (ulong)(((long)num6 << 32) | (uint)num7);
			for (int j = 0; j < 7; j++)
			{
				float num8 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
				float num9 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.35f;
				Main.tileBatch.Draw(value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num8, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num9) + screenOffset, sourceRectangle2, val4, _zero, 1f, drawData.tileSpriteEffect);
			}
		}
		if (drawData.typeCache == 663 && Main.moondialCooldown == 0)
		{
			Texture2D value2 = TextureAssets.GlowMask[335].Value;
			Rectangle sourceRectangle3 = new Rectangle((int)drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
			sourceRectangle3.Y += 54 * Main.moonPhase;
			Main.tileBatch.Draw(value2, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, sourceRectangle3, Color.White * ((float)(int)Main.mouseTextColor / 255f), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 286)
		{
			Main.tileBatch.Draw(TextureAssets.GlowSnail.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(75, 100, 255, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 582)
		{
			Main.tileBatch.Draw(TextureAssets.GlowMask[293].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(200, 100, 100, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 391)
		{
			Main.tileBatch.Draw(TextureAssets.GlowMask[131].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(250, 250, 250, 200), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 619)
		{
			Main.tileBatch.Draw(TextureAssets.GlowMask[300].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(75, 100, 255, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 270)
		{
			Main.tileBatch.Draw(TextureAssets.FireflyJar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 271)
		{
			Main.tileBatch.Draw(TextureAssets.LightningbugJar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 581)
		{
			Main.tileBatch.Draw(TextureAssets.GlowMask[291].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 316 || drawData.typeCache == 317 || drawData.typeCache == 318)
		{
			Main.tileBatch.Draw(TextureAssets.JellyfishBowl[drawData.typeCache - 316].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 149 && drawData.tileFrameX < 54)
		{
			Main.tileBatch.Draw(TextureAssets.XmasLight.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 300 || drawData.typeCache == 302 || drawData.typeCache == 303 || drawData.typeCache == 306)
		{
			int num10 = 9;
			if (drawData.typeCache == 302)
			{
				num10 = 10;
			}
			if (drawData.typeCache == 303)
			{
				num10 = 11;
			}
			if (drawData.typeCache == 306)
			{
				num10 = 12;
			}
			Main.tileBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle((int)drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		else if (Main.tileFlame[drawData.typeCache])
		{
			ulong seed2 = Main.TileFrameSeed ^ (ulong)(((long)tileX << 32) | (uint)tileY);
			int typeCache = drawData.typeCache;
			int num11 = 0;
			switch (typeCache)
			{
			case 4:
				num11 = 0;
				break;
			case 33:
			case 174:
				num11 = 1;
				break;
			case 100:
			case 173:
				num11 = 2;
				break;
			case 34:
				num11 = 3;
				break;
			case 93:
				num11 = 4;
				break;
			case 49:
				num11 = 5;
				break;
			case 372:
				num11 = 16;
				break;
			case 646:
				num11 = 17;
				break;
			case 98:
				num11 = 6;
				break;
			case 35:
				num11 = 7;
				break;
			case 42:
				num11 = 13;
				break;
			}
			switch (num11)
			{
			case 7:
			{
				for (int num94 = 0; num94 < 4; num94++)
				{
					float num95 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
					float num96 = (float)Utils.RandomInt(ref seed2, -10, 10) * 0.15f;
					num95 = 0f;
					num96 = 0f;
					Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num95, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num96) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
				}
				break;
			}
			case 1:
			{
				int num69 = Main.tile[tileX, tileY].frameY / 22;
				bool flag3 = num69 >= 44;
				switch (num69)
				{
				case 5:
				case 6:
				case 7:
				case 10:
				{
					for (int num88 = 0; num88 < 7; num88++)
					{
						float num89 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						float num90 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num89, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num90) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 8:
				{
					for (int num76 = 0; num76 < 7; num76++)
					{
						float num77 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						float num78 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num77, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num78) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 12:
				{
					for (int num79 = 0; num79 < 7; num79++)
					{
						float num80 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num81 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.15f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num80, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num81) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 14:
				{
					for (int num85 = 0; num85 < 8; num85++)
					{
						float num86 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num87 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num86, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num87) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 16:
				{
					for (int num82 = 0; num82 < 4; num82++)
					{
						float num83 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num84 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num83, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num84) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 27:
				case 28:
					Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
					break;
				case 43:
				{
					TileFlameData tileFlameData7 = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
					if (seed2 == 0L)
					{
						seed2 = tileFlameData7.flameSeed;
					}
					tileFlameData7.flameSeed = seed2;
					for (int num91 = 0; num91 < tileFlameData7.flameCount; num91++)
					{
						float num92 = (float)Utils.RandomInt(ref tileFlameData7.flameSeed, tileFlameData7.flameRangeXMin, tileFlameData7.flameRangeXMax) * tileFlameData7.flameRangeMultX;
						float num93 = (float)Utils.RandomInt(ref tileFlameData7.flameSeed, tileFlameData7.flameRangeYMin, tileFlameData7.flameRangeYMax) * tileFlameData7.flameRangeMultY;
						Main.tileBatch.Draw(tileFlameData7.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num92, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num93) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), tileFlameData7.flameColor, _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				default:
					if (flag3)
					{
						TileFlameData tileFlameData6 = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
						if (seed2 == 0L)
						{
							seed2 = tileFlameData6.flameSeed;
						}
						tileFlameData6.flameSeed = seed2;
						for (int num70 = 0; num70 < tileFlameData6.flameCount; num70++)
						{
							float num71 = (float)Utils.RandomInt(ref tileFlameData6.flameSeed, tileFlameData6.flameRangeXMin, tileFlameData6.flameRangeXMax) * tileFlameData6.flameRangeMultX;
							float num72 = (float)Utils.RandomInt(ref tileFlameData6.flameSeed, tileFlameData6.flameRangeYMin, tileFlameData6.flameRangeYMax) * tileFlameData6.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData6.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num71, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num72) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), tileFlameData6.flameColor, _zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else
					{
						for (int num73 = 0; num73 < 7; num73++)
						{
							float num74 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
							float num75 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
							Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num74, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num75) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), _zero, 1f, drawData.tileSpriteEffect);
						}
					}
					break;
				}
				break;
			}
			case 2:
			{
				int num97 = Main.tile[tileX, tileY].frameY / 36;
				bool flag4 = num97 >= 45;
				switch (num97)
				{
				case 3:
				{
					for (int num116 = 0; num116 < 3; num116++)
					{
						float num117 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.05f;
						float num118 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num117, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num118) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 6:
				{
					for (int num104 = 0; num104 < 5; num104++)
					{
						float num105 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num106 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num105, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num106) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 9:
				{
					for (int num107 = 0; num107 < 7; num107++)
					{
						float num108 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						float num109 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num108, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num109) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 11:
				{
					for (int num113 = 0; num113 < 7; num113++)
					{
						float num114 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num115 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.15f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num114, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num115) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 13:
				{
					for (int num110 = 0; num110 < 8; num110++)
					{
						float num111 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num112 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num111, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num112) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 28:
				case 29:
					Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
					break;
				case 44:
				{
					TileFlameData tileFlameData9 = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
					if (seed2 == 0L)
					{
						seed2 = tileFlameData9.flameSeed;
					}
					tileFlameData9.flameSeed = seed2;
					for (int num119 = 0; num119 < tileFlameData9.flameCount; num119++)
					{
						float num120 = (float)Utils.RandomInt(ref tileFlameData9.flameSeed, tileFlameData9.flameRangeXMin, tileFlameData9.flameRangeXMax) * tileFlameData9.flameRangeMultX;
						float num121 = (float)Utils.RandomInt(ref tileFlameData9.flameSeed, tileFlameData9.flameRangeYMin, tileFlameData9.flameRangeYMax) * tileFlameData9.flameRangeMultY;
						Main.tileBatch.Draw(tileFlameData9.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num120, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num121) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), tileFlameData9.flameColor, _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				default:
					if (flag4)
					{
						TileFlameData tileFlameData8 = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
						if (seed2 == 0L)
						{
							seed2 = tileFlameData8.flameSeed;
						}
						tileFlameData8.flameSeed = seed2;
						for (int num98 = 0; num98 < tileFlameData8.flameCount; num98++)
						{
							float num99 = (float)Utils.RandomInt(ref tileFlameData8.flameSeed, tileFlameData8.flameRangeXMin, tileFlameData8.flameRangeXMax) * tileFlameData8.flameRangeMultX;
							float num100 = (float)Utils.RandomInt(ref tileFlameData8.flameSeed, tileFlameData8.flameRangeYMin, tileFlameData8.flameRangeYMax) * tileFlameData8.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData8.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num99, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num100) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), tileFlameData8.flameColor, _zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else
					{
						for (int num101 = 0; num101 < 7; num101++)
						{
							float num102 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
							float num103 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
							Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num102, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num103) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), _zero, 1f, drawData.tileSpriteEffect);
						}
					}
					break;
				}
				break;
			}
			case 3:
			{
				int num14 = Main.tile[tileX, tileY].frameY / 54;
				if (Main.tile[tileX, tileY].frameX >= 108)
				{
					num14 += 37 * (Main.tile[tileX, tileY].frameX / 108);
				}
				bool flag = num14 >= 51;
				switch (num14)
				{
				case 8:
				{
					for (int n = 0; n < 7; n++)
					{
						float num19 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						float num20 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num19, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num20) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 9:
				{
					for (int num27 = 0; num27 < 3; num27++)
					{
						float num28 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.05f;
						float num29 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num28, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num29) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 11:
				{
					for (int num24 = 0; num24 < 7; num24++)
					{
						float num25 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						float num26 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num25, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num26) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 15:
				{
					for (int num36 = 0; num36 < 7; num36++)
					{
						float num37 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num38 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.15f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num37, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num38) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 17:
				case 20:
				{
					for (int num30 = 0; num30 < 7; num30++)
					{
						float num31 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						float num32 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num31, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num32) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 18:
				{
					for (int num21 = 0; num21 < 8; num21++)
					{
						float num22 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num23 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num22, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num23) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 34:
				case 35:
					Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
					break;
				case 50:
				{
					TileFlameData tileFlameData3 = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
					if (seed2 == 0L)
					{
						seed2 = tileFlameData3.flameSeed;
					}
					tileFlameData3.flameSeed = seed2;
					for (int num33 = 0; num33 < tileFlameData3.flameCount; num33++)
					{
						float num34 = (float)Utils.RandomInt(ref tileFlameData3.flameSeed, tileFlameData3.flameRangeXMin, tileFlameData3.flameRangeXMax) * tileFlameData3.flameRangeMultX;
						float num35 = (float)Utils.RandomInt(ref tileFlameData3.flameSeed, tileFlameData3.flameRangeYMin, tileFlameData3.flameRangeYMax) * tileFlameData3.flameRangeMultY;
						Main.tileBatch.Draw(tileFlameData3.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num34, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num35) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), tileFlameData3.flameColor, _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				default:
					if (flag)
					{
						TileFlameData tileFlameData2 = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
						if (seed2 == 0L)
						{
							seed2 = tileFlameData2.flameSeed;
						}
						tileFlameData2.flameSeed = seed2;
						for (int l = 0; l < tileFlameData2.flameCount; l++)
						{
							float num15 = (float)Utils.RandomInt(ref tileFlameData2.flameSeed, tileFlameData2.flameRangeXMin, tileFlameData2.flameRangeXMax) * tileFlameData2.flameRangeMultX;
							float num16 = (float)Utils.RandomInt(ref tileFlameData2.flameSeed, tileFlameData2.flameRangeYMin, tileFlameData2.flameRangeYMax) * tileFlameData2.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData2.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num15, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num16) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), tileFlameData2.flameColor, _zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else
					{
						for (int m = 0; m < 7; m++)
						{
							float num17 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
							float num18 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
							Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num17, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num18) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), _zero, 1f, drawData.tileSpriteEffect);
						}
					}
					break;
				}
				break;
			}
			case 4:
			{
				int num39 = Main.tile[tileX, tileY].frameY / 54;
				bool flag2 = num39 >= 45;
				switch (num39)
				{
				case 1:
				{
					for (int num66 = 0; num66 < 3; num66++)
					{
						float num67 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num68 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num67, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num68) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 2:
				case 4:
				{
					for (int num46 = 0; num46 < 7; num46++)
					{
						float num47 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						float num48 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num47, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num48) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 3:
				{
					for (int num54 = 0; num54 < 7; num54++)
					{
						float num55 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.2f;
						float num56 = (float)Utils.RandomInt(ref seed2, -20, 1) * 0.35f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num55, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num56) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 5:
				{
					for (int num63 = 0; num63 < 7; num63++)
					{
						float num64 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						float num65 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num64, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num65) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 9:
				{
					for (int num57 = 0; num57 < 7; num57++)
					{
						float num58 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num59 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.15f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num58, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num59) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 13:
				{
					for (int num49 = 0; num49 < 8; num49++)
					{
						float num50 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num51 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num50, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num51) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 12:
				{
					float num52 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.01f;
					float num53 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.01f;
					Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num52, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num53) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(Utils.RandomInt(ref seed2, 90, 111), Utils.RandomInt(ref seed2, 90, 111), Utils.RandomInt(ref seed2, 90, 111), 0), _zero, 1f, drawData.tileSpriteEffect);
					break;
				}
				case 28:
				case 29:
					Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
					break;
				case 44:
				{
					TileFlameData tileFlameData5 = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
					if (seed2 == 0L)
					{
						seed2 = tileFlameData5.flameSeed;
					}
					tileFlameData5.flameSeed = seed2;
					for (int num60 = 0; num60 < tileFlameData5.flameCount; num60++)
					{
						float num61 = (float)Utils.RandomInt(ref tileFlameData5.flameSeed, tileFlameData5.flameRangeXMin, tileFlameData5.flameRangeXMax) * tileFlameData5.flameRangeMultX;
						float num62 = (float)Utils.RandomInt(ref tileFlameData5.flameSeed, tileFlameData5.flameRangeYMin, tileFlameData5.flameRangeYMax) * tileFlameData5.flameRangeMultY;
						Main.tileBatch.Draw(tileFlameData5.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num61, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num62) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), tileFlameData5.flameColor, _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				default:
					if (flag2)
					{
						TileFlameData tileFlameData4 = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
						if (seed2 == 0L)
						{
							seed2 = tileFlameData4.flameSeed;
						}
						tileFlameData4.flameSeed = seed2;
						for (int num40 = 0; num40 < tileFlameData4.flameCount; num40++)
						{
							float num41 = (float)Utils.RandomInt(ref tileFlameData4.flameSeed, tileFlameData4.flameRangeXMin, tileFlameData4.flameRangeXMax) * tileFlameData4.flameRangeMultX;
							float num42 = (float)Utils.RandomInt(ref tileFlameData4.flameSeed, tileFlameData4.flameRangeYMin, tileFlameData4.flameRangeYMax) * tileFlameData4.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData4.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num41, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num42) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), tileFlameData4.flameColor, _zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else
					{
						for (int num43 = 0; num43 < 7; num43++)
						{
							float num44 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
							float num45 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
							Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num44, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num45) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), _zero, 1f, drawData.tileSpriteEffect);
						}
					}
					break;
				}
				break;
			}
			case 13:
			{
				int num122 = drawData.tileFrameY / 36;
				bool flag5 = num122 >= 51;
				switch (num122)
				{
				case 1:
				case 3:
				case 6:
				case 8:
				case 19:
				case 27:
				case 29:
				case 30:
				case 31:
				case 32:
				case 36:
				case 39:
				{
					for (int num135 = 0; num135 < 7; num135++)
					{
						float num136 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num137 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num136, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num137) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				case 2:
				case 16:
				case 25:
				{
					for (int num132 = 0; num132 < 7; num132++)
					{
						float num133 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num134 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.1f;
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num133, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num134) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), _zero, 1f, drawData.tileSpriteEffect);
					}
					break;
				}
				default:
					switch (num122)
					{
					case 29:
					{
						for (int num126 = 0; num126 < 7; num126++)
						{
							float num127 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
							float num128 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num127, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num128) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(25, 25, 25, 0), _zero, 1f, drawData.tileSpriteEffect);
						}
						break;
					}
					case 34:
					case 35:
						Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), _zero, 1f, drawData.tileSpriteEffect);
						break;
					case 50:
					{
						TileFlameData tileFlameData11 = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
						if (seed2 == 0L)
						{
							seed2 = tileFlameData11.flameSeed;
						}
						tileFlameData11.flameSeed = seed2;
						for (int num129 = 0; num129 < tileFlameData11.flameCount; num129++)
						{
							float num130 = (float)Utils.RandomInt(ref tileFlameData11.flameSeed, tileFlameData11.flameRangeXMin, tileFlameData11.flameRangeXMax) * tileFlameData11.flameRangeMultX;
							float num131 = (float)Utils.RandomInt(ref tileFlameData11.flameSeed, tileFlameData11.flameRangeYMin, tileFlameData11.flameRangeYMax) * tileFlameData11.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData11.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num130, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num131) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), tileFlameData11.flameColor, _zero, 1f, drawData.tileSpriteEffect);
						}
						break;
					}
					default:
						if (flag5)
						{
							TileFlameData tileFlameData10 = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
							if (seed2 == 0L)
							{
								seed2 = tileFlameData10.flameSeed;
							}
							tileFlameData10.flameSeed = seed2;
							for (int num123 = 0; num123 < tileFlameData10.flameCount; num123++)
							{
								float num124 = (float)Utils.RandomInt(ref tileFlameData10.flameSeed, tileFlameData10.flameRangeXMin, tileFlameData10.flameRangeXMax) * tileFlameData10.flameRangeMultX;
								float num125 = (float)Utils.RandomInt(ref tileFlameData10.flameSeed, tileFlameData10.flameRangeYMin, tileFlameData10.flameRangeYMax) * tileFlameData10.flameRangeMultY;
								Main.tileBatch.Draw(tileFlameData10.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num124, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num125) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), tileFlameData10.flameColor, _zero, 1f, drawData.tileSpriteEffect);
							}
						}
						break;
					}
					break;
				}
				break;
			}
			default:
			{
				Color val5 = new Color(100, 100, 100, 0);
				if (drawData.tileCache.type == 4)
				{
					switch (drawData.tileCache.frameY / 22)
					{
					case 14:
						val5 = new Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
						break;
					case 22:
						val5 = new Color(50, 50, 100, 20);
						break;
					case 23:
						val5 = new Color(255, 255, 255, 200);
						break;
					}
				}
				if (drawData.tileCache.type == 646)
				{
					val5 = new Color(100, 100, 100, 150);
				}
				for (int k = 0; k < 7; k++)
				{
					float num12 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
					float num13 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
					Main.tileBatch.Draw(TextureAssets.Flames[num11].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num12, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num13) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), val5, _zero, 1f, drawData.tileSpriteEffect);
				}
				break;
			}
			}
		}
		if (drawData.typeCache == 144)
		{
			Main.tileBatch.Draw(TextureAssets.Timer.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache == 237)
		{
			Main.tileBatch.Draw(TextureAssets.SunAltar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(Main.mouseTextColor / 2, Main.mouseTextColor / 2, Main.mouseTextColor / 2, 0), _zero, 1f, drawData.tileSpriteEffect);
		}
		if (drawData.typeCache != 658 || drawData.tileFrameX % 36 != 0 || drawData.tileFrameY % 54 != 0)
		{
			return;
		}
		int num138 = drawData.tileFrameY / 54;
		if (num138 != 2)
		{
			Texture2D value3 = TextureAssets.GlowMask[334].Value;
			Vector2 val6 = new Vector2(0f, -10f);
			Vector2 position = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - (float)drawData.tileWidth / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset + val6;
			Rectangle sourceRectangle4 = value3.Frame();
			Color val7 = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 0);
			if (num138 == 0)
			{
				val7 *= 0.75f;
			}
			Main.tileBatch.Draw(value3, position, sourceRectangle4, val7, _zero, 1f, drawData.tileSpriteEffect);
		}
	}

	private int GetPalmTreeVariant(int x, int y)
	{
		int num = -1;
		if (Main.tile[x, y].active() && Main.tile[x, y].type == 53)
		{
			num = 0;
		}
		if (Main.tile[x, y].active() && Main.tile[x, y].type == 234)
		{
			num = 1;
		}
		if (Main.tile[x, y].active() && Main.tile[x, y].type == 116)
		{
			num = 2;
		}
		if (Main.tile[x, y].active() && Main.tile[x, y].type == 112)
		{
			num = 3;
		}
		if (WorldGen.IsPalmOasisTree(x))
		{
			num += 4;
		}
		return num;
	}

	private void DrawSingleTile_SlicedBlock(Vector2 normalTilePosition, int tileX, int tileY, TileDrawInfo drawData)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		Color val = default;
		Vector2 origin = default;
		Rectangle val3 = default;
		Vector3 tileLight = default;
		Vector2 position = default;
		if (drawData.tileLight.R > _highQualityLightingRequirement.R || drawData.tileLight.G > _highQualityLightingRequirement.G || drawData.tileLight.B > _highQualityLightingRequirement.B)
		{
			Vector3[] slices = drawData.colorSlices;
			Lighting.GetColor9Slice(tileX, tileY, ref slices);
			Vector3 val2 = drawData.tileLight.ToVector3();
			Vector3 tint = drawData.colorTint.ToVector3();
			if (drawData.tileCache.fullbrightBlock())
			{
				slices = _glowPaintColorSlices;
			}
			for (int i = 0; i < 9; i++)
			{
				val3.X = 0;
				val3.Y = 0;
				val3.Width = 4;
				val3.Height = 4;
				switch (i)
				{
				case 1:
					val3.Width = 8;
					val3.X = 4;
					break;
				case 2:
					val3.X = 12;
					break;
				case 3:
					val3.Height = 8;
					val3.Y = 4;
					break;
				case 4:
					val3.Width = 8;
					val3.Height = 8;
					val3.X = 4;
					val3.Y = 4;
					break;
				case 5:
					val3.X = 12;
					val3.Y = 4;
					val3.Height = 8;
					break;
				case 6:
					val3.Y = 12;
					break;
				case 7:
					val3.Width = 8;
					val3.Height = 4;
					val3.X = 4;
					val3.Y = 12;
					break;
				case 8:
					val3.X = 12;
					val3.Y = 12;
					break;
				}
				tileLight.X = (slices[i].X + val2.X) * 0.5f;
				tileLight.Y = (slices[i].Y + val2.Y) * 0.5f;
				tileLight.Z = (slices[i].Z + val2.Z) * 0.5f;
				GetFinalLight(drawData.tileCache, drawData.typeCache, ref tileLight, ref tint);
				position.X = normalTilePosition.X + (float)val3.X;
				position.Y = normalTilePosition.Y + (float)val3.Y;
				val3.X += drawData.tileFrameX + drawData.addFrX;
				val3.Y += drawData.tileFrameY + drawData.addFrY;
				int num = (int)(tileLight.X * 255f);
				int num2 = (int)(tileLight.Y * 255f);
				int num3 = (int)(tileLight.Z * 255f);
				if (num > 255)
				{
					num = 255;
				}
				if (num2 > 255)
				{
					num2 = 255;
				}
				if (num3 > 255)
				{
					num3 = 255;
				}
				num3 <<= 16;
				num2 <<= 8;
				val.PackedValue = (uint)(num | num2 | num3 | -16777216);
				Main.tileBatch.Draw(drawData.drawTexture, position, val3, val, origin, 1f, drawData.tileSpriteEffect);
			}
		}
		else if (drawData.tileLight.R > _mediumQualityLightingRequirement.R || drawData.tileLight.G > _mediumQualityLightingRequirement.G || drawData.tileLight.B > _mediumQualityLightingRequirement.B)
		{
			Vector3[] slices2 = drawData.colorSlices;
			Lighting.GetColor4Slice(tileX, tileY, ref slices2);
			Vector3 val4 = drawData.tileLight.ToVector3();
			Vector3 tint2 = drawData.colorTint.ToVector3();
			if (drawData.tileCache.fullbrightBlock())
			{
				slices2 = _glowPaintColorSlices;
			}
			val3.Width = 8;
			val3.Height = 8;
			for (int j = 0; j < 4; j++)
			{
				val3.X = 0;
				val3.Y = 0;
				switch (j)
				{
				case 1:
					val3.X = 8;
					break;
				case 2:
					val3.Y = 8;
					break;
				case 3:
					val3.X = 8;
					val3.Y = 8;
					break;
				}
				tileLight.X = (slices2[j].X + val4.X) * 0.5f;
				tileLight.Y = (slices2[j].Y + val4.Y) * 0.5f;
				tileLight.Z = (slices2[j].Z + val4.Z) * 0.5f;
				GetFinalLight(drawData.tileCache, drawData.typeCache, ref tileLight, ref tint2);
				position.X = normalTilePosition.X + (float)val3.X;
				position.Y = normalTilePosition.Y + (float)val3.Y;
				val3.X += drawData.tileFrameX + drawData.addFrX;
				val3.Y += drawData.tileFrameY + drawData.addFrY;
				int num4 = (int)(tileLight.X * 255f);
				int num5 = (int)(tileLight.Y * 255f);
				int num6 = (int)(tileLight.Z * 255f);
				if (num4 > 255)
				{
					num4 = 255;
				}
				if (num5 > 255)
				{
					num5 = 255;
				}
				if (num6 > 255)
				{
					num6 = 255;
				}
				num6 <<= 16;
				num5 <<= 8;
				val.PackedValue = (uint)(num4 | num5 | num6 | -16777216);
				Main.tileBatch.Draw(drawData.drawTexture, position, val3, val, origin, 1f, drawData.tileSpriteEffect);
			}
		}
		else
		{
			Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), drawData.finalColor, _zero, 1f, drawData.tileSpriteEffect);
		}
	}

	private void DrawXmasTree(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
	{
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		int num = 2;
		if (tileY - drawData.tileFrameY > 0 && drawData.tileFrameY == 7 && Main.tile[tileX, tileY - drawData.tileFrameY] != null)
		{
			drawData.tileTop -= 16 * drawData.tileFrameY;
			drawData.tileFrameX = Main.tile[tileX, tileY - drawData.tileFrameY].frameX;
			drawData.tileFrameY = Main.tile[tileX, tileY - drawData.tileFrameY].frameY;
		}
		if (drawData.tileFrameX < 10)
		{
			return;
		}
		int num2 = 0;
		if ((drawData.tileFrameY & 1) == 1)
		{
			num2++;
		}
		if ((drawData.tileFrameY & 2) == 2)
		{
			num2 += 2;
		}
		if ((drawData.tileFrameY & 4) == 4)
		{
			num2 += 4;
		}
		int num3 = 0;
		if ((drawData.tileFrameY & 8) == 8)
		{
			num3++;
		}
		if ((drawData.tileFrameY & 0x10) == 16)
		{
			num3 += 2;
		}
		if ((drawData.tileFrameY & 0x20) == 32)
		{
			num3 += 4;
		}
		int num4 = 0;
		if ((drawData.tileFrameY & 0x40) == 64)
		{
			num4++;
		}
		if ((drawData.tileFrameY & 0x80) == 128)
		{
			num4 += 2;
		}
		if ((drawData.tileFrameY & 0x100) == 256)
		{
			num4 += 4;
		}
		if ((drawData.tileFrameY & 0x200) == 512)
		{
			num4 += 8;
		}
		int num5 = 0;
		if ((drawData.tileFrameY & 0x400) == 1024)
		{
			num5++;
		}
		if ((drawData.tileFrameY & 0x800) == 2048)
		{
			num5 += 2;
		}
		if ((drawData.tileFrameY & 0x1000) == 4096)
		{
			num5 += 4;
		}
		if ((drawData.tileFrameY & 0x2000) == 8192)
		{
			num5 += 8;
		}
		Color color = Lighting.GetColor(tileX + 1, tileY - 3);
		Main.tileBatch.Draw(TextureAssets.XmasTree[0].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num)) + screenOffset, new Rectangle(0, 0, 64, 128), color, _zero, 1f, (SpriteEffects)0);
		if (num2 > 0)
		{
			num2--;
			Color val = color;
			if (num2 != 3)
			{
				val = new Color(255, 255, 255, 255);
			}
			Main.tileBatch.Draw(TextureAssets.XmasTree[3].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num)) + screenOffset, new Rectangle(66 * num2, 0, 64, 128), val, _zero, 1f, (SpriteEffects)0);
		}
		if (num3 > 0)
		{
			num3--;
			Main.tileBatch.Draw(TextureAssets.XmasTree[1].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num)) + screenOffset, new Rectangle(66 * num3, 0, 64, 128), color, _zero, 1f, (SpriteEffects)0);
		}
		if (num4 > 0)
		{
			num4--;
			Main.tileBatch.Draw(TextureAssets.XmasTree[2].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num)) + screenOffset, new Rectangle(66 * num4, 0, 64, 128), color, _zero, 1f, (SpriteEffects)0);
		}
		if (num5 > 0)
		{
			num5--;
			Main.tileBatch.Draw(TextureAssets.XmasTree[4].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num)) + screenOffset, new Rectangle(66 * num5, 130 * Main.tileFrame[171], 64, 128), new Color(255, 255, 255, 255), _zero, 1f, (SpriteEffects)0);
		}
	}

	private void DrawTile_BackRope(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.InWorld(tileX, tileY, 1))
		{
			return;
		}
		int topRopeY = tileX;
		int bottomRopeY = tileY;
		if (WorldGen.IsRope(tileX, tileY, out topRopeY, out bottomRopeY))
		{
			Tile tile = Main.tile[tileX, topRopeY];
			if (tile != null)
			{
				int num = (tileY + tileX) % 3 * 18;
				Texture2D tileDrawTexture = GetTileDrawTexture(tile, tileX, tileY);
				Main.tileBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)(tileY * 16 - (int)screenPosition.Y)) + screenOffset, new Rectangle(90, num, 16, 16), drawData.tileLight, default, 1f, drawData.tileSpriteEffect);
			}
		}
	}

	private void DrawTile_MinecartTrack(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		drawData.tileLight = GetFinalLight(drawData.tileCache, drawData.typeCache, drawData.tileLight, drawData.colorTint);
		Minecart.TrackColors(tileX, tileY, drawData.tileCache, out var frontColor, out var backColor);
		drawData.drawTexture = GetTileDrawTexture(drawData.tileCache.type, frontColor);
		Texture2D tileDrawTexture = GetTileDrawTexture(drawData.tileCache.type, backColor);
		DrawTile_BackRope(screenPosition, screenOffset, tileX, tileY, drawData);
		if (drawData.tileFrameY != -1)
		{
			Main.tileBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)(tileY * 16 - (int)screenPosition.Y)) + screenOffset, Minecart.GetSourceRect(drawData.tileFrameY, Main.tileFrame[314]), drawData.tileLight, default, 1f, drawData.tileSpriteEffect);
		}
		Main.tileBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)(tileY * 16 - (int)screenPosition.Y)) + screenOffset, Minecart.GetSourceRect(drawData.tileFrameX, Main.tileFrame[314]), drawData.tileLight, default, 1f, drawData.tileSpriteEffect);
		if (Minecart.DrawLeftDecoration(drawData.tileFrameY))
		{
			Main.tileBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, Minecart.GetSourceRect(36), drawData.tileLight, default, 1f, drawData.tileSpriteEffect);
		}
		if (Minecart.DrawLeftDecoration(drawData.tileFrameX))
		{
			Main.tileBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, Minecart.GetSourceRect(36), drawData.tileLight, default, 1f, drawData.tileSpriteEffect);
		}
		if (Minecart.DrawRightDecoration(drawData.tileFrameY))
		{
			Main.tileBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, Minecart.GetSourceRect(37, Main.tileFrame[314]), drawData.tileLight, default, 1f, drawData.tileSpriteEffect);
		}
		if (Minecart.DrawRightDecoration(drawData.tileFrameX))
		{
			Main.tileBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, Minecart.GetSourceRect(37), drawData.tileLight, default, 1f, drawData.tileSpriteEffect);
		}
		if (Minecart.DrawBumper(drawData.tileFrameX))
		{
			Main.tileBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY - 1) * 16 - (int)screenPosition.Y)) + screenOffset, Minecart.GetSourceRect(39), drawData.tileLight, default, 1f, drawData.tileSpriteEffect);
		}
		else if (Minecart.DrawBouncyBumper(drawData.tileFrameX))
		{
			Main.tileBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY - 1) * 16 - (int)screenPosition.Y)) + screenOffset, Minecart.GetSourceRect(38), drawData.tileLight, default, 1f, drawData.tileSpriteEffect);
		}
	}

	private void DrawTile_LiquidBehindTile(bool solidLayer, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, Tile tileCache)
	{
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		Tile tile = Main.tile[tileX + 1, tileY];
		if (tile != null && tile.type != 379)
		{
			num = tile.liquid;
			num2 = tile.liquidType();
		}
		Tile tile2 = Main.tile[tileX - 1, tileY];
		if (tile2 != null && tile2.type != 379)
		{
			num3 = tile2.liquid;
			num4 = tile2.liquidType();
		}
		Tile tile3 = Main.tile[tileX, tileY - 1];
		if (tile3 != null && tile3.type != 379)
		{
			num5 = tile3.liquid;
			num6 = tile3.liquidType();
		}
		Tile tile4 = Main.tile[tileX, tileY + 1];
		if (tile4 != null && tile4.type != 379)
		{
			num7 = tile4.liquid;
			num8 = tile4.liquidType();
		}
		if (DebugOptions.hideWater || !tileCache.active() || tileCache.inActive() || _tileSolidTop[tileCache.type] || (tileCache.halfBrick() && (num3 > 160 || num > 160) && Main.instance.waterfallManager.CheckForWaterfall(tileX, tileY)) || (TileID.Sets.BlocksWaterDrawingBehindSelf[tileCache.type] && tileCache.slope() == 0))
		{
			return;
		}
		int num9 = 0;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		int num10 = 0;
		bool flag6 = false;
		int num11 = tileCache.slope();
		int num12 = tileCache.blockType();
		if (tileCache.type == 379 && tileCache.liquid > 0)
		{
			return;
		}
		if (tileCache.type == 546 && tileCache.liquid > 0)
		{
			flag5 = true;
			flag4 = true;
			flag = true;
			flag2 = true;
			switch (tileCache.liquidType())
			{
			case 0:
				flag6 = true;
				break;
			case 1:
				num10 = 1;
				break;
			case 2:
				num10 = 11;
				break;
			case 3:
				num10 = 14;
				break;
			}
			num9 = tileCache.liquid;
		}
		else
		{
			if (tileCache.liquid > 0 && num12 != 0 && (num12 != 1 || tileCache.liquid > 160))
			{
				flag5 = true;
				switch (tileCache.liquidType())
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num10 = 1;
					break;
				case 2:
					num10 = 11;
					break;
				case 3:
					num10 = 14;
					break;
				}
				if (tileCache.liquid > num9)
				{
					num9 = tileCache.liquid;
				}
			}
			if (num3 > 0 && num11 != 1 && num11 != 3)
			{
				flag = true;
				switch (num4)
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num10 = 1;
					break;
				case 2:
					num10 = 11;
					break;
				case 3:
					num10 = 14;
					break;
				}
				if (num3 > num9)
				{
					num9 = num3;
				}
			}
			if (num > 0 && num11 != 2 && num11 != 4)
			{
				flag2 = true;
				switch (num2)
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num10 = 1;
					break;
				case 2:
					num10 = 11;
					break;
				case 3:
					num10 = 14;
					break;
				}
				if (num > num9)
				{
					num9 = num;
				}
			}
			if (num5 > 0 && num11 != 3 && num11 != 4)
			{
				flag3 = true;
				switch (num6)
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num10 = 1;
					break;
				case 2:
					num10 = 11;
					break;
				case 3:
					num10 = 14;
					break;
				}
			}
			if (num7 > 0 && num11 != 1 && num11 != 2)
			{
				if (num7 > 240)
				{
					flag4 = true;
				}
				switch (num8)
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num10 = 1;
					break;
				case 2:
					num10 = 11;
					break;
				case 3:
					num10 = 14;
					break;
				}
			}
		}
		if (!flag3 && !flag4 && !flag && !flag2 && !flag5)
		{
			return;
		}
		if (waterStyleOverride != -1)
		{
			Main.waterStyle = waterStyleOverride;
		}
		if (num10 == 0)
		{
			num10 = Main.waterStyle;
		}
		Lighting.GetCornerColors(tileX, tileY, out var vertices);
		Vector2 val = new Vector2((float)(tileX * 16), (float)(tileY * 16));
		Rectangle liquidSize = new Rectangle(0, 4, 16, 16);
		if (flag4 && (flag | flag2))
		{
			flag = true;
			flag2 = true;
		}
		if (tileCache.active() && (Main.tileSolidTop[tileCache.type] || !Main.tileSolid[tileCache.type]))
		{
			return;
		}
		if ((!flag3 || !(flag | flag2)) && !(flag4 & flag3))
		{
			if (flag3)
			{
				liquidSize = new Rectangle(0, 4, 16, 4);
				if (tileCache.halfBrick() || tileCache.slope() != 0)
				{
					liquidSize = new Rectangle(0, 4, 16, 12);
				}
			}
			else if (flag4 && !flag && !flag2)
			{
				val = new Vector2((float)(tileX * 16), (float)(tileY * 16 + 12));
				liquidSize = new Rectangle(0, 4, 16, 4);
			}
			else
			{
				float num13 = (float)(256 - num9) / 32f;
				int num14 = 4;
				if (num5 == 0 && (num12 != 0 || !WorldGen.SolidTile(tileX, tileY - 1)))
				{
					num14 = 0;
				}
				int num15 = (int)num13 * 2;
				if (tileCache.slope() != 0)
				{
					val = new Vector2((float)(tileX * 16), (float)(tileY * 16 + num15));
					liquidSize = new Rectangle(0, num15, 16, 16 - num15);
				}
				else if ((flag & flag2) || tileCache.halfBrick())
				{
					val = new Vector2((float)(tileX * 16), (float)(tileY * 16 + num15));
					liquidSize = new Rectangle(0, num14, 16, 16 - num15);
				}
				else if (flag)
				{
					val = new Vector2((float)(tileX * 16), (float)(tileY * 16 + num15));
					liquidSize = new Rectangle(0, num14, 4, 16 - num15);
				}
				else
				{
					val = new Vector2((float)(tileX * 16 + 12), (float)(tileY * 16 + num15));
					liquidSize = new Rectangle(0, num14, 4, 16 - num15);
				}
			}
		}
		Vector2 position = val - screenPosition + screenOffset;
		float num16 = 0.5f;
		switch (num10)
		{
		case 1:
			num16 = Main.player[Main.myPlayer].lavaOpacity;
			break;
		case 11:
			num16 = Math.Max(num16 * 1.7f, 1f);
			break;
		}
		if ((num10 != 1 || !(Main.player[Main.myPlayer].lavaOpacity < 1f)) && ((double)tileY <= Main.worldSurface || num16 > 1f))
		{
			num16 = 1f;
			if (tileCache.wall == 21)
			{
				num16 = 0.9f;
			}
			else if (tileCache.wall > 0)
			{
				num16 = 0.6f;
			}
		}
		if (tileCache.halfBrick() && num5 > 0 && tileCache.wall > 0)
		{
			num16 = 0f;
		}
		if (num11 == 4 && num3 == 0 && !WorldGen.SolidTile(tileX - 1, tileY))
		{
			num16 = 0f;
		}
		if (num11 == 3 && num == 0 && !WorldGen.SolidTile(tileX + 1, tileY))
		{
			num16 = 0f;
		}
		ref Color bottomLeftColor = ref vertices.BottomLeftColor;
		bottomLeftColor *= num16;
		ref Color bottomRightColor = ref vertices.BottomRightColor;
		bottomRightColor *= num16;
		ref Color topLeftColor = ref vertices.TopLeftColor;
		topLeftColor *= num16;
		ref Color topRightColor = ref vertices.TopRightColor;
		topRightColor *= num16;
		if (tileCache.halfBrick() && num5 > 0 && (double)tileY > Main.worldSurface)
		{
			ref Color topLeftColor2 = ref vertices.TopLeftColor;
			topLeftColor2 *= 0f;
			ref Color topRightColor2 = ref vertices.TopRightColor;
			topRightColor2 *= 0f;
		}
		bool flag7 = false;
		if (flag6)
		{
			for (int i = 0; i < 15; i++)
			{
				if (Main.IsLiquidStyleWater(i) && Main.liquidAlpha[i] > 0f && i != num10)
				{
					DrawPartialLiquid(!solidLayer, tileCache, ref position, ref liquidSize, i, ref vertices);
					flag7 = true;
					break;
				}
			}
		}
		VertexColors colors = vertices;
		float num17 = (flag7 ? Main.liquidAlpha[num10] : 1f);
		ref Color bottomLeftColor2 = ref colors.BottomLeftColor;
		bottomLeftColor2 *= num17;
		ref Color bottomRightColor2 = ref colors.BottomRightColor;
		bottomRightColor2 *= num17;
		ref Color topLeftColor3 = ref colors.TopLeftColor;
		topLeftColor3 *= num17;
		ref Color topRightColor3 = ref colors.TopRightColor;
		topRightColor3 *= num17;
		if (num10 == 14)
		{
			LiquidRenderer.SetShimmerVertexColors(ref colors, solidLayer ? 0.75f : 1f, tileX, tileY);
		}
		DrawPartialLiquid(!solidLayer, tileCache, ref position, ref liquidSize, num10, ref colors);
	}

	private void CacheSpecialDraws_Part1(int tileX, int tileY, int tileType, int drawDataTileFrameX, int drawDataTileFrameY, bool skipDraw)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		if (tileType == 395)
		{
			Point val = new Point(tileX, tileY);
			if (drawDataTileFrameX % 36 != 0)
			{
				val.X--;
			}
			if (drawDataTileFrameY % 36 != 0)
			{
				val.Y--;
			}
			if (!_itemFrameTileEntityPositions.ContainsKey(val))
			{
				_itemFrameTileEntityPositions[val] = TileEntityType<TEItemFrame>.Find(val.X, val.Y);
				if (_itemFrameTileEntityPositions[val] != -1)
				{
					AddSpecialLegacyPoint(val);
				}
			}
		}
		if (tileType == 698)
		{
			Point val2 = new Point(tileX, tileY);
			if (drawDataTileFrameX % 18 != 0)
			{
				val2.X--;
			}
			if (drawDataTileFrameY % 36 != 0)
			{
				val2.Y--;
			}
			if (!_deadCellsDisplayJarTileEntityPositions.ContainsKey(val2))
			{
				_deadCellsDisplayJarTileEntityPositions[val2] = TileEntityType<TEDeadCellsDisplayJar>.Find(val2.X, val2.Y);
				if (_deadCellsDisplayJarTileEntityPositions[val2] != -1)
				{
					AddSpecialLegacyPoint(val2);
				}
			}
		}
		if (tileType == 520)
		{
			Point val3 = new Point(tileX, tileY);
			if (!_foodPlatterTileEntityPositions.ContainsKey(val3))
			{
				_foodPlatterTileEntityPositions[val3] = TileEntityType<TEFoodPlatter>.Find(val3.X, val3.Y);
				if (_foodPlatterTileEntityPositions[val3] != -1)
				{
					AddSpecialLegacyPoint(val3);
				}
			}
		}
		if (tileType == 471)
		{
			Point val4 = new Point(tileX, tileY);
			val4.X -= drawDataTileFrameX % 54 / 18;
			val4.Y -= drawDataTileFrameY % 54 / 18;
			if (!_weaponRackTileEntityPositions.ContainsKey(val4))
			{
				_weaponRackTileEntityPositions[val4] = TileEntityType<TEWeaponsRack>.Find(val4.X, val4.Y);
				if (_weaponRackTileEntityPositions[val4] != -1)
				{
					AddSpecialLegacyPoint(val4);
				}
			}
		}
		if (tileType == 470)
		{
			Point val5 = new Point(tileX, tileY);
			val5.X -= drawDataTileFrameX % 36 / 18;
			val5.Y -= drawDataTileFrameY % 54 / 18;
			if (!_displayDollTileEntityPositions.ContainsKey(val5))
			{
				_displayDollTileEntityPositions[val5] = TileEntityType<TEDisplayDoll>.Find(val5.X, val5.Y);
				if (_displayDollTileEntityPositions[val5] != -1)
				{
					AddSpecialLegacyPoint(val5);
				}
			}
		}
		if (tileType == 475)
		{
			Point val6 = new Point(tileX, tileY);
			val6.X -= drawDataTileFrameX % 54 / 18;
			val6.Y -= drawDataTileFrameY % 72 / 18;
			if (!_hatRackTileEntityPositions.ContainsKey(val6))
			{
				_hatRackTileEntityPositions[val6] = TileEntityType<TEHatRack>.Find(val6.X, val6.Y);
				if (_hatRackTileEntityPositions[val6] != -1)
				{
					AddSpecialLegacyPoint(val6);
				}
			}
		}
		if (tileType == 620 && drawDataTileFrameX == 0 && drawDataTileFrameY == 0)
		{
			AddSpecialLegacyPoint(tileX, tileY);
		}
		if (tileType == 237 && drawDataTileFrameX == 18 && drawDataTileFrameY == 0)
		{
			AddSpecialLegacyPoint(tileX, tileY);
		}
		if (skipDraw)
		{
			return;
		}
		switch (tileType)
		{
		case 323:
			if (drawDataTileFrameX <= 132 && drawDataTileFrameX >= 88)
			{
				AddSpecialPoint(tileX, tileY, TileCounterType.Tree);
			}
			break;
		case 5:
		case 583:
		case 584:
		case 585:
		case 586:
		case 587:
		case 588:
		case 589:
		case 596:
		case 616:
		case 634:
			if (drawDataTileFrameY >= 198 && drawDataTileFrameX >= 22)
			{
				AddSpecialPoint(tileX, tileY, TileCounterType.Tree);
			}
			break;
		}
	}

	private void CacheSpecialDraws_Part2(int tileX, int tileY, TileDrawInfo drawData)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		if (TileID.Sets.BasicChest[drawData.typeCache])
		{
			Point val = new Point(tileX, tileY);
			if (drawData.tileFrameX % 36 != 0)
			{
				val.X--;
			}
			if (drawData.tileFrameY % 36 != 0)
			{
				val.Y--;
			}
			if (!_chestPositions.ContainsKey(val))
			{
				_chestPositions[val] = Chest.FindChest(val.X, val.Y);
			}
			int num = drawData.tileFrameX / 18;
			int num2 = drawData.tileFrameY / 18;
			int num3 = drawData.tileFrameX / 36;
			int num4 = num * 18;
			drawData.addFrX = num4 - drawData.tileFrameX;
			int num5 = num2 * 18;
			if (_chestPositions[val] != -1)
			{
				int frame = Main.chest[_chestPositions[val]].frame;
				if (frame == 1)
				{
					num5 += 38;
				}
				if (frame == 2)
				{
					num5 += 76;
				}
			}
			drawData.addFrY = num5 - drawData.tileFrameY;
			if (num2 != 0)
			{
				drawData.tileHeight = 18;
			}
			if (drawData.typeCache == 21 && (num3 == 48 || num3 == 49))
			{
				drawData.glowSourceRect = new Rectangle(16 * (num % 2), drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
			}
		}
		if (drawData.typeCache != 378)
		{
			return;
		}
		Point val2 = new Point(tileX, tileY);
		if (drawData.tileFrameX % 36 != 0)
		{
			val2.X--;
		}
		if (drawData.tileFrameY % 54 != 0)
		{
			val2.Y -= drawData.tileFrameY / 18;
		}
		if (!_trainingDummyTileEntityPositions.ContainsKey(val2))
		{
			_trainingDummyTileEntityPositions[val2] = TileEntityType<TETrainingDummy>.Find(val2.X, val2.Y);
		}
		if (_trainingDummyTileEntityPositions[val2] != -1 && TileEntity.TryGet<TETrainingDummy>(_trainingDummyTileEntityPositions[val2], out var result))
		{
			int npc = result.npc;
			if (npc != -1)
			{
				int num6 = Main.npc[npc].frame.Y / 55;
				num6 *= 54;
				num6 += drawData.tileFrameY;
				drawData.addFrY = num6 - drawData.tileFrameY;
			}
		}
	}

	private static Color GetFinalLight(Tile tileCache, ushort typeCache, Color tileLight, Color tint)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)((float)(tileLight.R * tint.R) / 255f);
		int num2 = (int)((float)(tileLight.G * tint.G) / 255f);
		int num3 = (int)((float)(tileLight.B * tint.B) / 255f);
		if (num > 255)
		{
			num = 255;
		}
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		num3 <<= 16;
		num2 <<= 8;
		tileLight.PackedValue = (uint)(num | num2 | num3 | -16777216);
		if (tileCache.fullbrightBlock())
		{
			tileLight = Color.White;
		}
		if (tileCache.inActive())
		{
			tileLight = tileCache.actColor(tileLight);
		}
		else if (ShouldTileShine(typeCache, tileCache.frameX))
		{
			tileLight = Main.shine(tileLight, typeCache);
		}
		return tileLight;
	}

	private static void GetFinalLight(Tile tileCache, ushort typeCache, ref Vector3 tileLight, ref Vector3 tint)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		tileLight *= tint;
		if (tileCache.inActive())
		{
			tileCache.actColor(ref tileLight);
		}
		else if (ShouldTileShine(typeCache, tileCache.frameX))
		{
			Main.shine(ref tileLight, typeCache);
		}
	}

	private static bool ShouldTileShine(ushort type, short frameX)
	{
		if ((Main.shimmerAlpha > 0f && Main.tileSolid[type]) || type == 165 || type == 693 || type == 694)
		{
			return true;
		}
		if (!Main.tileShine2[type])
		{
			return false;
		}
		switch (type)
		{
		case 467:
		case 468:
			if (frameX >= 144)
			{
				return frameX < 178;
			}
			return false;
		case 21:
		case 441:
			if (frameX >= 36)
			{
				return frameX < 178;
			}
			return false;
		default:
			return true;
		}
	}

	private static bool IsTileDangerous(Player localPlayer, int tileX, int tileY, Tile tileCache, ushort typeCache)
	{
		bool flag = false || typeCache == 135 || typeCache == 137 || TileID.Sets.Boulders[typeCache] || typeCache == 141 || typeCache == 210 || typeCache == 442 || typeCache == 443 || typeCache == 444 || typeCache == 411 || typeCache == 485 || typeCache == 85 || typeCache == 654 || (typeCache == 314 && Minecart.IsPressurePlate(tileCache));
		flag |= Main.getGoodWorld && typeCache == 230;
		flag |= typeCache == 58;
		if (tileCache.slope() == 0 && !tileCache.inActive())
		{
			flag = flag || IsTileDangerous_CheckTouchDamage(localPlayer, typeCache, tileX, tileY) || typeCache == 483 || typeCache == 482 || typeCache == 481 || typeCache == 51 || typeCache == 229 || IsTileDangerous_CheckHot(localPlayer, typeCache, tileX, tileY);
			if (!localPlayer.iceSkate)
			{
				flag = flag || typeCache == 162;
			}
		}
		return flag;
	}

	private static bool IsTileDangerous_CheckTouchDamage(Player localPlayer, ushort typeCache, int tileX, int tileY)
	{
		bool flag = typeCache >= 0 && TileID.Sets.TouchDamageImmediate[typeCache] > 0;
		if (flag && typeCache == 80 && !Main.dontStarveWorld)
		{
			flag = false;
		}
		return flag;
	}

	private static bool IsTileDangerous_CheckHot(Player localPlayer, ushort typeCache, int tileX, int tileY)
	{
		bool result = typeCache >= 0 && TileID.Sets.TouchDamageHot[typeCache];
		if (localPlayer.fireWalk)
		{
			result = false;
		}
		return result;
	}

	private bool IsTileDrawLayerSolid(ushort typeCache)
	{
		if (TileID.Sets.DrawTileInSolidLayer[typeCache].HasValue)
		{
			return TileID.Sets.DrawTileInSolidLayer[typeCache].Value;
		}
		return _tileSolid[typeCache];
	}

	private void GetTileOutlineInfo(int x, int y, ushort typeCache, ref Color tileLight, ref Texture2D highlightTexture, ref Color highlightColor)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (Main.InSmartCursorHighlightArea(x, y, out var actuallySelected))
		{
			int num = (tileLight.R + tileLight.G + tileLight.B) / 3;
			if (num > 10)
			{
				highlightTexture = TextureAssets.HighlightMask[typeCache].Value;
				highlightColor = Colors.GetSelectionGlowColor(actuallySelected, num);
			}
		}
	}

	private void DrawPartialLiquid(bool behindBlocks, Tile tileCache, ref Vector2 position, ref Rectangle liquidSize, int liquidType, ref VertexColors colors)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		int num = tileCache.slope();
		bool flag = !TileID.Sets.BlocksWaterDrawingBehindSelf[tileCache.type];
		if (!behindBlocks)
		{
			flag = false;
		}
		if (flag || num == 0)
		{
			Main.tileBatch.Draw(TextureAssets.Liquid[liquidType].Value, position, liquidSize, colors, default, 1f, (SpriteEffects)0);
			return;
		}
		liquidSize.X += 18 * (num - 1);
		switch (num)
		{
		case 1:
			Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, colors, Vector2.Zero, 1f, (SpriteEffects)0);
			break;
		case 2:
			Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, colors, Vector2.Zero, 1f, (SpriteEffects)0);
			break;
		case 3:
			Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, colors, Vector2.Zero, 1f, (SpriteEffects)0);
			break;
		case 4:
			Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, colors, Vector2.Zero, 1f, (SpriteEffects)0);
			break;
		}
	}

	private bool InAPlaceWithWind(int x, int y, int width, int height)
	{
		return WorldGen.InAPlaceWithWind(x, y, width, height);
	}

	private void GetTileDrawData(int x, int y, Tile tileCache, ushort typeCache, ref short tileFrameX, ref short tileFrameY, out int tileWidth, out int tileHeight, out int tileTop, out int halfBrickHeight, out int addFrX, out int addFrY, out SpriteEffects tileSpriteEffect, out Texture2D glowTexture, out Rectangle glowSourceRect, out Color glowColor)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_125a: Unknown result type (might be due to invalid IL or missing references)
		//IL_125f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2876: Unknown result type (might be due to invalid IL or missing references)
		//IL_287b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2892: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_29de: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_30e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_316f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3174: Unknown result type (might be due to invalid IL or missing references)
		//IL_317c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3181: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cba: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_37bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_37c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_37c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3421: Unknown result type (might be due to invalid IL or missing references)
		//IL_3426: Unknown result type (might be due to invalid IL or missing references)
		//IL_342e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3433: Unknown result type (might be due to invalid IL or missing references)
		//IL_3303: Unknown result type (might be due to invalid IL or missing references)
		//IL_3308: Unknown result type (might be due to invalid IL or missing references)
		//IL_3310: Unknown result type (might be due to invalid IL or missing references)
		//IL_3315: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d77: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d89: Unknown result type (might be due to invalid IL or missing references)
		//IL_3562: Unknown result type (might be due to invalid IL or missing references)
		//IL_3567: Unknown result type (might be due to invalid IL or missing references)
		//IL_356f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3574: Unknown result type (might be due to invalid IL or missing references)
		//IL_34a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_34b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_34bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_35a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_35bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_283a: Unknown result type (might be due to invalid IL or missing references)
		//IL_283f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2846: Unknown result type (might be due to invalid IL or missing references)
		//IL_284b: Unknown result type (might be due to invalid IL or missing references)
		//IL_28d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_28d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_28df: Unknown result type (might be due to invalid IL or missing references)
		//IL_28e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_290c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2911: Unknown result type (might be due to invalid IL or missing references)
		//IL_2918: Unknown result type (might be due to invalid IL or missing references)
		//IL_291d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2923: Unknown result type (might be due to invalid IL or missing references)
		//IL_2928: Unknown result type (might be due to invalid IL or missing references)
		//IL_295b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2960: Unknown result type (might be due to invalid IL or missing references)
		//IL_2967: Unknown result type (might be due to invalid IL or missing references)
		//IL_296c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2972: Unknown result type (might be due to invalid IL or missing references)
		//IL_2977: Unknown result type (might be due to invalid IL or missing references)
		//IL_3128: Unknown result type (might be due to invalid IL or missing references)
		//IL_312d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3135: Unknown result type (might be due to invalid IL or missing references)
		//IL_313a: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_31bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_37f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_37fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3802: Unknown result type (might be due to invalid IL or missing references)
		//IL_3807: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2800: Unknown result type (might be due to invalid IL or missing references)
		//IL_2805: Unknown result type (might be due to invalid IL or missing references)
		//IL_3462: Unknown result type (might be due to invalid IL or missing references)
		//IL_3467: Unknown result type (might be due to invalid IL or missing references)
		//IL_346f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3474: Unknown result type (might be due to invalid IL or missing references)
		//IL_3344: Unknown result type (might be due to invalid IL or missing references)
		//IL_3349: Unknown result type (might be due to invalid IL or missing references)
		//IL_3351: Unknown result type (might be due to invalid IL or missing references)
		//IL_3356: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f00: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_3657: Unknown result type (might be due to invalid IL or missing references)
		//IL_365c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3664: Unknown result type (might be due to invalid IL or missing references)
		//IL_3669: Unknown result type (might be due to invalid IL or missing references)
		//IL_3709: Unknown result type (might be due to invalid IL or missing references)
		//IL_370e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3716: Unknown result type (might be due to invalid IL or missing references)
		//IL_371b: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_33c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_33c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3204: Unknown result type (might be due to invalid IL or missing references)
		//IL_3209: Unknown result type (might be due to invalid IL or missing references)
		//IL_3211: Unknown result type (might be due to invalid IL or missing references)
		//IL_3216: Unknown result type (might be due to invalid IL or missing references)
		//IL_3255: Unknown result type (might be due to invalid IL or missing references)
		//IL_325a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3262: Unknown result type (might be due to invalid IL or missing references)
		//IL_3267: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_34f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_34fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_300a: Unknown result type (might be due to invalid IL or missing references)
		//IL_300f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3017: Unknown result type (might be due to invalid IL or missing references)
		//IL_301c: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_35f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_35fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d42: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_3698: Unknown result type (might be due to invalid IL or missing references)
		//IL_369d: Unknown result type (might be due to invalid IL or missing references)
		//IL_36a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_36aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_374a: Unknown result type (might be due to invalid IL or missing references)
		//IL_374f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3757: Unknown result type (might be due to invalid IL or missing references)
		//IL_375c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e04: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_3296: Unknown result type (might be due to invalid IL or missing references)
		//IL_329b: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_304b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3050: Unknown result type (might be due to invalid IL or missing references)
		//IL_3058: Unknown result type (might be due to invalid IL or missing references)
		//IL_305d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e45: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c72: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c77: Unknown result type (might be due to invalid IL or missing references)
		tileTop = 0;
		tileWidth = 16;
		tileHeight = 16;
		halfBrickHeight = 0;
		addFrY = Main.tileFrame[typeCache] * 38;
		addFrX = 0;
		tileSpriteEffect = (SpriteEffects)0;
		glowTexture = null;
		glowSourceRect = Rectangle.Empty;
		glowColor = Color.Transparent;
		Color color = Lighting.GetColor(x, y);
		switch (typeCache)
		{
		case 752:
			tileHeight = 38;
			tileWidth = 36;
			tileTop = 2;
			break;
		case 751:
		{
			tileHeight = 46;
			tileWidth = 56;
			int num3 = (x + y * 2) % 7;
			tileFrameY += (short)(num3 * 46);
			break;
		}
		case 739:
		case 748:
		{
			int num19 = Main.tileFrame[typeCache];
			addFrY = num19 * 90;
			break;
		}
		case 726:
			tileFrameX = 0;
			tileFrameY = 0;
			tileWidth = 20;
			tileHeight = 20;
			break;
		case 719:
		{
			int num33 = (x + y + (int)(Main.GlobalTimeWrappedHourly * 15f)) % 14;
			int num34 = num33 / 4;
			int num35 = num33 % 4;
			addFrX += 288 * num34;
			addFrY += 270 * num35;
			break;
		}
		case 443:
			if (tileFrameX / 36 >= 2)
			{
				tileTop = -2;
			}
			else
			{
				tileTop = 2;
			}
			break;
		case 571:
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			tileTop = 2;
			break;
		case 698:
		{
			tileWidth = 36;
			int num61 = tileFrameX / 18;
			tileFrameX = (short)(num61 * 38);
			tileHeight = 44;
			break;
		}
		case 136:
			if (tileFrameX == 0)
			{
				tileTop = 2;
			}
			break;
		case 561:
			tileTop -= 2;
			tileHeight = 20;
			addFrY = tileFrameY / 18 * 4;
			break;
		case 518:
		{
			int num27 = tileCache.liquid / 16;
			num27 -= 3;
			if (WorldGen.SolidTile(x, y - 1) && num27 > 8)
			{
				num27 = 8;
			}
			if (tileCache.liquid == 0)
			{
				Tile tileSafely = Framing.GetTileSafely(x, y + 1);
				if (tileSafely.nactive())
				{
					switch (tileSafely.blockType())
					{
					case 1:
						num27 = -16 + Math.Max(8, tileSafely.liquid / 16);
						break;
					case 2:
					case 3:
						num27 -= 4;
						break;
					}
				}
			}
			tileTop -= num27;
			break;
		}
		case 330:
		case 331:
		case 332:
		case 333:
			tileTop += 2;
			break;
		case 129:
			addFrY = 0;
			if (tileFrameX >= 324)
			{
				int num55 = (tileFrameX - 324) / 18;
				int num56 = (num55 + Main.tileFrame[typeCache]) % 6 - num55;
				addFrX = num56 * 18;
			}
			break;
		case 5:
		{
			tileWidth = 20;
			tileHeight = 20;
			int treeBiome = GetTreeBiome(x, y, tileFrameX, tileFrameY);
			tileFrameX += (short)(176 * (treeBiome + 1));
			break;
		}
		case 583:
		case 584:
		case 585:
		case 586:
		case 587:
		case 588:
		case 589:
		case 596:
		case 616:
		case 634:
			tileWidth = 20;
			tileHeight = 20;
			break;
		case 476:
			tileWidth = 20;
			tileHeight = 18;
			break;
		case 323:
		{
			tileWidth = 20;
			tileHeight = 20;
			int palmTreeBiome = GetPalmTreeBiome(x, y);
			tileFrameY = (short)(22 * palmTreeBiome);
			break;
		}
		case 4:
			tileWidth = 20;
			tileHeight = 20;
			if (WorldGen.SolidTile(x, y - 1))
			{
				tileTop = 4;
			}
			break;
		case 78:
		case 85:
		case 133:
		case 134:
		case 173:
		case 210:
		case 233:
		case 254:
		case 283:
		case 378:
		case 457:
		case 466:
		case 520:
		case 651:
		case 652:
			tileTop = 2;
			break;
		case 100:
		{
			tileTop = 2;
			int num57 = tileFrameY / 2016;
			addFrY -= 2016 * num57;
			addFrX += 72 * num57;
			break;
		}
		case 530:
		{
			int num50 = y - tileFrameY % 36 / 18 + 2;
			int num51 = x - tileFrameX % 54 / 18;
			WorldGen.GetBiomeInfluence(num51, num51 + 3, num50, num50, out var corruptCount2, out var crimsonCount2, out var hallowedCount2);
			int num52 = corruptCount2;
			if (num52 < crimsonCount2)
			{
				num52 = crimsonCount2;
			}
			if (num52 < hallowedCount2)
			{
				num52 = hallowedCount2;
			}
			int num53 = 0;
			num53 = ((corruptCount2 != 0 || crimsonCount2 != 0 || hallowedCount2 != 0) ? ((hallowedCount2 == num52) ? 1 : ((crimsonCount2 != num52) ? 3 : 2)) : 0);
			addFrY += 36 * num53;
			tileTop = 2;
			break;
		}
		case 705:
			tileTop = 2;
			break;
		case 485:
		{
			tileTop = 2;
			int num14 = Main.tileFrameCounter[typeCache];
			num14 /= 5;
			int num15 = y - tileFrameY / 18;
			int num16 = x - tileFrameX / 18;
			num14 += num15 + num16;
			num14 %= 4;
			addFrY = num14 * 36;
			break;
		}
		case 489:
		{
			tileTop = 2;
			int num46 = y - tileFrameY / 18;
			int num47 = x - tileFrameX / 18;
			if (InAPlaceWithWind(num47, num46, 2, 3))
			{
				int num48 = Main.tileFrameCounter[typeCache];
				num48 /= 5;
				num48 += num46 + num47;
				num48 %= 16;
				addFrY = num48 * 54;
			}
			break;
		}
		case 490:
		{
			tileTop = 2;
			int y2 = y - tileFrameY / 18;
			int x2 = x - tileFrameX / 18;
			bool flag = InAPlaceWithWind(x2, y2, 2, 2);
			int num20 = (flag ? Main.tileFrame[typeCache] : 0);
			int num21 = 0;
			if (flag)
			{
				if (Math.Abs(Main.WindForVisuals) > 0.5f)
				{
					switch (Main.weatherVaneBobframe)
					{
					case 0:
						num21 = 0;
						break;
					case 1:
						num21 = 1;
						break;
					case 2:
						num21 = 2;
						break;
					case 3:
						num21 = 1;
						break;
					case 4:
						num21 = 0;
						break;
					case 5:
						num21 = -1;
						break;
					case 6:
						num21 = -2;
						break;
					case 7:
						num21 = -1;
						break;
					}
				}
				else
				{
					switch (Main.weatherVaneBobframe)
					{
					case 0:
						num21 = 0;
						break;
					case 1:
						num21 = 1;
						break;
					case 2:
						num21 = 0;
						break;
					case 3:
						num21 = -1;
						break;
					case 4:
						num21 = 0;
						break;
					case 5:
						num21 = 1;
						break;
					case 6:
						num21 = 0;
						break;
					case 7:
						num21 = -1;
						break;
					}
				}
			}
			num20 += num21;
			if (num20 < 0)
			{
				num20 += 12;
			}
			num20 %= 12;
			addFrY = num20 * 36;
			break;
		}
		case 33:
		case 49:
		case 174:
		case 372:
		case 646:
			tileHeight = 20;
			tileTop = -4;
			break;
		case 529:
		{
			int num37 = y + 1;
			WorldGen.GetBiomeInfluence(x, x, num37, num37, out var corruptCount, out var crimsonCount, out var hallowedCount);
			int num38 = corruptCount;
			if (num38 < crimsonCount)
			{
				num38 = crimsonCount;
			}
			if (num38 < hallowedCount)
			{
				num38 = hallowedCount;
			}
			int num39 = 0;
			if (corruptCount == 0 && crimsonCount == 0 && hallowedCount == 0)
			{
				num39 = ((x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance) ? 1 : 0);
			}
			else if (hallowedCount == num38)
			{
				num39 = 2;
			}
			else
			{
				num39 = ((crimsonCount != num38) ? 4 : 3);
			}
			addFrY += 34 * num39 - tileFrameY;
			tileHeight = 32;
			tileTop = -14;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		}
		case 3:
		case 24:
		case 61:
		case 71:
		case 110:
		case 201:
		case 637:
		case 703:
			tileHeight = 20;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 20:
		case 590:
		case 595:
			tileHeight = 18;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 615:
			tileHeight = 18;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 16:
		case 17:
		case 26:
		case 32:
		case 69:
		case 72:
		case 77:
		case 124:
		case 137:
		case 138:
		case 352:
		case 462:
		case 487:
		case 488:
		case 574:
		case 575:
		case 576:
		case 577:
		case 578:
		case 664:
		case 695:
		case 704:
		case 712:
		case 713:
		case 714:
		case 715:
		case 716:
			tileHeight = 18;
			break;
		case 79:
		{
			tileHeight = 18;
			int num32 = tileFrameY / 2016;
			addFrY -= 2016 * num32;
			addFrX += 144 * num32;
			break;
		}
		case 90:
		{
			int num31 = tileFrameY / 2016;
			addFrY -= 2016 * num31;
			addFrX += 144 * num31;
			break;
		}
		case 18:
		{
			int num30 = tileFrameX / 2016;
			addFrX -= 2016 * num30;
			addFrY += 20 * num30;
			break;
		}
		case 711:
			if (tileFrameX > 0)
			{
				tileWidth = 18;
			}
			tileHeight = 20;
			glowTexture = TextureAssets.Tile[711].Value;
			glowSourceRect = new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight);
			break;
		case 654:
			tileTop += 2;
			break;
		case 14:
		case 21:
		case 411:
		case 467:
		case 469:
			if (tileFrameY == 18)
			{
				tileHeight = 18;
			}
			break;
		case 15:
		case 497:
			if (tileFrameY % 40 == 18)
			{
				tileHeight = 18;
			}
			break;
		case 172:
		case 376:
			if (tileFrameY % 38 == 18)
			{
				tileHeight = 18;
			}
			break;
		case 27:
			if (tileFrameY % 74 == 54)
			{
				tileHeight = 18;
			}
			break;
		case 132:
		case 135:
			tileTop = 2;
			tileHeight = 18;
			break;
		case 82:
		case 83:
		case 84:
			tileHeight = 20;
			tileTop = -2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 324:
			tileWidth = 20;
			tileHeight = 20;
			tileTop = -2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 494:
			tileTop = 2;
			break;
		case 52:
		case 62:
		case 115:
		case 205:
		case 382:
		case 528:
		case 636:
		case 638:
			tileTop = -2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 80:
		case 142:
		case 143:
			tileTop = 2;
			break;
		case 139:
		{
			tileTop = 2;
			int num5 = tileFrameY / 2016;
			addFrY -= 2016 * num5;
			addFrX += 72 * num5;
			break;
		}
		case 73:
		case 74:
		case 113:
			tileTop = -12;
			tileHeight = 32;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 388:
		case 389:
		{
			int num58 = 94;
			tileTop = -2;
			if (tileFrameY == num58 - 20 || tileFrameY == num58 * 2 - 20 || tileFrameY == 0 || tileFrameY == num58)
			{
				tileHeight = 18;
			}
			if (tileFrameY != 0 && tileFrameY != num58)
			{
				tileTop = 0;
			}
			break;
		}
		case 227:
			tileWidth = 32;
			tileHeight = 38;
			if (tileFrameX == 238)
			{
				tileTop -= 6;
			}
			else
			{
				tileTop -= 20;
			}
			if (tileFrameX == 204)
			{
				WorldGen.GetCactusType(x, y, tileFrameX, tileFrameY, out var evil, out var good, out var crimson);
				if (good)
				{
					tileFrameX += 238;
				}
				if (evil)
				{
					tileFrameX += 204;
				}
				if (crimson)
				{
					tileFrameX += 272;
				}
			}
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 624:
		case 700:
			tileWidth = 20;
			tileHeight = 16;
			tileTop += 2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 656:
		case 701:
			tileWidth = 24;
			tileHeight = 34;
			tileTop -= 16;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 579:
		{
			tileWidth = 20;
			tileHeight = 20;
			tileTop -= 2;
			bool flag2 = (float)(x * 16 + 8) > Main.LocalPlayer.Center.X;
			if (tileFrameX > 0)
			{
				if (flag2)
				{
					addFrY = 22;
				}
				else
				{
					addFrY = 0;
				}
			}
			else if (flag2)
			{
				addFrY = 0;
			}
			else
			{
				addFrY = 22;
			}
			break;
		}
		case 567:
			tileWidth = 26;
			tileHeight = 18;
			if (tileFrameY == 0)
			{
				tileTop = -2;
			}
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 185:
		case 186:
		case 187:
			tileTop = 2;
			switch (typeCache)
			{
			case 185:
				if (tileFrameY == 18 && tileFrameX >= 576 && tileFrameX <= 882)
				{
					Main.tileShine2[185] = true;
				}
				else
				{
					Main.tileShine2[185] = false;
				}
				if (tileFrameY == 18)
				{
					int num26 = tileFrameX / 1908;
					addFrX -= 1908 * num26;
					addFrY += 18 * num26;
				}
				break;
			case 186:
				if (tileFrameX >= 864 && tileFrameX <= 1170)
				{
					Main.tileShine2[186] = true;
				}
				else
				{
					Main.tileShine2[186] = false;
				}
				break;
			case 187:
			{
				int num25 = tileFrameX / 1890;
				addFrX -= 1890 * num25;
				addFrY += 36 * num25;
				break;
			}
			}
			break;
		case 650:
			tileTop = 2;
			break;
		case 649:
		{
			tileTop = 2;
			int num24 = tileFrameX / 1908;
			addFrX -= 1908 * num24;
			addFrY += 18 * num24;
			break;
		}
		case 647:
		case 706:
			tileTop = 2;
			break;
		case 648:
		{
			tileTop = 2;
			int num23 = tileFrameX / 1890;
			addFrX -= 1890 * num23;
			addFrY += 36 * num23;
			break;
		}
		case 178:
			if (tileFrameY <= 36)
			{
				tileTop = 2;
			}
			break;
		case 184:
			tileWidth = 20;
			if (tileFrameY <= 36)
			{
				tileTop = 2;
			}
			else if (tileFrameY <= 108)
			{
				tileTop = -2;
			}
			break;
		case 519:
			tileTop = 2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 493:
			if (tileFrameY == 0)
			{
				int num6 = Main.tileFrameCounter[typeCache];
				float num7 = Math.Abs(Main.WindForVisuals);
				int num8 = y - tileFrameY / 18;
				int num9 = x - tileFrameX / 18;
				if (!InAPlaceWithWind(x, num8, 1, 1))
				{
					num7 = 0f;
				}
				if (!(num7 < 0.1f))
				{
					if (num7 < 0.5f)
					{
						num6 /= 20;
						num6 += num8 + num9;
						num6 %= 6;
						num6 = ((!(Main.WindForVisuals < 0f)) ? (num6 + 1) : (6 - num6));
						addFrY = num6 * 36;
					}
					else
					{
						num6 /= 10;
						num6 += num8 + num9;
						num6 %= 6;
						num6 = ((!(Main.WindForVisuals < 0f)) ? (num6 + 7) : (12 - num6));
						addFrY = num6 * 36;
					}
				}
			}
			tileTop = 2;
			break;
		case 28:
		case 105:
		case 470:
		case 475:
		case 506:
		case 547:
		case 548:
		case 552:
		case 560:
		case 597:
		case 613:
		case 621:
		case 622:
		case 623:
		case 653:
		case 699:
			tileTop = 2;
			break;
		case 617:
			tileTop = 2;
			tileFrameY %= 144;
			tileFrameX %= 54;
			break;
		case 614:
			addFrX = Main.tileFrame[typeCache] * 54;
			addFrY = 0;
			tileTop = 2;
			break;
		case 81:
			tileTop -= 8;
			tileHeight = 26;
			tileWidth = 24;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 272:
			addFrY = 0;
			break;
		case 106:
			addFrY = Main.tileFrame[typeCache] * 54;
			break;
		case 300:
		case 301:
		case 302:
		case 303:
		case 304:
		case 305:
		case 306:
		case 307:
		case 308:
		case 354:
		case 355:
		case 499:
			addFrY = Main.tileFrame[typeCache] * 54;
			tileTop = 2;
			break;
		case 377:
			addFrY = Main.tileFrame[typeCache] * 38;
			tileTop = 2;
			break;
		case 463:
		case 464:
			addFrY = Main.tileFrame[typeCache] * 72;
			tileTop = 2;
			break;
		case 491:
			tileTop = 2;
			addFrX = 54;
			break;
		case 379:
			addFrY = Main.tileFrame[typeCache] * 90;
			break;
		case 349:
		{
			tileTop = 2;
			int num62 = tileFrameX % 36;
			int num63 = tileFrameY % 54;
			if (Animation.GetTemporaryFrame(x - num62 / 18, y - num63 / 18, out var frameData4))
			{
				tileFrameX = (short)(36 * frameData4 + num62);
			}
			break;
		}
		case 441:
		case 468:
		{
			if (tileFrameY == 18)
			{
				tileHeight = 18;
			}
			int num59 = tileFrameX % 36;
			int num60 = tileFrameY % 38;
			if (Animation.GetTemporaryFrame(x - num59 / 18, y - num60 / 18, out var frameData3))
			{
				tileFrameY = (short)(38 * frameData3 + num60);
			}
			break;
		}
		case 390:
			addFrY = Main.tileFrame[typeCache] * 36;
			break;
		case 412:
			addFrY = 0;
			tileTop = 2;
			break;
		case 36:
			tileTop = 2;
			break;
		case 406:
		{
			tileHeight = 16;
			if (tileFrameY % 54 >= 36)
			{
				tileHeight = 18;
			}
			int num54 = Main.tileFrame[typeCache];
			if (tileFrameY >= 108)
			{
				num54 = 6 - tileFrameY / 54;
			}
			else if (tileFrameY >= 54)
			{
				num54 = Main.tileFrame[typeCache] - 1;
			}
			addFrY = num54 * 56;
			addFrY += tileFrameY / 54 * 2;
			break;
		}
		case 452:
		{
			int num49 = Main.tileFrame[typeCache];
			if (tileFrameX >= 54)
			{
				num49 = 0;
			}
			addFrY = num49 * 54;
			break;
		}
		case 455:
		{
			addFrY = 0;
			tileTop = 2;
			int num45 = 1 + Main.tileFrame[typeCache];
			if (!BirthdayParty.PartyIsUp)
			{
				num45 = 0;
			}
			addFrY = num45 * 54;
			break;
		}
		case 454:
			addFrY = Main.tileFrame[typeCache] * 54;
			break;
		case 453:
		{
			int num43 = Main.tileFrameCounter[typeCache];
			num43 /= 20;
			int num44 = y - tileFrameY / 18;
			num43 += num44 + x;
			num43 %= 3;
			addFrY = num43 * 54;
			break;
		}
		case 456:
		{
			int num40 = Main.tileFrameCounter[typeCache];
			num40 /= 20;
			int num41 = y - tileFrameY / 18;
			int num42 = x - tileFrameX / 18;
			num40 += num41 + num42;
			num40 %= 4;
			addFrY = num40 * 54;
			break;
		}
		case 405:
		{
			tileHeight = 16;
			if (tileFrameY > 0)
			{
				tileHeight = 18;
			}
			int num36 = Main.tileFrame[typeCache];
			if (tileFrameX >= 54)
			{
				num36 = 0;
			}
			addFrY = num36 * 38;
			break;
		}
		case 12:
		case 31:
		case 96:
		case 639:
		case 665:
		case 696:
			addFrY = Main.tileFrame[typeCache] * 36;
			break;
		case 238:
			tileTop = 2;
			addFrY = Main.tileFrame[typeCache] * 36;
			break;
		case 593:
		{
			if (tileFrameX >= 18)
			{
				addFrX = -18;
			}
			tileTop = 2;
			if (Animation.GetTemporaryFrame(x, y, out var frameData2))
			{
				addFrY = (short)(18 * frameData2);
			}
			else if (tileFrameX < 18)
			{
				addFrY = Main.tileFrame[typeCache] * 18;
			}
			else
			{
				addFrY = 0;
			}
			break;
		}
		case 594:
		{
			if (tileFrameX >= 36)
			{
				addFrX = -36;
			}
			tileTop = 2;
			int num28 = tileFrameX % 36;
			int num29 = tileFrameY % 36;
			if (Animation.GetTemporaryFrame(x - num28 / 18, y - num29 / 18, out var frameData))
			{
				addFrY = (short)(36 * frameData);
			}
			else if (tileFrameX < 36)
			{
				addFrY = Main.tileFrame[typeCache] * 36;
			}
			else
			{
				addFrY = 0;
			}
			break;
		}
		case 592:
			addFrY = Main.tileFrame[typeCache] * 54;
			break;
		case 228:
		case 231:
		case 243:
		case 247:
			tileTop = 2;
			addFrY = Main.tileFrame[typeCache] * 54;
			break;
		case 244:
			tileTop = 2;
			if (tileFrameX < 54)
			{
				addFrY = Main.tileFrame[typeCache] * 36;
			}
			else
			{
				addFrY = 0;
			}
			break;
		case 565:
			tileTop = 2;
			if (tileFrameX < 36)
			{
				addFrY = Main.tileFrame[typeCache] * 36;
			}
			else
			{
				addFrY = 0;
			}
			break;
		case 235:
			addFrY = Main.tileFrame[typeCache] * 18;
			break;
		case 217:
		case 218:
		case 564:
			addFrY = Main.tileFrame[typeCache] * 36;
			tileTop = 2;
			break;
		case 219:
		case 220:
		case 642:
			addFrY = Main.tileFrame[typeCache] * 54;
			tileTop = 2;
			break;
		case 270:
		case 271:
		case 581:
		{
			int num22 = Main.tileFrame[typeCache] + x % 6;
			if (x % 2 == 0)
			{
				num22 += 3;
			}
			if (x % 3 == 0)
			{
				num22 += 3;
			}
			if (x % 4 == 0)
			{
				num22 += 3;
			}
			while (num22 > 5)
			{
				num22 -= 6;
			}
			addFrX = num22 * 18;
			addFrY = 0;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		}
		case 660:
		{
			int num18 = Main.tileFrame[typeCache] + x % 5;
			if (x % 2 == 0)
			{
				num18 += 3;
			}
			if (x % 3 == 0)
			{
				num18 += 3;
			}
			if (x % 4 == 0)
			{
				num18 += 3;
			}
			while (num18 > 4)
			{
				num18 -= 5;
			}
			addFrX = num18 * 18;
			addFrY = 0;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		}
		case 572:
		{
			int num17;
			for (num17 = Main.tileFrame[typeCache] + x % 4; num17 > 3; num17 -= 4)
			{
			}
			addFrX = num17 * 18;
			addFrY = 0;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		}
		case 428:
			tileTop += 4;
			if (PressurePlateHelper.PressurePlatesPressed.ContainsKey(new Point(x, y)))
			{
				addFrX += 18;
			}
			break;
		case 442:
			tileWidth = 20;
			tileHeight = 20;
			switch (tileFrameX / 22)
			{
			case 1:
				tileTop = -4;
				break;
			case 2:
				tileTop = -2;
				tileWidth = 24;
				break;
			case 3:
				tileTop = -2;
				break;
			}
			break;
		case 275:
		case 276:
		case 277:
		case 278:
		case 279:
		case 280:
		case 281:
		case 296:
		case 297:
		case 309:
		case 358:
		case 359:
		case 413:
		case 414:
		case 542:
		case 550:
		case 551:
		case 553:
		case 554:
		case 558:
		case 559:
		case 599:
		case 600:
		case 601:
		case 602:
		case 603:
		case 604:
		case 605:
		case 606:
		case 607:
		case 608:
		case 609:
		case 610:
		case 611:
		case 612:
		case 632:
		case 640:
		case 643:
		case 644:
		case 645:
		case 710:
		{
			tileTop = 2;
			Main.critterCage = true;
			int bigAnimalCageFrame = GetBigAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			switch (typeCache)
			{
			case 275:
			case 359:
			case 599:
			case 600:
			case 601:
			case 602:
			case 603:
			case 604:
			case 605:
				addFrY = Main.bunnyCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 550:
			case 551:
				addFrY = Main.turtleCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 542:
				addFrY = Main.owlCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 276:
			case 413:
			case 414:
			case 606:
			case 607:
			case 608:
			case 609:
			case 610:
			case 611:
			case 612:
				addFrY = Main.squirrelCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 277:
				addFrY = Main.mallardCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 278:
				addFrY = Main.duckCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 553:
				addFrY = Main.grebeCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 554:
				addFrY = Main.seagullCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 279:
			case 358:
				addFrY = Main.birdCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 280:
				addFrY = Main.blueBirdCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 281:
				addFrY = Main.redBirdCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 632:
			case 640:
			case 643:
			case 644:
			case 645:
				addFrY = Main.macawCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 296:
			case 297:
				addFrY = Main.scorpionCageFrame[0, bigAnimalCageFrame] * 54;
				break;
			case 309:
				addFrY = Main.penguinCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 558:
			case 559:
				addFrY = Main.seahorseCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 710:
			{
				int num13 = Main.pufferfishCageFrame[bigAnimalCageFrame] / 33;
				addFrX = 108 * num13;
				addFrY = (Main.pufferfishCageFrame[bigAnimalCageFrame] - num13 * 33) * 54;
				break;
			}
			}
			break;
		}
		case 285:
		case 286:
		case 298:
		case 299:
		case 310:
		case 339:
		case 361:
		case 362:
		case 363:
		case 364:
		case 391:
		case 392:
		case 393:
		case 394:
		case 532:
		case 533:
		case 538:
		case 544:
		case 555:
		case 556:
		case 582:
		case 619:
		case 629:
		{
			tileTop = 2;
			Main.critterCage = true;
			int smallAnimalCageFrame2 = GetSmallAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			switch (typeCache)
			{
			case 285:
				addFrY = Main.snailCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 286:
			case 582:
				addFrY = Main.snail2CageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 298:
			case 361:
				addFrY = Main.frogCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 339:
			case 362:
				addFrY = Main.grasshopperCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 299:
			case 363:
				addFrY = Main.mouseCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 310:
			case 364:
			case 391:
			case 619:
				addFrY = Main.wormCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 392:
			case 393:
			case 394:
				addFrY = Main.slugCageFrame[typeCache - 392, smallAnimalCageFrame2] * 36;
				break;
			case 532:
				addFrY = Main.maggotCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 533:
				addFrY = Main.ratCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 538:
			case 544:
			case 629:
				addFrY = Main.ladybugCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 555:
			case 556:
				addFrY = Main.waterStriderCageFrame[smallAnimalCageFrame2] * 36;
				break;
			}
			break;
		}
		case 282:
		case 505:
		case 543:
		{
			tileTop = 2;
			Main.critterCage = true;
			int waterAnimalCageFrame5 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			addFrY = Main.fishBowlFrame[waterAnimalCageFrame5] * 36;
			break;
		}
		case 598:
		{
			tileTop = 2;
			Main.critterCage = true;
			int waterAnimalCageFrame4 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			addFrY = Main.lavaFishBowlFrame[waterAnimalCageFrame4] * 36;
			break;
		}
		case 568:
		case 569:
		case 570:
		{
			tileTop = 2;
			Main.critterCage = true;
			int waterAnimalCageFrame3 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			addFrY = Main.fairyJarFrame[waterAnimalCageFrame3] * 36;
			break;
		}
		case 288:
		case 289:
		case 290:
		case 291:
		case 292:
		case 293:
		case 294:
		case 295:
		case 360:
		case 580:
		case 620:
		{
			tileTop = 2;
			Main.critterCage = true;
			int waterAnimalCageFrame2 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			int num12 = typeCache - 288;
			if (typeCache == 360 || typeCache == 580 || typeCache == 620)
			{
				num12 = 8;
			}
			addFrY = Main.butterflyCageFrame[num12, waterAnimalCageFrame2] * 36;
			break;
		}
		case 521:
		case 522:
		case 523:
		case 524:
		case 525:
		case 526:
		case 527:
		{
			tileTop = 2;
			Main.critterCage = true;
			int waterAnimalCageFrame = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			int num11 = typeCache - 521;
			addFrY = Main.dragonflyJarFrame[num11, waterAnimalCageFrame] * 36;
			break;
		}
		case 316:
		case 317:
		case 318:
		{
			tileTop = 2;
			Main.critterCage = true;
			int smallAnimalCageFrame = GetSmallAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			int num10 = typeCache - 316;
			addFrY = Main.jellyfishCageFrame[num10, smallAnimalCageFrame] * 36;
			break;
		}
		case 207:
			tileTop = 2;
			if (tileFrameY >= 72)
			{
				addFrY = Main.tileFrame[typeCache];
				int num4 = x;
				if (tileFrameX % 36 != 0)
				{
					num4--;
				}
				addFrY += num4 % 6;
				if (addFrY >= 6)
				{
					addFrY -= 6;
				}
				addFrY *= 72;
			}
			else
			{
				addFrY = 0;
			}
			break;
		case 410:
			if (tileFrameY == 36)
			{
				tileHeight = 18;
			}
			if (tileFrameY >= 56)
			{
				addFrY = Main.tileFrame[typeCache];
				addFrY *= 56;
			}
			else
			{
				addFrY = 0;
			}
			break;
		case 480:
		case 509:
		case 657:
		case 720:
		case 721:
		case 725:
			tileTop = 2;
			if (tileFrameY >= 54)
			{
				addFrY = Main.tileFrame[typeCache];
				addFrY *= 54;
			}
			else
			{
				addFrY = 0;
			}
			break;
		case 658:
			tileTop = 2;
			switch (tileFrameY / 54)
			{
			default:
				addFrY = Main.tileFrame[typeCache];
				addFrY *= 54;
				break;
			case 1:
				addFrY = Main.tileFrame[typeCache];
				addFrY *= 54;
				addFrY += 486;
				break;
			case 2:
				addFrY = Main.tileFrame[typeCache];
				addFrY *= 54;
				addFrY += 972;
				break;
			}
			break;
		case 733:
			tileTop = 2;
			if (tileFrameY < 54)
			{
				addFrX += 54;
			}
			tileFrameX %= 54;
			tileFrameY %= 54;
			break;
		case 326:
		case 327:
		case 328:
		case 329:
		case 345:
		case 351:
		case 421:
		case 422:
		case 458:
		case 459:
		case 708:
			addFrY = Main.tileFrame[typeCache] * 90;
			break;
		case 541:
			addFrY = ((!_shouldShowInvisibleBlocks) ? 90 : 0);
			break;
		case 507:
		case 508:
		{
			int num = 20;
			int num2 = (Main.tileFrameCounter[typeCache] + x * 11 + y * 27) % (num * 8);
			addFrY = 90 * (num2 / num);
			break;
		}
		case 336:
		case 340:
		case 341:
		case 342:
		case 343:
		case 344:
			addFrY = Main.tileFrame[typeCache] * 90;
			tileTop = 2;
			break;
		case 89:
			tileTop = 2;
			break;
		case 102:
			tileTop = 2;
			break;
		case 753:
			tileTop = 2;
			break;
		}
		if (TileID.Sets.Campfires[tileCache.type])
		{
			if (tileFrameY < 36)
			{
				addFrY = Main.tileFrame[typeCache] * 36;
			}
			else
			{
				addFrY = 252;
			}
			tileTop = 2;
		}
		if (tileCache.halfBrick())
		{
			halfBrickHeight = 8;
		}
		switch (typeCache)
		{
		case 412:
			glowTexture = TextureAssets.GlowMask[202].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = new Color(255, 255, 255, 255);
			break;
		case 657:
			if (tileFrameY >= 54)
			{
				glowTexture = TextureAssets.GlowMask[330].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
				glowColor = Color.White;
			}
			break;
		case 656:
		case 701:
			glowTexture = TextureAssets.GlowMask[329].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = new Color(255, 255, 255, 0) * ((float)(int)Main.mouseTextColor / 255f);
			break;
		case 634:
			glowTexture = TextureAssets.GlowMask[315].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = Color.White;
			break;
		case 637:
			glowTexture = GetTileDrawTexture(tileCache, x, y);
			glowSourceRect = new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = Color.Lerp(Color.White, color, 0.75f);
			break;
		case 638:
			glowTexture = TextureAssets.GlowMask[327].Value;
			glowSourceRect = new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight);
			glowColor = Color.Lerp(Color.White, color, 0.75f);
			break;
		case 568:
			glowTexture = TextureAssets.GlowMask[268].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = Color.White;
			break;
		case 569:
			glowTexture = TextureAssets.GlowMask[269].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = Color.White;
			break;
		case 570:
			glowTexture = TextureAssets.GlowMask[270].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = Color.White;
			break;
		case 580:
			glowTexture = TextureAssets.GlowMask[289].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = new Color(225, 110, 110, 0);
			break;
		case 564:
			if (tileCache.frameX < 36)
			{
				glowTexture = TextureAssets.GlowMask[267].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
				glowColor = new Color(200, 200, 200, 0) * ((float)(int)Main.mouseTextColor / 255f);
			}
			addFrY = 0;
			break;
		case 184:
			if (tileCache.frameX == 110)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _lavaMossGlow;
			}
			if (tileCache.frameX == 132)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _kryptonMossGlow;
			}
			if (tileCache.frameX == 154)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _xenonMossGlow;
			}
			if (tileCache.frameX == 176)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _argonMossGlow;
			}
			if (tileCache.frameX == 198)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _violetMossGlow;
			}
			if (tileCache.frameX == 220)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			}
			break;
		case 463:
			glowTexture = TextureAssets.GlowMask[243].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = new Color(127, 127, 127, 0);
			break;
		case 19:
		{
			int num64 = tileFrameY / 18;
			if (num64 == 26)
			{
				glowTexture = TextureAssets.GlowMask[65].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 18, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 27)
			{
				glowTexture = TextureAssets.GlowMask[112].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 18, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 90:
		{
			int num64 = tileFrameY / 36;
			if (num64 == 27)
			{
				glowTexture = TextureAssets.GlowMask[52].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 28)
			{
				glowTexture = TextureAssets.GlowMask[113].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 79:
		{
			int num64 = tileFrameY / 36;
			if (num64 == 27)
			{
				glowTexture = TextureAssets.GlowMask[53].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 28)
			{
				glowTexture = TextureAssets.GlowMask[114].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 89:
		{
			int num64 = tileFrameX / 54;
			int num67 = tileFrameX / 1998;
			addFrX -= 1998 * num67;
			addFrY += 36 * num67;
			if (num64 == 29)
			{
				glowTexture = TextureAssets.GlowMask[66].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 30)
			{
				glowTexture = TextureAssets.GlowMask[123].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 100:
			if (tileFrameX / 36 == 0)
			{
				int num64 = tileFrameY / 36;
				if (num64 == 27)
				{
					glowTexture = TextureAssets.GlowMask[68].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
			}
			break;
		case 33:
			if (tileFrameX / 18 == 0)
			{
				int num64 = tileFrameY / 22;
				if (num64 == 26)
				{
					glowTexture = TextureAssets.GlowMask[61].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 22, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
			}
			break;
		case 15:
		case 497:
		{
			int num64 = tileFrameY / 40;
			int num71 = num64 / 51;
			addFrY -= 2040 * num71;
			addFrX += 36 * num71;
			if (typeCache == 15)
			{
				if (num64 == 32)
				{
					glowTexture = TextureAssets.GlowMask[54].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 40, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num64 == 33)
				{
					glowTexture = TextureAssets.GlowMask[116].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 40, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
			}
			break;
		}
		case 34:
			if (tileFrameX / 54 == 0)
			{
				int num64 = tileFrameY / 54;
				if (num64 == 33)
				{
					glowTexture = TextureAssets.GlowMask[55].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
			}
			break;
		case 21:
		case 467:
		{
			int num64 = tileFrameX / 36;
			if (num64 == 48)
			{
				glowTexture = TextureAssets.GlowMask[56].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 49)
			{
				glowTexture = TextureAssets.GlowMask[117].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 441:
		case 468:
		{
			int num64 = tileFrameX / 36;
			if (num64 == 48)
			{
				glowTexture = TextureAssets.GlowMask[56].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 49)
			{
				glowTexture = TextureAssets.GlowMask[117].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 10:
		{
			int num64 = tileFrameY / 54;
			if (tileFrameX < 54 && num64 == 32)
			{
				glowTexture = TextureAssets.GlowMask[57].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			break;
		}
		case 11:
		{
			int num64 = tileFrameY / 54;
			if (tileFrameX < 54)
			{
				if (num64 == 32)
				{
					glowTexture = TextureAssets.GlowMask[58].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num64 == 33)
				{
					glowTexture = TextureAssets.GlowMask[119].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
			}
			break;
		}
		case 88:
		{
			int num64 = tileFrameX / 54;
			int num73 = tileFrameX / 1998;
			addFrX -= 1998 * num73;
			addFrY += 36 * num73;
			if (num64 == 24)
			{
				glowTexture = TextureAssets.GlowMask[59].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 25)
			{
				glowTexture = TextureAssets.GlowMask[120].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 42:
		{
			int num64 = tileFrameY / 36;
			int num72 = tileFrameY / 2016;
			addFrY -= 2016 * num72;
			addFrX += 36 * num72;
			if (num64 == 33)
			{
				glowTexture = TextureAssets.GlowMask[63].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			break;
		}
		case 87:
		{
			int num64 = tileFrameX / 54;
			int num70 = tileFrameX / 1998;
			addFrX -= 1998 * num70;
			addFrY += 36 * num70;
			if (num64 == 26)
			{
				glowTexture = TextureAssets.GlowMask[64].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 27)
			{
				glowTexture = TextureAssets.GlowMask[121].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 14:
		{
			int num64 = tileFrameX / 54;
			if (num64 == 31)
			{
				glowTexture = TextureAssets.GlowMask[67].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 32)
			{
				glowTexture = TextureAssets.GlowMask[124].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 93:
		{
			int num64 = tileFrameY / 54;
			int num69 = tileFrameY / 1998;
			addFrY -= 1998 * num69;
			addFrX += 36 * num69;
			tileTop += 2;
			if (num64 == 27)
			{
				glowTexture = TextureAssets.GlowMask[62].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			break;
		}
		case 18:
		{
			int num64 = tileFrameX / 36;
			if (num64 == 27)
			{
				glowTexture = TextureAssets.GlowMask[69].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 28)
			{
				glowTexture = TextureAssets.GlowMask[125].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 101:
		{
			int num64 = tileFrameX / 54;
			int num68 = tileFrameX / 1998;
			addFrX -= 1998 * num68;
			addFrY += 72 * num68;
			if (num64 == 28)
			{
				glowTexture = TextureAssets.GlowMask[60].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 29)
			{
				glowTexture = TextureAssets.GlowMask[115].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 104:
		{
			int num64 = tileFrameX / 36;
			int num66 = tileFrameX / 2016;
			addFrX -= 2016 * num66;
			addFrY += 90 * num66;
			tileTop = 2;
			if (num64 == 24)
			{
				glowTexture = TextureAssets.GlowMask[51].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 25)
			{
				glowTexture = TextureAssets.GlowMask[118].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 172:
		{
			int num64 = tileFrameY / 38;
			int num65 = tileFrameY / 2014;
			addFrY -= 2014 * num65;
			addFrX += 36 * num65;
			if (num64 == 28)
			{
				glowTexture = TextureAssets.GlowMask[88].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 38, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 29)
			{
				glowTexture = TextureAssets.GlowMask[122].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 38, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		}
	}

	private bool IsWindBlocked(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile == null)
		{
			return true;
		}
		if (tile.wall > 0 && !WallID.Sets.AllowsWind[tile.wall])
		{
			return true;
		}
		if ((double)y > Main.worldSurface)
		{
			return true;
		}
		return false;
	}

	private int GetWaterAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
	{
		int num = x - tileFrameX / 18;
		int num2 = y - tileFrameY / 18;
		return num / 2 * (num2 / 3) % Main.cageFrames;
	}

	private int GetSmallAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
	{
		int num = x - tileFrameX / 18;
		int num2 = y - tileFrameY / 18;
		return num / 3 * (num2 / 3) % Main.cageFrames;
	}

	private int GetBigAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
	{
		int num = x - tileFrameX / 18;
		int num2 = y - tileFrameY / 18;
		return num / 6 * (num2 / 4) % Main.cageFrames;
	}

	public static void GetScreenDrawArea(bool useOffscreenRange, out Vector2 drawOffSet, out int firstTileX, out int lastTileX, out int firstTileY, out int lastTileY)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		Vector2 scaledPosition = Main.Camera.ScaledPosition;
		Vector2 scaledSize = Main.Camera.ScaledSize;
		drawOffSet = (Vector2)(useOffscreenRange ? new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange) : Vector2.Zero);
		firstTileX = (int)((scaledPosition.X - drawOffSet.X) / 16f - 1f);
		lastTileX = (int)((scaledPosition.X + scaledSize.X + drawOffSet.X) / 16f) + 2;
		firstTileY = (int)((scaledPosition.Y - drawOffSet.Y) / 16f - 1f);
		lastTileY = (int)((scaledPosition.Y + scaledSize.Y + drawOffSet.Y) / 16f) + 5;
		if (firstTileX < 4)
		{
			firstTileX = 4;
		}
		if (lastTileX > Main.maxTilesX - 4)
		{
			lastTileX = Main.maxTilesX - 4;
		}
		if (firstTileY < 4)
		{
			firstTileY = 4;
		}
		if (lastTileY > Main.maxTilesY - 4)
		{
			lastTileY = Main.maxTilesY - 4;
		}
		if (Main.sectionManager.AnyUnfinishedSections)
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			WorldGen.SectionTileFrameWithCheck(firstTileX, firstTileY, lastTileX, lastTileY);
			TimeLogger.SectionFraming.AddTime(fromTimestamp);
		}
		if (Main.sectionManager.AnyNeedRefresh)
		{
			TimeLogger.StartTimestamp fromTimestamp2 = TimeLogger.Start();
			WorldGen.RefreshSections(firstTileX, firstTileY, lastTileX, lastTileY);
			TimeLogger.SectionRefresh.AddTime(fromTimestamp2);
		}
	}

	public void ClearCachedTileDraws(bool solidLayer)
	{
		if (solidLayer)
		{
			_displayDollTileEntityPositions.Clear();
			_hatRackTileEntityPositions.Clear();
		}
		else
		{
			ClearSpecialBlockCounts();
		}
	}

	private void AddSpecialLegacyPoint(Point p)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		AddSpecialLegacyPoint(p.X, p.Y);
	}

	private void AddSpecialLegacyPoint(int x, int y)
	{
		_specialTileX[_specialTilesCount] = x;
		_specialTileY[_specialTilesCount] = y;
		_specialTilesCount++;
	}

	private void ClearLegacyCachedDraws()
	{
		_chestPositions.Clear();
		_trainingDummyTileEntityPositions.Clear();
		_foodPlatterTileEntityPositions.Clear();
		_itemFrameTileEntityPositions.Clear();
		_deadCellsDisplayJarTileEntityPositions.Clear();
		_weaponRackTileEntityPositions.Clear();
		_specialTilesCount = 0;
	}

	private Color DrawTiles_GetLightOverride(int j, int i, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, Color tileLight)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		if (tileCache.fullbrightBlock())
		{
			return Color.White;
		}
		switch (typeCache)
		{
		case 541:
		case 631:
			return Color.White;
		case 19:
			if (tileFrameY / 18 == 48)
			{
				return Color.White;
			}
			break;
		case 83:
		{
			int num2 = tileFrameX / 18;
			if (WorldGen.IsAlchemyPlantHarvestable(num2, j) && num2 == 5)
			{
				tileLight.A = (byte)(Main.mouseTextColor / 2);
				tileLight.G = Main.mouseTextColor;
				tileLight.B = Main.mouseTextColor;
			}
			break;
		}
		case 84:
			if (tileFrameX / 18 == 6)
			{
				byte b6 = (byte)((Main.mouseTextColor + tileLight.G * 2) / 3);
				byte b7 = (byte)((Main.mouseTextColor + tileLight.B * 2) / 3);
				if (b6 > tileLight.G)
				{
					tileLight.G = b6;
				}
				if (b7 > tileLight.B)
				{
					tileLight.B = b7;
				}
			}
			break;
		case 61:
		case 703:
			if (tileFrameX == 144)
			{
				byte b = (tileLight.B = (byte)(245f - (float)(int)Main.mouseTextColor * 1.5f));
				byte b3 = (tileLight.G = b);
				byte a2 = (tileLight.R = b3);
				tileLight.A = a2;
			}
			break;
		case 481:
		case 482:
		case 483:
		{
			float num = 1f + (float)Math.Sin(Main.GlobalTimeWrappedHourly / 1.5f * ((float)Math.PI * 2f)) * 0.15f;
			byte a = tileLight.A;
			tileLight *= num;
			tileLight.A = a;
			break;
		}
		}
		return tileLight;
	}

	private void DrawTiles_EmitParticles(int j, int i, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, Color tileLight)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1091: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_180f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1815: Unknown result type (might be due to invalid IL or missing references)
		//IL_1178: Unknown result type (might be due to invalid IL or missing references)
		//IL_118f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1195: Unknown result type (might be due to invalid IL or missing references)
		//IL_11be: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1870: Unknown result type (might be due to invalid IL or missing references)
		//IL_1887: Unknown result type (might be due to invalid IL or missing references)
		//IL_188d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1837: Unknown result type (might be due to invalid IL or missing references)
		//IL_184e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1854: Unknown result type (might be due to invalid IL or missing references)
		//IL_112d: Unknown result type (might be due to invalid IL or missing references)
		//IL_112f: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1310: Unknown result type (might be due to invalid IL or missing references)
		//IL_1328: Unknown result type (might be due to invalid IL or missing references)
		//IL_132e: Unknown result type (might be due to invalid IL or missing references)
		//IL_134d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1352: Unknown result type (might be due to invalid IL or missing references)
		//IL_1363: Unknown result type (might be due to invalid IL or missing references)
		//IL_1368: Unknown result type (might be due to invalid IL or missing references)
		//IL_120d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1226: Unknown result type (might be due to invalid IL or missing references)
		//IL_122c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1247: Unknown result type (might be due to invalid IL or missing references)
		//IL_124c: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_14da: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1501: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1910: Unknown result type (might be due to invalid IL or missing references)
		//IL_1915: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1526: Unknown result type (might be due to invalid IL or missing references)
		//IL_152d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1533: Unknown result type (might be due to invalid IL or missing references)
		//IL_1574: Unknown result type (might be due to invalid IL or missing references)
		//IL_157e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1583: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1662: Unknown result type (might be due to invalid IL or missing references)
		//IL_1679: Unknown result type (might be due to invalid IL or missing references)
		//IL_167f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1606: Unknown result type (might be due to invalid IL or missing references)
		//IL_1610: Unknown result type (might be due to invalid IL or missing references)
		//IL_1615: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1712: Unknown result type (might be due to invalid IL or missing references)
		//IL_172c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1732: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16be: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1767: Unknown result type (might be due to invalid IL or missing references)
		//IL_1771: Unknown result type (might be due to invalid IL or missing references)
		//IL_1776: Unknown result type (might be due to invalid IL or missing references)
		//IL_2087: Unknown result type (might be due to invalid IL or missing references)
		//IL_209f: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_212f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2147: Unknown result type (might be due to invalid IL or missing references)
		//IL_214d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_217d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2195: Unknown result type (might be due to invalid IL or missing references)
		//IL_219b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2248: Unknown result type (might be due to invalid IL or missing references)
		//IL_2261: Unknown result type (might be due to invalid IL or missing references)
		//IL_2267: Unknown result type (might be due to invalid IL or missing references)
		//IL_21dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2014: Unknown result type (might be due to invalid IL or missing references)
		//IL_201e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2023: Unknown result type (might be due to invalid IL or missing references)
		//IL_2320: Unknown result type (might be due to invalid IL or missing references)
		//IL_2339: Unknown result type (might be due to invalid IL or missing references)
		//IL_233f: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2301: Unknown result type (might be due to invalid IL or missing references)
		//IL_230b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2310: Unknown result type (might be due to invalid IL or missing references)
		//IL_237d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2399: Unknown result type (might be due to invalid IL or missing references)
		//IL_239f: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2407: Unknown result type (might be due to invalid IL or missing references)
		//IL_240d: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_24fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2500: Unknown result type (might be due to invalid IL or missing references)
		//IL_2474: Unknown result type (might be due to invalid IL or missing references)
		//IL_248c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2492: Unknown result type (might be due to invalid IL or missing references)
		//IL_2572: Unknown result type (might be due to invalid IL or missing references)
		//IL_2589: Unknown result type (might be due to invalid IL or missing references)
		//IL_258f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2610: Unknown result type (might be due to invalid IL or missing references)
		//IL_262c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2632: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b90: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2738: Unknown result type (might be due to invalid IL or missing references)
		//IL_273d: Unknown result type (might be due to invalid IL or missing references)
		//IL_275b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2927: Unknown result type (might be due to invalid IL or missing references)
		//IL_280b: Unknown result type (might be due to invalid IL or missing references)
		//IL_279b: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_27cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_27dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2940: Unknown result type (might be due to invalid IL or missing references)
		//IL_282c: Unknown result type (might be due to invalid IL or missing references)
		//IL_28cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_28e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_28fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2908: Unknown result type (might be due to invalid IL or missing references)
		//IL_290d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2959: Unknown result type (might be due to invalid IL or missing references)
		//IL_2849: Unknown result type (might be due to invalid IL or missing references)
		//IL_2976: Unknown result type (might be due to invalid IL or missing references)
		//IL_2863: Unknown result type (might be due to invalid IL or missing references)
		//IL_2993: Unknown result type (might be due to invalid IL or missing references)
		//IL_287d: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_289f: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_28bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a4c: Unknown result type (might be due to invalid IL or missing references)
		bool flag = IsVisible(tileCache);
		int leafFrequency = _leafFrequency;
		leafFrequency /= 4;
		if (typeCache == 718 && !Main.dayTime && _rand.Next(3) == 0 && !WorldGen.SolidTile3(i, j - 1))
		{
			if (Main.player[Main.myPlayer].RollLuck(100) == 0)
			{
				int num = Gore.NewGore(new Vector2((float)(i * 16 + _rand.Next(16)), (float)(j * 16 - 12)), default, 16);
				Main.gore[num].scale *= _rand.NextFloat() * 0.5f + 0.75f;
				Gore obj = Main.gore[num];
				obj.velocity *= 0.2f;
				Main.gore[num].velocity.Y -= (float)_rand.Next(5, 31) * 0.1f;
				if (_rand.Next(5) == 0)
				{
					Main.gore[num].velocity.Y -= (float)_rand.Next(5, 41) * 0.1f;
				}
				if (_rand.Next(3) == 0)
				{
					Gore obj2 = Main.gore[num];
					obj2.velocity *= 0.5f;
				}
				Gore obj3 = Main.gore[num];
				obj3.velocity /= Main.gore[num].scale;
				int num2 = Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, 16);
				Main.gore[num2].scale = Main.gore[num].scale;
				Main.gore[num2].position = Main.gore[num].position;
				Main.gore[num2].velocity = Main.gore[num].velocity;
			}
			if (Main.player[Main.myPlayer].RollLuck(60) == 0)
			{
				int num3 = Gore.NewGore(new Vector2((float)(i * 16 + _rand.Next(16)), (float)(j * 16 - 12)), default, 17);
				Main.gore[num3].scale *= _rand.NextFloat() * 0.5f + 0.75f;
				Gore obj4 = Main.gore[num3];
				obj4.velocity *= 0.2f;
				Main.gore[num3].velocity.Y -= (float)_rand.Next(5, 41) * 0.1f;
				if (_rand.Next(5) == 0)
				{
					Main.gore[num3].velocity.Y -= (float)_rand.Next(5, 51) * 0.1f;
				}
				if (_rand.Next(3) == 0)
				{
					Gore obj5 = Main.gore[num3];
					obj5.velocity *= 0.5f;
				}
				Gore obj6 = Main.gore[num3];
				obj6.velocity /= Main.gore[num3].scale;
				int num4 = Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, 17);
				Main.gore[num4].scale = Main.gore[num3].scale;
				Main.gore[num4].position = Main.gore[num3].position;
				Main.gore[num4].velocity = Main.gore[num3].velocity;
			}
			if (Main.player[Main.myPlayer].RollLuck(30) == 0)
			{
				int num5 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16 - 2)), 1, 1, 58, 0f, 0f, 150);
				Main.dust[num5].scale *= _rand.NextFloat() * 0.5f + 0.75f;
				Main.dust[num5].color = new Color(255, 255, 255, 0);
				Dust obj7 = Main.dust[num5];
				obj7.velocity *= 0.2f;
				Main.dust[num5].velocity.Y -= (float)_rand.Next(5, 51) * 0.1f;
				if (_rand.Next(5) == 0)
				{
					Main.dust[num5].velocity.Y -= (float)_rand.Next(5, 61) * 0.1f;
				}
				if (_rand.Next(3) == 0)
				{
					Dust obj8 = Main.dust[num5];
					obj8.velocity *= 0.5f;
				}
				Dust obj9 = Main.dust[num5];
				obj9.velocity /= Main.dust[num5].scale;
			}
		}
		if (typeCache == 244 && tileFrameX == 18 && tileFrameY == 18 && _rand.Next(2) == 0)
		{
			if (_rand.Next(500) == 0)
			{
				Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default, 415, (float)_rand.Next(51, 101) * 0.01f);
			}
			else if (_rand.Next(250) == 0)
			{
				Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default, 414, (float)_rand.Next(51, 101) * 0.01f);
			}
			else if (_rand.Next(80) == 0)
			{
				Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default, 413, (float)_rand.Next(51, 101) * 0.01f);
			}
			else if (_rand.Next(10) == 0)
			{
				Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default, 412, (float)_rand.Next(51, 101) * 0.01f);
			}
			else if (_rand.Next(3) == 0)
			{
				Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default, 411, (float)_rand.Next(51, 101) * 0.01f);
			}
		}
		if (typeCache == 565 && tileFrameX == 0 && tileFrameY == 18 && _rand.Next(3) == 0)
		{
			Vector2 val = new Point(i, j).ToWorldCoordinates();
			int type = 1202;
			float scale = 8f + Main.rand.NextFloat() * 1.6f;
			Vector2 position = val + new Vector2(0f, -18f);
			Vector2 val2 = Main.rand.NextVector2Circular(0.7f, 0.25f) * 0.4f + Main.rand.NextVector2CircularEdge(1f, 0.4f) * 0.1f;
			val2 *= 4f;
			Gore.NewGorePerfect(position, val2, type, scale);
		}
		if (typeCache == 215 && tileFrameY < 36 && _rand.Next(3) == 0 && tileFrameY == 0)
		{
			int num6 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 - 4)), 4, 8, 31, 0f, 0f, 100);
			if (tileFrameX == 0)
			{
				_dust[num6].position.X += _rand.Next(8);
			}
			if (tileFrameX == 36)
			{
				_dust[num6].position.X -= _rand.Next(8);
			}
			_dust[num6].alpha += _rand.Next(100);
			Dust obj10 = _dust[num6];
			obj10.velocity *= 0.2f;
			_dust[num6].velocity.Y -= 0.5f + (float)_rand.Next(10) * 0.1f;
			_dust[num6].fadeIn = 0.5f + (float)_rand.Next(10) * 0.1f;
		}
		if (typeCache == 592 && tileFrameY == 18 && _rand.Next(3) == 0)
		{
			int num7 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 + 4)), 4, 8, 31, 0f, 0f, 100);
			if (tileFrameX == 0)
			{
				_dust[num7].position.X += _rand.Next(8);
			}
			if (tileFrameX == 36)
			{
				_dust[num7].position.X -= _rand.Next(8);
			}
			_dust[num7].alpha += _rand.Next(100);
			Dust obj11 = _dust[num7];
			obj11.velocity *= 0.2f;
			_dust[num7].velocity.Y -= 0.5f + (float)_rand.Next(10) * 0.1f;
			_dust[num7].fadeIn = 0.5f + (float)_rand.Next(10) * 0.1f;
		}
		else if (typeCache == 406 && tileFrameY == 54 && tileFrameX == 0 && _rand.Next(3) == 0)
		{
			Vector2 position2 = new Vector2((float)(i * 16 + 16), (float)(j * 16 + 8));
			Vector2 velocity = new Vector2(0f, 0f);
			if (Main.WindForVisuals < 0f)
			{
				velocity.X = 0f - Main.WindForVisuals;
			}
			int type2 = _rand.Next(825, 828);
			if (_rand.Next(4) == 0)
			{
				Gore.NewGore(position2, velocity, type2, _rand.NextFloat() * 0.2f + 0.2f);
			}
			else if (_rand.Next(2) == 0)
			{
				Gore.NewGore(position2, velocity, type2, _rand.NextFloat() * 0.3f + 0.3f);
			}
			else
			{
				Gore.NewGore(position2, velocity, type2, _rand.NextFloat() * 0.4f + 0.4f);
			}
		}
		else if (typeCache == 452 && tileFrameY == 0 && tileFrameX == 0 && _rand.Next(3) == 0)
		{
			Vector2 position3 = new Vector2((float)(i * 16 + 16), (float)(j * 16 + 8));
			Vector2 velocity2 = new Vector2(0f, 0f);
			if (Main.WindForVisuals < 0f)
			{
				velocity2.X = 0f - Main.WindForVisuals;
			}
			int num8 = Main.tileFrame[typeCache];
			int type3 = 907 + num8 / 5;
			if (_rand.Next(2) == 0)
			{
				Gore.NewGore(position3, velocity2, type3, _rand.NextFloat() * 0.4f + 0.4f);
			}
		}
		if (typeCache == 192 && _rand.Next(leafFrequency) == 0)
		{
			EmitLivingTreeLeaf(i, j, 910);
		}
		if (typeCache == 384 && _rand.Next(leafFrequency) == 0)
		{
			EmitLivingTreeLeaf(i, j, 914);
		}
		if ((typeCache == 666 || typeCache == 712) && tileCache.liquid <= 0 && j - 1 > 0 && _rand.Next(100) == 0 && !WorldGen.ActiveAndWalkableTile(i, j - 1) && !WorldGen.AnyLiquidAt(i, j - 1))
		{
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.PooFly, new ParticleOrchestraSettings
			{
				PositionInWorld = new Vector2((float)(i * 16 + 8), (float)(j * 16 - 8))
			});
		}
		if (typeCache == 711 && tileFrameX == 0 && tileFrameY == 0)
		{
			if (_rand.Next(45) == 0)
			{
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.RainbowBoulder3, new ParticleOrchestraSettings
				{
					PositionInWorld = new Vector2((float)(i * 16 + 16), (float)(j * 16 + 16))
				});
			}
			if (_rand.Next(3) != 0)
			{
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.RainbowBoulder2, new ParticleOrchestraSettings
				{
					PositionInWorld = new Vector2((float)(i * 16 + 16), (float)(j * 16 + 16)) + _rand.NextVector2Circular(16f, 16f),
					MovementVector = _rand.NextVector2Circular(1f, 0.5f) * 0.5f
				});
			}
		}
		if (TileID.Sets.SpawnsNatureFlies[typeCache] && tileCache.liquid <= 0)
		{
			float lerpValue = Utils.GetLerpValue(0.08f, 0.18f, Math.Abs(Main.WindForVisuals), clamped: true);
			lerpValue += 0.3f;
			if (_rand.NextFloat() < lerpValue)
			{
				bool flag2 = _rand.Next(600) == 0;
				if (!flag2)
				{
					_windGrid.GetWindTime(i, j, 8, out var windTimeLeft, out int directionX, out directionX);
					flag2 = windTimeLeft > 0 && _rand.Next(48) == 0;
				}
				if (flag2)
				{
					ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.NatureFly, new ParticleOrchestraSettings
					{
						PositionInWorld = new Vector2((float)(i * 16 + 8), (float)(j * 16))
					});
				}
			}
		}
		if (_rand.Next(1200) == 0)
		{
			bool flag3 = j + 1 < 0;
			bool flag4 = false;
			int num9 = 3;
			if ((double)j < Main.worldSurface)
			{
				if (_rand.Next(10) != 0)
				{
					flag3 = true;
				}
				else
				{
					num9--;
					flag4 = true;
				}
			}
			if (!TileID.Sets.MakesRubbleDust[typeCache])
			{
				flag3 = true;
			}
			if (!flag3 && WorldGen.ActiveAndWalkableTile(i, j + 1))
			{
				flag3 = true;
			}
			if (!flag3 && !WallID.Sets.AllowsWind[Main.tile[i, j].wall])
			{
				if (_rand.Next(2) == 0)
				{
					flag3 = true;
				}
				else
				{
					num9--;
				}
			}
			if (!flag3)
			{
				for (int k = 0; k < num9; k++)
				{
					int num10 = WorldGen.KillTile_MakeTileDust(i, j, tileCache);
					Dust dust = Main.dust[num10];
					dust.position.Y += 8f;
					dust.velocity *= 0.1f;
					if (flag4)
					{
						dust.scale -= 0.3f;
					}
				}
			}
		}
		if (!flag)
		{
			return;
		}
		if (typeCache == 238 && _rand.Next(10) == 0)
		{
			int num11 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 168);
			_dust[num11].noGravity = true;
			_dust[num11].alpha = 200;
		}
		if (typeCache == 139)
		{
			if (tileCache.frameX == 36 && tileCache.frameY % 36 == 0 && (int)Main.timeForVisualEffects % 7 == 0 && _rand.Next(3) == 0)
			{
				int num12 = _rand.Next(570, 573);
				Vector2 position4 = new Vector2((float)(i * 16 + 8), (float)(j * 16 - 8));
				Vector2 velocity3 = new Vector2(Main.WindForVisuals * 2f, -0.5f);
				velocity3.X *= 1f + (float)_rand.Next(-50, 51) * 0.01f;
				velocity3.Y *= 1f + (float)_rand.Next(-50, 51) * 0.01f;
				if (num12 == 572)
				{
					position4.X -= 8f;
				}
				if (num12 == 571)
				{
					position4.X -= 4f;
				}
				Gore.NewGore(position4, velocity3, num12, 0.8f);
			}
		}
		else if (typeCache == 463)
		{
			if (tileFrameY == 54 && tileFrameX == 0)
			{
				for (int l = 0; l < 4; l++)
				{
					if (_rand.Next(2) != 0)
					{
						Dust dust2 = Dust.NewDustDirect(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 36, 8, 16);
						dust2.noGravity = true;
						dust2.alpha = 140;
						dust2.fadeIn = 1.2f;
						dust2.velocity = Vector2.Zero;
					}
				}
			}
			if (tileFrameY == 18 && (tileFrameX == 0 || tileFrameX == 36))
			{
				for (int m = 0; m < 1; m++)
				{
					if (_rand.Next(13) == 0)
					{
						Dust dust3 = Dust.NewDustDirect(new Vector2((float)(i * 16), (float)(j * 16)), 8, 8, 274);
						dust3.position = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8));
						dust3.position.X += ((tileFrameX == 36) ? 4 : (-4));
						dust3.noGravity = true;
						dust3.alpha = 128;
						dust3.fadeIn = 1.2f;
						dust3.noLight = true;
						dust3.velocity = new Vector2(0f, _rand.NextFloatDirection() * 1.2f);
					}
				}
			}
		}
		else if (typeCache == 497)
		{
			if (tileCache.frameY / 40 == 31 && tileCache.frameY % 40 == 0)
			{
				for (int n = 0; n < 1; n++)
				{
					if (_rand.Next(10) == 0)
					{
						Dust dust4 = Dust.NewDustDirect(new Vector2((float)(i * 16), (float)(j * 16 + 8)), 16, 12, 43);
						dust4.noGravity = true;
						dust4.alpha = 254;
						dust4.color = Color.White;
						dust4.scale = 0.7f;
						dust4.velocity = Vector2.Zero;
						dust4.noLight = true;
					}
				}
			}
		}
		else if (typeCache == 165 && tileFrameX >= 162 && tileFrameX <= 214 && tileFrameY == 72)
		{
			if (_rand.Next(60) == 0)
			{
				int num13 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 + 6)), 8, 4, 153);
				_dust[num13].scale -= (float)_rand.Next(3) * 0.1f;
				_dust[num13].velocity.Y = 0f;
				_dust[num13].velocity.X *= 0.05f;
				_dust[num13].alpha = 100;
			}
		}
		else if (typeCache == 42 && tileFrameX == 0)
		{
			int num14 = tileFrameY / 36;
			if (tileFrameY / 18 % 2 == 1)
			{
				switch (num14)
				{
				case 7:
					if (_rand.Next(50) == 0)
					{
						int num17 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 + 4)), 8, 8, 58, 0f, 0f, 150);
						Dust obj14 = _dust[num17];
						obj14.velocity *= 0.5f;
					}
					if (_rand.Next(100) == 0)
					{
						int num18 = Gore.NewGore(new Vector2((float)(i * 16 - 2), (float)(j * 16 - 4)), default, _rand.Next(16, 18));
						_gore[num18].scale *= 0.7f;
						Gore obj15 = _gore[num18];
						obj15.velocity *= 0.25f;
					}
					break;
				case 29:
					if (_rand.Next(40) == 0)
					{
						int num19 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 8, 8, 59, 0f, 0f, 100);
						if (_rand.Next(3) != 0)
						{
							_dust[num19].noGravity = true;
						}
						Dust obj16 = _dust[num19];
						obj16.velocity *= 0.3f;
						_dust[num19].velocity.Y -= 1.5f;
					}
					break;
				case 50:
					if (_rand.Next(10) == 0)
					{
						int num16 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 8, 8, 57, 0f, 0f, 100);
						if (_rand.Next(3) != 0)
						{
							_dust[num16].noGravity = true;
						}
						Dust obj13 = _dust[num16];
						obj13.velocity *= 0.3f;
						_dust[num16].velocity.Y -= 1.5f;
					}
					break;
				case 51:
					if (_rand.Next(40) == 0)
					{
						int num15 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 + 2)), 4, 4, 242, 0f, 0f, 100);
						if (_rand.Next(3) != 0)
						{
							_dust[num15].noGravity = true;
						}
						Dust obj12 = _dust[num15];
						obj12.velocity *= 0.3f;
						_dust[num15].velocity.Y -= 1.5f;
					}
					break;
				}
			}
		}
		if (typeCache == 4 && _rand.Next(40) == 0 && tileFrameX < 66)
		{
			int num20 = (int)MathHelper.Clamp((float)(tileCache.frameY / 22), 0f, (float)(TorchID.Count - 1));
			int num21 = TorchID.Dust[num20];
			int num22 = 0;
			num22 = tileFrameX switch
			{
				22 => Dust.NewDust(new Vector2((float)(i * 16 + 6), (float)(j * 16)), 4, 4, num21, 0f, 0f, 100), 
				44 => Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16)), 4, 4, num21, 0f, 0f, 100), 
				_ => Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 4, 4, num21, 0f, 0f, 100), 
			};
			if (_rand.Next(3) != 0)
			{
				_dust[num22].noGravity = true;
			}
			Dust obj17 = _dust[num22];
			obj17.velocity *= 0.3f;
			_dust[num22].velocity.Y -= 1.5f;
			if (num21 == 66)
			{
				_dust[num22].color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				_dust[num22].noGravity = true;
			}
		}
		if (typeCache == 93 && _rand.Next(40) == 0 && tileFrameX == 0)
		{
			int num23 = tileFrameY / 54;
			if (tileFrameY / 18 % 3 == 0)
			{
				int num24;
				switch (num23)
				{
				case 0:
				case 6:
				case 7:
				case 8:
				case 10:
				case 14:
				case 15:
				case 16:
					num24 = 6;
					break;
				case 20:
					num24 = 59;
					break;
				case 44:
					num24 = 57;
					break;
				case 45:
					num24 = 242;
					break;
				default:
					num24 = -1;
					break;
				}
				if (num24 != -1)
				{
					int num25 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 + 2)), 4, 4, num24, 0f, 0f, 100);
					if (_rand.Next(3) != 0)
					{
						_dust[num25].noGravity = true;
					}
					Dust obj18 = _dust[num25];
					obj18.velocity *= 0.3f;
					_dust[num25].velocity.Y -= 1.5f;
				}
			}
		}
		if (typeCache == 100 && _rand.Next(40) == 0 && tileFrameX < 36)
		{
			int num26 = tileFrameY / 36;
			if (tileFrameY / 18 % 2 == 0)
			{
				int num27;
				switch (num26)
				{
				case 0:
				case 5:
				case 7:
				case 8:
				case 10:
				case 12:
				case 14:
				case 15:
				case 16:
					num27 = 6;
					break;
				case 20:
					num27 = 59;
					break;
				case 44:
					num27 = 57;
					break;
				case 45:
					num27 = 242;
					break;
				default:
					num27 = -1;
					break;
				}
				if (num27 != -1)
				{
					int num28 = 0;
					Vector2 position5;
					if (tileFrameX != 0)
					{
						position5 = ((_rand.Next(3) != 0) ? new Vector2((float)(i * 16), (float)(j * 16 + 2)) : new Vector2((float)(i * 16 + 6), (float)(j * 16 + 2)));
					}
					else
					{
						position5 = ((_rand.Next(3) != 0) ? new Vector2((float)(i * 16 + 14), (float)(j * 16 + 2)) : new Vector2((float)(i * 16 + 4), (float)(j * 16 + 2)));
					}
					num28 = Dust.NewDust(position5, 4, 4, num27, 0f, 0f, 100);
					if (_rand.Next(3) != 0)
					{
						_dust[num28].noGravity = true;
					}
					Dust obj19 = _dust[num28];
					obj19.velocity *= 0.3f;
					_dust[num28].velocity.Y -= 1.5f;
				}
			}
		}
		if (typeCache == 98 && _rand.Next(40) == 0 && tileFrameY == 0 && tileFrameX == 0)
		{
			int num29 = Dust.NewDust(new Vector2((float)(i * 16 + 12), (float)(j * 16 + 2)), 4, 4, 6, 0f, 0f, 100);
			if (_rand.Next(3) != 0)
			{
				_dust[num29].noGravity = true;
			}
			Dust obj20 = _dust[num29];
			obj20.velocity *= 0.3f;
			_dust[num29].velocity.Y -= 1.5f;
		}
		if (typeCache == 49 && tileFrameX == 0 && _rand.Next(2) == 0)
		{
			int num30 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 - 4)), 4, 4, 172, 0f, 0f, 100);
			if (_rand.Next(3) == 0)
			{
				_dust[num30].scale = 0.5f;
			}
			else
			{
				_dust[num30].scale = 0.9f;
				_dust[num30].noGravity = true;
			}
			Dust obj21 = _dust[num30];
			obj21.velocity *= 0.3f;
			_dust[num30].velocity.Y -= 1.5f;
		}
		if (typeCache == 372 && tileFrameX == 0 && _rand.Next(2) == 0)
		{
			int num31 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 - 4)), 4, 4, 242, 0f, 0f, 100);
			if (_rand.Next(3) == 0)
			{
				_dust[num31].scale = 0.5f;
			}
			else
			{
				_dust[num31].scale = 0.9f;
				_dust[num31].noGravity = true;
			}
			Dust obj22 = _dust[num31];
			obj22.velocity *= 0.3f;
			_dust[num31].velocity.Y -= 1.5f;
		}
		if (typeCache == 646 && tileFrameX == 0)
		{
			_rand.Next(2);
		}
		if (typeCache == 34 && _rand.Next(40) == 0 && tileFrameX % 108 < 54)
		{
			int num32 = tileFrameY / 54;
			if (tileFrameX >= 108)
			{
				num32 += 37 * (tileFrameX / 108);
			}
			int num33 = tileFrameX / 18 % 3;
			if (tileFrameY / 18 % 3 == 1 && num33 != 1)
			{
				int num34;
				switch (num32)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 12:
				case 13:
				case 16:
				case 19:
				case 21:
					num34 = 6;
					break;
				case 25:
					num34 = 59;
					break;
				case 50:
					num34 = 57;
					break;
				case 51:
					num34 = 242;
					break;
				default:
					num34 = -1;
					break;
				}
				if (num34 != -1)
				{
					int num35 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16 + 2)), 14, 6, num34, 0f, 0f, 100);
					if (_rand.Next(3) != 0)
					{
						_dust[num35].noGravity = true;
					}
					Dust obj23 = _dust[num35];
					obj23.velocity *= 0.3f;
					_dust[num35].velocity.Y -= 1.5f;
				}
			}
		}
		if (typeCache == 83)
		{
			int style = tileFrameX / 18;
			if (WorldGen.IsAlchemyPlantHarvestable(style, j))
			{
				EmitAlchemyHerbParticles(j, i, style);
			}
		}
		if (typeCache == 22 && _rand.Next(400) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14);
		}
		else if ((typeCache == 23 || typeCache == 24 || typeCache == 32) && _rand.Next(500) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14);
		}
		else if (typeCache == 25 && _rand.Next(700) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14);
		}
		else if (typeCache == 112 && _rand.Next(700) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14);
		}
		else if ((typeCache == 31 || typeCache == 696) && _rand.Next(20) == 0)
		{
			if (tileFrameX >= 36)
			{
				int num36 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 5, 0f, 0f, 100);
				_dust[num36].velocity.Y = 0f;
				_dust[num36].velocity.X *= 0.3f;
			}
			else
			{
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 100);
			}
		}
		else if ((typeCache == 26 || typeCache == 695) && _rand.Next(20) == 0)
		{
			if (tileFrameX >= 54)
			{
				int num37 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 5, 0f, 0f, 100);
				_dust[num37].scale = 1.5f;
				_dust[num37].noGravity = true;
				Dust obj24 = _dust[num37];
				obj24.velocity *= 0.75f;
			}
			else
			{
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 100);
			}
		}
		else if ((typeCache == 71 || typeCache == 72) && tileCache.color() == 0 && _rand.Next(500) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 41, 0f, 0f, 250, default, 0.8f);
		}
		else if ((typeCache == 17 || typeCache == 77 || typeCache == 133) && _rand.Next(40) == 0)
		{
			if ((tileFrameX == 18) & (tileFrameY == 18))
			{
				int num38 = Dust.NewDust(new Vector2((float)(i * 16 - 4), (float)(j * 16 - 6)), 8, 6, 6, 0f, 0f, 100);
				if (_rand.Next(3) != 0)
				{
					_dust[num38].noGravity = true;
				}
			}
		}
		else if (typeCache == 405 && _rand.Next(20) == 0)
		{
			if ((tileFrameX == 18) & (tileFrameY == 18))
			{
				int num39 = Dust.NewDust(new Vector2((float)(i * 16 - 4), (float)(j * 16 - 6)), 24, 10, 6, 0f, 0f, 100);
				if (_rand.Next(5) != 0)
				{
					_dust[num39].noGravity = true;
				}
			}
		}
		else if (typeCache == 37 && _rand.Next(250) == 0)
		{
			int num40 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 6, 0f, 0f, 0, default, _rand.Next(3));
			if (_dust[num40].scale > 1f)
			{
				_dust[num40].noGravity = true;
			}
		}
		else if ((typeCache == 58 || typeCache == 76 || typeCache == 684) && _rand.Next(250) == 0)
		{
			int num41 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 6, 0f, 0f, 0, default, _rand.Next(3));
			if (_dust[num41].scale > 1f)
			{
				_dust[num41].noGravity = true;
			}
			_dust[num41].noLight = true;
		}
		else if (typeCache == 61 || typeCache == 703)
		{
			if (tileFrameX == 144 && _rand.Next(60) == 0)
			{
				int num42 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 44, 0f, 0f, 250, default, 0.4f);
				_dust[num42].fadeIn = 0.7f;
			}
		}
		else if (Main.tileShine[typeCache] > 0)
		{
			if (tileLight.R <= 20 && tileLight.B <= 20 && tileLight.G <= 20)
			{
				return;
			}
			int num43 = tileLight.R;
			if (tileLight.G > num43)
			{
				num43 = tileLight.G;
			}
			if (tileLight.B > num43)
			{
				num43 = tileLight.B;
			}
			num43 /= 30;
			if (_rand.Next(Main.tileShine[typeCache]) >= num43 || ((typeCache == 21 || typeCache == 441) && (tileFrameX < 36 || tileFrameX >= 180) && (tileFrameX < 396 || tileFrameX > 409)) || ((typeCache == 467 || typeCache == 468) && (tileFrameX < 144 || tileFrameX >= 180)))
			{
				return;
			}
			Color newColor = Color.White;
			switch (typeCache)
			{
			case 617:
			{
				int x = i;
				int y = j;
				WorldGen.GetTopLeftAndStyles(ref x, ref y, 3, 4, 18, 18);
				int num45 = y;
				Tile tile = Main.tile[x + 1, y + 1];
				if (!IsVisible(tile))
				{
					num45 = y + 3;
				}
				if (j >= num45)
				{
					int num46 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
					Dust obj26 = _dust[num46];
					obj26.velocity *= 0f;
				}
				return;
			}
			case 178:
			{
				switch (tileFrameX / 18)
				{
				case 0:
					newColor = new Color(255, 0, 255, 255);
					break;
				case 1:
					newColor = new Color(255, 255, 0, 255);
					break;
				case 2:
					newColor = new Color(0, 0, 255, 255);
					break;
				case 3:
					newColor = new Color(0, 255, 0, 255);
					break;
				case 4:
					newColor = new Color(255, 0, 0, 255);
					break;
				case 5:
					newColor = new Color(255, 255, 255, 255);
					break;
				case 6:
					newColor = new Color(255, 255, 0, 255);
					break;
				}
				int num44 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
				Dust obj25 = _dust[num44];
				obj25.velocity *= 0f;
				return;
			}
			case 63:
				newColor = new Color(0, 0, 255, 255);
				break;
			}
			if (typeCache == 64)
			{
				newColor = new Color(255, 0, 0, 255);
			}
			if (typeCache == 65)
			{
				newColor = new Color(0, 255, 0, 255);
			}
			if (typeCache == 66)
			{
				newColor = new Color(255, 255, 0, 255);
			}
			if (typeCache == 67)
			{
				newColor = new Color(255, 0, 255, 255);
			}
			if (typeCache == 68)
			{
				newColor = new Color(255, 255, 255, 255);
			}
			if (typeCache == 566)
			{
				newColor = new Color(255, 255, 0, 255);
			}
			if (typeCache == 12 || typeCache == 665)
			{
				newColor = new Color(255, 0, 0, 255);
			}
			if (typeCache == 639)
			{
				newColor = new Color(0, 0, 255, 255);
			}
			if (typeCache == 204)
			{
				newColor = new Color(255, 0, 0, 255);
			}
			if (typeCache == 211)
			{
				newColor = new Color(50, 255, 100, 255);
			}
			int num47 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
			Dust obj27 = _dust[num47];
			obj27.velocity *= 0f;
		}
		else if (Main.tileSolid[tileCache.type] && Main.shimmerAlpha > 0f && (tileLight.R > 20 || tileLight.B > 20 || tileLight.G > 20))
		{
			int num48 = tileLight.R;
			if (tileLight.G > num48)
			{
				num48 = tileLight.G;
			}
			if (tileLight.B > num48)
			{
				num48 = tileLight.B;
			}
			int maxValue = 500;
			if ((float)_rand.Next(maxValue) < 2f * Main.shimmerAlpha)
			{
				Color white = Color.White;
				float scale2 = ((float)num48 / 255f + 1f) / 2f;
				int num49 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, white, scale2);
				Dust obj28 = _dust[num49];
				obj28.velocity *= 0f;
			}
		}
	}

	private void EmitLivingTreeLeaf(int i, int j, int leafGoreType)
	{
		EmitLivingTreeLeaf_Below(i, j, leafGoreType);
		if (_rand.Next(2) == 0)
		{
			EmitLivingTreeLeaf_Sideways(i, j, leafGoreType);
		}
	}

	private void EmitLivingTreeLeaf_Below(int x, int y, int leafGoreType)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[x, y + 1];
		if (!WorldGen.SolidTile(tile) && tile.liquid <= 0)
		{
			float windForVisuals = Main.WindForVisuals;
			if ((!(windForVisuals < -0.2f) || (!WorldGen.SolidTile(Main.tile[x - 1, y + 1]) && !WorldGen.SolidTile(Main.tile[x - 2, y + 1]))) && (!(windForVisuals > 0.2f) || (!WorldGen.SolidTile(Main.tile[x + 1, y + 1]) && !WorldGen.SolidTile(Main.tile[x + 2, y + 1]))))
			{
				Gore.NewGorePerfect(new Vector2((float)(x * 16), (float)(y * 16 + 16)), Vector2.Zero, leafGoreType).Frame.CurrentColumn = Main.tile[x, y].color();
			}
		}
	}

	private void EmitLivingTreeLeaf_Sideways(int x, int y, int leafGoreType)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		if (Main.WindForVisuals > 0.2f)
		{
			num = 1;
		}
		else if (Main.WindForVisuals < -0.2f)
		{
			num = -1;
		}
		Tile tile = Main.tile[x + num, y];
		if (!WorldGen.SolidTile(tile) && tile.liquid <= 0)
		{
			int num2 = 0;
			if (num == -1)
			{
				num2 = -10;
			}
			Gore.NewGorePerfect(new Vector2((float)(x * 16 + 8 + 4 * num + num2), (float)(y * 16 + 8)), Vector2.Zero, leafGoreType).Frame.CurrentColumn = Main.tile[x, y].color();
		}
	}

	private void EmitLiquidDrops(int j, int i, Tile tileCache, ushort typeCache)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		int num = 60;
		switch (typeCache)
		{
		case 374:
			num = 120;
			break;
		case 375:
			num = 180;
			break;
		case 461:
			num = 180;
			break;
		}
		if (tileCache.liquid != 0 || _rand.Next(num * 2) != 0)
		{
			return;
		}
		Rectangle val = new Rectangle(i * 16, j * 16, 16, 16);
		val.X -= 34;
		val.Width += 68;
		val.Y -= 100;
		val.Height = 400;
		for (int k = 0; k < 600; k++)
		{
			Gore gore = _gore[k];
			if (gore.active && gore.type >= 0 && gore.type < GoreID.Count && GoreID.Sets.IsDrip[gore.type])
			{
				Rectangle val2 = new Rectangle((int)gore.position.X, (int)gore.position.Y, 16, 16);
				if (val.Intersects(val2))
				{
					return;
				}
			}
		}
		Vector2 position = new Vector2((float)(i * 16), (float)(j * 16));
		int type = 706;
		if (Main.waterStyle == 14)
		{
			type = 706;
		}
		else if (Main.waterStyle == 13)
		{
			type = 706;
		}
		else if (Main.waterStyle == 12)
		{
			type = 1147;
		}
		else if (Main.waterStyle > 1)
		{
			type = 706 + Main.waterStyle - 1;
		}
		if (typeCache == 374)
		{
			type = 716;
		}
		if (typeCache == 375)
		{
			type = 717;
		}
		if (typeCache == 461)
		{
			type = 943;
			if (Main.SceneMetrics.ZoneCorrupt)
			{
				type = 1160;
			}
			if (Main.SceneMetrics.ZoneCrimson)
			{
				type = 1161;
			}
			if (Main.SceneMetrics.ZoneHallow)
			{
				type = 1162;
			}
		}
		if (typeCache == 709)
		{
			type = 1383;
		}
		int num2 = Gore.NewGore(position, default, type);
		Gore obj = _gore[num2];
		obj.velocity *= 0f;
	}

	private float GetWindCycle(int x, int y, double windCounter)
	{
		if (!Main.SettingsEnabled_TilesSwayInWind)
		{
			return 0f;
		}
		float num = (float)x * 0.5f + (float)(y / 100) * 0.5f;
		float num2 = (float)Math.Cos(windCounter * 6.2831854820251465 + (double)num) * 0.5f;
		if (Main.remixWorld)
		{
			if (!((double)y > Main.worldSurface))
			{
				return 0f;
			}
			num2 += Main.WindForVisuals;
		}
		else
		{
			if (!((double)y < Main.worldSurface))
			{
				return 0f;
			}
			num2 += Main.WindForVisuals;
		}
		float lerpValue = Utils.GetLerpValue(0.08f, 0.18f, Math.Abs(Main.WindForVisuals), clamped: true);
		return num2 * lerpValue;
	}

	private bool ShouldSwayInWind(int x, int y, Tile tileCache)
	{
		if (!Main.SettingsEnabled_TilesSwayInWind)
		{
			return false;
		}
		if (!TileID.Sets.SwaysInWindBasic[tileCache.type])
		{
			return false;
		}
		if (tileCache.type == 227 && (tileCache.frameX == 204 || tileCache.frameX == 238 || tileCache.frameX == 408 || tileCache.frameX == 442 || tileCache.frameX == 476))
		{
			return false;
		}
		return true;
	}

	private void UpdateLeafFrequency()
	{
		float num = Math.Abs(Main.WindForVisuals);
		if (num <= 0.1f)
		{
			_leafFrequency = 2000;
		}
		else if (num <= 0.2f)
		{
			_leafFrequency = 1000;
		}
		else if (num <= 0.3f)
		{
			_leafFrequency = 450;
		}
		else if (num <= 0.4f)
		{
			_leafFrequency = 300;
		}
		else if (num <= 0.5f)
		{
			_leafFrequency = 200;
		}
		else if (num <= 0.6f)
		{
			_leafFrequency = 130;
		}
		else if (num <= 0.7f)
		{
			_leafFrequency = 75;
		}
		else if (num <= 0.8f)
		{
			_leafFrequency = 50;
		}
		else if (num <= 0.9f)
		{
			_leafFrequency = 40;
		}
		else if (num <= 1f)
		{
			_leafFrequency = 30;
		}
		else if (num <= 1.1f)
		{
			_leafFrequency = 20;
		}
		else
		{
			_leafFrequency = 10;
		}
		_leafFrequency *= 7;
	}

	private void EnsureWindGridSize()
	{
		GetScreenDrawArea(!Main.drawToScreen, out var _, out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
		_windGrid.SetSize(lastTileX - firstTileX, lastTileY - firstTileY);
	}

	private void EmitTreeLeaves(int tilePosX, int tilePosY, int grassPosX, int grassPosY)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		if (!_isActiveAndNotPaused)
		{
			return;
		}
		int treeHeight = grassPosY - tilePosY;
		Tile tile = Main.tile[tilePosX, tilePosY];
		if (tile.liquid > 0)
		{
			return;
		}
		WorldGen.GetTreeLeaf(tilePosX, tile, Main.tile[grassPosX, grassPosY], ref treeHeight, out var _, out var passStyle);
		int num;
		switch (passStyle)
		{
		case -1:
		case 912:
		case 913:
		case 1278:
			return;
		default:
			num = ((passStyle >= 1113 && passStyle <= 1121) ? 1 : 0);
			break;
		case 917:
		case 918:
		case 919:
		case 920:
		case 921:
		case 922:
		case 923:
		case 924:
		case 925:
			num = 1;
			break;
		}
		bool flag = (byte)num != 0;
		int num2 = _leafFrequency;
		bool flag2 = tilePosX - grassPosX != 0;
		if (flag)
		{
			num2 /= 2;
		}
		if (!WorldGen.DoesWindBlowAtThisHeight(tilePosY))
		{
			num2 = 10000;
		}
		if (flag2)
		{
			num2 *= 3;
		}
		if (_rand.Next(num2) != 0)
		{
			return;
		}
		int num3 = 2;
		Vector2 val = new Vector2((float)(tilePosX * 16 + 8), (float)(tilePosY * 16 + 8));
		if (flag2)
		{
			int num4 = tilePosX - grassPosX;
			val.X += num4 * 12;
			int num5 = 0;
			if (tile.frameY == 220)
			{
				num5 = 1;
			}
			else if (tile.frameY == 242)
			{
				num5 = 2;
			}
			if (tile.frameX == 66)
			{
				switch (num5)
				{
				case 0:
					val += new Vector2(0f, -6f);
					break;
				case 1:
					val += new Vector2(0f, -6f);
					break;
				case 2:
					val += new Vector2(0f, 8f);
					break;
				}
			}
			else
			{
				switch (num5)
				{
				case 0:
					val += new Vector2(0f, 4f);
					break;
				case 1:
					val += new Vector2(2f, -6f);
					break;
				case 2:
					val += new Vector2(6f, -6f);
					break;
				}
			}
		}
		else
		{
			val += new Vector2(-16f, -16f);
			if (flag)
			{
				val.Y -= Main.rand.Next(0, 28) * 4;
			}
		}
		if (!WorldGen.SolidTile(val.ToTileCoordinates()))
		{
			Gore.NewGoreDirect(val, Utils.RandomVector2(Main.rand, -num3, num3), passStyle, 0.7f + Main.rand.NextFloat() * 0.6f).Frame.CurrentColumn = Main.tile[tilePosX, tilePosY].color();
		}
	}

	private void DrawSpecialTilesLegacy(Vector2 screenPosition, Vector2 offSet)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		if (_specialTilesCount == 0)
		{
			return;
		}
		RestartLayeredBatch();
		for (int i = 0; i < _specialTilesCount; i++)
		{
			int num = _specialTileX[i];
			int num2 = _specialTileY[i];
			Tile tile = Main.tile[num, num2];
			ushort type = tile.type;
			short frameX = tile.frameX;
			short frameY = tile.frameY;
			Main.tileBatch.SetLayer(0u, 0);
			if (type == 237)
			{
				Main.spriteBatch.Draw(TextureAssets.SunOrb.Value, new Vector2((float)(num * 16 - (int)screenPosition.X) + 8f, (float)(num2 * 16 - (int)screenPosition.Y - 36)) + offSet, (Rectangle?)new Rectangle(0, 0, TextureAssets.SunOrb.Width(), TextureAssets.SunOrb.Height()), new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 0), Main.sunCircle, new Vector2((float)(TextureAssets.SunOrb.Width() / 2), (float)(TextureAssets.SunOrb.Height() / 2)), 1f, (SpriteEffects)0, 0f);
			}
			if (type == 334 && frameX >= 5000)
			{
				int num3 = frameX;
				int num4 = 0;
				int num5 = num3 % 5000;
				num5 -= 100;
				while (num3 >= 5000)
				{
					num4++;
					num3 -= 5000;
				}
				int frameX2 = Main.tile[num + 1, num2].frameX;
				frameX2 = ((frameX2 < 25000) ? (frameX2 - 10000) : (frameX2 - 25000));
				Item item = new Item();
				item.netDefaults(num5);
				item.Prefix(frameX2);
				Main.instance.LoadItem(item.type);
				Texture2D value = TextureAssets.Item[item.type].Value;
				Rectangle val = ((Main.itemAnimations[item.type] == null) ? value.Frame() : Main.itemAnimations[item.type].GetFrame(value));
				int width = val.Width;
				int height = val.Height;
				float num6 = 1f;
				if (width > 40 || height > 40)
				{
					num6 = ((width <= height) ? (40f / (float)height) : (40f / (float)width));
				}
				num6 *= item.scale;
				SpriteEffects effects = (SpriteEffects)0;
				if (num4 >= 3)
				{
					effects = (SpriteEffects)1;
				}
				Color color = Lighting.GetColor(num, num2);
				Main.tileBatch.Draw(value, new Vector2((float)(num * 16 - (int)screenPosition.X + 24), (float)(num2 * 16 - (int)screenPosition.Y + 8)) + offSet, val, Lighting.GetColor(num, num2), new Vector2((float)(width / 2), (float)(height / 2)), num6, effects);
				if (item.color != default(Color))
				{
					Main.tileBatch.Draw(value, new Vector2((float)(num * 16 - (int)screenPosition.X + 24), (float)(num2 * 16 - (int)screenPosition.Y + 8)) + offSet, val, item.GetColor(color), new Vector2((float)(width / 2), (float)(height / 2)), num6, effects);
				}
			}
			if (type == 395 && TileEntity.TryGetAt<TEItemFrame>(num, num2, out var result))
			{
				Item item2 = result.item;
				if (!item2.IsAir)
				{
					Vector2 screenPositionForItemCenter = new Vector2((float)(num * 16 - (int)screenPosition.X + 16), (float)(num2 * 16 - (int)screenPosition.Y + 16)) + offSet;
					Color color2 = Lighting.GetColor(num, num2);
					ItemSlot.DrawItemIcon(item2, 40, Main.spriteBatch, screenPositionForItemCenter, item2.scale, 20f, color2);
				}
			}
			if (type == 520 && TileEntity.TryGetAt<TEFoodPlatter>(num, num2, out var result2))
			{
				Item item3 = result2.item;
				if (!item3.IsAir)
				{
					Main.instance.LoadItem(item3.type);
					Texture2D value2 = TextureAssets.Item[item3.type].Value;
					Rectangle val2 = ((!ItemID.Sets.IsFood[item3.type]) ? value2.Frame() : value2.Frame(1, 3, 0, 2));
					int width2 = val2.Width;
					int height2 = val2.Height;
					float num7 = 1f;
					SpriteEffects effects2 = (tile.frameX == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
					Color color3 = Lighting.GetColor(num, num2);
					Color currentColor = color3;
					float scale = 1f;
					ItemSlot.GetItemLight(ref currentColor, ref scale, item3);
					num7 *= scale;
					Vector2 position = new Vector2((float)(num * 16 - (int)screenPosition.X + 8), (float)(num2 * 16 - (int)screenPosition.Y + 16)) + offSet;
					position.Y += 2f;
					Vector2 origin = new Vector2((float)(width2 / 2), (float)height2);
					Main.tileBatch.Draw(value2, position, val2, currentColor, origin, num7, effects2);
					if (item3.color != default(Color))
					{
						Main.tileBatch.Draw(value2, position, val2, item3.GetColor(color3), origin, num7, effects2);
					}
				}
			}
			if (type == 471 && TileEntity.TryGetAt<TEWeaponsRack>(num, num2, out var result3))
			{
				Item item4 = result3.item;
				if (!item4.IsAir)
				{
					Vector2 screenPositionForItemCenter2 = new Vector2((float)(num * 16 - (int)screenPosition.X + 24), (float)(num2 * 16 - (int)screenPosition.Y + 24)) + offSet;
					Color color4 = Lighting.GetColor(num, num2);
					bool flip = true;
					if (tile.frameX < 54)
					{
						flip = false;
					}
					ItemSlot.DrawItemIcon(item4, 40, Main.spriteBatch, screenPositionForItemCenter2, item4.scale, 40f, color4, 1f, flip);
				}
			}
			if (type == 620)
			{
				Texture2D value3 = TextureAssets.Extra[202].Value;
				int num8 = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame = GetWaterAnimalCageFrame(num, num2, frameX, frameY);
				int num9 = 8;
				int num10 = Main.butterflyCageFrame[num9, waterAnimalCageFrame];
				int num11 = 6;
				float num12 = 1f;
				Rectangle sourceRectangle = new Rectangle(0, 34 * num10, 32, 32);
				Vector2 val3 = new Vector2((float)(num * 16 - (int)screenPosition.X), (float)(num2 * 16 - (int)screenPosition.Y + num8)) + offSet;
				Main.tileBatch.Draw(value3, val3, sourceRectangle, new Color(255, 255, 255, 255), Vector2.Zero, 1f, (SpriteEffects)0);
				for (int j = 0; j < num11; j++)
				{
					Color val4 = new Color(127, 127, 127, 0).MultiplyRGBA(Main.hslToRgb((Main.GlobalTimeWrappedHourly + (float)j / (float)num11) % 1f, 1f, 0.5f));
					val4 *= 1f - num12 * 0.5f;
					val4.A = 0;
					int num13 = 2;
					Vector2 position2 = val3 + ((float)j / (float)num11 * ((float)Math.PI * 2f)).ToRotationVector2() * ((float)num13 * num12 + 2f);
					Main.tileBatch.Draw(value3, position2, sourceRectangle, val4, Vector2.Zero, 1f, (SpriteEffects)0);
				}
				Main.tileBatch.Draw(value3, val3, sourceRectangle, new Color(255, 255, 255, 0) * 0.1f, Vector2.Zero, 1f, (SpriteEffects)0);
			}
		}
	}

	private void DrawEntities_DisplayDolls()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, (Effect)null, Main.Transform);
		foreach (KeyValuePair<Point, int> displayDollTileEntityPosition in _displayDollTileEntityPositions)
		{
			if (displayDollTileEntityPosition.Value != -1 && TileEntity.TryGetAt<TEDisplayDoll>(displayDollTileEntityPosition.Key.X, displayDollTileEntityPosition.Key.Y, out var result))
			{
				result.Draw(displayDollTileEntityPosition.Key.X, displayDollTileEntityPosition.Key.Y);
			}
		}
		Main.spriteBatch.End();
	}

	private void DrawEntities_HatRacks()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, (Effect)null, Main.Transform);
		foreach (KeyValuePair<Point, int> hatRackTileEntityPosition in _hatRackTileEntityPositions)
		{
			if (hatRackTileEntityPosition.Value != -1 && TileEntity.TryGetAt<TEHatRack>(hatRackTileEntityPosition.Key.X, hatRackTileEntityPosition.Key.Y, out var result))
			{
				result.Draw(hatRackTileEntityPosition.Key.X, hatRackTileEntityPosition.Key.Y);
			}
		}
		Main.spriteBatch.End();
	}

	private void DrawTrees()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_0997: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 0;
		int num2 = _specialsCount[num];
		float num3 = 0.08f;
		float num4 = 0.06f;
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.active())
			{
				continue;
			}
			ushort type = tile.type;
			short frameX = tile.frameX;
			short frameY = tile.frameY;
			bool flag = tile.wall > 0;
			WorldGen.GetTreeFoliageDataMethod getTreeFoliageDataMethod = null;
			try
			{
				bool flag2 = false;
				switch (type)
				{
				case 5:
					flag2 = true;
					getTreeFoliageDataMethod = WorldGen.GetCommonTreeFoliageData;
					break;
				case 583:
				case 584:
				case 585:
				case 586:
				case 587:
				case 588:
				case 589:
					flag2 = true;
					getTreeFoliageDataMethod = WorldGen.GetGemTreeFoliageData;
					break;
				case 596:
				case 616:
					flag2 = true;
					getTreeFoliageDataMethod = WorldGen.GetVanityTreeFoliageData;
					break;
				case 634:
					flag2 = true;
					getTreeFoliageDataMethod = WorldGen.GetAshTreeFoliageData;
					break;
				}
				if (flag2 && frameY >= 198 && frameX >= 22)
				{
					int treeFrame = WorldGen.GetTreeFrame(tile);
					switch (frameX)
					{
					case 22:
					{
						int treeStyle3 = 0;
						int topTextureFrameWidth3 = 80;
						int topTextureFrameHeight3 = 80;
						int num13 = 0;
						int grassPosX = x + num13;
						int floorY3 = y;
						if (!getTreeFoliageDataMethod(x, y, num13, ref treeFrame, ref treeStyle3, out floorY3, out topTextureFrameWidth3, out topTextureFrameHeight3))
						{
							continue;
						}
						EmitTreeLeaves(x, y, grassPosX, floorY3);
						if (treeStyle3 == 14)
						{
							float num14 = (float)_rand.Next(28, 42) * 0.005f;
							num14 += (float)(270 - Main.mouseTextColor) / 1000f;
							if (tile.color() == 0)
							{
								Lighting.AddLight(x, y, 0.1f, 0.2f + num14 / 2f, 0.7f + num14);
							}
							else
							{
								Color val4 = WorldGen.paintColor(tile.color());
								float r3 = (float)(int)val4.R / 255f;
								float g3 = (float)(int)val4.G / 255f;
								float b3 = (float)(int)val4.B / 255f;
								Lighting.AddLight(x, y, r3, g3, b3);
							}
						}
						byte tileColor3 = tile.color();
						Texture2D treeTopTexture = GetTreeTopTexture(treeStyle3, 0, tileColor3);
						Vector2 position3 = (position3 = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8), (float)(y * 16 - (int)unscaledPosition.Y + 16)) + zero);
						float num15 = 0f;
						if (!flag)
						{
							num15 = GetWindCycle(x, y, _treeWindCounter);
						}
						position3.X += num15 * 2f;
						position3.Y += Math.Abs(num15) * 2f;
						Color color3 = Lighting.GetColor(x, y);
						if (tile.fullbrightBlock())
						{
							color3 = Color.White;
						}
						DrawNature(treeTopTexture, position3, new Rectangle(treeFrame * (topTextureFrameWidth3 + 2), 0, topTextureFrameWidth3, topTextureFrameHeight3), color3, num15 * num3, new Vector2((float)(topTextureFrameWidth3 / 2), (float)topTextureFrameHeight3), 1f, (SpriteEffects)0, 0f);
						if (type == 634)
						{
							Texture2D value3 = TextureAssets.GlowMask[316].Value;
							Color white3 = Color.White;
							DrawNatureGlowmask(value3, position3, new Rectangle(treeFrame * (topTextureFrameWidth3 + 2), 0, topTextureFrameWidth3, topTextureFrameHeight3), white3, num15 * num3, new Vector2((float)(topTextureFrameWidth3 / 2), (float)topTextureFrameHeight3), 1f, (SpriteEffects)0, 0f);
						}
						break;
					}
					case 44:
					{
						int treeStyle2 = 0;
						int num9 = x;
						int floorY2 = y;
						int num10 = 1;
						if (!getTreeFoliageDataMethod(x, y, num10, ref treeFrame, ref treeStyle2, out floorY2, out var _, out var _))
						{
							continue;
						}
						EmitTreeLeaves(x, y, num9 + num10, floorY2);
						if (treeStyle2 == 14)
						{
							float num11 = (float)_rand.Next(28, 42) * 0.005f;
							num11 += (float)(270 - Main.mouseTextColor) / 1000f;
							if (tile.color() == 0)
							{
								Lighting.AddLight(x, y, 0.1f, 0.2f + num11 / 2f, 0.7f + num11);
							}
							else
							{
								Color val3 = WorldGen.paintColor(tile.color());
								float r2 = (float)(int)val3.R / 255f;
								float g2 = (float)(int)val3.G / 255f;
								float b2 = (float)(int)val3.B / 255f;
								Lighting.AddLight(x, y, r2, g2, b2);
							}
						}
						byte tileColor2 = tile.color();
						Texture2D treeBranchTexture2 = GetTreeBranchTexture(treeStyle2, 0, tileColor2);
						Vector2 position2 = new Vector2((float)(x * 16), (float)(y * 16)) - unscaledPosition.Floor() + zero + new Vector2(16f, 12f);
						float num12 = 0f;
						if (!flag)
						{
							num12 = GetWindCycle(x, y, _treeWindCounter);
						}
						if (num12 > 0f)
						{
							position2.X += num12;
						}
						position2.X += Math.Abs(num12) * 2f;
						Color color2 = Lighting.GetColor(x, y);
						if (tile.fullbrightBlock())
						{
							color2 = Color.White;
						}
						DrawNature(treeBranchTexture2, position2, new Rectangle(0, treeFrame * 42, 40, 40), color2, num12 * num4, new Vector2(40f, 24f), 1f, (SpriteEffects)0, 0f);
						if (type == 634)
						{
							Texture2D value2 = TextureAssets.GlowMask[317].Value;
							Color white2 = Color.White;
							DrawNatureGlowmask(value2, position2, new Rectangle(0, treeFrame * 42, 40, 40), white2, num12 * num4, new Vector2(40f, 24f), 1f, (SpriteEffects)0, 0f);
						}
						break;
					}
					case 66:
					{
						int treeStyle = 0;
						int num5 = x;
						int floorY = y;
						int num6 = -1;
						if (!getTreeFoliageDataMethod(x, y, num6, ref treeFrame, ref treeStyle, out floorY, out var _, out var _))
						{
							continue;
						}
						EmitTreeLeaves(x, y, num5 + num6, floorY);
						if (treeStyle == 14)
						{
							float num7 = (float)_rand.Next(28, 42) * 0.005f;
							num7 += (float)(270 - Main.mouseTextColor) / 1000f;
							if (tile.color() == 0)
							{
								Lighting.AddLight(x, y, 0.1f, 0.2f + num7 / 2f, 0.7f + num7);
							}
							else
							{
								Color val2 = WorldGen.paintColor(tile.color());
								float r = (float)(int)val2.R / 255f;
								float g = (float)(int)val2.G / 255f;
								float b = (float)(int)val2.B / 255f;
								Lighting.AddLight(x, y, r, g, b);
							}
						}
						byte tileColor = tile.color();
						Texture2D treeBranchTexture = GetTreeBranchTexture(treeStyle, 0, tileColor);
						Vector2 position = new Vector2((float)(x * 16), (float)(y * 16)) - unscaledPosition.Floor() + zero + new Vector2(0f, 18f);
						float num8 = 0f;
						if (!flag)
						{
							num8 = GetWindCycle(x, y, _treeWindCounter);
						}
						if (num8 < 0f)
						{
							position.X += num8;
						}
						position.X -= Math.Abs(num8) * 2f;
						Color color = Lighting.GetColor(x, y);
						if (tile.fullbrightBlock())
						{
							color = Color.White;
						}
						DrawNature(treeBranchTexture, position, new Rectangle(42, treeFrame * 42, 40, 40), color, num8 * num4, new Vector2(0f, 30f), 1f, (SpriteEffects)0, 0f);
						if (type == 634)
						{
							Texture2D value = TextureAssets.GlowMask[317].Value;
							Color white = Color.White;
							DrawNatureGlowmask(value, position, new Rectangle(42, treeFrame * 42, 40, 40), white, num8 * num4, new Vector2(0f, 30f), 1f, (SpriteEffects)0, 0f);
						}
						break;
					}
					}
				}
				if (type == 323 && frameX >= 88 && frameX <= 132)
				{
					int num16 = 0;
					switch (frameX)
					{
					case 110:
						num16 = 1;
						break;
					case 132:
						num16 = 2;
						break;
					}
					int treeTextureIndex = 15;
					int num17 = 80;
					int num18 = 80;
					int num19 = 32;
					int num20 = 0;
					int palmTreeBiome = GetPalmTreeBiome(x, y);
					int num21 = palmTreeBiome * 82;
					if (palmTreeBiome >= 4 && palmTreeBiome <= 7)
					{
						treeTextureIndex = 21;
						num17 = 114;
						num18 = 98;
						num21 = (palmTreeBiome - 4) * 98;
						num19 = 48;
						num20 = 2;
					}
					int frameY2 = Main.tile[x, y].frameY;
					byte tileColor4 = tile.color();
					Texture2D treeTopTexture2 = GetTreeTopTexture(treeTextureIndex, palmTreeBiome, tileColor4);
					Vector2 position4 = new Vector2((float)(x * 16 - (int)unscaledPosition.X - num19 + frameY2 + num17 / 2), (float)(y * 16 - (int)unscaledPosition.Y + 16 + num20)) + zero;
					float num22 = 0f;
					if (!flag)
					{
						num22 = GetWindCycle(x, y, _treeWindCounter);
					}
					position4.X += num22 * 2f;
					position4.Y += Math.Abs(num22) * 2f;
					Color color4 = Lighting.GetColor(x, y);
					if (tile.fullbrightBlock())
					{
						color4 = Color.White;
					}
					DrawNature(treeTopTexture2, position4, new Rectangle(num16 * (num17 + 2), num21, num17, num18), color4, num22 * num3, new Vector2((float)(num17 / 2), (float)num18), 1f, (SpriteEffects)0, 0f);
				}
			}
			catch
			{
			}
		}
	}

	private Texture2D GetTreeTopTexture(int treeTextureIndex, int treeTextureStyle, byte tileColor)
	{
		Texture2D val = _paintSystem.TryGetTreeTopAndRequestIfNotReady(treeTextureIndex, treeTextureStyle, tileColor);
		if (val == null)
		{
			val = TextureAssets.TreeTop[treeTextureIndex].Value;
		}
		return val;
	}

	private Texture2D GetTreeBranchTexture(int treeTextureIndex, int treeTextureStyle, byte tileColor)
	{
		Texture2D val = _paintSystem.TryGetTreeBranchAndRequestIfNotReady(treeTextureIndex, treeTextureStyle, tileColor);
		if (val == null)
		{
			val = TextureAssets.TreeBranch[treeTextureIndex].Value;
		}
		return val;
	}

	private void DrawGrass()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 1;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.active() || !IsVisible(tile))
			{
				continue;
			}
			ushort type = tile.type;
			short tileFrameX = tile.frameX;
			short tileFrameY = tile.frameY;
			GetTileDrawData(x, y, tile, type, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var glowTexture, out var glowSourceRect, out var glowColor);
			bool flag = _rand.Next(4) == 0;
			Color tileLight = Lighting.GetColor(x, y);
			DrawAnimatedTile_AdjustForVisionChangers(x, y, tile, type, tileFrameX, tileFrameY, ref tileLight, flag);
			tileLight = DrawTiles_GetLightOverride(y, x, tile, type, tileFrameX, tileFrameY, tileLight);
			if (_isActiveAndNotPaused & flag)
			{
				DrawTiles_EmitParticles(y, x, tile, type, tileFrameX, tileFrameY, tileLight);
			}
			if (type == 83 && WorldGen.IsAlchemyPlantHarvestable(tileFrameX / 18, y))
			{
				type = 84;
				Main.instance.LoadTiles(type);
			}
			Vector2 position = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8), (float)(y * 16 - (int)unscaledPosition.Y + 16)) + zero;
			float num3 = GetWindCycle(x, y, _grassWindCounter);
			if (!WallID.Sets.AllowsWind[tile.wall])
			{
				num3 = 0f;
			}
			if (!InAPlaceWithWind(x, y, 1, 1))
			{
				num3 = 0f;
			}
			num3 += GetWindGridPush(x, y, 20, 0.35f);
			position.X += num3 * 1f;
			position.Y += Math.Abs(num3) * 1f;
			Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, y);
			if (tileDrawTexture != null)
			{
				DrawNature(tileDrawTexture, position, new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), tileLight, num3 * 0.1f, new Vector2((float)(tileWidth / 2), (float)(16 - halfBrickHeight - tileTop)), 1f, tileSpriteEffect, 0f);
				if (glowTexture != null)
				{
					DrawNatureGlowmask(glowTexture, position, glowSourceRect, glowColor, num3 * 0.1f, new Vector2((float)(tileWidth / 2), (float)(16 - halfBrickHeight - tileTop)), 1f, tileSpriteEffect, 0f);
				}
			}
		}
	}

	private void DrawAnyDirectionalGrass()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 10;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.active() || !IsVisible(tile))
			{
				continue;
			}
			ushort type = tile.type;
			short tileFrameX = tile.frameX;
			short tileFrameY = tile.frameY;
			GetTileDrawData(x, y, tile, type, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var glowTexture, out var _, out var glowColor);
			bool flag = _rand.Next(4) == 0;
			Color tileLight = Lighting.GetColor(x, y);
			DrawAnimatedTile_AdjustForVisionChangers(x, y, tile, type, tileFrameX, tileFrameY, ref tileLight, flag);
			tileLight = DrawTiles_GetLightOverride(y, x, tile, type, tileFrameX, tileFrameY, tileLight);
			if (_isActiveAndNotPaused & flag)
			{
				DrawTiles_EmitParticles(y, x, tile, type, tileFrameX, tileFrameY, tileLight);
			}
			if (type == 83 && WorldGen.IsAlchemyPlantHarvestable(tileFrameX / 18, y))
			{
				type = 84;
				Main.instance.LoadTiles(type);
			}
			Vector2 position = new Vector2((float)(x * 16 - (int)unscaledPosition.X), (float)(y * 16 - (int)unscaledPosition.Y)) + zero;
			float num3 = GetWindCycle(x, y, _grassWindCounter);
			if (!WallID.Sets.AllowsWind[tile.wall])
			{
				num3 = 0f;
			}
			if (!InAPlaceWithWind(x, y, 1, 1))
			{
				num3 = 0f;
			}
			GetWindGridPush2Axis(x, y, 20, 0.35f, out var pushX, out var pushY);
			int num4 = 1;
			int num5 = 0;
			Vector2 origin = new Vector2((float)(tileWidth / 2), (float)(16 - halfBrickHeight - tileTop));
			switch (tileFrameY / 54)
			{
			case 0:
				num4 = 1;
				num5 = 0;
				origin = new Vector2((float)(tileWidth / 2), (float)(16 - halfBrickHeight - tileTop));
				position.X += 8f;
				position.Y += 16f;
				position.X += num3;
				position.Y += Math.Abs(num3);
				break;
			case 1:
				num3 *= -1f;
				num4 = -1;
				num5 = 0;
				origin = new Vector2((float)(tileWidth / 2), (float)(-tileTop));
				position.X += 8f;
				position.X += 0f - num3;
				position.Y += 0f - Math.Abs(num3);
				break;
			case 2:
				num4 = 0;
				num5 = 1;
				origin = new Vector2(2f, (float)((16 - halfBrickHeight - tileTop) / 2));
				position.Y += 8f;
				position.Y += num3;
				position.X += 0f - Math.Abs(num3);
				break;
			case 3:
				num3 *= -1f;
				num4 = 0;
				num5 = -1;
				origin = new Vector2(14f, (float)((16 - halfBrickHeight - tileTop) / 2));
				position.X += 16f;
				position.Y += 8f;
				position.Y += 0f - num3;
				position.X += Math.Abs(num3);
				break;
			}
			num3 += pushX * (float)num4 + pushY * (float)num5;
			Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, y);
			if (tileDrawTexture != null)
			{
				DrawNature(tileDrawTexture, position, new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), tileLight, num3 * 0.1f, origin, 1f, tileSpriteEffect, 0f);
				if (glowTexture != null)
				{
					DrawNatureGlowmask(glowTexture, position, new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), glowColor, num3 * 0.1f, origin, 1f, tileSpriteEffect, 0f);
				}
			}
		}
	}

	private void DrawAnimatedTile_AdjustForVisionChangers(int i, int j, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, ref Color tileLight, bool canDoDust)
	{
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		if (_perspectivePlayer.dangerSense && IsTileDangerous(_perspectivePlayer, i, j, tileCache, typeCache))
		{
			if (tileLight.R < byte.MaxValue)
			{
				tileLight.R = byte.MaxValue;
			}
			if (tileLight.G < 50)
			{
				tileLight.G = 50;
			}
			if (tileLight.B < 50)
			{
				tileLight.B = 50;
			}
			if ((_isActiveAndNotPaused & canDoDust) && _rand.Next(30) == 0)
			{
				int num = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 60, 0f, 0f, 100, default, 0.3f);
				_dust[num].fadeIn = 1f;
				Dust obj = _dust[num];
				obj.velocity *= 0.1f;
				_dust[num].noLight = true;
				_dust[num].noGravity = true;
			}
		}
		if (_perspectivePlayer.findTreasure && Main.IsTileSpelunkable(typeCache, tileFrameX, tileFrameY))
		{
			if (tileLight.R < 200)
			{
				tileLight.R = 200;
			}
			if (tileLight.G < 170)
			{
				tileLight.G = 170;
			}
			if (_isActiveAndNotPaused && ((_rand.Next(60) == 0) & canDoDust))
			{
				int num2 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 204, 0f, 0f, 150, default, 0.3f);
				_dust[num2].fadeIn = 1f;
				Dust obj2 = _dust[num2];
				obj2.velocity *= 0.1f;
				_dust[num2].noLight = true;
			}
		}
		if (!_perspectivePlayer.biomeSight)
		{
			return;
		}
		Color sightColor = Color.White;
		if (Main.IsTileBiomeSightable(typeCache, tileFrameX, tileFrameY, ref sightColor))
		{
			if (tileLight.R < sightColor.R)
			{
				tileLight.R = sightColor.R;
			}
			if (tileLight.G < sightColor.G)
			{
				tileLight.G = sightColor.G;
			}
			if (tileLight.B < sightColor.B)
			{
				tileLight.B = sightColor.B;
			}
			if ((_isActiveAndNotPaused & canDoDust) && _rand.Next(480) == 0)
			{
				Color newColor = sightColor;
				int num3 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 267, 0f, 0f, 150, newColor, 0.3f);
				_dust[num3].noGravity = true;
				_dust[num3].fadeIn = 1f;
				Dust obj3 = _dust[num3];
				obj3.velocity *= 0.1f;
				_dust[num3].noLightEmittance = true;
			}
		}
	}

	private float GetWindGridPush(int i, int j, int pushAnimationTimeTotal, float pushForcePerFrame)
	{
		_windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out var windTimeLeft, out var directionX, out var _);
		if (windTimeLeft >= pushAnimationTimeTotal / 2)
		{
			return (float)(pushAnimationTimeTotal - windTimeLeft) * pushForcePerFrame * (float)directionX;
		}
		return (float)windTimeLeft * pushForcePerFrame * (float)directionX;
	}

	private void GetWindGridPush2Axis(int i, int j, int pushAnimationTimeTotal, float pushForcePerFrame, out float pushX, out float pushY)
	{
		_windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out var windTimeLeft, out var directionX, out var directionY);
		if (windTimeLeft >= pushAnimationTimeTotal / 2)
		{
			pushX = (float)(pushAnimationTimeTotal - windTimeLeft) * pushForcePerFrame * (float)directionX;
			pushY = (float)(pushAnimationTimeTotal - windTimeLeft) * pushForcePerFrame * (float)directionY;
		}
		else
		{
			pushX = (float)windTimeLeft * pushForcePerFrame * (float)directionX;
			pushY = (float)windTimeLeft * pushForcePerFrame * (float)directionY;
		}
	}

	private float GetWindGridPushComplex(int i, int j, int pushAnimationTimeTotal, float totalPushForce, int loops, bool flipDirectionPerLoop)
	{
		_windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out var windTimeLeft, out var directionX, out var _);
		float num = (float)windTimeLeft / (float)pushAnimationTimeTotal;
		int num2 = (int)(num * (float)loops);
		float num3 = num * (float)loops % 1f;
		_ = 1f / (float)loops;
		if (flipDirectionPerLoop && num2 % 2 == 1)
		{
			directionX *= -1;
		}
		if (num * (float)loops % 1f > 0.5f)
		{
			return (1f - num3) * totalPushForce * (float)directionX * (float)(loops - num2);
		}
		return num3 * totalPushForce * (float)directionX * (float)(loops - num2);
	}

	private void DrawMasterTrophies()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		int num = 9;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			Tile tile = Main.tile[val.X, val.Y];
			if (tile == null || !tile.active())
			{
				continue;
			}
			Tile tile2 = Main.tile[val.X + 1, val.Y + 1];
			if (tile2 != null && tile2.active() && IsVisible(tile2))
			{
				Texture2D value = TextureAssets.Extra[198].Value;
				int frameY = tile.frameX / 54;
				bool flag = tile.frameY / 72 != 0;
				int horizontalFrames = 1;
				int verticalFrames = 28;
				Rectangle val2 = value.Frame(horizontalFrames, verticalFrames, 0, frameY);
				Vector2 val3 = val2.Size() / 2f;
				Vector2 val4 = val.ToWorldCoordinates(24f, 64f);
				float num3 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 5f);
				Vector2 val5 = val4 + new Vector2(0f, -40f) + new Vector2(0f, num3 * 4f);
				Color val6 = Lighting.GetColor(val.X, val.Y);
				if (tile2.fullbrightBlock())
				{
					val6 = Color.White;
				}
				SpriteEffects val7 = (SpriteEffects)(flag ? 1 : 0);
				Main.spriteBatch.Draw(value, val5 - Main.screenPosition, (Rectangle?)val2, val6, 0f, val3, 1f, val7, 0f);
				float num4 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 2f) * 0.3f + 0.7f;
				Color val8 = val6;
				val8.A = 0;
				val8 = val8 * 0.1f * num4;
				for (float num5 = 0f; num5 < 1f; num5 += 1f / 6f)
				{
					Main.spriteBatch.Draw(value, val5 - Main.screenPosition + ((float)Math.PI * 2f * num5).ToRotationVector2() * (6f + num3 * 2f), (Rectangle?)val2, val8, 0f, val3, 1f, val7, 0f);
				}
			}
		}
	}

	private void DrawTeleportationPylons()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		int num = 8;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			Tile tile = Main.tile[val.X, val.Y];
			if (tile == null || !tile.active())
			{
				continue;
			}
			Tile tile2 = Main.tile[val.X + 1, val.Y + 1];
			if (tile2 == null || !tile2.active() || !IsVisible(tile2))
			{
				continue;
			}
			Texture2D value = TextureAssets.Extra[181].Value;
			int num3 = tile.frameX / 54;
			int num4 = 3;
			int horizontalFrames = num4 + 11;
			int verticalFrames = 8;
			int frameY = (Main.tileFrameCounter[597] + val.X + val.Y) % 64 / 8;
			Rectangle val2 = value.Frame(horizontalFrames, verticalFrames, num4 + num3, frameY);
			Rectangle value2 = value.Frame(horizontalFrames, verticalFrames, 2, frameY);
			value.Frame(horizontalFrames, verticalFrames, 0, frameY);
			Vector2 val3 = val2.Size() / 2f;
			Vector2 val4 = val.ToWorldCoordinates(24f, 64f);
			float num5 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 5f);
			Vector2 val5 = val4 + new Vector2(0f, -40f) + new Vector2(0f, num5 * 4f);
			bool flag = _rand.Next(4) == 0;
			if ((_isActiveAndNotPaused & flag) && _rand.Next(10) == 0)
			{
				Rectangle dustBox = Utils.CenteredRectangle(val5, val2.Size());
				TeleportPylonsSystem.SpawnInWorldDust(num3, dustBox);
			}
			Color val6 = Lighting.GetColor(val.X, val.Y);
			if (tile2.fullbrightBlock())
			{
				val6 = Color.White;
			}
			val6 = Color.Lerp(val6, Color.White, 0.8f);
			Main.spriteBatch.Draw(value, val5 - Main.screenPosition, (Rectangle?)val2, val6 * 0.7f, 0f, val3, 1f, (SpriteEffects)0, 0f);
			float num6 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 1f) * 0.2f + 0.8f;
			Color val7 = new Color(255, 255, 255, 0) * 0.1f * num6;
			for (float num7 = 0f; num7 < 1f; num7 += 1f / 6f)
			{
				Main.spriteBatch.Draw(value, val5 - Main.screenPosition + ((float)Math.PI * 2f * num7).ToRotationVector2() * (6f + num5 * 2f), (Rectangle?)val2, val7, 0f, val3, 1f, (SpriteEffects)0, 0f);
			}
			int num8 = 0;
			if (Main.InSmartCursorHighlightArea(val.X, val.Y, out var actuallySelected))
			{
				num8 = 1;
				if (actuallySelected)
				{
					num8 = 2;
				}
			}
			if (num8 != 0)
			{
				int num9 = (val6.R + val6.G + val6.B) / 3;
				if (num9 > 10)
				{
					Color selectionGlowColor = Colors.GetSelectionGlowColor(num8 == 2, num9);
					Main.spriteBatch.Draw(value, val5 - Main.screenPosition, (Rectangle?)value2, selectionGlowColor, 0f, val3, 1f, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	private void DrawVoidLenses()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		int num = 6;
		int num2 = _specialsCount[num];
		_voidLensData.Clear();
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			VoidLensHelper voidLensHelper = new VoidLensHelper(val.ToWorldCoordinates(), 1f);
			if (!Main.gamePaused)
			{
				voidLensHelper.Update();
			}
			int selectionMode = 0;
			if (Main.InSmartCursorHighlightArea(val.X, val.Y, out var actuallySelected))
			{
				selectionMode = 1;
				if (actuallySelected)
				{
					selectionMode = 2;
				}
			}
			voidLensHelper.DrawToDrawData(_voidLensData, selectionMode);
		}
		foreach (DrawData voidLensDatum in _voidLensData)
		{
			voidLensDatum.Draw(Main.spriteBatch);
		}
	}

	private void DrawMultiTileGrass()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 2;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int num3 = val.Y;
			int sizeX = 1;
			int num4 = 1;
			Tile tile = Main.tile[x, num3];
			if (tile != null && tile.active())
			{
				switch (Main.tile[x, num3].type)
				{
				case 27:
					sizeX = 2;
					num4 = 5;
					break;
				case 236:
				case 238:
				case 702:
					sizeX = (num4 = 2);
					break;
				case 233:
					sizeX = ((Main.tile[x, num3].frameY != 0) ? 2 : 3);
					num4 = 2;
					break;
				case 530:
				case 651:
				case 705:
					sizeX = 3;
					num4 = 2;
					break;
				case 485:
				case 490:
				case 521:
				case 522:
				case 523:
				case 524:
				case 525:
				case 526:
				case 527:
				case 652:
					sizeX = 2;
					num4 = 2;
					break;
				case 489:
					sizeX = 2;
					num4 = 3;
					break;
				case 493:
					sizeX = 1;
					num4 = 2;
					break;
				case 519:
					sizeX = 1;
					num4 = ClimbCatTail(x, num3);
					num3 -= num4 - 1;
					break;
				}
				DrawMultiTileGrassInWind(unscaledPosition, zero, x, num3, sizeX, num4);
			}
		}
	}

	private int ClimbCatTail(int originx, int originy)
	{
		int num = 0;
		int num2 = originy;
		while (num2 > 10)
		{
			Tile tile = Main.tile[originx, num2];
			if (!tile.active() || tile.type != 519)
			{
				break;
			}
			if (tile.frameX >= 180)
			{
				num++;
				break;
			}
			num2--;
			num++;
		}
		return num;
	}

	private void DrawMultiTileVines()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 3;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			int sizeX = 1;
			int sizeY = 1;
			Tile tile = Main.tile[x, y];
			if (tile != null && tile.active())
			{
				switch (Main.tile[x, y].type)
				{
				case 34:
					sizeX = 3;
					sizeY = 3;
					break;
				case 454:
					sizeX = 4;
					sizeY = 3;
					break;
				case 42:
				case 270:
				case 271:
				case 572:
				case 581:
				case 660:
					sizeX = 1;
					sizeY = 2;
					break;
				case 91:
					sizeX = 1;
					sizeY = 3;
					break;
				case 95:
				case 126:
				case 444:
					sizeX = 2;
					sizeY = 2;
					break;
				case 465:
				case 591:
				case 592:
					sizeX = 2;
					sizeY = 3;
					break;
				case 698:
					sizeX = 1;
					sizeY = 1;
					break;
				}
				DrawMultiTileVinesInWind(unscaledPosition, zero, x, y, sizeX, sizeY);
			}
		}
	}

	private void DrawVines()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 4;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			DrawVineStrip(unscaledPosition, zero, x, y);
		}
	}

	private void DrawReverseVines()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 7;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			DrawRisingVineStrip(unscaledPosition, zero, x, y);
		}
	}

	private void DrawMultiTileGrassInWind(Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		float windCycle = GetWindCycle(topLeftX, topLeftY, _sunflowerWindCounter);
		Vector2 val = new Vector2((float)(topLeftX * 16 - (int)screenPosition.X) + (float)sizeX * 16f * 0.5f, (float)(topLeftY * 16 - (int)screenPosition.Y + 16 * sizeY)) + offSet;
		float num = 0.07f;
		int type = Main.tile[topLeftX, topLeftY].type;
		Texture2D val2 = null;
		Color color = Color.Transparent;
		bool flag = InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY);
		bool flag2 = false;
		int num2 = 0;
		switch (type)
		{
		case 27:
			color = Color.White;
			flag2 = true;
			num2 = 74;
			break;
		case 519:
			flag = InAPlaceWithWind(topLeftX, topLeftY, sizeX, 1);
			break;
		default:
			num = 0.15f;
			break;
		case 521:
		case 522:
		case 523:
		case 524:
		case 525:
		case 526:
		case 527:
			num = 0f;
			flag = false;
			break;
		}
		for (int i = topLeftX; i < topLeftX + sizeX; i++)
		{
			for (int j = topLeftY; j < topLeftY + sizeY; j++)
			{
				Tile tile = Main.tile[i, j];
				ushort type2 = tile.type;
				if (type2 != type || !IsVisible(tile))
				{
					continue;
				}
				Math.Abs(((float)(i - topLeftX) + 0.5f) / (float)sizeX - 0.5f);
				short tileFrameX = tile.frameX;
				short tileFrameY = tile.frameY;
				float num3 = 1f - (float)(j - topLeftY + 1) / (float)sizeY;
				if (num3 == 0f)
				{
					num3 = 0.1f;
				}
				if (!flag)
				{
					num3 = 0f;
				}
				GetTileDrawData(i, j, tile, type2, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
				bool flag3 = _rand.Next(4) == 0;
				Color tileLight = Lighting.GetColor(i, j);
				DrawAnimatedTile_AdjustForVisionChangers(i, j, tile, type2, tileFrameX, tileFrameY, ref tileLight, flag3);
				tileLight = DrawTiles_GetLightOverride(j, i, tile, type2, tileFrameX, tileFrameY, tileLight);
				if (_isActiveAndNotPaused & flag3)
				{
					DrawTiles_EmitParticles(j, i, tile, type2, tileFrameX, tileFrameY, tileLight);
				}
				Vector2 val3 = new Vector2((float)(i * 16 - (int)screenPosition.X), (float)(j * 16 - (int)screenPosition.Y + tileTop)) + offSet;
				if (tile.type == 493 && tile.frameY == 0)
				{
					if (Main.WindForVisuals >= 0f)
					{
						tileSpriteEffect = (SpriteEffects)(tileSpriteEffect ^ (SpriteEffects)1);
					}
					if (((int)tileSpriteEffect & 1) == 0)
					{
						val3.X -= 6f;
					}
					else
					{
						val3.X += 6f;
					}
				}
				Vector2 val4 = new Vector2(windCycle * 1f, Math.Abs(windCycle) * 2f * num3);
				Vector2 origin = val - val3;
				Texture2D tileDrawTexture = GetTileDrawTexture(tile, i, j);
				if (tileDrawTexture != null)
				{
					if (flag2)
					{
						val2 = tileDrawTexture;
					}
					SideFlags sideFlags = SideFlags.None;
					if (i > topLeftX)
					{
						sideFlags |= SideFlags.Left;
					}
					if (i < topLeftX + sizeX - 1)
					{
						sideFlags |= SideFlags.Right;
					}
					if (j > topLeftY)
					{
						sideFlags |= SideFlags.Top;
					}
					if (j < topLeftY + sizeY - 1)
					{
						sideFlags |= SideFlags.Bottom;
					}
					DrawNature(tileDrawTexture, val + new Vector2(0f, val4.Y), new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), tileLight, windCycle * num * num3, origin, 1f, tileSpriteEffect, 0f, sideFlags);
					if (val2 != null)
					{
						DrawNatureGlowmask(val2, val + new Vector2(0f, val4.Y), new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY + num2, tileWidth, tileHeight - halfBrickHeight), color, windCycle * num * num3, origin, 1f, tileSpriteEffect, 0f);
					}
				}
			}
		}
	}

	private void DrawVineStrip(Vector2 screenPosition, Vector2 offSet, int x, int startY)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		Vector2 val = new Vector2((float)(x * 16 + 8), (float)(startY * 16 - 2));
		float num3 = Math.Abs(Main.WindForVisuals) / 1.2f;
		num3 = MathHelper.Lerp(0.2f, 1f, num3);
		float num4 = -0.08f * num3;
		float windCycle = GetWindCycle(x, startY, _vineWindCounter);
		float num5 = 0f;
		float num6 = 0f;
		for (int i = startY; i < Main.maxTilesY - 10; i++)
		{
			Tile tile = Main.tile[x, i];
			if (tile == null)
			{
				break;
			}
			ushort type = tile.type;
			if (!tile.active() || !TileID.Sets.VineThreads[type])
			{
				break;
			}
			num++;
			if (num2 >= 5)
			{
				num4 += 0.0075f * num3;
			}
			if (num2 >= 2)
			{
				num4 += 0.0025f;
			}
			if (Main.remixWorld)
			{
				if (WallID.Sets.AllowsWind[tile.wall] && (double)i > Main.worldSurface)
				{
					num2++;
				}
			}
			else if (WallID.Sets.AllowsWind[tile.wall] && (double)i < Main.worldSurface)
			{
				num2++;
			}
			float windGridPush = GetWindGridPush(x, i, 20, 0.01f);
			num5 = ((windGridPush != 0f || num6 == 0f) ? (num5 - windGridPush) : (num5 * -0.78f));
			num6 = windGridPush;
			short tileFrameX = tile.frameX;
			short tileFrameY = tile.frameY;
			Color color = Lighting.GetColor(x, i);
			GetTileDrawData(x, i, tile, type, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var glowTexture, out var glowSourceRect, out var glowColor);
			Vector2 position = new Vector2((float)(-(int)screenPosition.X), (float)(-(int)screenPosition.Y)) + offSet + val;
			if (tile.fullbrightBlock())
			{
				color = Color.White;
			}
			float num7 = (float)num2 * num4 * windCycle + num5;
			if (_perspectivePlayer.biomeSight)
			{
				Color sightColor = Color.White;
				if (Main.IsTileBiomeSightable(type, tileFrameX, tileFrameY, ref sightColor))
				{
					if (color.R < sightColor.R)
					{
						color.R = sightColor.R;
					}
					if (color.G < sightColor.G)
					{
						color.G = sightColor.G;
					}
					if (color.B < sightColor.B)
					{
						color.B = sightColor.B;
					}
					if (_isActiveAndNotPaused && _rand.Next(480) == 0)
					{
						Color newColor = sightColor;
						int num8 = Dust.NewDust(new Vector2((float)(x * 16), (float)(i * 16)), 16, 16, 267, 0f, 0f, 150, newColor, 0.3f);
						_dust[num8].noGravity = true;
						_dust[num8].fadeIn = 1f;
						Dust obj = _dust[num8];
						obj.velocity *= 0.1f;
						_dust[num8].noLightEmittance = true;
					}
				}
			}
			Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, i);
			if (tileDrawTexture == null)
			{
				break;
			}
			if (IsVisible(tile))
			{
				Tile tile2 = Main.tile[x, i + 1];
				bool flag = tile2.active() && TileID.Sets.VineThreads[tile2.type];
				DrawNature(tileDrawTexture, position, new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), color, num7, new Vector2((float)(tileWidth / 2), (float)(halfBrickHeight - tileTop)), 1f, tileSpriteEffect, 0f, flag ? SideFlags.Bottom : SideFlags.None);
				if (glowTexture != null)
				{
					DrawNatureGlowmask(glowTexture, position, glowSourceRect, glowColor, num7, new Vector2((float)(tileWidth / 2), (float)(halfBrickHeight - tileTop)), 1f, tileSpriteEffect, 0f);
				}
			}
			val += (num7 + (float)Math.PI / 2f).ToRotationVector2() * 16f;
		}
	}

	private void DrawRisingVineStrip(Vector2 screenPosition, Vector2 offSet, int x, int startY)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		Vector2 val = new Vector2((float)(x * 16 + 8), (float)(startY * 16 + 16 + 2));
		float num3 = Math.Abs(Main.WindForVisuals) / 1.2f;
		num3 = MathHelper.Lerp(0.2f, 1f, num3);
		float num4 = -0.08f * num3;
		float windCycle = GetWindCycle(x, startY, _vineWindCounter);
		float num5 = 0f;
		float num6 = 0f;
		for (int num7 = startY; num7 > 10; num7--)
		{
			Tile tile = Main.tile[x, num7];
			if (tile != null)
			{
				ushort type = tile.type;
				if (!tile.active() || !TileID.Sets.ReverseVineThreads[type])
				{
					break;
				}
				num++;
				if (num2 >= 5)
				{
					num4 += 0.0075f * num3;
				}
				if (num2 >= 2)
				{
					num4 += 0.0025f;
				}
				if (WallID.Sets.AllowsWind[tile.wall] && (double)num7 < Main.worldSurface)
				{
					num2++;
				}
				float windGridPush = GetWindGridPush(x, num7, 40, -0.004f);
				num5 = ((windGridPush != 0f || num6 == 0f) ? (num5 - windGridPush) : (num5 * -0.78f));
				num6 = windGridPush;
				short tileFrameX = tile.frameX;
				short tileFrameY = tile.frameY;
				Color color = Lighting.GetColor(x, num7);
				GetTileDrawData(x, num7, tile, type, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
				Vector2 position = new Vector2((float)(-(int)screenPosition.X), (float)(-(int)screenPosition.Y)) + offSet + val;
				if (tile.fullbrightBlock())
				{
					color = Color.White;
				}
				float num8 = (float)num2 * (0f - num4) * windCycle + num5;
				Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, num7);
				if (tileDrawTexture == null)
				{
					break;
				}
				if (IsVisible(tile))
				{
					Tile tile2 = Main.tile[x, num7 - 1];
					bool flag = tile2.active() && TileID.Sets.ReverseVineThreads[tile2.type];
					DrawNature(tileDrawTexture, position, new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), color, num8, new Vector2((float)(tileWidth / 2), (float)(halfBrickHeight - tileTop + tileHeight)), 1f, tileSpriteEffect, 0f, flag ? SideFlags.Top : SideFlags.None);
				}
				val += (num8 - (float)Math.PI / 2f).ToRotationVector2() * 16f;
			}
		}
	}

	private float GetAverageWindGridPush(int topLeftX, int topLeftY, int sizeX, int sizeY, int totalPushTime, float pushForcePerFrame)
	{
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				float windGridPush = GetWindGridPush(topLeftX + i, topLeftY + j, totalPushTime, pushForcePerFrame);
				if (windGridPush != 0f)
				{
					num += windGridPush;
					num2++;
				}
			}
		}
		if (num2 == 0)
		{
			return 0f;
		}
		return num / (float)num2;
	}

	private float GetHighestWindGridPushComplex(int topLeftX, int topLeftY, int sizeX, int sizeY, int totalPushTime, float pushForcePerFrame, int loops, bool swapLoopDir)
	{
		float result = 0f;
		int num = int.MaxValue;
		for (int i = 0; i < 1; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				_windGrid.GetWindTime(topLeftX + i + sizeX / 2, topLeftY + j, totalPushTime, out var windTimeLeft, out var _, out var _);
				float windGridPushComplex = GetWindGridPushComplex(topLeftX + i, topLeftY + j, totalPushTime, pushForcePerFrame, loops, swapLoopDir);
				if (windTimeLeft < num && windTimeLeft != 0)
				{
					result = windGridPushComplex;
					num = windTimeLeft;
				}
			}
		}
		return result;
	}

	private void DrawMultiTileVinesInWind(Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Unknown result type (might be due to invalid IL or missing references)
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_098d: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cae: Unknown result type (might be due to invalid IL or missing references)
		float windCycle = GetWindCycle(topLeftX, topLeftY, _sunflowerWindCounter);
		float num = windCycle;
		int totalPushTime = 60;
		float pushForcePerFrame = 1.26f;
		float highestWindGridPushComplex = GetHighestWindGridPushComplex(topLeftX, topLeftY, sizeX, sizeY, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex;
		Vector2 val = new Vector2((float)(topLeftX * 16 - (int)screenPosition.X) + (float)sizeX * 16f * 0.5f, (float)(topLeftY * 16 - (int)screenPosition.Y)) + offSet;
		float num2 = 0.07f;
		Tile tile = Main.tile[topLeftX, topLeftY];
		int type = tile.type;
		Vector2 val2 = new Vector2(0f, -2f);
		val += val2;
		if ((type != 465 && (uint)(type - 591) > 1u) ? (sizeX == 1 && WorldGen.IsBelowANonHammeredPlatform(topLeftX, topLeftY)) : (WorldGen.IsBelowANonHammeredPlatform(topLeftX, topLeftY) && WorldGen.IsBelowANonHammeredPlatform(topLeftX + 1, topLeftY)))
		{
			val.Y -= 8f;
			val2.Y -= 8f;
		}
		Texture2D val3 = null;
		Color val4 = Color.Transparent;
		float? num3 = null;
		float num4 = 1f;
		float num5 = -4f;
		bool flag2 = false;
		bool flag3 = false;
		num2 = 0.15f;
		switch (type)
		{
		case 34:
		case 126:
			num3 = 1f;
			num5 = 0f;
			switch (tile.frameY / 54 + tile.frameX / 108 * 37)
			{
			case 9:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				num2 *= 0.3f;
				break;
			case 11:
				num2 *= 0.5f;
				break;
			case 12:
				num3 = null;
				num5 = -1f;
				break;
			case 18:
				num3 = null;
				num5 = -1f;
				break;
			case 21:
				num3 = null;
				num5 = -1f;
				break;
			case 23:
				num3 = 0f;
				break;
			case 25:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				break;
			case 32:
				num2 *= 0.5f;
				break;
			case 33:
				num2 *= 0.5f;
				break;
			case 35:
				num3 = 0f;
				break;
			case 36:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				break;
			case 37:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				num2 *= 0.5f;
				break;
			case 39:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				break;
			case 40:
			case 41:
			case 42:
			case 43:
				num3 = null;
				num5 = -2f;
				flag2 = true;
				num2 *= 0.5f;
				break;
			case 44:
				num3 = null;
				num5 = -3f;
				break;
			case 54:
			case 55:
			case 60:
			case 65:
			case 68:
			case 70:
				num3 = 0f;
				break;
			case 67:
				num3 = 1f;
				break;
			}
			break;
		case 42:
			num3 = 1f;
			num5 = 0f;
			switch (tile.frameY / 36)
			{
			case 0:
				num3 = null;
				num5 = -1f;
				break;
			case 9:
				num3 = 0f;
				break;
			case 12:
				num3 = null;
				num5 = -1f;
				break;
			case 14:
				num3 = null;
				num5 = -1f;
				break;
			case 28:
				num3 = null;
				num5 = -1f;
				break;
			case 30:
				num3 = 0f;
				break;
			case 32:
				num3 = 0f;
				break;
			case 33:
				num3 = 0f;
				break;
			case 34:
				num3 = null;
				num5 = -1f;
				break;
			case 35:
				num3 = 0f;
				break;
			case 38:
				num3 = null;
				num5 = -1f;
				break;
			case 39:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				break;
			case 40:
			case 41:
			case 42:
			case 43:
				num3 = 0f;
				num3 = null;
				num5 = -1f;
				flag2 = true;
				break;
			case 54:
			case 55:
			case 60:
			case 65:
			case 67:
			case 70:
				num3 = 0f;
				break;
			}
			break;
		case 95:
		case 270:
		case 271:
		case 444:
		case 454:
		case 572:
		case 581:
		case 660:
			num3 = 1f;
			num5 = 0f;
			break;
		case 591:
			num4 = 0.5f;
			num5 = -2f;
			break;
		case 592:
			num4 = 0.5f;
			num5 = -2f;
			val3 = TextureAssets.GlowMask[294].Value;
			val4 = new Color(255, 255, 255, 0);
			break;
		case 698:
			num4 = 0.5f;
			num5 = -1f;
			offSet.X -= 10f;
			flag3 = true;
			break;
		}
		if (flag2)
		{
			val += new Vector2(0f, 16f);
		}
		num2 *= -1f;
		bool flag4 = InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY);
		if (flag3 || !flag4)
		{
			windCycle -= num;
		}
		ulong num6 = 0uL;
		for (int i = topLeftX; i < topLeftX + sizeX; i++)
		{
			for (int j = topLeftY; j < topLeftY + sizeY; j++)
			{
				Tile tile2 = Main.tile[i, j];
				ushort type2 = tile2.type;
				if (type2 != type || !IsVisible(tile2))
				{
					continue;
				}
				Math.Abs(((float)(i - topLeftX) + 0.5f) / (float)sizeX - 0.5f);
				short tileFrameX = tile2.frameX;
				short tileFrameY = tile2.frameY;
				float num7 = (float)(j - topLeftY + 1) / (float)sizeY;
				if (num7 == 0f)
				{
					num7 = 0.1f;
				}
				if (num3.HasValue)
				{
					num7 = num3.Value;
				}
				if (flag2 && j == topLeftY)
				{
					num7 = 0f;
				}
				GetTileDrawData(i, j, tile2, type2, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
				bool flag5 = _rand.Next(4) == 0;
				Color tileLight = Lighting.GetColor(i, j);
				DrawAnimatedTile_AdjustForVisionChangers(i, j, tile2, type2, tileFrameX, tileFrameY, ref tileLight, flag5);
				tileLight = DrawTiles_GetLightOverride(j, i, tile2, type2, tileFrameX, tileFrameY, tileLight);
				if (_isActiveAndNotPaused & flag5)
				{
					DrawTiles_EmitParticles(j, i, tile2, type2, tileFrameX, tileFrameY, tileLight);
				}
				Vector2 val5 = new Vector2((float)(i * 16 - (int)screenPosition.X), (float)(j * 16 - (int)screenPosition.Y + tileTop)) + offSet;
				val5 += val2;
				Vector2 val6 = new Vector2(windCycle * num4, Math.Abs(windCycle) * num5 * num7);
				Vector2 val7 = val - val5;
				Texture2D tileDrawTexture = GetTileDrawTexture(tile2, i, j);
				if (tileDrawTexture == null)
				{
					continue;
				}
				Vector2 val8 = val + new Vector2(0f, val6.Y);
				Rectangle val9 = new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight);
				float num8 = windCycle * num2 * num7;
				if (type2 == 660 && j == topLeftY + sizeY - 1)
				{
					Texture2D value = TextureAssets.Extra[260].Value;
					_ = ((float)((i + j) % 200) * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f;
					Color white = Color.White;
					Main.spriteBatch.Draw(value, val8, (Rectangle?)val9, white, num8, val7, 1f, tileSpriteEffect, 0f);
				}
				Main.spriteBatch.Draw(tileDrawTexture, val8, (Rectangle?)val9, tileLight, num8, val7, 1f, tileSpriteEffect, 0f);
				if (type2 == 660 && j == topLeftY + sizeY - 1)
				{
					Texture2D value2 = TextureAssets.Extra[260].Value;
					Color val10 = Main.hslToRgb(((float)((i + j) % 200) * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f, 1f, 0.8f);
					val10.A = 127;
					Rectangle value3 = val9;
					Vector2 val11 = val8;
					Vector2 val12 = val7;
					Main.spriteBatch.Draw(value2, val11, (Rectangle?)value3, val10, num8, val12, 1f, tileSpriteEffect, 0f);
				}
				if (type2 == 698 && TileEntity.TryGetAt<TEDeadCellsDisplayJar>(topLeftX, topLeftY, out var result))
				{
					Item item = result.item;
					short num9 = (short)(tileFrameX / 38);
					int num10 = 22;
					int num11 = 0;
					switch (num9)
					{
					default:
						num10 = 22;
						num11 = -1;
						break;
					case 1:
						num10 = 18;
						break;
					case 2:
						num10 = 20;
						break;
					}
					Rectangle val13 = val9;
					val13.Y += 46;
					Rectangle value4 = val13;
					value4.Y += 46;
					int num12 = 1;
					Color val14 = new Color(150, 150, 255);
					if (!item.IsAir)
					{
						num12 = item.rare;
						if (item.expert)
						{
							num12 = -12;
						}
						val14 = num12 switch
						{
							-12 => new Color((int)(byte)Main.DiscoR, (int)(byte)Main.DiscoG, (int)(byte)Main.DiscoB), 
							-13 => new Color(255, (int)(byte)(Main.masterColor * 200f), 0), 
							_ => item.GetPopupRarityColor(), 
						};
					}
					Vector3 val15 = Main.rgbToHsl(val14);
					float x = val15.X;
					Color val16 = val14;
					val14 *= 0.25f;
					if (num12 != -1)
					{
						val16 = Main.hslToRgb(x, MathHelper.Clamp(val15.Y + 0.5f, 0f, 1f), MathHelper.Clamp(val15.Z, 0f, 1f), 127);
					}
					Main.spriteBatch.Draw(tileDrawTexture, val8, (Rectangle?)value4, val14, num8, val7, 1f, tileSpriteEffect, 0f);
					if (!item.IsAir)
					{
						Vector2 spinningpoint = new Vector2(-2f, (float)(24 + num11));
						switch (tileFrameX / 38)
						{
						case 1:
							spinningpoint.Y += 4f;
							break;
						case 2:
							spinningpoint.Y += 6f;
							break;
						}
						Vector2 screenPositionForItemCenter = val8 + spinningpoint.RotatedBy(num8) + new Vector2(2f, 0f);
						tileLight = Color.Lerp(tileLight, Color.White, 0.5f);
						ItemSlot.DrawItemIcon(item, 40, Main.spriteBatch, screenPositionForItemCenter, item.scale, num10, tileLight);
					}
					val14.A = byte.MaxValue;
					Main.spriteBatch.Draw(tileDrawTexture, val8, (Rectangle?)val13, val16, num8, val7, 1f, tileSpriteEffect, 0f);
				}
				if (val3 != null)
				{
					Main.spriteBatch.Draw(val3, val8, (Rectangle?)val9, val4, num8, val7, 1f, tileSpriteEffect, 0f);
				}
				TileFlameData tileFlameData = GetTileFlameData(i, j, type2, tileFrameY);
				if (num6 == 0L)
				{
					num6 = tileFlameData.flameSeed;
				}
				tileFlameData.flameSeed = num6;
				for (int k = 0; k < tileFlameData.flameCount; k++)
				{
					float num13 = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeXMin, tileFlameData.flameRangeXMax) * tileFlameData.flameRangeMultX;
					float num14 = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeYMin, tileFlameData.flameRangeYMax) * tileFlameData.flameRangeMultY;
					Main.spriteBatch.Draw(tileFlameData.flameTexture, val8 + new Vector2(num13, num14), (Rectangle?)val9, tileFlameData.flameColor, num8, val7, 1f, tileSpriteEffect, 0f);
				}
			}
		}
	}

	private void EmitAlchemyHerbParticles(int j, int i, int style)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		if (style == 0 && _rand.Next(100) == 0)
		{
			int num = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16 - 4)), 16, 16, 19, 0f, 0f, 160, default, 0.1f);
			_dust[num].velocity.X /= 2f;
			_dust[num].velocity.Y /= 2f;
			_dust[num].noGravity = true;
			_dust[num].fadeIn = 1f;
		}
		if (style == 1 && _rand.Next(100) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 41, 0f, 0f, 250, default, 0.8f);
		}
		if (style == 3)
		{
			if (_rand.Next(200) == 0)
			{
				int num2 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 100, default, 0.2f);
				_dust[num2].fadeIn = 1.2f;
			}
			if (_rand.Next(75) == 0)
			{
				int num3 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 27, 0f, 0f, 100);
				_dust[num3].velocity.X /= 2f;
				_dust[num3].velocity.Y /= 2f;
			}
		}
		if (style == 4 && _rand.Next(150) == 0)
		{
			int num4 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 8, 16);
			_dust[num4].velocity.X /= 3f;
			_dust[num4].velocity.Y /= 3f;
			_dust[num4].velocity.Y -= 0.7f;
			_dust[num4].alpha = 50;
			_dust[num4].scale *= 0.1f;
			_dust[num4].fadeIn = 0.9f;
			_dust[num4].noGravity = true;
		}
		if (style == 5 && _rand.Next(40) == 0)
		{
			int num5 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16 - 6)), 16, 16, 6, 0f, 0f, 0, default, 1.5f);
			_dust[num5].velocity.Y -= 2f;
			_dust[num5].noGravity = true;
		}
		if (style == 6 && _rand.Next(30) == 0)
		{
			int num6 = Dust.NewDust(newColor: new Color(50, 255, 255, 255), Position: new Vector2((float)(i * 16), (float)(j * 16)), Width: 16, Height: 16, Type: 43, SpeedX: 0f, SpeedY: 0f, Alpha: 254, Scale: 0.5f);
			Dust obj = _dust[num6];
			obj.velocity *= 0f;
		}
	}
}
