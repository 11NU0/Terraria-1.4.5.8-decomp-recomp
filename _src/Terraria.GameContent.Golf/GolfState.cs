using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent.Golf;

public class GolfState
{
	private const int BALL_RETURN_PENALTY = 1;

	private int golfScoreTime;

	private int golfScoreTimeMax = 3600;

	private int golfScoreDelay = 90;

	private double _lastRecordedBallTime;

	private Vector2? _lastRecordedBallLocation;

	private bool _waitingForBallToSettle;

	private Vector2 _lastSwingPosition;

	private Projectile _lastHitGolfBall;

	private int _lastRecordedSwingCount;

	private GolfBallTrackRecord[] _hitRecords = new GolfBallTrackRecord[1000];

	public float ScoreAdjustment => (float)golfScoreTime / (float)golfScoreTimeMax;

	public bool ShouldScoreHole => golfScoreTime >= golfScoreDelay;

	private void UpdateScoreTime()
	{
		if (golfScoreTime < golfScoreTimeMax)
		{
			golfScoreTime++;
		}
	}

	public void ResetScoreTime()
	{
		golfScoreTime = 0;
	}

	public void SetScoreTime()
	{
		golfScoreTime = golfScoreTimeMax;
	}

	public bool TryGetCameraTrackingPosition(out Vector2 cameraPosition)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		Projectile lastHitBall = GetLastHitBall();
		if (lastHitBall != null && _waitingForBallToSettle)
		{
			cameraPosition = lastHitBall.Center;
			return true;
		}
		if (_lastRecordedBallTime + 2.0 >= Main.gameTimeCache.TotalGameTime.TotalSeconds && lastHitBall == null && _lastRecordedBallLocation.HasValue)
		{
			cameraPosition = _lastRecordedBallLocation.Value;
			return true;
		}
		cameraPosition = default;
		return false;
	}

	public void WorldClear()
	{
		_lastHitGolfBall = null;
		_lastRecordedBallLocation = null;
		_lastRecordedBallTime = 0.0;
		_lastRecordedSwingCount = 0;
		_waitingForBallToSettle = false;
	}

	public void CancelBallTracking()
	{
		_waitingForBallToSettle = false;
	}

	public void RecordSwing(Projectile golfBall)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		_lastSwingPosition = golfBall.position;
		_lastHitGolfBall = golfBall;
		_lastRecordedSwingCount = (int)golfBall.ai[1];
		_waitingForBallToSettle = true;
		int golfBallId = GetGolfBallId(golfBall);
		if (_hitRecords[golfBallId] == null || _lastRecordedSwingCount == 1)
		{
			_hitRecords[golfBallId] = new GolfBallTrackRecord();
		}
		_hitRecords[golfBallId].RecordHit(golfBall.position);
	}

	private int GetGolfBallId(Projectile golfBall)
	{
		return golfBall.whoAmI;
	}

	public Projectile GetLastHitBall()
	{
		if (_lastHitGolfBall == null || !_lastHitGolfBall.active || !ProjectileID.Sets.IsAGolfBall[_lastHitGolfBall.type] || _lastHitGolfBall.owner != Main.myPlayer || _lastRecordedSwingCount != (int)_lastHitGolfBall.ai[1])
		{
			return null;
		}
		return _lastHitGolfBall;
	}

	public void Update()
	{
		UpdateScoreTime();
		Projectile lastHitBall = GetLastHitBall();
		if (lastHitBall == null)
		{
			_waitingForBallToSettle = false;
			return;
		}
		if (_waitingForBallToSettle)
		{
			_waitingForBallToSettle = (int)lastHitBall.localAI[1] == 1;
		}
		bool flag = false;
		int type = Main.LocalPlayer.HeldItem.type;
		if (type == 3611)
		{
			flag = true;
		}
		if (!Item.IsAGolfingItem(Main.LocalPlayer.HeldItem) && !flag)
		{
			_waitingForBallToSettle = false;
		}
	}

	public void RecordBallInfo(Projectile golfBall)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (GetLastHitBall() == golfBall && _waitingForBallToSettle)
		{
			_lastRecordedBallLocation = golfBall.Center;
			_lastRecordedBallTime = Main.gameTimeCache.TotalGameTime.TotalSeconds;
		}
	}

	public void LandBall(Projectile golfBall)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		int golfBallId = GetGolfBallId(golfBall);
		_hitRecords[golfBallId]?.RecordHit(golfBall.position);
	}

	public int GetGolfBallScore(Projectile golfBall)
	{
		int golfBallId = GetGolfBallId(golfBall);
		GolfBallTrackRecord golfBallTrackRecord = _hitRecords[golfBallId];
		if (golfBallTrackRecord == null)
		{
			return 0;
		}
		return (int)((float)golfBallTrackRecord.GetAccumulatedScore() * ScoreAdjustment);
	}

	public void ResetGolfBall()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		Projectile lastHitBall = GetLastHitBall();
		if (lastHitBall != null && !(Vector2.Distance(lastHitBall.position, _lastSwingPosition) < 1f))
		{
			lastHitBall.position = _lastSwingPosition;
			lastHitBall.velocity = Vector2.Zero;
			lastHitBall.ai[1]++;
			lastHitBall.netUpdate2 = true;
			_lastRecordedSwingCount = (int)lastHitBall.ai[1];
		}
	}
}
