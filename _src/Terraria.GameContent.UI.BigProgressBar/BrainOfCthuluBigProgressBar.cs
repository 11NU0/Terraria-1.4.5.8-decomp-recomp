using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar;

public class BrainOfCthuluBigProgressBar : IBigProgressBar
{
	private BigProgressBarCache _cache;

	private NPC _creeperForReference;

	public BrainOfCthuluBigProgressBar()
	{
		_creeperForReference = new NPC();
	}

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
		int brainOfCthuluCreepersCount = NPC.GetBrainOfCthuluCreepersCount();
		_creeperForReference.SetDefaults(267, nPC.GetMatchingSpawnParams());
		int num = _creeperForReference.lifeMax * brainOfCthuluCreepersCount;
		float num2 = 0f;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC2 = Main.npc[i];
			if (nPC2.active && nPC2.type == _creeperForReference.type)
			{
				num2 += (float)nPC2.life;
			}
		}
		float current = (float)nPC.life + num2;
		int num3 = nPC.lifeMax + num;
		_cache.SetLife(current, num3);
		return true;
	}

	public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		int num = NPCID.Sets.BossHeadTextures[266];
		Texture2D value = TextureAssets.NpcHeadBoss[num].Value;
		Rectangle barIconFrame = value.Frame();
		BigProgressBarHelper.DrawFancyBar(spriteBatch, _cache.LifeCurrent, _cache.LifeMax, value, barIconFrame);
	}
}
