using System.IO;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities;

public class ShimmerFlyLeashedCritter : FlyLeashedCritter
{
	public new static ShimmerFlyLeashedCritter Prototype = new ShimmerFlyLeashedCritter();

	private byte oldPositionsLength;

	private Vector2[] oldPositions;

	protected override void SetDefaults(Item sample)
	{
		base.SetDefaults(sample);
		if (Main.netMode == 0)
		{
			oldPositions = LeashedCritter._dummy.oldPos;
		}
		oldPositionsLength = (byte)LeashedCritter._dummy.oldPos.Length;
	}

	public override void NetSend(BinaryWriter writer, bool full)
	{
		base.NetSend(writer, full);
		if (full)
		{
			writer.Write(oldPositionsLength);
		}
	}

	public override void NetReceive(BinaryReader reader, bool full)
	{
		base.NetReceive(reader, full);
		if (full)
		{
			oldPositionsLength = reader.ReadByte();
			oldPositions = new Vector2[oldPositionsLength];
		}
	}

	protected override void VisualEffects()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		base.VisualEffects();
		if (oldPositions != null)
		{
			for (int num = oldPositions.Length - 1; num > 0; num--)
			{
				oldPositions[num] = oldPositions[num - 1];
			}
			oldPositions[0] = position + netOffset;
		}
	}

	public override void Draw()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		Vector2[] oldPos = LeashedCritter._dummy.oldPos;
		LeashedCritter._dummy.oldPos = oldPositions;
		Main.instance.LoadNPC(npcType);
		int num = 4;
		frame = new Rectangle(frame.X, frame.Y, TextureAssets.Npc[npcType].Width() / num, TextureAssets.Npc[npcType].Height() / Main.npcFrameCount[npcType]);
		base.Draw();
		LeashedCritter._dummy.oldPos = oldPos;
	}
}
