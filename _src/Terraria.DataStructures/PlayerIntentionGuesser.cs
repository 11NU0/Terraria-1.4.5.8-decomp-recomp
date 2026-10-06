using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria.DataStructures;

public class PlayerIntentionGuesser
{
	public int LastX;

	public int LastY;

	public Vector2 LastPosition;

	public Vector2 LastCenter;

	public Vector2 LastMouse;

	public int LastDirection;

	public int LastWidth;

	public GuessedPlayerIntention Intention;

	public SmartCursorHelper.SmartCursorUsageInfo UsageProxy = new SmartCursorHelper.SmartCursorUsageInfo();

	public int TimeWithIntention;

	public int PlayerActiveActionTimeLeft;

	public void Track(Player player, int x, int y, GuessedPlayerIntention intention)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (PlayerActiveActionTimeLeft != 0)
		{
			LastX = x;
			LastY = y;
			Intention = intention;
			LastPosition = player.position;
			LastCenter = player.Center;
			LastDirection = player.direction;
			LastWidth = player.width;
			LastMouse = Main.MouseWorld;
		}
	}

	public void AllowTracking(int time = 60)
	{
		PlayerActiveActionTimeLeft = time;
	}

	public void Update(Player player)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (player.whoAmI != Main.myPlayer)
		{
			return;
		}
		TimeWithIntention++;
		if (PlayerActiveActionTimeLeft > 0)
		{
			PlayerActiveActionTimeLeft--;
		}
		if (Intention != GuessedPlayerIntention.None)
		{
			float num = player.Center.Distance(LastCenter);
			bool flag = false;
			if (num > 80f)
			{
				flag = true;
			}
			if (player.controlJump)
			{
				flag = true;
			}
			bool usingOrReusingItem = player.UsingOrReusingItem;
			if (usingOrReusingItem && Intention == GuessedPlayerIntention.HarvestTreasure && player.HeldItem.pick <= 0)
			{
				flag = true;
			}
			if (usingOrReusingItem && Intention == GuessedPlayerIntention.HarvestTrees && player.HeldItem.axe <= 0)
			{
				flag = true;
			}
			if (TimeWithIntention >= 480)
			{
				flag = true;
			}
			if (player.dead)
			{
				flag = true;
			}
			if (flag)
			{
				Intention = GuessedPlayerIntention.None;
				TimeWithIntention = 0;
			}
		}
	}

	public void PrepareUsageProxy(Player player, int itemType, int areaInflateWidth, int areaInflateHeight)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		UsageProxy.player = player;
		if (UsageProxy.item == null)
		{
			UsageProxy.item = new Item();
		}
		UsageProxy.item.SetDefaults(itemType);
		UsageProxy.position = LastPosition;
		UsageProxy.Center = LastCenter;
		UsageProxy.mouse = LastMouse;
		UsageProxy.screenTargetX = LastX;
		UsageProxy.screenTargetY = LastY;
		UsageProxy.screenTargetX = Utils.Clamp(UsageProxy.screenTargetX, 10, Main.maxTilesX - 10);
		UsageProxy.screenTargetY = Utils.Clamp(UsageProxy.screenTargetY, 10, Main.maxTilesY - 10);
		Rectangle val = new Rectangle(LastX, LastY, 1, 1);
		val.Inflate(areaInflateWidth, areaInflateHeight);
		Rectangle val2 = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
		val2.Inflate(-10, -10);
		Rectangle val3 = default;
		val3 = Rectangle.Intersect(val, val2);
		UsageProxy.reachableStartX = val3.Left;
		UsageProxy.reachableStartY = val3.Top;
		UsageProxy.reachableEndX = val3.Right;
		UsageProxy.reachableEndY = val3.Bottom;
	}
}
