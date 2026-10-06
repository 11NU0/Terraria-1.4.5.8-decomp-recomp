using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Liquid;

public class LiquidRenderer
{
	private struct LiquidCache
	{
		public float LiquidLevel;

		public float VisibleLiquidLevel;

		public float Opacity;

		public bool IsSolid;

		public bool IsHalfBrick;

		public bool HasLiquid;

		public bool HasVisibleLiquid;

		public bool HasWall;

		public Point FrameOffset;

		public bool HasLeftEdge;

		public bool HasRightEdge;

		public bool HasTopEdge;

		public bool HasBottomEdge;

		public float LeftWall;

		public float RightWall;

		public float BottomWall;

		public float TopWall;

		public float VisibleLeftWall;

		public float VisibleRightWall;

		public float VisibleBottomWall;

		public float VisibleTopWall;

		public byte Type;

		public byte VisibleType;
	}

	private struct LiquidDrawCache
	{
		public Rectangle SourceRectangle;

		public Vector2 LiquidOffset;

		public bool IsVisible;

		public float Opacity;

		public byte Type;

		public bool IsSurfaceLiquid;

		public bool HasWall;
	}

	private struct SpecialLiquidDrawCache
	{
		public int X;

		public int Y;

		public Rectangle SourceRectangle;

		public Vector2 LiquidOffset;

		public bool IsVisible;

		public float Opacity;

		public byte Type;

		public bool IsSurfaceLiquid;

		public bool HasWall;
	}

	private const int ANIMATION_FRAME_COUNT = 16;

	private const int CACHE_PADDING = 2;

	private const int CACHE_PADDING_2 = 4;

	private static readonly int[] WATERFALL_LENGTH = new int[4] { 10, 3, 2, 10 };

	private static readonly float[] DEFAULT_OPACITY = new float[4] { 0.6f, 0.95f, 0.95f, 0.75f };

	private static readonly byte[] WAVE_MASK_STRENGTH = new byte[5];

	private static readonly byte[] VISCOSITY_MASK = new byte[5] { 0, 200, 240, 0, 0 };

	public const float MIN_LIQUID_SIZE = 0.25f;

	public static LiquidRenderer Instance;

	private readonly Asset<Texture2D>[] _liquidTextures = new Asset<Texture2D>[15];

	private LiquidCache[] _cache = new LiquidCache[1];

	private LiquidDrawCache[] _drawCache = new LiquidDrawCache[1];

	private SpecialLiquidDrawCache[] _drawCacheForShimmer = new SpecialLiquidDrawCache[1];

	private int _animationFrame;

	private int _waterfallAnimationFrame;

	private Rectangle _drawArea = new Rectangle(0, 0, 1, 1);

	private readonly UnifiedRandom _random = new UnifiedRandom();

	private Color[] _waveMask = new Color[1];

	private float _frameState;

	private float _waterfallFrameState;

	private static Tile[,] Tiles => Main.tile;

	public event Action<Color[], Rectangle> WaveFilters;

	public static void LoadContent()
	{
		Instance = new LiquidRenderer();
		Instance.PrepareAssets();
	}

	public LiquidRenderer()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
	}

	private void PrepareAssets()
	{
		if (!Main.dedServ)
		{
			for (int i = 0; i < _liquidTextures.Length; i++)
			{
				_liquidTextures[i] = Main.Assets.Request<Texture2D>("Images/Misc/water_" + i, (AssetRequestMode)1);
			}
		}
	}

	private unsafe void InternalPrepareDraw(Rectangle drawArea)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_11dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_139b: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_102a: Unknown result type (might be due to invalid IL or missing references)
		//IL_102f: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1056: Unknown result type (might be due to invalid IL or missing references)
		//IL_1277: Unknown result type (might be due to invalid IL or missing references)
		//IL_128d: Unknown result type (might be due to invalid IL or missing references)
		//IL_145a: Unknown result type (might be due to invalid IL or missing references)
		//IL_145f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1476: Unknown result type (might be due to invalid IL or missing references)
		//IL_147b: Unknown result type (might be due to invalid IL or missing references)
		//IL_12be: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_114c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1151: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = new Rectangle(drawArea.X - 2, drawArea.Y - 2, drawArea.Width + 4, drawArea.Height + 4);
		_drawArea = drawArea;
		if (_cache.Length < val.Width * val.Height + 1)
		{
			_cache = new LiquidCache[val.Width * val.Height + 1];
		}
		if (_drawCache.Length < drawArea.Width * drawArea.Height + 1)
		{
			_drawCache = new LiquidDrawCache[drawArea.Width * drawArea.Height + 1];
		}
		if (_drawCacheForShimmer.Length < drawArea.Width * drawArea.Height + 1)
		{
			_drawCacheForShimmer = new SpecialLiquidDrawCache[drawArea.Width * drawArea.Height + 1];
		}
		if (_waveMask.Length < drawArea.Width * drawArea.Height)
		{
			_waveMask = new Color[drawArea.Width * drawArea.Height];
		}
		Tile tile = null;
		fixed (LiquidCache* ptr = &_cache[1])
		{
			LiquidCache* ptr2 = ptr;
			int num = val.Height * 2 + 2;
			ptr2 = ptr;
			for (int i = val.X; i < val.X + val.Width; i++)
			{
				for (int j = val.Y; j < val.Y + val.Height; j++)
				{
					tile = Tiles[i, j];
					if (tile == null)
					{
						tile = new Tile();
					}
					ptr2->LiquidLevel = (float)(int)tile.liquid / 255f;
					ptr2->IsHalfBrick = tile.halfBrick() && ptr2[-1].HasLiquid && !TileID.Sets.Platforms[tile.type];
					ptr2->IsSolid = WorldGen.SolidOrSlopedTile(tile);
					ptr2->HasLiquid = tile.liquid != 0;
					ptr2->VisibleLiquidLevel = 0f;
					ptr2->HasWall = tile.wall != 0;
					ptr2->Type = tile.liquidType();
					if (ptr2->IsHalfBrick && !ptr2->HasLiquid)
					{
						ptr2->Type = ptr2[-1].Type;
					}
					ptr2++;
				}
			}
			ptr2 = ptr;
			float num2 = 0f;
			ptr2 += num;
			for (int k = 2; k < val.Width - 2; k++)
			{
				for (int l = 2; l < val.Height - 2; l++)
				{
					num2 = 0f;
					if (ptr2->IsHalfBrick && ptr2[-1].HasLiquid)
					{
						num2 = 1f;
					}
					else if (!ptr2->HasLiquid)
					{
						LiquidCache liquidCache = ptr2[-1];
						LiquidCache liquidCache2 = ptr2[1];
						LiquidCache liquidCache3 = ptr2[-val.Height];
						LiquidCache liquidCache4 = ptr2[val.Height];
						if (liquidCache.HasLiquid && liquidCache2.HasLiquid && liquidCache.Type == liquidCache2.Type && !liquidCache.IsSolid && !liquidCache2.IsSolid)
						{
							num2 = liquidCache.LiquidLevel + liquidCache2.LiquidLevel;
							ptr2->Type = liquidCache.Type;
						}
						if (liquidCache3.HasLiquid && liquidCache4.HasLiquid && liquidCache3.Type == liquidCache4.Type && !liquidCache3.IsSolid && !liquidCache4.IsSolid)
						{
							num2 = Math.Max(num2, liquidCache3.LiquidLevel + liquidCache4.LiquidLevel);
							ptr2->Type = liquidCache3.Type;
						}
						num2 *= 0.5f;
					}
					else
					{
						num2 = ptr2->LiquidLevel;
					}
					ptr2->VisibleLiquidLevel = num2;
					ptr2->HasVisibleLiquid = num2 != 0f;
					ptr2++;
				}
				ptr2 += 4;
			}
			ptr2 = ptr;
			for (int m = 0; m < val.Width; m++)
			{
				for (int n = 0; n < val.Height - 10; n++)
				{
					if (ptr2->HasVisibleLiquid && (!ptr2->IsSolid || ptr2->IsHalfBrick))
					{
						ptr2->Opacity = 1f;
						ptr2->VisibleType = ptr2->Type;
						float num3 = 1f / (float)(WATERFALL_LENGTH[ptr2->Type] + 1);
						float num4 = 1f;
						for (int num5 = 1; num5 <= WATERFALL_LENGTH[ptr2->Type]; num5++)
						{
							num4 -= num3;
							if (ptr2[num5].IsSolid)
							{
								break;
							}
							ptr2[num5].VisibleLiquidLevel = Math.Max(ptr2[num5].VisibleLiquidLevel, ptr2->VisibleLiquidLevel * num4);
							ptr2[num5].Opacity = num4;
							ptr2[num5].VisibleType = ptr2->Type;
						}
					}
					if (ptr2->IsSolid && !ptr2->IsHalfBrick)
					{
						ptr2->VisibleLiquidLevel = 1f;
						ptr2->HasVisibleLiquid = false;
					}
					else
					{
						ptr2->HasVisibleLiquid = ptr2->VisibleLiquidLevel != 0f;
					}
					ptr2++;
				}
				ptr2 += 10;
			}
			ptr2 = ptr;
			ptr2 += num;
			for (int num6 = 2; num6 < val.Width - 2; num6++)
			{
				for (int num7 = 2; num7 < val.Height - 2; num7++)
				{
					if (!ptr2->HasVisibleLiquid)
					{
						ptr2->HasLeftEdge = false;
						ptr2->HasTopEdge = false;
						ptr2->HasRightEdge = false;
						ptr2->HasBottomEdge = false;
					}
					else
					{
						LiquidCache liquidCache = ptr2[-1];
						LiquidCache liquidCache2 = ptr2[1];
						LiquidCache liquidCache3 = ptr2[-val.Height];
						LiquidCache liquidCache4 = ptr2[val.Height];
						float num8 = 0f;
						float num9 = 1f;
						float num10 = 0f;
						float num11 = 1f;
						float visibleLiquidLevel = ptr2->VisibleLiquidLevel;
						if (!liquidCache.HasVisibleLiquid)
						{
							num10 += liquidCache2.VisibleLiquidLevel * (1f - visibleLiquidLevel);
						}
						if (!liquidCache2.HasVisibleLiquid && !liquidCache2.IsSolid && !liquidCache2.IsHalfBrick)
						{
							num11 -= liquidCache.VisibleLiquidLevel * (1f - visibleLiquidLevel);
						}
						if (!liquidCache3.HasVisibleLiquid && !liquidCache3.IsSolid && !liquidCache3.IsHalfBrick)
						{
							num8 += liquidCache4.VisibleLiquidLevel * (1f - visibleLiquidLevel);
						}
						if (!liquidCache4.HasVisibleLiquid && !liquidCache4.IsSolid && !liquidCache4.IsHalfBrick)
						{
							num9 -= liquidCache3.VisibleLiquidLevel * (1f - visibleLiquidLevel);
						}
						ptr2->LeftWall = num8;
						ptr2->RightWall = num9;
						ptr2->BottomWall = num11;
						ptr2->TopWall = num10;
						Point zero = Point.Zero;
						ptr2->HasTopEdge = (!liquidCache.HasVisibleLiquid && !liquidCache.IsSolid) || num10 != 0f;
						ptr2->HasBottomEdge = (!liquidCache2.HasVisibleLiquid && !liquidCache2.IsSolid) || num11 != 1f;
						ptr2->HasLeftEdge = (!liquidCache3.HasVisibleLiquid && !liquidCache3.IsSolid) || num8 != 0f;
						ptr2->HasRightEdge = (!liquidCache4.HasVisibleLiquid && !liquidCache4.IsSolid) || num9 != 1f;
						if (!ptr2->HasLeftEdge)
						{
							if (ptr2->HasRightEdge)
							{
								zero.X += 32;
							}
							else
							{
								zero.X += 16;
							}
						}
						if (ptr2->HasLeftEdge && ptr2->HasRightEdge)
						{
							zero.X = 16;
							zero.Y += 32;
							if (ptr2->HasTopEdge)
							{
								zero.Y = 16;
							}
						}
						else if (!ptr2->HasTopEdge)
						{
							if (!ptr2->HasLeftEdge && !ptr2->HasRightEdge)
							{
								zero.Y += 48;
							}
							else
							{
								zero.Y += 16;
							}
						}
						if (zero.Y == 16 && (ptr2->HasLeftEdge ^ ptr2->HasRightEdge) && (num7 + val.Y) % 2 == 0)
						{
							zero.Y += 16;
						}
						System.Runtime.CompilerServices.Unsafe.Write(&ptr2->FrameOffset, zero);
					}
					ptr2++;
				}
				ptr2 += 4;
			}
			ptr2 = ptr;
			ptr2 += num;
			for (int num12 = 2; num12 < val.Width - 2; num12++)
			{
				for (int num13 = 2; num13 < val.Height - 2; num13++)
				{
					if (ptr2->HasVisibleLiquid)
					{
						LiquidCache liquidCache = ptr2[-1];
						LiquidCache liquidCache2 = ptr2[1];
						LiquidCache liquidCache3 = ptr2[-val.Height];
						LiquidCache liquidCache4 = ptr2[val.Height];
						ptr2->VisibleLeftWall = ptr2->LeftWall;
						ptr2->VisibleRightWall = ptr2->RightWall;
						ptr2->VisibleTopWall = ptr2->TopWall;
						ptr2->VisibleBottomWall = ptr2->BottomWall;
						if (liquidCache.HasVisibleLiquid && liquidCache2.HasVisibleLiquid)
						{
							if (ptr2->HasLeftEdge)
							{
								ptr2->VisibleLeftWall = (ptr2->LeftWall * 2f + liquidCache.LeftWall + liquidCache2.LeftWall) * 0.25f;
							}
							if (ptr2->HasRightEdge)
							{
								ptr2->VisibleRightWall = (ptr2->RightWall * 2f + liquidCache.RightWall + liquidCache2.RightWall) * 0.25f;
							}
						}
						if (liquidCache3.HasVisibleLiquid && liquidCache4.HasVisibleLiquid)
						{
							if (ptr2->HasTopEdge)
							{
								ptr2->VisibleTopWall = (ptr2->TopWall * 2f + liquidCache3.TopWall + liquidCache4.TopWall) * 0.25f;
							}
							if (ptr2->HasBottomEdge)
							{
								ptr2->VisibleBottomWall = (ptr2->BottomWall * 2f + liquidCache3.BottomWall + liquidCache4.BottomWall) * 0.25f;
							}
						}
					}
					ptr2++;
				}
				ptr2 += 4;
			}
			ptr2 = ptr;
			ptr2 += num;
			for (int num14 = 2; num14 < val.Width - 2; num14++)
			{
				for (int num15 = 2; num15 < val.Height - 2; num15++)
				{
					if (ptr2->HasLiquid)
					{
						LiquidCache liquidCache = ptr2[-1];
						LiquidCache liquidCache2 = ptr2[1];
						LiquidCache liquidCache3 = ptr2[-val.Height];
						LiquidCache liquidCache4 = ptr2[val.Height];
						if (ptr2->HasTopEdge && !ptr2->HasBottomEdge && (ptr2->HasLeftEdge ^ ptr2->HasRightEdge))
						{
							if (ptr2->HasRightEdge)
							{
								ptr2->VisibleRightWall = liquidCache2.VisibleRightWall;
								ptr2->VisibleTopWall = liquidCache3.VisibleTopWall;
							}
							else
							{
								ptr2->VisibleLeftWall = liquidCache2.VisibleLeftWall;
								ptr2->VisibleTopWall = liquidCache4.VisibleTopWall;
							}
						}
						else if (liquidCache2.FrameOffset.X == 16 && liquidCache2.FrameOffset.Y == 32)
						{
							if (ptr2->VisibleLeftWall > 0.5f)
							{
								ptr2->VisibleLeftWall = 0f;
								System.Runtime.CompilerServices.Unsafe.Write(&ptr2->FrameOffset, new Point(0, 0));
							}
							else if (ptr2->VisibleRightWall < 0.5f)
							{
								ptr2->VisibleRightWall = 1f;
								System.Runtime.CompilerServices.Unsafe.Write(&ptr2->FrameOffset, new Point(32, 0));
							}
						}
					}
					ptr2++;
				}
				ptr2 += 4;
			}
			ptr2 = ptr;
			ptr2 += num;
			for (int num16 = 2; num16 < val.Width - 2; num16++)
			{
				for (int num17 = 2; num17 < val.Height - 2; num17++)
				{
					if (ptr2->HasLiquid)
					{
						LiquidCache liquidCache = ptr2[-1];
						LiquidCache liquidCache2 = ptr2[1];
						LiquidCache liquidCache3 = ptr2[-val.Height];
						LiquidCache liquidCache4 = ptr2[val.Height];
						if (!ptr2->HasBottomEdge && !ptr2->HasLeftEdge && !ptr2->HasTopEdge && !ptr2->HasRightEdge)
						{
							if (liquidCache3.HasTopEdge && liquidCache.HasLeftEdge)
							{
								ptr2->FrameOffset.X = Math.Max(4, (int)(16f - liquidCache.VisibleLeftWall * 16f)) - 4;
								ptr2->FrameOffset.Y = 48 + Math.Max(4, (int)(16f - liquidCache3.VisibleTopWall * 16f)) - 4;
								ptr2->VisibleLeftWall = 0f;
								ptr2->VisibleTopWall = 0f;
								ptr2->VisibleRightWall = 1f;
								ptr2->VisibleBottomWall = 1f;
							}
							else if (liquidCache4.HasTopEdge && liquidCache.HasRightEdge)
							{
								ptr2->FrameOffset.X = 32 - Math.Min(16, (int)(liquidCache.VisibleRightWall * 16f) - 4);
								ptr2->FrameOffset.Y = 48 + Math.Max(4, (int)(16f - liquidCache4.VisibleTopWall * 16f)) - 4;
								ptr2->VisibleLeftWall = 0f;
								ptr2->VisibleTopWall = 0f;
								ptr2->VisibleRightWall = 1f;
								ptr2->VisibleBottomWall = 1f;
							}
						}
					}
					ptr2++;
				}
				ptr2 += 4;
			}
			ptr2 = ptr;
			ptr2 += num;
			fixed (LiquidDrawCache* ptr3 = &_drawCache[0])
			{
				fixed (Color* ptr4 = &_waveMask[0])
				{
					LiquidDrawCache* ptr5 = ptr3;
					Color* ptr6 = ptr4;
					for (int num18 = 2; num18 < val.Width - 2; num18++)
					{
						for (int num19 = 2; num19 < val.Height - 2; num19++)
						{
							if (ptr2->HasVisibleLiquid)
							{
								float num20 = Math.Min(0.75f, ptr2->VisibleLeftWall);
								float num21 = Math.Max(0.25f, ptr2->VisibleRightWall);
								float num22 = Math.Min(0.75f, ptr2->VisibleTopWall);
								float num23 = Math.Max(0.25f, ptr2->VisibleBottomWall);
								if (ptr2->IsHalfBrick && ptr2->IsSolid && num23 > 0.5f)
								{
									num23 = 0.5f;
								}
								ptr5->IsVisible = ptr2->HasWall || !ptr2->IsHalfBrick || !ptr2->HasLiquid || !(ptr2->LiquidLevel < 1f);
								System.Runtime.CompilerServices.Unsafe.Write(&ptr5->SourceRectangle, new Rectangle((int)(16f - num21 * 16f) + ptr2->FrameOffset.X, (int)(16f - num23 * 16f) + ptr2->FrameOffset.Y, (int)Math.Ceiling((num21 - num20) * 16f), (int)Math.Ceiling((num23 - num22) * 16f)));
								ptr5->IsSurfaceLiquid = ptr2->FrameOffset.X == 16 && ptr2->FrameOffset.Y == 0 && (double)(num19 + val.Y) > Main.worldSurface - 40.0;
								ptr5->Opacity = ptr2->Opacity;
								System.Runtime.CompilerServices.Unsafe.Write(&ptr5->LiquidOffset, new Vector2((float)Math.Floor(num20 * 16f), (float)Math.Floor(num22 * 16f)));
								ptr5->Type = ptr2->VisibleType;
								ptr5->HasWall = ptr2->HasWall;
								byte b = WAVE_MASK_STRENGTH[ptr2->VisibleType];
								byte g = (ptr6->R = (byte)(b >> 1));
								ptr6->G = g;
								ptr6->B = VISCOSITY_MASK[ptr2->VisibleType];
								ptr6->A = b;
								LiquidCache* ptr7 = ptr2 - 1;
								if (num19 != 2 && !ptr7->HasVisibleLiquid && !ptr7->IsSolid && !ptr7->IsHalfBrick)
								{
									System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr6 - System.Runtime.CompilerServices.Unsafe.SizeOf<Color>(), *ptr6);
								}
							}
							else
							{
								ptr5->IsVisible = false;
								int num24 = ((!ptr2->IsSolid && !ptr2->IsHalfBrick) ? 4 : 3);
								byte b3 = WAVE_MASK_STRENGTH[num24];
								byte g2 = (ptr6->R = (byte)(b3 >> 1));
								ptr6->G = g2;
								ptr6->B = VISCOSITY_MASK[num24];
								ptr6->A = b3;
							}
							ptr2++;
							ptr5++;
							ptr6 = (Color*)((byte*)ptr6 + System.Runtime.CompilerServices.Unsafe.SizeOf<Color>());
						}
						ptr2 += 4;
					}
				}
			}
			ptr2 = ptr;
			for (int num25 = val.X; num25 < val.X + val.Width; num25++)
			{
				for (int num26 = val.Y; num26 < val.Y + val.Height; num26++)
				{
					if (ptr2->VisibleType == 1 && ptr2->HasVisibleLiquid && Dust.lavaBubbles < 200)
					{
						if (_random.Next(700) == 0)
						{
							Dust.NewDust(new Vector2((float)(num25 * 16), (float)(num26 * 16)), 16, 16, 35, 0f, 0f, 0, Color.White);
						}
						if (_random.Next(350) == 0)
						{
							int num27 = Dust.NewDust(new Vector2((float)(num25 * 16), (float)(num26 * 16)), 16, 8, 35, 0f, 0f, 50, Color.White, 1.5f);
							Dust obj = Main.dust[num27];
							obj.velocity *= 0.8f;
							Main.dust[num27].velocity.X *= 2f;
							Main.dust[num27].velocity.Y -= (float)_random.Next(1, 7) * 0.1f;
							if (_random.Next(10) == 0)
							{
								Main.dust[num27].velocity.Y *= _random.Next(2, 5);
							}
							Main.dust[num27].noGravity = true;
						}
					}
					ptr2++;
				}
			}
			fixed (LiquidDrawCache* ptr8 = &_drawCache[0])
			{
				fixed (SpecialLiquidDrawCache* ptr9 = &_drawCacheForShimmer[0])
				{
					LiquidDrawCache* ptr10 = ptr8;
					SpecialLiquidDrawCache* ptr11 = ptr9;
					for (int num28 = 2; num28 < val.Width - 2; num28++)
					{
						for (int num29 = 2; num29 < val.Height - 2; num29++)
						{
							if (ptr10->IsVisible && ptr10->Type == 3)
							{
								ptr11->X = num28;
								ptr11->Y = num29;
								ptr11->IsVisible = ptr10->IsVisible;
								ptr11->HasWall = ptr10->HasWall;
								ptr11->IsSurfaceLiquid = ptr10->IsSurfaceLiquid;
								System.Runtime.CompilerServices.Unsafe.Write(&ptr11->LiquidOffset, ptr10->LiquidOffset);
								ptr11->Opacity = ptr10->Opacity;
								System.Runtime.CompilerServices.Unsafe.Write(&ptr11->SourceRectangle, ptr10->SourceRectangle);
								ptr11->Type = ptr10->Type;
								ptr10->IsVisible = false;
								ptr11++;
							}
							ptr10++;
						}
					}
					ptr11->IsVisible = false;
				}
			}
		}
		if (WaveFilters != null)
		{
			WaveFilters(_waveMask, GetCachedDrawArea());
		}
	}

	public unsafe void DrawNormalLiquids(SpriteBatch spriteBatch, Vector2 drawOffset, int waterStyle, float globalAlpha, bool isBackgroundDraw, bool waterOnly)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		Rectangle drawArea = _drawArea;
		Main.tileBatch.Restart();
		fixed (LiquidDrawCache* ptr = &_drawCache[0])
		{
			LiquidDrawCache* ptr2 = ptr;
			for (int i = drawArea.X; i < drawArea.X + drawArea.Width; i++)
			{
				for (int j = drawArea.Y; j < drawArea.Y + drawArea.Height; j++)
				{
					if (ptr2->IsVisible && (!waterOnly || ptr2->Type == 0))
					{
						Main.tileBatch.SetLayer(0u, 0);
						Rectangle sourceRectangle = ptr2->SourceRectangle;
						if (ptr2->IsSurfaceLiquid)
						{
							sourceRectangle.Y = 1280;
						}
						else if (sourceRectangle.X == 16)
						{
							sourceRectangle.Y += _waterfallAnimationFrame * 80;
						}
						else
						{
							sourceRectangle.Y += _animationFrame * 80;
						}
						Vector2 liquidOffset = ptr2->LiquidOffset;
						float num = ptr2->Opacity * (isBackgroundDraw ? 1f : DEFAULT_OPACITY[ptr2->Type]);
						int num2 = ptr2->Type;
						switch (num2)
						{
						case 0:
							num2 = waterStyle;
							num *= globalAlpha;
							break;
						case 1:
							num *= Main.player[Main.myPlayer].lavaOpacity;
							break;
						case 2:
							num2 = 11;
							break;
						}
						num = Math.Min(1f, num);
						Lighting.GetCornerColors(i, j, out var vertices);
						ref Color bottomLeftColor = ref vertices.BottomLeftColor;
						bottomLeftColor *= num;
						ref Color bottomRightColor = ref vertices.BottomRightColor;
						bottomRightColor *= num;
						ref Color topLeftColor = ref vertices.TopLeftColor;
						topLeftColor *= num;
						ref Color topRightColor = ref vertices.TopRightColor;
						topRightColor *= num;
						Main.DrawTileInWater(drawOffset, i, j);
						Main.tileBatch.Draw(_liquidTextures[num2].Value, new Vector2((float)(i << 4), (float)(j << 4)) + drawOffset + liquidOffset, sourceRectangle, vertices, Vector2.Zero, 1f, (SpriteEffects)0);
					}
					ptr2++;
				}
			}
		}
		int value = Main.tileBatch.End();
		(isBackgroundDraw ? TimeLogger.LiquidBackgroundDrawCalls : TimeLogger.LiquidDrawCalls).Add(value);
	}

	public unsafe void DrawShimmer(SpriteBatch spriteBatch, Vector2 drawOffset, bool isBackgroundDraw)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		Rectangle drawArea = _drawArea;
		Main.tileBatch.Restart();
		fixed (SpecialLiquidDrawCache* ptr = &_drawCacheForShimmer[0])
		{
			SpecialLiquidDrawCache* ptr2 = ptr;
			int num = _drawCacheForShimmer.Length;
			for (int i = 0; i < num; i++)
			{
				if (!ptr2->IsVisible)
				{
					break;
				}
				Main.tileBatch.SetLayer(0u, 0);
				Rectangle sourceRectangle = ptr2->SourceRectangle;
				if (ptr2->IsSurfaceLiquid)
				{
					sourceRectangle.Y = 1280;
				}
				else
				{
					sourceRectangle.Y += _animationFrame * 80;
				}
				Vector2 liquidOffset = ptr2->LiquidOffset;
				float val = ptr2->Opacity * (isBackgroundDraw ? 1f : 0.75f);
				int num2 = 14;
				val = Math.Min(1f, val);
				int num3 = ptr2->X + drawArea.X - 2;
				int num4 = ptr2->Y + drawArea.Y - 2;
				Lighting.GetCornerColors(num3, num4, out var vertices);
				SetShimmerVertexColors(ref vertices, val, num3, num4);
				Main.DrawTileInWater(drawOffset, num3, num4);
				Main.tileBatch.Draw(_liquidTextures[num2].Value, new Vector2((float)(num3 << 4), (float)(num4 << 4)) + drawOffset + liquidOffset, sourceRectangle, vertices, Vector2.Zero, 1f, (SpriteEffects)0);
				sourceRectangle = ptr2->SourceRectangle;
				bool flag = sourceRectangle.X != 16 || sourceRectangle.Y % 80 != 48;
				if (flag || (num3 + num4) % 2 == 0)
				{
					sourceRectangle.X += 48;
					sourceRectangle.Y += 80 * GetShimmerFrame(flag, num3, num4);
					SetShimmerVertexColors_Sparkle(ref vertices, ptr2->Opacity, num3, num4, flag);
					Main.tileBatch.Draw(_liquidTextures[num2].Value, new Vector2((float)(num3 << 4), (float)(num4 << 4)) + drawOffset + liquidOffset, sourceRectangle, vertices, Vector2.Zero, 1f, (SpriteEffects)0);
				}
				ptr2++;
			}
		}
		int value = Main.tileBatch.End();
		(isBackgroundDraw ? TimeLogger.LiquidBackgroundDrawCalls : TimeLogger.LiquidDrawCalls).Add(value);
	}

	public static VertexColors SetShimmerVertexColors_Sparkle(ref VertexColors colors, float opacity, int x, int y, bool top)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		colors.BottomLeftColor = GetShimmerGlitterColor(top, x, y + 1);
		colors.BottomRightColor = GetShimmerGlitterColor(top, x + 1, y + 1);
		colors.TopLeftColor = GetShimmerGlitterColor(top, x, y);
		colors.TopRightColor = GetShimmerGlitterColor(top, x + 1, y);
		ref Color bottomLeftColor = ref colors.BottomLeftColor;
		bottomLeftColor *= opacity;
		ref Color bottomRightColor = ref colors.BottomRightColor;
		bottomRightColor *= opacity;
		ref Color topLeftColor = ref colors.TopLeftColor;
		topLeftColor *= opacity;
		ref Color topRightColor = ref colors.TopRightColor;
		topRightColor *= opacity;
		return colors;
	}

	public static void SetShimmerVertexColors(ref VertexColors colors, float opacity, int x, int y)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		colors.BottomLeftColor = Color.White;
		colors.BottomRightColor = Color.White;
		colors.TopLeftColor = Color.White;
		colors.TopRightColor = Color.White;
		ref Color bottomLeftColor = ref colors.BottomLeftColor;
		bottomLeftColor *= opacity;
		ref Color bottomRightColor = ref colors.BottomRightColor;
		bottomRightColor *= opacity;
		ref Color topLeftColor = ref colors.TopLeftColor;
		topLeftColor *= opacity;
		ref Color topRightColor = ref colors.TopRightColor;
		topRightColor *= opacity;
		colors.BottomLeftColor = new Color(colors.BottomLeftColor.ToVector4() * GetShimmerBaseColor(x, y + 1));
		colors.BottomRightColor = new Color(colors.BottomRightColor.ToVector4() * GetShimmerBaseColor(x + 1, y + 1));
		colors.TopLeftColor = new Color(colors.TopLeftColor.ToVector4() * GetShimmerBaseColor(x, y));
		colors.TopRightColor = new Color(colors.TopRightColor.ToVector4() * GetShimmerBaseColor(x + 1, y));
	}

	public static float GetShimmerWave(ref float worldPositionX, ref float worldPositionY)
	{
		return (float)Math.Sin(((double)((worldPositionX + worldPositionY / 6f) / 10f) - Main.timeForVisualEffects / 360.0) * 6.2831854820251465);
	}

	public static Color GetShimmerGlitterColor(bool top, float worldPositionX, float worldPositionY)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		Color val = Main.hslToRgb((float)(((double)(worldPositionX + worldPositionY / 6f) + Main.timeForVisualEffects / 30.0) / 6.0) % 1f, 1f, 0.5f);
		val.A = 0;
		return new Color(val.ToVector4() * GetShimmerGlitterOpacity(top, worldPositionX, worldPositionY));
	}

	public static float GetShimmerGlitterOpacity(bool top, float worldPositionX, float worldPositionY)
	{
		if (top)
		{
			return 0.5f;
		}
		float num = Utils.Remap((float)Math.Sin(((double)((worldPositionX + worldPositionY / 6f) / 10f) - Main.timeForVisualEffects / 360.0) * 6.2831854820251465), -0.5f, 1f, 0f, 0.35f);
		float num2 = (float)Math.Sin((double)((float)SimpleWhiteNoise((uint)worldPositionX, (uint)worldPositionY) / 10f) + Main.timeForVisualEffects / 180.0);
		return Utils.Remap(num * num2, 0f, 0.5f, 0f, 1f);
	}

	private static uint SimpleWhiteNoise(uint x, uint y)
	{
		x = 36469 * (x & 0xFFFF) + (x >> 16);
		y = 18012 * (y & 0xFFFF) + (y >> 16);
		return (x << 16) + y;
	}

	public int GetShimmerFrame(bool top, float worldPositionX, float worldPositionY)
	{
		worldPositionX += 0.5f;
		worldPositionY += 0.5f;
		double num = (double)((worldPositionX + worldPositionY / 6f) / 10f) - Main.timeForVisualEffects / 360.0;
		if (!top)
		{
			num += (double)(worldPositionX + worldPositionY);
		}
		return ((int)num % 16 + 16) % 16;
	}

	public static Vector4 GetShimmerBaseColor(float worldPositionX, float worldPositionY)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		float shimmerWave = GetShimmerWave(ref worldPositionX, ref worldPositionY);
		return Vector4.Lerp(new Vector4(165f / 255f, 130f / 255f, 14f / 15f, 1f), new Vector4(205f / 255f, 205f / 255f, 1f, 1f), 0.1f + shimmerWave * 0.4f);
	}

	public bool HasFullWater(int x, int y)
	{
		x -= _drawArea.X;
		y -= _drawArea.Y;
		int num = x * _drawArea.Height + y;
		if (num >= 0 && num < _drawCache.Length)
		{
			if (_drawCache[num].IsVisible)
			{
				return !_drawCache[num].IsSurfaceLiquid;
			}
			return false;
		}
		return true;
	}

	public float GetVisibleLiquid(int x, int y)
	{
		x -= _drawArea.X;
		y -= _drawArea.Y;
		if (x < 0 || x >= _drawArea.Width || y < 0 || y >= _drawArea.Height)
		{
			return 0f;
		}
		int num = (x + 2) * (_drawArea.Height + 4) + y + 2;
		if (!_cache[num].HasVisibleLiquid)
		{
			return 0f;
		}
		return _cache[num].VisibleLiquidLevel;
	}

	public void Update(GameTime gameTime)
	{
		if (!FocusHelper.PauseLiquidRenderer)
		{
			float num = Main.windSpeedCurrent * 25f;
			num = ((!(num < 0f)) ? (num + 6f) : (num - 6f));
			_frameState += num * (float)gameTime.ElapsedGameTime.TotalSeconds;
			_waterfallFrameState += 0.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (_frameState < 0f)
			{
				_frameState += 16f;
			}
			_frameState %= 16f;
			_waterfallFrameState %= 16f;
			_animationFrame = (int)_frameState;
			_waterfallAnimationFrame = (int)_waterfallFrameState;
		}
	}

	public void PrepareDraw(Rectangle drawArea)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		InternalPrepareDraw(drawArea);
	}

	public void SetWaveMaskData(ref Texture2D texture)
	{
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected Obj, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected Obj, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (texture == null || texture.Width < _drawArea.Height || texture.Height < _drawArea.Width)
			{
				Console.WriteLine("WaveMaskData texture recreated. {0}x{1}", _drawArea.Height, _drawArea.Width);
				if (texture != null)
				{
					try
					{
						((GraphicsResource)texture).Dispose();
					}
					catch
					{
					}
				}
				texture = new Texture2D(((Game)Main.instance).GraphicsDevice, _drawArea.Height, _drawArea.Width, false, (SurfaceFormat)0);
			}
			texture.SetData<Color>(0, (Rectangle?)new Rectangle(0, 0, _drawArea.Height, _drawArea.Width), _waveMask, 0, _drawArea.Width * _drawArea.Height);
		}
		catch
		{
			texture = new Texture2D(((Game)Main.instance).GraphicsDevice, _drawArea.Height, _drawArea.Width, false, (SurfaceFormat)0);
			texture.SetData<Color>(0, (Rectangle?)new Rectangle(0, 0, _drawArea.Height, _drawArea.Width), _waveMask, 0, _drawArea.Width * _drawArea.Height);
		}
	}

	public Rectangle GetCachedDrawArea()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return _drawArea;
	}
}
