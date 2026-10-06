using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.LeashedEntities;

public abstract class LeashedCritter : LeashedEntity
{
	protected static NPC _dummy = new NPC();

	public int anchorStyle;

	protected int npcType;

	protected int spriteDirection;

	protected Rectangle frame;

	protected double frameCounter;

	protected LCG32Random rand;

	protected short WaitTime;

	protected byte State;

	protected Point16 TargetPosition;

	protected Vector2 netOffset;

	protected float scale = 1f;

	protected int strayingRangeInBlocksX;

	protected int strayingRangeInBlocksY;

	protected bool isAquatic;

	protected bool drawBubble = true;

	protected static readonly float gravity = 0.3f;

	protected static readonly float maxFallSpeed = 10f;

	protected const int RecallDuration = 20;

	public void SetDefaults(int itemType)
	{
		SetDefaults(ContentSamples.ItemsByType[itemType]);
	}

	protected virtual void SetDefaults(Item sample)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		npcType = sample.makeNPC;
		_dummy.SetDefaults(npcType);
		Size = _dummy.Size;
	}

	public override void NetSend(BinaryWriter writer, bool full)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (full)
		{
			writer.Write7BitEncodedInt(npcType);
			writer.WriteVector2(Size);
		}
		writer.WritePackedVector2(position - AnchorPosition.ToWorldCoordinates());
		writer.Write(direction > 0);
		writer.Write(rand.state);
		writer.Write(WaitTime);
		writer.Write(State);
		writer.Write((sbyte)(TargetPosition.X - AnchorPosition.X));
		writer.Write((sbyte)(TargetPosition.Y - AnchorPosition.Y));
	}

	public override void NetReceive(BinaryReader reader, bool full)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		if (full)
		{
			npcType = reader.Read7BitEncodedInt();
			Size = reader.ReadVector2();
		}
		Vector2 val = position;
		position = reader.ReadPackedVector2() + AnchorPosition.ToWorldCoordinates();
		direction = (reader.ReadBoolean() ? 1 : (-1));
		rand.state = reader.ReadUInt32();
		WaitTime = reader.ReadInt16();
		State = reader.ReadByte();
		TargetPosition = new Point16(AnchorPosition.X + reader.ReadSByte(), AnchorPosition.Y + reader.ReadSByte());
		if (full)
		{
			netOffset = Vector2.Zero;
		}
		else
		{
			netOffset += val - position;
		}
		if (full)
		{
			Update();
		}
	}

	public override void Spawn(bool newlyAdded)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Center = AnchorPosition.ToWorldCoordinates();
		TargetPosition = AnchorPosition;
		rand = new LCG32Random((uint)Main.rand.Next());
	}

	public override void Update()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		netOffset = netOffset.MoveTowards(Vector2.Zero, 2f);
	}

	protected void Recall()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		bool flag = Main.netMode != 2;
		if (flag)
		{
			Recall_CreateDustAtLocation();
		}
		Center = AnchorPosition.ToWorldCoordinates() - new Vector2(0f, 16f);
		velocity = Vector2.Zero;
		if (flag)
		{
			Recall_CreateDustAtLocation();
		}
	}

	private void Recall_CreateDustAtLocation()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 10; i++)
		{
			Dust dust = Dust.NewDustDirect(position, width, height, 15, 0f, 0f, 150, default, 1.1f);
			dust.noLight = (dust.noLightEmittance = true);
		}
	}

	protected virtual void VisualEffects()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		if (npcType < 0 || !NPCID.Sets.IsGoldCritter[npcType])
		{
			return;
		}
		position += netOffset;
		Color color = Lighting.GetColor((int)Center.X / 16, (int)Center.Y / 16);
		if (color.R > 20 || color.B > 20 || color.G > 20)
		{
			int num = color.R;
			if (color.G > num)
			{
				num = color.G;
			}
			if (color.B > num)
			{
				num = color.B;
			}
			num /= 30;
			if (Main.rand.Next(300) < num)
			{
				int num2 = Dust.NewDust(position, width, height, 43, 0f, 0f, 254, new Color(255, 255, 0), 0.5f);
				Dust obj = Main.dust[num2];
				obj.velocity *= 0f;
			}
		}
		position -= netOffset;
	}

	protected virtual void CopyToDummy()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		_dummy.type = npcType;
		_dummy.Size = Size;
		_dummy.frame = frame;
		_dummy.frameCounter = frameCounter;
		_dummy.position = Center + new Vector2(0f, 8f) - new Vector2(Size.X / 2f, Size.Y);
		_dummy.velocity = velocity;
		_dummy.direction = direction;
		_dummy.spriteDirection = spriteDirection;
		_dummy.scale = scale;
		_dummy.rotation = 0f;
		_dummy.alpha = 0;
		_dummy.wet = false;
		Array.Clear(_dummy.ai, 0, _dummy.ai.Length);
		Array.Clear(_dummy.localAI, 0, _dummy.localAI.Length);
	}

	protected void CopyFromDummy()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		frame = _dummy.frame;
		frameCounter = _dummy.frameCounter;
		spriteDirection = _dummy.spriteDirection;
	}

	public override void Draw()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadNPC(npcType);
		if (frame.Width == 0 || frame.Height == 0)
		{
			frame = new Rectangle(0, 0, TextureAssets.Npc[npcType].Width(), TextureAssets.Npc[npcType].Height() / Main.npcFrameCount[npcType]);
		}
		CopyToDummy();
		NPC dummy = _dummy;
		dummy.position += netOffset + GetDrawOffset();
		Main.instance.DrawNPCDirect(Main.spriteBatch, _dummy, behindTiles: true, Main.screenPosition);
		if (ShouldDrawBubble())
		{
			DrawBubble();
		}
	}

	private bool ShouldDrawBubble()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		if (!drawBubble)
		{
			return false;
		}
		Point val = Center.ToTileCoordinates();
		Tile tileSafely = Framing.GetTileSafely(val.X, val.Y);
		if (!tileSafely.nactive() || !Main.tileSolid[tileSafely.type] || TileID.Sets.Platforms[tileSafely.type])
		{
			return TileRequiresBubble(tileSafely);
		}
		bool flag = tileSafely.halfBrick();
		byte b = tileSafely.slope();
		if (b == 0 && !flag)
		{
			return false;
		}
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		if (flag)
		{
			flag2 = (flag4 = (flag5 = true));
		}
		else
		{
			switch (b)
			{
			case 1:
				flag2 = (flag5 = true);
				break;
			case 2:
				flag2 = (flag4 = true);
				break;
			case 3:
				flag3 = (flag5 = true);
				break;
			case 4:
				flag3 = (flag4 = true);
				break;
			}
		}
		if (flag2 && TileRequiresBubble(Framing.GetTileSafely(val.X, val.Y - 1)))
		{
			return true;
		}
		if (flag3 && TileRequiresBubble(Framing.GetTileSafely(val.X, val.Y + 1)))
		{
			return true;
		}
		if (flag4 && TileRequiresBubble(Framing.GetTileSafely(val.X - 1, val.Y)))
		{
			return true;
		}
		if (flag5 && TileRequiresBubble(Framing.GetTileSafely(val.X + 1, val.Y)))
		{
			return true;
		}
		return false;
	}

	private bool TileRequiresBubble(Tile tile)
	{
		byte liquid = tile.liquid;
		if (!isAquatic || liquid >= byte.MaxValue)
		{
			if (!isAquatic)
			{
				return liquid > 0;
			}
			return false;
		}
		return true;
	}

	public virtual Vector2 GetDrawOffset()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Zero;
	}

	protected void DrawBubble()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadGore(413);
		Texture2D value = TextureAssets.Gore[413].Value;
		Rectangle val = value.Frame();
		Vector2 val2 = val.Size() / 2f;
		Vector2 center = _dummy.Center;
		Point tileCoords = center.ToTileCoordinates();
		int num = 4;
		Vector2 val3 = frame.Size() * _dummy.scale / (val.Size() - new Vector2((float)(num * 2)));
		float num2 = Math.Max(1f, Math.Max(val3.X, val3.Y));
		Main.spriteBatch.Draw(value, center - Main.screenPosition, (Rectangle?)val, Lighting.GetColor(tileCoords), 0f, val2, num2, (SpriteEffects)0, 0f);
	}
}
