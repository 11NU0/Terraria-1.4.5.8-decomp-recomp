using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria;

public class Mount
{
	private class DrillBeam
	{
		public Point16 curTileTarget;

		public int cooldown;

		public int lastPurpose;

		public DrillBeam()
		{
			curTileTarget = Point16.NegativeOne;
			cooldown = 0;
			lastPurpose = 0;
		}
	}

	private class DrillMountData
	{
		public float diodeRotationTarget;

		public float diodeRotation;

		public float outerRingRotation;

		public DrillBeam[] beams;

		public int beamCooldown;

		public Vector2 crosshairPosition;

		public DrillMountData()
		{
			beams = new DrillBeam[8];
			for (int i = 0; i < beams.Length; i++)
			{
				beams[i] = new DrillBeam();
			}
		}
	}

	private class BooleanMountData
	{
		public bool boolean;

		public BooleanMountData()
		{
			boolean = false;
		}
	}

	private class SelectiveFlyingMountData
	{
		public bool showFlyingFrames;

		public bool allowedToFly;

		public SelectiveFlyingMountData()
		{
			showFlyingFrames = false;
			allowedToFly = false;
		}
	}

	private class ExtraFrameMountData
	{
		public int frame;

		public float frameCounter;

		public ExtraFrameMountData()
		{
			frame = 0;
			frameCounter = 0f;
		}
	}

	public class MountDelegatesData
	{
		public delegate bool OverridePositionMethod(Player player, out Vector2? position);

		public delegate bool OverrideSizeMethod(Player player, out Vector2? size);

		public delegate Dust AdjustDashDustMethod(Player player, int currentDustCount, Dust dust);

		public Action<Vector2> MinecartDust;

		public Action<Player, Vector2, int, int> MinecartJumpingSound;

		public Action<Player, Vector2, int, int> MinecartLandingSound;

		public Action<Player, Vector2, int, int> MinecartBumperSound;

		public OverridePositionMethod MouthPosition;

		public OverridePositionMethod HandPosition;

		public OverrideSizeMethod PlayerSize;

		public AdjustDashDustMethod DashDust;

		public MountDelegatesData()
		{
			MinecartDust = DelegateMethods.Minecart.Sparks;
			MinecartJumpingSound = DelegateMethods.Minecart.JumpingSound;
			MinecartLandingSound = DelegateMethods.Minecart.LandingSound;
			MinecartBumperSound = DelegateMethods.Minecart.BumperSound;
		}
	}

	public class MountData
	{
		public Asset<Texture2D> backTexture = Asset<Texture2D>.Empty;

		public Asset<Texture2D> backTextureGlow = Asset<Texture2D>.Empty;

		public Asset<Texture2D> backTextureExtra = Asset<Texture2D>.Empty;

		public Asset<Texture2D> backTextureExtraGlow = Asset<Texture2D>.Empty;

		public Asset<Texture2D> frontTexture = Asset<Texture2D>.Empty;

		public Asset<Texture2D> frontTextureGlow = Asset<Texture2D>.Empty;

		public Asset<Texture2D> frontTextureExtra = Asset<Texture2D>.Empty;

		public Asset<Texture2D> frontTextureExtraGlow = Asset<Texture2D>.Empty;

		public int textureWidth;

		public int textureHeight;

		public int xOffset;

		public int yOffset;

		public int[] playerYOffsets;

		public int bodyFrame;

		public int playerHeadOffset;

		public int heightBoost;

		public int buff;

		public int flightTimeMax;

		public bool usesHover;

		public float runSpeed;

		public float dashSpeed;

		public float swimSpeed;

		public float acceleration;

		public float jumpSpeed;

		public int jumpHeight;

		public float fallDamage;

		public int extraFall;

		public int fatigueMax;

		public bool constantJump;

		public bool blockExtraJumps;

		public int abilityChargeMax;

		public int abilityDuration;

		public int abilityCooldown;

		public int walkingGraceTimeMax;

		public bool dismountsOnItemUse;

		public int spawnDust;

		public bool spawnDustNoGravity;

		public int totalFrames;

		public int standingFrameStart;

		public int standingFrameCount;

		public int standingFrameDelay;

		public int runningFrameStart;

		public int runningFrameCount;

		public int runningFrameDelay;

		public int flyingFrameStart;

		public int flyingFrameCount;

		public int flyingFrameDelay;

		public int inAirFrameStart;

		public int inAirFrameCount;

		public int inAirFrameDelay;

		public int idleFrameStart;

		public int idleFrameCount;

		public int idleFrameDelay;

		public bool idleFrameLoop;

		public int swimFrameStart;

		public int swimFrameCount;

		public int swimFrameDelay;

		public int dashingFrameStart;

		public int dashingFrameCount;

		public int dashingFrameDelay;

		public bool Minecart;

		public bool CanRideMinecartTracks;

		public bool CanUseWings;

		public bool MovementStatsAreAdditive;

		public Vector3 lightColor = Vector3.One;

		public bool emitsLight;

		public MountDelegatesData delegations = new MountDelegatesData();

		public int playerXOffset;

		public MountData()
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		}
	}

	public enum DismountCheckResult
	{
		Succeeded,
		FailedCCed,
		FailedNoSpace
	}

	public static int currentShader = 0;

	public const int FrameStanding = 0;

	public const int FrameRunning = 1;

	public const int FrameInAir = 2;

	public const int FrameFlying = 3;

	public const int FrameSwimming = 4;

	public const int FrameDashing = 5;

	public const int DrawBack = 0;

	public const int DrawBackExtra = 1;

	public const int DrawFront = 2;

	public const int DrawFrontExtra = 3;

	private static MountData[] mounts;

	private static Vector2[] scutlixEyePositions;

	private static Vector2 scutlixTextureSize;

	public const int scutlixBaseDamage = 50;

	public static Vector2 drillDiodePoint1 = new Vector2(36f, -6f);

	public static Vector2 drillDiodePoint2 = new Vector2(36f, 8f);

	public static Vector2 drillTextureSize;

	public const int drillTextureWidth = 80;

	public const float drillRotationChange = (float)Math.PI / 60f;

	public static int drillPickPower = 210;

	public static int drillPickTime = 1;

	public static int amountOfBeamsAtOnce = 2;

	public const float maxDrillLength = 48f;

	private static Vector2 santankTextureSize;

	private MountData _data;

	private int _type;

	private bool _flipDraw;

	private int _frame;

	private float _frameCounter;

	private int _frameExtra;

	private float _frameExtraCounter;

	private int _frameState;

	private int _flyTime;

	private int _idleTime;

	private int _idleTimeNext;

	private float _fatigue;

	private float _fatigueMax;

	private bool _abilityCharging;

	private int _abilityCharge;

	private int _abilityCooldown;

	private int _abilityDuration;

	private bool _abilityActive;

	private bool _aiming;

	private bool _shouldSuperCart;

	private int _walkingGraceTimeLeft;

	public List<DrillDebugDraw> _debugDraw;

	private object _mountSpecificData;

	private bool _active;

	public static float SuperCartRunSpeed = 20f;

	public static float SuperCartDashSpeed = 20f;

	public static float SuperCartAcceleration = 0.1f;

	public static int SuperCartJumpHeight = 15;

	public static float SuperCartJumpSpeed = 5.15f;

	private MountDelegatesData _defaultDelegatesData = new MountDelegatesData();

	public static int[] idleFrames_Rat = new int[11]
	{
		0, 1, 3, 2, 3, 2, 3, 2, 1, 0,
		0
	};

	public bool Active => _active;

	public int Type => _type;

	public int Frame => _frame;

	public int FlyTime => _flyTime;

	public int BuffType => _data.buff;

	public int BodyFrame => _data.bodyFrame;

	public int XOffset => _data.xOffset;

	public int YOffset => _data.yOffset;

	public int RunningGraceTime => _walkingGraceTimeLeft;

	public int PlayerXOFfset => _data.playerXOffset;

	public int PlayerOffset
	{
		get
		{
			if (!_active)
			{
				return 0;
			}
			if (_frame >= _data.totalFrames)
			{
				return 0;
			}
			return _data.playerYOffsets[_frame];
		}
	}

	public int PlayerOffsetHitbox
	{
		get
		{
			if (!_active)
			{
				return 0;
			}
			return -PlayerOffset + _data.heightBoost;
		}
	}

	public int PlayerHeadOffset
	{
		get
		{
			if (!_active)
			{
				return 0;
			}
			return _data.playerHeadOffset;
		}
	}

	public int HeightBoost => _data.heightBoost;

	public float RunSpeed
	{
		get
		{
			if (_type == 4 && _frameState == 4)
			{
				return _data.swimSpeed;
			}
			if ((_type == 12 || _type == 44 || _type == 49) && _frameState == 4)
			{
				return _data.swimSpeed;
			}
			if (_type == 12 && _frameState == 2)
			{
				return _data.runSpeed + 13.5f;
			}
			if (_type == 44 && _frameState == 2)
			{
				return _data.runSpeed + 4f;
			}
			if (_type == 5 && _frameState == 2)
			{
				float num = _fatigue / _fatigueMax;
				return _data.runSpeed + 4f * (1f - num);
			}
			if (_type == 50 && _frameState == 2)
			{
				return _data.runSpeed + 2f;
			}
			if (_shouldSuperCart)
			{
				return SuperCartRunSpeed;
			}
			return _data.runSpeed;
		}
	}

	public float DashSpeed
	{
		get
		{
			if (_shouldSuperCart)
			{
				return SuperCartDashSpeed;
			}
			return _data.dashSpeed;
		}
	}

	public float Acceleration
	{
		get
		{
			if (_shouldSuperCart)
			{
				return SuperCartAcceleration;
			}
			return _data.acceleration;
		}
	}

	public int ExtraFall => _data.extraFall;

	public float FallDamage => _data.fallDamage;

	public bool AutoJump => _data.constantJump;

	public bool BlockExtraJumps => _data.blockExtraJumps;

	public bool IsConsideredASlimeMount
	{
		get
		{
			if (_type != 3)
			{
				return _type == 50;
			}
			return true;
		}
	}

	public bool Cart
	{
		get
		{
			if (_data == null || !_active)
			{
				return false;
			}
			return _data.Minecart;
		}
	}

	public bool CanGrindRails
	{
		get
		{
			if (_data == null || !_active)
			{
				return false;
			}
			return _data.CanRideMinecartTracks;
		}
	}

	public bool AnyTrackRider
	{
		get
		{
			if (_data == null || !_active)
			{
				return false;
			}
			if (!_data.Minecart)
			{
				return _data.CanRideMinecartTracks;
			}
			return true;
		}
	}

	public bool CanUseWings
	{
		get
		{
			if (_data == null || !_active)
			{
				return true;
			}
			return _data.CanUseWings;
		}
	}

	public bool MovementStatsAreAdditive
	{
		get
		{
			if (_data == null || !_active)
			{
				return false;
			}
			return _data.MovementStatsAreAdditive;
		}
	}

	public bool IsAtIdle
	{
		get
		{
			if (_data == null || !_active)
			{
				return false;
			}
			if (_data.idleFrameCount != 0)
			{
				return _idleTime >= _idleTimeNext;
			}
			return false;
		}
	}

	public MountDelegatesData Delegations
	{
		get
		{
			if (_data == null)
			{
				return _defaultDelegatesData;
			}
			return _data.delegations;
		}
	}

	public Vector2 Origin
	{
		get
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)_data.textureWidth / 2f, (float)_data.textureHeight / (2f * (float)_data.totalFrames));
		}
	}

	public bool AbilityCharging => _abilityCharging;

	public bool AbilityActive => _abilityActive;

	public float AbilityCharge => (float)_abilityCharge / (float)_data.abilityChargeMax;

	public bool AllowDirectionChange
	{
		get
		{
			int type = _type;
			if (type == 9)
			{
				return _abilityCooldown < _data.abilityCooldown / 2;
			}
			return true;
		}
	}

	public bool DismountOnItemUse
	{
		get
		{
			if (!Active)
			{
				return false;
			}
			return _data.dismountsOnItemUse;
		}
	}

	public void ApplyDummyFrameCounters()
	{
		_frameCounter = 0f;
	}

	private static void MeowcartLandingSound(Player Player, Vector2 Position, int Width, int Height)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(37, (int)Position.X + Width / 2, (int)Position.Y + Height / 2, 5);
	}

	private static void MeowcartBumperSound(Player Player, Vector2 Position, int Width, int Height)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(37, (int)Position.X + Width / 2, (int)Position.Y + Height / 2, 3);
	}

	public Mount()
	{
		_debugDraw = new List<DrillDebugDraw>();
		Reset();
	}

	public void Reset()
	{
		_active = false;
		_type = -1;
		_flipDraw = false;
		_frame = 0;
		_frameCounter = 0f;
		_frameExtra = 0;
		_frameExtraCounter = 0f;
		_frameState = 0;
		_flyTime = 0;
		_idleTime = 0;
		_idleTimeNext = -1;
		_walkingGraceTimeLeft = 0;
		_fatigueMax = 0f;
		_abilityCharging = false;
		_abilityCharge = 0;
		_aiming = false;
		_shouldSuperCart = false;
	}

	public static void Initialize()
	{
		//IL_154b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1550: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_192a: Unknown result type (might be due to invalid IL or missing references)
		//IL_192f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1944: Unknown result type (might be due to invalid IL or missing references)
		//IL_1949: Unknown result type (might be due to invalid IL or missing references)
		//IL_195e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1963: Unknown result type (might be due to invalid IL or missing references)
		//IL_1978: Unknown result type (might be due to invalid IL or missing references)
		//IL_197d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1992: Unknown result type (might be due to invalid IL or missing references)
		//IL_1997: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ccb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f63: Unknown result type (might be due to invalid IL or missing references)
		//IL_341e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3423: Unknown result type (might be due to invalid IL or missing references)
		mounts = new MountData[MountID.Count];
		MountData mountData = new MountData();
		mounts[0] = mountData;
		mountData.spawnDust = 57;
		mountData.spawnDustNoGravity = false;
		mountData.buff = 90;
		mountData.heightBoost = 20;
		mountData.flightTimeMax = 160;
		mountData.runSpeed = 5.5f;
		mountData.dashSpeed = 12f;
		mountData.acceleration = 0.09f;
		mountData.jumpHeight = 17;
		mountData.jumpSpeed = 5.31f;
		mountData.totalFrames = 12;
		int[] array = new int[mountData.totalFrames];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 30;
		}
		array[1] += 2;
		array[11] += 2;
		mountData.playerYOffsets = array;
		mountData.xOffset = 13;
		mountData.bodyFrame = 3;
		mountData.yOffset = -7;
		mountData.playerHeadOffset = 22;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 6;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 6;
		mountData.flyingFrameCount = 6;
		mountData.flyingFrameDelay = 6;
		mountData.flyingFrameStart = 6;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 1;
		mountData.idleFrameCount = 4;
		mountData.idleFrameDelay = 30;
		mountData.idleFrameStart = 2;
		mountData.idleFrameLoop = true;
		mountData.swimFrameCount = mountData.inAirFrameCount;
		mountData.swimFrameDelay = mountData.inAirFrameDelay;
		mountData.swimFrameStart = mountData.inAirFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.RudolphMount[0];
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.RudolphMount[1];
			mountData.frontTextureExtra = TextureAssets.RudolphMount[2];
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[2] = mountData;
		mountData.spawnDust = 58;
		mountData.buff = 129;
		mountData.heightBoost = 20;
		mountData.flightTimeMax = 160;
		mountData.runSpeed = 5f;
		mountData.dashSpeed = 9f;
		mountData.acceleration = 0.08f;
		mountData.jumpHeight = 10;
		mountData.jumpSpeed = 6.01f;
		mountData.totalFrames = 16;
		array = new int[mountData.totalFrames];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = 22;
		}
		array[12] += 2;
		array[13] += 4;
		array[14] += 2;
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 8;
		mountData.playerHeadOffset = 22;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 7;
		mountData.runningFrameCount = 5;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 11;
		mountData.flyingFrameCount = 6;
		mountData.flyingFrameDelay = 6;
		mountData.flyingFrameStart = 1;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 0;
		mountData.idleFrameCount = 3;
		mountData.idleFrameDelay = 20;
		mountData.idleFrameStart = 8;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = mountData.inAirFrameCount;
		mountData.swimFrameDelay = mountData.inAirFrameDelay;
		mountData.swimFrameStart = mountData.inAirFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.PigronMount;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[1] = mountData;
		mountData.spawnDust = 15;
		mountData.buff = 128;
		mountData.heightBoost = 20;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.8f;
		mountData.runSpeed = 4f;
		mountData.dashSpeed = 7.8f;
		mountData.acceleration = 0.13f;
		mountData.jumpHeight = 15;
		mountData.jumpSpeed = 5.01f;
		mountData.totalFrames = 7;
		array = new int[mountData.totalFrames];
		for (int k = 0; k < array.Length; k++)
		{
			array[k] = 14;
		}
		array[2] += 2;
		array[3] += 4;
		array[4] += 8;
		array[5] += 8;
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 4;
		mountData.playerHeadOffset = 22;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 7;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 6;
		mountData.flyingFrameDelay = 6;
		mountData.flyingFrameStart = 1;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 5;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = mountData.inAirFrameCount;
		mountData.swimFrameDelay = mountData.inAirFrameDelay;
		mountData.swimFrameStart = mountData.inAirFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.BunnyMount;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[3] = mountData;
		mountData.spawnDust = 56;
		mountData.buff = 130;
		mountData.heightBoost = 20;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.5f;
		mountData.extraFall = 10;
		mountData.runSpeed = 4f;
		mountData.dashSpeed = 4f;
		mountData.acceleration = 0.18f;
		mountData.jumpHeight = 12;
		mountData.jumpSpeed = 8.25f;
		mountData.constantJump = true;
		mountData.totalFrames = 4;
		array = new int[mountData.totalFrames];
		for (int l = 0; l < array.Length; l++)
		{
			array[l] = 20;
		}
		array[1] += 2;
		array[3] -= 2;
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 11;
		mountData.playerHeadOffset = 22;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 4;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 0;
		mountData.flyingFrameDelay = 0;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 1;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.SlimeMount;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[6] = mountData;
		mountData.Minecart = true;
		mountData.delegations = new MountDelegatesData();
		mountData.delegations.MinecartDust = DelegateMethods.Minecart.Sparks;
		mountData.spawnDust = 213;
		mountData.buff = 118;
		mountData.heightBoost = 10;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 1f;
		mountData.runSpeed = 13f;
		mountData.dashSpeed = 13f;
		mountData.acceleration = 0.04f;
		mountData.jumpHeight = 15;
		mountData.jumpSpeed = 5.15f;
		mountData.blockExtraJumps = true;
		mountData.totalFrames = 3;
		array = new int[mountData.totalFrames];
		for (int m = 0; m < array.Length; m++)
		{
			array[m] = 8;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 13;
		mountData.playerHeadOffset = 14;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 3;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 0;
		mountData.flyingFrameDelay = 0;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 0;
		mountData.inAirFrameDelay = 0;
		mountData.inAirFrameStart = 0;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.MinecartMount;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[15] = mountData;
		SetAsMinecart(mountData, 208, TextureAssets.DesertMinecartMount);
		mountData = new MountData();
		mounts[18] = mountData;
		SetAsMinecart(mountData, 220, TextureAssets.Extra[108]);
		mountData = new MountData();
		mounts[19] = mountData;
		SetAsMinecart(mountData, 222, TextureAssets.Extra[109]);
		mountData = new MountData();
		mounts[20] = mountData;
		SetAsMinecart(mountData, 224, TextureAssets.Extra[110]);
		mountData = new MountData();
		mounts[21] = mountData;
		SetAsMinecart(mountData, 226, TextureAssets.Extra[111]);
		mountData = new MountData();
		mounts[22] = mountData;
		SetAsMinecart(mountData, 228, TextureAssets.Extra[112]);
		mountData = new MountData();
		mounts[24] = mountData;
		SetAsMinecart(mountData, 231, TextureAssets.Extra[115]);
		mountData.frontTextureGlow = TextureAssets.Extra[116];
		mountData = new MountData();
		mounts[25] = mountData;
		SetAsMinecart(mountData, 233, TextureAssets.Extra[117]);
		mountData = new MountData();
		mounts[26] = mountData;
		SetAsMinecart(mountData, 235, TextureAssets.Extra[118]);
		mountData = new MountData();
		mounts[27] = mountData;
		SetAsMinecart(mountData, 237, TextureAssets.Extra[119]);
		mountData = new MountData();
		mounts[28] = mountData;
		SetAsMinecart(mountData, 239, TextureAssets.Extra[120]);
		mountData = new MountData();
		mounts[29] = mountData;
		SetAsMinecart(mountData, 241, TextureAssets.Extra[121]);
		mountData = new MountData();
		mounts[30] = mountData;
		SetAsMinecart(mountData, 243, TextureAssets.Extra[122]);
		mountData = new MountData();
		mounts[31] = mountData;
		SetAsMinecart(mountData, 245, TextureAssets.Extra[123]);
		mountData = new MountData();
		mounts[32] = mountData;
		SetAsMinecart(mountData, 247, TextureAssets.Extra[124]);
		mountData = new MountData();
		mounts[33] = mountData;
		SetAsMinecart(mountData, 249, TextureAssets.Extra[125]);
		mountData.delegations.MinecartDust = DelegateMethods.Minecart.SparksMeow;
		mountData.delegations.MinecartLandingSound = MeowcartLandingSound;
		mountData.delegations.MinecartBumperSound = MeowcartBumperSound;
		mountData = new MountData();
		mounts[34] = mountData;
		SetAsMinecart(mountData, 251, TextureAssets.Extra[126]);
		mountData = new MountData();
		mounts[35] = mountData;
		SetAsMinecart(mountData, 253, TextureAssets.Extra[127]);
		mountData = new MountData();
		mounts[36] = mountData;
		SetAsMinecart(mountData, 255, TextureAssets.Extra[128]);
		mountData = new MountData();
		mounts[38] = mountData;
		SetAsMinecart(mountData, 269, TextureAssets.Extra[150]);
		if (Main.netMode != 2)
		{
			mountData.backTexture = mountData.frontTexture;
		}
		mountData = new MountData();
		mounts[39] = mountData;
		SetAsMinecart(mountData, 272, TextureAssets.Extra[155]);
		mountData.yOffset -= 2;
		if (Main.netMode != 2)
		{
			mountData.frontTextureExtra = TextureAssets.Extra[165];
		}
		mountData.runSpeed = 6f;
		mountData.dashSpeed = 6f;
		mountData.acceleration = 0.02f;
		mountData = new MountData();
		mounts[16] = mountData;
		mountData.Minecart = true;
		mountData.delegations = new MountDelegatesData();
		mountData.delegations.MinecartDust = DelegateMethods.Minecart.Sparks;
		mountData.spawnDust = 213;
		mountData.buff = 210;
		mountData.heightBoost = 10;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 1f;
		mountData.runSpeed = 13f;
		mountData.dashSpeed = 13f;
		mountData.acceleration = 0.04f;
		mountData.jumpHeight = 15;
		mountData.jumpSpeed = 5.15f;
		mountData.blockExtraJumps = true;
		mountData.totalFrames = 3;
		array = new int[mountData.totalFrames];
		for (int n = 0; n < array.Length; n++)
		{
			array[n] = 8;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 13;
		mountData.playerHeadOffset = 14;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 3;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 0;
		mountData.flyingFrameDelay = 0;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 0;
		mountData.inAirFrameDelay = 0;
		mountData.inAirFrameStart = 0;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.FishMinecartMount;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[51] = mountData;
		SetAsMinecart(mountData, 338, TextureAssets.Extra[246], -10, -8);
		mountData.spawnDust = 211;
		mountData.delegations.MinecartDust = DelegateMethods.Minecart.SparksFart;
		mountData.delegations.MinecartBumperSound = DelegateMethods.Minecart.BumperSoundFart;
		mountData.delegations.MinecartLandingSound = DelegateMethods.Minecart.LandingSoundFart;
		mountData.delegations.MinecartJumpingSound = DelegateMethods.Minecart.JumpingSoundFart;
		mountData = new MountData();
		mounts[53] = mountData;
		SetAsMinecart(mountData, 346, TextureAssets.Extra[251], -10, -8);
		mountData.spawnDust = 211;
		mountData.delegations.MinecartDust = DelegateMethods.Minecart.SparksTerraFart;
		mountData.delegations.MinecartBumperSound = DelegateMethods.Minecart.BumperSoundFart;
		mountData.delegations.MinecartLandingSound = DelegateMethods.Minecart.LandingSoundFart;
		mountData.delegations.MinecartJumpingSound = DelegateMethods.Minecart.JumpingSoundFart;
		mountData = new MountData();
		mounts[4] = mountData;
		mountData.spawnDust = 56;
		mountData.buff = 131;
		mountData.heightBoost = 26;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 1f;
		mountData.runSpeed = 2f;
		mountData.dashSpeed = 5f;
		mountData.swimSpeed = 10f;
		mountData.acceleration = 0.08f;
		mountData.jumpHeight = 12;
		mountData.jumpSpeed = 3.7f;
		mountData.totalFrames = 12;
		array = new int[mountData.totalFrames];
		for (int num = 0; num < array.Length; num++)
		{
			array[num] = 26;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 13;
		mountData.playerHeadOffset = 28;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 6;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 0;
		mountData.flyingFrameDelay = 0;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 3;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = 6;
		mountData.swimFrameDelay = 12;
		mountData.swimFrameStart = 6;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.TurtleMount;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[5] = mountData;
		mountData.spawnDust = 152;
		mountData.buff = 132;
		mountData.heightBoost = 16;
		mountData.flightTimeMax = 320;
		mountData.fatigueMax = 320;
		mountData.fallDamage = 0f;
		mountData.usesHover = true;
		mountData.runSpeed = 2f;
		mountData.dashSpeed = 2f;
		mountData.acceleration = 0.16f;
		mountData.jumpHeight = 10;
		mountData.jumpSpeed = 4f;
		mountData.blockExtraJumps = true;
		mountData.totalFrames = 12;
		array = new int[mountData.totalFrames];
		for (int num2 = 0; num2 < array.Length; num2++)
		{
			array[num2] = 16;
		}
		array[8] = 18;
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 4;
		mountData.playerHeadOffset = 18;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 5;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 3;
		mountData.flyingFrameDelay = 12;
		mountData.flyingFrameStart = 5;
		mountData.inAirFrameCount = 3;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 5;
		mountData.idleFrameCount = 4;
		mountData.idleFrameDelay = 12;
		mountData.idleFrameStart = 8;
		mountData.idleFrameLoop = true;
		mountData.swimFrameCount = 0;
		mountData.swimFrameDelay = 12;
		mountData.swimFrameStart = 0;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.BeeMount[0];
			mountData.backTextureExtra = TextureAssets.BeeMount[1];
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[7] = mountData;
		mountData.spawnDust = 226;
		mountData.spawnDustNoGravity = true;
		mountData.buff = 141;
		mountData.heightBoost = 16;
		mountData.flightTimeMax = 320;
		mountData.fatigueMax = 320;
		mountData.fallDamage = 0f;
		mountData.usesHover = true;
		mountData.runSpeed = 8f;
		mountData.dashSpeed = 8f;
		mountData.acceleration = 0.16f;
		mountData.jumpHeight = 10;
		mountData.jumpSpeed = 4f;
		mountData.blockExtraJumps = true;
		mountData.totalFrames = 8;
		array = new int[mountData.totalFrames];
		for (int num3 = 0; num3 < array.Length; num3++)
		{
			array[num3] = 16;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 4;
		mountData.playerHeadOffset = 18;
		mountData.standingFrameCount = 8;
		mountData.standingFrameDelay = 4;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 8;
		mountData.runningFrameDelay = 4;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 8;
		mountData.flyingFrameDelay = 4;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 8;
		mountData.inAirFrameDelay = 4;
		mountData.inAirFrameStart = 0;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 12;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = true;
		mountData.swimFrameCount = 0;
		mountData.swimFrameDelay = 12;
		mountData.swimFrameStart = 0;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.UfoMount[0];
			mountData.frontTextureExtra = TextureAssets.UfoMount[1];
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[8] = mountData;
		mountData.spawnDust = 226;
		mountData.buff = 142;
		mountData.heightBoost = 16;
		mountData.flightTimeMax = 320;
		mountData.fatigueMax = 320;
		mountData.fallDamage = 1f;
		mountData.usesHover = true;
		mountData.swimSpeed = 4f;
		mountData.runSpeed = 6f;
		mountData.dashSpeed = 4f;
		mountData.acceleration = 0.16f;
		mountData.jumpHeight = 10;
		mountData.jumpSpeed = 4f;
		mountData.blockExtraJumps = true;
		mountData.emitsLight = true;
		mountData.lightColor = new Vector3(0.3f, 0.3f, 0.4f);
		mountData.totalFrames = 1;
		array = new int[mountData.totalFrames];
		for (int num4 = 0; num4 < array.Length; num4++)
		{
			array[num4] = 4;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 4;
		mountData.playerHeadOffset = 18;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 1;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 1;
		mountData.flyingFrameDelay = 12;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 0;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 12;
		mountData.idleFrameStart = 8;
		mountData.swimFrameCount = 0;
		mountData.swimFrameDelay = 12;
		mountData.swimFrameStart = 0;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.DrillMount[0];
			mountData.backTextureGlow = TextureAssets.DrillMount[3];
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.backTextureExtraGlow = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.DrillMount[1];
			mountData.frontTextureGlow = TextureAssets.DrillMount[4];
			mountData.frontTextureExtra = TextureAssets.DrillMount[2];
			mountData.frontTextureExtraGlow = TextureAssets.DrillMount[5];
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		drillTextureSize = new Vector2(80f, 80f);
		if (!Main.dedServ)
		{
			Vector2 val = new Vector2((float)mountData.textureWidth, (float)(mountData.textureHeight / mountData.totalFrames));
			if (drillTextureSize != val)
			{
				throw new Exception("Be sure to update the Drill texture origin to match the actual texture size of " + mountData.textureWidth + ", " + mountData.textureHeight + ".");
			}
		}
		mountData = new MountData();
		mounts[9] = mountData;
		mountData.spawnDust = 15;
		mountData.buff = 143;
		mountData.heightBoost = 16;
		mountData.flightTimeMax = 0;
		mountData.fatigueMax = 0;
		mountData.fallDamage = 0f;
		mountData.abilityChargeMax = 40;
		mountData.abilityCooldown = 20;
		mountData.abilityDuration = 0;
		mountData.runSpeed = 8f;
		mountData.dashSpeed = 8f;
		mountData.acceleration = 0.4f;
		mountData.jumpHeight = 22;
		mountData.jumpSpeed = 10.01f;
		mountData.blockExtraJumps = false;
		mountData.totalFrames = 12;
		array = new int[mountData.totalFrames];
		for (int num5 = 0; num5 < array.Length; num5++)
		{
			array[num5] = 16;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 6;
		mountData.playerHeadOffset = 18;
		mountData.standingFrameCount = 6;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 6;
		mountData.runningFrameCount = 6;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 0;
		mountData.flyingFrameDelay = 12;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 1;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 12;
		mountData.idleFrameStart = 6;
		mountData.idleFrameLoop = true;
		mountData.swimFrameCount = 0;
		mountData.swimFrameDelay = 12;
		mountData.swimFrameStart = 0;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.ScutlixMount[0];
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.ScutlixMount[1];
			mountData.frontTextureExtra = TextureAssets.ScutlixMount[2];
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		scutlixEyePositions = new Vector2[10];
		scutlixEyePositions[0] = new Vector2(60f, 2f);
		scutlixEyePositions[1] = new Vector2(70f, 6f);
		scutlixEyePositions[2] = new Vector2(68f, 6f);
		scutlixEyePositions[3] = new Vector2(76f, 12f);
		scutlixEyePositions[4] = new Vector2(80f, 10f);
		scutlixEyePositions[5] = new Vector2(84f, 18f);
		scutlixEyePositions[6] = new Vector2(74f, 20f);
		scutlixEyePositions[7] = new Vector2(76f, 24f);
		scutlixEyePositions[8] = new Vector2(70f, 34f);
		scutlixEyePositions[9] = new Vector2(76f, 34f);
		scutlixTextureSize = new Vector2(45f, 54f);
		if (!Main.dedServ)
		{
			Vector2 val2 = new Vector2((float)(mountData.textureWidth / 2), (float)(mountData.textureHeight / mountData.totalFrames));
			if (scutlixTextureSize != val2)
			{
				throw new Exception("Be sure to update the Scutlix texture origin to match the actual texture size of " + mountData.textureWidth + ", " + mountData.textureHeight + ".");
			}
		}
		for (int num6 = 0; num6 < scutlixEyePositions.Length; num6++)
		{
			ref Vector2 reference = ref scutlixEyePositions[num6];
			reference -= scutlixTextureSize;
		}
		mountData = new MountData();
		mounts[10] = mountData;
		mountData.spawnDust = 15;
		mountData.buff = 162;
		mountData.heightBoost = 34;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.2f;
		mountData.runSpeed = 4f;
		mountData.dashSpeed = 12f;
		mountData.acceleration = 0.3f;
		mountData.jumpHeight = 10;
		mountData.jumpSpeed = 8.01f;
		mountData.totalFrames = 16;
		array = new int[mountData.totalFrames];
		for (int num7 = 0; num7 < array.Length; num7++)
		{
			array[num7] = 28;
		}
		array[3] += 2;
		array[4] += 2;
		array[7] += 2;
		array[8] += 2;
		array[12] += 2;
		array[13] += 2;
		array[15] += 4;
		mountData.playerYOffsets = array;
		mountData.xOffset = 5;
		mountData.bodyFrame = 3;
		mountData.yOffset = 1;
		mountData.playerHeadOffset = 34;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 7;
		mountData.runningFrameDelay = 15;
		mountData.runningFrameStart = 1;
		mountData.dashingFrameCount = 6;
		mountData.dashingFrameDelay = 40;
		mountData.dashingFrameStart = 9;
		mountData.flyingFrameCount = 6;
		mountData.flyingFrameDelay = 6;
		mountData.flyingFrameStart = 1;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 15;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = mountData.inAirFrameCount;
		mountData.swimFrameDelay = mountData.inAirFrameDelay;
		mountData.swimFrameStart = mountData.inAirFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.UnicornMount;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[11] = mountData;
		mountData.Minecart = true;
		mountData.delegations = new MountDelegatesData();
		mountData.delegations.MinecartDust = DelegateMethods.Minecart.SparksMech;
		mountData.spawnDust = 213;
		mountData.buff = 166;
		mountData.heightBoost = 12;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 1f;
		mountData.runSpeed = 13f;
		mountData.dashSpeed = 13f;
		mountData.acceleration = 0.04f;
		mountData.jumpHeight = 15;
		mountData.jumpSpeed = 5.15f;
		mountData.blockExtraJumps = true;
		mountData.totalFrames = 3;
		array = new int[mountData.totalFrames];
		for (int num8 = 0; num8 < array.Length; num8++)
		{
			array[num8] = 9;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = -1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 11;
		mountData.playerHeadOffset = 14;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 3;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 0;
		mountData.flyingFrameDelay = 0;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 0;
		mountData.inAirFrameDelay = 0;
		mountData.inAirFrameStart = 0;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.MinecartMechMount[0];
			mountData.frontTextureGlow = TextureAssets.MinecartMechMount[1];
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[12] = mountData;
		mountData.spawnDust = 15;
		mountData.buff = 168;
		mountData.heightBoost = 14;
		mountData.flightTimeMax = 320;
		mountData.fatigueMax = 320;
		mountData.fallDamage = 0f;
		mountData.usesHover = true;
		mountData.runSpeed = 2f;
		mountData.dashSpeed = 1f;
		mountData.acceleration = 0.2f;
		mountData.jumpHeight = 4;
		mountData.jumpSpeed = 3f;
		mountData.swimSpeed = 16f;
		mountData.blockExtraJumps = true;
		mountData.totalFrames = 23;
		array = new int[mountData.totalFrames];
		for (int num9 = 0; num9 < array.Length; num9++)
		{
			array[num9] = 12;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 2;
		mountData.bodyFrame = 3;
		mountData.yOffset = 16;
		mountData.playerHeadOffset = 16;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 8;
		mountData.runningFrameCount = 7;
		mountData.runningFrameDelay = 14;
		mountData.runningFrameStart = 8;
		mountData.flyingFrameCount = 8;
		mountData.flyingFrameDelay = 16;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 8;
		mountData.inAirFrameDelay = 6;
		mountData.inAirFrameStart = 0;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = 8;
		mountData.swimFrameDelay = 4;
		mountData.swimFrameStart = 15;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.CuteFishronMount[0];
			mountData.backTextureGlow = TextureAssets.CuteFishronMount[1];
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[13] = mountData;
		mountData.Minecart = true;
		mountData.delegations = new MountDelegatesData();
		mountData.delegations.MinecartDust = DelegateMethods.Minecart.Sparks;
		mountData.spawnDust = 213;
		mountData.buff = 184;
		mountData.heightBoost = 10;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 1f;
		mountData.runSpeed = 10f;
		mountData.dashSpeed = 10f;
		mountData.acceleration = 0.03f;
		mountData.jumpHeight = 12;
		mountData.jumpSpeed = 5.15f;
		mountData.blockExtraJumps = true;
		mountData.totalFrames = 3;
		array = new int[mountData.totalFrames];
		for (int num10 = 0; num10 < array.Length; num10++)
		{
			array[num10] = 8;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 13;
		mountData.playerHeadOffset = 14;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 3;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 0;
		mountData.flyingFrameDelay = 0;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 0;
		mountData.inAirFrameDelay = 0;
		mountData.inAirFrameStart = 0;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.MinecartWoodMount;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[14] = mountData;
		mountData.spawnDust = 15;
		mountData.buff = 193;
		mountData.heightBoost = 6;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.2f;
		mountData.runSpeed = 8f;
		mountData.acceleration = 0.25f;
		mountData.jumpHeight = 20;
		mountData.jumpSpeed = 8.01f;
		mountData.totalFrames = 8;
		array = new int[mountData.totalFrames];
		for (int num11 = 0; num11 < array.Length; num11++)
		{
			array[num11] = 8;
		}
		array[1] += 2;
		array[3] += 2;
		array[6] += 2;
		mountData.playerYOffsets = array;
		mountData.xOffset = 4;
		mountData.bodyFrame = 3;
		mountData.yOffset = 8;
		mountData.playerHeadOffset = 10;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 6;
		mountData.runningFrameDelay = 30;
		mountData.runningFrameStart = 2;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 1;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = mountData.inAirFrameCount;
		mountData.swimFrameDelay = mountData.inAirFrameDelay;
		mountData.swimFrameStart = mountData.inAirFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.BasiliskMount;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[17] = mountData;
		mountData.spawnDust = 15;
		mountData.buff = 212;
		mountData.heightBoost = 16;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.2f;
		mountData.runSpeed = 8f;
		mountData.acceleration = 0.25f;
		mountData.jumpHeight = 20;
		mountData.jumpSpeed = 8.01f;
		mountData.totalFrames = 4;
		array = new int[mountData.totalFrames];
		for (int num12 = 0; num12 < array.Length; num12++)
		{
			array[num12] = 8;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 2;
		mountData.bodyFrame = 3;
		mountData.yOffset = 17 - mountData.heightBoost;
		mountData.playerHeadOffset = 18;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 4;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 1;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = mountData.inAirFrameCount;
		mountData.swimFrameDelay = mountData.inAirFrameDelay;
		mountData.swimFrameStart = mountData.inAirFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.Extra[97];
			mountData.backTextureExtra = TextureAssets.Extra[96];
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTextureExtra.Width();
			mountData.textureHeight = mountData.backTextureExtra.Height();
		}
		mountData = new MountData();
		mounts[23] = mountData;
		mountData.spawnDust = 43;
		mountData.spawnDustNoGravity = true;
		mountData.buff = 230;
		mountData.heightBoost = 0;
		mountData.flightTimeMax = 320;
		mountData.fatigueMax = 320;
		mountData.fallDamage = 0f;
		mountData.usesHover = true;
		mountData.runSpeed = 9f;
		mountData.dashSpeed = 9f;
		mountData.acceleration = 0.16f;
		mountData.jumpHeight = 10;
		mountData.jumpSpeed = 4f;
		mountData.blockExtraJumps = true;
		mountData.totalFrames = 6;
		array = new int[mountData.totalFrames];
		for (int num13 = 0; num13 < array.Length; num13++)
		{
			array[num13] = 6;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = -2;
		mountData.bodyFrame = 0;
		mountData.yOffset = 8;
		mountData.playerHeadOffset = 0;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 0;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 1;
		mountData.runningFrameDelay = 0;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 1;
		mountData.flyingFrameDelay = 0;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 6;
		mountData.inAirFrameDelay = 8;
		mountData.inAirFrameStart = 0;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = true;
		mountData.swimFrameCount = 0;
		mountData.swimFrameDelay = 0;
		mountData.swimFrameStart = 0;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.Extra[113];
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[37] = mountData;
		mountData.spawnDust = 282;
		mountData.buff = 265;
		mountData.heightBoost = 12;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.2f;
		mountData.runSpeed = 6f;
		mountData.acceleration = 0.15f;
		mountData.jumpHeight = 14;
		mountData.jumpSpeed = 6.01f;
		mountData.totalFrames = 10;
		array = new int[mountData.totalFrames];
		for (int num14 = 0; num14 < array.Length; num14++)
		{
			array[num14] = 20;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 5;
		mountData.bodyFrame = 4;
		mountData.yOffset = 1;
		mountData.playerHeadOffset = 20;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 7;
		mountData.runningFrameDelay = 20;
		mountData.runningFrameStart = 2;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 1;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = mountData.runningFrameCount;
		mountData.swimFrameDelay = 10;
		mountData.swimFrameStart = mountData.runningFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.Extra[149];
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[40] = mountData;
		SetAsHorse(mountData, 275, TextureAssets.Extra[161]);
		mountData = new MountData();
		mounts[41] = mountData;
		SetAsHorse(mountData, 276, TextureAssets.Extra[162]);
		mountData = new MountData();
		mounts[42] = mountData;
		SetAsHorse(mountData, 277, TextureAssets.Extra[163]);
		mountData = new MountData();
		mounts[43] = mountData;
		mountData.spawnDust = 15;
		mountData.buff = 278;
		mountData.heightBoost = 12;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.25f;
		mountData.extraFall = 20;
		mountData.runSpeed = 5f;
		mountData.acceleration = 0.1f;
		mountData.jumpHeight = 8;
		mountData.jumpSpeed = 8f;
		mountData.constantJump = true;
		mountData.totalFrames = 4;
		array = new int[mountData.totalFrames];
		for (int num15 = 0; num15 < array.Length; num15++)
		{
			array[num15] = 14;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 5;
		mountData.bodyFrame = 4;
		mountData.yOffset = 10;
		mountData.playerHeadOffset = 10;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 5;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 4;
		mountData.runningFrameDelay = 5;
		mountData.runningFrameStart = 0;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 5;
		mountData.inAirFrameStart = 0;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = 1;
		mountData.swimFrameDelay = 5;
		mountData.swimFrameStart = 0;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.Extra[164];
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[44] = mountData;
		mountData.spawnDust = 228;
		mountData.buff = 279;
		mountData.heightBoost = 24;
		mountData.flightTimeMax = 320;
		mountData.fatigueMax = 320;
		mountData.fallDamage = 0f;
		mountData.usesHover = true;
		mountData.runSpeed = 3f;
		mountData.dashSpeed = 6f;
		mountData.acceleration = 0.12f;
		mountData.jumpHeight = 3;
		mountData.jumpSpeed = 1f;
		mountData.swimSpeed = mountData.runSpeed;
		mountData.blockExtraJumps = true;
		mountData.totalFrames = 10;
		array = new int[mountData.totalFrames];
		for (int num16 = 0; num16 < array.Length; num16++)
		{
			array[num16] = 9;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 0;
		mountData.bodyFrame = 3;
		mountData.yOffset = 8;
		mountData.playerHeadOffset = 16;
		mountData.runningFrameCount = 10;
		mountData.runningFrameDelay = 8;
		mountData.runningFrameStart = 0;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.Extra[166];
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[45] = mountData;
		mountData.spawnDust = 6;
		mountData.buff = 280;
		mountData.heightBoost = 25;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.1f;
		mountData.runSpeed = 12f;
		mountData.dashSpeed = 16f;
		mountData.acceleration = 0.5f;
		mountData.jumpHeight = 14;
		mountData.jumpSpeed = 7f;
		mountData.emitsLight = true;
		mountData.lightColor = new Vector3(0.6f, 0.4f, 0.35f);
		mountData.totalFrames = 8;
		array = new int[mountData.totalFrames];
		for (int num17 = 0; num17 < array.Length; num17++)
		{
			array[num17] = 30;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 0;
		mountData.bodyFrame = 0;
		mountData.xOffset = 2;
		mountData.yOffset = 1;
		mountData.playerHeadOffset = 20;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 20;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 6;
		mountData.runningFrameDelay = 20;
		mountData.runningFrameStart = 2;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 20;
		mountData.inAirFrameStart = 1;
		mountData.swimFrameCount = mountData.runningFrameCount;
		mountData.swimFrameDelay = 20;
		mountData.swimFrameStart = mountData.runningFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.Extra[167];
			mountData.backTextureGlow = TextureAssets.GlowMask[283];
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[46] = mountData;
		mountData.spawnDust = 15;
		mountData.buff = 281;
		mountData.heightBoost = 0;
		mountData.flightTimeMax = 0;
		mountData.fatigueMax = 0;
		mountData.fallDamage = 0f;
		mountData.abilityChargeMax = 40;
		mountData.abilityCooldown = 40;
		mountData.abilityDuration = 0;
		mountData.runSpeed = 8f;
		mountData.dashSpeed = 8f;
		mountData.acceleration = 0.4f;
		mountData.jumpHeight = 8;
		mountData.jumpSpeed = 9.01f;
		mountData.blockExtraJumps = false;
		mountData.totalFrames = 27;
		array = new int[mountData.totalFrames];
		for (int num18 = 0; num18 < array.Length; num18++)
		{
			array[num18] = 4;
			if (num18 == 1 || num18 == 2 || num18 == 7 || num18 == 8)
			{
				array[num18] += 2;
			}
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = 1;
		mountData.playerHeadOffset = 2;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 11;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 0;
		mountData.inAirFrameCount = 11;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 1;
		mountData.swimFrameCount = mountData.runningFrameCount;
		mountData.swimFrameDelay = mountData.runningFrameDelay;
		mountData.swimFrameStart = mountData.runningFrameStart;
		santankTextureSize = new Vector2(23f, 2f);
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.Extra[168];
			mountData.frontTextureExtra = TextureAssets.Extra[168];
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[47] = mountData;
		mountData.spawnDust = 5;
		mountData.buff = 282;
		mountData.heightBoost = 34;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.2f;
		mountData.runSpeed = 4f;
		mountData.dashSpeed = 12f;
		mountData.acceleration = 0.3f;
		mountData.jumpHeight = 10;
		mountData.jumpSpeed = 8.01f;
		mountData.totalFrames = 16;
		array = new int[mountData.totalFrames];
		for (int num19 = 0; num19 < array.Length; num19++)
		{
			array[num19] = 30;
		}
		array[3] += 2;
		array[4] += 2;
		array[7] += 2;
		array[8] += 2;
		array[12] += 2;
		array[13] += 2;
		array[15] += 4;
		mountData.playerYOffsets = array;
		mountData.xOffset = 5;
		mountData.bodyFrame = 3;
		mountData.yOffset = -1;
		mountData.playerHeadOffset = 34;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 7;
		mountData.runningFrameDelay = 15;
		mountData.runningFrameStart = 1;
		mountData.dashingFrameCount = 6;
		mountData.dashingFrameDelay = 40;
		mountData.dashingFrameStart = 9;
		mountData.flyingFrameCount = 6;
		mountData.flyingFrameDelay = 6;
		mountData.flyingFrameStart = 1;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 15;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = mountData.inAirFrameCount;
		mountData.swimFrameDelay = mountData.inAirFrameDelay;
		mountData.swimFrameStart = mountData.inAirFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.Extra[169];
			mountData.backTextureGlow = TextureAssets.GlowMask[284];
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[48] = mountData;
		mountData.spawnDust = 62;
		mountData.buff = 283;
		mountData.heightBoost = 14;
		mountData.flightTimeMax = 320;
		mountData.fallDamage = 0f;
		mountData.usesHover = true;
		mountData.runSpeed = 8f;
		mountData.dashSpeed = 8f;
		mountData.acceleration = 0.2f;
		mountData.jumpHeight = 5;
		mountData.jumpSpeed = 6f;
		mountData.swimSpeed = mountData.runSpeed;
		mountData.totalFrames = 6;
		array = new int[mountData.totalFrames];
		for (int num20 = 0; num20 < array.Length; num20++)
		{
			array[num20] = 9;
		}
		array[0] += 6;
		array[1] += 6;
		array[2] += 4;
		array[3] += 4;
		array[4] += 4;
		array[5] += 6;
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 0;
		mountData.yOffset = 16;
		mountData.playerHeadOffset = 16;
		mountData.runningFrameCount = 6;
		mountData.runningFrameDelay = 8;
		mountData.runningFrameStart = 0;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.Extra[170];
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[49] = mountData;
		mountData.spawnDust = 35;
		mountData.buff = 305;
		mountData.heightBoost = 8;
		mountData.runSpeed = 2f;
		mountData.dashSpeed = 1f;
		mountData.acceleration = 0.4f;
		mountData.jumpHeight = 4;
		mountData.jumpSpeed = 3f;
		mountData.swimSpeed = 14f;
		mountData.blockExtraJumps = true;
		mountData.flightTimeMax = 0;
		mountData.fatigueMax = 320;
		mountData.usesHover = true;
		mountData.emitsLight = true;
		mountData.lightColor = new Vector3(0.3f, 0.15f, 0.1f);
		mountData.totalFrames = 8;
		array = new int[mountData.totalFrames];
		for (int num21 = 0; num21 < array.Length; num21++)
		{
			array[num21] = 10;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 2;
		mountData.bodyFrame = 3;
		mountData.yOffset = 1;
		mountData.playerHeadOffset = 16;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 4;
		mountData.runningFrameCount = 4;
		mountData.runningFrameDelay = 14;
		mountData.runningFrameStart = 4;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 6;
		mountData.inAirFrameStart = 4;
		mountData.swimFrameCount = 4;
		mountData.swimFrameDelay = 16;
		mountData.swimFrameStart = 0;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.Extra[172];
			mountData.backTextureGlow = TextureAssets.GlowMask[285];
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[50] = mountData;
		mountData.spawnDust = 243;
		mountData.buff = 318;
		mountData.heightBoost = 20;
		mountData.flightTimeMax = 80;
		mountData.fallDamage = 0.5f;
		mountData.runSpeed = 5.5f;
		mountData.dashSpeed = 5.5f;
		mountData.acceleration = 0.2f;
		mountData.jumpHeight = 10;
		mountData.jumpSpeed = 7.25f;
		mountData.constantJump = true;
		mountData.totalFrames = 8;
		array = new int[mountData.totalFrames];
		for (int num22 = 0; num22 < array.Length; num22++)
		{
			array[num22] = 20;
		}
		array[1] += 2;
		array[4] += 2;
		array[5] += 2;
		mountData.playerYOffsets = array;
		mountData.xOffset = 1;
		mountData.bodyFrame = 3;
		mountData.yOffset = -1;
		mountData.playerHeadOffset = 22;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 5;
		mountData.runningFrameDelay = 16;
		mountData.runningFrameStart = 0;
		mountData.flyingFrameCount = 0;
		mountData.flyingFrameDelay = 0;
		mountData.flyingFrameStart = 0;
		mountData.inAirFrameCount = 1;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 5;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		if (Main.netMode != 2)
		{
			mountData.backTexture = TextureAssets.Extra[204];
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.backTexture.Width();
			mountData.textureHeight = mountData.backTexture.Height();
		}
		mountData = new MountData();
		mounts[52] = mountData;
		mountData.delegations = new MountDelegatesData();
		mountData.delegations.MouthPosition = DelegateMethods.Mount.WolfMouthPosition;
		mountData.delegations.HandPosition = DelegateMethods.Mount.NoPosition;
		mountData.spawnDust = 31;
		mountData.buff = 342;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.1f;
		mountData.runSpeed = 9.5f;
		mountData.acceleration = 0.18f;
		mountData.jumpHeight = 18;
		mountData.jumpSpeed = 9.01f;
		mountData.totalFrames = 15;
		array = new int[mountData.totalFrames];
		for (int num23 = 0; num23 < array.Length; num23++)
		{
			array[num23] = 0;
		}
		array[1] += -14;
		array[2] += -10;
		array[3] += -8;
		array[4] += 18;
		array[5] += -2;
		ref int reference2 = ref array[6];
		array[7] += 2;
		array[8] += 4;
		array[9] += 4;
		array[10] += 2;
		ref int reference3 = ref array[11];
		array[12] += 4;
		array[13] += 2;
		array[14] += -4;
		mountData.playerYOffsets = array;
		mountData.xOffset = 4;
		mountData.bodyFrame = 3;
		mountData.yOffset = -1;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 6;
		mountData.runningFrameDelay = 20;
		mountData.runningFrameStart = 5;
		mountData.inAirFrameCount = 3;
		mountData.inAirFrameDelay = 12;
		mountData.inAirFrameStart = 12;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = mountData.inAirFrameCount;
		mountData.swimFrameDelay = mountData.inAirFrameDelay;
		mountData.swimFrameStart = mountData.inAirFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.Extra[253];
			mountData.frontTextureExtra = TextureAssets.Extra[254];
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[54] = mountData;
		mountData.delegations = new MountDelegatesData();
		mountData.delegations.MouthPosition = DelegateMethods.Mount.VelociraptorMouthPosition;
		mountData.delegations.HandPosition = DelegateMethods.Mount.NoPosition;
		mountData.spawnDust = 31;
		mountData.buff = 370;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.1f;
		mountData.runSpeed = 4.5f;
		mountData.dashSpeed = 7.5f;
		mountData.acceleration = 0.15f;
		mountData.jumpHeight = 15;
		mountData.jumpSpeed = 6.01f;
		mountData.totalFrames = 21;
		array = new int[mountData.totalFrames];
		for (int num24 = 0; num24 < array.Length; num24++)
		{
			array[num24] = 0;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 4;
		mountData.bodyFrame = 0;
		mountData.yOffset = -4;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 7;
		mountData.runningFrameDelay = 20;
		mountData.runningFrameStart = 8;
		mountData.inAirFrameCount = 2;
		mountData.inAirFrameDelay = 6;
		mountData.inAirFrameStart = 5;
		mountData.flyingFrameCount = 5;
		mountData.flyingFrameDelay = 6;
		mountData.flyingFrameStart = 16;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = mountData.inAirFrameCount;
		mountData.swimFrameDelay = mountData.inAirFrameDelay;
		mountData.swimFrameStart = mountData.inAirFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.Extra[271];
			mountData.frontTextureExtra = TextureAssets.Extra[272];
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[55] = mountData;
		mountData.delegations = new MountDelegatesData();
		mountData.delegations.MouthPosition = DelegateMethods.Mount.NoPosition;
		mountData.delegations.HandPosition = DelegateMethods.Mount.NoPosition;
		mountData.delegations.PlayerSize = DelegateMethods.Mount.RatPlayerSize;
		mountData.spawnDust = 31;
		mountData.buff = 374;
		mountData.flightTimeMax = 0;
		mountData.fallDamage = 0.1f;
		mountData.dismountsOnItemUse = true;
		mountData.runSpeed = 4.5f;
		mountData.dashSpeed = 7.5f;
		mountData.acceleration = 0.15f;
		mountData.jumpHeight = 15;
		mountData.jumpSpeed = 6.01f;
		mountData.totalFrames = 16;
		array = new int[mountData.totalFrames];
		for (int num25 = 0; num25 < array.Length; num25++)
		{
			array[num25] = 0;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = 0;
		mountData.bodyFrame = 3;
		mountData.yOffset = -2;
		mountData.playerHeadOffset = -30;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 12;
		mountData.standingFrameStart = 0;
		mountData.runningFrameCount = 7;
		mountData.runningFrameDelay = 12;
		mountData.runningFrameStart = 4;
		mountData.dashingFrameCount = 5;
		mountData.dashingFrameDelay = 16;
		mountData.dashingFrameStart = 11;
		mountData.inAirFrameCount = 3;
		mountData.inAirFrameDelay = 6;
		mountData.inAirFrameStart = 12;
		mountData.idleFrameCount = 4;
		mountData.idleFrameDelay = 16;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.swimFrameCount = mountData.inAirFrameCount;
		mountData.swimFrameDelay = mountData.inAirFrameDelay;
		mountData.swimFrameStart = mountData.inAirFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.Extra[273];
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[56] = mountData;
		mountData.delegations = new MountDelegatesData();
		mountData.delegations.MouthPosition = DelegateMethods.Mount.NoPosition;
		mountData.delegations.HandPosition = DelegateMethods.Mount.NoPosition;
		mountData.delegations.PlayerSize = DelegateMethods.Mount.BatPlayerSize;
		mountData.delegations.DashDust = DelegateMethods.Mount.BatDashDust;
		mountData.spawnDust = 31;
		mountData.buff = 377;
		mountData.flightTimeMax = 320;
		mountData.fatigueMax = 320;
		mountData.fallDamage = 0f;
		mountData.usesHover = true;
		mountData.dismountsOnItemUse = true;
		mountData.blockExtraJumps = true;
		mountData.dashSpeed = 4.5f;
		mountData.runSpeed = mountData.dashSpeed;
		mountData.acceleration = 0.2f;
		mountData.jumpHeight = 8;
		mountData.jumpSpeed = 5f;
		mountData.totalFrames = 16;
		array = new int[mountData.totalFrames];
		for (int num26 = 0; num26 < array.Length; num26++)
		{
			array[num26] = 0;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = -4;
		mountData.bodyFrame = 3;
		mountData.yOffset = -1;
		mountData.playerHeadOffset = -24;
		mountData.standingFrameCount = 1;
		mountData.standingFrameDelay = 6;
		mountData.standingFrameStart = 0;
		mountData.dashingFrameCount = 6;
		mountData.dashingFrameDelay = 40;
		mountData.dashingFrameStart = 9;
		mountData.flyingFrameCount = 8;
		mountData.flyingFrameDelay = 5;
		mountData.flyingFrameStart = 1;
		mountData.idleFrameCount = 0;
		mountData.idleFrameDelay = 0;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = false;
		mountData.runningFrameCount = mountData.flyingFrameCount;
		mountData.runningFrameDelay = mountData.flyingFrameDelay;
		mountData.runningFrameStart = mountData.flyingFrameStart;
		mountData.inAirFrameCount = mountData.flyingFrameCount;
		mountData.inAirFrameDelay = mountData.flyingFrameDelay;
		mountData.inAirFrameStart = mountData.flyingFrameStart;
		mountData.swimFrameCount = mountData.flyingFrameCount;
		mountData.swimFrameDelay = mountData.flyingFrameDelay;
		mountData.swimFrameStart = mountData.flyingFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.Extra[274];
			mountData.frontTextureGlow = TextureAssets.GlowMask[370];
			mountData.frontTextureExtra = Asset<Texture2D>.Empty;
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[61] = mountData;
		mountData.delegations = new MountDelegatesData();
		mountData.delegations.MouthPosition = DelegateMethods.Mount.NoPosition;
		mountData.delegations.HandPosition = DelegateMethods.Mount.NoPosition;
		mountData.delegations.PlayerSize = DelegateMethods.Mount.PixiePlayerSize;
		mountData.delegations.DashDust = DelegateMethods.Mount.PixieDashDust;
		mountData.spawnDust = 43;
		mountData.spawnDustNoGravity = true;
		mountData.buff = 384;
		mountData.flightTimeMax = 320;
		mountData.fatigueMax = 320;
		mountData.fallDamage = 0f;
		mountData.usesHover = true;
		mountData.dismountsOnItemUse = true;
		mountData.blockExtraJumps = true;
		mountData.dashSpeed = 4.5f;
		mountData.runSpeed = mountData.dashSpeed;
		mountData.acceleration = 0.2f;
		mountData.jumpHeight = 8;
		mountData.jumpSpeed = 5f;
		mountData.totalFrames = 4;
		array = new int[mountData.totalFrames];
		for (int num27 = 0; num27 < array.Length; num27++)
		{
			array[num27] = 0;
		}
		mountData.playerYOffsets = array;
		mountData.xOffset = -2;
		mountData.yOffset = -1;
		mountData.bodyFrame = 0;
		mountData.playerHeadOffset = -24;
		mountData.standingFrameCount = 4;
		mountData.standingFrameDelay = 8;
		mountData.standingFrameStart = 0;
		mountData.dashingFrameCount = 4;
		mountData.dashingFrameDelay = 2;
		mountData.dashingFrameStart = 0;
		mountData.flyingFrameCount = 4;
		mountData.flyingFrameDelay = 6;
		mountData.flyingFrameStart = 0;
		mountData.idleFrameCount = 4;
		mountData.idleFrameDelay = 8;
		mountData.idleFrameStart = 0;
		mountData.idleFrameLoop = true;
		mountData.runningFrameCount = mountData.flyingFrameCount;
		mountData.runningFrameDelay = mountData.flyingFrameDelay;
		mountData.runningFrameStart = mountData.flyingFrameStart;
		mountData.inAirFrameCount = mountData.flyingFrameCount;
		mountData.inAirFrameDelay = mountData.flyingFrameDelay;
		mountData.inAirFrameStart = mountData.flyingFrameStart;
		mountData.swimFrameCount = mountData.flyingFrameCount;
		mountData.swimFrameDelay = mountData.flyingFrameDelay;
		mountData.swimFrameStart = mountData.flyingFrameStart;
		if (Main.netMode != 2)
		{
			mountData.backTexture = Asset<Texture2D>.Empty;
			mountData.backTextureExtra = Asset<Texture2D>.Empty;
			mountData.frontTexture = TextureAssets.Extra[290];
			mountData.frontTextureGlow = Asset<Texture2D>.Empty;
			mountData.frontTextureExtra = TextureAssets.Extra[291];
			mountData.textureWidth = mountData.frontTexture.Width();
			mountData.textureHeight = mountData.frontTexture.Height();
		}
		mountData = new MountData();
		mounts[57] = mountData;
		SetAsRollerSkate(mountData, 378);
		mountData = new MountData();
		mounts[58] = mountData;
		SetAsRollerSkate(mountData, 379);
		mountData = new MountData();
		mounts[59] = mountData;
		SetAsRollerSkate(mountData, 380);
		mountData = new MountData();
		mounts[60] = mountData;
		SetAsRollerSkate(mountData, 381);
		mountData = new MountData();
		mounts[62] = mountData;
		SetAsChillet(mountData, 387, TextureAssets.Extra[301], hardmode: false);
		mountData = new MountData();
		mounts[63] = mountData;
		SetAsChillet(mountData, 388, TextureAssets.Extra[302], hardmode: false);
		mountData = new MountData();
		mounts[64] = mountData;
		SetAsChillet(mountData, 391, TextureAssets.Extra[301], hardmode: true);
		mountData = new MountData();
		mounts[65] = mountData;
		SetAsChillet(mountData, 392, TextureAssets.Extra[302], hardmode: true);
	}

	public static void SetAsRollerSkate(MountData newMount, int buff)
	{
		newMount.spawnDust = 31;
		newMount.buff = buff;
		newMount.CanRideMinecartTracks = true;
		newMount.CanUseWings = true;
		newMount.MovementStatsAreAdditive = true;
		newMount.dashSpeed = 1f;
		newMount.runSpeed = newMount.dashSpeed;
		newMount.acceleration = 0.1f;
		newMount.jumpHeight = 2;
		newMount.jumpSpeed = 1f;
		newMount.fallDamage = 1f;
		newMount.blockExtraJumps = false;
		newMount.totalFrames = 1;
		int[] array = new int[newMount.totalFrames];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 2;
		}
		newMount.playerYOffsets = array;
		newMount.xOffset = 0;
		newMount.bodyFrame = 3;
		newMount.yOffset = 0;
		newMount.standingFrameCount = 1;
		newMount.standingFrameDelay = 6;
		newMount.standingFrameStart = 0;
		if (Main.netMode != 2)
		{
			newMount.backTexture = Asset<Texture2D>.Empty;
			newMount.backTextureExtra = Asset<Texture2D>.Empty;
			newMount.frontTexture = Asset<Texture2D>.Empty;
			newMount.frontTextureExtra = Asset<Texture2D>.Empty;
			newMount.textureWidth = 1;
			newMount.textureHeight = 1;
		}
	}

	public static void SetAsHorse(MountData newMount, int buff, Asset<Texture2D> texture)
	{
		newMount.spawnDust = 3;
		newMount.buff = buff;
		newMount.heightBoost = 34;
		newMount.flightTimeMax = 0;
		newMount.fallDamage = 0.5f;
		newMount.runSpeed = 3f;
		newMount.dashSpeed = 9f;
		newMount.acceleration = 0.25f;
		newMount.jumpHeight = 6;
		newMount.jumpSpeed = 7.01f;
		newMount.totalFrames = 16;
		int[] array = new int[newMount.totalFrames];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 28;
		}
		array[3] += 2;
		array[4] += 2;
		array[7] += 2;
		array[8] += 2;
		array[12] += 2;
		array[13] += 2;
		array[15] += 4;
		newMount.playerYOffsets = array;
		newMount.xOffset = 5;
		newMount.bodyFrame = 3;
		newMount.yOffset = 1;
		newMount.playerHeadOffset = 34;
		newMount.standingFrameCount = 1;
		newMount.standingFrameDelay = 12;
		newMount.standingFrameStart = 0;
		newMount.runningFrameCount = 7;
		newMount.runningFrameDelay = 15;
		newMount.runningFrameStart = 1;
		newMount.dashingFrameCount = 6;
		newMount.dashingFrameDelay = 40;
		newMount.dashingFrameStart = 9;
		newMount.flyingFrameCount = 6;
		newMount.flyingFrameDelay = 6;
		newMount.flyingFrameStart = 1;
		newMount.inAirFrameCount = 1;
		newMount.inAirFrameDelay = 12;
		newMount.inAirFrameStart = 15;
		newMount.idleFrameCount = 0;
		newMount.idleFrameDelay = 0;
		newMount.idleFrameStart = 0;
		newMount.idleFrameLoop = false;
		newMount.swimFrameCount = newMount.inAirFrameCount;
		newMount.swimFrameDelay = newMount.inAirFrameDelay;
		newMount.swimFrameStart = newMount.inAirFrameStart;
		if (Main.netMode != 2)
		{
			newMount.backTexture = texture;
			newMount.backTextureExtra = Asset<Texture2D>.Empty;
			newMount.frontTexture = Asset<Texture2D>.Empty;
			newMount.frontTextureExtra = Asset<Texture2D>.Empty;
			newMount.textureWidth = newMount.backTexture.Width();
			newMount.textureHeight = newMount.backTexture.Height();
		}
	}

	public static void SetAsChillet(MountData newMount, int buff, Asset<Texture2D> texture, bool hardmode)
	{
		newMount.spawnDust = 15;
		newMount.buff = buff;
		newMount.heightBoost = 4;
		newMount.flightTimeMax = 0;
		newMount.fallDamage = 0.5f;
		if (hardmode)
		{
			newMount.runSpeed = 3f;
			newMount.dashSpeed = 9f;
			newMount.acceleration = 0.32f;
			newMount.jumpHeight = 11;
			newMount.jumpSpeed = 8.1f;
		}
		else
		{
			newMount.runSpeed = 3f;
			newMount.dashSpeed = 6.5f;
			newMount.acceleration = 0.32f;
			newMount.jumpHeight = 6;
			newMount.jumpSpeed = 8.01f;
		}
		newMount.walkingGraceTimeMax = 10;
		newMount.totalFrames = 13;
		int[] array = new int[newMount.totalFrames];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 10;
		}
		array[4] += 2;
		array[5] += 6;
		array[6] += 2;
		ref int reference = ref array[7];
		array[8] += 2;
		array[9] += 6;
		array[10] += 6;
		array[11] += 4;
		newMount.playerYOffsets = array;
		newMount.xOffset = 7;
		newMount.bodyFrame = 3;
		newMount.yOffset = -10;
		newMount.playerHeadOffset = 10;
		newMount.standingFrameCount = 4;
		newMount.standingFrameDelay = 12;
		newMount.standingFrameStart = 0;
		newMount.runningFrameCount = 6;
		newMount.runningFrameDelay = 30;
		newMount.runningFrameStart = 6;
		newMount.dashingFrameCount = 7;
		newMount.dashingFrameDelay = 130;
		newMount.dashingFrameStart = 5;
		newMount.flyingFrameCount = 6;
		newMount.flyingFrameDelay = 6;
		newMount.flyingFrameStart = 1;
		newMount.inAirFrameCount = 1;
		newMount.inAirFrameDelay = 12;
		newMount.inAirFrameStart = 4;
		newMount.idleFrameCount = 4;
		newMount.idleFrameDelay = 4;
		newMount.idleFrameStart = 0;
		newMount.idleFrameLoop = true;
		newMount.swimFrameCount = newMount.inAirFrameCount;
		newMount.swimFrameDelay = newMount.inAirFrameDelay;
		newMount.swimFrameStart = newMount.inAirFrameStart;
		if (Main.netMode != 2)
		{
			newMount.backTexture = texture;
			newMount.backTextureExtra = Asset<Texture2D>.Empty;
			newMount.frontTexture = Asset<Texture2D>.Empty;
			newMount.frontTextureExtra = Asset<Texture2D>.Empty;
			newMount.textureWidth = newMount.backTexture.Width();
			newMount.textureHeight = newMount.backTexture.Height();
		}
	}

	public static void SetAsMinecart(MountData newMount, int buff, Asset<Texture2D> texture, int verticalOffset = 0, int playerVerticalOffset = 0)
	{
		newMount.Minecart = true;
		newMount.delegations = new MountDelegatesData();
		newMount.delegations.MinecartDust = DelegateMethods.Minecart.Sparks;
		newMount.spawnDust = 213;
		newMount.buff = buff;
		newMount.heightBoost = 10;
		newMount.flightTimeMax = 0;
		newMount.fallDamage = 1f;
		newMount.runSpeed = 13f;
		newMount.dashSpeed = 13f;
		newMount.acceleration = 0.04f;
		newMount.jumpHeight = 15;
		newMount.jumpSpeed = 5.15f;
		newMount.blockExtraJumps = true;
		newMount.totalFrames = 3;
		int[] array = new int[newMount.totalFrames];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 8 - verticalOffset + playerVerticalOffset;
		}
		newMount.playerYOffsets = array;
		newMount.xOffset = 1;
		newMount.bodyFrame = 3;
		newMount.yOffset = 13 + verticalOffset;
		newMount.playerHeadOffset = 14;
		newMount.standingFrameCount = 1;
		newMount.standingFrameDelay = 12;
		newMount.standingFrameStart = 0;
		newMount.runningFrameCount = 3;
		newMount.runningFrameDelay = 12;
		newMount.runningFrameStart = 0;
		newMount.flyingFrameCount = 0;
		newMount.flyingFrameDelay = 0;
		newMount.flyingFrameStart = 0;
		newMount.inAirFrameCount = 0;
		newMount.inAirFrameDelay = 0;
		newMount.inAirFrameStart = 0;
		newMount.idleFrameCount = 0;
		newMount.idleFrameDelay = 0;
		newMount.idleFrameStart = 0;
		newMount.idleFrameLoop = false;
		if (Main.netMode != 2)
		{
			newMount.backTexture = Asset<Texture2D>.Empty;
			newMount.backTextureExtra = Asset<Texture2D>.Empty;
			newMount.frontTexture = texture;
			newMount.frontTextureExtra = Asset<Texture2D>.Empty;
			newMount.textureWidth = newMount.frontTexture.Width();
			newMount.textureHeight = newMount.frontTexture.Height();
		}
	}

	public static int GetHeightBoost(int MountType)
	{
		if (MountType <= -1 || MountType >= MountID.Count)
		{
			return 0;
		}
		return mounts[MountType].heightBoost;
	}

	public int JumpHeight(float xVelocity)
	{
		int num = _data.jumpHeight;
		switch (_type)
		{
		case 0:
			num += (int)(Math.Abs(xVelocity) / 4f);
			break;
		case 1:
			num += (int)(Math.Abs(xVelocity) / 2.5f);
			break;
		case 4:
		case 49:
			if (_frameState == 4)
			{
				num += 5;
			}
			break;
		}
		if (_shouldSuperCart)
		{
			num = SuperCartJumpHeight;
		}
		return num;
	}

	public float JumpSpeed(float xVelocity)
	{
		float num = _data.jumpSpeed;
		switch (_type)
		{
		case 0:
		case 1:
			num += Math.Abs(xVelocity) / 7f;
			break;
		case 4:
		case 49:
			if (_frameState == 4)
			{
				num += 2.5f;
			}
			break;
		}
		if (_shouldSuperCart)
		{
			num = SuperCartJumpSpeed;
		}
		return num;
	}

	public bool CanFly(Player mountedPlayer)
	{
		if (!_active)
		{
			return false;
		}
		if (_type == 54 && ((SelectiveFlyingMountData)_mountSpecificData).allowedToFly)
		{
			return true;
		}
		if (_data.flightTimeMax == 0)
		{
			return false;
		}
		if (_type == 48)
		{
			return false;
		}
		return true;
	}

	public bool CanHover()
	{
		if (!_active || !_data.usesHover)
		{
			return false;
		}
		if (_type == 49)
		{
			return _frameState == 4;
		}
		if (!_data.usesHover)
		{
			return false;
		}
		return true;
	}

	public IEntitySource GetProjectileSpawnSource(Player mountedPlayer)
	{
		return new EntitySource_Mount(mountedPlayer, Type);
	}

	public void StartAbilityCharge(Player mountedPlayer)
	{
		if (Main.myPlayer == mountedPlayer.whoAmI)
		{
			int type = _type;
			if (type == 9)
			{
				int type2 = 441;
				float num = Main.screenPosition.X + (float)Main.mouseX;
				float num2 = Main.screenPosition.Y + (float)Main.mouseY;
				float ai = num - mountedPlayer.position.X;
				float ai2 = num2 - mountedPlayer.position.Y;
				Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), num, num2, 0f, 0f, type2, 0, 0f, mountedPlayer.whoAmI, ai, ai2);
				_abilityCharging = true;
			}
		}
		else
		{
			int type = _type;
			if (type == 9)
			{
				_abilityCharging = true;
			}
		}
	}

	public void StopAbilityCharge()
	{
		int type = _type;
		if (type == 9 || type == 46)
		{
			_abilityCharging = false;
			_abilityCooldown = _data.abilityCooldown;
			_abilityDuration = _data.abilityDuration;
		}
	}

	public bool CheckBuff(int buffID)
	{
		return _data.buff == buffID;
	}

	public void AbilityRecovery()
	{
		if (_abilityCharging)
		{
			if (_abilityCharge < _data.abilityChargeMax)
			{
				_abilityCharge++;
			}
		}
		else if (_abilityCharge > 0)
		{
			_abilityCharge--;
		}
		if (_abilityCooldown > 0)
		{
			_abilityCooldown--;
		}
		if (_abilityDuration > 0)
		{
			_abilityDuration--;
		}
	}

	public void FatigueRecovery()
	{
		if (_fatigue > 2f)
		{
			_fatigue -= 2f;
		}
		else
		{
			_fatigue = 0f;
		}
	}

	public bool Flight()
	{
		if (_flyTime <= 0)
		{
			return false;
		}
		_flyTime--;
		return true;
	}

	public void UpdateAfterEquips(Player mountedPlayer)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (!_active)
		{
			return;
		}
		if (_type == 54)
		{
			bool flag = mountedPlayer.wingsLogic > 0;
			((SelectiveFlyingMountData)_mountSpecificData).allowedToFly = flag;
			if (flag && mountedPlayer.empressBrooch)
			{
				_flyTime = mountedPlayer.wingTimeMax;
			}
		}
		if (_type != 55)
		{
			return;
		}
		mountedPlayer.spikedBoots++;
		mountedPlayer.fullRotationOrigin = mountedPlayer.Size / 2f;
		if (mountedPlayer.slideDir >= 1)
		{
			if ((float)Math.PI / 2f - Math.Abs(mountedPlayer.fullRotation) <= 0.1f)
			{
				mountedPlayer.fullRotation = -(float)Math.PI / 2f;
			}
			else
			{
				mountedPlayer.fullRotation = mountedPlayer.fullRotation.AngleLerp(-(float)Math.PI / 2f, 0.5f);
			}
		}
		else if (mountedPlayer.slideDir <= -1)
		{
			if ((float)Math.PI / 2f - Math.Abs(mountedPlayer.fullRotation) <= 0.1f)
			{
				mountedPlayer.fullRotation = (float)Math.PI / 2f;
			}
			else
			{
				mountedPlayer.fullRotation = mountedPlayer.fullRotation.AngleLerp((float)Math.PI / 2f, 0.5f);
			}
		}
		else if (Math.Abs(mountedPlayer.fullRotation) <= 0.1f)
		{
			mountedPlayer.fullRotation = 0f;
		}
		else
		{
			mountedPlayer.fullRotation = mountedPlayer.fullRotation.AngleLerp(0f, 0.5f);
		}
	}

	public void UpdateDrill(Player mountedPlayer, bool controlUp, bool controlDown)
	{
		DrillMountData drillMountData = (DrillMountData)_mountSpecificData;
		for (int i = 0; i < drillMountData.beams.Length; i++)
		{
			DrillBeam drillBeam = drillMountData.beams[i];
			if (drillBeam.cooldown > 1)
			{
				drillBeam.cooldown--;
			}
			else if (drillBeam.cooldown == 1)
			{
				drillBeam.cooldown = 0;
				drillBeam.curTileTarget = Point16.NegativeOne;
			}
		}
		drillMountData.diodeRotation = drillMountData.diodeRotation * 0.85f + 0.15f * drillMountData.diodeRotationTarget;
		if (drillMountData.beamCooldown > 0)
		{
			drillMountData.beamCooldown--;
		}
	}

	public void UseDrill(Player mountedPlayer)
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		if (_type != 8 || !_abilityActive)
		{
			return;
		}
		DrillMountData drillMountData = (DrillMountData)_mountSpecificData;
		bool flag = mountedPlayer.whoAmI == Main.myPlayer;
		if (mountedPlayer.controlUseItem)
		{
			for (int i = 0; i < amountOfBeamsAtOnce; i++)
			{
				if (drillMountData.beamCooldown != 0)
				{
					break;
				}
				for (int j = 0; j < drillMountData.beams.Length; j++)
				{
					DrillBeam drillBeam = drillMountData.beams[j];
					if (drillBeam.cooldown != 0)
					{
						continue;
					}
					Point16 point = DrillSmartCursor_Blocks(mountedPlayer, drillMountData);
					if (point == Point16.NegativeOne)
					{
						continue;
					}
					drillBeam.curTileTarget = point;
					int pickPower = drillPickPower;
					if (flag)
					{
						bool flag2 = true;
						if (WorldGen.InWorld(point.X, point.Y, 0) && Main.tile[point.X, point.Y] != null && Main.tile[point.X, point.Y].type == 26 && !Main.hardMode)
						{
							flag2 = false;
							mountedPlayer.Hurt(PlayerDeathReason.ByOther(4), mountedPlayer.statLife / 2, -mountedPlayer.direction);
						}
						if (mountedPlayer.noBuilding)
						{
							flag2 = false;
						}
						if (flag2)
						{
							mountedPlayer.PickTile(point.X, point.Y, pickPower);
						}
					}
					Vector2 val = new Vector2((float)(point.X << 4) + 8f, (float)(point.Y << 4) + 8f);
					float num = (val - mountedPlayer.Center).ToRotation();
					for (int k = 0; k < 2; k++)
					{
						float num2 = num + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
						float num3 = (float)Main.rand.NextDouble() * 2f + 2f;
						Vector2 val2 = new Vector2((float)Math.Cos(num2) * num3, (float)Math.Sin(num2) * num3);
						int num4 = Dust.NewDust(val, 0, 0, 230, val2.X, val2.Y);
						Main.dust[num4].noGravity = true;
						Main.dust[num4].customData = mountedPlayer;
					}
					if (flag)
					{
						Tile.SmoothSlope(point.X, point.Y, applyToNeighbors: true, sync: true);
					}
					drillBeam.cooldown = drillPickTime;
					drillBeam.lastPurpose = 0;
					break;
				}
			}
		}
		if (!mountedPlayer.controlUseTile)
		{
			return;
		}
		for (int l = 0; l < amountOfBeamsAtOnce; l++)
		{
			if (drillMountData.beamCooldown != 0)
			{
				break;
			}
			for (int m = 0; m < drillMountData.beams.Length; m++)
			{
				DrillBeam drillBeam2 = drillMountData.beams[m];
				if (drillBeam2.cooldown != 0)
				{
					continue;
				}
				Point16 point2 = DrillSmartCursor_Walls(mountedPlayer, drillMountData);
				if (point2 == Point16.NegativeOne)
				{
					continue;
				}
				drillBeam2.curTileTarget = point2;
				int damage = drillPickPower;
				if (flag)
				{
					bool flag3 = true;
					if (mountedPlayer.noBuilding)
					{
						flag3 = false;
					}
					if (flag3)
					{
						mountedPlayer.PickWall(point2.X, point2.Y, damage);
					}
				}
				Vector2 val3 = new Vector2((float)(point2.X << 4) + 8f, (float)(point2.Y << 4) + 8f);
				float num5 = (val3 - mountedPlayer.Center).ToRotation();
				for (int n = 0; n < 2; n++)
				{
					float num6 = num5 + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
					float num7 = (float)Main.rand.NextDouble() * 2f + 2f;
					Vector2 val4 = new Vector2((float)Math.Cos(num6) * num7, (float)Math.Sin(num6) * num7);
					int num8 = Dust.NewDust(val3, 0, 0, 230, val4.X, val4.Y);
					Main.dust[num8].noGravity = true;
					Main.dust[num8].customData = mountedPlayer;
				}
				drillBeam2.cooldown = drillPickTime;
				drillBeam2.lastPurpose = 1;
				break;
			}
		}
	}

	private Point16 DrillSmartCursor_Blocks(Player mountedPlayer, DrillMountData data)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = ((mountedPlayer.whoAmI != Main.myPlayer) ? data.crosshairPosition : (Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY)));
		Vector2 center = mountedPlayer.Center;
		Vector2 val2 = val - center;
		float num = val2.Length();
		if (num > 224f)
		{
			num = 224f;
		}
		num += 32f;
		val2.Normalize();
		Vector2 end = center + val2 * num;
		Point16 tilePoint = new Point16(-1, -1);
		if (!Utils.PlotTileLine(center, end, 65.6f, (int x, int y) =>
		{
			tilePoint = new Point16(x, y);
			for (int i = 0; i < data.beams.Length; i++)
			{
				if (data.beams[i].curTileTarget == tilePoint && data.beams[i].lastPurpose == 0)
				{
					return true;
				}
			}
			if (!WorldGen.CanKillTile(x, y))
			{
				return true;
			}
			return (Main.tile[x, y] == null || Main.tile[x, y].inActive() || !Main.tile[x, y].active()) ? true : false;
		}))
		{
			return tilePoint;
		}
		return new Point16(-1, -1);
	}

	private Point16 DrillSmartCursor_Walls(Player mountedPlayer, DrillMountData data)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = ((mountedPlayer.whoAmI != Main.myPlayer) ? data.crosshairPosition : (Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY)));
		Vector2 center = mountedPlayer.Center;
		Vector2 val2 = val - center;
		float num = val2.Length();
		if (num > 224f)
		{
			num = 224f;
		}
		num += 32f;
		num += 16f;
		val2.Normalize();
		Vector2 end = center + val2 * num;
		Point16 tilePoint = new Point16(-1, -1);
		if (!Utils.PlotTileLine(center, end, 97.6f, (int x, int y) =>
		{
			tilePoint = new Point16(x, y);
			for (int i = 0; i < data.beams.Length; i++)
			{
				if (data.beams[i].curTileTarget == tilePoint && data.beams[i].lastPurpose == 1)
				{
					return true;
				}
			}
			Tile tile = Main.tile[x, y];
			if (tile == null)
			{
				return false;
			}
			return (tile.wall <= 0 || !Player.CanPlayerSmashWall(x, y)) ? true : false;
		}))
		{
			return tilePoint;
		}
		return new Point16(-1, -1);
	}

	public void UseAbility(Player mountedPlayer, Vector2 mousePosition, bool toggleOn)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		switch (_type)
		{
		case 9:
		{
			if (Main.myPlayer != mountedPlayer.whoAmI)
			{
				break;
			}
			int type2 = 606;
			mousePosition = ClampToDeadZone(mountedPlayer, mousePosition);
			Vector2 val5 = default;
			val5.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
			val5.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
			int num3 = (_frameExtra - 6) * 2;
			Vector2 val6 = default;
			for (int i = 0; i < 2; i++)
			{
				val6.Y = val5.Y + scutlixEyePositions[num3 + i].Y + (float)_data.yOffset;
				if (mountedPlayer.direction == -1)
				{
					val6.X = val5.X - scutlixEyePositions[num3 + i].X - (float)_data.xOffset;
				}
				else
				{
					val6.X = val5.X + scutlixEyePositions[num3 + i].X + (float)_data.xOffset;
				}
				Vector2 val7 = mousePosition - val6;
				val7.Normalize();
				val7 *= 14f;
				int damage3 = 150;
				val6 += val7;
				Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), val6.X, val6.Y, val7.X, val7.Y, type2, damage3, 0f, Main.myPlayer);
			}
			break;
		}
		case 46:
			if (Main.myPlayer == mountedPlayer.whoAmI)
			{
				if (_abilityCooldown <= 10)
				{
					int damage = 120;
					Vector2 val = mountedPlayer.Center + new Vector2((float)(mountedPlayer.width * -mountedPlayer.direction), 26f);
					Vector2 val2 = new Vector2(0f, -4f).RotatedByRandom(0.10000000149011612);
					Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), val.X, val.Y, val2.X, val2.Y, 930, damage, 0f, Main.myPlayer);
					SoundEngine.PlaySound(SoundID.Item89.SoundId, (int)val.X, (int)val.Y, SoundID.Item89.Style, 0.2f);
				}
				int type = 14;
				int damage2 = 100;
				mousePosition = ClampToDeadZone(mountedPlayer, mousePosition);
				Vector2 val3 = default;
				val3.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
				val3.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
				Vector2 val4 = new Vector2(val3.X + (float)(mountedPlayer.width * mountedPlayer.direction), val3.Y - 12f);
				Vector2 v = mousePosition - val4;
				v = v.SafeNormalize(Vector2.Zero);
				v *= 12f;
				v = v.RotatedByRandom(0.20000000298023224);
				Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), val4.X, val4.Y, v.X, v.Y, type, damage2, 0f, Main.myPlayer);
				SoundEngine.PlaySound(SoundID.Item11.SoundId, (int)val4.X, (int)val4.Y, SoundID.Item11.Style, 0.2f);
			}
			break;
		case 8:
			if (Main.myPlayer == mountedPlayer.whoAmI)
			{
				if (!toggleOn)
				{
					_abilityActive = false;
				}
				else if (!_abilityActive)
				{
					if (mountedPlayer.whoAmI == Main.myPlayer)
					{
						float num = Main.screenPosition.X + (float)Main.mouseX;
						float num2 = Main.screenPosition.Y + (float)Main.mouseY;
						float ai = num - mountedPlayer.position.X;
						float ai2 = num2 - mountedPlayer.position.Y;
						Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), num, num2, 0f, 0f, 453, 0, 0f, mountedPlayer.whoAmI, ai, ai2);
					}
					_abilityActive = true;
				}
			}
			else
			{
				_abilityActive = toggleOn;
			}
			break;
		}
	}

	public bool Hover(Player mountedPlayer)
	{
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		bool flag = DoesHoverIgnoresFatigue();
		bool flag2 = _frameState == 2 || _frameState == 4;
		if (_type == 49)
		{
			flag2 = _frameState == 4;
		}
		if (_type == 56)
		{
			flag2 = _frameState == 2 || _frameState == 3;
		}
		if (_type == 61)
		{
			flag2 = _frameState == 2 || _frameState == 3 || _frameState == 4;
		}
		if (flag2)
		{
			bool flag3 = true;
			float num = 1f;
			float num2 = mountedPlayer.gravity / Player.defaultGravity;
			if (mountedPlayer.slowFall)
			{
				num2 /= 3f;
			}
			if (num2 < 0.25f)
			{
				num2 = 0.25f;
			}
			if (!flag)
			{
				if (_flyTime > 0)
				{
					_flyTime--;
				}
				else if (_fatigue < _fatigueMax)
				{
					_fatigue += num2;
				}
				else
				{
					flag3 = false;
				}
			}
			if (_type == 12 && !mountedPlayer.MountFishronSpecial)
			{
				num = 0.5f;
			}
			float num3 = _fatigue / _fatigueMax;
			if (flag)
			{
				num3 = 0f;
			}
			bool flag4 = true;
			if (_type == 48)
			{
				flag4 = false;
			}
			float num4 = 4f * num3;
			float num5 = 4f * num3;
			bool flag5 = false;
			if (_type == 48)
			{
				num4 = 0f;
				num5 = 0f;
				if (!flag3)
				{
					flag5 = true;
				}
				if (mountedPlayer.controlDown)
				{
					num5 = 8f;
				}
			}
			if (num4 == 0f)
			{
				num4 = -0.001f;
			}
			if (num5 == 0f)
			{
				num5 = -0.001f;
			}
			float num6 = mountedPlayer.velocity.Y;
			if ((flag4 && (mountedPlayer.controlUp || mountedPlayer.controlJump)) & flag3)
			{
				num4 = -2f - 6f * (1f - num3);
				if (_type == 48)
				{
					num4 /= 3f;
				}
				if (_type == 56 || _type == 61)
				{
					num4 = 0f - _data.dashSpeed;
				}
				num6 -= _data.acceleration * num;
			}
			else if (mountedPlayer.controlDown)
			{
				num5 = 8f;
				if (_type == 56 || _type == 61)
				{
					num5 = _data.dashSpeed;
				}
				num6 += _data.acceleration * num;
			}
			else if (flag5)
			{
				float num7 = mountedPlayer.gravity * mountedPlayer.gravDir;
				num6 += num7;
				num5 = 4f;
			}
			else
			{
				_ = mountedPlayer.jump;
			}
			if (num6 < num4)
			{
				num6 = ((!(num4 - num6 < _data.acceleration)) ? (num6 + _data.acceleration * num) : num4);
			}
			else if (num6 > num5)
			{
				num6 = ((!(num6 - num5 < _data.acceleration)) ? (num6 - _data.acceleration * num) : num5);
			}
			if (_type == 56 || _type == 61)
			{
				if (num4 != -0.001f)
				{
					num6 = MathHelper.Max(num6, num4);
				}
				if (num5 != -0.001f)
				{
					num6 = MathHelper.Min(num6, num5);
				}
			}
			mountedPlayer.velocity.Y = num6;
			if (num4 == -0.001f && num5 == -0.001f && num6 == -0.001f)
			{
				mountedPlayer.position.Y -= -0.001f;
				TryStabilizingSmallMountPositionBetweenSlopes(mountedPlayer);
			}
			mountedPlayer.fallStart = (int)(mountedPlayer.position.Y / 16f);
		}
		else if (!flag)
		{
			mountedPlayer.velocity.Y += mountedPlayer.gravity * mountedPlayer.gravDir;
		}
		else if (mountedPlayer.velocity.Y == 0f)
		{
			Vector2 velocity = Vector2.UnitY * mountedPlayer.gravDir * 1f;
			if (Collision.TileCollision(mountedPlayer.position, velocity, mountedPlayer.width, mountedPlayer.height, fallThrough: false, fall2: false, (int)mountedPlayer.gravDir).Y != 0f || mountedPlayer.controlDown)
			{
				mountedPlayer.velocity.Y = 0.001f;
			}
		}
		else if (mountedPlayer.velocity.Y == -0.001f)
		{
			mountedPlayer.velocity.Y -= -0.001f;
		}
		if (_type == 7)
		{
			float num8 = mountedPlayer.velocity.X / _data.dashSpeed;
			if ((double)num8 > 0.95)
			{
				num8 = 0.95f;
			}
			if ((double)num8 < -0.95)
			{
				num8 = -0.95f;
			}
			float fullRotation = (float)Math.PI / 4f * num8 / 2f;
			float num9 = Math.Abs(2f - (float)_frame / 2f) / 2f;
			Lighting.AddLight((int)(mountedPlayer.position.X + (float)(mountedPlayer.width / 2)) / 16, (int)(mountedPlayer.position.Y + (float)(mountedPlayer.height / 2)) / 16, 0.4f, 0.2f * num9, 0f);
			mountedPlayer.fullRotation = fullRotation;
		}
		else if (_type == 8)
		{
			float num10 = mountedPlayer.velocity.X / _data.dashSpeed;
			if ((double)num10 > 0.95)
			{
				num10 = 0.95f;
			}
			if ((double)num10 < -0.95)
			{
				num10 = -0.95f;
			}
			float fullRotation2 = (float)Math.PI / 4f * num10 / 2f;
			mountedPlayer.fullRotation = fullRotation2;
			DrillMountData drillMountData = (DrillMountData)_mountSpecificData;
			float outerRingRotation = drillMountData.outerRingRotation;
			outerRingRotation += mountedPlayer.velocity.X / 80f;
			if (outerRingRotation > (float)Math.PI)
			{
				outerRingRotation -= (float)Math.PI * 2f;
			}
			else if (outerRingRotation < -(float)Math.PI)
			{
				outerRingRotation += (float)Math.PI * 2f;
			}
			drillMountData.outerRingRotation = outerRingRotation;
		}
		else if (_type == 23)
		{
			float num11 = (0f - mountedPlayer.velocity.Y) / _data.dashSpeed;
			num11 = MathHelper.Clamp(num11, -1f, 1f);
			float num12 = mountedPlayer.velocity.X / _data.dashSpeed;
			num12 = MathHelper.Clamp(num12, -1f, 1f);
			float num13 = -(float)Math.PI / 16f * num11 * (float)mountedPlayer.direction;
			float num14 = (float)Math.PI / 16f * num12;
			float fullRotation3 = num13 + num14;
			mountedPlayer.fullRotation = fullRotation3;
			mountedPlayer.fullRotationOrigin = new Vector2((float)(mountedPlayer.width / 2), (float)mountedPlayer.height);
		}
		return true;
	}

	private static void TryStabilizingSmallMountPositionBetweenSlopes(Player mountedPlayer)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (mountedPlayer.height < 42)
		{
			Vector4 vec = Collision.SlopeCollision(mountedPlayer.position, mountedPlayer.velocity, mountedPlayer.width, mountedPlayer.height);
			mountedPlayer.position = vec.XY();
		}
	}

	private bool DoesHoverIgnoresFatigue()
	{
		if (_type != 7 && _type != 8 && _type != 12 && _type != 23 && _type != 44 && _type != 49 && _type != 56)
		{
			return _type == 61;
		}
		return true;
	}

	private float GetWitchBroomTrinketRotation(Player player)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		float num = Utils.Clamp(player.velocity.X / 10f, -1f, 1f);
		float num2 = 0f;
		Point val = player.Center.ToTileCoordinates();
		float num3 = 0.5f;
		if (WorldGen.InAPlaceWithWind(val.X, val.Y, 1, 1))
		{
			num3 = 1f;
		}
		num2 = (float)Math.Sin((float)player.miscCounter / 300f * ((float)Math.PI * 2f) * 3f) * ((float)Math.PI / 4f) * Math.Abs(Main.WindForVisuals) * 0.5f + (float)Math.PI / 4f * (0f - Main.WindForVisuals) * 0.5f;
		num2 *= num3;
		return num * (float)Math.Sin((float)player.miscCounter / 150f * ((float)Math.PI * 2f) * 3f) * ((float)Math.PI / 4f) * 0.5f + num * ((float)Math.PI / 4f) * 0.5f + num2;
	}

	private Vector2 GetWitchBroomTrinketOriginOffset(Player player)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(29 * player.direction), -4f);
	}

	public void UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_129d: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_21bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_22bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_229c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1330: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_134e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f21: Unknown result type (might be due to invalid IL or missing references)
		//IL_226c: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1102: Unknown result type (might be due to invalid IL or missing references)
		//IL_110c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1111: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_1495: Unknown result type (might be due to invalid IL or missing references)
		//IL_14af: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14db: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1509: Unknown result type (might be due to invalid IL or missing references)
		//IL_150e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1513: Unknown result type (might be due to invalid IL or missing references)
		//IL_151c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1521: Unknown result type (might be due to invalid IL or missing references)
		//IL_1523: Unknown result type (might be due to invalid IL or missing references)
		//IL_1528: Unknown result type (might be due to invalid IL or missing references)
		//IL_152d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1548: Unknown result type (might be due to invalid IL or missing references)
		//IL_154d: Unknown result type (might be due to invalid IL or missing references)
		//IL_136c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1387: Unknown result type (might be due to invalid IL or missing references)
		//IL_138d: Unknown result type (might be due to invalid IL or missing references)
		//IL_139b: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_158e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1594: Unknown result type (might be due to invalid IL or missing references)
		//IL_159e: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15df: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_142e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1440: Unknown result type (might be due to invalid IL or missing references)
		//IL_1445: Unknown result type (might be due to invalid IL or missing references)
		//IL_144a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1458: Unknown result type (might be due to invalid IL or missing references)
		//IL_146a: Unknown result type (might be due to invalid IL or missing references)
		//IL_146f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1025: Unknown result type (might be due to invalid IL or missing references)
		//IL_102a: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_091d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_0940: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_204b: Unknown result type (might be due to invalid IL or missing references)
		if (_frameState != state)
		{
			if (_type == 56 && ((_frameState == 5 && (mountedPlayer.dash <= 0 || mountedPlayer.dashDelay >= 0)) || (_frameState != 5 && mountedPlayer.dash > 0 && mountedPlayer.dashDelay < 0)))
			{
				_frameCounter = 0f;
			}
			_frameState = state;
			bool flag = true;
			if (_type == 48 && (state == 1 || state == 2))
			{
				flag = false;
			}
			if (_type == 56 && (state == 1 || state == 4 || state == 2))
			{
				flag = false;
			}
			if (_type == 61 && (state == 1 || state == 4 || state == 2))
			{
				flag = false;
			}
			if (_type == 55 && (state == 1 || state == 5 || state == 4 || (state == 2 && mountedPlayer.sliding)))
			{
				flag = false;
			}
			if (flag)
			{
				_frameCounter = 0f;
			}
		}
		if (state != 0)
		{
			_idleTime = 0;
		}
		if (mountedPlayer.isDisplayDollOrInanimate)
		{
			_idleTime = 0;
		}
		if (velocity.Y == 0f)
		{
			_walkingGraceTimeLeft = _data.walkingGraceTimeMax;
		}
		else if (_walkingGraceTimeLeft > 0)
		{
			_walkingGraceTimeLeft--;
		}
		if (mountedPlayer.justJumped || (mountedPlayer.controlDown && mountedPlayer.velocity.Y > 0f))
		{
			_walkingGraceTimeLeft = 0;
		}
		if (_data.emitsLight)
		{
			Point val = mountedPlayer.Center.ToTileCoordinates();
			Lighting.AddLight(val.X, val.Y, _data.lightColor.X, _data.lightColor.Y, _data.lightColor.Z);
		}
		Color newColor;
		switch (_type)
		{
		case 61:
		{
			Point val10 = mountedPlayer.Center.ToTileCoordinates();
			newColor = Projectile.GetFairyQueenWeaponsColorFull(mountedPlayer.whoAmI, mountedPlayer.Center, 0.41f, 1f, 0.1f);
			Vector3 val11 = newColor.ToVector3() * 0.55f;
			Lighting.AddLight(val10.X, val10.Y, val11.X, val11.Y, val11.Z);
			if (_frameState == 4)
			{
				_frameState = (state = 2);
			}
			break;
		}
		case 55:
			if (mountedPlayer.sliding)
			{
				_frameState = (state = 1);
				break;
			}
			switch (state)
			{
			case 4:
				_frameState = (state = 2);
				break;
			case 1:
			case 5:
				if (Math.Abs(mountedPlayer.velocity.X) > 4f)
				{
					_frameState = (state = 5);
				}
				break;
			}
			break;
		case 56:
			if (mountedPlayer.dash > 0 && mountedPlayer.dashDelay < 0)
			{
				_frameState = (state = 5);
			}
			else if (state != 0 && state != 1)
			{
				_frameState = (state = 2);
			}
			break;
		case 52:
			if (state == 4)
			{
				state = 2;
			}
			break;
		case 54:
			UpdateFrame_Velociraptor(mountedPlayer, ref state);
			break;
		case 17:
			UpdateFrame_GolfCart(mountedPlayer, state, velocity);
			break;
		case 5:
			if (state != 2)
			{
				_frameExtra = 0;
				_frameExtraCounter = 0f;
			}
			break;
		case 7:
			state = 2;
			break;
		case 49:
		{
			if (state != 4 && mountedPlayer.wet)
			{
				_frameState = (state = 4);
			}
			velocity.Length();
			float num4 = mountedPlayer.velocity.Y * 0.05f;
			if (mountedPlayer.direction < 0)
			{
				num4 *= -1f;
			}
			mountedPlayer.fullRotation = num4;
			mountedPlayer.fullRotationOrigin = new Vector2((float)(mountedPlayer.width / 2), (float)(mountedPlayer.height / 2));
			break;
		}
		case 43:
			if (mountedPlayer.velocity.Y == 0f)
			{
				mountedPlayer.isPerformingPogostickTricks = false;
			}
			if (mountedPlayer.isPerformingPogostickTricks)
			{
				mountedPlayer.fullRotation += (float)mountedPlayer.direction * ((float)Math.PI * 2f) / 30f;
			}
			else
			{
				mountedPlayer.fullRotation = (float)Math.Sign(mountedPlayer.velocity.X) * Utils.GetLerpValue(0f, RunSpeed - 0.2f, Math.Abs(mountedPlayer.velocity.X), clamped: true) * 0.4f;
			}
			mountedPlayer.fullRotationOrigin = new Vector2((float)(mountedPlayer.width / 2), (float)mountedPlayer.height * 0.8f);
			break;
		case 9:
			if (_aiming)
			{
				break;
			}
			_frameExtraCounter++;
			if (_frameExtraCounter >= 12f)
			{
				_frameExtraCounter = 0f;
				_frameExtra++;
				if (_frameExtra >= 6)
				{
					_frameExtra = 0;
				}
			}
			break;
		case 46:
			if (state != 0)
			{
				state = 1;
			}
			if (!_aiming)
			{
				if (state == 0)
				{
					_frameExtra = 12;
					_frameExtraCounter = 0f;
					break;
				}
				if (_frameExtra < 12)
				{
					_frameExtra = 12;
				}
				_frameExtraCounter += Math.Abs(velocity.X);
				if (_frameExtraCounter >= 8f)
				{
					_frameExtraCounter = 0f;
					_frameExtra++;
					if (_frameExtra >= 24)
					{
						_frameExtra = 12;
					}
				}
				break;
			}
			if (_frameExtra < 24)
			{
				_frameExtra = 24;
			}
			_frameExtraCounter++;
			if (_frameExtraCounter >= 3f)
			{
				_frameExtraCounter = 0f;
				_frameExtra++;
				if (_frameExtra >= 27)
				{
					_frameExtra = 24;
				}
			}
			break;
		case 8:
		{
			if (state != 0 && state != 1)
			{
				break;
			}
			Vector2 val6 = default;
			val6.X = mountedPlayer.position.X;
			val6.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
			int num10 = (int)(val6.X / 16f);
			_ = val6.Y / 16f;
			float num11 = 0f;
			float num12 = mountedPlayer.width;
			while (num12 > 0f)
			{
				float num13 = (float)((num10 + 1) * 16) - val6.X;
				if (num13 > num12)
				{
					num13 = num12;
				}
				num11 += Collision.GetTileRotation(val6) * num13;
				num12 -= num13;
				val6.X += num13;
				num10++;
			}
			float num14 = num11 / (float)mountedPlayer.width - mountedPlayer.fullRotation;
			float num15 = 0f;
			float num16 = (float)Math.PI / 20f;
			if (num14 < 0f)
			{
				num15 = ((!(num14 > 0f - num16)) ? (0f - num16) : num14);
			}
			else if (num14 > 0f)
			{
				num15 = ((!(num14 < num16)) ? num16 : num14);
			}
			if (num15 != 0f)
			{
				mountedPlayer.fullRotation += num15;
				if (mountedPlayer.fullRotation > (float)Math.PI / 4f)
				{
					mountedPlayer.fullRotation = (float)Math.PI / 4f;
				}
				if (mountedPlayer.fullRotation < -(float)Math.PI / 4f)
				{
					mountedPlayer.fullRotation = -(float)Math.PI / 4f;
				}
			}
			break;
		}
		case 10:
		case 40:
		case 41:
		case 42:
		case 47:
		{
			bool flag7 = Math.Abs(velocity.X) > DashSpeed - RunSpeed / 2f;
			if (state == 1)
			{
				bool flag8 = false;
				if (flag7)
				{
					state = 5;
					if (_frameExtra < 6)
					{
						flag8 = true;
					}
					_frameExtra++;
				}
				else
				{
					_frameExtra = 0;
				}
				if ((_type == 10 || _type == 47) & flag8)
				{
					int num21 = 6;
					if (_type == 10)
					{
						num21 = Utils.SelectRandom<int>(Main.rand, 176, 177, 179);
					}
					Vector2 val12 = mountedPlayer.Center + new Vector2((float)(mountedPlayer.width * mountedPlayer.direction), 0f);
					Vector2 val13 = new Vector2(40f, 30f);
					float num22 = (float)Math.PI * 2f * Main.rand.NextFloat();
					for (float num23 = 0f; num23 < 14f; num23++)
					{
						Dust[] dust11 = Main.dust;
						int type = num21;
						newColor = default;
						Dust dust12 = dust11[Dust.NewDust(val12, 0, 0, type, 0f, 0f, 0, newColor)];
						Vector2 val14 = Vector2.UnitY.RotatedBy(num23 * ((float)Math.PI * 2f) / 14f + num22);
						val14 *= 0.2f * (float)_frameExtra;
						dust12.position = val12 + val14 * val13;
						dust12.velocity = val14 + new Vector2(RunSpeed - (float)(Math.Sign(velocity.X) * _frameExtra * 2), 0f);
						dust12.noGravity = true;
						if (_type == 47)
						{
							dust12.noLightEmittance = true;
						}
						dust12.scale = 1f + Main.rand.NextFloat() * 0.8f;
						dust12.fadeIn = Main.rand.NextFloat() * 2f;
						dust12.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
					}
				}
			}
			if ((_type == 10) & flag7)
			{
				Dust[] dust13 = Main.dust;
				Vector2 position3 = mountedPlayer.position;
				int width5 = mountedPlayer.width;
				int height2 = mountedPlayer.height;
				int type2 = Utils.SelectRandom<int>(Main.rand, 176, 177, 179);
				newColor = default;
				Dust obj3 = dust13[Dust.NewDust(position3, width5, height2, type2, 0f, 0f, 0, newColor)];
				obj3.velocity = Vector2.Zero;
				obj3.noGravity = true;
				obj3.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
				obj3.fadeIn = 1f + Main.rand.NextFloat() * 2f;
				obj3.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
			}
			if (((_type == 47) & flag7) && velocity.Y == 0f)
			{
				int num24 = (int)mountedPlayer.Center.X / 16;
				int num25 = (int)(mountedPlayer.position.Y + (float)mountedPlayer.height - 1f) / 16;
				Tile tile = Main.tile[num24, num25 + 1];
				if (tile != null && tile.active() && tile.liquid == 0 && WorldGen.SolidTileAllowBottomSlope(num24, num25 + 1))
				{
					ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.WallOfFleshGoatMountFlames, new ParticleOrchestraSettings
					{
						PositionInWorld = new Vector2((float)(num24 * 16 + 8), (float)(num25 * 16 + 16))
					}, mountedPlayer.whoAmI);
				}
			}
			break;
		}
		case 44:
		{
			state = 1;
			bool flag3 = Math.Abs(velocity.X) > DashSpeed - RunSpeed / 4f;
			if (_mountSpecificData == null)
			{
				_mountSpecificData = false;
			}
			bool flag4 = (bool)_mountSpecificData;
			if (flag4 && !flag3)
			{
				_mountSpecificData = false;
			}
			else if (!flag4 & flag3)
			{
				_mountSpecificData = true;
				Vector2 val3 = mountedPlayer.Center + new Vector2((float)(mountedPlayer.width * mountedPlayer.direction), 0f);
				Vector2 val4 = new Vector2(40f, 30f);
				float num5 = (float)Math.PI * 2f * Main.rand.NextFloat();
				for (float num6 = 0f; num6 < 20f; num6++)
				{
					Dust[] dust4 = Main.dust;
					newColor = default;
					Dust dust5 = dust4[Dust.NewDust(val3, 0, 0, 228, 0f, 0f, 0, newColor)];
					Vector2 val5 = Vector2.UnitY.RotatedBy(num6 * ((float)Math.PI * 2f) / 20f + num5);
					val5 *= 0.8f;
					dust5.position = val3 + val5 * val4;
					dust5.velocity = val5 + new Vector2(RunSpeed - (float)Math.Sign(velocity.Length()), 0f);
					if (velocity.X > 0f)
					{
						dust5.velocity.X *= -1f;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust5.velocity *= 0.5f;
					}
					dust5.noGravity = true;
					dust5.scale = 1.5f + Main.rand.NextFloat() * 0.8f;
					dust5.fadeIn = 0f;
					dust5.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
				}
			}
			int num7 = (int)RunSpeed - (int)Math.Abs(velocity.X);
			if (num7 <= 0)
			{
				num7 = 1;
			}
			if (Main.rand.Next(num7) == 0)
			{
				int num8 = 22;
				int num9 = mountedPlayer.width / 2 + 10;
				Vector2 bottom = mountedPlayer.Bottom;
				bottom.X -= (float)num9;
				bottom.Y -= num8 - 6;
				Dust[] dust6 = Main.dust;
				Vector2 position = bottom;
				int width3 = num9 * 2;
				newColor = default;
				Dust obj2 = dust6[Dust.NewDust(position, width3, num8, 228, 0f, 0f, 0, newColor)];
				obj2.velocity = Vector2.Zero;
				obj2.noGravity = true;
				obj2.noLight = true;
				obj2.scale = 0.25f + Main.rand.NextFloat() * 0.8f;
				obj2.fadeIn = 0.5f + Main.rand.NextFloat() * 2f;
				obj2.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
			}
			break;
		}
		case 45:
		{
			bool flag5 = Math.Abs(velocity.X) > DashSpeed * 0.9f;
			if (_mountSpecificData == null)
			{
				_mountSpecificData = false;
			}
			bool flag6 = (bool)_mountSpecificData;
			if (flag6 && !flag5)
			{
				_mountSpecificData = false;
			}
			else if (!flag6 & flag5)
			{
				_mountSpecificData = true;
				Vector2 val7 = mountedPlayer.Center + new Vector2((float)(mountedPlayer.width * mountedPlayer.direction), 0f);
				Vector2 val8 = new Vector2(40f, 30f);
				float num17 = (float)Math.PI * 2f * Main.rand.NextFloat();
				for (float num18 = 0f; num18 < 20f; num18++)
				{
					Dust[] dust7 = Main.dust;
					newColor = default;
					Dust dust8 = dust7[Dust.NewDust(val7, 0, 0, 6, 0f, 0f, 0, newColor)];
					Vector2 val9 = Vector2.UnitY.RotatedBy(num18 * ((float)Math.PI * 2f) / 20f + num17);
					val9 *= 0.8f;
					dust8.position = val7 + val9 * val8;
					dust8.velocity = val9 + new Vector2(RunSpeed - (float)Math.Sign(velocity.Length()), 0f);
					if (velocity.X > 0f)
					{
						dust8.velocity.X *= -1f;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust8.velocity *= 0.5f;
					}
					dust8.noGravity = true;
					dust8.scale = 1.5f + Main.rand.NextFloat() * 0.8f;
					dust8.fadeIn = 0f;
					dust8.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
				}
			}
			if (flag5)
			{
				int num19 = Utils.SelectRandom(Main.rand, new short[3] { 6, 6, 31 });
				int num20 = 6;
				Dust[] dust9 = Main.dust;
				Vector2 position2 = mountedPlayer.Center - new Vector2((float)num20, (float)(num20 - 12));
				int width4 = num20 * 2;
				int height = num20 * 2;
				newColor = default;
				Dust dust10 = dust9[Dust.NewDust(position2, width4, height, num19, 0f, 0f, 0, newColor)];
				dust10.velocity = mountedPlayer.velocity * 0.1f;
				if (Main.rand.Next(2) == 0)
				{
					dust10.noGravity = true;
				}
				dust10.scale = 0.7f + Main.rand.NextFloat() * 0.8f;
				if (Main.rand.Next(3) == 0)
				{
					dust10.fadeIn = 0.1f;
				}
				if (num19 == 31)
				{
					dust10.noGravity = true;
					dust10.scale *= 1.5f;
					dust10.fadeIn = 0.2f;
				}
				dust10.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
			}
			break;
		}
		case 48:
			state = 1;
			break;
		case 39:
			_frameExtraCounter++;
			if (_frameExtraCounter > 6f)
			{
				_frameExtraCounter = 0f;
				_frameExtra++;
				if (_frameExtra > 5)
				{
					_frameExtra = 0;
				}
			}
			break;
		case 50:
			if (mountedPlayer.velocity.Y == 0f)
			{
				_frameExtraCounter = 0f;
				_frameExtra = 3;
				break;
			}
			_frameExtraCounter++;
			if (_flyTime > 0)
			{
				_frameExtraCounter++;
			}
			if (_frameExtraCounter > 7f)
			{
				_frameExtraCounter = 0f;
				_frameExtra++;
				if (_frameExtra > 3)
				{
					_frameExtra = 0;
				}
			}
			break;
		case 14:
		{
			bool flag2 = Math.Abs(velocity.X) > RunSpeed / 2f;
			float num = Math.Sign(mountedPlayer.velocity.X);
			float num2 = 12f;
			float num3 = 40f;
			if (!flag2)
			{
				mountedPlayer.basiliskCharge = 0f;
			}
			else
			{
				mountedPlayer.basiliskCharge = Utils.Clamp(mountedPlayer.basiliskCharge + 1f / 180f, 0f, 1f);
			}
			if ((double)mountedPlayer.position.Y > Main.worldSurface * 16.0 + 160.0)
			{
				Lighting.AddLight(mountedPlayer.Center, 0.5f, 0.1f, 0.1f);
			}
			if (flag2 && velocity.Y == 0f)
			{
				for (int i = 0; i < 2; i++)
				{
					Dust[] dust = Main.dust;
					Vector2 bottomLeft = mountedPlayer.BottomLeft;
					int width = mountedPlayer.width;
					newColor = default;
					Dust obj = dust[Dust.NewDust(bottomLeft, width, 6, 31, 0f, 0f, 0, newColor)];
					obj.velocity = new Vector2(velocity.X * 0.15f, Main.rand.NextFloat() * -2f);
					obj.noLight = true;
					obj.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
					obj.fadeIn = 0.5f + Main.rand.NextFloat() * 1f;
					obj.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
				}
				if (mountedPlayer.cMount == 0)
				{
					mountedPlayer.position += new Vector2(num * 24f, 0f);
					mountedPlayer.FloorVisuals(Falling: true);
					mountedPlayer.position -= new Vector2(num * 24f, 0f);
				}
			}
			if (num != (float)mountedPlayer.direction)
			{
				break;
			}
			for (int j = 0; j < (int)(3f * mountedPlayer.basiliskCharge); j++)
			{
				Dust[] dust2 = Main.dust;
				Vector2 bottomLeft2 = mountedPlayer.BottomLeft;
				int width2 = mountedPlayer.width;
				newColor = default;
				Dust dust3 = dust2[Dust.NewDust(bottomLeft2, width2, 6, 6, 0f, 0f, 0, newColor)];
				Vector2 val2 = mountedPlayer.Center + new Vector2(num * num3, num2);
				dust3.position = mountedPlayer.Center + new Vector2(num * (num3 - 2f), num2 - 6f + Main.rand.NextFloat() * 12f);
				dust3.velocity = (dust3.position - val2).SafeNormalize(Vector2.Zero) * (3.5f + Main.rand.NextFloat() * 0.5f);
				if (dust3.velocity.Y < 0f)
				{
					dust3.velocity.Y *= 1f + 2f * Main.rand.NextFloat();
				}
				dust3.velocity += mountedPlayer.velocity * 0.55f;
				dust3.velocity *= mountedPlayer.velocity.Length() / RunSpeed;
				dust3.velocity *= mountedPlayer.basiliskCharge;
				dust3.noGravity = true;
				dust3.noLight = true;
				dust3.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
				dust3.fadeIn = 0.5f + Main.rand.NextFloat() * 1f;
				dust3.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
			}
			break;
		}
		}
		switch (state)
		{
		case 0:
			if (_data.idleFrameCount != 0)
			{
				if (_type == 5)
				{
					if (_fatigue != 0f)
					{
						if (_idleTime == 0)
						{
							_idleTimeNext = _idleTime + 1;
						}
					}
					else
					{
						_idleTime = 0;
						_idleTimeNext = 2;
					}
				}
				else if (_idleTime == 0)
				{
					_idleTimeNext = Main.rand.Next(900, 1500);
					if (_type == 17)
					{
						_idleTimeNext = Main.rand.Next(120, 300);
					}
					if (_type == 2)
					{
						_idleTimeNext = Main.rand.Next(600, 900);
					}
					if (_type == 55)
					{
						_idleTimeNext = Main.rand.Next(120, 420);
					}
					if (_type == 62 || _type == 63 || _type == 64 || _type == 65)
					{
						_idleTimeNext = Main.rand.Next(900, 1500);
					}
				}
				_idleTime++;
			}
			_frameCounter++;
			if (_data.idleFrameCount != 0 && _idleTime >= _idleTimeNext)
			{
				float num34 = _data.idleFrameDelay;
				if (_type == 5)
				{
					num34 *= 2f - 1f * _fatigue / _fatigueMax;
				}
				int idleFrameCount = _data.idleFrameCount;
				if (_type == 55)
				{
					int num35 = (int)(((float)_idleTime - (float)_idleTimeNext) / num34 / (float)idleFrameCount * (float)idleFrames_Rat.Length);
					if (num35 >= idleFrames_Rat.Length)
					{
						_frameCounter = 0f;
						_frame = _data.standingFrameStart;
						_idleTime = 0;
					}
					else
					{
						_frame = _data.idleFrameStart + idleFrames_Rat[num35];
					}
					break;
				}
				int num36 = (int)((float)(_idleTime - _idleTimeNext) / num34);
				if (num36 >= idleFrameCount)
				{
					if (_data.idleFrameLoop)
					{
						_idleTime = _idleTimeNext;
						_frame = _data.idleFrameStart;
						if ((_type == 62 || _type == 63 || _type == 64 || _type == 65) && Main.rand.Next(20) == 0)
						{
							_frameCounter = 0f;
							_frame = _data.standingFrameStart;
							_idleTime = 0;
						}
					}
					else
					{
						_frameCounter = 0f;
						_frame = _data.standingFrameStart;
						_idleTime = 0;
					}
				}
				else
				{
					_frame = _data.idleFrameStart + num36;
					if (_data.idleFrameLoop)
					{
						if (_frame < _data.idleFrameStart || _frame >= _data.idleFrameStart + _data.idleFrameCount)
						{
							_frame = _data.idleFrameStart;
						}
					}
					else if (_frame < _data.idleFrameStart || _frame >= _data.idleFrameStart + _data.idleFrameCount)
					{
						_frame = _data.standingFrameStart;
					}
				}
				if (_type == 5)
				{
					_frameExtra = _frame;
				}
				if (_type == 17)
				{
					_frameExtra = _frame;
					_frame = 0;
				}
			}
			else
			{
				if (_frameCounter > (float)_data.standingFrameDelay)
				{
					_frameCounter -= _data.standingFrameDelay;
					_frame++;
				}
				if (_frame < _data.standingFrameStart || _frame >= _data.standingFrameStart + _data.standingFrameCount)
				{
					_frame = _data.standingFrameStart;
				}
			}
			break;
		case 1:
		{
			float num26;
			switch (_type)
			{
			case 9:
			case 46:
				num26 = ((!_flipDraw) ? Math.Abs(velocity.X) : (0f - Math.Abs(velocity.X)));
				break;
			case 44:
				num26 = Math.Max(1f, Math.Abs(velocity.X) * 0.25f);
				break;
			case 48:
				num26 = Math.Max(0.5f, velocity.Length() * 0.125f);
				break;
			case 50:
				num26 = Math.Abs(velocity.X) * 0.5f;
				break;
			case 55:
				num26 = ((!mountedPlayer.sliding) ? Math.Abs(velocity.X) : velocity.Length());
				break;
			case 56:
				num26 = MathHelper.Clamp(velocity.Length() * 0.5f, 1f, 2f);
				break;
			default:
				num26 = Math.Abs(velocity.X);
				break;
			}
			_frameCounter += num26;
			if (num26 >= 0f)
			{
				if (_frameCounter > (float)_data.runningFrameDelay)
				{
					_frameCounter -= _data.runningFrameDelay;
					_frame++;
				}
				if (_frame < _data.runningFrameStart || _frame >= _data.runningFrameStart + _data.runningFrameCount)
				{
					_frame = _data.runningFrameStart;
				}
			}
			else
			{
				if (_frameCounter < 0f)
				{
					_frameCounter += _data.runningFrameDelay;
					_frame--;
				}
				if (_frame < _data.runningFrameStart || _frame >= _data.runningFrameStart + _data.runningFrameCount)
				{
					_frame = _data.runningFrameStart + _data.runningFrameCount - 1;
				}
			}
			break;
		}
		case 3:
		{
			float num32 = 1f;
			if (_type == 56 || _type == 61)
			{
				num32 = MathHelper.Clamp(velocity.Length() * 0.5f, 1f, 2f);
			}
			_frameCounter += num32;
			int num33 = _data.flyingFrameDelay;
			if (Type == 54 && _flyTime > 0)
			{
				num33 -= 2;
			}
			if (_frameCounter > (float)num33)
			{
				_frameCounter -= num33;
				_frame++;
			}
			if (_frame < _data.flyingFrameStart || _frame >= _data.flyingFrameStart + _data.flyingFrameCount)
			{
				_frame = _data.flyingFrameStart;
			}
			break;
		}
		case 2:
		{
			float num27 = 1f;
			if (_type == 56 || _type == 61)
			{
				num27 = MathHelper.Clamp(velocity.Length() * 0.5f, 1f, 2f);
			}
			int frame = _frame;
			_frameCounter += num27;
			if (_frameCounter > (float)_data.inAirFrameDelay)
			{
				_frameCounter -= _data.inAirFrameDelay;
				_frame++;
			}
			if (_frame < _data.inAirFrameStart || _frame >= _data.inAirFrameStart + _data.inAirFrameCount)
			{
				_frame = _data.inAirFrameStart;
			}
			if (_type == 62 || _type == 63 || _type == 64 || _type == 65)
			{
				int num28 = _data.inAirFrameDelay - 1;
				if (frame < 4)
				{
					_frameCounter = num28;
				}
				num28 = 6;
				if (_frameCounter < (float)num28)
				{
					_frame = 5;
				}
				else
				{
					_frameCounter = num28;
				}
			}
			if (_type == 4)
			{
				if (velocity.Y < 0f)
				{
					_frame = 3;
				}
				else
				{
					_frame = 6;
				}
			}
			else if (_type == 52 || _type == 55)
			{
				if (velocity.Y < 0f)
				{
					_frame = _data.inAirFrameStart;
				}
				if (_frame == _data.inAirFrameStart + _data.inAirFrameCount - 1)
				{
					_frameCounter = 0f;
				}
			}
			else if (_type == 54)
			{
				if (mountedPlayer.grappling[0] >= 0)
				{
					if (velocity.Length() > 0.01f)
					{
						if (_frame == _data.inAirFrameStart + _data.inAirFrameCount - 1)
						{
							_frameCounter = 0f;
						}
					}
					else
					{
						_frame = 7;
					}
				}
				else if (velocity.Y < 0f)
				{
					if (_frame == _data.inAirFrameStart + _data.inAirFrameCount - 1)
					{
						_frameCounter = 0f;
					}
				}
				else
				{
					_frame = 7;
				}
			}
			else if (_type == 5)
			{
				float num29 = _fatigue / _fatigueMax;
				_frameExtraCounter += 6f - 4f * num29;
				if (_frameExtraCounter > (float)_data.flyingFrameDelay)
				{
					_frameExtra++;
					_frameExtraCounter -= _data.flyingFrameDelay;
				}
				if (_frameExtra < _data.flyingFrameStart || _frameExtra >= _data.flyingFrameStart + _data.flyingFrameCount)
				{
					_frameExtra = _data.flyingFrameStart;
				}
			}
			else if (_type == 23)
			{
				float num30 = mountedPlayer.velocity.Length();
				if (num30 < 1f)
				{
					_frame = 0;
					_frameCounter = 0f;
				}
				else if (num30 > 5f)
				{
					_frameCounter++;
				}
			}
			break;
		}
		case 4:
		{
			int num31 = (int)((Math.Abs(velocity.X) + Math.Abs(velocity.Y)) / 2f);
			_frameCounter += num31;
			if (_frameCounter > (float)_data.swimFrameDelay)
			{
				_frameCounter -= _data.swimFrameDelay;
				_frame++;
			}
			if (_frame < _data.swimFrameStart || _frame >= _data.swimFrameStart + _data.swimFrameCount)
			{
				_frame = _data.swimFrameStart;
			}
			if (Type == 37 && velocity.X == 0f)
			{
				_frame = 4;
			}
			break;
		}
		case 5:
		{
			int type3 = _type;
			float num26;
			if (type3 != 9)
			{
				num26 = Math.Abs(velocity.X);
			}
			else
			{
				num26 = ((!_flipDraw) ? Math.Abs(velocity.X) : (0f - Math.Abs(velocity.X)));
			}
			_frameCounter += num26;
			if (num26 >= 0f)
			{
				if (_frameCounter > (float)_data.dashingFrameDelay)
				{
					_frameCounter -= _data.dashingFrameDelay;
					_frame++;
				}
				if (_frame < _data.dashingFrameStart || _frame >= _data.dashingFrameStart + _data.dashingFrameCount)
				{
					_frame = _data.dashingFrameStart;
				}
			}
			else
			{
				if (_frameCounter < 0f)
				{
					_frameCounter += _data.dashingFrameDelay;
					_frame--;
				}
				if (_frame < _data.dashingFrameStart || _frame >= _data.dashingFrameStart + _data.dashingFrameCount)
				{
					_frame = _data.dashingFrameStart + _data.dashingFrameCount - 1;
				}
			}
			break;
		}
		}
		if ((_type == 62 || _type == 63 || _type == 64 || _type == 65) && mountedPlayer.dashDelay < 0 && mountedPlayer.dashDelay >= -5)
		{
			_frame = 5;
		}
	}

	public void UpdateFrame_Velociraptor(Player mountedPlayer, ref int state)
	{
		SelectiveFlyingMountData selectiveFlyingMountData = (SelectiveFlyingMountData)_mountSpecificData;
		if (state == 4)
		{
			state = (selectiveFlyingMountData.allowedToFly ? 3 : 2);
		}
		bool num = state == 2 || state == 3;
		int num2 = ((mountedPlayer.wings > 0) ? mountedPlayer.wings : mountedPlayer.wingsLogic);
		if (num && selectiveFlyingMountData.allowedToFly && ((num2 > 0 && !ArmorIDs.Wing.Sets.AlwaysAnimated[num2]) || mountedPlayer.ShouldDrawWingsThatAreAlwaysAnimated(ignoreMounts: true)))
		{
			state = 3;
		}
		else if (state == 3)
		{
			state = 2;
		}
	}

	public void TryBeginningFlight(Player mountedPlayer, int state)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		if (_frameState == state || mountedPlayer.tongued || (state != 2 && state != 3) || !CanHover() || mountedPlayer.controlUp || mountedPlayer.controlDown || mountedPlayer.controlJump)
		{
			return;
		}
		Vector2 velocity = Vector2.UnitY * mountedPlayer.gravDir;
		if (Collision.TileCollision(mountedPlayer.position + new Vector2(0f, -0.001f), velocity, mountedPlayer.width, mountedPlayer.height, fallThrough: false, fall2: false, (int)mountedPlayer.gravDir).Y != 0f)
		{
			if (DoesHoverIgnoresFatigue())
			{
				mountedPlayer.position.Y += -0.001f;
				return;
			}
			float num = mountedPlayer.gravity * mountedPlayer.gravDir;
			mountedPlayer.position.Y -= mountedPlayer.velocity.Y;
			mountedPlayer.velocity.Y -= num;
		}
	}

	public int GetIntendedGroundedFrame(Player mountedPlayer)
	{
		bool num = MountID.Sets.CanUseHooks[Type] && mountedPlayer.grappling[0] >= 0;
		bool flag = mountedPlayer.velocity.X == 0f || ((mountedPlayer.slippy || mountedPlayer.slippy2 || mountedPlayer.windPushed) && !mountedPlayer.controlLeft && !mountedPlayer.controlRight);
		if (num)
		{
			return 2;
		}
		if (flag)
		{
			return 0;
		}
		return 1;
	}

	public void TryLanding(Player mountedPlayer)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		if ((_frameState == 3 || _frameState == 2) && !mountedPlayer.controlUp && !mountedPlayer.controlDown && !mountedPlayer.controlJump)
		{
			Vector2 velocity = Vector2.UnitY * mountedPlayer.gravDir * 4f;
			if (Collision.TileCollision(mountedPlayer.position, velocity, mountedPlayer.width, mountedPlayer.height, fallThrough: false, fall2: false, (int)mountedPlayer.gravDir).Y == 0f)
			{
				UpdateFrame(mountedPlayer, GetIntendedGroundedFrame(mountedPlayer), mountedPlayer.velocity);
			}
		}
	}

	private void UpdateFrame_GolfCart(Player mountedPlayer, int state, Vector2 velocity)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		if (state != 2)
		{
			if (_frameExtraCounter != 0f || _frameExtra != 0)
			{
				if (_frameExtraCounter == -1f)
				{
					_frameExtraCounter = 0f;
					_frameExtra = 1;
				}
				if (++_frameExtraCounter >= 6f)
				{
					_frameExtraCounter = 0f;
					if (_frameExtra > 0)
					{
						_frameExtra--;
					}
				}
			}
			else
			{
				_frameExtra = 0;
				_frameExtraCounter = 0f;
			}
		}
		else if (velocity.Y >= 0f)
		{
			if (_frameExtra < 1)
			{
				_frameExtra = 1;
			}
			if (_frameExtra == 2)
			{
				_frameExtraCounter = -1f;
			}
			else if (++_frameExtraCounter >= 6f)
			{
				_frameExtraCounter = 0f;
				if (_frameExtra < 2)
				{
					_frameExtra++;
				}
			}
		}
		if (state != 2 && state != 0 && state != 3 && state != 4)
		{
			EmitGolfCartWheelDust(mountedPlayer, mountedPlayer.Bottom + new Vector2((float)(mountedPlayer.direction * -20), 0f));
			EmitGolfCartWheelDust(mountedPlayer, mountedPlayer.Bottom + new Vector2((float)(mountedPlayer.direction * 20), 0f));
		}
		EmitGolfCartlight(mountedPlayer.Bottom + new Vector2((float)(mountedPlayer.direction * 40), -20f), mountedPlayer.direction);
	}

	private static void EmitGolfCartSmoke(Player mountedPlayer, bool rushing)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = mountedPlayer.Bottom + new Vector2((float)(-mountedPlayer.direction * 34), (0f - mountedPlayer.gravDir) * 12f);
		Dust dust = Dust.NewDustDirect(position, 0, 0, 31, -mountedPlayer.direction, (0f - mountedPlayer.gravDir) * 0.24f, 100);
		dust.position = position;
		dust.velocity *= 0.1f;
		dust.velocity += new Vector2((float)(-mountedPlayer.direction), (0f - mountedPlayer.gravDir) * 0.25f);
		dust.scale = 0.5f;
		if (mountedPlayer.velocity.X != 0f)
		{
			dust.velocity += new Vector2((float)Math.Sign(mountedPlayer.velocity.X) * 1.3f, 0f);
		}
		if (rushing)
		{
			dust.fadeIn = 0.8f;
		}
	}

	private static void EmitGolfCartlight(Vector2 worldLocation, int playerDirection)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		if (playerDirection == -1)
		{
			num = (float)Math.PI;
		}
		float num2 = (float)Math.PI / 32f;
		int num3 = 5;
		float num4 = 200f;
		DelegateMethods.v2_1 = worldLocation.ToTileCoordinates().ToVector2();
		DelegateMethods.f_1 = num4 / 16f;
		DelegateMethods.v3_1 = new Vector3(0.7f, 0.7f, 0.7f);
		for (float num5 = 0f; num5 < (float)num3; num5++)
		{
			Vector2 val = (num + num2 * (num5 - (float)(num3 / 2))).ToRotationVector2();
			Utils.PlotTileLine(worldLocation, worldLocation + val * num4, 8f, DelegateMethods.CastLightOpen_StopForSolids_ScaleWithDistance);
		}
	}

	private static bool ShouldGolfCartEmitLight()
	{
		return true;
	}

	private static void EmitGolfCartWheelDust(Player mountedPlayer, Vector2 legSpot)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		if (Main.rand.Next(5) != 0)
		{
			return;
		}
		Point val = (legSpot + new Vector2(0f, mountedPlayer.gravDir * 2f)).ToTileCoordinates();
		if (!WorldGen.InWorld(val.X, val.Y, 10))
		{
			return;
		}
		Tile tileSafely = Framing.GetTileSafely(val.X, val.Y);
		if (WorldGen.SolidTile(val))
		{
			int num = WorldGen.KillTile_GetTileDustAmount(fail: true, tileSafely);
			if (num > 1)
			{
				num = 1;
			}
			Vector2 val2 = new Vector2((float)(-mountedPlayer.direction), (0f - mountedPlayer.gravDir) * 1f);
			for (int i = 0; i < num; i++)
			{
				Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(val.X, val.Y, tileSafely)];
				obj.velocity *= 0.2f;
				obj.velocity += val2;
				obj.position = legSpot;
				obj.scale *= 0.8f;
				obj.fadeIn *= 0.8f;
			}
		}
	}

	private void DoGemMinecartEffect(Player mountedPlayer, int dustType)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		if (Main.rand.Next(10) == 0)
		{
			Vector2 val = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(22f, 10f);
			Vector2 val2 = new Vector2(0f, 10f) * mountedPlayer.Directions;
			Vector2 pos = mountedPlayer.Center + val2 + val;
			pos = mountedPlayer.RotatedRelativePoint(pos);
			Dust dust = Dust.NewDustPerfect(pos, dustType);
			dust.noGravity = true;
			dust.fadeIn = 0.6f;
			dust.scale = 0.4f;
			dust.velocity *= 0.25f;
			dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
		}
	}

	private void DoSteamMinecartEffect(Player mountedPlayer, int dustType)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		float num = Math.Abs(mountedPlayer.velocity.X);
		if (!(num < 1f) && (!(num < 6f) || _frame == 0))
		{
			Vector2 val = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(3f, 3f);
			Vector2 val2 = new Vector2(-10f, -4f) * mountedPlayer.Directions;
			Vector2 pos = mountedPlayer.Center + val2 + val;
			pos = mountedPlayer.RotatedRelativePoint(pos);
			Dust dust = Dust.NewDustPerfect(pos, dustType);
			dust.noGravity = true;
			dust.fadeIn = 0.6f;
			dust.scale = 1.8f;
			dust.velocity *= 0.25f;
			dust.velocity.Y -= 2f;
			dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
		}
	}

	private void DoExhaustMinecartEffect(Player mountedPlayer, int dustType)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		float num = mountedPlayer.velocity.Length();
		if (num < 1f && Main.rand.Next(4) != 0)
		{
			return;
		}
		int num2 = 1 + (int)num / 6;
		while (num2 > 0)
		{
			num2--;
			Vector2 val = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(3f, 3f);
			Vector2 val2 = new Vector2(-18f, 20f) * mountedPlayer.Directions;
			if (num > 6f)
			{
				val2.X += 4 * mountedPlayer.direction;
			}
			if (num2 > 0)
			{
				val2 += mountedPlayer.velocity * (float)(num2 / 3);
			}
			Vector2 pos = mountedPlayer.Center + val2 + val;
			pos = mountedPlayer.RotatedRelativePoint(pos);
			Dust dust = Dust.NewDustPerfect(pos, dustType);
			dust.noGravity = true;
			dust.fadeIn = 0.6f;
			dust.scale = 1.2f;
			dust.velocity *= 0.2f;
			if (num < 1f)
			{
				dust.velocity.X -= 0.5f * (float)mountedPlayer.direction;
			}
			dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
		}
	}

	private void DoConfettiMinecartEffect(Player mountedPlayer)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		float num = mountedPlayer.velocity.Length();
		if ((num < 1f && Main.rand.Next(6) != 0) || (num < 3f && Main.rand.Next(3) != 0))
		{
			return;
		}
		int num2 = 1 + (int)num / 6;
		while (num2 > 0)
		{
			num2--;
			float num3 = Main.rand.NextFloat() * 2f;
			Vector2 val = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(3f, 8f);
			Vector2 val2 = new Vector2(-18f, 4f) * mountedPlayer.Directions;
			val2.X += num * (float)mountedPlayer.direction * 0.5f + (float)(mountedPlayer.direction * num2) * num3;
			if (num2 > 0)
			{
				val2 += mountedPlayer.velocity * (float)(num2 / 3);
			}
			Vector2 pos = mountedPlayer.Center + val2 + val;
			pos = mountedPlayer.RotatedRelativePoint(pos);
			Dust dust = Dust.NewDustPerfect(pos, 139 + Main.rand.Next(4));
			dust.noGravity = true;
			dust.fadeIn = 0.6f;
			dust.scale = 0.5f + num3 / 2f;
			dust.velocity *= 0.2f;
			if (num < 1f)
			{
				dust.velocity.X -= 0.5f * (float)mountedPlayer.direction;
			}
			dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
		}
	}

	public void UpdateEffects(Player mountedPlayer)
	{
		//IL_0949: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ceb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f38: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09db: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_0867: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Unknown result type (might be due to invalid IL or missing references)
		//IL_100a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1013: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Unknown result type (might be due to invalid IL or missing references)
		//IL_1039: Unknown result type (might be due to invalid IL or missing references)
		//IL_1054: Unknown result type (might be due to invalid IL or missing references)
		//IL_1056: Unknown result type (might be due to invalid IL or missing references)
		//IL_1062: Unknown result type (might be due to invalid IL or missing references)
		//IL_1076: Unknown result type (might be due to invalid IL or missing references)
		//IL_1085: Unknown result type (might be due to invalid IL or missing references)
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_108f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0def: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_109b: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1109: Unknown result type (might be due to invalid IL or missing references)
		//IL_110e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1113: Unknown result type (might be due to invalid IL or missing references)
		//IL_111b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1130: Unknown result type (might be due to invalid IL or missing references)
		//IL_113f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1149: Unknown result type (might be due to invalid IL or missing references)
		//IL_114e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		mountedPlayer.autoJump = AutoJump;
		_shouldSuperCart = MountID.Sets.Cart[_type] && mountedPlayer.UsingSuperCart;
		if (_shouldSuperCart)
		{
			CastSuperCartLaser(mountedPlayer);
			float num = 1f + Math.Abs(mountedPlayer.velocity.X) / RunSpeed * 2.5f;
			mountedPlayer.statDefense += (int)(2f * num);
		}
		Color newColor;
		switch (_type)
		{
		case 62:
		case 63:
		case 64:
		case 65:
			mountedPlayer.meleeDamage += 0.1f;
			mountedPlayer.rangedDamage += 0.1f;
			mountedPlayer.magicDamage += 0.1f;
			mountedPlayer.minionDamage += 0.1f;
			break;
		case 23:
		{
			Vector2 pos3 = mountedPlayer.Center + GetWitchBroomTrinketOriginOffset(mountedPlayer) + (GetWitchBroomTrinketRotation(mountedPlayer) + (float)Math.PI / 2f).ToRotationVector2() * 11f;
			Vector3 rgb = new Vector3(1f, 0.75f, 0.5f) * 0.85f;
			Vector2 val13 = mountedPlayer.RotatedRelativePoint(pos3);
			Lighting.AddLight(val13, rgb);
			if (Main.rand.Next(45) == 0)
			{
				Vector2 val14 = Main.rand.NextVector2Circular(4f, 4f);
				Dust dust5 = Dust.NewDustPerfect(val13 + val14, 43, Vector2.Zero, 254, new Color(255, 255, 0, 255), 0.3f);
				if (val14 != Vector2.Zero)
				{
					dust5.velocity = val13.DirectionTo(dust5.position) * 0.2f;
				}
				dust5.fadeIn = 0.3f;
				dust5.noLightEmittance = true;
				dust5.customData = mountedPlayer;
				dust5.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
			}
			float num13 = 0.1f;
			num13 += mountedPlayer.velocity.Length() / 30f;
			Vector2 pos4 = mountedPlayer.Center + new Vector2(18f - 20f * Main.rand.NextFloat() * (float)mountedPlayer.direction, 12f);
			Vector2 pos5 = mountedPlayer.Center + new Vector2((float)(52 * mountedPlayer.direction), -6f);
			pos4 = mountedPlayer.RotatedRelativePoint(pos4);
			pos5 = mountedPlayer.RotatedRelativePoint(pos5);
			if (!(Main.rand.NextFloat() <= num13))
			{
				break;
			}
			float num14 = Main.rand.NextFloat();
			for (float num15 = 0f; num15 < 1f; num15 += 0.125f)
			{
				if (Main.rand.Next(15) == 0)
				{
					Vector2 spinningpoint = ((float)Math.PI * 2f * num15 + num14).ToRotationVector2() * new Vector2(0.5f, 1f) * 4f;
					spinningpoint = spinningpoint.RotatedBy(mountedPlayer.fullRotation);
					Dust dust6 = Dust.NewDustPerfect(pos4 + spinningpoint, 43, Vector2.Zero, 254, new Color(255, 255, 0, 255), 0.3f);
					dust6.velocity = spinningpoint * 0.025f + pos5.DirectionTo(dust6.position) * 0.5f;
					dust6.fadeIn = 0.3f;
					dust6.noLightEmittance = true;
					dust6.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
				}
			}
			break;
		}
		case 25:
			DoGemMinecartEffect(mountedPlayer, 86);
			break;
		case 26:
			DoGemMinecartEffect(mountedPlayer, 87);
			break;
		case 27:
			DoGemMinecartEffect(mountedPlayer, 88);
			break;
		case 28:
			DoGemMinecartEffect(mountedPlayer, 89);
			break;
		case 29:
			DoGemMinecartEffect(mountedPlayer, 90);
			break;
		case 30:
			DoGemMinecartEffect(mountedPlayer, 91);
			break;
		case 31:
			DoGemMinecartEffect(mountedPlayer, 262);
			break;
		case 9:
		case 46:
		{
			if (_type == 46)
			{
				mountedPlayer.hasJumpOption_Santank = true;
			}
			Vector2 center = mountedPlayer.Center;
			Vector2 val = center;
			bool flag2 = false;
			float num2 = 1500f;
			float num3 = 850f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC nPC = Main.npc[i];
				if (!nPC.CanBeChasedBy(this))
				{
					continue;
				}
				Vector2 v = nPC.Center - center;
				float num4 = v.Length();
				if (num4 < num3 && ((Vector2.Distance(val, center) > num4 && num4 < num2) || !flag2))
				{
					bool flag3 = true;
					float num5 = Math.Abs(v.ToRotation());
					if (mountedPlayer.direction == 1 && (double)num5 > 1.047197594907988)
					{
						flag3 = false;
					}
					else if (mountedPlayer.direction == -1 && (double)num5 < 2.0943951461045853)
					{
						flag3 = false;
					}
					if (Collision.CanHitLine(center, 0, 0, nPC.position, nPC.width, nPC.height) & flag3)
					{
						num2 = num4;
						val = nPC.Center;
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				bool flag4 = _abilityCooldown == 0;
				if (_type == 46)
				{
					flag4 = _abilityCooldown % 10 == 0;
				}
				if (flag4 && mountedPlayer.whoAmI == Main.myPlayer)
				{
					AimAbility(mountedPlayer, val);
					if (_abilityCooldown == 0)
					{
						StopAbilityCharge();
					}
					UseAbility(mountedPlayer, val, toggleOn: false);
				}
				else
				{
					AimAbility(mountedPlayer, val);
					_abilityCharging = true;
				}
			}
			else
			{
				_abilityCharging = false;
				ResetHeadPosition();
			}
			break;
		}
		case 10:
			mountedPlayer.hasJumpOption_Unicorn = true;
			if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
			{
				mountedPlayer.noKnockback = true;
			}
			if (mountedPlayer.itemAnimation > 0 && mountedPlayer.inventory[mountedPlayer.selectedItem].type == 1260)
			{
				AchievementsHelper.HandleSpecialEvent(mountedPlayer, 5);
			}
			break;
		case 47:
			mountedPlayer.hasJumpOption_WallOfFleshGoat = true;
			if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
			{
				mountedPlayer.noKnockback = true;
			}
			break;
		case 14:
			mountedPlayer.hasJumpOption_Basilisk = true;
			if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
			{
				mountedPlayer.noKnockback = true;
			}
			break;
		case 40:
		case 41:
		case 42:
			if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
			{
				mountedPlayer.noKnockback = true;
			}
			break;
		case 12:
			if (mountedPlayer.MountFishronSpecial)
			{
				newColor = Colors.CurrentLiquidColor;
				Vector3 val11 = newColor.ToVector3();
				val11 *= 0.4f;
				Point val12 = (mountedPlayer.Center + Vector2.UnitX * (float)mountedPlayer.direction * 20f + mountedPlayer.velocity * 10f).ToTileCoordinates();
				if (!WorldGen.SolidTile(val12.X, val12.Y))
				{
					Lighting.AddLight(val12.X, val12.Y, val11.X, val11.Y, val11.Z);
				}
				else
				{
					Lighting.AddLight(mountedPlayer.Center + Vector2.UnitX * (float)mountedPlayer.direction * 20f, val11.X, val11.Y, val11.Z);
				}
				mountedPlayer.meleeDamage += 0.15f;
				mountedPlayer.rangedDamage += 0.15f;
				mountedPlayer.magicDamage += 0.15f;
				mountedPlayer.minionDamage += 0.15f;
			}
			if (mountedPlayer.statLife <= mountedPlayer.statLifeMax2 / 2)
			{
				mountedPlayer.MountFishronSpecialCounter = 60f;
			}
			if (mountedPlayer.wet || (Main.raining && WorldGen.InAPlaceWithWind(mountedPlayer.position, mountedPlayer.width, mountedPlayer.height)))
			{
				mountedPlayer.MountFishronSpecialCounter = 420f;
			}
			break;
		case 8:
			if (mountedPlayer.ownedProjectileCounts[453] < 1)
			{
				_abilityActive = false;
			}
			break;
		case 11:
		{
			Vector3 val6 = new Vector3(0.4f, 0.12f, 0.15f);
			float num8 = 1f + Math.Abs(mountedPlayer.velocity.X) / RunSpeed * 2.5f;
			int num9 = Math.Sign(mountedPlayer.velocity.X);
			if (num9 == 0)
			{
				num9 = mountedPlayer.direction;
			}
			if (Main.netMode == 2)
			{
				break;
			}
			val6 *= num8;
			Lighting.AddLight(mountedPlayer.Center, val6.X, val6.Y, val6.Z);
			Lighting.AddLight(mountedPlayer.Top, val6.X, val6.Y, val6.Z);
			Lighting.AddLight(mountedPlayer.Bottom, val6.X, val6.Y, val6.Z);
			Lighting.AddLight(mountedPlayer.Left, val6.X, val6.Y, val6.Z);
			Lighting.AddLight(mountedPlayer.Right, val6.X, val6.Y, val6.Z);
			float num10 = -24f;
			if (mountedPlayer.direction != num9)
			{
				num10 = -22f;
			}
			if (num9 == -1)
			{
				num10++;
			}
			Vector2 val7 = new Vector2(num10 * (float)num9, -19f).RotatedBy(mountedPlayer.fullRotation);
			Vector2 val8 = new Vector2(MathHelper.Lerp(0f, -8f, mountedPlayer.fullRotation / ((float)Math.PI / 4f)), MathHelper.Lerp(0f, 2f, Math.Abs(mountedPlayer.fullRotation / ((float)Math.PI / 4f)))).RotatedBy(mountedPlayer.fullRotation);
			if (num9 == Math.Sign(mountedPlayer.fullRotation))
			{
				val8 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(mountedPlayer.fullRotation / ((float)Math.PI / 4f)));
			}
			Vector2 val9 = mountedPlayer.Bottom + val7 + val8;
			Vector2 val10 = mountedPlayer.oldPosition + mountedPlayer.Size * new Vector2(0.5f, 1f) + val7 + val8;
			if (Vector2.Distance(val9, val10) > 3f)
			{
				int num11 = (int)Vector2.Distance(val9, val10) / 3;
				if (Vector2.Distance(val9, val10) % 3f != 0f)
				{
					num11++;
				}
				for (float num12 = 1f; num12 <= (float)num11; num12++)
				{
					Dust[] dust3 = Main.dust;
					Vector2 center2 = mountedPlayer.Center;
					newColor = default;
					Dust obj = dust3[Dust.NewDust(center2, 0, 0, 182, 0f, 0f, 0, newColor)];
					obj.position = Vector2.Lerp(val10, val9, num12 / (float)num11);
					obj.noGravity = true;
					obj.velocity = Vector2.Zero;
					obj.customData = mountedPlayer;
					obj.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
				}
			}
			else
			{
				Dust[] dust4 = Main.dust;
				Vector2 center3 = mountedPlayer.Center;
				newColor = default;
				Dust obj2 = dust4[Dust.NewDust(center3, 0, 0, 182, 0f, 0f, 0, newColor)];
				obj2.position = val9;
				obj2.noGravity = true;
				obj2.velocity = Vector2.Zero;
				obj2.customData = mountedPlayer;
				obj2.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
			}
			break;
		}
		case 22:
		{
			mountedPlayer.lavaMax += 420;
			Vector2 val2 = mountedPlayer.Center + new Vector2(20f, 10f) * mountedPlayer.Directions;
			Vector2 pos = val2 + mountedPlayer.velocity;
			Vector2 pos2 = val2 + new Vector2(-1f, -0.5f) * mountedPlayer.Directions;
			val2 = mountedPlayer.RotatedRelativePoint(val2);
			pos = mountedPlayer.RotatedRelativePoint(pos);
			pos2 = mountedPlayer.RotatedRelativePoint(pos2);
			Vector2 val3 = mountedPlayer.shadowPos[2] - mountedPlayer.position + val2;
			Vector2 val4 = pos - val2;
			val2 += val4;
			val3 += val4;
			Vector2 val5 = pos - pos2;
			float num6 = MathHelper.Clamp(mountedPlayer.velocity.Length() / 5f, 0f, 1f);
			for (float num7 = 0f; num7 <= 1f; num7 += 0.1f)
			{
				if (!(Main.rand.NextFloat() < num6))
				{
					Vector2 position = Vector2.Lerp(val3, val2, num7);
					Vector2? velocity = Main.rand.NextVector2Circular(0.5f, 0.5f) * num6;
					newColor = default;
					Dust dust2 = Dust.NewDustPerfect(position, 65, velocity, 0, newColor);
					dust2.scale = 0.6f;
					dust2.fadeIn = 0f;
					dust2.customData = mountedPlayer;
					dust2.velocity *= -1f;
					dust2.noGravity = true;
					dust2.velocity -= val5;
					dust2.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
					if (Main.rand.Next(10) == 0)
					{
						dust2.fadeIn = 1.3f;
						dust2.velocity = Main.rand.NextVector2Circular(3f, 3f) * num6;
					}
				}
			}
			break;
		}
		case 16:
			mountedPlayer.ignoreWater = true;
			break;
		case 24:
			DelegateMethods.v3_1 = new Vector3(0.1f, 0.3f, 1f) * 0.4f;
			Utils.PlotTileLine(mountedPlayer.MountedCenter, mountedPlayer.MountedCenter + mountedPlayer.velocity * 6f, 40f, DelegateMethods.CastLightOpen);
			Utils.PlotTileLine(mountedPlayer.Left, mountedPlayer.Right, 40f, DelegateMethods.CastLightOpen);
			break;
		case 36:
			DoSteamMinecartEffect(mountedPlayer, 303);
			break;
		case 32:
			DoExhaustMinecartEffect(mountedPlayer, 31);
			break;
		case 34:
			DoConfettiMinecartEffect(mountedPlayer);
			break;
		case 37:
			mountedPlayer.canFloatInWater = true;
			mountedPlayer.accFlipper = true;
			break;
		case 55:
		case 56:
			mountedPlayer.IsAllowedToHoldItems = false;
			mountedPlayer.noItems = true;
			break;
		case 61:
		{
			mountedPlayer.IsAllowedToHoldItems = false;
			mountedPlayer.noItems = true;
			bool flag = Main.rand.Next(15) == 0;
			if ((int)Main.timeForVisualEffects % 2 == 0 && (flag || (float)(Main.rand.Next(6) + 1) < mountedPlayer.velocity.Length()))
			{
				Color fairyQueenWeaponsColorFull = Projectile.GetFairyQueenWeaponsColorFull(mountedPlayer.whoAmI, mountedPlayer.Center, 0.41f, 1f, 0.45f, 1f, 0.7f);
				Color fairyQueenWeaponsColorFull2 = Projectile.GetFairyQueenWeaponsColorFull(mountedPlayer.whoAmI, mountedPlayer.Center, 0.41f, 1f, 0f, 1f, 0.7f);
				Dust dust = Dust.NewDustDirect(mountedPlayer.Center, 0, 0, 278, 0f, 0f, 200, Color.Lerp(fairyQueenWeaponsColorFull, fairyQueenWeaponsColorFull2, Main.rand.NextFloat()), 0.65f);
				dust.position = mountedPlayer.Center + new Vector2(0f, -2f);
				if (flag)
				{
					dust.velocity *= 0.4f;
				}
				else
				{
					dust.velocity *= 0.04f * mountedPlayer.velocity.Length();
				}
				dust.velocity += mountedPlayer.velocity * 0.3f;
				dust.position += mountedPlayer.velocity * 0.7f;
				dust.position += (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2() * Main.rand.NextFloat() * 2f;
				dust.noGravity = true;
				dust.noLight = true;
			}
			break;
		}
		case 57:
		case 58:
		case 59:
		case 60:
			mountedPlayer.MinecartSettings.MagnetOffset.Y -= 5f;
			mountedPlayer.MinecartSettings.MinecartTextureWidth = 4f;
			mountedPlayer.MinecartSettings.MagnetOffset.X = 2f;
			mountedPlayer.MinecartSettings.WheelOffset.X = 4f;
			mountedPlayer.doorHelper.AllowOpeningDoorsByVelocityAloneForATime(60);
			break;
		case 13:
		case 15:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 33:
		case 35:
		case 38:
		case 39:
		case 43:
		case 44:
		case 45:
		case 48:
		case 49:
		case 50:
		case 51:
		case 52:
		case 53:
		case 54:
			break;
		}
	}

	private void CastSuperCartLaser(Player mountedPlayer)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		int num = Math.Sign(mountedPlayer.velocity.X);
		if (num == 0)
		{
			num = mountedPlayer.direction;
		}
		if (mountedPlayer.whoAmI != Main.myPlayer || mountedPlayer.velocity.X == 0f)
		{
			return;
		}
		Vector2 minecartMechPoint = GetMinecartMechPoint(mountedPlayer, 20, -19);
		int damage = 60;
		int num2 = 0;
		float num3 = 0f;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (nPC.active && nPC.immune[mountedPlayer.whoAmI] <= 0 && !nPC.dontTakeDamage && nPC.Distance(minecartMechPoint) < 300f && nPC.CanBeChasedBy(mountedPlayer) && Collision.CanHitLine(nPC.position, nPC.width, nPC.height, minecartMechPoint, 0, 0) && Math.Abs(MathHelper.WrapAngle(MathHelper.WrapAngle(nPC.AngleFrom(minecartMechPoint)) - MathHelper.WrapAngle((mountedPlayer.fullRotation + (float)num == -1f) ? ((float)Math.PI) : 0f))) < (float)Math.PI / 4f)
			{
				minecartMechPoint = GetMinecartMechPoint(mountedPlayer, -20, -39);
				Vector2 val = nPC.position + nPC.Size * Utils.RandomVector2(Main.rand, 0f, 1f) - minecartMechPoint;
				num3 += val.ToRotation();
				num2++;
				int num4 = Projectile.NewProjectile(GetProjectileSpawnSource(mountedPlayer), minecartMechPoint.X, minecartMechPoint.Y, val.X, val.Y, 591, 0, 0f, mountedPlayer.whoAmI, mountedPlayer.whoAmI);
				Main.projectile[num4].Center = nPC.Center;
				Main.projectile[num4].damage = damage;
				Main.projectile[num4].Damage();
				Main.projectile[num4].damage = 0;
				Main.projectile[num4].Center = minecartMechPoint;
			}
		}
	}

	public static Vector2 GetMinecartMechPoint(Player mountedPlayer, int offX, int offY)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		int num = Math.Sign(mountedPlayer.velocity.X);
		if (num == 0)
		{
			num = mountedPlayer.direction;
		}
		float num2 = offX;
		int num3 = Math.Sign(offX);
		if (mountedPlayer.direction != num)
		{
			num2 -= (float)num3;
		}
		if (num == -1)
		{
			num2 -= (float)num3;
		}
		Vector2 val = new Vector2(num2 * (float)num, (float)offY).RotatedBy(mountedPlayer.fullRotation);
		Vector2 val2 = new Vector2(MathHelper.Lerp(0f, -8f, mountedPlayer.fullRotation / ((float)Math.PI / 4f)), MathHelper.Lerp(0f, 2f, Math.Abs(mountedPlayer.fullRotation / ((float)Math.PI / 4f)))).RotatedBy(mountedPlayer.fullRotation);
		if (num == Math.Sign(mountedPlayer.fullRotation))
		{
			val2 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(mountedPlayer.fullRotation / ((float)Math.PI / 4f)));
		}
		return mountedPlayer.Bottom + val + val2;
	}

	public void ResetFlightTime(Player mountedPlayer)
	{
		_flyTime = (_active ? _data.flightTimeMax : 0);
		if (_type == 0)
		{
			_flyTime += (int)(Math.Abs(mountedPlayer.velocity.X) * 20f);
		}
		if (_type == 54)
		{
			_flyTime = mountedPlayer.wingTimeMax;
		}
	}

	public void CheckMountBuff(Player mountedPlayer)
	{
		if (_type != -1 && mountedPlayer.FindBuffIndex(_data.buff) == -1)
		{
			TryDismount(mountedPlayer);
		}
	}

	public void ResetHeadPosition()
	{
		if (_aiming)
		{
			_aiming = false;
			if (_type != 46)
			{
				_frameExtra = 0;
			}
			_flipDraw = false;
		}
	}

	private Vector2 ClampToDeadZone(Player mountedPlayer, Vector2 position)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		int num;
		int num2;
		switch (_type)
		{
		case 9:
			num = (int)scutlixTextureSize.Y;
			num2 = (int)scutlixTextureSize.X;
			break;
		case 46:
			num = (int)santankTextureSize.Y;
			num2 = (int)santankTextureSize.X;
			break;
		case 8:
			num = (int)drillTextureSize.Y;
			num2 = (int)drillTextureSize.X;
			break;
		default:
			return position;
		}
		Vector2 center = mountedPlayer.Center;
		position -= center;
		if (position.X > (float)(-num2) && position.X < (float)num2 && position.Y > (float)(-num) && position.Y < (float)num)
		{
			float num3 = (float)num2 / Math.Abs(position.X);
			float num4 = (float)num / Math.Abs(position.Y);
			position = ((!(num3 > num4)) ? (position * num3) : (position * num4));
		}
		return position + center;
	}

	public bool AimAbility(Player mountedPlayer, Vector2 mousePosition)
	{
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		_aiming = true;
		switch (_type)
		{
		case 9:
		{
			int frameExtra = _frameExtra;
			int direction = mountedPlayer.direction;
			float num3 = MathHelper.ToDegrees((ClampToDeadZone(mountedPlayer, mousePosition) - mountedPlayer.Center).ToRotation());
			if (num3 > 90f)
			{
				mountedPlayer.direction = -1;
				num3 = 180f - num3;
			}
			else if (num3 < -90f)
			{
				mountedPlayer.direction = -1;
				num3 = -180f - num3;
			}
			else
			{
				mountedPlayer.direction = 1;
			}
			if ((mountedPlayer.direction > 0 && mountedPlayer.velocity.X < 0f) || (mountedPlayer.direction < 0 && mountedPlayer.velocity.X > 0f))
			{
				_flipDraw = true;
			}
			else
			{
				_flipDraw = false;
			}
			if (num3 >= 0f)
			{
				if ((double)num3 < 22.5)
				{
					_frameExtra = 8;
				}
				else if ((double)num3 < 67.5)
				{
					_frameExtra = 9;
				}
				else if ((double)num3 < 112.5)
				{
					_frameExtra = 10;
				}
			}
			else if ((double)num3 > -22.5)
			{
				_frameExtra = 8;
			}
			else if ((double)num3 > -67.5)
			{
				_frameExtra = 7;
			}
			else if ((double)num3 > -112.5)
			{
				_frameExtra = 6;
			}
			float abilityCharge = AbilityCharge;
			if (abilityCharge > 0f)
			{
				Vector2 val = default;
				val.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
				val.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
				int num4 = (_frameExtra - 6) * 2;
				Vector2 val2 = default;
				for (int i = 0; i < 2; i++)
				{
					val2.Y = val.Y + scutlixEyePositions[num4 + i].Y;
					if (mountedPlayer.direction == -1)
					{
						val2.X = val.X - scutlixEyePositions[num4 + i].X - (float)_data.xOffset;
					}
					else
					{
						val2.X = val.X + scutlixEyePositions[num4 + i].X + (float)_data.xOffset;
					}
					Lighting.AddLight((int)(val2.X / 16f), (int)(val2.Y / 16f), 1f * abilityCharge, 0f, 0f);
				}
			}
			if (_frameExtra == frameExtra)
			{
				return mountedPlayer.direction != direction;
			}
			return true;
		}
		case 46:
		{
			int frameExtra = _frameExtra;
			int direction = mountedPlayer.direction;
			float num3 = MathHelper.ToDegrees((ClampToDeadZone(mountedPlayer, mousePosition) - mountedPlayer.Center).ToRotation());
			if (num3 > 90f)
			{
				mountedPlayer.direction = -1;
				num3 = 180f - num3;
			}
			else if (num3 < -90f)
			{
				mountedPlayer.direction = -1;
				num3 = -180f - num3;
			}
			else
			{
				mountedPlayer.direction = 1;
			}
			if ((mountedPlayer.direction > 0 && mountedPlayer.velocity.X < 0f) || (mountedPlayer.direction < 0 && mountedPlayer.velocity.X > 0f))
			{
				_flipDraw = true;
			}
			else
			{
				_flipDraw = false;
			}
			float abilityCharge = AbilityCharge;
			if (abilityCharge > 0f)
			{
				Vector2 val3 = default;
				val3.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
				val3.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
				for (int j = 0; j < 2; j++)
				{
					Vector2 val4 = new Vector2(val3.X + (float)(mountedPlayer.width * mountedPlayer.direction), val3.Y - 12f);
					Lighting.AddLight((int)(val4.X / 16f), (int)(val4.Y / 16f), 0.7f, 0.4f, 0.4f);
				}
			}
			if (_frameExtra == frameExtra)
			{
				return mountedPlayer.direction != direction;
			}
			return true;
		}
		case 8:
		{
			Vector2 v = ClampToDeadZone(mountedPlayer, mousePosition) - mountedPlayer.Center;
			DrillMountData drillMountData = (DrillMountData)_mountSpecificData;
			float num = v.ToRotation();
			if (num < 0f)
			{
				num += (float)Math.PI * 2f;
			}
			drillMountData.diodeRotationTarget = num;
			float num2 = drillMountData.diodeRotation % ((float)Math.PI * 2f);
			if (num2 < 0f)
			{
				num2 += (float)Math.PI * 2f;
			}
			if (num2 < num)
			{
				if (num - num2 > (float)Math.PI)
				{
					num2 += (float)Math.PI * 2f;
				}
			}
			else if (num2 - num > (float)Math.PI)
			{
				num2 -= (float)Math.PI * 2f;
			}
			drillMountData.diodeRotation = num2;
			drillMountData.crosshairPosition = mousePosition;
			return true;
		}
		default:
			return false;
		}
	}

	public void Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, Vector2 Position, Color drawColor, SpriteEffects playerEffect, float shadow)
	{
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0891: Unknown result type (might be due to invalid IL or missing references)
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0940: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Unknown result type (might be due to invalid IL or missing references)
		//IL_1015: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Unknown result type (might be due to invalid IL or missing references)
		//IL_102a: Unknown result type (might be due to invalid IL or missing references)
		//IL_102c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Unknown result type (might be due to invalid IL or missing references)
		//IL_1033: Unknown result type (might be due to invalid IL or missing references)
		//IL_1042: Unknown result type (might be due to invalid IL or missing references)
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_104a: Unknown result type (might be due to invalid IL or missing references)
		//IL_104f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d83: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dac: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1117: Unknown result type (might be due to invalid IL or missing references)
		//IL_1128: Unknown result type (might be due to invalid IL or missing references)
		//IL_112a: Unknown result type (might be due to invalid IL or missing references)
		//IL_112c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1131: Unknown result type (might be due to invalid IL or missing references)
		//IL_1138: Unknown result type (might be due to invalid IL or missing references)
		//IL_1142: Unknown result type (might be due to invalid IL or missing references)
		//IL_1147: Unknown result type (might be due to invalid IL or missing references)
		if (playerDrawData == null)
		{
			return;
		}
		Texture2D val2;
		Texture2D val;
		switch (drawType)
		{
		case 0:
			val = _data.backTexture.Value;
			val2 = _data.backTextureGlow.Value;
			break;
		case 1:
			val = _data.backTextureExtra.Value;
			val2 = _data.backTextureExtraGlow.Value;
			break;
		case 2:
			if (_type == 0 && _idleTime >= _idleTimeNext)
			{
				return;
			}
			val = _data.frontTexture.Value;
			val2 = _data.frontTextureGlow.Value;
			break;
		case 3:
			val = _data.frontTextureExtra.Value;
			val2 = _data.frontTextureExtraGlow.Value;
			break;
		default:
			val = null;
			val2 = null;
			break;
		}
		int type = _type;
		if (type == 50 && val != null)
		{
			PlayerQueenSlimeMountTextureContent queenSlimeMount = TextureAssets.RenderTargets.QueenSlimeMount;
			queenSlimeMount.Request();
			if (queenSlimeMount.IsReady)
			{
				val = (Texture2D)(object)queenSlimeMount.GetTarget();
			}
		}
		if (val == null)
		{
			return;
		}
		type = _type;
		if ((type == 0 || type == 9) && drawType == 3 && shadow != 0f)
		{
			return;
		}
		int num = XOffset;
		int num2 = YOffset + PlayerOffset;
		if (drawPlayer.direction <= 0)
		{
			num *= -1;
		}
		Position.X = (int)(Position.X - Main.screenPosition.X + (float)(drawPlayer.width / 2) + (float)num);
		Position.Y = (int)(Position.Y - Main.screenPosition.Y + (float)(drawPlayer.height / 2) + (float)num2);
		int num3 = 0;
		bool flag = true;
		int num4 = _data.totalFrames;
		int num5 = _data.textureHeight;
		switch (_type)
		{
		case 23:
			num3 = _frame;
			break;
		case 9:
			num3 = drawType switch
			{
				0 => _frame, 
				2 => _frameExtra, 
				3 => _frameExtra, 
				_ => 0, 
			};
			break;
		case 46:
			num3 = drawType switch
			{
				2 => _frame, 
				3 => _frameExtra, 
				_ => 0, 
			};
			break;
		case 5:
			num3 = drawType switch
			{
				0 => _frame, 
				1 => _frameExtra, 
				_ => 0, 
			};
			break;
		case 17:
			num5 = val.Height;
			switch (drawType)
			{
			case 0:
				num3 = _frame;
				num4 = 4;
				break;
			case 1:
				num3 = _frameExtra;
				num4 = 4;
				break;
			default:
				num3 = 0;
				break;
			}
			break;
		case 52:
			if (drawType == 3)
			{
				if (drawPlayer.itemAnimation > 0)
				{
					Rectangle bodyFrame2 = drawPlayer.bodyFrame;
					int num6 = bodyFrame2.Y / bodyFrame2.Height;
					int useStyle2 = drawPlayer.lastVisualizedSelectedItem.useStyle;
					num3 = Utils.Clamp(num6, 1, 4);
					if (num3 == 3 || num6 == 0 || useStyle2 == 13)
					{
						num3 = _frame;
					}
					if (useStyle2 == 12 && drawPlayer.itemAnimation > drawPlayer.itemAnimationMax / 2)
					{
						num3 = 3;
					}
				}
				else
				{
					_ = drawPlayer.lastVisualizedSelectedItem.holdStyle;
					num3 = _frame;
				}
			}
			else
			{
				num3 = _frame;
			}
			break;
		case 54:
			if (drawType == 3)
			{
				if (drawPlayer.itemAnimation > 0)
				{
					Rectangle bodyFrame = drawPlayer.bodyFrame;
					int value = bodyFrame.Y / bodyFrame.Height;
					int useStyle = drawPlayer.lastVisualizedSelectedItem.useStyle;
					num3 = Utils.Clamp(value, 1, 4);
					if (useStyle == 12 && drawPlayer.itemAnimation > drawPlayer.itemAnimationMax / 2)
					{
						num3 = 3;
					}
					if (useStyle == 2 || useStyle == 9 || useStyle == 4 || useStyle == 14)
					{
						num3 = 2;
					}
					if (useStyle == 8 || useStyle == 11)
					{
						num3 = 3;
					}
				}
				else
				{
					switch (drawPlayer.lastVisualizedSelectedItem.holdStyle)
					{
					case 1:
					case 6:
						num3 = 3;
						break;
					case 2:
						num3 = 2;
						break;
					default:
						num3 = _frame;
						break;
					}
				}
			}
			else
			{
				num3 = _frame;
			}
			break;
		case 39:
			num5 = val.Height;
			switch (drawType)
			{
			case 2:
				num3 = _frame;
				num4 = 3;
				break;
			case 3:
				num3 = _frameExtra;
				num4 = 6;
				break;
			default:
				num3 = 0;
				break;
			}
			break;
		case 62:
		case 63:
		case 64:
		case 65:
			num3 = _frame;
			if (num3 < 4 && drawPlayer.petting.isPetting && drawPlayer.petting.mount)
			{
				num3 = 12;
			}
			break;
		default:
			num3 = _frame;
			break;
		}
		int num7 = num5 / num4;
		Rectangle value2 = new Rectangle(0, num7 * num3, _data.textureWidth, num7);
		if (flag)
		{
			value2.Height -= 2;
		}
		switch (_type)
		{
		case 0:
			if (drawType == 3)
			{
				drawColor = Color.White;
			}
			break;
		case 9:
			if (drawType == 3)
			{
				if (_abilityCharge == 0)
				{
					return;
				}
				drawColor = Color.Multiply(Color.White, (float)_abilityCharge / (float)_data.abilityChargeMax);
				drawColor.A = 0;
			}
			break;
		case 7:
			if (drawType == 3)
			{
				drawColor = new Color(250, 250, 250, 255) * drawPlayer.stealth * (1f - shadow);
			}
			break;
		case 61:
			drawColor = new Color(drawColor.ToVector4() * 0.5f + new Vector4(0.5f));
			if (drawType == 3)
			{
				drawColor = Projectile.GetFairyQueenWeaponsColorFull(drawPlayer.whoAmI, drawPlayer.Center, 0.41f, 1f, 0.15f, 1f, 0.7f);
				drawColor.A = (byte)((float)(int)drawColor.A * 0.65f);
			}
			drawColor *= drawPlayer.stealth * (1f - shadow);
			break;
		}
		Color val3 = new Color(drawColor.ToVector4() * 0.25f + new Vector4(0.75f));
		switch (_type)
		{
		case 56:
			if (drawType == 2)
			{
				val3 = Color.White;
				val3.A = 0;
			}
			break;
		case 45:
			if (drawType == 2)
			{
				val3 = new Color(150, 110, 110, 100);
			}
			break;
		case 11:
			if (drawType == 2)
			{
				val3 = Color.White;
				val3.A = 127;
			}
			break;
		case 12:
			if (drawType == 0)
			{
				float num8 = MathHelper.Clamp(drawPlayer.MountFishronSpecialCounter / 60f, 0f, 1f);
				val3 = Colors.CurrentLiquidColor;
				if (val3 == Color.Transparent)
				{
					val3 = Color.White;
				}
				val3.A = 127;
				val3 *= num8;
			}
			break;
		case 24:
			if (drawType == 2)
			{
				val3 = Color.SkyBlue * 0.5f;
				val3.A = 20;
			}
			break;
		}
		float num9 = 0f;
		switch (_type)
		{
		case 8:
		{
			DrillMountData drillMountData = (DrillMountData)_mountSpecificData;
			switch (drawType)
			{
			case 0:
				num9 = drillMountData.outerRingRotation - num9;
				break;
			case 3:
				num9 = drillMountData.diodeRotation - num9 - drawPlayer.fullRotation;
				break;
			}
			break;
		}
		case 7:
			num9 = drawPlayer.fullRotation;
			break;
		}
		Vector2 origin = Origin;
		type = _type;
		_ = 8;
		float scale = 1f;
		SpriteEffects val4 = _type switch
		{
			7 => (SpriteEffects)0, 
			8 => (SpriteEffects)((drawPlayer.direction == 1 && drawType == 2) ? 1 : 0), 
			_ => playerEffect, 
		};
		if (Cart)
		{
			val4 = (SpriteEffects)((Math.Sign(drawPlayer.velocity.X) == -drawPlayer.direction) ? (playerEffect ^ (SpriteEffects)1) : playerEffect);
		}
		bool flag2 = false;
		switch (_type)
		{
		case 50:
			if (drawType == 0)
			{
				Vector2 position = Position + new Vector2(0f, (float)(8 - PlayerOffset + 20));
				Rectangle value7 = new Rectangle(0, num7 * _frameExtra, _data.textureWidth, num7);
				if (flag)
				{
					value7.Height -= 2;
				}
				DrawData item = new DrawData(TextureAssets.Extra[207].Value, position, value7, drawColor, num9, origin, scale, val4);
				item.shader = currentShader;
				playerDrawData.Add(item);
			}
			break;
		case 35:
		{
			if (drawType != 2)
			{
				break;
			}
			ExtraFrameMountData extraFrameMountData = (ExtraFrameMountData)_mountSpecificData;
			int num11 = -36;
			if (((int)val4 & 1) != 0)
			{
				num11 *= -1;
			}
			Vector2 val6 = new Vector2((float)num11, -26f);
			if (shadow == 0f)
			{
				if (Math.Abs(drawPlayer.velocity.X) > 1f)
				{
					extraFrameMountData.frameCounter += Math.Min(2f, Math.Abs(drawPlayer.velocity.X * 0.4f));
					while (extraFrameMountData.frameCounter > 6f)
					{
						extraFrameMountData.frameCounter -= 6f;
						extraFrameMountData.frame++;
						if ((extraFrameMountData.frame > 2 && extraFrameMountData.frame < 5) || extraFrameMountData.frame > 7)
						{
							extraFrameMountData.frame = 0;
						}
					}
				}
				else
				{
					extraFrameMountData.frameCounter++;
					while (extraFrameMountData.frameCounter > 6f)
					{
						extraFrameMountData.frameCounter -= 6f;
						extraFrameMountData.frame++;
						if (extraFrameMountData.frame > 5)
						{
							extraFrameMountData.frame = 5;
						}
					}
				}
			}
			Texture2D value5 = TextureAssets.Extra[142].Value;
			Rectangle value6 = value5.Frame(1, 8, 0, extraFrameMountData.frame);
			if (flag)
			{
				value6.Height -= 2;
			}
			DrawData item = new DrawData(value5, Position + val6, value6, drawColor, num9, origin, scale, val4);
			item.shader = currentShader;
			playerDrawData.Add(item);
			break;
		}
		case 38:
			if (drawType == 0)
			{
				int num10 = 0;
				if (((int)val4 & 1) != 0)
				{
					num10 = 22;
				}
				Vector2 val5 = new Vector2((float)num10, -10f);
				Texture2D value3 = TextureAssets.Extra[151].Value;
				Rectangle value4 = value3.Frame();
				DrawData item = new DrawData(value3, Position + val5, value4, drawColor, num9, origin, scale, val4);
				item.shader = currentShader;
				playerDrawData.Add(item);
				flag2 = true;
			}
			break;
		}
		if (!flag2)
		{
			DrawData item = new DrawData(val, Position, value2, drawColor, num9, origin, scale, val4);
			item.shader = currentShader;
			playerDrawData.Add(item);
			if (val2 != null)
			{
				item = new DrawData(val2, Position, value2, val3 * ((float)(int)drawColor.A / 255f), num9, origin, scale, val4);
				item.shader = currentShader;
			}
			playerDrawData.Add(item);
		}
		switch (_type)
		{
		case 50:
			if (drawType == 0)
			{
				val = TextureAssets.Extra[205].Value;
				DrawData item = new DrawData(val, Position, value2, drawColor, num9, origin, scale, val4);
				item.shader = currentShader;
				playerDrawData.Add(item);
				Vector2 position3 = Position + new Vector2(0f, (float)(8 - PlayerOffset + 20));
				Rectangle value9 = new Rectangle(0, num7 * _frameExtra, _data.textureWidth, num7);
				if (flag)
				{
					value9.Height -= 2;
				}
				val = TextureAssets.Extra[206].Value;
				item = new DrawData(val, position3, value9, drawColor, num9, origin, scale, val4);
				item.shader = currentShader;
				playerDrawData.Add(item);
			}
			break;
		case 45:
		{
			if (drawType != 0 || shadow != 0f)
			{
				break;
			}
			if (Math.Abs(drawPlayer.velocity.X) > DashSpeed * 0.9f)
			{
				val3 = new Color(255, 220, 220, 200);
				scale = 1.1f;
			}
			for (int k = 0; k < 2; k++)
			{
				Vector2 position2 = Position + new Vector2((float)Main.rand.Next(-10, 11) * 0.1f, (float)Main.rand.Next(-10, 11) * 0.1f);
				value2 = new Rectangle(0, num7 * 3, _data.textureWidth, num7);
				if (flag)
				{
					value2.Height -= 2;
				}
				DrawData item = new DrawData(val2, position2, value2, val3, num9, origin, scale, val4);
				item.shader = currentShader;
				playerDrawData.Add(item);
			}
			break;
		}
		case 17:
			if (drawType == 1 && ShouldGolfCartEmitLight())
			{
				value2 = new Rectangle(0, num7 * 3, _data.textureWidth, num7);
				if (flag)
				{
					value2.Height -= 2;
				}
				drawColor = Color.White * 1f;
				drawColor.A = 0;
				DrawData item = new DrawData(val, Position, value2, drawColor, num9, origin, scale, val4);
				item.shader = currentShader;
				playerDrawData.Add(item);
			}
			break;
		case 23:
			if (drawType == 0)
			{
				val = TextureAssets.Extra[114].Value;
				value2 = val.Frame(2);
				int width = value2.Width;
				value2.Width -= 2;
				float witchBroomTrinketRotation = GetWitchBroomTrinketRotation(drawPlayer);
				Vector2 val12 = Position + GetWitchBroomTrinketOriginOffset(drawPlayer);
				num9 = witchBroomTrinketRotation;
				origin = new Vector2((float)(value2.Width / 2), 0f);
				DrawData item = new DrawData(val, val12.Floor(), value2, drawColor, num9, origin, scale, val4);
				item.shader = currentShader;
				playerDrawData.Add(item);
				Color val13 = new Color(new Vector3(0.9f, 0.85f, 0f));
				val13.A /= 2;
				float num14 = ((float)drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 1f;
				Color color = new Color(80, 70, 40, 0) * (num14 / 8f + 0.5f) * 0.8f;
				value2.X += width;
				for (int l = 0; l < 4; l++)
				{
					item = new DrawData(val, (val12 + ((float)l * ((float)Math.PI / 2f)).ToRotationVector2() * num14).Floor(), value2, color, num9, origin, scale, val4);
					item.shader = currentShader;
					playerDrawData.Add(item);
				}
			}
			break;
		case 8:
		{
			if (drawType != 3)
			{
				break;
			}
			DrillMountData drillMountData2 = (DrillMountData)_mountSpecificData;
			Rectangle value8 = new Rectangle(0, 0, 1, 1);
			Vector2 val7 = drillDiodePoint1.RotatedBy(drillMountData2.diodeRotation);
			Vector2 val8 = drillDiodePoint2.RotatedBy(drillMountData2.diodeRotation);
			for (int i = 0; i < drillMountData2.beams.Length; i++)
			{
				DrillBeam drillBeam = drillMountData2.beams[i];
				if (drillBeam.curTileTarget == Point16.NegativeOne)
				{
					continue;
				}
				for (int j = 0; j < 2; j++)
				{
					Vector2 val9 = new Vector2((float)(drillBeam.curTileTarget.X * 16 + 8), (float)(drillBeam.curTileTarget.Y * 16 + 8)) - Main.screenPosition - Position;
					Vector2 val10;
					Color val11;
					if (j == 0)
					{
						val10 = val7;
						val11 = Color.CornflowerBlue;
					}
					else
					{
						val10 = val8;
						val11 = Color.LightGreen;
					}
					val11.A = 128;
					val11 *= 0.5f;
					Vector2 v = val9 - val10;
					float num12 = v.ToRotation();
					float num13 = v.Length();
					DrawData item = new DrawData(scale: new Vector2(2f, num13), texture: TextureAssets.MagicPixel.Value, position: val10 + Position, sourceRect: value8, color: val11, rotation: num12 - (float)Math.PI / 2f, origin: Vector2.Zero, effect: (SpriteEffects)0);
					item.ignorePlayerRotation = true;
					item.shader = currentShader;
					playerDrawData.Add(item);
				}
			}
			break;
		}
		}
		if (Main.myPlayer == drawPlayer.whoAmI && (_type == 62 || _type == 63 || _type == 64 || _type == 65))
		{
			TryPettingMount(drawPlayer);
		}
	}

	private void TryPettingMount(Player player)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu || Main.gamePaused || Math.Abs(player.velocity.X) >= 1f)
		{
			return;
		}
		Vector2 v = Main.ReverseGravitySupport(Main.MouseScreen) + Main.screenPosition;
		Rectangle val = Utils.CenteredRectangle(player.Bottom + new Vector2((float)(player.direction * 26), -54f), new Vector2(24f, 24f));
		bool flag = val.Contains(v.ToPoint());
		bool num = flag & !player.lastMouseInterface;
		if (!Main.SmartCursorIsUsed)
		{
			_ = !PlayerInput.UsingGamepad;
		}
		else
			_ = 0;
		if (!num)
		{
			return;
		}
		if (flag)
		{
			player.noThrow = 4;
			player.cursorItemIconEnabled = true;
			switch (_type)
			{
			case 62:
				player.cursorItemIconID = 5665;
				break;
			case 63:
				player.cursorItemIconID = 5666;
				break;
			case 64:
				player.cursorItemIconID = 6150;
				break;
			case 65:
				player.cursorItemIconID = 6151;
				break;
			}
		}
		if (PlayerInput.UsingGamepad)
		{
			player.GamepadEnableGrappleCooldown();
		}
		if (Main.mouseRight && Main.mouseRightRelease && Player.BlockInteractionWithProjectiles == 0)
		{
			Main.mouseRightRelease = false;
			player.tileInteractAttempted = true;
			player.tileInteractionHappened = true;
			player.releaseUseTile = false;
			player.PetMount(new PlayerPettingInfo(_type, isPetSmall: false));
			EmoteBubble.NewBubble(0, new WorldUIAnchor((Entity)Main.LocalPlayer), 60);
		}
	}

	public DismountCheckResult TryDismountWithResult(Player mountedPlayer)
	{
		DismountCheckResult dismountCheckResult = CanDismountWithResult(mountedPlayer);
		if (dismountCheckResult == DismountCheckResult.Succeeded)
		{
			Dismount(mountedPlayer);
		}
		return dismountCheckResult;
	}

	public bool TryDismount(Player mountedPlayer)
	{
		return TryDismountWithResult(mountedPlayer) == DismountCheckResult.Succeeded;
	}

	public void Dismount(Player mountedPlayer, bool ignoreEffect = false)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		if (!_active)
		{
			return;
		}
		bool cart = Cart;
		_active = false;
		mountedPlayer.ClearBuff(_data.buff);
		_mountSpecificData = null;
		int type = _type;
		if ((uint)(type - 55) <= 1u && mountedPlayer.noItems && !mountedPlayer.cursed)
		{
			mountedPlayer.noItems = false;
		}
		if (cart)
		{
			mountedPlayer.cartFlip = false;
			mountedPlayer.lastBoost = Vector2.Zero;
		}
		mountedPlayer.fullRotation = 0f;
		mountedPlayer.fullRotationOrigin = Vector2.Zero;
		if (mountedPlayer.petting.isPetting && mountedPlayer.petting.mount)
		{
			mountedPlayer.StopPettingAnimal();
		}
		if (!ignoreEffect)
		{
			DoSpawnDust(mountedPlayer, isDismounting: true);
		}
		Reset();
		if (!mountedPlayer.isDisplayDollOrInanimate && mountedPlayer.whoAmI == Main.myPlayer)
		{
			NetMessage.SendData(13, -1, -1, null, mountedPlayer.whoAmI);
		}
		if (Collision.TryChangingSizeFromBottomCenter(mountedPlayer.Hitbox, 20, 42, out var changedHitbox))
		{
			Vector2 val = changedHitbox.TopLeft() - mountedPlayer.position;
			mountedPlayer.position += val;
			mountedPlayer.width = 20;
			mountedPlayer.height = 42;
			for (int i = 0; i < mountedPlayer.shadowPos.Length; i++)
			{
				ref Vector2 reference = ref mountedPlayer.shadowPos[i];
				reference += val;
			}
		}
		else
		{
			mountedPlayer.position.Y += mountedPlayer.height;
			mountedPlayer.width = 20;
			mountedPlayer.height = 42;
			mountedPlayer.position.Y -= mountedPlayer.height;
		}
	}

	public void SetMount(int m, Player mountedPlayer)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		if (_type == m || m <= -1 || m >= MountID.Count || (m == 5 && mountedPlayer.wet))
		{
			return;
		}
		if (_active)
		{
			mountedPlayer.ClearBuff(_data.buff);
			if (AnyTrackRider)
			{
				mountedPlayer.cartFlip = false;
				mountedPlayer.lastBoost = Vector2.Zero;
			}
			mountedPlayer.fullRotation = 0f;
			mountedPlayer.fullRotationOrigin = Vector2.Zero;
			_mountSpecificData = null;
		}
		else
		{
			_active = true;
		}
		_flyTime = 0;
		_type = m;
		_data = mounts[m];
		_fatigueMax = _data.fatigueMax;
		if (!mountedPlayer.isDisplayDollOrInanimate && mountedPlayer.whoAmI == Main.myPlayer)
		{
			NetMessage.SendData(13, -1, -1, null, mountedPlayer.whoAmI);
		}
		mountedPlayer.AddBuff(_data.buff, 3600);
		_flipDraw = false;
		if (_type == 44)
		{
			mountedPlayer.velocity *= 0.2f;
			mountedPlayer.dash = 0;
			mountedPlayer.dashType = 0;
			mountedPlayer.dashDelay = 0;
			mountedPlayer.dashTime = 0;
		}
		if (_type == 9 && _abilityCooldown < 20)
		{
			_abilityCooldown = 20;
		}
		if (_type == 46 && _abilityCooldown < 40)
		{
			_abilityCooldown = 40;
		}
		MountDelegatesData.OverrideSizeMethod playerSize = _data.delegations.PlayerSize;
		if (playerSize != null && playerSize(mountedPlayer, out var size) && size.HasValue)
		{
			Vector2 value = size.Value;
			Vector2 bottom = mountedPlayer.Bottom;
			mountedPlayer.position = mountedPlayer.Bottom;
			for (int i = 0; i < mountedPlayer.shadowPos.Length; i++)
			{
				mountedPlayer.shadowPos[i].X += mountedPlayer.width / 2;
				mountedPlayer.shadowPos[i].Y += mountedPlayer.height;
			}
			mountedPlayer.width = (int)value.X;
			mountedPlayer.height = (int)value.Y;
			mountedPlayer.position = new Vector2(bottom.X - value.X / 2f, bottom.Y - (float)mountedPlayer.height);
			for (int j = 0; j < mountedPlayer.shadowPos.Length; j++)
			{
				mountedPlayer.shadowPos[j].X -= mountedPlayer.width / 2;
				mountedPlayer.shadowPos[j].Y -= mountedPlayer.height;
			}
		}
		else
		{
			mountedPlayer.position.Y += mountedPlayer.height;
			for (int k = 0; k < mountedPlayer.shadowPos.Length; k++)
			{
				mountedPlayer.shadowPos[k].Y += mountedPlayer.height;
			}
			mountedPlayer.height = 42 + _data.heightBoost;
			mountedPlayer.position.Y -= mountedPlayer.height;
			for (int l = 0; l < mountedPlayer.shadowPos.Length; l++)
			{
				mountedPlayer.shadowPos[l].Y -= mountedPlayer.height;
			}
		}
		mountedPlayer.ResetAdvancedShadows();
		if (_type == 7 || _type == 8)
		{
			mountedPlayer.fullRotationOrigin = new Vector2((float)(mountedPlayer.width / 2), (float)(mountedPlayer.height / 2));
		}
		int type = _type;
		if ((uint)(type - 62) <= 3u)
		{
			SoundEngine.PlaySound(SoundID.PalChillet, mountedPlayer.Center);
		}
		if (_type == 8)
		{
			_mountSpecificData = new DrillMountData();
		}
		if (_type == 35)
		{
			_mountSpecificData = new ExtraFrameMountData();
		}
		if (_type == 54)
		{
			_mountSpecificData = new SelectiveFlyingMountData();
		}
		FinalizeMountData(m, mountedPlayer);
		DoSpawnDust(mountedPlayer, isDismounting: false);
		if (_type == 38 && mountedPlayer.whoAmI == Main.myPlayer)
		{
			AchievementsHelper.NotifyProgressionEvent(32);
		}
	}

	public void FinalizeMountData(int m, Player mountedPlayer)
	{
		if (_type == 8 && mountedPlayer.isDisplayDollOrInanimate)
		{
			float diodeRotation = 0f;
			if (mountedPlayer.direction == -1)
			{
				diodeRotation = (float)Math.PI;
			}
			((DrillMountData)_mountSpecificData).diodeRotation = diodeRotation;
		}
	}

	public void DoFailedDismountDust(Player mountedPlayer, int dustAmount)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 2)
		{
			return;
		}
		Color newColor = Color.Transparent;
		if (_type == 56)
		{
			newColor = Color.Black;
		}
		if (_type == 61)
		{
			newColor = Projectile.GetFairyQueenWeaponsColorFull(mountedPlayer.whoAmI, mountedPlayer.Center, 0.27f, 1f, 0.15f, 1f, 0.7f);
		}
		Rectangle hitbox = mountedPlayer.Hitbox;
		hitbox.Inflate(10, 10);
		for (int i = 0; i < dustAmount; i++)
		{
			int spawnDust = _data.spawnDust;
			float scale = 1f;
			int alpha = 0;
			int num = Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, spawnDust, 0f, 0f, alpha, newColor, scale);
			Main.dust[num].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
			if (_type == 56)
			{
				Dust obj = Main.dust[num];
				obj.scale = 0.7f;
				obj.velocity *= 0.4f;
				obj.velocity.Y += mountedPlayer.gravDir * 0.5f;
			}
			if (_type == 61)
			{
				Dust obj2 = Main.dust[num];
				obj2.scale *= 1f + 0.3f * Main.rand.NextFloat();
				obj2.velocity += mountedPlayer.velocity * Main.rand.NextFloat();
			}
			if (_data.spawnDustNoGravity)
			{
				Main.dust[num].noGravity = true;
			}
			else if (Main.rand.Next(2) == 0)
			{
				Main.dust[num].scale *= 1.3f;
				Main.dust[num].noGravity = true;
			}
			else
			{
				Dust obj3 = Main.dust[num];
				obj3.velocity *= 0.5f;
			}
			Dust obj4 = Main.dust[num];
			obj4.velocity += mountedPlayer.velocity * 0.8f;
		}
	}

	private void DoSpawnDust(Player mountedPlayer, bool isDismounting)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_0959: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_083b: Unknown result type (might be due to invalid IL or missing references)
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 2)
		{
			return;
		}
		if (_type == 52)
		{
			for (int i = 0; i < 100; i++)
			{
				_ = _data.spawnDust;
				Dust dust = Dust.NewDustDirect(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), mountedPlayer.width + 40, mountedPlayer.height, 267, 0f, 0f, 60, new Color(130, 60, 255, 70));
				dust.scale += (float)Main.rand.Next(-10, 21) * 0.01f;
				dust.noGravity = true;
				dust.velocity += mountedPlayer.velocity * 0.8f;
				dust.velocity *= Main.rand.NextFloat();
				dust.velocity.Y += 2f * Main.rand.NextFloatDirection();
				dust.noLight = true;
				if (Main.rand.Next(3) == 0)
				{
					Dust dust2 = Dust.CloneDust(dust);
					dust2.color = Color.White;
					dust2.scale *= 0.5f;
					dust2.alpha = 0;
				}
			}
			return;
		}
		if (_type == 62 || _type == 63 || _type == 64 || _type == 65)
		{
			for (int j = 0; j < 100; j++)
			{
				_ = _data.spawnDust;
				Dust dust3 = Dust.NewDustDirect(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), mountedPlayer.width + 40, mountedPlayer.height, 306, 0f, 0f, 60, new Color(100, 227, 255, 127), 2f);
				dust3.scale += (float)Main.rand.Next(-10, 21) * 0.01f;
				dust3.noGravity = true;
				dust3.velocity += mountedPlayer.velocity * 0.8f;
				dust3.velocity *= Main.rand.NextFloat();
				dust3.velocity.Y += 4f * Main.rand.NextFloatDirection();
				dust3.noLight = true;
				if (Main.rand.Next(3) == 0)
				{
					Dust dust4 = Dust.CloneDust(dust3);
					dust4.color = Color.White;
					dust4.scale *= 0.5f;
					dust4.alpha = 0;
				}
			}
			return;
		}
		Color newColor = Color.Transparent;
		if (_type == 23)
		{
			newColor = new Color(255, 255, 0, 255);
		}
		if (_type == 56)
		{
			newColor = Color.Black;
		}
		if (_type == 61)
		{
			newColor = Projectile.GetFairyQueenWeaponsColorFull(mountedPlayer.whoAmI, mountedPlayer.Center, 0.27f, 1f, 0.15f, 1f, 0.7f);
		}
		int num = 100;
		if (_type > 0 && MountID.Sets.IsRollerSkates[_type])
		{
			num = 40;
		}
		Rectangle val = new Rectangle((int)mountedPlayer.position.X - 20, (int)mountedPlayer.position.Y, mountedPlayer.width + 40, mountedPlayer.height);
		if (_type == 56 || _type == 61)
		{
			if (isDismounting)
			{
				Vector2 bottom = mountedPlayer.Bottom;
				val = new Rectangle((int)bottom.X - (int)mountedPlayer.DefaultSize.X / 2, (int)bottom.Y - (int)mountedPlayer.DefaultSize.Y, (int)mountedPlayer.DefaultSize.X, (int)mountedPlayer.DefaultSize.Y);
			}
			else
			{
				val = mountedPlayer.Hitbox;
			}
			val.Inflate(10, 10);
		}
		if (_type > 0 && MountID.Sets.IsRollerSkates[_type])
		{
			int num2 = 10;
			int num3 = mountedPlayer.width / 2;
			int num4 = 10;
			val = new Rectangle((int)mountedPlayer.Center.X - num3 - num2, (int)mountedPlayer.Bottom.Y - num4, Math.Max(1, num3 + num2 * 2), num4);
		}
		for (int k = 0; k < num; k++)
		{
			if (MountID.Sets.Cart[_type])
			{
				if (k % 10 == 0)
				{
					int type = Main.rand.Next(61, 64);
					int num5 = Gore.NewGore(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), Vector2.Zero, type);
					Main.gore[num5].alpha = 100;
					Main.gore[num5].velocity = Vector2.Transform(new Vector2(1f, 0f), Matrix.CreateRotationZ((float)(Main.rand.NextDouble() * 6.2831854820251465)));
				}
				continue;
			}
			int type2 = _data.spawnDust;
			float scale = 1f;
			int alpha = 0;
			if (_type == 40 || _type == 41 || _type == 42)
			{
				type2 = ((Main.rand.Next(2) != 0) ? 16 : 31);
				scale = 0.9f;
				alpha = 50;
				if (_type == 42)
				{
					type2 = 31;
				}
				if (_type == 41)
				{
					type2 = 16;
				}
			}
			if (_type == 61 && Main.rand.Next(isDismounting ? 2 : 5) != 0)
			{
				continue;
			}
			int num6 = Dust.NewDust(val.TopLeft(), val.Width, val.Height, type2, 0f, 0f, alpha, newColor, scale);
			Main.dust[num6].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
			if (_type > 0 && MountID.Sets.IsRollerSkates[_type])
			{
				Dust obj = Main.dust[num6];
				obj.velocity *= 0.8f;
				obj.velocity.Y *= 0.75f;
				obj.velocity.Y += mountedPlayer.gravDir * 0.25f;
				obj.noLightEmittance = true;
				obj.noGravity = true;
			}
			if (_type == 56)
			{
				Dust obj2 = Main.dust[num6];
				obj2.scale = 0.7f;
				obj2.velocity *= 0.4f;
				obj2.velocity.Y += mountedPlayer.gravDir * 0.5f;
			}
			if (_type == 61)
			{
				Dust dust5 = Main.dust[num6];
				dust5.scale *= 1f + 0.3f * Main.rand.NextFloat();
				if (!isDismounting)
				{
					dust5.position = Vector2.Lerp(dust5.position, val.Center.ToVector2(), 0.7f);
					dust5.velocity *= -2f;
					dust5.velocity.Y--;
				}
				dust5.velocity += mountedPlayer.velocity * Main.rand.NextFloat();
			}
			if (_data.spawnDustNoGravity)
			{
				Main.dust[num6].noGravity = true;
			}
			else if (Main.rand.Next(2) == 0)
			{
				Main.dust[num6].scale *= 1.3f;
				Main.dust[num6].noGravity = true;
			}
			else
			{
				Dust obj3 = Main.dust[num6];
				obj3.velocity *= 0.5f;
			}
			Dust obj4 = Main.dust[num6];
			obj4.velocity += mountedPlayer.velocity * 0.8f;
			if (_type == 40 || _type == 41 || _type == 42)
			{
				Dust obj5 = Main.dust[num6];
				obj5.velocity *= Main.rand.NextFloat();
			}
		}
		if (_type == 40 || _type == 41 || _type == 42)
		{
			for (int l = 0; l < 5; l++)
			{
				int type3 = Main.rand.Next(61, 64);
				if (_type == 41 || (_type == 40 && Main.rand.Next(2) == 0))
				{
					type3 = Main.rand.Next(11, 14);
				}
				int num7 = Gore.NewGore(new Vector2(mountedPlayer.position.X + (float)(mountedPlayer.direction * 8), mountedPlayer.position.Y + 20f), Vector2.Zero, type3);
				Main.gore[num7].alpha = 100;
				Main.gore[num7].velocity = Vector2.Transform(new Vector2(1f, 0f), Matrix.CreateRotationZ((float)(Main.rand.NextDouble() * 6.2831854820251465))) * 1.4f;
			}
		}
		if (_type == 23)
		{
			for (int m = 0; m < 4; m++)
			{
				int type4 = Main.rand.Next(61, 64);
				int num8 = Gore.NewGore(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), Vector2.Zero, type4);
				Main.gore[num8].alpha = 100;
				Main.gore[num8].velocity = Vector2.Transform(new Vector2(1f, 0f), Matrix.CreateRotationZ((float)(Main.rand.NextDouble() * 6.2831854820251465)));
			}
		}
	}

	public static bool DismountsOnItemUse(int mountType)
	{
		return mounts[mountType].dismountsOnItemUse;
	}

	public bool CanVisuallyHoldItem(Player mountedPlayer, Item item)
	{
		if (Type >= 0 && MountID.Sets.DontHoldItems[Type])
		{
			return false;
		}
		return true;
	}

	public bool CanMount(int m, Player mountingPlayer)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		MountData obj = mounts[m];
		Vector2 val = mountingPlayer.Size + new Vector2(0f, (float)mounts[m].heightBoost);
		MountDelegatesData.OverrideSizeMethod playerSize = obj.delegations.PlayerSize;
		if (playerSize != null && playerSize(mountingPlayer, out var size) && size.HasValue)
		{
			val.X = (int)size.Value.X;
			val.Y = (int)size.Value.Y;
		}
		if (!mountingPlayer.CanFitInSpaceWithSize(val))
		{
			return false;
		}
		if (m == 56 && (mountingPlayer.wet || mountingPlayer.dripping || Collision.WetCollision(mountingPlayer.Bottom - val * 0.5f, (int)val.X, (int)val.Y)))
		{
			return false;
		}
		if (!MountID.Sets.Cart[m] && !MountID.Sets.CanUseHooks[m] && mountingPlayer.grappling[0] >= 0)
		{
			return false;
		}
		return true;
	}

	public bool CanDismount(Player mountingPlayer)
	{
		return CanDismountWithResult(mountingPlayer) == DismountCheckResult.Succeeded;
	}

	public DismountCheckResult CanDismountWithResult(Player mountingPlayer)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		DismountCheckResult result = DismountCheckResult.Succeeded;
		Rectangle changedHitbox;
		if (MountID.Sets.DontDismountWhenCCed[Type] && mountingPlayer.CCed)
		{
			result = DismountCheckResult.FailedCCed;
		}
		else if (!Collision.TryChangingSizeFromBottomCenter(mountingPlayer.Hitbox, 20, 42, out changedHitbox))
		{
			result = DismountCheckResult.FailedNoSpace;
		}
		return result;
	}

	public void TryEarlyDismount(Player player)
	{
		if (Active && _data.dismountsOnItemUse && CanDismount(player))
		{
			Dismount(player);
		}
	}

	public bool FindTileHeight(Vector2 position, int maxTilesDown, out float tileHeight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)(position.X / 16f);
		int num2 = (int)(position.Y / 16f);
		for (int i = 0; i <= maxTilesDown; i++)
		{
			Tile tile = Main.tile[num, num2];
			bool flag = Main.tileSolid[tile.type];
			bool flag2 = Main.tileSolidTop[tile.type];
			if (!tile.active() || !flag || flag2)
			{
			}
			num2++;
		}
		tileHeight = 0f;
		return true;
	}

	static Mount()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
	}
}
