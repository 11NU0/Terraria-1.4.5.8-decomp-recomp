using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Testing;

namespace Terraria.GameInput;

public class LockOnHelper
{
	public enum LockOnMode
	{
		FocusTarget,
		TargetClosest,
		ThreeDS
	}

	private const float LOCKON_RANGE = 2000f;

	private const int LOCKON_HOLD_LIFETIME = 40;

	public static LockOnMode UseMode = LockOnMode.ThreeDS;

	private static bool _enabled;

	private static bool _canLockOn;

	private static List<int> _targets = new List<int>();

	private static int _pickedTarget;

	private static int _lifeTimeCounter;

	private static int _lifeTimeArrowDisplay;

	private static int _threeDSTarget = -1;

	private static int _targetClosestTarget = -1;

	public static bool ForceUsability = false;

	private static float[,] _drawProgress = new float[Main.maxNPCs, 2];

	public static NPC AimedTarget
	{
		get
		{
			if (_pickedTarget == -1 || _targets.Count < 1)
			{
				return null;
			}
			return Main.npc[_targets[_pickedTarget]];
		}
	}

	public static Vector2 PredictedPosition
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			NPC aimedTarget = AimedTarget;
			if (aimedTarget == null)
			{
				return Vector2.Zero;
			}
			Vector2 val = aimedTarget.Center;
			if (NPC.GetNPCLocation(_targets[_pickedTarget], seekHead: true, averageDirection: false, out var index, out var pos))
			{
				val = pos;
				val += Main.npc[index].Distance(Main.player[Main.myPlayer].Center) / 2000f * Main.npc[index].velocity * 45f;
			}
			Player player = Main.player[Main.myPlayer];
			int num = ItemID.Sets.LockOnAimAbove[player.inventory[player.selectedItem].type];
			while (num > 0 && val.Y > 100f)
			{
				Point val2 = val.ToTileCoordinates();
				val2.Y -= 4;
				if (!WorldGen.InWorld(val2.X, val2.Y, 10) || WorldGen.SolidTile(val2.X, val2.Y))
				{
					break;
				}
				val.Y -= 16f;
				num--;
			}
			float? num2 = ItemID.Sets.LockOnAimCompensation[player.inventory[player.selectedItem].type];
			if (num2.HasValue)
			{
				val.Y -= aimedTarget.height / 2;
				Vector2 v = val - player.Center;
				Vector2 val3 = v.SafeNormalize(Vector2.Zero);
				val3.Y--;
				float num3 = v.Length();
				num3 = (float)Math.Pow(num3 / 700f, 2.0) * 700f;
				val.Y += val3.Y * num3 * num2.Value * 1f;
				val.X += (0f - val3.X) * num3 * num2.Value * 1f;
			}
			return val;
		}
	}

	public static bool Enabled => _enabled;

	public static void CycleUseModes()
	{
		switch (UseMode)
		{
		case LockOnMode.FocusTarget:
			UseMode = LockOnMode.TargetClosest;
			break;
		case LockOnMode.TargetClosest:
			UseMode = LockOnMode.ThreeDS;
			break;
		case LockOnMode.ThreeDS:
			UseMode = LockOnMode.TargetClosest;
			break;
		}
	}

	public static void Update()
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		_canLockOn = false;
		if (!CanUseLockonSystem())
		{
			SetActive(on: false);
			return;
		}
		if (--_lifeTimeArrowDisplay < 0)
		{
			_lifeTimeArrowDisplay = 0;
		}
		FindMostViableTarget(LockOnMode.ThreeDS, ref _threeDSTarget);
		FindMostViableTarget(LockOnMode.TargetClosest, ref _targetClosestTarget);
		if (PlayerInput.Triggers.JustPressed.LockOn && !PlayerInput.WritingText)
		{
			_lifeTimeCounter = 40;
			_lifeTimeArrowDisplay = 30;
			HandlePressing();
		}
		if (!_enabled)
		{
			return;
		}
		if (UseMode == LockOnMode.FocusTarget && PlayerInput.Triggers.Current.LockOn)
		{
			if (_lifeTimeCounter <= 0)
			{
				SetActive(on: false);
				return;
			}
			_lifeTimeCounter--;
		}
		NPC aimedTarget = AimedTarget;
		if (!ValidTarget(aimedTarget))
		{
			SetActive(on: false);
		}
		if (UseMode == LockOnMode.TargetClosest)
		{
			SetActive(on: false);
			SetActive(CanEnable());
		}
		if (_enabled)
		{
			Player player = Main.player[Main.myPlayer];
			Vector2 predictedPosition = PredictedPosition;
			bool flag = false;
			if (ShouldLockOn(player) && (ItemID.Sets.LockOnIgnoresCollision[player.inventory[player.selectedItem].type] || Collision.CanHit(player.Center, 0, 0, predictedPosition, 0, 0) || Collision.CanHitLine(player.Center, 0, 0, predictedPosition, 0, 0) || Collision.CanHit(player.Center, 0, 0, aimedTarget.Center, 0, 0) || Collision.CanHitLine(player.Center, 0, 0, aimedTarget.Center, 0, 0)))
			{
				flag = true;
			}
			if (flag)
			{
				_canLockOn = true;
			}
		}
	}

	public static bool CanUseLockonSystem()
	{
		if (!ForceUsability)
		{
			return PlayerInput.UsingGamepad;
		}
		return true;
	}

	public static void SetUP()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (_canLockOn)
		{
			_ = AimedTarget;
			SetLockPosition(Main.ReverseGravitySupport(PredictedPosition - Main.screenPosition));
		}
	}

	public static void SetDOWN()
	{
		if (_canLockOn)
		{
			ResetLockPosition();
		}
	}

	private static bool ShouldLockOn(Player p)
	{
		if (ItemID.Sets.NeverLocksOn[p.inventory[p.selectedItem].type])
		{
			return false;
		}
		return true;
	}

	public static void Toggle(bool forceOff = false)
	{
		_lifeTimeCounter = 40;
		_lifeTimeArrowDisplay = 30;
		HandlePressing();
		if (forceOff)
		{
			_enabled = false;
		}
	}

	private static void FindMostViableTarget(LockOnMode context, ref int targetVar)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		targetVar = -1;
		if (UseMode == context && CanUseLockonSystem())
		{
			List<int> t = new List<int>();
			int t2 = -1;
			Utils.Swap(ref t, ref _targets);
			Utils.Swap(ref t2, ref _pickedTarget);
			RefreshTargets(Main.MouseWorld, 2000f);
			GetClosestTarget(Main.MouseWorld);
			Utils.Swap(ref t, ref _targets);
			Utils.Swap(ref t2, ref _pickedTarget);
			if (t2 >= 0)
			{
				targetVar = t[t2];
			}
			t.Clear();
		}
	}

	private static void HandlePressing()
	{
		if (UseMode == LockOnMode.TargetClosest)
		{
			SetActive(!_enabled);
		}
		else if (UseMode == LockOnMode.ThreeDS)
		{
			if (!_enabled)
			{
				SetActive(on: true);
			}
			else
			{
				CycleTargetThreeDS();
			}
		}
		else if (!_enabled)
		{
			SetActive(on: true);
		}
		else
		{
			CycleTargetFocus();
		}
	}

	private static void CycleTargetFocus()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		int num = _targets[_pickedTarget];
		RefreshTargets(Main.MouseWorld, 2000f);
		if (_targets.Count < 1 || (_targets.Count == 1 && num == _targets[0]))
		{
			SetActive(on: false);
			return;
		}
		_pickedTarget = 0;
		for (int i = 0; i < _targets.Count; i++)
		{
			if (_targets[i] > num)
			{
				_pickedTarget = i;
				break;
			}
		}
	}

	private static void CycleTargetThreeDS()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		int num = _targets[_pickedTarget];
		RefreshTargets(Main.MouseWorld, 2000f);
		GetClosestTarget(Main.MouseWorld);
		if (_targets.Count < 1 || (_targets.Count == 1 && num == _targets[0]) || num == _targets[_pickedTarget])
		{
			SetActive(on: false);
		}
	}

	private static bool CanEnable()
	{
		if (Main.player[Main.myPlayer].dead)
		{
			return false;
		}
		return true;
	}

	private static void SetActive(bool on)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (on)
		{
			if (CanEnable())
			{
				RefreshTargets(Main.MouseWorld, 2000f);
				GetClosestTarget(Main.MouseWorld);
				if (_pickedTarget >= 0)
				{
					_enabled = true;
				}
			}
		}
		else
		{
			_enabled = false;
			_targets.Clear();
			_lifeTimeCounter = 0;
			_threeDSTarget = -1;
			_targetClosestTarget = -1;
		}
	}

	private static void RefreshTargets(Vector2 position, float radius)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		_targets.Clear();
		Rectangle val = Utils.CenteredRectangle(Main.player[Main.myPlayer].Center, Main.MaxWorldViewSize.ToVector2());
		_ = Main.player[Main.myPlayer].Center;
		Main.player[Main.myPlayer].DirectionTo(Main.MouseWorld);
		for (int i = 0; i < Main.npc.Length; i++)
		{
			NPC nPC = Main.npc[i];
			if (ValidTarget(nPC) && !(nPC.Distance(position) > radius) && val.Intersects(nPC.Hitbox))
			{
				Vector3 subLight = Lighting.GetSubLight(nPC.Center);
				if (!(subLight.Length() / 3f < 0.03f))
				{
					_targets.Add(i);
				}
			}
		}
	}

	private static void GetClosestTarget(Vector2 position)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		_pickedTarget = -1;
		float num = -1f;
		if (UseMode == LockOnMode.ThreeDS)
		{
			Vector2 center = Main.player[Main.myPlayer].Center;
			Vector2 val = Main.player[Main.myPlayer].DirectionTo(Main.MouseWorld);
			for (int i = 0; i < _targets.Count; i++)
			{
				int num2 = _targets[i];
				NPC obj = Main.npc[num2];
				float num3 = Vector2.Dot(obj.DirectionFrom(center), val);
				if (ValidTarget(obj) && (_pickedTarget == -1 || !(num3 <= num)))
				{
					_pickedTarget = i;
					num = num3;
				}
			}
			return;
		}
		for (int j = 0; j < _targets.Count; j++)
		{
			int num4 = _targets[j];
			NPC nPC = Main.npc[num4];
			if (ValidTarget(nPC) && (_pickedTarget == -1 || !(nPC.Distance(position) >= num)))
			{
				_pickedTarget = j;
				num = nPC.Distance(position);
			}
		}
	}

	private static bool ValidTarget(NPC n)
	{
		if (n == null || !n.active || n.dontTakeDamage || n.friendly || n.isLikeATownNPC || n.life < 1 || n.immortal)
		{
			return false;
		}
		if (n.aiStyle == 25 && n.ai[0] == 0f)
		{
			return false;
		}
		return true;
	}

	private static void SetLockPosition(Vector2 position)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		PlayerInput.LockOnCachePosition();
		Main.mouseX = (PlayerInput.MouseX = (int)position.X);
		Main.mouseY = (PlayerInput.MouseY = (int)position.Y);
	}

	private static void ResetLockPosition()
	{
		PlayerInput.LockOnUnCachePosition();
		Main.mouseX = PlayerInput.MouseX;
		Main.mouseY = PlayerInput.MouseY;
	}

	public static void Draw(SpriteBatch spriteBatch)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return;
		}
		Texture2D value = TextureAssets.LockOnCursor.Value;
		Rectangle val = new Rectangle(0, 0, value.Width, 12);
		Rectangle val2 = new Rectangle(0, 16, value.Width, 12);
		Color t = Main.OurFavoriteColor.MultiplyRGBA(new Color(0.75f, 0.75f, 0.75f, 1f));
		t.A = 220;
		Color t2 = Main.OurFavoriteColor;
		t2.A = 220;
		float num = 0.94f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f)) * 0.06f;
		t2 *= num;
		t *= num;
		Utils.Swap(ref t, ref t2);
		Color val3 = t.MultiplyRGBA(new Color(0.8f, 0.8f, 0.8f, 0.8f));
		Color val4 = t.MultiplyRGBA(new Color(0.8f, 0.8f, 0.8f, 0.8f));
		float gravDir = Main.player[Main.myPlayer].gravDir;
		float num2 = 1f;
		float num3 = 0.1f;
		float num4 = 0.8f;
		float num5 = 1f;
		float num6 = 10f;
		float num7 = 10f;
		bool flag = false;
		for (int i = 0; i < _drawProgress.GetLength(0); i++)
		{
			int num8 = 0;
			if (_pickedTarget != -1 && _targets.Count > 0 && i == _targets[_pickedTarget])
			{
				num8 = 2;
			}
			else if ((flag && _targets.Contains(i)) || (UseMode == LockOnMode.ThreeDS && _threeDSTarget == i) || (UseMode == LockOnMode.TargetClosest && _targetClosestTarget == i))
			{
				num8 = 1;
			}
			_drawProgress[i, 0] = MathHelper.Clamp(_drawProgress[i, 0] + ((num8 == 1) ? num3 : (0f - num3)), 0f, 1f);
			_drawProgress[i, 1] = MathHelper.Clamp(_drawProgress[i, 1] + ((num8 == 2) ? num3 : (0f - num3)), 0f, 1f);
			float num9 = _drawProgress[i, 0];
			if (num9 > 0f)
			{
				float num10 = 1f - num9 * num9;
				Vector2 pos = Main.npc[i].Top + new Vector2(0f, 0f - num7 - num10 * num6) * gravDir - Main.screenPosition;
				pos = Main.ReverseGravitySupport(pos, (float)Main.npc[i].height);
				spriteBatch.Draw(value, pos, (Rectangle?)val, val3 * num9, 0f, val.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num4 * (1f + num9) / 2f, (SpriteEffects)0, 0f);
				spriteBatch.Draw(value, pos, (Rectangle?)val2, val4 * num9 * num9, 0f, val2.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num4 * (1f + num9) / 2f, (SpriteEffects)0, 0f);
			}
			float num11 = _drawProgress[i, 1];
			if (num11 > 0f)
			{
				int num12 = Main.npc[i].width;
				if (Main.npc[i].height > num12)
				{
					num12 = Main.npc[i].height;
				}
				num12 += 20;
				if ((float)num12 < 70f)
				{
					num5 *= (float)num12 / 70f;
				}
				float num13 = 3f;
				Vector2 val5 = Main.npc[i].Center;
				if (_targets.Count >= 0 && _pickedTarget >= 0 && _pickedTarget < _targets.Count && i == _targets[_pickedTarget] && NPC.GetNPCLocation(i, seekHead: true, averageDirection: false, out var _, out var pos2))
				{
					val5 = pos2;
				}
				for (int j = 0; (float)j < num13; j++)
				{
					float num14 = (float)Math.PI * 2f / num13 * (float)j + Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) * 0.25f;
					Vector2 val6 = new Vector2(0f, (float)(num12 / 2)).RotatedBy(num14);
					Vector2 pos3 = val5 + val6 - Main.screenPosition;
					pos3 = Main.ReverseGravitySupport(pos3);
					float num15 = num14 * (float)((gravDir == 1f) ? 1 : (-1)) + (float)Math.PI * (float)((gravDir == 1f) ? 1 : 0);
					spriteBatch.Draw(value, pos3, (Rectangle?)val, t * num11, num15, val.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num5 * (1f + num11) / 2f, (SpriteEffects)0, 0f);
					spriteBatch.Draw(value, pos3, (Rectangle?)val2, t2 * num11 * num11, num15, val2.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num5 * (1f + num11) / 2f, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	internal static void AddInputSnapshotComponents()
	{
		StateSnapshot.Input.AddVal("LockOnHelper.UseMode", () => UseMode, (LockOnMode v) =>
		{
			UseMode = v;
		});
		StateSnapshot.Input.AddVal("LockOnHelper._enabled", () => _enabled, (bool v) =>
		{
			_enabled = v;
		});
		StateSnapshot.Input.AddVal("LockOnHelper._canLockOn", () => _canLockOn, (bool v) =>
		{
			_canLockOn = v;
		});
		StateSnapshot.Input.AddRef("LockOnHelper._targets", () => _targets, (List<int> v) =>
		{
			_targets = v;
		});
		StateSnapshot.Input.AddVal("LockOnHelper._pickedTarget", () => _pickedTarget, (int v) =>
		{
			_pickedTarget = v;
		});
		StateSnapshot.Input.AddVal("LockOnHelper._lifeTimeCounter", () => _lifeTimeCounter, (int v) =>
		{
			_lifeTimeCounter = v;
		});
		StateSnapshot.Input.AddVal("LockOnHelper._lifeTimeArrowDisplay", () => _lifeTimeArrowDisplay, (int v) =>
		{
			_lifeTimeArrowDisplay = v;
		});
		StateSnapshot.Input.AddVal("LockOnHelper._threeDSTarget", () => _threeDSTarget, (int v) =>
		{
			_threeDSTarget = v;
		});
		StateSnapshot.Input.AddVal("LockOnHelper._targetClosestTarget", () => _targetClosestTarget, (int v) =>
		{
			_targetClosestTarget = v;
		});
		StateSnapshot.Input.AddVal("LockOnHelper.ForceUsability", () => ForceUsability, (bool v) =>
		{
			ForceUsability = v;
		});
	}
}
