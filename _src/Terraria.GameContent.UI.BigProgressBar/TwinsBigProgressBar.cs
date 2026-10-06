using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar;

public class TwinsBigProgressBar : IBigProgressBar
{
	private BigProgressBarCache _cache;

	private int _headIndex;

	public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
	{
		if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > Main.maxNPCs)
		{
			return false;
		}
		NPC nPC = Main.npc[info.npcIndexToAimAt];
		if (!nPC.active)
		{
			return false;
		}
		int num = ((nPC.type == 126) ? 125 : 126);
		int num2 = nPC.lifeMax;
		int num3 = nPC.life;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC2 = Main.npc[i];
			if (nPC2.active && nPC2.type == num)
			{
				num2 += nPC2.lifeMax;
				num3 += nPC2.life;
				break;
			}
		}
		_cache.SetLife(num3, num2);
		_headIndex = nPC.GetBossHeadTextureIndex();
		return true;
	}

	public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value = TextureAssets.NpcHeadBoss[_headIndex].Value;
		Rectangle barIconFrame = value.Frame();
		BigProgressBarHelper.DrawFancyBar(spriteBatch, _cache.LifeCurrent, _cache.LifeMax, value, barIconFrame);
	}
}
