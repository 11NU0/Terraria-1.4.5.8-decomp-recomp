using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LootSimulation;

public class SimulatorInfo
{
	public Player player;

	private double _originalDayTimeCounter;

	private bool _originalDayTimeFlag;

	private Vector2 _originalPlayerPosition;

	public bool runningExpertMode;

	public LootSimulationItemCounter itemCounter;

	public NPC npcVictim;

	public SimulatorInfo()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		player = new Player();
		_originalDayTimeCounter = Main.time;
		_originalDayTimeFlag = Main.dayTime;
		_originalPlayerPosition = player.position;
		runningExpertMode = false;
	}

	public void ReturnToOriginalDaytime()
	{
		Main.dayTime = _originalDayTimeFlag;
		Main.time = _originalDayTimeCounter;
	}

	public void AddItem(int itemId, int amount)
	{
		itemCounter.AddItem(itemId, amount, runningExpertMode);
	}

	public void ReturnToOriginalPlayerPosition()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		player.position = _originalPlayerPosition;
	}
}
