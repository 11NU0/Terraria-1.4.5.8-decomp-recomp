using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders;

public class MoonLordScreenShaderData : ScreenShaderData
{
	private bool _aimAtPlayer;

	public static float LatestFilterStrength = 1f;

	private Vector2 _targetPositionInWorld;

	public MoonLordScreenShaderData(string passName, bool aimAtPlayer)
		: base(passName)
	{
		_aimAtPlayer = aimAtPlayer;
	}

	private void UpdateMoonLordIndex()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		if (_aimAtPlayer)
		{
			return;
		}
		Vector2 val = Vector2.Zero;
		Vector2 val2 = Vector2.Zero;
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < Main.npc.Length; i++)
		{
			if (!Main.npc[i].active)
			{
				continue;
			}
			if (Main.npc[i].type == 398)
			{
				val = Main.npc[i].Center;
				if (Main.npc[i].ai[0] == -2f)
				{
					num2 = Utils.Remap(Main.npc[i].ai[1], 55f, 60f, 1f, 0f);
				}
			}
			if (Main.npc[i].type != 400 || Main.npc[i].ai[0] != 4f)
			{
				continue;
			}
			float num3 = Main.npc[i].ai[1];
			float num4 = 0f;
			int j = 0;
			int num5 = 0;
			int[,] moonLordAttacksArray = NPC.MoonLordAttacksArray2;
			for (; j < 10; j++)
			{
				num4 = moonLordAttacksArray[1, j];
				if (!(num4 + (float)num5 <= num3))
				{
					break;
				}
				num5 += (int)num4;
			}
			int num6 = (int)num3 - num5;
			float num7 = Utils.Remap(num6, 0f, 180f, 0f, 1f) * Utils.Remap(num6, 340f, 360f, 1f, 0f);
			if (num7 > num)
			{
				val2 = Main.npc[i].Center;
				num = num7;
			}
		}
		_targetPositionInWorld = val;
		if (num > 0f)
		{
			_targetPositionInWorld = Vector2.Lerp(_targetPositionInWorld, val2, num);
		}
		if (num2 > 0f)
		{
			_targetPositionInWorld = Vector2.Lerp(_targetPositionInWorld, val, num2);
		}
	}

	public override void Apply()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		LatestFilterStrength = 0f;
		UpdateMoonLordIndex();
		if (_aimAtPlayer)
		{
			LatestFilterStrength = 1f;
			UseTargetPosition(Main.SceneMetrics.Center);
		}
		else
		{
			LatestFilterStrength = Utils.Remap(Vector2.Distance(_targetPositionInWorld, Main.SceneMetrics.Center), 4000f, 8000f, 1f, 0f);
			UseTargetPosition(_targetPositionInWorld);
		}
		base.Apply();
	}
}
