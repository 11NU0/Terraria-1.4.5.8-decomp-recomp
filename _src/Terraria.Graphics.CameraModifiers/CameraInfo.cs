using Microsoft.Xna.Framework;

namespace Terraria.Graphics.CameraModifiers;

public struct CameraInfo
{
	public Vector2 CameraPosition;

	public Vector2 OriginalCameraCenter;

	public Vector2 OriginalCameraPosition;

	public CameraInfo(Vector2 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		OriginalCameraPosition = position;
		OriginalCameraCenter = position + Main.ScreenSize.ToVector2() / 2f;
		CameraPosition = OriginalCameraPosition;
	}
}
