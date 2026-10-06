using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities;

public class FairyLeashedCritter : FlyerLeashedCritter
{
	public new static FairyLeashedCritter Prototype = new FairyLeashedCritter();

	public FairyLeashedCritter()
	{
		minWaitTime = 30;
		maxWaitTime = 90;
		maxFlySpeed = 1.1f;
		acceleration = 0.05f;
		rotationScalar = 0.25f;
		brakeDuration = 30;
	}

	protected override void VisualEffects()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		base.VisualEffects();
		Color val = Color.HotPink;
		Color val2 = Color.LightPink;
		int num = 4;
		if (npcType == 584)
		{
			val = Color.LimeGreen;
			val2 = Color.LightSeaGreen;
		}
		if (npcType == 585)
		{
			val = Color.RoyalBlue;
			val2 = Color.LightBlue;
		}
		if ((int)Main.timeForVisualEffects % 4 == 0 && Main.rand.Next(4) != 0)
		{
			position += netOffset;
			Dust dust = Dust.NewDustDirect(Center - new Vector2(4f) + Main.rand.NextVector2Circular(2f, 2f), num, num, 278, 0f, 0f, 200, Color.Lerp(val, val2, Main.rand.NextFloat()), 0.65f);
			dust.velocity *= 0f;
			dust.velocity += velocity * 0.3f;
			dust.noGravity = true;
			dust.noLight = true;
			position -= netOffset;
		}
		Lighting.AddLight(Center, val.ToVector3() * 0.7f);
	}
}
