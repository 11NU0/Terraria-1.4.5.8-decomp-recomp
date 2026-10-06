using System;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.CameraModifiers;

public class FixedMovePerFrameCameraModifier : ICameraModifier
{
	private Vector2 _startPosition;

	private Vector2 _offset;

	private float _movePerFrameX;

	private float _movePerFrameY;

	public string UniqueIdentity { get; private set; }

	public bool Finished { get; private set; }

	public bool IsAScreenShake { get; set; }

	public FixedMovePerFrameCameraModifier(Vector2 startPosition, Vector2 offset, float movePerFrameX, float movePerFrameY, string uniqueIdentity = null)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		_startPosition = startPosition;
		_offset = offset;
		_movePerFrameX = movePerFrameX;
		_movePerFrameY = movePerFrameY;
		UniqueIdentity = uniqueIdentity;
		IsAScreenShake = true;
	}

	public void Update(ref CameraInfo cameraInfo)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		ref Vector2 cameraPosition = ref cameraInfo.CameraPosition;
		cameraPosition += _offset;
		if (_offset.X > 0f)
		{
			_offset.X = Math.Max(0f, _offset.X - _movePerFrameX);
		}
		if (_offset.X < 0f)
		{
			_offset.X = Math.Min(0f, _offset.X + _movePerFrameX);
		}
		if (_offset.Y > 0f)
		{
			_offset.Y = Math.Max(0f, _offset.Y - _movePerFrameY);
		}
		if (_offset.Y < 0f)
		{
			_offset.Y = Math.Min(0f, _offset.Y + _movePerFrameY);
		}
		if (_offset == Vector2.Zero)
		{
			Finished = true;
		}
	}
}
