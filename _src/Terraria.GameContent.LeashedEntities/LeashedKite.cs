using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.LeashedEntities;

public class LeashedKite : LeashedEntity
{
	public static LeashedKite Prototype;

	private static Projectile _dummy = new Projectile();

	public int projType;

	public int frame;

	public int frameCounter;

	public float rotation;

	public int spriteDirection = 1;

	public float kiteDistance = 250f;

	public float windTarget;

	public float windCurrent;

	public float timeCounter;

	public float cloudAlpha;

	public int timeWithoutWind;

	public float projectileLocalAI0;

	public float projectileLocalAI1;

	public Vector2[] oldPos;

	public float[] oldRot;

	public int[] oldSpriteDirection;

	public Vector2 netOffset;

	private Vector2 AnchorWorldPosition
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return AnchorPosition.ToWorldCoordinates();
		}
	}

	public void SetDefaults(int projType)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		this.projType = projType;
		_dummy.SetDefaults(projType);
		Size = _dummy.Size;
	}

	public override void NetSend(BinaryWriter writer, bool full)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (full)
		{
			writer.Write7BitEncodedInt(projType);
		}
		writer.WriteVector2(position);
		writer.WritePackedVector2(velocity);
		writer.Write((byte)((double)(rotation * 256f) / (Math.PI * 2.0)));
		writer.Write(windTarget);
		writer.Write(cloudAlpha);
		writer.Write(timeCounter);
	}

	public override void NetReceive(BinaryReader reader, bool full)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		if (full)
		{
			SetDefaults(reader.Read7BitEncodedInt());
		}
		Vector2 val = position;
		position = reader.ReadVector2();
		velocity = reader.ReadPackedVector2();
		rotation = (float)((double)(int)reader.ReadByte() * Math.PI * 2.0 / 256.0);
		windTarget = reader.ReadSingle();
		cloudAlpha = reader.ReadSingle();
		timeCounter = reader.ReadSingle();
		if (full)
		{
			netOffset = Vector2.Zero;
		}
		else
		{
			netOffset += val - position;
		}
		if (full)
		{
			Update();
			FixFirstTimeAppearance();
		}
	}

	private void FixFirstTimeAppearance()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.InAPlaceWithWind(position, width, height))
		{
			projectileLocalAI0 = 300f;
			projectileLocalAI1 = 1f;
		}
	}

	public override void Draw()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadProjectile(projType);
		CopyToDummy();
		Projectile dummy = _dummy;
		dummy.position += netOffset;
		Main.DrawKite(_dummy, AnchorWorldPosition, windCurrent);
	}

	public override void Update()
	{
		Update(fastForward: false);
	}

	public void Update(bool fastForward)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		if (oldPos == null)
		{
			int num = ProjectileID.Sets.TrailCacheLength[projType];
			oldPos = new Vector2[num];
			oldRot = new float[num];
			oldSpriteDirection = new int[num];
		}
		if (NearbySectionsMissing())
		{
			return;
		}
		if (fastForward || Vector2.DistanceSquared(position, oldPos[0]) > 256f)
		{
			for (int i = 0; i < oldPos.Length; i++)
			{
				oldPos[i] = position;
				oldRot[i] = rotation;
				oldSpriteDirection[i] = spriteDirection;
			}
		}
		if (Main.netMode != 1)
		{
			windTarget = Main.WindForVisuals;
			cloudAlpha = Main.cloudAlpha;
		}
		if (WorldGen.InAPlaceWithWind(position, width, height))
		{
			windCurrent = (fastForward ? windTarget : MathHelper.Lerp(windCurrent, windTarget, 0.05f));
		}
		else
		{
			windTarget = 0f;
		}
		bool flag = Math.Abs(windCurrent) >= 0.2f;
		timeWithoutWind = ((!flag) ? (fastForward ? 3600 : (timeWithoutWind + 1)) : 0);
		kiteDistance = Utils.Remap(timeWithoutWind, 120f, 420f, 250f, 48f);
		MoveKite(fastForward);
		netOffset = netOffset.MoveTowards(Vector2.Zero, 2f);
	}

	private void MoveKite(bool fastForward = false)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		CopyToDummy();
		_dummy.owner = 255;
		Player player = Main.player[255];
		Vector2 val = (player.Center = AnchorWorldPosition);
		if (timeWithoutWind == 0)
		{
			int num = ((!(_dummy.Center.X - val.X < 0f)) ? 1 : (-1));
			_dummy.spriteDirection = num;
			player.direction = num;
		}
		timeCounter += 1f / 60f;
		KiteFlyingInfo info = new KiteFlyingInfo
		{
			BobOffset = (val.X + val.Y * 0.92f) * 0.0025f,
			WindInWorld = windCurrent,
			CloudAlpha = cloudAlpha,
			GlobalTime = timeCounter,
			CanReelThroughBlocks = false
		};
		if (fastForward)
		{
			info.GlobalTime = 0f;
			info.BobOffset = 0f;
			_dummy.localAI[0] = 0f;
			_dummy.localAI[1] = 0f;
			Vector2 val2 = _dummy.position;
			_dummy.position = val + new Vector2((float)Math.Sign(info.WindInWorld), -3f + 2f * info.CloudAlpha).SafeNormalize(Vector2.Zero) * (kiteDistance + 2f);
			_dummy.velocity = Vector2.Zero;
			info.CanReelThroughBlocks = true;
			for (int num2 = 10; num2 > 0; num2--)
			{
				_dummy.KiteLogic(val, info);
				Projectile dummy = _dummy;
				dummy.position += _dummy.velocity * (float)num2;
			}
			info.CanReelThroughBlocks = false;
			Vector2 val3 = _dummy.velocity;
			Vector2 val4 = _dummy.position - val;
			_dummy.position = val2;
			if (val3.Y < 0f)
			{
				int num3 = (int)Math.Ceiling(kiteDistance / 16f);
				Vector2 val5 = val4 / (float)num3;
				for (int i = 0; i < num3; i++)
				{
					_dummy.HandleMovement(_dummy.velocity = val5);
				}
				_dummy.velocity = val3;
			}
			else
			{
				_dummy.localAI[0] = 300f;
				_dummy.localAI[1] = 1f;
				_dummy.HandleMovement(_dummy.velocity = new Vector2((float)(((info.WindInWorld >= 0f) ? 1 : (-1)) * 16), 8f));
				_dummy.velocity = Vector2.Zero;
			}
			for (int num4 = oldPos.Length - 1; num4 >= 0; num4--)
			{
				oldPos[num4] = _dummy.position;
				oldRot[num4] = _dummy.rotation;
				oldSpriteDirection[num4] = _dummy.spriteDirection;
			}
		}
		else
		{
			Utils.Shift(oldPos, 1);
			Utils.Shift(oldRot, 1);
			Utils.Shift(oldSpriteDirection, 1);
			oldPos[0] = position;
			oldRot[0] = rotation;
			oldSpriteDirection[0] = spriteDirection;
			_dummy.KiteLogic(val, info);
			_dummy.HandleMovement(_dummy.velocity);
			_dummy.GetCollisionParams(out var resizeAnchor, out var colWidth, out var colHeight);
			if (Collision.SolidFullTiles(_dummy.Center - new Vector2((float)colWidth, (float)colHeight) * resizeAnchor, new Vector2((float)colWidth, (float)colHeight)))
			{
				_dummy.Bottom = _dummy.Bottom.MoveTowards(val, 2f);
			}
		}
		CopyFromDummy();
	}

	public override void Spawn(bool newlyAdded)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		CopyToDummy();
		_dummy.GetCollisionParams(out var resizeAnchor, out var colWidth, out var colHeight);
		Center = AnchorWorldPosition - new Vector2((float)colWidth, (float)colHeight) * (new Vector2(0.5f, 1f) - resizeAnchor);
		if (newlyAdded)
		{
			velocity = new Vector2(0f, -5f);
		}
		else
		{
			velocity = Vector2.Zero;
		}
		rotation = 0f;
		windCurrent = (windTarget = Main.WindForVisuals);
		cloudAlpha = Main.cloudAlpha;
		Update(!newlyAdded);
	}

	private void CopyToDummy()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		_dummy.type = projType;
		_dummy.Size = Size;
		_dummy.frame = frame;
		_dummy.frameCounter = frameCounter;
		_dummy.position = position;
		_dummy.velocity = velocity;
		_dummy.rotation = rotation;
		_dummy.spriteDirection = spriteDirection;
		_dummy.oldPos = oldPos;
		_dummy.oldRot = oldRot;
		_dummy.oldSpriteDirection = oldSpriteDirection;
		_dummy.scale = 1f;
		_dummy.ai[0] = kiteDistance;
		_dummy.localAI[0] = projectileLocalAI0;
		_dummy.localAI[1] = projectileLocalAI1;
		_dummy.extraUpdates = 0;
		_dummy.correctSlopeCollision = true;
	}

	private void CopyFromDummy()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		frame = _dummy.frame;
		frameCounter = _dummy.frameCounter;
		position = _dummy.position;
		velocity = _dummy.velocity;
		rotation = _dummy.rotation;
		spriteDirection = _dummy.spriteDirection;
		projectileLocalAI0 = _dummy.localAI[0];
		projectileLocalAI1 = _dummy.localAI[1];
	}
}
