using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;

namespace Terraria.DataStructures;

public class SpriteDrawBuffer
{
	private readonly GraphicsDevice graphicsDevice;

	private readonly SpriteBatch spriteBatch;

	private readonly int bufferSize;

	private DynamicVertexBuffer vertexBuffer;

	private IndexBuffer indexBuffer;

	private int vertexCount;

	private VertexPositionColorTexture[] vertices;

	private Texture[] textures;

	private int uploadedSpriteIndex = -1;

	private VertexBufferBinding[] preBindVertexBuffers;

	private IndexBuffer preBindIndexBuffer;

	public SpriteDrawBuffer(GraphicsDevice graphicsDevice, int bufferSize = 2048)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected Obj, but got Unknown
		this.graphicsDevice = graphicsDevice;
		this.bufferSize = bufferSize;
		ResizeArrays(bufferSize);
		spriteBatch = new SpriteBatch(graphicsDevice);
	}

	public void ResizeArrays(int count)
	{
		Array.Resize(ref vertices, count * 4);
		Array.Resize(ref textures, count);
	}

	public void ApplyDefaultSpriteEffect(RasterizerState rasterizer, Matrix transformation)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, rasterizer, (Effect)null, transformation);
		spriteBatch.End();
	}

	public void ApplyDefaultSpriteEffect()
	{
		spriteBatch.Begin();
		spriteBatch.End();
	}

	private void CheckBuffers()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected Obj, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected Obj, but got Unknown
		if (vertexBuffer == null || ((GraphicsResource)vertexBuffer).IsDisposed)
		{
			if (vertexBuffer != null)
			{
				((GraphicsResource)vertexBuffer).Dispose();
			}
			vertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), bufferSize * 4, (BufferUsage)1);
		}
		if (indexBuffer == null || ((GraphicsResource)indexBuffer).IsDisposed)
		{
			if (indexBuffer != null)
			{
				((GraphicsResource)indexBuffer).Dispose();
			}
			indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), bufferSize * 6, (BufferUsage)1);
			indexBuffer.SetData<ushort>(GenIndexBuffer(bufferSize));
		}
	}

	private static ushort[] GenIndexBuffer(int maxSprites)
	{
		ushort[] array = new ushort[maxSprites * 6];
		int num = 0;
		ushort num2 = 0;
		while (num < maxSprites)
		{
			array[num++] = num2;
			array[num++] = (ushort)(num2 + 1);
			array[num++] = (ushort)(num2 + 2);
			array[num++] = (ushort)(num2 + 3);
			array[num++] = (ushort)(num2 + 2);
			array[num++] = (ushort)(num2 + 1);
			num2 += 4;
		}
		return array;
	}

	private void Bind()
	{
		if (preBindVertexBuffers == null)
		{
			preBindVertexBuffers = graphicsDevice.GetVertexBuffers();
			preBindIndexBuffer = graphicsDevice.Indices;
			graphicsDevice.SetVertexBuffer((VertexBuffer)(object)vertexBuffer);
			graphicsDevice.Indices = indexBuffer;
		}
	}

	public void Unbind()
	{
		if (preBindVertexBuffers != null)
		{
			graphicsDevice.SetVertexBuffers(preBindVertexBuffers);
			graphicsDevice.Indices = preBindIndexBuffer;
			preBindVertexBuffers = null;
			preBindIndexBuffer = null;
		}
	}

	public int DrawRange(int index, int count)
	{
		vertexCount = 0;
		CheckBuffers();
		Bind();
		graphicsDevice.Textures[0] = textures[index];
		int num = 0;
		while (count > 0)
		{
			if (uploadedSpriteIndex < 0 || index < uploadedSpriteIndex || index + count > uploadedSpriteIndex + bufferSize)
			{
				vertexBuffer.SetData<VertexPositionColorTexture>(vertices, index * 4, Math.Min(vertices.Length - index * 4, bufferSize * 4), (SetDataOptions)1);
				uploadedSpriteIndex = index;
			}
			int num2 = Math.Min(count, bufferSize);
			int num3 = index - uploadedSpriteIndex;
			graphicsDevice.DrawIndexedPrimitives((PrimitiveType)0, num3 * 4, 0, num2 * 4, 0, num2 * 2);
			count -= num2;
			index += num2;
			num++;
		}
		return num;
	}

	public void DrawSingle(int index)
	{
		DrawRange(index, 1);
	}

	public int DrawAll()
	{
		if (vertexCount == 0)
		{
			return 0;
		}
		int num = vertexCount / 4;
		Texture val = textures[0];
		int num2 = 0;
		int num3 = 0;
		for (int i = 1; i < num; i++)
		{
			Texture val2 = textures[i];
			if (val2 != val)
			{
				num3 += DrawRange(num2, i - num2);
				num2 = i;
				val = val2;
			}
		}
		num3 += DrawRange(num2, num - num2);
		Unbind();
		return num3;
	}

	public void Draw(Texture2D texture, Vector2 position, VertexColors colors)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, position, null, colors, 0f, Vector2.Zero, 1f, (SpriteEffects)0);
	}

	public void Draw(Texture2D texture, Rectangle destination, VertexColors colors)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, destination, null, colors);
	}

	public void Draw(Texture2D texture, Rectangle destination, Rectangle? sourceRectangle, VertexColors colors)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, destination, sourceRectangle, colors, 0f, Vector2.Zero, (SpriteEffects)0);
	}

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors color, float rotation, Vector2 origin, float scale, SpriteEffects effects)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale, scale), effects);
	}

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		float num;
		float num2;
		if (sourceRectangle.HasValue)
		{
			num = (float)sourceRectangle.Value.Width * scale.X;
			num2 = (float)sourceRectangle.Value.Height * scale.Y;
		}
		else
		{
			num = (float)texture.Width * scale.X;
			num2 = (float)texture.Height * scale.Y;
		}
		Draw(texture, new Vector4(position.X, position.Y, num, num2), sourceRectangle, colors, rotation, origin, effects);
	}

	public void Draw(Texture2D texture, Rectangle destination, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, SpriteEffects effects)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, new Vector4((float)destination.X, (float)destination.Y, (float)destination.Width, (float)destination.Height), sourceRectangle, colors, rotation, origin, effects);
	}

	public void Draw(Texture2D texture, Vector4 destination, VertexColors colors, float rotation = 0f, Vector2 origin = default(Vector2), SpriteEffects effects = (SpriteEffects)0)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, destination, null, colors, rotation, origin, effects);
	}

	public void Draw(Texture2D texture, Vector4 destinationRectangle, Rectangle? sourceRectangle, VertexColors colors, float rotation = 0f, Vector2 origin = default(Vector2), SpriteEffects effect = (SpriteEffects)0, float depth = 0f)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		Vector4 val = default;
		if (sourceRectangle.HasValue)
		{
			val.X = sourceRectangle.Value.X;
			val.Y = sourceRectangle.Value.Y;
			val.Z = sourceRectangle.Value.Width;
			val.W = sourceRectangle.Value.Height;
		}
		else
		{
			val.X = 0f;
			val.Y = 0f;
			val.Z = texture.Width;
			val.W = texture.Height;
		}
		Vector2 val2 = default;
		val2.X = val.X / (float)texture.Width;
		val2.Y = val.Y / (float)texture.Height;
		Vector2 val3 = default;
		val3.X = (val.X + val.Z) / (float)texture.Width;
		val3.Y = (val.Y + val.W) / (float)texture.Height;
		if (((int)effect & 2) != 0)
		{
			float y = val3.Y;
			val3.Y = val2.Y;
			val2.Y = y;
		}
		if (((int)effect & 1) != 0)
		{
			float x = val3.X;
			val3.X = val2.X;
			val2.X = x;
		}
		QueueSprite(destinationRectangle, -origin, colors, val, val2, val3, texture, depth, rotation);
	}

	private void QueueSprite(Vector4 destinationRect, Vector2 origin, VertexColors colors, Vector4 sourceRectangle, Vector2 texCoordTL, Vector2 texCoordBR, Texture2D texture, float depth, float rotation)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		uploadedSpriteIndex = -1;
		float num = origin.X / sourceRectangle.Z;
		float num2 = origin.Y / sourceRectangle.W;
		float x = destinationRect.X;
		float y = destinationRect.Y;
		float z = destinationRect.Z;
		float w = destinationRect.W;
		float num3 = num * z;
		float num4 = num2 * w;
		float num5;
		float num6;
		if (rotation != 0f)
		{
			num5 = (float)Math.Cos(rotation);
			num6 = (float)Math.Sin(rotation);
		}
		else
		{
			num5 = 1f;
			num6 = 0f;
		}
		int num7 = vertexCount / 4;
		if (num7 >= textures.Length)
		{
			ResizeArrays(textures.Length * 2);
		}
		textures[num7] = (Texture)(object)texture;
		PushVertex(new Vector3(x + num3 * num5 - num4 * num6, y + num3 * num6 + num4 * num5, depth), colors.TopLeftColor, texCoordTL);
		PushVertex(new Vector3(x + (num3 + z) * num5 - num4 * num6, y + (num3 + z) * num6 + num4 * num5, depth), colors.TopRightColor, new Vector2(texCoordBR.X, texCoordTL.Y));
		PushVertex(new Vector3(x + num3 * num5 - (num4 + w) * num6, y + num3 * num6 + (num4 + w) * num5, depth), colors.BottomLeftColor, new Vector2(texCoordTL.X, texCoordBR.Y));
		PushVertex(new Vector3(x + (num3 + z) * num5 - (num4 + w) * num6, y + (num3 + z) * num6 + (num4 + w) * num5, depth), colors.BottomRightColor, texCoordBR);
	}

	private void PushVertex(Vector3 pos, Color color, Vector2 texCoord)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		SetVertex(ref vertices[vertexCount++], pos, color, texCoord);
	}

	private static void SetVertex(ref VertexPositionColorTexture vertex, Vector3 pos, Color color, Vector2 texCoord)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		vertex.Position = pos;
		vertex.Color = color;
		vertex.TextureCoordinate = texCoord;
	}
}
