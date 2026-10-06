namespace Terraria.DataStructures;

public interface IRoomCheckFeedback_Scoring
{
	void BeginScoring();

	void ReportScore(int x, int y, int score);

	void SetAsHighScore(int x, int y, int score);

	void EndScoring();
}
