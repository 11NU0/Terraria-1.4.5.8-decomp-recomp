namespace Terraria.GameContent.LeashedEntities;

public abstract class FlyLeashedCritter : FlyerLeashedCritter
{
	protected override void SetDefaults(Item sample)
	{
		base.SetDefaults(sample);
		scale = (float)Main.rand.Next(75, 111) * 0.01f;
	}
}
