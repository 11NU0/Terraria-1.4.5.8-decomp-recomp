using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Testing;

namespace Terraria.Graphics;

public class TileBatch
{
	private struct SpriteData
	{
		public Vector4 Source;

		public Vector4 Destination;

		public Vector2 Origin;

		public VertexColors Colors;

		public float Rotation;

		public SpriteEffects Effects;
	}

	private struct DataSlice
	{
		public int Start;

		public int Length;

		public int Next;
	}

	private struct LayerBatch : IComparable<LayerBatch>
	{
		public uint LayerStack;

		public ushort Texture;

		public int Head;

		public int Tail;

		public int Length;

		public int NextSprite;

		public ulong SortKey => ((ulong)LayerStack << 16) | Texture;

		public bool CurrentSliceIsFull
		{
			get
			{
				if (Length >= 2)
				{
					return (Length & (Length - 1)) == 0;
				}
				return false;
			}
		}

		public int CompareTo(LayerBatch other)
		{
			return SortKey.CompareTo(other.SortKey);
		}
	}

	private struct LayerBatchKey : IEquatable<LayerBatchKey>
	{
		public uint LayerStack;

		public Texture2D Texture;

		public bool Equals(LayerBatchKey other)
		{
			return LayerStack == other.LayerStack;
		}

		public override int GetHashCode()
		{
			return (int)LayerStack ^ ((object)Texture).GetHashCode();
		}
	}

	private struct RecentLayerCacheEntry(Texture texture, int batchIndex)
	{
		public readonly Texture Texture = texture;

		public readonly int BatchIndex = batchIndex;
	}

	private const int MinSliceLength = 2;

	private static readonly float[] CORNER_OFFSET_X = new float[4] { 0f, 1f, 1f, 0f };

	private static readonly float[] CORNER_OFFSET_Y = new float[4] { 0f, 0f, 1f, 1f };

	private GraphicsDevice _graphicsDevice;

	private SpriteData[] _spriteDataQueue = new SpriteData[2048];

	private Texture2D[] _spriteTextures = new Texture2D[2048];

	private int _queuedSpriteCount;

	private bool _layeredSortingEnabled;

	private DataSlice[] _batchData = new DataSlice[2048];

	private int _batchDataCount;

	private LayerBatch[] _batches = new LayerBatch[2048];

	private int _batchCount;

	private uint? _nextLayerStack;

	private int _currentBatchIndex;

	private LayerBatchKey _currentBatchKey;

	private Dictionary<LayerBatchKey, int> _batchLookup = new Dictionary<LayerBatchKey, int>();

	private readonly RecentLayerCacheEntry[] _batchLookupCache = new RecentLayerCacheEntry[2048];

	private Texture2D[] _passTextures = new Texture2D[512];

	private int _passTextureCount;

	private Dictionary<Texture2D, ushort> _textureIdLookup = new Dictionary<Texture2D, ushort>();

	private SpriteBatch _spriteBatch;

	private static Vector2 _vector2Zero;

	private static Rectangle? _nullRectangle;

	private DynamicVertexBuffer _vertexBuffer;

	private DynamicIndexBuffer _indexBuffer;

	private short[] _fallbackIndexData;

	private VertexPositionColorTexture[] _vertices = new VertexPositionColorTexture[8192];

	private int _vertexBufferPosition;

	private int _drawCalls;

	public TileBatch(GraphicsDevice graphicsDevice)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected Obj, but got Unknown
		_graphicsDevice = graphicsDevice;
		_spriteBatch = new SpriteBatch(graphicsDevice);
		Allocate();
	}

	private void Allocate()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected Obj, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected Obj, but got Unknown
		if (_vertexBuffer == null || ((GraphicsResource)_vertexBuffer).IsDisposed)
		{
			_vertexBuffer = new DynamicVertexBuffer(_graphicsDevice, typeof(VertexPositionColorTexture), 8192, (BufferUsage)1);
			_vertexBufferPosition = 0;
			_vertexBuffer.ContentLost += delegate
			{
				_vertexBufferPosition = 0;
			};
		}
		if (_indexBuffer != null && !((GraphicsResource)_indexBuffer).IsDisposed)
		{
			return;
		}
		if (_fallbackIndexData == null)
		{
			_fallbackIndexData = new short[12288];
			for (int num = 0; num < 2048; num++)
			{
				_fallbackIndexData[num * 6] = (short)(num * 4);
				_fallbackIndexData[num * 6 + 1] = (short)(num * 4 + 1);
				_fallbackIndexData[num * 6 + 2] = (short)(num * 4 + 2);
				_fallbackIndexData[num * 6 + 3] = (short)(num * 4);
				_fallbackIndexData[num * 6 + 4] = (short)(num * 4 + 2);
				_fallbackIndexData[num * 6 + 5] = (short)(num * 4 + 3);
			}
		}
		_indexBuffer = new DynamicIndexBuffer(_graphicsDevice, typeof(short), 12288, (BufferUsage)1);
		((IndexBuffer)_indexBuffer).SetData<short>(_fallbackIndexData);
		_indexBuffer.ContentLost += delegate
		{
			((IndexBuffer)_indexBuffer).SetData<short>(_fallbackIndexData);
		};
	}

	private void FlushRenderState()
	{
		Allocate();
		_graphicsDevice.SetVertexBuffer((VertexBuffer)(object)_vertexBuffer);
		_graphicsDevice.Indices = (IndexBuffer)(object)_indexBuffer;
		_graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		_drawCalls = 0;
	}

	public void Dispose()
	{
		if (_vertexBuffer != null)
		{
			((GraphicsResource)_vertexBuffer).Dispose();
		}
		if (_indexBuffer != null)
		{
			((GraphicsResource)_indexBuffer).Dispose();
		}
	}

	public void Begin(RasterizerState rasterizer, Matrix transformation)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, rasterizer, (Effect)null, transformation);
		_spriteBatch.End();
		if (_queuedSpriteCount > 0)
		{
			throw new InvalidOperationException("Sprites have already been added before calling Begin");
		}
	}

	public void Begin()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Begin(RasterizerState.CullCounterClockwise, Matrix.Identity);
	}

	public int Restart()
	{
		return End();
	}

	public void SetLayer(uint layer, byte stack = 0)
	{
		if (layer >= 16777216)
		{
			throw new ArgumentOutOfRangeException("Max Layer Exceeded");
		}
		SetLayerStack((layer << 8) | stack);
	}

	public void SetLayerStack(uint layerStack)
	{
		if (!_layeredSortingEnabled)
		{
			if (_queuedSpriteCount > 0)
			{
				throw new InvalidOperationException("Sprites have already been added before setting the first layer");
			}
			_layeredSortingEnabled = true;
		}
		_nextLayerStack = layerStack;
	}

	public unsafe void Draw(Texture2D texture, Vector2 position, VertexColors colors)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		int nextSpriteIndex = GetNextSpriteIndex(texture);
		fixed (SpriteData* ptr = &_spriteDataQueue[nextSpriteIndex])
		{
			Vector2 val = texture.Size();
			ptr->Source.X = 0f;
			ptr->Source.Y = 0f;
			ptr->Source.Z = val.X;
			ptr->Source.W = val.Y;
			ptr->Destination.X = position.X;
			ptr->Destination.Y = position.Y;
			ptr->Destination.Z = val.X;
			ptr->Destination.W = val.Y;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Origin, default(Vector2));
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Colors, colors);
			ptr->Rotation = 0f;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Effects, (SpriteEffects)0);
		}
	}

	public unsafe void Draw(Texture2D texture, Vector2 position, Rectangle sourceRectangle, VertexColors colors, Vector2 origin = default(Vector2), float scale = 1f, SpriteEffects effects = (SpriteEffects)0)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		int nextSpriteIndex = GetNextSpriteIndex(texture);
		fixed (SpriteData* ptr = &_spriteDataQueue[nextSpriteIndex])
		{
			ptr->Source.X = sourceRectangle.X;
			ptr->Source.Y = sourceRectangle.Y;
			ptr->Source.Z = sourceRectangle.Width;
			ptr->Source.W = sourceRectangle.Height;
			ptr->Destination.X = position.X;
			ptr->Destination.Y = position.Y;
			ptr->Destination.Z = scale * (float)sourceRectangle.Width;
			ptr->Destination.W = scale * (float)sourceRectangle.Height;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Origin, origin);
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Colors, colors);
			ptr->Rotation = 0f;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Effects, effects);
		}
	}

	public unsafe void Draw(Texture2D texture, Vector4 destination, VertexColors colors)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		int nextSpriteIndex = GetNextSpriteIndex(texture);
		fixed (SpriteData* ptr = &_spriteDataQueue[nextSpriteIndex])
		{
			ptr->Source.X = 0f;
			ptr->Source.Y = 0f;
			ptr->Source.Z = texture.Width;
			ptr->Source.W = texture.Height;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Destination, destination);
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Origin, default(Vector2));
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Colors, colors);
			ptr->Rotation = 0f;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Effects, (SpriteEffects)0);
		}
	}

	public unsafe void Draw(Texture2D texture, Vector4 destination, Rectangle sourceRectangle, VertexColors colors)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		int nextSpriteIndex = GetNextSpriteIndex(texture);
		fixed (SpriteData* ptr = &_spriteDataQueue[nextSpriteIndex])
		{
			ptr->Source.X = sourceRectangle.X;
			ptr->Source.Y = sourceRectangle.Y;
			ptr->Source.Z = sourceRectangle.Width;
			ptr->Source.W = sourceRectangle.Height;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Destination, destination);
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Origin, default(Vector2));
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Colors, colors);
			ptr->Rotation = 0f;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Effects, (SpriteEffects)0);
		}
	}

	public unsafe void Draw(Texture2D texture, Vector4 destination, Rectangle sourceRectangle, VertexColors colors, Vector2 origin = default(Vector2), SpriteEffects effects = (SpriteEffects)0, float rotation = 0f)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		int nextSpriteIndex = GetNextSpriteIndex(texture);
		fixed (SpriteData* ptr = &_spriteDataQueue[nextSpriteIndex])
		{
			ptr->Source.X = sourceRectangle.X;
			ptr->Source.Y = sourceRectangle.Y;
			ptr->Source.Z = sourceRectangle.Width;
			ptr->Source.W = sourceRectangle.Height;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Destination, destination);
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Origin, origin);
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Colors, colors);
			ptr->Rotation = rotation;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Effects, effects);
		}
	}

	public unsafe void Draw(Texture2D texture, Vector4 destination, VertexColors colors, Vector2 origin = default(Vector2), SpriteEffects effects = (SpriteEffects)0, float rotation = 0f)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		int nextSpriteIndex = GetNextSpriteIndex(texture);
		fixed (SpriteData* ptr = &_spriteDataQueue[nextSpriteIndex])
		{
			ptr->Source.X = 0f;
			ptr->Source.Y = 0f;
			ptr->Source.Z = texture.Width;
			ptr->Source.W = texture.Height;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Destination, destination);
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Origin, origin);
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Colors, colors);
			ptr->Rotation = rotation;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Effects, effects);
		}
	}

	public unsafe void Draw(Texture2D texture, Rectangle destinationRectangle, VertexColors colors)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		int nextSpriteIndex = GetNextSpriteIndex(texture);
		fixed (SpriteData* ptr = &_spriteDataQueue[nextSpriteIndex])
		{
			ptr->Source.X = 0f;
			ptr->Source.Y = 0f;
			ptr->Source.Z = texture.Width;
			ptr->Source.W = texture.Height;
			ptr->Destination.X = destinationRectangle.X;
			ptr->Destination.Y = destinationRectangle.Y;
			ptr->Destination.Z = destinationRectangle.Width;
			ptr->Destination.W = destinationRectangle.Height;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Origin, default(Vector2));
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Colors, colors);
			ptr->Rotation = 0f;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Effects, (SpriteEffects)0);
		}
	}

	public unsafe void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, VertexColors colors)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		int nextSpriteIndex = GetNextSpriteIndex(texture);
		fixed (SpriteData* ptr = &_spriteDataQueue[nextSpriteIndex])
		{
			ptr->Source.X = sourceRectangle.X;
			ptr->Source.Y = sourceRectangle.Y;
			ptr->Source.Z = sourceRectangle.Width;
			ptr->Source.W = sourceRectangle.Height;
			ptr->Destination.X = destinationRectangle.X;
			ptr->Destination.Y = destinationRectangle.Y;
			ptr->Destination.Z = destinationRectangle.Width;
			ptr->Destination.W = destinationRectangle.Height;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Origin, default(Vector2));
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Colors, colors);
			ptr->Rotation = 0f;
			System.Runtime.CompilerServices.Unsafe.Write(&ptr->Effects, (SpriteEffects)0);
		}
	}

	private static short[] CreateIndexData()
	{
		short[] array = new short[12288];
		for (int i = 0; i < 2048; i++)
		{
			array[i * 6] = (short)(i * 4);
			array[i * 6 + 1] = (short)(i * 4 + 1);
			array[i * 6 + 2] = (short)(i * 4 + 2);
			array[i * 6 + 3] = (short)(i * 4);
			array[i * 6 + 4] = (short)(i * 4 + 2);
			array[i * 6 + 5] = (short)(i * 4 + 3);
		}
		return array;
	}

	private int GetNextSpriteIndex(Texture2D texture)
	{
		if (_layeredSortingEnabled)
		{
			if (_nextLayerStack.HasValue)
			{
				uint value = _nextLayerStack.Value;
				if (texture != _currentBatchKey.Texture || value != _currentBatchKey.LayerStack)
				{
					SwitchBatch(texture, value);
				}
			}
			else if (texture != _currentBatchKey.Texture)
			{
				SwitchBatch(texture, _currentBatchKey.LayerStack + 1);
			}
			_nextLayerStack = null;
			return GetNextSpriteIndex(ref _batches[_currentBatchIndex]);
		}
		if (_queuedSpriteCount >= _spriteDataQueue.Length)
		{
			Array.Resize(ref _spriteDataQueue, _spriteDataQueue.Length << 1);
		}
		if (_queuedSpriteCount >= _spriteTextures.Length)
		{
			Array.Resize(ref _spriteTextures, _spriteTextures.Length << 1);
		}
		_spriteTextures[_queuedSpriteCount] = texture;
		return _queuedSpriteCount++;
	}

	private int GetNextSpriteIndex(ref LayerBatch layerBatchState)
	{
		if (layerBatchState.CurrentSliceIsFull)
		{
			int newSpriteBufferSlice = GetNewSpriteBufferSlice(layerBatchState.Length);
			_batchData[layerBatchState.Tail].Next = newSpriteBufferSlice;
			layerBatchState.Tail = newSpriteBufferSlice;
			layerBatchState.NextSprite = _batchData[newSpriteBufferSlice].Start;
		}
		layerBatchState.Length++;
		return layerBatchState.NextSprite++;
	}

	private int GetNewSpriteBufferSlice(int length)
	{
		if (_batchDataCount == _batchData.Length)
		{
			Array.Resize(ref _batchData, _batchData.Length * 2);
		}
		int num = _batchDataCount++;
		_batchData[num] = new DataSlice
		{
			Start = _queuedSpriteCount,
			Length = length
		};
		_queuedSpriteCount += length;
		while (_queuedSpriteCount > _spriteDataQueue.Length)
		{
			Array.Resize(ref _spriteDataQueue, _spriteDataQueue.Length * 2);
		}
		return num;
	}

	private void SwitchBatch(Texture2D texture, uint layerStack)
	{
		LayerBatchKey currentBatchKey = _currentBatchKey;
		int currentBatchIndex = _currentBatchIndex;
		_currentBatchKey = new LayerBatchKey
		{
			LayerStack = layerStack,
			Texture = texture
		};
		uint num = (layerStack >> 14) | (layerStack & 0xFFFF);
		if (num < _batchLookupCache.Length && (object)_batchLookupCache[num].Texture == texture)
		{
			_currentBatchIndex = _batchLookupCache[num].BatchIndex;
		}
		else if (!_batchLookup.TryGetValue(_currentBatchKey, out _currentBatchIndex))
		{
			CreateBatch();
		}
		uint num2 = (currentBatchKey.LayerStack >> 14) | (currentBatchKey.LayerStack & 0xFFFF);
		if (num2 < _batchLookupCache.Length)
		{
			_batchLookupCache[num2] = new RecentLayerCacheEntry((Texture)(object)currentBatchKey.Texture, currentBatchIndex);
		}
	}

	private void CreateBatch()
	{
		Texture2D texture = _currentBatchKey.Texture;
		if (!_textureIdLookup.TryGetValue(texture, out var value))
		{
			value = (_textureIdLookup[texture] = (ushort)_passTextureCount);
			if (_passTextureCount == _passTextures.Length)
			{
				Array.Resize(ref _passTextures, _passTextures.Length * 2);
			}
			_passTextures[_passTextureCount++] = texture;
		}
		if (_batchCount == _batches.Length)
		{
			Array.Resize(ref _batches, _batches.Length * 2);
		}
		int newSpriteBufferSlice = GetNewSpriteBufferSlice(2);
		_batches[_currentBatchIndex = _batchCount++] = new LayerBatch
		{
			LayerStack = _currentBatchKey.LayerStack,
			Texture = value,
			Head = newSpriteBufferSlice,
			Tail = newSpriteBufferSlice,
			NextSprite = _batchData[newSpriteBufferSlice].Start
		};
		_batchLookup[_currentBatchKey] = _currentBatchIndex;
	}

	public int End()
	{
		_layeredSortingEnabled = false;
		if (_queuedSpriteCount == 0)
		{
			return 0;
		}
		FlushRenderState();
		if (_passTextureCount > 0)
		{
			FlushLayered();
		}
		else
		{
			Flush();
		}
		return _drawCalls;
	}

	private void Flush()
	{
		Texture2D val = null;
		int num = 0;
		for (int i = 0; i < _queuedSpriteCount; i++)
		{
			if (_spriteTextures[i] != val)
			{
				if (i > num)
				{
					RenderBatch(val, _spriteDataQueue, num, i - num);
				}
				num = i;
				val = _spriteTextures[i];
			}
		}
		RenderBatch(val, _spriteDataQueue, num, _queuedSpriteCount - num);
		Array.Clear(_spriteTextures, 0, _queuedSpriteCount);
		_queuedSpriteCount = 0;
	}

	private void RenderBatch(Texture2D texture, SpriteData[] sprites, int offset, int count)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		_graphicsDevice.Textures[0] = (Texture)(object)texture;
		while (count > 0)
		{
			SetDataOptions val = (SetDataOptions)2;
			int num = count;
			if (num > 2048 - _vertexBufferPosition)
			{
				num = 2048 - _vertexBufferPosition;
				if (num < 256)
				{
					_vertexBufferPosition = 0;
					val = (SetDataOptions)1;
					num = count;
					if (num > 2048)
					{
						num = 2048;
					}
				}
			}
			FillVertexBuffer(texture, sprites, offset, num, 0);
			int num2 = _vertexBufferPosition * System.Runtime.CompilerServices.Unsafe.SizeOf<VertexPositionColorTexture>() * 4;
			_vertexBuffer.SetData<VertexPositionColorTexture>(num2, _vertices, 0, num * 4, System.Runtime.CompilerServices.Unsafe.SizeOf<VertexPositionColorTexture>(), val);
			int num3 = _vertexBufferPosition * 4;
			int num4 = num * 4;
			int num5 = _vertexBufferPosition * 6;
			int num6 = num * 2;
			_graphicsDevice.DrawIndexedPrimitives((PrimitiveType)0, 0, num3, num4, num5, num6);
			_vertexBufferPosition += num;
			offset += num;
			count -= num;
			_drawCalls++;
		}
	}

	private unsafe void FillVertexBuffer(Texture2D texture, SpriteData[] sprites, int offset, int count, int vbSpriteOffset)
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f / (float)texture.Width;
		float num2 = 1f / (float)texture.Height;
		fixed (SpriteData* ptr = &sprites[offset])
		{
			fixed (VertexPositionColorTexture* ptr2 = &_vertices[vbSpriteOffset * 4])
			{
				SpriteData* ptr3 = ptr;
				VertexPositionColorTexture* ptr4 = ptr2;
				for (int i = 0; i < count; i++)
				{
					float num3;
					float num4;
					if (ptr3->Rotation != 0f)
					{
						num3 = (float)Math.Cos(ptr3->Rotation);
						num4 = (float)Math.Sin(ptr3->Rotation);
					}
					else
					{
						num3 = 1f;
						num4 = 0f;
					}
					float num5 = ptr3->Origin.X / ptr3->Source.Z;
					float num6 = ptr3->Origin.Y / ptr3->Source.W;
					System.Runtime.CompilerServices.Unsafe.Write(&ptr4->Color, ptr3->Colors.TopLeftColor);
					System.Runtime.CompilerServices.Unsafe.Write(&ptr4[1].Color, ptr3->Colors.TopRightColor);
					System.Runtime.CompilerServices.Unsafe.Write(&ptr4[2].Color, ptr3->Colors.BottomRightColor);
					System.Runtime.CompilerServices.Unsafe.Write(&ptr4[3].Color, ptr3->Colors.BottomLeftColor);
					for (int j = 0; j < 4; j++)
					{
						float num7 = CORNER_OFFSET_X[j];
						float num8 = CORNER_OFFSET_Y[j];
						float num9 = (num7 - num5) * ptr3->Destination.Z;
						float num10 = (num8 - num6) * ptr3->Destination.W;
						float x = ptr3->Destination.X + num9 * num3 - num10 * num4;
						float y = ptr3->Destination.Y + num9 * num4 + num10 * num3;
						if (((int)ptr3->Effects & 2) != 0)
						{
							num8 = 1f - num8;
						}
						if (((int)ptr3->Effects & 1) != 0)
						{
							num7 = 1f - num7;
						}
						ptr4->Position.X = x;
						ptr4->Position.Y = y;
						ptr4->Position.Z = 0f;
						ptr4->TextureCoordinate.X = (ptr3->Source.X + num7 * ptr3->Source.Z) * num;
						ptr4->TextureCoordinate.Y = (ptr3->Source.Y + num8 * ptr3->Source.W) * num2;
						ptr4++;
					}
					ptr3++;
				}
			}
		}
	}

	private void FlushLayered()
	{
		Array.Sort(_batches, 0, _batchCount);
		int vbCount = 0;
		_vertexBufferPosition = 0;
		for (int i = 0; i < _batchCount; i++)
		{
			LayerBatch layerBatch = _batches[i];
			Texture2D val = _passTextures[layerBatch.Texture];
			_graphicsDevice.Textures[0] = (Texture)(object)val;
			int num = layerBatch.Length;
			int num2 = i;
			int batchOffset = 0;
			DataSlice currentSlice = default;
			do
			{
				if (_vertexBufferPosition == vbCount)
				{
					vbCount = 0;
					_vertexBufferPosition = 0;
					while (vbCount < num && FillVertexBuffer(_batches[num2], ref currentSlice, ref batchOffset, ref vbCount))
					{
						num2++;
						batchOffset = 0;
					}
					while (vbCount < 2048 && num2 < _batchCount)
					{
						layerBatch = _batches[num2];
						if (vbCount + layerBatch.Length > 2048)
						{
							break;
						}
						FillVertexBuffer(layerBatch, ref vbCount);
						num2++;
					}
					_vertexBuffer.SetData<VertexPositionColorTexture>(_vertices, 0, vbCount * 4, (SetDataOptions)1);
				}
				int num3 = Math.Min(num, vbCount - _vertexBufferPosition);
				_graphicsDevice.DrawIndexedPrimitives((PrimitiveType)0, _vertexBufferPosition * 4, 0, num3 * 4, 0, num3 * 2);
				_vertexBufferPosition += num3;
				num -= num3;
				_drawCalls++;
			}
			while (num > 0);
		}
		_queuedSpriteCount = 0;
		_batchDataCount = 0;
		_batchCount = 0;
		_batchLookup.Clear();
		Array.Clear(_batchLookupCache, 0, _batchLookupCache.Length);
		_passTextureCount = 0;
		_textureIdLookup.Clear();
		_currentBatchKey = default;
	}

	private void FillVertexBuffer(LayerBatch batch, ref int vbCount)
	{
		DataSlice currentSlice = default;
		int batchOffset = 0;
		Invariant.Assert(FillVertexBuffer(batch, ref currentSlice, ref batchOffset, ref vbCount), "No vertices uploaded");
	}

	private bool FillVertexBuffer(LayerBatch batch, ref DataSlice currentSlice, ref int batchOffset, ref int vbCount)
	{
		if (batchOffset == 0)
		{
			currentSlice = _batchData[batch.Head];
		}
		Texture2D texture = _passTextures[batch.Texture];
		while (batchOffset < batch.Length)
		{
			if (currentSlice.Length == 0)
			{
				currentSlice = _batchData[currentSlice.Next];
			}
			int num = Math.Min(Math.Min(batch.Length - batchOffset, currentSlice.Length), 2048 - vbCount);
			if (num == 0)
			{
				return false;
			}
			FillVertexBuffer(texture, _spriteDataQueue, currentSlice.Start, num, vbCount);
			vbCount += num;
			batchOffset += num;
			currentSlice.Start += num;
			currentSlice.Length -= num;
		}
		return true;
	}
}
