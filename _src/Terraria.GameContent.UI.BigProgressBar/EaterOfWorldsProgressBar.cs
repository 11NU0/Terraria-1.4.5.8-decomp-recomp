using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar;

public class EaterOfWorldsProgressBar : IBigProgressBar
{
	private BigProgressBarCache _cache;

	private NPC _segmentForReference;

	public EaterOfWorldsProgressBar()
	{
		_segmentForReference = new NPC();
	}

	public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
	{
		if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > Main.maxNPCs)
		{
			return false;
		}
		NPC nPC = Main.npc[info.npcIndexToAimAt];
		if (!nPC.active && !TryFindingAnotherEOWPiece(ref info))
		{
			return false;
		}
		int num = 2;
		int num2 = NPC.GetEaterOfWorldsSegmentsCount() + num;
		_segmentForReference.SetDefaults(14, nPC.GetMatchingSpawnParams());
		int num3 = 0;
		int num4 = _segmentForReference.lifeMax * num2;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC2 = Main.npc[i];
			if (nPC2.active && nPC2.type >= 13 && nPC2.type <= 15)
			{
				num3 += nPC2.life;
			}
		}
		int num5 = num3;
		int num6 = num4;
		_cache.SetLife(num5, num6);
		return true;
	}

	public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		int num = NPCID.Sets.BossHeadTextures[13];
		Texture2D value = TextureAssets.NpcHeadBoss[num].Value;
		Rectangle barIconFrame = value.Frame();
		BigProgressBarHelper.DrawFancyBar(spriteBatch, _cache.LifeCurrent, _cache.LifeMax, value, barIconFrame);
	}

	private bool TryFindingAnotherEOWPiece(ref BigProgressBarInfo info)
	{
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (nPC.active && nPC.type >= 13 && nPC.type <= 15)
			{
				info.npcIndexToAimAt = i;
				return true;
			}
		}
		return false;
	}
}
