namespace Terraria.DataStructures;

public class NoRoomCheckFeedback : IRoomCheckFeedback, IRoomCheckFeedback_Spread, IRoomCheckFeedback_Scoring
{
	public static NoRoomCheckFeedback WithText = new NoRoomCheckFeedback(displayText: true);

	public static NoRoomCheckFeedback WithoutText = new NoRoomCheckFeedback(displayText: false);

	public bool StopOnFail => true;

	public bool DisplayText { get; private set; }

	public NoRoomCheckFeedback(bool displayText)
	{
		DisplayText = displayText;
	}

	public void BeginSpread(int x, int y)
	{
	}

	public void StartedInASolidTile(int x, int y)
	{
	}

	public void TooCloseToWorldEdge(int x, int y, int iteration)
	{
	}

	public void AnyBlockScannedHere(int x, int y, int iteration)
	{
	}

	public void RoomTooBig(int x, int y, int iteration)
	{
	}

	public void BlockingWall(int x, int y, int iteration)
	{
	}

	public void BlockingOpenGate(int x, int y, int iteration)
	{
	}

	public void Stinkbug(int x, int y, int iteration)
	{
	}

	public void EchoStinkbug(int x, int y, int iteration)
	{
	}

	public void MissingAWall(int x, int y, int iteration)
	{
	}

	public void UnsafeWall(int x, int y, int iteration)
	{
	}

	public void EndSpread()
	{
	}

	public void BeginScoring()
	{
	}

	public void ReportScore(int x, int y, int score)
	{
	}

	public void SetAsHighScore(int x, int y, int score)
	{
	}

	public void EndScoring()
	{
	}
}
