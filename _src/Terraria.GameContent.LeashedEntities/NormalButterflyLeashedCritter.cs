using System.IO;

namespace Terraria.GameContent.LeashedEntities;

public class NormalButterflyLeashedCritter : FlyLeashedCritter
{
	public new static NormalButterflyLeashedCritter Prototype = new NormalButterflyLeashedCritter();

	protected byte variant;

	protected override void SetDefaults(Item sample)
	{
		base.SetDefaults(sample);
		variant = (byte)sample.placeStyle;
	}

	protected override void CopyToDummy()
	{
		base.CopyToDummy();
		LeashedCritter._dummy.ai[2] = (int)variant;
	}

	public override void NetSend(BinaryWriter writer, bool full)
	{
		base.NetSend(writer, full);
		if (full)
		{
			writer.Write(variant);
		}
	}

	public override void NetReceive(BinaryReader reader, bool full)
	{
		base.NetReceive(reader, full);
		if (full)
		{
			variant = reader.ReadByte();
		}
	}
}
