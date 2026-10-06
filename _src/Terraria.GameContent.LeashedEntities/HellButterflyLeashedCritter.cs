namespace Terraria.GameContent.LeashedEntities;

public class HellButterflyLeashedCritter : FlyLeashedCritter
{
	public new static HellButterflyLeashedCritter Prototype = new HellButterflyLeashedCritter();

	protected override void VisualEffects()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		base.VisualEffects();
		position += netOffset;
		Lighting.AddLight((int)Center.X / 16, (int)Center.Y / 16, 0.6f, 0.3f, 0.1f);
		if (Main.rand.Next(60) == 0)
		{
			int num = Dust.NewDust(position, width, height, 6, 0f, 0f, 254);
			Dust obj = Main.dust[num];
			obj.velocity *= 0f;
		}
		position -= netOffset;
	}
}
