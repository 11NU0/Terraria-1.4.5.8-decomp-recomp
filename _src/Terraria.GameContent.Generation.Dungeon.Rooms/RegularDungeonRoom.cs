using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class RegularDungeonRoom : DungeonRoom
{
	public int _innerBoundsSize;

	public RegularDungeonRoom(DungeonRoomSettings settings)
		: base(settings)
	{
	}

	public override void CalculateRoom(DungeonData data)
	{
		calculated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		RegularRoom(data, x, y, generating: false);
		calculated = true;
	}

	public override bool GenerateRoom(DungeonData data)
	{
		generated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		RegularRoom(data, x, y, generating: true);
		generated = true;
		return true;
	}

	public void RegularRoom(DungeonData data, int i, int j, bool generating)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom unifiedRandom = new UnifiedRandom(settings.RandomSeed);
		RegularDungeonRoomSettings regularDungeonRoomSettings = (RegularDungeonRoomSettings)settings;
		Point val = new Point(i, j);
		if (Processed)
		{
			val = InnerBounds.Center;
		}
		int num = 6 + unifiedRandom.Next(7);
		int num2 = 8;
		if (regularDungeonRoomSettings.OverrideInnerBoundsSize > 0)
		{
			num = regularDungeonRoomSettings.OverrideInnerBoundsSize;
		}
		if (regularDungeonRoomSettings.OverrideOuterBoundsSize > 0)
		{
			num2 = regularDungeonRoomSettings.OverrideOuterBoundsSize;
		}
		if (Processed)
		{
			num = _innerBoundsSize;
		}
		int totalBoundsSize = num + num2;
		InnerBounds.SetBounds(val.X, val.Y, val.X, val.Y);
		OuterBounds.SetBounds(val.X, val.Y, val.X, val.Y);
		GenerateDungeonSquareRoom(data, InnerBounds, OuterBounds, (Vector2D)(val), settings.StyleData, num, totalBoundsSize, generating, generating);
		_innerBoundsSize = num;
		InnerBounds.CalculateHitbox();
		OuterBounds.CalculateHitbox();
	}
}
