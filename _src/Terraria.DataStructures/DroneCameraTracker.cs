using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public class DroneCameraTracker
{
	private Projectile _trackedProjectile;

	private int _lastTrackedType;

	public void Track(Projectile proj)
	{
		_trackedProjectile = proj;
		_lastTrackedType = proj.type;
	}

	public void WorldClear()
	{
		_lastTrackedType = 0;
		_trackedProjectile = null;
	}

	public bool TryTracking(out Vector2 cameraPosition)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		cameraPosition = default;
		if (_trackedProjectile == null || !_trackedProjectile.active || _trackedProjectile.type != _lastTrackedType || _trackedProjectile.owner != Main.myPlayer || !Main.LocalPlayer.remoteVisionForDrone)
		{
			_trackedProjectile = null;
			return false;
		}
		cameraPosition = _trackedProjectile.Center;
		return true;
	}
}
