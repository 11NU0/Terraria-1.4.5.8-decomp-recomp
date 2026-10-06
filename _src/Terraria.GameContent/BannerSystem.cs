using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Net;

namespace Terraria.GameContent;

public static class BannerSystem
{
	public class NetBannersModule : NetModule
	{
		private enum MessageType
		{
			FullState,
			KillCountUpdate,
			ClaimCountUpdate,
			ClaimRequest,
			ClaimResponse
		}

		public static NetPacket WriteFullState()
		{
			NetPacket result = NetModule.CreatePacket<NetBannersModule>();
			result.Writer.Write((byte)0);
			Save(result.Writer);
			return result;
		}

		public static NetPacket WriteKillCountUpdate(int bannerId)
		{
			NetPacket result = NetModule.CreatePacket<NetBannersModule>();
			result.Writer.Write((byte)1);
			result.Writer.Write((short)bannerId);
			result.Writer.Write(killCount[bannerId]);
			return result;
		}

		public static NetPacket WriteClaimCountUpdate(int bannerId)
		{
			NetPacket result = NetModule.CreatePacket<NetBannersModule>();
			result.Writer.Write((byte)2);
			result.Writer.Write((short)bannerId);
			result.Writer.Write(claimableBanners[bannerId]);
			return result;
		}

		public static NetPacket WriteClaimRequest(int bannerId, int amount)
		{
			NetPacket result = NetModule.CreatePacket<NetBannersModule>();
			result.Writer.Write((byte)3);
			result.Writer.Write((short)bannerId);
			result.Writer.Write((ushort)amount);
			return result;
		}

		public static NetPacket WriteClaimResponse(int bannerId, int amount, bool granted)
		{
			NetPacket result = NetModule.CreatePacket<NetBannersModule>();
			result.Writer.Write((byte)4);
			result.Writer.Write((short)bannerId);
			result.Writer.Write((ushort)amount);
			result.Writer.Write(granted);
			return result;
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			switch ((MessageType)reader.ReadByte())
			{
			case MessageType.FullState:
				if (Main.netMode == 2)
				{
					return false;
				}
				Load(reader, 326);
				Array.Clear(newBanner, 0, newBanner.Length);
				break;
			case MessageType.KillCountUpdate:
				if (Main.netMode == 2)
				{
					return false;
				}
				killCount[reader.ReadInt16()] = reader.ReadInt32();
				break;
			case MessageType.ClaimCountUpdate:
			{
				if (Main.netMode == 2)
				{
					return false;
				}
				short num = reader.ReadInt16();
				ushort num2 = claimableBanners[num];
				claimableBanners[num] = reader.ReadUInt16();
				if (newBanner[num] && claimableBanners[num] == 0)
				{
					newBanner[num] = false;
					RefreshAnyNewClaimableBanners();
				}
				if (num2 == 0 && claimableBanners[num] == 1)
				{
					newBanner[num] = true;
					AnyNewClaimableBanners = true;
				}
				break;
			}
			case MessageType.ClaimRequest:
				HandleBannerClaimRequest(reader.ReadInt16(), reader.ReadUInt16(), userId);
				break;
			case MessageType.ClaimResponse:
				if (Main.netMode == 2)
				{
					return false;
				}
				HandleBannerClaimResponse(reader.ReadInt16(), reader.ReadUInt16(), reader.ReadBoolean());
				break;
			}
			return true;
		}
	}

	public static readonly int MaxBannerTypes = 293;

	private static int[] killCount = new int[MaxBannerTypes];

	private static ushort[] claimableBanners = new ushort[MaxBannerTypes];

	private static bool[] newBanner = new bool[MaxBannerTypes];

	public static bool AnyNewClaimableBanners;

	public static int GetKillCount(int banner)
	{
		return killCount[banner];
	}

	public static ushort[] GetClaimableBannerCounts()
	{
		return claimableBanners;
	}

	public static bool IsNewBanner(int banner)
	{
		return newBanner[banner];
	}

	public static void SetNewBanner(int banner)
	{
		newBanner[banner] = true;
		AnyNewClaimableBanners = true;
	}

	public static void ClearNewBanner(int banner)
	{
		newBanner[banner] = false;
		if (AnyNewClaimableBanners)
		{
			RefreshAnyNewClaimableBanners();
		}
	}

	public static void Clear()
	{
		Array.Clear(killCount, 0, killCount.Length);
		Array.Clear(claimableBanners, 0, claimableBanners.Length);
		Array.Clear(newBanner, 0, newBanner.Length);
		AnyNewClaimableBanners = false;
	}

	public static void Save(BinaryWriter writer)
	{
		writer.Write((short)killCount.Length);
		int[] array = killCount;
		foreach (int value in array)
		{
			writer.Write(value);
		}
		writer.Write((short)claimableBanners.Length);
		ushort[] array2 = claimableBanners;
		foreach (ushort value2 in array2)
		{
			writer.Write(value2);
		}
	}

	public static void Load(BinaryReader reader, int version)
	{
		int num = reader.ReadInt16();
		for (int i = 0; i < num; i++)
		{
			int num2 = reader.ReadInt32();
			if (i < killCount.Length)
			{
				killCount[i] = num2;
			}
		}
		if (version < 289)
		{
			return;
		}
		num = reader.ReadInt16();
		for (int j = 0; j < num; j++)
		{
			ushort num3 = reader.ReadUInt16();
			if (j < claimableBanners.Length)
			{
				claimableBanners[j] = num3;
			}
		}
	}

	public static void ValidateWorld(BinaryReader reader, int version)
	{
		int num = reader.ReadInt16();
		for (int i = 0; i < num; i++)
		{
			reader.ReadInt32();
		}
		if (version >= 289)
		{
			num = reader.ReadInt16();
			for (int j = 0; j < num; j++)
			{
				reader.ReadUInt16();
			}
		}
	}

	public static void AddNPCKillBy(int npcType, int plr)
	{
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 1)
		{
			return;
		}
		int num = NPCtoBanner(npcType);
		if (num <= 0)
		{
			return;
		}
		AddKill(num);
		int num2 = ItemID.Sets.KillsToBanner[BannerToItem(num)];
		if (killCount[num] % num2 == 0)
		{
			if (claimableBanners[num] < Item.CommonMaxStack)
			{
				AddClaimableBanner(num);
			}
			int netID = BannerToNPC(num);
			NetworkText text = NetworkText.FromKey("Game.EnemiesDefeatedAnnouncement", killCount[num], NetworkText.FromKey(Lang.GetNPCName(netID).Key));
			if (plr >= 0 && plr < 255)
			{
				text = NetworkText.FromKey("Game.EnemiesDefeatedByAnnouncement", Main.player[plr].name, killCount[num], NetworkText.FromKey(Lang.GetNPCName(netID).Key));
			}
			ChatHelper.BroadcastChatMessage(text, new Color(250, 250, 0));
		}
	}

	private static void AddClaimableBanner(int banner)
	{
		newBanner[banner] |= claimableBanners[banner] == 0;
		AnyNewClaimableBanners |= newBanner[banner];
		claimableBanners[banner]++;
		if (Main.netMode == 2)
		{
			NetManager.Instance.Broadcast(NetBannersModule.WriteClaimCountUpdate(banner));
		}
	}

	private static void AddKill(int banner)
	{
		killCount[banner]++;
		if (Main.netMode == 2)
		{
			NetManager.Instance.Broadcast(NetBannersModule.WriteKillCountUpdate(banner));
		}
	}

	public static void RequestBannerClaim(int banner, int amount)
	{
		FakeCursorItem.Add(BannerToItem(banner), amount);
		NetManager.Instance.SendToServerOrLoopback(NetBannersModule.WriteClaimRequest(banner, amount));
	}

	private static void RefreshAnyNewClaimableBanners()
	{
		AnyNewClaimableBanners = false;
		for (int i = 0; i < newBanner.Length; i++)
		{
			AnyNewClaimableBanners |= newBanner[i];
		}
	}

	private static void HandleBannerClaimRequest(int banner, ushort amount, int plr)
	{
		if (amount < 1)
		{
			amount = 1;
		}
		ushort num = Math.Min(amount, claimableBanners[banner]);
		if (num > 0)
		{
			claimableBanners[banner] -= num;
			amount -= num;
			if (Main.netMode == 2)
			{
				NetManager.Instance.Broadcast(NetBannersModule.WriteClaimCountUpdate(banner));
			}
			if (claimableBanners[banner] == 0)
			{
				newBanner[banner] = false;
				if (AnyNewClaimableBanners)
				{
					RefreshAnyNewClaimableBanners();
				}
			}
			NetManager.Instance.SendToClientOrLoopback(NetBannersModule.WriteClaimResponse(banner, num, granted: true), plr);
		}
		if (amount > 0)
		{
			NetManager.Instance.SendToClient(NetBannersModule.WriteClaimResponse(banner, amount, granted: false), plr);
		}
	}

	private static void HandleBannerClaimResponse(int banner, ushort amount, bool granted)
	{
		int num = BannerToItem(banner);
		FakeCursorItem.Remove(num, amount);
		if (granted)
		{
			int amountLeftToAdd = amount;
			Player localPlayer = Main.LocalPlayer;
			if ((!Main.playerInventory || localPlayer.dead || !Main.mouseItem.TryAddStack(num, ref amountLeftToAdd)) && amountLeftToAdd > 0)
			{
				localPlayer.QuickSpawnItem(localPlayer.GetItemSource_InventoryOverflow(), num, amountLeftToAdd);
			}
		}
	}

	public static int BannerToItem(int banner)
	{
		if (banner == 292)
		{
			return 5673;
		}
		if (banner == 291)
		{
			return 5672;
		}
		if (banner == 290)
		{
			return 5651;
		}
		if (banner == 289)
		{
			return 5352;
		}
		if (banner >= 276)
		{
			return 4965 + banner - 276;
		}
		if (banner >= 274)
		{
			return 4687 + banner - 274;
		}
		if (banner == 273)
		{
			return 4602;
		}
		if (banner >= 267)
		{
			return 4541 + banner - 267;
		}
		if (banner >= 257)
		{
			return 3837 + banner - 257;
		}
		if (banner >= 252)
		{
			return 3789 + banner - 252;
		}
		if (banner == 251)
		{
			return 3780;
		}
		if (banner >= 249)
		{
			return 3593 + banner - 249;
		}
		if (banner >= 186)
		{
			return 3390 + banner - 186;
		}
		if (banner >= 88)
		{
			return 2897 + banner - 88;
		}
		return 1615 + banner - 1;
	}

	public static int NPCtoBanner(int i)
	{
		switch (i)
		{
		case 102:
			return 1;
		case 250:
			return 2;
		case 257:
			return 3;
		case 69:
			return 4;
		case 157:
			return 5;
		case 77:
			return 6;
		case 49:
			return 7;
		case 74:
		case 297:
		case 298:
			return 8;
		case 163:
		case 238:
			return 9;
		case 241:
			return 10;
		case 242:
			return 11;
		case 239:
		case 240:
			return 12;
		case 39:
		case 40:
		case 41:
			return 13;
		case 46:
		case 303:
		case 337:
		case 540:
			return 14;
		case 120:
			return 15;
		case 85:
		case 629:
			return 16;
		case 109:
		case 378:
			return 17;
		case 47:
			return 18;
		case 57:
			return 19;
		case 67:
			return 20;
		case 173:
			return 21;
		case 179:
			return 22;
		case 83:
			return 23;
		case 62:
		case 66:
			return 24;
		case 2:
		case 190:
		case 191:
		case 192:
		case 193:
		case 194:
		case 317:
		case 318:
			return 25;
		case 177:
			return 26;
		case 6:
			return 27;
		case 84:
			return 28;
		case 161:
		case 431:
			return 29;
		case 181:
			return 30;
		case 182:
			return 31;
		case 224:
			return 32;
		case 226:
			return 33;
		case 162:
			return 34;
		case 259:
		case 260:
		case 261:
			return 35;
		case 256:
			return 36;
		case 122:
			return 37;
		case 27:
			return 38;
		case 29:
		case 30:
			return 39;
		case 26:
			return 40;
		case 73:
			return 41;
		case 28:
			return 42;
		case 55:
		case 230:
			return 43;
		case 48:
			return 44;
		case 60:
			return 45;
		case 174:
			return 46;
		case 42:
		case 231:
		case 232:
		case 233:
		case 234:
		case 235:
			return 47;
		case 169:
			return 48;
		case 206:
			return 49;
		case 24:
		case 25:
			return 50;
		case 63:
			return 51;
		case 236:
		case 237:
			return 52;
		case 198:
		case 199:
			return 53;
		case 43:
			return 54;
		case 23:
			return 55;
		case 205:
			return 56;
		case 78:
			return 57;
		case 258:
			return 58;
		case 252:
			return 59;
		case 170:
		case 171:
		case 180:
			return 60;
		case 58:
			return 61;
		case 212:
			return 62;
		case 75:
			return 63;
		case 223:
			return 64;
		case 253:
			return 65;
		case 65:
			return 66;
		case 21:
		case 201:
		case 202:
		case 203:
		case 322:
		case 323:
		case 324:
		case 449:
		case 450:
		case 451:
		case 452:
			return 67;
		case 32:
		case 33:
			return 68;
		case 1:
		case 302:
		case 333:
		case 334:
		case 335:
		case 336:
			return 69;
		case 185:
			return 70;
		case 164:
		case 165:
			return 71;
		case 254:
		case 255:
			return 72;
		case 166:
			return 73;
		case 153:
			return 74;
		case 141:
			return 75;
		case 225:
			return 76;
		case 86:
			return 77;
		case 158:
		case 159:
			return 78;
		case 61:
			return 79;
		case 195:
		case 196:
			return 80;
		case 104:
			return 81;
		case 155:
			return 82;
		case 98:
		case 99:
		case 100:
			return 83;
		case 10:
		case 11:
		case 12:
		case 95:
		case 96:
		case 97:
			return 84;
		case 82:
			return 85;
		case 87:
		case 88:
		case 89:
		case 90:
		case 91:
		case 92:
			return 86;
		case 3:
		case 132:
		case 186:
		case 187:
		case 188:
		case 189:
		case 200:
		case 319:
		case 320:
		case 321:
		case 331:
		case 332:
		case 430:
		case 432:
		case 433:
		case 434:
		case 435:
		case 436:
		case 590:
		case 591:
		case 632:
		case 691:
			return 87;
		case 175:
			return 88;
		case 197:
			return 89;
		case 273:
		case 274:
		case 275:
		case 276:
			return 91;
		case 379:
			return 92;
		case 438:
			return 93;
		case 287:
			return 95;
		case 101:
			return 96;
		case 217:
			return 97;
		case 168:
			return 98;
		case -1:
		case 81:
			return 99;
		case 94:
		case 112:
			return 100;
		case 183:
			return 101;
		case 34:
			return 102;
		case 218:
			return 103;
		case 7:
		case 8:
		case 9:
			return 104;
		case 285:
		case 286:
			return 105;
		case 52:
			return 106;
		case 71:
			return 107;
		case 288:
			return 108;
		case 350:
			return 109;
		case 347:
			return 110;
		case 251:
			return 111;
		case 352:
			return 112;
		case 316:
			return 113;
		case 93:
			return 114;
		case 289:
			return 115;
		case 152:
			return 116;
		case 342:
			return 117;
		case 111:
			return 118;
		case 315:
			return 120;
		case 277:
		case 278:
		case 279:
		case 280:
			return 121;
		case 329:
			return 122;
		case 304:
			return 123;
		case 150:
			return 124;
		case 243:
			return 125;
		case 147:
			return 126;
		case 268:
			return 127;
		case 137:
			return 128;
		case 138:
			return 129;
		case 51:
			return 130;
		case 351:
			return 132;
		case 219:
			return 133;
		case 151:
			return 134;
		case 59:
			return 135;
		case 381:
			return 136;
		case 388:
			return 137;
		case 386:
			return 138;
		case 389:
			return 139;
		case 385:
			return 140;
		case 383:
		case 384:
			return 141;
		case 382:
			return 142;
		case 390:
			return 143;
		case 387:
			return 144;
		case 144:
			return 145;
		case -5:
		case 16:
			return 146;
		case 283:
		case 284:
			return 147;
		case 348:
		case 349:
			return 148;
		case 290:
			return 149;
		case 148:
		case 149:
			return 150;
		case -4:
			return 151;
		case 330:
			return 152;
		case 140:
			return 153;
		case 341:
			return 154;
		case 281:
		case 282:
			return 156;
		case 244:
			return 157;
		case 301:
			return 158;
		case 172:
			return 160;
		case 269:
		case 270:
		case 271:
		case 272:
			return 161;
		case 305:
		case 306:
		case 307:
		case 308:
		case 309:
		case 310:
		case 311:
		case 312:
		case 313:
		case 314:
			return 162;
		case 391:
			return 163;
		case 110:
			return 164;
		case 293:
			return 165;
		case 291:
			return 166;
		case -2:
		case 121:
			return 167;
		case 56:
			return 168;
		case 145:
			return 169;
		case 143:
			return 170;
		case 184:
			return 171;
		case 204:
			return 172;
		case 326:
			return 173;
		case 221:
			return 174;
		case 292:
			return 175;
		case 53:
			return 176;
		case 45:
		case 665:
			return 177;
		case 44:
			return 178;
		case 167:
			return 179;
		case 380:
			return 180;
		case 343:
			return 184;
		case 338:
		case 339:
		case 340:
			return 185;
		case -6:
			return 90;
		case -3:
			return 119;
		case -10:
			return 131;
		case -7:
			return 155;
		case -8:
			return 159;
		case -9:
			return 183;
		case 471:
		case 472:
			return 186;
		case 498:
		case 499:
		case 500:
		case 501:
		case 502:
		case 503:
		case 504:
		case 505:
		case 506:
			return 187;
		case 496:
		case 497:
			return 188;
		case 494:
		case 495:
			return 189;
		case 462:
			return 190;
		case 461:
			return 191;
		case 468:
			return 192;
		case 477:
		case 478:
		case 479:
			return 193;
		case 469:
			return 195;
		case 460:
			return 196;
		case 466:
			return 197;
		case 467:
			return 198;
		case 463:
			return 199;
		case 480:
			return 201;
		case 481:
			return 202;
		case 483:
			return 203;
		case 482:
			return 204;
		case 489:
			return 205;
		case 490:
			return 206;
		case 513:
		case 514:
		case 515:
			return 207;
		case 510:
		case 511:
		case 512:
			return 208;
		case 509:
		case 581:
			return 209;
		case 508:
		case 580:
			return 210;
		case 524:
		case 525:
		case 526:
		case 527:
			return 211;
		case 528:
		case 529:
			return 212;
		case 533:
			return 213;
		case 532:
			return 214;
		case 530:
		case 531:
			return 215;
		case 411:
			return 216;
		case 402:
		case 403:
		case 404:
			return 217;
		case 407:
		case 408:
			return 218;
		case 409:
		case 410:
			return 219;
		case 406:
			return 220;
		case 405:
			return 221;
		case 418:
			return 222;
		case 417:
			return 223;
		case 412:
		case 413:
		case 414:
			return 224;
		case 416:
		case 518:
			return 225;
		case 415:
		case 516:
			return 226;
		case 419:
			return 227;
		case 424:
			return 228;
		case 421:
			return 229;
		case 420:
			return 230;
		case 423:
			return 231;
		case 428:
			return 232;
		case 426:
			return 233;
		case 427:
			return 234;
		case 429:
			return 235;
		case 425:
			return 236;
		case 216:
			return 237;
		case 214:
			return 238;
		case 213:
			return 239;
		case 215:
			return 240;
		case 520:
			return 241;
		case 156:
			return 242;
		case 64:
			return 243;
		case 103:
			return 244;
		case 79:
			return 245;
		case 80:
			return 246;
		case 31:
		case 294:
		case 295:
		case 296:
			return 247;
		case 154:
			return 248;
		case 537:
			return 249;
		case 220:
			return 250;
		case 541:
			return 251;
		case 542:
			return 252;
		case 543:
			return 253;
		case 544:
			return 254;
		case 545:
			return 255;
		case 546:
			return 256;
		case 555:
		case 556:
		case 557:
			return 257;
		case 552:
		case 553:
		case 554:
			return 258;
		case 566:
		case 567:
			return 259;
		case 570:
		case 571:
			return 260;
		case 574:
		case 575:
			return 261;
		case 572:
		case 573:
			return 262;
		case 568:
		case 569:
			return 263;
		case 558:
		case 559:
		case 560:
			return 264;
		case 561:
		case 562:
		case 563:
			return 265;
		case 578:
			return 266;
		case 536:
			return 267;
		case 586:
			return 268;
		case 587:
			return 269;
		case 619:
			return 270;
		case 621:
		case 622:
		case 623:
			return 271;
		case 620:
			return 272;
		case 618:
			return 273;
		case 628:
			return 274;
		case 624:
			return 275;
		case 631:
			return 276;
		case 630:
			return 277;
		case 635:
			return 278;
		case 634:
			return 279;
		case 582:
			return 280;
		case 464:
			return 281;
		case 465:
			return 282;
		case 470:
			return 283;
		case 473:
			return 284;
		case 474:
			return 285;
		case 475:
			return 286;
		case 176:
			return 287;
		case 133:
			return 288;
		case 676:
			return 289;
		case 692:
			return 290;
		case 693:
			return 291;
		case 694:
			return 292;
		default:
			return 0;
		}
	}

	public static int BannerToNPC(int i)
	{
		return i switch
		{
			1 => 102, 
			2 => 250, 
			3 => 257, 
			4 => 69, 
			5 => 157, 
			6 => 77, 
			7 => 49, 
			8 => 74, 
			9 => 163, 
			10 => 241, 
			11 => 242, 
			12 => 239, 
			13 => 39, 
			14 => 46, 
			15 => 120, 
			16 => 85, 
			17 => 109, 
			18 => 47, 
			19 => 57, 
			20 => 67, 
			21 => 173, 
			22 => 179, 
			23 => 83, 
			24 => 62, 
			25 => 2, 
			26 => 177, 
			27 => 6, 
			28 => 84, 
			29 => 161, 
			30 => 181, 
			31 => 182, 
			32 => 224, 
			33 => 226, 
			34 => 162, 
			35 => 259, 
			36 => 256, 
			37 => 122, 
			38 => 27, 
			39 => 29, 
			40 => 26, 
			41 => 73, 
			42 => 28, 
			43 => 55, 
			44 => 48, 
			45 => 60, 
			46 => 174, 
			47 => 42, 
			48 => 169, 
			49 => 206, 
			50 => 24, 
			51 => 63, 
			52 => 236, 
			53 => 199, 
			54 => 43, 
			55 => 23, 
			56 => 205, 
			57 => 78, 
			58 => 258, 
			59 => 252, 
			60 => 170, 
			61 => 58, 
			62 => 212, 
			63 => 75, 
			64 => 223, 
			65 => 253, 
			66 => 65, 
			67 => 21, 
			68 => 32, 
			69 => 1, 
			70 => 185, 
			71 => 164, 
			72 => 254, 
			73 => 166, 
			74 => 153, 
			75 => 141, 
			76 => 225, 
			77 => 86, 
			78 => 158, 
			79 => 61, 
			80 => 196, 
			81 => 104, 
			82 => 155, 
			83 => 98, 
			84 => 10, 
			85 => 82, 
			86 => 87, 
			87 => 3, 
			88 => 175, 
			89 => 197, 
			91 => 273, 
			92 => 379, 
			93 => 438, 
			95 => 287, 
			96 => 101, 
			97 => 217, 
			98 => 168, 
			99 => 81, 
			100 => 94, 
			101 => 183, 
			102 => 34, 
			103 => 218, 
			104 => 7, 
			105 => 285, 
			106 => 52, 
			107 => 71, 
			108 => 288, 
			109 => 350, 
			110 => 347, 
			111 => 251, 
			112 => 352, 
			113 => 316, 
			114 => 93, 
			115 => 289, 
			116 => 152, 
			117 => 342, 
			118 => 111, 
			120 => 315, 
			121 => 277, 
			122 => 329, 
			123 => 304, 
			124 => 150, 
			125 => 243, 
			126 => 147, 
			127 => 268, 
			128 => 137, 
			129 => 138, 
			130 => 51, 
			132 => 351, 
			133 => 219, 
			134 => 151, 
			135 => 59, 
			136 => 381, 
			137 => 388, 
			138 => 386, 
			139 => 389, 
			140 => 385, 
			141 => 383, 
			142 => 382, 
			143 => 390, 
			144 => 387, 
			145 => 144, 
			146 => 16, 
			147 => 283, 
			148 => 348, 
			149 => 290, 
			150 => 148, 
			151 => -4, 
			152 => 330, 
			153 => 140, 
			154 => 341, 
			156 => 281, 
			157 => 244, 
			158 => 301, 
			160 => 172, 
			161 => 269, 
			162 => 305, 
			163 => 391, 
			164 => 110, 
			165 => 293, 
			166 => 291, 
			167 => 121, 
			168 => 56, 
			169 => 145, 
			170 => 143, 
			171 => 184, 
			172 => 204, 
			173 => 326, 
			174 => 221, 
			175 => 292, 
			176 => 53, 
			177 => 45, 
			178 => 44, 
			179 => 167, 
			180 => 380, 
			184 => 343, 
			185 => 338, 
			90 => -6, 
			119 => -3, 
			131 => -10, 
			155 => -7, 
			159 => -8, 
			183 => -9, 
			186 => 471, 
			187 => 498, 
			188 => 496, 
			189 => 494, 
			190 => 462, 
			191 => 461, 
			192 => 468, 
			193 => 477, 
			195 => 469, 
			196 => 460, 
			197 => 466, 
			198 => 467, 
			199 => 463, 
			201 => 480, 
			202 => 481, 
			203 => 483, 
			204 => 482, 
			205 => 489, 
			206 => 490, 
			207 => 513, 
			208 => 510, 
			209 => 581, 
			210 => 580, 
			211 => 524, 
			212 => 529, 
			213 => 533, 
			214 => 532, 
			215 => 530, 
			216 => 411, 
			217 => 402, 
			218 => 407, 
			219 => 409, 
			220 => 406, 
			221 => 405, 
			222 => 418, 
			223 => 417, 
			224 => 412, 
			225 => 416, 
			226 => 415, 
			227 => 419, 
			228 => 424, 
			229 => 421, 
			230 => 420, 
			231 => 423, 
			232 => 428, 
			233 => 426, 
			234 => 427, 
			235 => 429, 
			236 => 425, 
			237 => 216, 
			238 => 214, 
			239 => 213, 
			240 => 215, 
			241 => 520, 
			242 => 156, 
			243 => 64, 
			244 => 103, 
			245 => 79, 
			246 => 80, 
			247 => 31, 
			248 => 154, 
			249 => 537, 
			250 => 220, 
			251 => 541, 
			252 => 542, 
			253 => 543, 
			254 => 544, 
			255 => 545, 
			256 => 546, 
			257 => 555, 
			258 => 552, 
			259 => 566, 
			260 => 570, 
			261 => 574, 
			262 => 572, 
			263 => 568, 
			264 => 558, 
			265 => 561, 
			266 => 578, 
			267 => 536, 
			268 => 586, 
			269 => 587, 
			270 => 619, 
			271 => 621, 
			272 => 620, 
			273 => 618, 
			274 => 628, 
			275 => 624, 
			276 => 631, 
			277 => 630, 
			278 => 635, 
			279 => 634, 
			280 => 582, 
			281 => 464, 
			282 => 465, 
			283 => 470, 
			284 => 473, 
			285 => 474, 
			286 => 475, 
			287 => 176, 
			288 => 133, 
			289 => 676, 
			290 => 692, 
			291 => 693, 
			292 => 694, 
			_ => 0, 
		};
	}
}
