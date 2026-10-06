using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Cinematics;

public class DSTFilm : Film
{
	private NPC _deerclops;

	private Projectile _chester;

	private Vector2 _startPoint;

	private Item _oldItem;

	public DSTFilm()
	{
		BuildSequence();
	}

	public override void OnBegin()
	{
		PrepareScene();
		Main.hideUI = true;
		base.OnBegin();
	}

	public override void OnEnd()
	{
		ClearScene();
		Main.hideUI = false;
		base.OnEnd();
	}

	private void BuildSequence()
	{
		AppendKeyFrames(EquipDSTShaderItem);
		AppendEmptySequence(60);
		AppendKeyFrames(CreateDeerclops, CreateChester, ControlPlayer);
		AppendEmptySequence(60);
		AppendEmptySequence(187);
		AppendKeyFrames(StopBeforeCliff);
		AppendEmptySequence(20);
		AppendKeyFrames(TurnPlayerToTheLeft);
		AppendEmptySequence(20);
		AppendKeyFrames(DeerclopsAttack);
		AppendEmptySequence(60);
		AppendKeyFrames(RemoveDSTShaderItem);
	}

	private void PrepareScene()
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		Main.dayTime = true;
		Main.time = 13500.0;
		Main.time = 43638.0;
		Main.windSpeedCurrent = (Main.windSpeedTarget = 0.36799997f);
		Main.windCounter = 2011;
		Main.cloudAlpha = 0f;
		Main.raining = true;
		Main.rainTime = 3600;
		Main.maxRaining = (Main.oldMaxRaining = (Main.cloudAlpha = 0.9f));
		Main.raining = true;
		Main.maxRaining = (Main.oldMaxRaining = (Main.cloudAlpha = 0.6f));
		Main.raining = true;
		Main.maxRaining = (Main.oldMaxRaining = (Main.cloudAlpha = 0.6f));
		_startPoint = new Point(4050, 488).ToWorldCoordinates();
		_startPoint -= new Vector2(1280f, 0f);
	}

	private void ClearScene()
	{
		if (_deerclops != null)
		{
			_deerclops.active = false;
		}
		if (_chester != null)
		{
			_chester.active = false;
		}
		Main.LocalPlayer.isControlledByFilm = false;
	}

	private void EquipDSTShaderItem(FrameEventData evt)
	{
		_oldItem = Main.LocalPlayer.armor[3];
		Item item = new Item();
		item.SetDefaults(5113);
		Main.LocalPlayer.armor[3] = item;
	}

	private void RemoveDSTShaderItem(FrameEventData evt)
	{
		Main.LocalPlayer.armor[3] = _oldItem;
	}

	private void CreateDeerclops(FrameEventData evt)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_deerclops = PlaceNPCOnGround(668, _startPoint);
		_deerclops.immortal = true;
		_deerclops.dontTakeDamage = true;
		_deerclops.takenDamageMultiplier = 0f;
		_deerclops.immune[255] = 100000;
		_deerclops.immune[Main.myPlayer] = 100000;
		_deerclops.ai[0] = -1f;
		_deerclops.velocity.Y = 4f;
		_deerclops.velocity.X = 6f;
		_deerclops.position.X -= 24f;
		_deerclops.direction = (_deerclops.spriteDirection = 1);
	}

	private NPC PlaceNPCOnGround(int type, Vector2 position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		FindFloorAt(position, out var x, out var y);
		if (type == 668)
		{
			y -= 240;
		}
		int start = 100;
		int num = NPC.NewNPC(new EntitySource_Film(), x, y, type, start);
		return Main.npc[num];
	}

	private void CreateChester(FrameEventData evt)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		FindFloorAt(_startPoint + new Vector2(110f, 0f), out var x, out var y);
		y -= 240;
		int num = Projectile.NewProjectile(null, x, y, 0f, 0f, 960, 0, 0f, Main.myPlayer, -1f);
		_chester = Main.projectile[num];
		_chester.velocity.Y = 4f;
		_chester.velocity.X = 6f;
	}

	private void ControlPlayer(FrameEventData evt)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Main.LocalPlayer;
		localPlayer.isControlledByFilm = true;
		localPlayer.controlRight = true;
		FindFloorAt(_startPoint + new Vector2(150f, 0f), out var x, out var y);
		localPlayer.BottomLeft = new Vector2((float)x, (float)y);
		localPlayer.velocity.X = 6f;
	}

	private void StopBeforeCliff(FrameEventData evt)
	{
		Main.LocalPlayer.controlRight = false;
		_chester.ai[0] = -2f;
	}

	private void TurnPlayerToTheLeft(FrameEventData evt)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Main.LocalPlayer.ChangeDir(-1);
		_chester.velocity = new Vector2(-0.1f, 0f);
		_chester.spriteDirection = (_chester.direction = -1);
		_deerclops.ai[0] = 1f;
		_deerclops.ai[1] = 0f;
		_deerclops.TargetClosest();
	}

	private void DeerclopsAttack(FrameEventData evt)
	{
		Main.LocalPlayer.controlJump = true;
		_chester.velocity.Y = -11.4f;
		_deerclops.ai[0] = 1f;
		_deerclops.ai[1] = 0f;
		_deerclops.TargetClosest();
	}

	private static void FindFloorAt(Vector2 position, out int x, out int y)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		x = (int)position.X;
		y = (int)position.Y;
		int i = x / 16;
		int j;
		for (j = y / 16; !WorldGen.SolidTile(i, j); j++)
		{
		}
		y = j * 16;
	}
}
