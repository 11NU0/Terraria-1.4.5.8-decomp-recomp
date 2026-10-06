namespace Terraria.GameContent.LeashedEntities;

public class SnailLeashedCritter : CrawlerLeashedCritter
{
	public new static SnailLeashedCritter Prototype = new SnailLeashedCritter();

	protected override void SetDefaults(Item sample)
	{
		base.SetDefaults(sample);
		if (npcType == 359)
		{
			scale = (float)Main.rand.Next(80, 111) * 0.01f;
		}
	}

	protected override void VisualEffects()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		base.VisualEffects();
		switch (npcType)
		{
		case 360:
			Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.1f, 0.2f, 0.7f);
			break;
		case 655:
			Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.6f, 0.3f, 0.1f);
			break;
		}
	}
}
