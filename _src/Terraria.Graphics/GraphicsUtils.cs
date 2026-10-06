using System;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics;

public static class GraphicsUtils
{
	private static FieldInfo SpriteBatch_spriteQueueCount;

	private static FieldInfo SpriteBatch_spriteTextures;

	public static int PendingDrawCallCount(this SpriteBatch spriteBatch)
	{
		if (SpriteBatch_spriteQueueCount == null)
		{
			bool flag = typeof(SpriteBatch).Assembly.GetName().Name == "Microsoft.Xna.Framework.Graphics";
			SpriteBatch_spriteQueueCount = typeof(SpriteBatch).GetField(flag ? "spriteQueueCount" : "numSprites", BindingFlags.Instance | BindingFlags.NonPublic);
			SpriteBatch_spriteTextures = typeof(SpriteBatch).GetField(flag ? "spriteTextures" : "textureInfo", BindingFlags.Instance | BindingFlags.NonPublic);
		}
		int n = (int)SpriteBatch_spriteQueueCount.GetValue(spriteBatch);
		Texture[] textures = (Texture[])(object)(Texture2D[])SpriteBatch_spriteTextures.GetValue(spriteBatch);
		return DrawCallCount(textures, n);
	}

	private static int DrawCallCount(Texture[] textures, int n)
	{
		int num = 0;
		if (Program.IsXna)
		{
			Texture val = null;
			int num2 = 0;
			int vbPos = 0;
			for (int i = 0; i < n; i++)
			{
				Texture val2 = textures[i];
				if (val2 != val)
				{
					if (i > num2)
					{
						num += DrawCallCountXNA(i - num2, ref vbPos);
					}
					num2 = i;
					val = val2;
				}
			}
			num += DrawCallCountXNA(n - num2, ref vbPos);
		}
		else
		{
			int num3 = 0;
			while (true)
			{
				int num4 = Math.Min(n, 2048);
				Texture val3 = textures[num3];
				for (int j = 1; j < num4; j++)
				{
					Texture val4 = textures[num3 + j];
					if (val4 != val3)
					{
						num++;
						val3 = val4;
					}
				}
				num++;
				if (n <= 2048)
				{
					break;
				}
				n -= 2048;
				num3 += 2048;
			}
		}
		return num;
	}

	private static int DrawCallCountXNA(int count, ref int vbPos)
	{
		int num = 0;
		while (count > 0)
		{
			int num2 = count;
			if (num2 > 2048 - vbPos)
			{
				num2 = 2048 - vbPos;
				if (num2 < 256)
				{
					vbPos = 0;
					num2 = count;
					if (num2 > 2048)
					{
						num2 = 2048;
					}
				}
			}
			vbPos += num2;
			count -= num2;
			num++;
		}
		return num;
	}
}
