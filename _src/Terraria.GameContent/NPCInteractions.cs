using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent;

public class NPCInteractions
{
	public static class Actions
	{
		public class OpenSign : NPCInteraction
		{
			public override bool Condition()
			{
				return LocalPlayer.sign > -1;
			}

			public override string GetText()
			{
				if (Main.editSign)
				{
					return Lang.inter[47].Value;
				}
				return Lang.inter[48].Value;
			}

			public override void Interact()
			{
				if (Main.editSign)
				{
					Main.SubmitSignText();
				}
				else
				{
					IngameFancyUI.OpenVirtualKeyboard(1);
				}
			}
		}

		public class OpenShop : NPCInteraction
		{
			private int _shopIndex;

			private int _npcType;

			private string _customTextKey;

			public OpenShop(int npcType, int shopIndex, string customTextKey = null)
			{
				_npcType = npcType;
				_shopIndex = shopIndex;
				_customTextKey = customTextKey;
			}

			public override bool Condition()
			{
				return TalkNPCType == _npcType;
			}

			public override string GetText()
			{
				if (_customTextKey != null)
				{
					return Language.GetTextValue(_customTextKey);
				}
				return Lang.inter[28].Value;
			}

			public override void Interact()
			{
				Main.instance.OpenShop(_shopIndex);
			}
		}

		public class StardewValleyBit : NPCInteraction
		{
			public override bool ShowExclamation => true;

			public override bool Condition()
			{
				if (TalkNPCType == 20)
				{
					return Main.CanDryadPlayStardewAnimation(LocalPlayer, TalkNPC);
				}
				return false;
			}

			public override string GetText()
			{
				return Language.GetTextValue("StardewTalk.GiveColaButtonText");
			}

			public override void Interact()
			{
				Main.DoNPCPortraitHop();
				SoundEngine.PlaySound(12);
				Main.DryadText_Do_StardewValleyBit();
			}
		}

		public class DryadPurification : NPCInteraction
		{
			public override bool Condition()
			{
				if (TalkNPCType == 20)
				{
					return !Main.CanDryadPlayStardewAnimation(LocalPlayer, TalkNPC);
				}
				return false;
			}

			public override string GetText()
			{
				return Lang.inter[49].Value;
			}

			public override void Interact()
			{
				Main.DoNPCPortraitHop();
				SoundEngine.PlaySound(12);
				Main.npcChatText = Lang.GetDryadWorldStatusDialog(out var worldIsEntirelyPure);
				if (worldIsEntirelyPure)
				{
					AchievementsHelper.HandleSpecialEvent(LocalPlayer, 27);
				}
			}
		}

		public class AnglerQuest : NPCInteraction
		{
			public override bool ShowExclamation => !Main.anglerQuestFinished;

			public override bool Condition()
			{
				return TalkNPCType == 369;
			}

			public override string GetText()
			{
				return Lang.inter[64].Value;
			}

			public override void Interact()
			{
				Main.NPCChatText_DoAnglerQuest();
			}
		}

		public class PetAnimal : NPCInteraction
		{
			public override bool Condition()
			{
				return NPCID.Sets.IsTownPet[TalkNPCType];
			}

			public override string GetText()
			{
				return Language.GetTextValue("UI.PetTheAnimal");
			}

			public override void Interact()
			{
				LocalPlayer.PetAnimal(Main.npc[LocalPlayer.talkNPC].GetPettingInfo(LocalPlayer));
			}
		}

		public class OldManCurse : NPCInteraction
		{
			public override bool Condition()
			{
				if (TalkNPCType == 37)
				{
					return !Main.IsItDay();
				}
				return false;
			}

			public override string GetText()
			{
				return Lang.inter[50].Value;
			}

			public override void Interact()
			{
				if (Main.netMode == 0)
				{
					NPC.SpawnSkeletron(Main.myPlayer);
				}
				else
				{
					NetMessage.SendData(51, -1, -1, null, Main.myPlayer, 1f);
				}
				Main.npcChatText = "";
			}
		}

		public class GuideTip : NPCInteraction
		{
			public override bool Condition()
			{
				return TalkNPCType == 22;
			}

			public override string GetText()
			{
				return Lang.inter[51].Value;
			}

			public override void Interact()
			{
				SoundEngine.PlaySound(12);
				Main.HelpText();
				Main.DoNPCPortraitHop();
			}
		}

		public class TaxCollectorCollectTaxes : NPCInteraction
		{
			public override bool Condition()
			{
				return TalkNPCType == 441;
			}

			public override string GetText()
			{
				return Lang.inter[89].Value;
			}

			public override void Interact()
			{
				Main.NPCChatText_DoTaxCollector();
			}

			public override bool TryAddCoins(ref Color chatColor, out int coinValue)
			{
				coinValue = 0;
				Main.GetCoinValueText_TaxCollector(ref chatColor, ref coinValue);
				return coinValue > 0;
			}
		}

		public class NurseHeal : NPCInteraction
		{
			public override bool Condition()
			{
				return TalkNPCType == 18;
			}

			public override string GetText()
			{
				return Lang.inter[54].Value;
			}

			public override void Interact()
			{
				Main.NPCChatText_DoNurseHeal(Main.GetNurseHealCost());
			}

			public override bool TryAddCoins(ref Color chatColor, out int coinValue)
			{
				coinValue = Main.GetNurseHealCost();
				Main.GetCoinValueText_Nurse(ref chatColor, ref coinValue);
				return coinValue > 0;
			}
		}

		public class CloseChat : NPCInteraction
		{
			public override bool Condition()
			{
				return true;
			}

			public override string GetText()
			{
				return Lang.inter[52].Value;
			}

			public override void Interact()
			{
				Main.CloseNPCChatOrSign();
			}
		}

		public class ReportHappiness : NPCInteraction
		{
			public override bool Condition()
			{
				if (NPC.CanShowHomelessText(Main.LocalPlayer.talkNPC))
				{
					return false;
				}
				return LocalPlayer.currentShoppingSettings.HappinessReport != "";
			}

			public override string GetText()
			{
				return Language.GetTextValue("UI.NPCCheckHappiness");
			}

			public override void Interact()
			{
				Main.npcChatCornerItem = 0;
				SoundEngine.PlaySound(12);
				Main.npcChatText = LocalPlayer.currentShoppingSettings.HappinessReport;
				Main.DoNPCPortraitHop();
			}
		}

		public class RequestHome : NPCInteraction
		{
			public override bool ShowExclamation => true;

			public override bool Condition()
			{
				return NPC.CanShowHomelessText(Main.LocalPlayer.talkNPC);
			}

			public override string GetText()
			{
				return Language.GetTextValue("UI.NPCHousing");
			}

			public override void Interact()
			{
				Main.npcChatCornerItem = -1;
				SoundEngine.PlaySound(12);
				Main.DoNPCPortraitHop();
				NPC talkNPC = TalkNPC;
				string text = "TownNPCMood_" + NPCID.Search.GetName(talkNPC.netID);
				if (talkNPC.type == 633 && talkNPC.altTexture == 2)
				{
					text += "Transformed";
				}
				if (talkNPC.type == 638)
				{
					text = "DogChatter";
				}
				else if (talkNPC.type == 637)
				{
					text = "CatChatter";
				}
				else if (talkNPC.type == 656)
				{
					text = "BunnyChatter";
				}
				else if (NPCID.Sets.IsTownSlime[talkNPC.type])
				{
					string slimeType = Lang.GetSlimeType(talkNPC);
					text = "Slime" + slimeType + "Chatter";
				}
				Main.npcChatText = Language.GetTextValue(text + ".NoHome");
				Main.npcChatText += "\n\n";
				if (talkNPC.type == 160)
				{
					Main.npcChatText += Language.GetTextValueWith("HousingText.HousingRequirements_Truffle", new
					{
						NPCName = talkNPC.FullName
					});
				}
				else
				{
					Main.npcChatText += Language.GetTextValue("HousingText.HousingRequirements");
				}
			}
		}

		public class PartyGirlMusicSwap : NPCInteraction
		{
			public override bool Condition()
			{
				return TalkNPCType == 208;
			}

			public override string GetText()
			{
				return Language.GetTextValue("GameUI.Music");
			}

			public override void Interact()
			{
				Main.NPCChatText_PartyGirlSwapMusic();
			}
		}

		public class GuideReverseCrafting : NPCInteraction
		{
			public override bool Condition()
			{
				return TalkNPCType == 22;
			}

			public override string GetText()
			{
				return Lang.inter[25].Value;
			}

			public override void Interact()
			{
				Main.NPCChatText_GuideReverseCrafting();
			}
		}

		public class TinkererReforge : NPCInteraction
		{
			public override bool Condition()
			{
				return TalkNPCType == 107;
			}

			public override string GetText()
			{
				return Lang.inter[19].Value;
			}

			public override void Interact()
			{
				Main.NPCChatText_TinkererReforge();
			}
		}

		public class StylistHairWindow : NPCInteraction
		{
			public override bool Condition()
			{
				return TalkNPCType == 353;
			}

			public override string GetText()
			{
				return Language.GetTextValue("GameUI.HairStyle");
			}

			public override void Interact()
			{
				Main.OpenHairWindow();
			}
		}

		public class DyeTraderRarePlant : NPCInteraction
		{
			public override bool Condition()
			{
				if (TalkNPCType == 207)
				{
					return Main.hardMode;
				}
				return false;
			}

			public override string GetText()
			{
				return Lang.inter[107].Value;
			}

			public override void Interact()
			{
				Main.NPCChatText_DyeTraderRarePlant();
			}
		}

		public class TavernkeepAdvice : NPCInteraction
		{
			public override bool Condition()
			{
				return TalkNPCType == 550;
			}

			public override string GetText()
			{
				return Language.GetTextValue("UI.BartenderHelp");
			}

			public override void Interact()
			{
				Main.NPCChatText_TavernkeepAdvice();
			}
		}
	}

	public static List<NPCInteraction> All = new List<NPCInteraction>();

	public static void Initialize()
	{
		Shop(17, 1);
		Shop(19, 2);
		Shop(20, 3);
		Shop(38, 4);
		Shop(54, 5);
		Shop(107, 6);
		Shop(108, 7);
		Shop(124, 8);
		Shop(142, 9);
		Shop(160, 10);
		Shop(178, 11);
		Shop(207, 12);
		Shop(208, 13);
		Shop(209, 14);
		Shop(227, 15);
		Shop(228, 16);
		Shop(229, 17);
		Shop(353, 18);
		Shop(368, 19);
		Shop(453, 20);
		Shop(550, 21);
		Shop(588, 22);
		Shop(633, 23);
		Shop(663, 24);
		Shop(227, 25, "GameUI.PainterDecor");
		Register(new Actions.TaxCollectorCollectTaxes());
		Register(new Actions.NurseHeal());
		Register(new Actions.CloseChat());
		Register(new Actions.OpenSign());
		Register(new Actions.StardewValleyBit());
		Register(new Actions.DryadPurification());
		Register(new Actions.AnglerQuest());
		Register(new Actions.PetAnimal());
		Register(new Actions.OldManCurse());
		Register(new Actions.GuideTip());
		Register(new Actions.PartyGirlMusicSwap());
		Register(new Actions.GuideReverseCrafting());
		Register(new Actions.TinkererReforge());
		Register(new Actions.StylistHairWindow());
		Register(new Actions.DyeTraderRarePlant());
		Register(new Actions.TavernkeepAdvice());
		Register(new Actions.ReportHappiness());
		Register(new Actions.RequestHome());
	}

	private static void Shop(int npcType, int shopIndex, string customTextKey = null)
	{
		Register(new Actions.OpenShop(npcType, shopIndex, customTextKey));
	}

	private static void Register(NPCInteraction interaction)
	{
		All.Add(interaction);
	}
}
