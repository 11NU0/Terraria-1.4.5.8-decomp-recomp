namespace Terraria;

public struct NPCSpawnParams
{
	public float? sizeScaleOverride;

	public int? playerCountForMultiplayerDifficultyOverride;

	public float? difficultyOverride;

	public NPCSpawnParams WithScale(float scaleOverride)
	{
		return new NPCSpawnParams
		{
			sizeScaleOverride = scaleOverride,
			playerCountForMultiplayerDifficultyOverride = playerCountForMultiplayerDifficultyOverride,
			difficultyOverride = difficultyOverride
		};
	}
}
