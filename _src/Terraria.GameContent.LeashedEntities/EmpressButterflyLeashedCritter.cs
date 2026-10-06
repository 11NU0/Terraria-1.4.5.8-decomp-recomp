using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities;

public class EmpressButterflyLeashedCritter : FlyLeashedCritter
{
	public new static EmpressButterflyLeashedCritter Prototype = new EmpressButterflyLeashedCritter();

	private float fadeAmount;

	private const int FadeAwayCap = 50;

	private float Opacity => Utils.GetLerpValue(60f, 25f, fadeAmount, clamped: true);

	protected override void CopyToDummy()
	{
		base.CopyToDummy();
		LeashedCritter._dummy.ai[2] = fadeAmount;
		LeashedCritter._dummy.Opacity = Opacity;
	}

	protected override void VisualEffects()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		base.VisualEffects();
		Color val = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.33f % 1f, 1f, 0.5f);
		Vector3 val2 = val.ToVector3() * 0.3f;
		val2 += Vector3.One * 0.1f;
		Lighting.AddLight(Center, val2);
		bool value = Main.LocalPlayer.Center.Distance(Center) > 300f;
		fadeAmount = MathHelper.Clamp(fadeAmount + (float)value.ToDirectionInt(), 0f, 50f);
		if (!(fadeAmount > 0f))
		{
			return;
		}
		float opacity = Opacity;
		int num = 1;
		for (int i = 0; i < num; i++)
		{
			if (Main.rand.Next(5) == 0)
			{
				float num2 = MathHelper.Lerp(0.9f, 0.6f, opacity);
				Color newColor = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.3f % 1f, 1f, 0.5f) * 0.5f;
				int num3 = Dust.NewDust(position, width, height, 267, 0f, 0f, 0, newColor);
				Main.dust[num3].position = Center + Main.rand.NextVector2Circular(width, height);
				Dust obj = Main.dust[num3];
				obj.velocity *= Main.rand.NextFloat() * 0.8f;
				Dust obj2 = Main.dust[num3];
				obj2.velocity += velocity * 0.6f;
				Main.dust[num3].noGravity = true;
				Main.dust[num3].fadeIn = 0.6f + Main.rand.NextFloat() * 0.7f * num2;
				Main.dust[num3].scale = 0.35f;
				if (num3 != 6000)
				{
					Dust dust = Dust.CloneDust(num3);
					dust.scale /= 2f;
					dust.fadeIn *= 0.85f;
					dust.color = new Color(255, 255, 255, 255) * 0.5f;
				}
			}
		}
	}
}
