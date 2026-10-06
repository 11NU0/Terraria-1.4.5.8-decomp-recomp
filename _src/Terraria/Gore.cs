using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria;

public class Gore
{
	public static int goreTime = 600;

	public Vector2 position;

	public Vector2 velocity;

	public float rotation;

	public float scale;

	public int alpha;

	public int type;

	public float light;

	public bool active;

	public bool sticky = true;

	public int timeLeft = goreTime;

	public bool behindTiles;

	public byte frameCounter;

	public SpriteFrame Frame = new SpriteFrame(1, 1);

	public float Width
	{
		get
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (TextureAssets.Gore[type].IsLoaded)
			{
				return scale * (float)Frame.GetSourceRectangle(TextureAssets.Gore[type].Value).Width;
			}
			return 1f;
		}
	}

	public float Height
	{
		get
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (TextureAssets.Gore[type].IsLoaded)
			{
				return scale * (float)Frame.GetSourceRectangle(TextureAssets.Gore[type].Value).Height;
			}
			return 1f;
		}
	}

	public Rectangle AABBRectangle
	{
		get
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			if (TextureAssets.Gore[type].IsLoaded)
			{
				Rectangle sourceRectangle = Frame.GetSourceRectangle(TextureAssets.Gore[type].Value);
				return new Rectangle((int)position.X, (int)position.Y, (int)((float)sourceRectangle.Width * scale), (int)((float)sourceRectangle.Height * scale));
			}
			return new Rectangle(0, 0, 1, 1);
		}
	}

	[Old("Please use Frame instead.")]
	public byte frame
	{
		get
		{
			return Frame.CurrentRow;
		}
		set
		{
			Frame.CurrentRow = value;
		}
	}

	[Old("Please use Frame instead.")]
	public byte numFrames
	{
		get
		{
			return Frame.RowCount;
		}
		set
		{
			SpriteFrame spriteFrame = new SpriteFrame(Frame.ColumnCount, value);
			spriteFrame.CurrentColumn = Frame.CurrentColumn;
			spriteFrame.CurrentRow = Frame.CurrentRow;
			SpriteFrame spriteFrame2 = spriteFrame;
			Frame = spriteFrame2;
		}
	}

	private void UpdateAmbientFloorCloud()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		timeLeft -= GoreID.Sets.DisappearSpeed[type];
		if (timeLeft <= 0)
		{
			active = false;
			return;
		}
		bool flag = false;
		Point val = (position + new Vector2(15f, 0f)).ToTileCoordinates();
		Tile tile = Main.tile[val.X, val.Y];
		Tile tile2 = Main.tile[val.X, val.Y + 1];
		Tile tile3 = Main.tile[val.X, val.Y + 2];
		if (tile == null || tile2 == null || tile3 == null)
		{
			active = false;
			return;
		}
		if (WorldGen.SolidTile(tile) || (!WorldGen.SolidTile(tile2) && !WorldGen.SolidTile(tile3)))
		{
			flag = true;
		}
		if (timeLeft <= 30)
		{
			flag = true;
		}
		velocity.X = 0.4f * Main.WindForVisuals;
		if (!flag)
		{
			if (alpha > 220)
			{
				alpha--;
			}
		}
		else
		{
			alpha++;
			if (alpha >= 255)
			{
				active = false;
				return;
			}
		}
		position += velocity;
	}

	private void UpdateAmbientAirborneCloud()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		timeLeft -= GoreID.Sets.DisappearSpeed[type];
		if (timeLeft <= 0)
		{
			active = false;
			return;
		}
		bool flag = false;
		Point val = (position + new Vector2(15f, 0f)).ToTileCoordinates();
		rotation = velocity.ToRotation();
		Tile tile = Main.tile[val.X, val.Y];
		if (tile == null)
		{
			active = false;
			return;
		}
		if (WorldGen.SolidTile(tile))
		{
			flag = true;
		}
		if (timeLeft <= 60)
		{
			flag = true;
		}
		if (!flag)
		{
			if (alpha > 240 && Main.rand.Next(5) == 0)
			{
				alpha--;
			}
		}
		else
		{
			if (Main.rand.Next(5) == 0)
			{
				alpha++;
			}
			if (alpha >= 255)
			{
				active = false;
				return;
			}
		}
		position += velocity;
	}

	private void UpdateFogMachineCloud()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		timeLeft -= GoreID.Sets.DisappearSpeed[type];
		if (timeLeft <= 0)
		{
			active = false;
			return;
		}
		bool flag = false;
		Point val = (position + new Vector2(15f, 0f)).ToTileCoordinates();
		if (WorldGen.SolidTile(Main.tile[val.X, val.Y]))
		{
			flag = true;
		}
		if (timeLeft <= 240)
		{
			flag = true;
		}
		if (!flag)
		{
			if (alpha > 225 && Main.rand.Next(2) == 0)
			{
				alpha--;
			}
		}
		else
		{
			if (Main.rand.Next(2) == 0)
			{
				alpha++;
			}
			if (alpha >= 255)
			{
				active = false;
				return;
			}
		}
		position += velocity;
	}

	private void UpdateLightningBunnySparks()
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		if (frameCounter == 0)
		{
			frameCounter = 1;
			Frame.CurrentRow = (byte)Main.rand.Next(3);
		}
		timeLeft -= GoreID.Sets.DisappearSpeed[type];
		if (timeLeft <= 0)
		{
			active = false;
			return;
		}
		alpha = (int)MathHelper.Lerp(255f, 0f, (float)timeLeft / 15f);
		float num = (255f - (float)alpha) / 255f;
		num *= scale;
		Lighting.AddLight(position + new Vector2(Width / 2f, Height / 2f), num * 0.4f, num, num);
		position += velocity;
	}

	private float ChumFloatingChunk_GetWaterLine(int X, int Y)
	{
		float result = position.Y + Height;
		if (Main.tile[X, Y - 1] == null)
		{
			Main.tile[X, Y - 1] = new Tile();
		}
		if (Main.tile[X, Y] == null)
		{
			Main.tile[X, Y] = new Tile();
		}
		if (Main.tile[X, Y + 1] == null)
		{
			Main.tile[X, Y + 1] = new Tile();
		}
		if (Main.tile[X, Y - 1].liquid > 0)
		{
			result = Y * 16;
			result -= (float)(Main.tile[X, Y - 1].liquid / 16);
		}
		else if (Main.tile[X, Y].liquid > 0)
		{
			result = (Y + 1) * 16;
			result -= (float)(Main.tile[X, Y].liquid / 16);
		}
		else if (Main.tile[X, Y + 1].liquid > 0)
		{
			result = (Y + 2) * 16;
			result -= (float)(Main.tile[X, Y + 1].liquid / 16);
		}
		return result;
	}

	private bool DeactivateIfOutsideOfWorld()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		Point val = position.ToTileCoordinates();
		if (!WorldGen.InWorld(val.X, val.Y))
		{
			active = false;
			return true;
		}
		if (Main.tile[val.X, val.Y] == null)
		{
			active = false;
			return true;
		}
		return false;
	}

	public void Update()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_172a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2114: Unknown result type (might be due to invalid IL or missing references)
		//IL_211a: Unknown result type (might be due to invalid IL or missing references)
		//IL_211f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2124: Unknown result type (might be due to invalid IL or missing references)
		//IL_207f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2085: Unknown result type (might be due to invalid IL or missing references)
		//IL_2095: Unknown result type (might be due to invalid IL or missing references)
		//IL_209a: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17be: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1794: Unknown result type (might be due to invalid IL or missing references)
		//IL_179b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_196f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1989: Unknown result type (might be due to invalid IL or missing references)
		//IL_198f: Unknown result type (might be due to invalid IL or missing references)
		//IL_141f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1426: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d33: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1571: Unknown result type (might be due to invalid IL or missing references)
		//IL_158b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1591: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1122: Unknown result type (might be due to invalid IL or missing references)
		//IL_1127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 2 || !active)
		{
			return;
		}
		if (sticky)
		{
			if (DeactivateIfOutsideOfWorld())
			{
				return;
			}
			float num = velocity.Length();
			if (num > 32f)
			{
				velocity *= 32f / num;
			}
		}
		switch (GoreID.Sets.SpecialAI[type])
		{
		case 4:
			UpdateAmbientFloorCloud();
			return;
		case 5:
			UpdateAmbientAirborneCloud();
			return;
		case 6:
			UpdateFogMachineCloud();
			return;
		case 7:
			UpdateLightningBunnySparks();
			return;
		}
		if ((type == 1217 || type == 1218) && frameCounter == 0)
		{
			frameCounter = 1;
			Frame.CurrentRow = (byte)Main.rand.Next(3);
		}
		bool flag = type >= 1024 && type <= 1026;
		if (type >= 276 && type <= 282)
		{
			velocity.X *= 0.98f;
			velocity.Y *= 0.98f;
			if (velocity.Y < scale)
			{
				velocity.Y += 0.05f;
			}
			if ((double)velocity.Y > 0.1)
			{
				if (velocity.X > 0f)
				{
					rotation += 0.01f;
				}
				else
				{
					rotation -= 0.01f;
				}
			}
		}
		if (type >= 570 && type <= 572)
		{
			scale -= 0.001f;
			if ((double)scale <= 0.01)
			{
				scale = 0.01f;
				timeLeft = 0;
			}
			sticky = false;
			rotation = velocity.X * 0.1f;
		}
		else if (type >= 0 && type < GoreID.Count && GoreID.Sets.IsDrip[type])
		{
			if (type == 943 || (type >= 1160 && type <= 1162))
			{
				alpha = 0;
			}
			else if ((double)position.Y < Main.worldSurface * 16.0 + 8.0)
			{
				alpha = 0;
			}
			else
			{
				alpha = 100;
			}
			int num2 = 4;
			frameCounter++;
			if (frame <= 4)
			{
				int num3 = (int)(position.X / 16f);
				int num4 = (int)(position.Y / 16f) - 1;
				if (WorldGen.InWorld(num3, num4) && !Main.tile[num3, num4].active())
				{
					active = false;
				}
				if (frame == 0)
				{
					num2 = 24 + Main.rand.Next(256);
				}
				if (frame == 1)
				{
					num2 = 24 + Main.rand.Next(256);
				}
				if (frame == 2)
				{
					num2 = 24 + Main.rand.Next(256);
				}
				if (frame == 3)
				{
					num2 = 24 + Main.rand.Next(96);
				}
				if (frame == 5)
				{
					num2 = 16 + Main.rand.Next(64);
				}
				if (type == 716)
				{
					num2 *= 2;
				}
				if (type == 717)
				{
					num2 *= 4;
				}
				if ((type == 943 || (type >= 1160 && type <= 1162)) && frame < 6)
				{
					num2 = 4;
				}
				if (frameCounter >= num2)
				{
					frameCounter = 0;
					frame++;
					if (frame == 5)
					{
						int num5 = NewGore(position, velocity, type);
						Main.gore[num5].frame = 9;
						Gore obj = Main.gore[num5];
						obj.velocity *= 0f;
					}
				}
			}
			else if (frame <= 6)
			{
				num2 = 8;
				if (type == 716)
				{
					num2 *= 2;
				}
				if (type == 717)
				{
					num2 *= 3;
				}
				if (frameCounter >= num2)
				{
					frameCounter = 0;
					frame++;
					if (frame == 7)
					{
						active = false;
					}
				}
			}
			else if (frame <= 9)
			{
				num2 = 6;
				if (type == 716)
				{
					num2 = (int)((double)num2 * 1.5);
					velocity.Y += 0.175f;
				}
				else if (type == 717)
				{
					num2 *= 2;
					velocity.Y += 0.15f;
				}
				else if (type == 943)
				{
					num2 = (int)((double)num2 * 1.5);
					velocity.Y += 0.2f;
				}
				else
				{
					velocity.Y += 0.2f;
				}
				if ((double)velocity.Y < 0.5)
				{
					velocity.Y = 0.5f;
				}
				if (velocity.Y > 12f)
				{
					velocity.Y = 12f;
				}
				if (frameCounter >= num2)
				{
					frameCounter = 0;
					frame++;
				}
				if (frame > 9)
				{
					frame = 7;
				}
			}
			else
			{
				if (type == 716)
				{
					num2 *= 2;
				}
				else if (type == 717)
				{
					num2 *= 6;
				}
				velocity.Y += 0.1f;
				if (frameCounter >= num2)
				{
					frameCounter = 0;
					frame++;
				}
				velocity *= 0f;
				if (frame > 14)
				{
					active = false;
				}
			}
		}
		else if (type == 11 || type == 12 || type == 13 || type == 61 || type == 62 || type == 63 || type == 99 || type == 220 || type == 221 || type == 222 || (type >= 375 && type <= 377) || (type >= 435 && type <= 437) || (type >= 861 && type <= 862))
		{
			velocity.Y *= 0.98f;
			velocity.X *= 0.98f;
			scale -= 0.007f;
			if ((double)scale < 0.1)
			{
				scale = 0.1f;
				alpha = 255;
			}
		}
		else if (type == 16 || type == 17)
		{
			velocity.Y *= 0.98f;
			velocity.X *= 0.98f;
			scale -= 0.01f;
			if ((double)scale < 0.1)
			{
				scale = 0.1f;
				alpha = 255;
			}
		}
		else if (type == 1201)
		{
			if (frameCounter == 0)
			{
				frameCounter = 1;
				Frame.CurrentRow = (byte)Main.rand.Next(4);
			}
			scale -= 0.002f;
			if ((double)scale < 0.1)
			{
				scale = 0.1f;
				alpha = 255;
			}
			rotation += velocity.X * 0.1f;
			int num6 = (int)(position.X + 6f) / 16;
			int num7 = (int)(position.Y - 6f) / 16;
			if (Main.tile[num6, num7] == null || Main.tile[num6, num7].liquid <= 0)
			{
				velocity.Y += 0.2f;
				if (velocity.Y < 0f)
				{
					velocity *= 0.92f;
				}
			}
			else
			{
				velocity.Y += 0.005f;
				float num8 = velocity.Length();
				if (num8 > 1f)
				{
					velocity *= 0.1f;
				}
				else if (num8 > 0.1f)
				{
					velocity *= 0.98f;
				}
			}
		}
		else if (type == 1208)
		{
			if (frameCounter == 0)
			{
				frameCounter = 1;
				Frame.CurrentRow = (byte)Main.rand.Next(4);
			}
			Vector2 val = position + new Vector2(Width, Height) / 2f;
			int num9 = (int)val.X / 16;
			int num10 = (int)val.Y / 16;
			bool flag2 = Main.tile[num9, num10] != null && Main.tile[num9, num10].liquid > 0;
			scale -= 0.0005f;
			if ((double)scale < 0.1)
			{
				scale = 0.1f;
				alpha = 255;
			}
			rotation += velocity.X * 0.1f;
			if (flag2)
			{
				velocity.X *= 0.9f;
				int num11 = (int)val.X / 16;
				int num12 = (int)(val.Y / 16f);
				_ = position.Y / 16f;
				int num13 = (int)((position.Y + Height) / 16f);
				if (Main.tile[num11, num12] == null)
				{
					Main.tile[num11, num12] = new Tile();
				}
				if (Main.tile[num11, num13] == null)
				{
					Main.tile[num11, num13] = new Tile();
				}
				if (velocity.Y > 0f)
				{
					velocity.Y *= 0.5f;
				}
				num11 = (int)(val.X / 16f);
				num12 = (int)(val.Y / 16f);
				float num14 = ChumFloatingChunk_GetWaterLine(num11, num12);
				if (val.Y > num14)
				{
					velocity.Y -= 0.1f;
					if (velocity.Y < -8f)
					{
						velocity.Y = -8f;
					}
					if (val.Y + velocity.Y < num14)
					{
						velocity.Y = num14 - val.Y;
					}
				}
				else
				{
					velocity.Y = num14 - val.Y;
				}
				bool flag3 = !flag2 && velocity.Length() < 0.8f;
				int maxValue = (flag2 ? 270 : 15);
				if (Main.rand.Next(maxValue) == 0 && !flag3)
				{
					Gore gore = NewGoreDirect(position + Vector2.UnitY * 6f, Vector2.Zero, 1201, scale * 0.7f);
					if (flag2)
					{
						gore.velocity = Vector2.UnitX * Main.rand.NextFloatDirection() * 0.5f + Vector2.UnitY * Main.rand.NextFloat();
					}
					else if (gore.velocity.Y < 0f)
					{
						gore.velocity.Y = 0f - gore.velocity.Y;
					}
				}
			}
			else
			{
				if (velocity.Y == 0f)
				{
					velocity.X *= 0.95f;
				}
				velocity.X *= 0.98f;
				velocity.Y += 0.3f;
				if (velocity.Y > 15.9f)
				{
					velocity.Y = 15.9f;
				}
			}
		}
		else if (type == 331)
		{
			alpha += 5;
			velocity.Y *= 0.95f;
			velocity.X *= 0.95f;
			rotation = velocity.X * 0.1f;
		}
		else if (GoreID.Sets.SpecialAI[type] == 3)
		{
			if (++frameCounter >= 8 && velocity.Y > 0.2f)
			{
				frameCounter = 0;
				int num15 = Frame.CurrentRow / 4;
				if (++Frame.CurrentRow >= 4 + num15 * 4)
				{
					Frame.CurrentRow = (byte)(num15 * 4);
				}
			}
		}
		else if (GoreID.Sets.SpecialAI[type] != 1 && GoreID.Sets.SpecialAI[type] != 2)
		{
			if (type >= 907 && type <= 909)
			{
				rotation = 0f;
				velocity.X *= 0.98f;
				if (velocity.Y > 0f && velocity.Y < 0.001f)
				{
					velocity.Y = -0.5f + Main.rand.NextFloat() * -3f;
				}
				if (velocity.Y > -1f)
				{
					velocity.Y -= 0.1f;
				}
				if (scale < 1f)
				{
					scale += 0.1f;
				}
				if (++frameCounter >= 8)
				{
					frameCounter = 0;
					if (++frame >= 3)
					{
						frame = 0;
					}
				}
			}
			else if (type == 1218)
			{
				if (timeLeft > 8)
				{
					timeLeft = 8;
				}
				velocity.X *= 0.95f;
				if (Math.Abs(velocity.X) <= 0.1f)
				{
					velocity.X = 0f;
				}
				if (alpha < 100 && velocity.Length() > 0f && Main.rand.Next(5) == 0)
				{
					int num16 = 246;
					switch (Frame.CurrentRow)
					{
					case 0:
						num16 = 246;
						break;
					case 1:
						num16 = 245;
						break;
					case 2:
						num16 = 244;
						break;
					}
					int num17 = Dust.NewDust(position + new Vector2(6f, 4f), 4, 4, num16);
					Main.dust[num17].alpha = 255;
					Main.dust[num17].scale = 0.8f;
					Main.dust[num17].velocity = Vector2.Zero;
				}
				velocity.Y += 0.2f;
				rotation = 0f;
			}
			else if (type < 411 || type > 430)
			{
				velocity.Y += 0.2f;
				rotation += velocity.X * 0.05f;
			}
			else if (GoreID.Sets.SpecialAI[type] != 3)
			{
				rotation += velocity.X * 0.1f;
			}
		}
		if (type >= 580 && type <= 582)
		{
			rotation = 0f;
			velocity.X *= 0.95f;
		}
		if (GoreID.Sets.SpecialAI[type] == 2)
		{
			if (timeLeft < 60)
			{
				alpha += Main.rand.Next(1, 7);
			}
			else if (alpha > 100)
			{
				alpha -= Main.rand.Next(1, 4);
			}
			if (alpha < 0)
			{
				alpha = 0;
			}
			if (alpha > 255)
			{
				timeLeft = 0;
			}
			velocity.X = (velocity.X * 50f + Main.WindForVisuals * 2f + (float)Main.rand.Next(-10, 11) * 0.1f) / 51f;
			float num18 = 0f;
			if (velocity.X < 0f)
			{
				num18 = velocity.X * 0.2f;
			}
			velocity.Y = (velocity.Y * 50f + -0.35f + num18 + (float)Main.rand.Next(-10, 11) * 0.2f) / 51f;
			rotation = velocity.X * 0.6f;
			float num19 = -1f;
			if (TextureAssets.Gore[type].IsLoaded)
			{
				Rectangle val2 = new Rectangle((int)position.X, (int)position.Y, (int)((float)TextureAssets.Gore[type].Width() * scale), (int)((float)TextureAssets.Gore[type].Height() * scale));
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active && !Main.player[i].dead)
					{
						Rectangle val3 = new Rectangle((int)Main.player[i].position.X, (int)Main.player[i].position.Y, Main.player[i].width, Main.player[i].height);
						if (val2.Intersects(val3))
						{
							timeLeft = 0;
							num19 = Main.player[i].velocity.Length();
							break;
						}
					}
				}
			}
			if (timeLeft > 0)
			{
				if (Main.rand.Next(2) == 0)
				{
					timeLeft--;
				}
				if (Main.rand.Next(50) == 0)
				{
					timeLeft -= 5;
				}
				if (Main.rand.Next(100) == 0)
				{
					timeLeft -= 10;
				}
			}
			else
			{
				alpha = 255;
				if (TextureAssets.Gore[type].IsLoaded && num19 != -1f)
				{
					float num20 = (float)TextureAssets.Gore[type].Width() * scale * 0.8f;
					float x = position.X;
					float y = position.Y;
					float num21 = (float)TextureAssets.Gore[type].Width() * scale;
					float num22 = (float)TextureAssets.Gore[type].Height() * scale;
					int num23 = 31;
					for (int j = 0; (float)j < num20; j++)
					{
						int num24 = Dust.NewDust(new Vector2(x, y), (int)num21, (int)num22, num23);
						Dust obj2 = Main.dust[num24];
						obj2.velocity *= (1f + num19) / 3f;
						Main.dust[num24].noGravity = true;
						Main.dust[num24].alpha = 100;
						Main.dust[num24].scale = scale;
					}
				}
			}
		}
		if (type >= 411 && type <= 430)
		{
			alpha = 50;
			velocity.X = (velocity.X * 50f + Main.WindForVisuals * 2f + (float)Main.rand.Next(-10, 11) * 0.1f) / 51f;
			velocity.Y = (velocity.Y * 50f + -0.25f + (float)Main.rand.Next(-10, 11) * 0.2f) / 51f;
			rotation = velocity.X * 0.3f;
			if (TextureAssets.Gore[type].IsLoaded)
			{
				Rectangle val4 = new Rectangle((int)position.X, (int)position.Y, (int)((float)TextureAssets.Gore[type].Width() * scale), (int)((float)TextureAssets.Gore[type].Height() * scale));
				for (int k = 0; k < 255; k++)
				{
					if (Main.player[k].active && !Main.player[k].dead)
					{
						Rectangle val5 = new Rectangle((int)Main.player[k].position.X, (int)Main.player[k].position.Y, Main.player[k].width, Main.player[k].height);
						if (val4.Intersects(val5))
						{
							timeLeft = 0;
						}
					}
				}
				if (Collision.SolidCollision(position, (int)((float)TextureAssets.Gore[type].Width() * scale), (int)((float)TextureAssets.Gore[type].Height() * scale)))
				{
					timeLeft = 0;
				}
			}
			if (timeLeft > 0)
			{
				if (Main.rand.Next(2) == 0)
				{
					timeLeft--;
				}
				if (Main.rand.Next(50) == 0)
				{
					timeLeft -= 5;
				}
				if (Main.rand.Next(100) == 0)
				{
					timeLeft -= 10;
				}
			}
			else
			{
				alpha = 255;
				if (TextureAssets.Gore[type].IsLoaded)
				{
					float num25 = (float)TextureAssets.Gore[type].Width() * scale * 0.8f;
					float x2 = position.X;
					float y2 = position.Y;
					float num26 = (float)TextureAssets.Gore[type].Width() * scale;
					float num27 = (float)TextureAssets.Gore[type].Height() * scale;
					int num28 = 176;
					if (type >= 416 && type <= 420)
					{
						num28 = 177;
					}
					if (type >= 421 && type <= 425)
					{
						num28 = 178;
					}
					if (type >= 426 && type <= 430)
					{
						num28 = 179;
					}
					for (int l = 0; (float)l < num25; l++)
					{
						int num29 = Dust.NewDust(new Vector2(x2, y2), (int)num26, (int)num27, num28);
						Main.dust[num29].noGravity = true;
						Main.dust[num29].alpha = 100;
						Main.dust[num29].scale = scale;
					}
				}
			}
		}
		else if (GoreID.Sets.SpecialAI[type] != 3 && GoreID.Sets.SpecialAI[type] != 1)
		{
			if (type >= 0 && type < GoreID.Count && GoreID.Sets.IsDrip[type])
			{
				if (type == 716 || type == 1383)
				{
					float num30 = 1f;
					float num31 = 1f;
					float num32 = 1f;
					float num33 = 1f;
					if (type == 716)
					{
						num30 = 1f;
						num31 = 0.5f;
						num32 = 0.1f;
						num33 = 0.6f;
					}
					else if (type == 1383)
					{
						Point val6 = position.ToTileCoordinates();
						Vector4 shimmerBaseColor = LiquidRenderer.GetShimmerBaseColor(val6.X, val6.Y);
						num30 = shimmerBaseColor.X;
						num31 = shimmerBaseColor.Y;
						num32 = shimmerBaseColor.Z;
						num33 = 0.7f;
					}
					if (frame != 0)
					{
						if (frame == 1)
						{
							num33 *= 0.2f;
						}
						else if (frame == 2)
						{
							num33 *= 0.3f;
						}
						else if (frame == 3)
						{
							num33 *= 0.4f;
						}
						else if (frame == 4)
						{
							num33 *= 0.5f;
						}
						else if (frame == 5)
						{
							num33 *= 0.4f;
						}
						else if (frame == 6)
						{
							num33 *= 0.2f;
						}
						else if (frame <= 9)
						{
							num33 *= 0.5f;
						}
						else if (frame == 10)
						{
							num33 *= 0.5f;
						}
						else if (frame == 11)
						{
							num33 *= 0.4f;
						}
						else if (frame == 12)
						{
							num33 *= 0.3f;
						}
						else if (frame == 13)
						{
							num33 *= 0.2f;
						}
						else
						{
							num33 = ((frame != 14) ? 0f : (num33 * 0.1f));
						}
					}
					else
					{
						num33 *= 0.1f;
					}
					num30 *= num33;
					num31 *= num33;
					num32 *= num33;
					Lighting.AddLight(position + new Vector2(8f, 8f), num30, num31, num32);
				}
				bool flag4 = type == 716 || type == 717 || type == 943 || (type >= 1160 && type <= 1162);
				Vector2 val7 = velocity;
				velocity = Collision.TileCollision(position, velocity, 16, 14);
				if (velocity != val7)
				{
					if (frame < 10)
					{
						frame = 10;
						frameCounter = 0;
						if (!flag4)
						{
							SoundEngine.PlaySound(39, (int)position.X + 8, (int)position.Y + 8, Main.rand.Next(2));
						}
					}
				}
				else if (Collision.WetCollision(position + velocity, 16, 14))
				{
					if (frame < 10)
					{
						frame = 10;
						frameCounter = 0;
						if (!flag4)
						{
							SoundEngine.PlaySound(39, (int)position.X + 8, (int)position.Y + 8, 2);
						}
						((WaterShaderData)Filters.Scene["WaterDistortion"].GetShader()).QueueRipple(position + new Vector2(8f, 8f));
					}
					int num34 = (int)(position.X + 8f) / 16;
					int num35 = (int)(position.Y + 14f) / 16;
					if (Main.tile[num34, num35] != null && Main.tile[num34, num35].liquid > 0)
					{
						velocity *= 0f;
						position.Y = num35 * 16 - Main.tile[num34, num35].liquid / 16;
					}
				}
			}
			else if (sticky)
			{
				int num36 = 32;
				if (TextureAssets.Gore[type].IsLoaded)
				{
					num36 = TextureAssets.Gore[type].Width();
					if (TextureAssets.Gore[type].Height() < num36)
					{
						num36 = TextureAssets.Gore[type].Height();
					}
				}
				if (flag)
				{
					num36 = 4;
				}
				num36 = (int)((float)num36 * 0.9f);
				_ = velocity;
				velocity = Collision.TileCollision(position, velocity, (int)((float)num36 * scale), (int)((float)num36 * scale));
				if (velocity.Y == 0f)
				{
					if (flag)
					{
						velocity.X *= 0.94f;
					}
					else
					{
						velocity.X *= 0.97f;
					}
					if ((double)velocity.X > -0.01 && (double)velocity.X < 0.01)
					{
						velocity.X = 0f;
					}
				}
				if (timeLeft > 0)
				{
					timeLeft -= GoreID.Sets.DisappearSpeed[type];
				}
				else
				{
					alpha += GoreID.Sets.DisappearSpeedAlpha[type];
				}
			}
			else
			{
				alpha += 2 * GoreID.Sets.DisappearSpeedAlpha[type];
			}
		}
		if (type >= 907 && type <= 909)
		{
			int num37 = 32;
			if (TextureAssets.Gore[type].IsLoaded)
			{
				num37 = TextureAssets.Gore[type].Width();
				if (TextureAssets.Gore[type].Height() < num37)
				{
					num37 = TextureAssets.Gore[type].Height();
				}
			}
			num37 = (int)((float)num37 * 0.9f);
			Vector4 val8 = Collision.SlopeCollision(position, velocity, num37, num37, 0f, fall: true);
			position.X = val8.X;
			position.Y = val8.Y;
			velocity.X = val8.Z;
			velocity.Y = val8.W;
		}
		if (GoreID.Sets.SpecialAI[type] == 1)
		{
			Gore_UpdateSail();
		}
		else if (GoreID.Sets.SpecialAI[type] == 3)
		{
			Gore_UpdateLeaf();
		}
		else
		{
			position += velocity;
		}
		if (alpha >= 255)
		{
			active = false;
		}
		if (light > 0f)
		{
			float num38 = light * scale;
			float num39 = light * scale;
			float num40 = light * scale;
			if (type == 16)
			{
				num40 *= 0.3f;
				num39 *= 0.8f;
			}
			else if (type == 17)
			{
				num39 *= 0.6f;
				num38 *= 0.3f;
			}
			if (TextureAssets.Gore[type].IsLoaded)
			{
				Lighting.AddLight((int)((position.X + (float)TextureAssets.Gore[type].Width() * scale / 2f) / 16f), (int)((position.Y + (float)TextureAssets.Gore[type].Height() * scale / 2f) / 16f), num38, num39, num40);
			}
			else
			{
				Lighting.AddLight((int)((position.X + 32f * scale / 2f) / 16f), (int)((position.Y + 32f * scale / 2f) / 16f), num38, num39, num40);
			}
		}
	}

	private void Gore_UpdateLeaf()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = position + new Vector2(12f) / 2f - new Vector2(4f) / 2f;
		val.Y -= 4f;
		Vector2 val2 = position - val;
		if (velocity.Y < 0f)
		{
			Vector2 val3 = new Vector2(velocity.X, -0.2f);
			int num = 4;
			num = (int)((float)num * 0.9f);
			Point val4 = (new Vector2((float)num, (float)num) / 2f + val).ToTileCoordinates();
			if (!WorldGen.InWorld(val4.X, val4.Y))
			{
				active = false;
				return;
			}
			Tile tile = Main.tile[val4.X, val4.Y];
			if (tile == null)
			{
				active = false;
				return;
			}
			int num2 = 6;
			Rectangle val5 = new Rectangle(val4.X * 16, val4.Y * 16 + tile.liquid / 16, 16, 16 - tile.liquid / 16);
			Rectangle val6 = new Rectangle((int)val.X, (int)val.Y + num2, num, num);
			bool flag = tile != null && tile.liquid > 0 && val5.Intersects(val6);
			if (flag)
			{
				if (tile.honey())
				{
					val3.X = 0f;
				}
				else if (tile.lava())
				{
					active = false;
					for (int i = 0; i < 5; i++)
					{
						Dust.NewDust(position, num, num, 31, 0f, -0.2f);
					}
				}
				else
				{
					val3.X = Main.WindForVisuals;
				}
				if ((double)position.Y > Main.worldSurface * 16.0)
				{
					val3.X = 0f;
				}
			}
			if (!WorldGen.SolidTile(val4.X, val4.Y + 1) && !flag)
			{
				velocity.Y = 0.1f;
				timeLeft = 0;
				alpha += 20;
			}
			val3 = Collision.TileCollision(val, val3, num, num);
			if (flag)
			{
				rotation = val3.ToRotation() + (float)Math.PI / 2f;
			}
			val3.X *= 0.94f;
			if (!flag || ((double)val3.X > -0.01 && (double)val3.X < 0.01))
			{
				val3.X = 0f;
			}
			if (timeLeft > 0)
			{
				timeLeft -= GoreID.Sets.DisappearSpeed[type];
			}
			else
			{
				alpha += GoreID.Sets.DisappearSpeedAlpha[type];
			}
			velocity.X = val3.X;
			position.X += velocity.X;
			return;
		}
		velocity.Y += (float)Math.PI / 180f;
		Vector2 val7 = new Vector2(Vector2.UnitY.RotatedBy(velocity.Y).X * 1f, Math.Abs(Vector2.UnitY.RotatedBy(velocity.Y).Y) * 1f);
		int num3 = 4;
		if ((double)position.Y < Main.worldSurface * 16.0)
		{
			val7.X += Main.WindForVisuals * 4f;
		}
		Vector2 val8 = val7;
		val7 = Collision.TileCollision(val, val7, num3, num3);
		Vector4 val9 = Collision.SlopeCollision(val, val7, num3, num3, 1f);
		position.X = val9.X;
		position.Y = val9.Y;
		val7.X = val9.Z;
		val7.Y = val9.W;
		position += val2;
		if (val7 != val8)
		{
			velocity.Y = -1f;
		}
		Point val10 = (new Vector2(Width, Height) * 0.5f + position).ToTileCoordinates();
		if (!WorldGen.InWorld(val10.X, val10.Y))
		{
			active = false;
			return;
		}
		Tile tile2 = Main.tile[val10.X, val10.Y];
		if (tile2 == null)
		{
			active = false;
			return;
		}
		int num4 = 6;
		Rectangle val11 = new Rectangle(val10.X * 16, val10.Y * 16 + tile2.liquid / 16, 16, 16 - tile2.liquid / 16);
		Rectangle val12 = new Rectangle((int)val.X, (int)val.Y + num4, num3, num3);
		if (tile2 != null && tile2.liquid > 0 && val11.Intersects(val12))
		{
			velocity.Y = -1f;
		}
		position += val7;
		rotation = val7.ToRotation() + (float)Math.PI / 2f;
		if (timeLeft > 0)
		{
			timeLeft -= GoreID.Sets.DisappearSpeed[type];
		}
		else
		{
			alpha += GoreID.Sets.DisappearSpeedAlpha[type];
		}
	}

	private void Gore_UpdateSail()
	{
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		if (velocity.Y < 0f)
		{
			Vector2 val = new Vector2(velocity.X, 0.6f);
			int num = 32;
			if (TextureAssets.Gore[type].IsLoaded)
			{
				num = TextureAssets.Gore[type].Width();
				if (TextureAssets.Gore[type].Height() < num)
				{
					num = TextureAssets.Gore[type].Height();
				}
			}
			num = (int)((float)num * 0.9f);
			val = Collision.TileCollision(position, val, (int)((float)num * scale), (int)((float)num * scale));
			val.X *= 0.97f;
			if ((double)val.X > -0.01 && (double)val.X < 0.01)
			{
				val.X = 0f;
			}
			if (timeLeft > 0)
			{
				timeLeft--;
			}
			else
			{
				alpha++;
			}
			velocity.X = val.X;
			return;
		}
		velocity.Y += (float)Math.PI / 60f;
		Vector2 val2 = new Vector2(Vector2.UnitY.RotatedBy(velocity.Y).X * 2f, Math.Abs(Vector2.UnitY.RotatedBy(velocity.Y).Y) * 3f);
		val2 *= 2f;
		int num2 = 32;
		if (TextureAssets.Gore[type].IsLoaded)
		{
			num2 = TextureAssets.Gore[type].Width();
			if (TextureAssets.Gore[type].Height() < num2)
			{
				num2 = TextureAssets.Gore[type].Height();
			}
		}
		Vector2 val3 = val2;
		val2 = Collision.TileCollision(position, val2, (int)((float)num2 * scale), (int)((float)num2 * scale));
		if (val2 != val3)
		{
			velocity.Y = -1f;
		}
		position += val2;
		rotation = val2.ToRotation() + (float)Math.PI;
		if (timeLeft > 0)
		{
			timeLeft--;
		}
		else
		{
			alpha++;
		}
	}

	public static Gore NewGorePerfect(Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Gore gore = NewGoreDirect(Position, Velocity, Type, Scale);
		gore.position = Position;
		gore.velocity = Velocity;
		return gore;
	}

	public static Gore NewGoreDirect(Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Main.gore[NewGore(Position, Velocity, Type, Scale)];
	}

	public static int NewGore(Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 2)
		{
			return 600;
		}
		if (Main.gamePaused)
		{
			return 600;
		}
		if (WorldGen.isGeneratingOrLoadingWorld)
		{
			return 600;
		}
		if (Main.rand == null)
		{
			Main.rand = new UnifiedRandom();
		}
		if (Type == -1)
		{
			return 600;
		}
		int num = 600;
		for (int i = 0; i < 600; i++)
		{
			if (!Main.gore[i].active)
			{
				num = i;
				break;
			}
		}
		if (num == 600)
		{
			return num;
		}
		Main.gore[num].Frame = new SpriteFrame(1, 1);
		Main.gore[num].frameCounter = 0;
		Main.gore[num].behindTiles = false;
		Main.gore[num].light = 0f;
		Main.gore[num].position = Position;
		Main.gore[num].velocity = Velocity;
		Main.gore[num].velocity.Y -= (float)Main.rand.Next(10, 31) * 0.1f;
		Main.gore[num].velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
		Main.gore[num].type = Type;
		Main.gore[num].active = true;
		Main.gore[num].alpha = 0;
		Main.gore[num].rotation = 0f;
		Main.gore[num].scale = Scale;
		if (!ChildSafety.Disabled && ChildSafety.DangerousGore(Type))
		{
			Type = Main.rand.Next(11, 14);
			Main.gore[num].type = Type;
			Main.gore[num].scale = Main.rand.NextFloat() * 0.5f + 0.5f;
			Gore obj = Main.gore[num];
			obj.velocity /= 2f;
		}
		if (goreTime == 0 || Type == 11 || Type == 12 || Type == 13 || Type == 16 || Type == 17 || Type == 61 || Type == 62 || Type == 63 || Type == 99 || Type == 220 || Type == 221 || Type == 222 || Type == 435 || Type == 436 || Type == 437 || (Type >= 861 && Type <= 862))
		{
			Main.gore[num].sticky = false;
		}
		else if (Type >= 375 && Type <= 377)
		{
			Main.gore[num].sticky = false;
			Main.gore[num].alpha = 100;
		}
		else
		{
			Main.gore[num].sticky = true;
			Main.gore[num].timeLeft = goreTime;
		}
		if (Type >= 0 && Type < GoreID.Count && GoreID.Sets.IsDrip[Type])
		{
			Main.gore[num].numFrames = 15;
			Main.gore[num].behindTiles = true;
			Main.gore[num].timeLeft = goreTime * 3;
		}
		if (Type == 16 || Type == 17)
		{
			Main.gore[num].alpha = 100;
			Main.gore[num].scale = 0.7f;
			Main.gore[num].light = 1f;
		}
		if (Type >= 570 && Type <= 572)
		{
			Main.gore[num].velocity = Velocity;
		}
		if (Type == 1201 || Type == 1208)
		{
			Main.gore[num].Frame = new SpriteFrame(1, 4);
		}
		if (Type == 1217 || Type == 1218)
		{
			Main.gore[num].Frame = new SpriteFrame(1, 3);
		}
		if (Type == 1225)
		{
			Main.gore[num].Frame = new SpriteFrame(1, 3);
			Main.gore[num].timeLeft = 10 + Main.rand.Next(6);
			Main.gore[num].sticky = false;
			if (TextureAssets.Gore[Type].IsLoaded)
			{
				Main.gore[num].position.X = Position.X - (float)(TextureAssets.Gore[Type].Width() / 2) * Scale;
				Main.gore[num].position.Y = Position.Y - (float)TextureAssets.Gore[Type].Height() * Scale / 2f;
			}
		}
		int num2 = GoreID.Sets.SpecialAI[Type];
		if (num2 == 3)
		{
			Main.gore[num].velocity = new Vector2((Main.rand.NextFloat() - 0.5f) * 1f, Main.rand.NextFloat() * ((float)Math.PI * 2f));
			bool flag = (Type >= 910 && Type <= 925) || (Type >= 1113 && Type <= 1121) || (Type >= 1248 && Type <= 1255) || Type == 1257 || Type == 1278;
			Gore obj2 = Main.gore[num];
			SpriteFrame spriteFrame = new SpriteFrame((byte)((!flag) ? 1u : 32u), 8)
			{
				CurrentRow = (byte)Main.rand.Next(8)
			};
			obj2.Frame = spriteFrame;
			Main.gore[num].frameCounter = (byte)Main.rand.Next(8);
		}
		if (num2 == 1)
		{
			Main.gore[num].velocity = new Vector2((Main.rand.NextFloat() - 0.5f) * 3f, Main.rand.NextFloat() * ((float)Math.PI * 2f));
		}
		if (Type >= 411 && Type <= 430 && TextureAssets.Gore[Type].IsLoaded)
		{
			Main.gore[num].position.X = Position.X - (float)(TextureAssets.Gore[Type].Width() / 2) * Scale;
			Main.gore[num].position.Y = Position.Y - (float)TextureAssets.Gore[Type].Height() * Scale;
			Main.gore[num].velocity.Y *= (float)Main.rand.Next(90, 150) * 0.01f;
			Main.gore[num].velocity.X *= (float)Main.rand.Next(40, 90) * 0.01f;
			int num3 = Main.rand.Next(4) * 5;
			Main.gore[num].type += num3;
			Main.gore[num].timeLeft = Main.rand.Next(goreTime / 2, goreTime * 2);
			Main.gore[num].sticky = true;
			if (goreTime == 0)
			{
				Main.gore[num].timeLeft = Main.rand.Next(150, 600);
			}
		}
		if (Type >= 907 && Type <= 909)
		{
			Main.gore[num].sticky = true;
			Main.gore[num].numFrames = 3;
			Main.gore[num].frame = (byte)Main.rand.Next(3);
			Main.gore[num].frameCounter = (byte)Main.rand.Next(5);
			Main.gore[num].rotation = 0f;
		}
		if (num2 == 2)
		{
			Main.gore[num].sticky = false;
			if (TextureAssets.Gore[Type].IsLoaded)
			{
				Main.gore[num].alpha = 150;
				Main.gore[num].velocity = Velocity;
				Main.gore[num].position.X = Position.X - (float)(TextureAssets.Gore[Type].Width() / 2) * Scale;
				Main.gore[num].position.Y = Position.Y - (float)TextureAssets.Gore[Type].Height() * Scale / 2f;
				Main.gore[num].timeLeft = Main.rand.Next(goreTime / 2, goreTime + 1);
			}
		}
		if (num2 == 4)
		{
			Main.gore[num].alpha = 254;
			Main.gore[num].timeLeft = 300;
		}
		if (num2 == 5)
		{
			Main.gore[num].alpha = 254;
			Main.gore[num].timeLeft = 240;
		}
		if (num2 == 6)
		{
			Main.gore[num].alpha = 254;
			Main.gore[num].timeLeft = 480;
		}
		if (Main.gore[num].DeactivateIfOutsideOfWorld())
		{
			return 600;
		}
		return num;
	}

	public Color GetAlpha(Color newColor)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)(255 - alpha) / 255f;
		if (type == 16 || type == 17)
		{
			return new Color(255, 255, 255, 0);
		}
		if (type == 716 || type == 1383)
		{
			return new Color(255, 255, 255, 200);
		}
		if (type >= 570 && type <= 572)
		{
			byte b = (byte)(255 - alpha);
			return new Color((int)b, (int)b, (int)b, b / 2);
		}
		if (type == 331)
		{
			return new Color(255, 255, 255, 50);
		}
		if (type == 1225)
		{
			return new Color(num, num, num, num);
		}
		int num2 = (int)((float)(int)newColor.R * num);
		int num3 = (int)((float)(int)newColor.G * num);
		int num4 = (int)((float)(int)newColor.B * num);
		int num5 = newColor.A - alpha;
		if (num5 < 0)
		{
			num5 = 0;
		}
		if (num5 > 255)
		{
			num5 = 255;
		}
		if (type >= 1202 && type <= 1204)
		{
			return new Color(num2, num3, num4, (num5 < 20) ? num5 : 20);
		}
		return new Color(num2, num3, num4, num5);
	}
}
