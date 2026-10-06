using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameInput;

public class SmartSelectGamepadPointer
{
	private Vector2 _size;

	private Vector2 _center;

	private Vector2 _distUniform = new Vector2(80f, 64f);

	public bool ShouldBeUsed()
	{
		if (PlayerInput.UsingGamepad && Main.LocalPlayer.controlTorch)
		{
			return Main.SmartCursorIsUsed;
		}
		return false;
	}

	public void SmartSelectLookup_GetTargetTile(Player player, out int tX, out int tY)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		tX = (int)(((float)Main.mouseX + Main.screenPosition.X) / 16f);
		tY = (int)(((float)Main.mouseY + Main.screenPosition.Y) / 16f);
		if (player.gravDir == -1f)
		{
			tY = (int)((Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY) / 16f);
		}
		if (ShouldBeUsed())
		{
			Point val = GetPointerPosition().ToPoint();
			tX = (int)(((float)val.X + Main.screenPosition.X) / 16f);
			tY = (int)(((float)val.Y + Main.screenPosition.Y) / 16f);
			if (player.gravDir == -1f)
			{
				tY = (int)((Main.screenPosition.Y + (float)Main.screenHeight - (float)val.Y) / 16f);
			}
		}
	}

	public void UpdateSize(Vector2 size)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_size = size;
	}

	public void UpdateCenter(Vector2 center)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_center = center;
	}

	public Vector2 GetPointerPosition()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = (new Vector2((float)Main.mouseX, (float)Main.mouseY) - _center) / _size;
		float num = Math.Abs(val.X);
		if (num < Math.Abs(val.Y))
		{
			num = Math.Abs(val.Y);
		}
		if (num > 1f)
		{
			val /= num;
		}
		val *= Main.GameViewMatrix.RenderZoom.X;
		return val * _distUniform + _center;
	}

	public SmartSelectGamepadPointer()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
	}
}
