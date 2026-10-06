namespace Terraria.DataStructures;

public interface IRoomCheckFeedback_Spread
{
	bool StopOnFail { get; }

	bool DisplayText { get; }

	void BeginSpread(int x, int y);

	void StartedInASolidTile(int x, int y);

	void TooCloseToWorldEdge(int x, int y, int iteration);

	void AnyBlockScannedHere(int x, int y, int iteration);

	void RoomTooBig(int x, int y, int iteration);

	void BlockingWall(int x, int y, int iteration);

	void BlockingOpenGate(int x, int y, int iteration);

	void Stinkbug(int x, int y, int iteration);

	void EchoStinkbug(int x, int y, int iteration);

	void MissingAWall(int x, int y, int iteration);

	void UnsafeWall(int x, int y, int iteration);

	void EndSpread();
}
