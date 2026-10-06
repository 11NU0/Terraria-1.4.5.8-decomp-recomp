namespace Terraria.GameContent.LeashedEntities;

public class FireflyLeashedCritter : FlyLeashedCritter
{
	public new static FireflyLeashedCritter Prototype = new FireflyLeashedCritter();

	private bool lightOn;

	private int timer;

	protected override void CopyToDummy()
	{
		base.CopyToDummy();
		LeashedCritter._dummy.localAI[2] = (lightOn ? 1 : 0);
	}

	protected override void VisualEffects()
	{
		base.VisualEffects();
		UpdateTimer();
		if (lightOn && timer > 3)
		{
			AddLight();
		}
	}

	private void AddLight()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		int i = (int)Center.X / 16;
		int j = (int)Center.Y / 16;
		float num = LeashedCritter._dummy.scale;
		switch (npcType)
		{
		case 355:
			Lighting.AddLight(i, j, 0.109500006f * num, 0.15f * num, 0.0615f * num);
			break;
		case 358:
			Lighting.AddLight(i, j, 0.10124999f * num, 0.21374999f * num, 0.225f * num);
			break;
		case 654:
			Lighting.AddLight(i, j, 0.225f * num, 0.105000004f * num, 0.060000002f * num);
			break;
		}
	}

	private void UpdateTimer()
	{
		if (--timer <= 0)
		{
			timer = 0;
			if (lightOn || !Main.dayTime || !((double)(position.Y / 16f) < Main.worldSurface + 10.0))
			{
				lightOn = !lightOn;
				timer = (lightOn ? Main.rand.Next(10, 30) : Main.rand.Next(30, 180));
			}
		}
	}
}
