using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.ItemDropRules;

public static class CommonCode
{
	public static void DropItemFromNPC(NPC npc, int itemId, int stack, bool scattered = false)
	{
		if (itemId > 0 && itemId < ItemID.Count)
		{
			int x = (int)npc.position.X + npc.width / 2;
			int y = (int)npc.position.Y + npc.height / 2;
			if (scattered)
			{
				x = (int)npc.position.X + Main.rand.Next(npc.width + 1);
				y = (int)npc.position.Y + Main.rand.Next(npc.height + 1);
			}
			int itemIndex = Item.NewItem(npc.GetItemSource_Loot(), x, y, 0, 0, itemId, stack, noBroadcast: false, -1);
			ModifyItemDropFromNPC(npc, itemIndex);
		}
	}

	public static void DropItemLocalPerClientAndSetNPCMoneyTo0(NPC npc, int itemId, int stack, bool interactionRequired = true)
	{
		if (itemId > 0 && itemId < ItemID.Count)
		{
			int num = Item.NewItem(npc.GetItemSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, itemId, stack, noBroadcast: true, -1);
			Main.item[num].MakeInstanced((Player p) => npc.playerInteraction[p.whoAmI] || !interactionRequired);
			npc.value = 0f;
		}
	}

	public static void DropItemForEachInteractingPlayerOnThePlayer(NPC npc, int itemId, UnifiedRandom rng, int chanceNumerator, int chanceDenominator, int stack = 1, bool interactionRequired = true)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (itemId <= 0 || itemId >= ItemID.Count)
		{
			return;
		}
		if (Main.netMode == 2)
		{
			for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if (player.active && (npc.playerInteraction[i] || !interactionRequired) && rng.Next(chanceDenominator) < chanceNumerator)
				{
					int itemIndex = Item.NewItem(npc.GetItemSource_Loot(), player.Center, itemId, stack, -1);
					ModifyItemDropFromNPC(npc, itemIndex);
				}
			}
		}
		else if (rng.Next(chanceDenominator) < chanceNumerator)
		{
			DropItemFromNPC(npc, itemId, stack);
		}
		npc.value = 0f;
	}

	public static void ModifyItemDropFromNPC(NPC npc, int itemIndex)
	{
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		WorldItem worldItem = Main.item[itemIndex];
		switch (worldItem.type)
		{
		case 23:
			if (npc.type == 1 && npc.netID != -1 && npc.netID != -2 && npc.netID != -5 && npc.netID != -6)
			{
				worldItem.color = npc.color;
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
			}
			if (Main.remixWorld && npc.type == 59)
			{
				worldItem.color = new Color(255, 127, 0);
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
			}
			break;
		case 319:
			switch (npc.netID)
			{
			case 542:
				worldItem.color = new Color(189, 148, 96, 255);
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
				break;
			case 543:
				worldItem.color = new Color(112, 85, 89, 255);
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
				break;
			case 544:
				worldItem.color = new Color(145, 27, 40, 255);
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
				break;
			case 545:
				worldItem.color = new Color(158, 113, 164, 255);
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
				break;
			}
			break;
		}
	}
}
