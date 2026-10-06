using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Animations;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.Skies.CreditsRoll;

public class CreditsRollComposer
{
	private struct SimplifiedNPCInfo(int npcType, Vector2 simplifiedPosition)
	{
		private Vector2 _simplifiedPosition = simplifiedPosition;

		private int _npcType = npcType;

		public void SpawnNPC(AddNPCMethod methodToUse, Vector2 baseAnchor, int startTime, int totalSceneTime)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			Vector2 properPosition = GetProperPosition(baseAnchor);
			int lookDirection = ((!(_simplifiedPosition.X > 0f)) ? 1 : (-1));
			int num = 240;
			int timeToJumpAt = (totalSceneTime - num) / 2 - 20 + (int)(_simplifiedPosition.X * -8f);
			methodToUse(_npcType, properPosition, lookDirection, startTime, totalSceneTime, timeToJumpAt);
		}

		private Vector2 GetProperPosition(Vector2 baseAnchor)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			return baseAnchor + _simplifiedPosition * new Vector2(26f, 24f);
		}
	}

	private delegate void AddNPCMethod(int npcType, Vector2 sceneAnchoePosition, int lookDirection, int fromTime, int duration, int timeToJumpAt);

	private Vector2 _originAtBottom = new Vector2(0.5f, 1f);

	private Vector2 _emoteBubbleOffsetWhenOnLeft = new Vector2(-14f, -38f);

	private Vector2 _emoteBubbleOffsetWhenOnRight = new Vector2(14f, -38f);

	private Vector2 _backgroundOffset = new Vector2(76f, 166f);

	private int _endTime;

	private List<IAnimationSegment> _segments;

	public void FillSegments_Test(List<IAnimationSegment> segmentsList, out int endTime)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		_segments = segmentsList;
		int num = 0;
		int num2 = 80;
		Vector2 sceneAnchorPosition = Vector2.UnitY * -1f * (float)num2;
		num += PlaySegment_PrincessAndEveryoneThanksPlayer(num, sceneAnchorPosition).totalTime;
		_endTime = num + 20;
		endTime = _endTime;
	}

	public void FillSegments(List<IAnimationSegment> segmentsList, out int endTime, bool inGame)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		_segments = segmentsList;
		int num = 0;
		int num2 = 80;
		Vector2 val = Vector2.UnitY * -1f * (float)num2;
		int num3 = 210;
		Vector2 val2 = val + Vector2.UnitX * 200f;
		Vector2 val3 = val2;
		if (!inGame)
		{
			val3 = (val2 = Vector2.UnitY * 80f);
		}
		int num4 = num3 * 3;
		int num5 = num3 * 3;
		int num6 = num4 - num5;
		if (!inGame)
		{
			num5 = 180;
			num6 = num4 - num5;
		}
		num += num5;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Creator", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_GuideRunningFromZombie(num, val2).totalTime;
		num += num3;
		val2.X *= -1f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_ExecutiveProducer", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_MerchantAndTravelingMerchantTryingToSellJunk(num, val2).totalTime;
		num += num3;
		val2.X *= -1f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Designer", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_DemolitionistAndArmsDealerArguingThenNurseComes(num, val2).totalTime;
		num += num3;
		val2.X *= -1f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Programming", val2).totalTime;
		num += num3;
		num += PlaySegment_TinkererAndMechanic(num, val2).totalTime;
		num += num3;
		val2.X *= 0f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Graphics", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_DryadSayingByeToTavernKeep(num, val2).totalTime;
		num += num3;
		val2 = val3;
		val2.X *= -1f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Music", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_WizardPartyGirlDyeTraderAndPainterPartyWithBunnies(num, val2).totalTime;
		num += num3;
		val2.X *= -1f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Sound", val2).totalTime;
		num += num3;
		num += PlaySegment_ClothierChasingTruffle(num, val2).totalTime;
		num += num3;
		val2.X *= -1f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Dialog", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_AnglerAndPirateTalkAboutFish(num, val2).totalTime;
		num += num3;
		val2.X *= 0f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_QualityAssurance", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_ZoologistAndPetsAnnoyGolfer(num, val2).totalTime;
		num += num3;
		val2 = val3;
		val2.X *= -1f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_BusinessDevelopment", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_SkeletonMerchantSearchesThroughBones(num, val2).totalTime;
		num += num3;
		val2.X *= -1f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Marketing", val2).totalTime;
		num += num3;
		num += PlaySegment_DryadTurningToTree(num, val2).totalTime;
		num += num3;
		val2.X *= -1f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_PublicRelations", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_SteampunkerRepairingCyborg(num, val2).totalTime;
		num += num3;
		val2.X *= 0f;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Webmaster", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_SantaAndTaxCollectorThrowingPresents(num, val2).totalTime;
		num += num3;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Playtesting", val2).totalTime;
		num += num3;
		num += PlaySegment_Grox_WitchDoctorGoingToHisPeople(num, val2).totalTime;
		num += num3;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Localization", val2).totalTime;
		num += num3;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_SpecialThanksto", val2).totalTime;
		num += num3 * 2;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_Dedication", val2).totalTime;
		num += num3;
		num += PlaySegment_PrincessAndEveryoneThanksPlayer(num, val2).totalTime;
		num += num3;
		num += PlaySegment_TextRoll(num, "CreditsRollCategory_EndingNotes", val2).totalTime;
		num += num6;
		_endTime = num + 10;
		endTime = _endTime;
	}

	private SegmentInforReport PlaySegment_PrincessAndEveryoneThanksPlayer(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition.Y += 40f;
		int num = -2;
		int num2 = 2;
		List<int> list = new List<int> { 228, 178, 550, 208, 160, 209 };
		List<int> list2 = new List<int> { 353, 633, 207, 588, 227, 368 };
		List<int> list3 = new List<int> { 22, 19, 18, 17, 38, 54, 108 };
		List<int> list4 = new List<int> { 663, 20, 441, 107, 124, 229, 369 };
		List<SimplifiedNPCInfo> list5 = new List<SimplifiedNPCInfo>();
		for (int i = 0; i < list.Count; i++)
		{
			int npcType = list[i];
			list5.Add(new SimplifiedNPCInfo(npcType, new Vector2((float)(num - i), -1f)));
		}
		for (int j = 0; j < list3.Count; j++)
		{
			int npcType2 = list3[j];
			list5.Add(new SimplifiedNPCInfo(npcType2, new Vector2((float)(num - j) + 0.5f, 0f)));
		}
		for (int k = 0; k < list2.Count; k++)
		{
			int npcType3 = list2[k];
			list5.Add(new SimplifiedNPCInfo(npcType3, new Vector2((float)(num2 + k), -1f)));
		}
		for (int l = 0; l < list4.Count; l++)
		{
			int npcType4 = list4[l];
			list5.Add(new SimplifiedNPCInfo(npcType4, new Vector2((float)(num2 + l) - 0.5f, 0f)));
		}
		int num3 = 240;
		int num4 = 400;
		int num5 = num4 + num3;
		Asset<Texture2D> val = TextureAssets.Extra[241];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2(0f, -92f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> item = new Segments.SpriteSegment(val, startTime, data, sceneAnchorPosition).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51)).Then(new Actions.Sprites.Wait(num5))
			.Then(new Actions.Sprites.Fade(0f, 85));
		_segments.Add(item);
		foreach (SimplifiedNPCInfo item3 in list5)
		{
			item3.SpawnNPC(AddWavingNPC, sceneAnchorPosition, startTime, num5);
		}
		float num6 = 3f;
		float num7 = -0.05f;
		int num8 = 60;
		float num9 = num6 * (float)num8 + num7 * ((float)(num8 * num8) * 0.5f);
		int num10 = startTime + num3;
		int num11 = 51;
		Segments.AnimationSegmentWithActions<Player> item2 = new Segments.PlayerSegment(num10 - num8 + num11, sceneAnchorPosition + new Vector2(0f, 0f - num9), _originAtBottom).UseShaderEffect(new Segments.PlayerSegment.ImmediateSpritebatchForPlayerDyesEffect()).Then(new Actions.Players.Fade(0f)).With(new Actions.Players.LookAt(1))
			.With(new Actions.Players.Fade(1f, num8))
			.Then(new Actions.Players.Wait(num4 / 2))
			.With(new Actions.Players.MoveWithAcceleration(new Vector2(0f, num6), new Vector2(0f, num7), num8))
			.Then(new Actions.Players.Wait(num4 / 2 - 60))
			.With(new Actions.Players.LookAt(-1))
			.Then(new Actions.Players.Wait(120))
			.With(new Actions.Players.LookAt(1))
			.Then(new Actions.Players.Fade(0f, 85));
		_segments.Add(item2);
		return new SegmentInforReport
		{
			totalTime = num5 + 85 + 60
		};
	}

	private void AddWavingNPC(int npcType, Vector2 sceneAnchoePosition, int lookDirection, int fromTime, int duration, int timeToJumpAt)
	{
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		float num2 = 4f;
		float num3 = 0.2f;
		float num4 = num2 * 2f / num3;
		int num5 = NPCID.Sets.AttackType[npcType] * 6 + npcType % 13 * 2 + 20;
		int num6 = 0;
		if (npcType % 7 != 0)
		{
			num6 = 0;
		}
		bool num7 = npcType == 663 || npcType == 108;
		bool flag = false;
		if (num7)
		{
			num6 = 180;
		}
		int num8 = 240;
		int num9 = lookDirection;
		int num10 = -1;
		int duration2 = 0;
		switch (npcType)
		{
		case 54:
		case 107:
		case 227:
		case 229:
		case 353:
		case 550:
		case 663:
			num9 *= -1;
			break;
		}
		if ((uint)(npcType - 207) <= 2u || npcType == 228 || (uint)(npcType - 368) <= 1u)
		{
			flag = true;
		}
		switch (npcType)
		{
		case 107:
			num10 = 0;
			break;
		case 208:
			num10 = 127;
			break;
		case 353:
			num10 = 136;
			break;
		case 54:
			num10 = 126;
			break;
		case 368:
			num10 = 15;
			break;
		case 229:
			num10 = 85;
			break;
		}
		if (num10 != -1)
		{
			duration2 = npcType % 6 * 20 + 60;
		}
		int num11 = duration - timeToJumpAt - num - num8;
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions = new Segments.NPCSegment(fromTime, npcType, sceneAnchoePosition, _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).With(new Actions.NPCs.LookAt(num9));
		if (flag)
		{
			animationSegmentWithActions.With(new Actions.NPCs.PartyHard());
		}
		animationSegmentWithActions.Then(new Actions.NPCs.Wait(num8)).Then(new Actions.NPCs.LookAt(lookDirection)).Then(new Actions.NPCs.Wait(timeToJumpAt))
			.Then(new Actions.NPCs.MoveWithAcceleration(new Vector2(0f, 0f - num2), new Vector2(0f, num3), (int)num4))
			.With(new Actions.NPCs.Move(new Vector2(0f, 1E-05f), (int)num4))
			.Then(new Actions.NPCs.Wait(num11 - 90 + num5))
			.Then(new Actions.NPCs.Wait(90 - num5));
		if (num6 > 0)
		{
			animationSegmentWithActions.With(new Actions.NPCs.Blink(num6));
		}
		animationSegmentWithActions.Then(new Actions.NPCs.Fade(3, 85));
		if (npcType == 663)
		{
			AddEmote(sceneAnchoePosition, fromTime, duration, num5, 17, lookDirection);
		}
		if (num10 != -1)
		{
			AddEmote(sceneAnchoePosition, fromTime, duration2, 0, num10, num9);
		}
		_segments.Add(animationSegmentWithActions);
	}

	private void AddEmote(Vector2 sceneAnchoePosition, int fromTime, int duration, int blinkTime, int emoteId, int direction)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		Segments.EmoteSegment item = new Segments.EmoteSegment(emoteId, fromTime + duration - blinkTime, 60, sceneAnchoePosition + _emoteBubbleOffsetWhenOnRight, (direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
		_segments.Add(item);
	}

	private SegmentInforReport PlaySegment_TextRoll(int startTime, string sourceCategory, Vector2 anchorOffset = default(Vector2))
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		anchorOffset.Y -= 40f;
		int num = 55;
		LocalizedText[] array = Language.FindAll(Lang.CreateDialogFilter(sourceCategory + "."));
		for (int i = 0; i < array.Length; i++)
		{
			_segments.Add(new Segments.LocalizedTextSegment(startTime + i * num, array[i], anchorOffset));
		}
		return new SegmentInforReport
		{
			totalTime = array.Length * num + num * -1
		};
	}

	private SegmentInforReport PlaySegment_GuideEmotingAtRainbowPanel(int startTime)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> val = TextureAssets.Extra[156];
		DrawData data = new DrawData(val.Value, Vector2.Zero, null, Color.White, 0f, val.Size() / 2f, 0.25f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, startTime, data, new Vector2(0f, -60f)).Then(new Actions.Sprites.Fade(0f, 0)).Then(new Actions.Sprites.Fade(1f, 60)).Then(new Actions.Sprites.Wait(60))
			.Then(new Actions.Sprites.Fade(0f, 60));
		_segments.Add(animationSegmentWithActions);
		return new SegmentInforReport
		{
			totalTime = (int)animationSegmentWithActions.DedicatedTimeNeeded
		};
	}

	private SegmentInforReport PlaySegment_Grox_DryadSayingByeToTavernKeep(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = 0;
		sceneAnchorPosition.X += num2;
		int num3 = 30;
		int num4 = 10;
		Asset<Texture2D> val = TextureAssets.Extra[235];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + new Vector2((float)num4, 0f) + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(120));
		_segments.Add(animationSegmentWithActions);
		int num5 = 300;
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 20, sceneAnchorPosition + new Vector2((float)(num4 + num5), 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 120));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions3 = new Segments.NPCSegment(startTime, 550, sceneAnchorPosition + new Vector2((float)(num4 + num5 - 16 - num3), 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 120));
		Asset<Texture2D> val3 = TextureAssets.Extra[240];
		Rectangle val4 = val3.Frame(1, 8);
		DrawData data2 = new DrawData(val3.Value, Vector2.Zero, val4, Color.White, 0f, val4.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions4 = new Segments.SpriteSegment(val3, startTime, data2, sceneAnchorPosition + new Vector2((float)num4, 2f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51));
		num += (int)animationSegmentWithActions3.DedicatedTimeNeeded;
		int num6 = 90;
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 90));
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 30));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(60));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(90));
		num += 90;
		int num7 = num6 * 5;
		int num8 = num4 + num5 - 120 - 30;
		int num9 = num4 + num5 - 120 - 106 - num3;
		Segments.EmoteSegment item = new Segments.EmoteSegment(14, num, num6, sceneAnchorPosition + new Vector2((float)num8, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(133, num + num6, num6, sceneAnchorPosition + new Vector2((float)num9, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item3 = new Segments.EmoteSegment(78, num + num6 * 2, num6, sceneAnchorPosition + new Vector2((float)num8, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		Segments.EmoteSegment item4 = new Segments.EmoteSegment(15, num + num6 * 4, num6, sceneAnchorPosition + new Vector2((float)num9, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item5 = new Segments.EmoteSegment(15, num + num6 * 4, num6, sceneAnchorPosition + new Vector2((float)num8, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions3.Then(new Actions.NPCs.LookAt(1));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num6 * 3));
		animationSegmentWithActions3.Then(new Actions.NPCs.ShowItem(num6, 353));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num7));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num7));
		num += num7;
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 30));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(30));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(30));
		num += 30;
		Main.instance.LoadNPC(550);
		Asset<Texture2D> val5 = TextureAssets.Npc[550];
		Rectangle val6 = val5.Frame(1, Main.npcFrameCount[550]);
		DrawData data3 = new DrawData(val5.Value, Vector2.Zero, val6, Color.White, 0f, val6.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions5 = new Segments.SpriteSegment(val5, num, data3, sceneAnchorPosition + new Vector2((float)(num9 - 30), 2f)).Then(new Actions.Sprites.Fade(1f));
		animationSegmentWithActions5.Then(new Actions.Sprites.SimulateGravity(new Vector2(-0.2f, -0.35f), Vector2.Zero, 0f, 80)).With(new Actions.Sprites.SetFrameSequence(80, new Point[13]
		{
			new Point(0, 2),
			new Point(0, 3),
			new Point(0, 4),
			new Point(0, 5),
			new Point(0, 6),
			new Point(0, 7),
			new Point(0, 8),
			new Point(0, 9),
			new Point(0, 10),
			new Point(0, 11),
			new Point(0, 12),
			new Point(0, 13),
			new Point(0, 14)
		}, 4, 0, 0)).With(new Actions.Sprites.Fade(0f, 85));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(80));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(80));
		num += 80;
		animationSegmentWithActions4.Then(new Actions.Sprites.SetFrameSequence(num - startTime, new Point[8]
		{
			new Point(0, 0),
			new Point(0, 1),
			new Point(0, 2),
			new Point(0, 3),
			new Point(0, 4),
			new Point(0, 5),
			new Point(0, 6),
			new Point(0, 7)
		}, 5, 0, 0));
		Segments.EmoteSegment item6 = new Segments.EmoteSegment(10, num, num6, sceneAnchorPosition + new Vector2((float)num8, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions4.Then(new Actions.Sprites.Fade(0f, num6 - 30));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num6));
		animationSegmentWithActions2.Then(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions4);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(animationSegmentWithActions5);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(item);
		_segments.Add(item2);
		_segments.Add(item3);
		_segments.Add(item5);
		_segments.Add(item4);
		_segments.Add(item6);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_Grox_SteampunkerRepairingCyborg(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = 30;
		sceneAnchorPosition.X += num2;
		int num3 = 60;
		Asset<Texture2D> val = TextureAssets.Extra[232];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(60));
		_segments.Add(animationSegmentWithActions);
		val = TextureAssets.Extra[233];
		val2 = val.Frame();
		data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions2 = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(60));
		_segments.Add(animationSegmentWithActions);
		_segments.Add(animationSegmentWithActions2);
		Asset<Texture2D> val3 = TextureAssets.Extra[230];
		Rectangle val4 = val3.Frame(1, 21);
		DrawData data2 = new DrawData(val3.Value, Vector2.Zero, val4, Color.White, 0f, val4.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)0);
		Segments.SpriteSegment spriteSegment = new Segments.SpriteSegment(val3, num, data2, sceneAnchorPosition + new Vector2(0f, 4f));
		spriteSegment.Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60)).Then(new Actions.Sprites.Wait(60));
		Asset<Texture2D> val5 = TextureAssets.Extra[229];
		Rectangle val6 = val5.Frame(1, 2);
		DrawData data3 = new DrawData(val5.Value, Vector2.Zero, val6, Color.White, 0f, val6.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)0);
		Segments.SpriteSegment spriteSegment2 = new Segments.SpriteSegment(val5, num, data3, sceneAnchorPosition + new Vector2((float)num3, 4f));
		spriteSegment2.Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60)).Then(new Actions.Sprites.Wait(60));
		num += (int)spriteSegment.DedicatedTimeNeeded;
		int num4 = 120;
		spriteSegment.Then(new Actions.Sprites.SetFrameSequence(num4, new Point[2]
		{
			new Point(0, 0),
			new Point(0, 1)
		}, 10, 0, 0));
		spriteSegment2.Then(new Actions.Sprites.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		animationSegmentWithActions2.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		Point[] array = new Point[29]
		{
			new Point(0, 2),
			new Point(0, 3),
			new Point(0, 4),
			new Point(0, 5),
			new Point(0, 6),
			new Point(0, 7),
			new Point(0, 8),
			new Point(0, 9),
			new Point(0, 10),
			new Point(0, 11),
			new Point(0, 12),
			new Point(0, 13),
			new Point(0, 14),
			new Point(0, 15),
			new Point(0, 16),
			new Point(0, 17),
			new Point(0, 18),
			new Point(0, 19),
			new Point(0, 20),
			new Point(0, 15),
			new Point(0, 16),
			new Point(0, 17),
			new Point(0, 18),
			new Point(0, 19),
			new Point(0, 20),
			new Point(0, 17),
			new Point(0, 18),
			new Point(0, 19),
			new Point(0, 20)
		};
		int num5 = 6;
		int num6 = num5 * array.Length;
		spriteSegment.Then(new Actions.Sprites.SetFrameSequence(array, num5, 0, 0));
		int durationInFrames = num6 / 2;
		spriteSegment2.Then(new Actions.Sprites.Wait(durationInFrames));
		spriteSegment2.Then(new Actions.Sprites.SetFrame(0, 1, 0, 0));
		spriteSegment2.Then(new Actions.Sprites.Wait(durationInFrames));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num6));
		animationSegmentWithActions2.Then(new Actions.Sprites.Wait(num6));
		num += num6;
		array = new Point[4]
		{
			new Point(0, 17),
			new Point(0, 18),
			new Point(0, 19),
			new Point(0, 20)
		};
		spriteSegment.Then(new Actions.Sprites.SetFrameSequence(187, array, num5, 0, 0)).With(new Actions.Sprites.Fade(0f, 127));
		spriteSegment2.Then(new Actions.Sprites.Fade(0f, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(spriteSegment);
		_segments.Add(spriteSegment2);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_Grox_SantaAndTaxCollectorThrowingPresents(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = 0;
		sceneAnchorPosition.X += num2;
		int num3 = 120;
		Asset<Texture2D> val = TextureAssets.Extra[236];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, startTime, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(120));
		_segments.Add(animationSegmentWithActions);
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 142, sceneAnchorPosition + new Vector2(-30f, 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 120));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions3 = new Segments.NPCSegment(startTime, 441, sceneAnchorPosition + new Vector2((float)num3, 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Wait(120));
		Asset<Texture2D> val3 = TextureAssets.Extra[239];
		Rectangle val4 = val3.Frame(1, 8);
		DrawData data2 = new DrawData(val3.Value, Vector2.Zero, val4, Color.White, 0f, val4.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)1);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions4 = new Segments.SpriteSegment(val3, num, data2, sceneAnchorPosition + new Vector2((float)(num2 - 44), 4f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		int num4 = 120;
		int num5 = 90;
		Segments.EmoteSegment item = new Segments.EmoteSegment(125, num, num4, sceneAnchorPosition + new Vector2(30f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(10, num, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		int num6 = num4 + 30;
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions3.Then(new Actions.NPCs.LookAt(-1));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num6));
		num += num6;
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		Segments.EmoteSegment item3 = new Segments.EmoteSegment(3, num, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		Segments.EmoteSegment item4 = new Segments.EmoteSegment(136, num, num4, sceneAnchorPosition + new Vector2(30f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item5 = new Segments.EmoteSegment(15, num, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		animationSegmentWithActions2.Then(new Actions.NPCs.ShowItem(num5 + num4 + num4, 3749));
		animationSegmentWithActions4.Then(new Actions.Sprites.SetFrameSequence(num - startTime, new Point[8]
		{
			new Point(0, 0),
			new Point(0, 1),
			new Point(0, 2),
			new Point(0, 3),
			new Point(0, 4),
			new Point(0, 5),
			new Point(0, 6),
			new Point(0, 7)
		}, 5, 0, 0));
		animationSegmentWithActions2.Then(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions3.Then(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions4.Then(new Actions.Sprites.Fade(0f, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions4);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(item);
		_segments.Add(item2);
		_segments.Add(item3);
		_segments.Add(item4);
		_segments.Add(item5);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_Grox_WitchDoctorGoingToHisPeople(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_0932: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = 0;
		sceneAnchorPosition.X += num2;
		int num3 = 60;
		Asset<Texture2D> val = TextureAssets.Extra[231];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, startTime, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(120));
		_segments.Add(animationSegmentWithActions);
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 228, sceneAnchorPosition + new Vector2(-60f, 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 120));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions3 = new Segments.NPCSegment(startTime, 663, sceneAnchorPosition + new Vector2(-110f, 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 120));
		Point[] frameIndices = new Point[5]
		{
			new Point(0, 3),
			new Point(0, 4),
			new Point(0, 5),
			new Point(0, 6),
			new Point(0, 7)
		};
		Point[] frameIndices2 = new Point[4]
		{
			new Point(0, 3),
			new Point(0, 2),
			new Point(0, 1),
			new Point(0, 0)
		};
		Main.instance.LoadNPC(199);
		Asset<Texture2D> val3 = TextureAssets.Npc[199];
		Rectangle val4 = val3.Frame(1, Main.npcFrameCount[199]);
		DrawData data2 = new DrawData(val3.Value, Vector2.Zero, val4, Color.White, 0f, val4.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)0);
		new DrawData(val3.Value, Vector2.Zero, val4, Color.White, 0f, val4.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)1);
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions4 = new Segments.NPCSegment(startTime, 198, sceneAnchorPosition + new Vector2((float)(num3 * 2), 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Wait(120));
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions5 = new Segments.SpriteSegment(val3, startTime, data2, sceneAnchorPosition + new Vector2((float)(num3 * 3 - 20 + 120), 4f)).Then(new Actions.Sprites.SetFrame(0, 3, 0, 0)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 25))
			.Then(new Actions.Sprites.SimulateGravity(new Vector2(-1f, 0f), Vector2.Zero, 0f, 120))
			.With(new Actions.Sprites.SetFrameSequence(120, frameIndices, 6, 0, 0));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		int num4 = 120;
		Segments.EmoteSegment item = new Segments.EmoteSegment(10, num, num4, sceneAnchorPosition + new Vector2(0f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		int num5 = 6;
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions5.Then(new Actions.Sprites.SetFrameSequence(frameIndices2, num5, 0, 0));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		int durationInFrames = num4 - num5 * 4;
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions6 = new Segments.NPCSegment(num - num4 + num5 * 4, 198, sceneAnchorPosition + new Vector2((float)(num3 * 3 - 20), 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(-1)).Then(new Actions.NPCs.Wait(durationInFrames));
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(92, num, num4, sceneAnchorPosition + new Vector2(-50f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions2.Then(new Actions.NPCs.LookAt(-1));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions6.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		int num6 = 60;
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), num6));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions6.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num6));
		num += num6;
		Segments.EmoteSegment item3 = new Segments.EmoteSegment(87, num, num4, sceneAnchorPosition + new Vector2((float)(num3 * 2), 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num4)).Then(new Actions.NPCs.LookAt(-1));
		animationSegmentWithActions6.Then(new Actions.NPCs.Wait(num4)).Then(new Actions.NPCs.LookAt(-1));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		Segments.EmoteSegment item4 = new Segments.EmoteSegment(49, num, num4, sceneAnchorPosition + new Vector2(30f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions6.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		int num7 = num4 + num4 / 2;
		Segments.EmoteSegment item5 = new Segments.EmoteSegment(10, num, num4, sceneAnchorPosition + new Vector2((float)(num3 * 2), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		Segments.EmoteSegment item6 = new Segments.EmoteSegment(0, num + num4 / 2, num4, sceneAnchorPosition + new Vector2((float)(num3 * 3 - 20), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num7));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num7));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num7));
		animationSegmentWithActions6.Then(new Actions.NPCs.Wait(num7));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num7));
		num += num7;
		Segments.EmoteSegment item7 = new Segments.EmoteSegment(17, num, num4, sceneAnchorPosition + new Vector2(-50f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item8 = new Segments.EmoteSegment(3, num, num4, sceneAnchorPosition + new Vector2(30f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions6.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(-0.4f, 0f), 160)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 160)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions4.Then(new Actions.NPCs.Move(new Vector2(-0.8f, 0f), 160)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions6.Then(new Actions.NPCs.Move(new Vector2(-0.8f, 0f), 160)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions4);
		_segments.Add(animationSegmentWithActions6);
		_segments.Add(animationSegmentWithActions5);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(item);
		_segments.Add(item2);
		_segments.Add(item3);
		_segments.Add(item4);
		_segments.Add(item5);
		_segments.Add(item6);
		_segments.Add(item8);
		_segments.Add(item7);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private Vector2 GetSceneFixVector()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(0f - _backgroundOffset.X, 0f);
	}

	private SegmentInforReport PlaySegment_DryadTurningToTree(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		Asset<Texture2D> val = TextureAssets.Extra[217];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2(0f, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 20, sceneAnchorPosition, _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 60))
			.Then(new Actions.NPCs.Wait(10))
			.Then(new Actions.NPCs.Fade(0));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		Asset<Texture2D> val3 = TextureAssets.Extra[215];
		Rectangle val4 = val3.Frame(1, 9);
		DrawData data2 = new DrawData(val3.Value, Vector2.Zero, val4, Color.White, 0f, val4.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)1);
		Vector2 val5 = new Vector2(1f, 0f) * 60f + new Vector2(2f, 4f);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions3 = new Segments.SpriteSegment(val3, num, data2, sceneAnchorPosition + val5).Then(new Actions.Sprites.SetFrameSequence(new Point[9]
		{
			new Point(0, 0),
			new Point(0, 1),
			new Point(0, 2),
			new Point(0, 3),
			new Point(0, 4),
			new Point(0, 5),
			new Point(0, 6),
			new Point(0, 7),
			new Point(0, 8)
		}, 8, 0, 0)).Then(new Actions.Sprites.Wait(30));
		num += (int)animationSegmentWithActions3.DedicatedTimeNeeded;
		Segments.AnimationSegmentWithActions<NPC> item = new Segments.NPCSegment(num, 46, sceneAnchorPosition + new Vector2(-100f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 60))
			.Then(new Actions.NPCs.Wait(90))
			.Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 120))
			.With(new Actions.NPCs.Fade(3, 85));
		Segments.AnimationSegmentWithActions<NPC> item2 = new Segments.NPCSegment(num + 60, 299, sceneAnchorPosition + new Vector2(170f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 60))
			.Then(new Actions.NPCs.Wait(60))
			.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 90))
			.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 85))
			.With(new Actions.NPCs.Fade(3, 85));
		float num2 = 1.5f;
		Segments.AnimationSegmentWithActions<NPC> item3 = new Segments.NPCSegment(num + 45, 74, sceneAnchorPosition + new Vector2(-80f, -70f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(num2, 0f), 85))
			.With(new Actions.NPCs.MoveWithRotor(new Vector2(10f, 0f), (float)Math.PI * 2f / 85f, new Vector2(0f, 1f), 85))
			.Then(new Actions.NPCs.Move(new Vector2(num2, 0f), 85))
			.With(new Actions.NPCs.MoveWithRotor(new Vector2(4f, 0f), (float)Math.PI * 2f / 85f, new Vector2(0f, 1f), 85))
			.With(new Actions.NPCs.Fade(3, 85));
		Segments.AnimationSegmentWithActions<NPC> item4 = new Segments.NPCSegment(num + 180, 656, sceneAnchorPosition + new Vector2(20f, 0f), _originAtBottom).Then(new Actions.NPCs.Variant(1)).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255))
			.With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Wait(60))
			.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 60))
			.Then(new Actions.NPCs.Wait(60))
			.Then(new Actions.NPCs.DoBunnyRestAnimation(90))
			.Then(new Actions.NPCs.Wait(90))
			.With(new Actions.NPCs.Fade(3, 120));
		Segments.EmoteSegment item5 = new Segments.EmoteSegment(0, num + 360, 60, sceneAnchorPosition + new Vector2(36f, -10f), (SpriteEffects)1, Vector2.Zero);
		animationSegmentWithActions3.Then(new Actions.Sprites.Wait(420)).Then(new Actions.Sprites.Fade(0f, 120));
		num += 620;
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num - startTime - 180)).Then(new Actions.Sprites.Fade(0f, 120));
		_segments.Add(animationSegmentWithActions);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(item);
		_segments.Add(item2);
		_segments.Add(item3);
		_segments.Add(item4);
		_segments.Add(item5);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_SantaItemExample(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = 0;
		for (int i = 0; i < num; i++)
		{
			int num2 = Main.rand.NextFromList(new short[4] { 599, 1958, 3749, 1869 });
			Main.instance.LoadItem(num2);
			Asset<Texture2D> val = TextureAssets.Item[num2];
			DrawData data = new DrawData(val.Value, Vector2.Zero, null, Color.White, 0f, val.Size() / 2f, 1f, (SpriteEffects)0);
			Vector2 initialVelocity = Vector2.UnitY * -12f + Main.rand.NextVector2Circular(6f, 3f).RotatedBy((float)(i - num / 2) * ((float)Math.PI * 2f) * 0.1f);
			Vector2 gravityPerFrame = Vector2.UnitY * 0.2f;
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> item = new Segments.SpriteSegment(val, startTime, data, sceneAnchorPosition).Then(new Actions.Sprites.SimulateGravity(initialVelocity, gravityPerFrame, Main.rand.NextFloatDirection() * 0.2f, 60)).With(new Actions.Sprites.Fade(0f, 60));
			_segments.Add(item);
		}
		Segments.AnimationSegmentWithActions<NPC> item2 = new Segments.NPCSegment(startTime, 142, sceneAnchorPosition, _originAtBottom).Then(new Actions.NPCs.ShowItem(30, 267)).Then(new Actions.NPCs.Wait(10)).Then(new Actions.NPCs.ShowItem(30, 600))
			.Then(new Actions.NPCs.Wait(10))
			.Then(new Actions.NPCs.ShowItem(30, 2))
			.Then(new Actions.NPCs.Wait(10));
		_segments.Add(item2);
		return new SegmentInforReport
		{
			totalTime = 170
		};
	}

	private SegmentInforReport PlaySegment_Grox_SkeletonMerchantSearchesThroughBones(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_077b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = 30;
		sceneAnchorPosition.X += num2;
		int num3 = 100;
		Asset<Texture2D> val = TextureAssets.Extra[220];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(60));
		_segments.Add(animationSegmentWithActions);
		int num4 = 10;
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 453, sceneAnchorPosition + new Vector2((float)num4, 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 60));
		Asset<Texture2D> val3 = TextureAssets.Extra[227];
		DrawData data2 = new DrawData(val3.Value, Vector2.Zero, null, Color.White, 0f, val3.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions3 = new Segments.SpriteSegment(val3, startTime, data2, sceneAnchorPosition + new Vector2((float)num3, 2f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51)).Then(new Actions.Sprites.Wait(60));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		int num5 = 90;
		Segments.EmoteSegment item = new Segments.EmoteSegment(87, num, num5, sceneAnchorPosition + new Vector2((float)(60 + num4), 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions3.Then(new Actions.Sprites.Wait(num5));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		Asset<Texture2D> val4 = TextureAssets.Extra[228];
		Rectangle val5 = val4.Frame(1, 14);
		DrawData data3 = new DrawData(val4.Value, Vector2.Zero, val5, Color.White, 0f, val5.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)1);
		Segments.SpriteSegment spriteSegment = new Segments.SpriteSegment(val4, num, data3, sceneAnchorPosition + new Vector2((float)(num3 - 10), 4f));
		spriteSegment.Then(new Actions.Sprites.SetFrameSequence(new Point[4]
		{
			new Point(0, 1),
			new Point(0, 2),
			new Point(0, 3),
			new Point(0, 4)
		}, 5, 0, 0));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(20)).With(new Actions.NPCs.Fade(255));
		animationSegmentWithActions3.Then(new Actions.Sprites.Wait(20));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(20));
		num += 20;
		int num6 = 10;
		Main.instance.LoadItem(154);
		Asset<Texture2D> val6 = TextureAssets.Item[154];
		DrawData drawData = new DrawData(val6.Value, Vector2.Zero, null, Color.White, 0f, val6.Size() / 2f, 1f, (SpriteEffects)0);
		Main.instance.LoadItem(1274);
		Asset<Texture2D> val7 = TextureAssets.Item[1274];
		DrawData drawData2 = new DrawData(val7.Value, Vector2.Zero, null, Color.White, 0f, val7.Size() / 2f, 1f, (SpriteEffects)0);
		Vector2 anchorOffset = sceneAnchorPosition + new Vector2((float)num3, -8f);
		for (int i = 0; i < num6; i++)
		{
			Vector2 initialVelocity = Vector2.UnitY * -5f + Main.rand.NextVector2Circular(2.5f, 0.3f + Main.rand.NextFloat() * 0.2f).RotatedBy((float)(i - num6 / 2) * ((float)Math.PI * 2f) * 0.1f);
			Vector2 gravityPerFrame = Vector2.UnitY * 0.1f;
			int targetTime = num + i * 10;
			DrawData data4 = drawData;
			Asset<Texture2D> asset = val6;
			if (i == num6 - 3)
			{
				data4 = drawData2;
				asset = val7;
			}
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> item2 = new Segments.SpriteSegment(asset, targetTime, data4, anchorOffset).Then(new Actions.Sprites.SimulateGravity(initialVelocity, gravityPerFrame, Main.rand.NextFloatDirection() * 0.2f, 60)).With(new Actions.Sprites.Fade(0f, 60));
			_segments.Add(item2);
		}
		int num7 = 30 + num6 * 10;
		spriteSegment.Then(new Actions.Sprites.SetFrameSequence(num7, new Point[4]
		{
			new Point(0, 5),
			new Point(0, 6),
			new Point(0, 7),
			new Point(0, 8)
		}, 5, 0, 0));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num7));
		animationSegmentWithActions3.Then(new Actions.Sprites.Wait(num7));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num7));
		num += num7;
		Segments.EmoteSegment item3 = new Segments.EmoteSegment(3, num, num5, sceneAnchorPosition + new Vector2((float)(80 + num4), 4f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		spriteSegment.Then(new Actions.Sprites.Wait(num5)).With(new Actions.Sprites.SetFrame(0, 5, 0, 0));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions3.Then(new Actions.Sprites.Wait(num5));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		spriteSegment.Then(new Actions.Sprites.SetFrameSequence(new Point[4]
		{
			new Point(0, 9),
			new Point(0, 10),
			new Point(0, 11),
			new Point(0, 13)
		}, 5, 0, 0));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(20));
		animationSegmentWithActions3.Then(new Actions.Sprites.Wait(20));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(20));
		num += 20;
		int num8 = 90;
		spriteSegment.Then(new Actions.Sprites.Fade(0f));
		animationSegmentWithActions2.Then(new Actions.NPCs.ShowItem(num8, 3258)).With(new Actions.NPCs.Fade(-255)).With(new Actions.NPCs.LookAt(-1));
		animationSegmentWithActions3.Then(new Actions.Sprites.Wait(num8));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num8));
		num += num8;
		Segments.EmoteSegment item4 = new Segments.EmoteSegment(136, num, num5, sceneAnchorPosition + new Vector2((float)(60 + num4), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0, new Vector2(-1f, 0f));
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), num5));
		animationSegmentWithActions3.Then(new Actions.Sprites.Wait(num5));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions3.Then(new Actions.Sprites.Fade(0f, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(spriteSegment);
		_segments.Add(item);
		_segments.Add(item3);
		_segments.Add(item4);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_Grox_MerchantAndTravelingMerchantTryingToSellJunk(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = 40;
		sceneAnchorPosition.X += num2;
		int num3 = 62;
		Asset<Texture2D> val = TextureAssets.Extra[223];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(60));
		_segments.Add(animationSegmentWithActions);
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 17, sceneAnchorPosition, _originAtBottom).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Wait(60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions3 = new Segments.NPCSegment(startTime, 368, sceneAnchorPosition + new Vector2((float)num3, 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(-1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Wait(60));
		Asset<Texture2D> val3 = TextureAssets.Extra[239];
		Rectangle val4 = val3.Frame(1, 8);
		DrawData data2 = new DrawData(val3.Value, Vector2.Zero, val4, Color.White, 0f, val4.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)1);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions4 = new Segments.SpriteSegment(val3, num, data2, sceneAnchorPosition + new Vector2((float)(num2 - 128), 4f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		int num4 = 90;
		int num5 = 60;
		animationSegmentWithActions2.Then(new Actions.NPCs.ShowItem(num5, 8));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		Segments.EmoteSegment item = new Segments.EmoteSegment(11, num, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions3.Then(new Actions.NPCs.ShowItem(num5, 2242));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(11, num, num4, sceneAnchorPosition + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		animationSegmentWithActions2.Then(new Actions.NPCs.ShowItem(num5, 88));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		Segments.EmoteSegment item3 = new Segments.EmoteSegment(11, num, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions3.Then(new Actions.NPCs.ShowItem(num5, 4761));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		Segments.EmoteSegment item4 = new Segments.EmoteSegment(11, num, num4, sceneAnchorPosition + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		int num6 = num5 + 30;
		animationSegmentWithActions2.Then(new Actions.NPCs.ShowItem(num6, 2));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num6));
		num += num6;
		Segments.EmoteSegment item5 = new Segments.EmoteSegment(10, num, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions3.Then(new Actions.NPCs.ShowItem(num6, 52));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num6));
		num += num6;
		Segments.EmoteSegment item6 = new Segments.EmoteSegment(85, num, num4, sceneAnchorPosition + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item7 = new Segments.EmoteSegment(85, num, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		animationSegmentWithActions4.Then(new Actions.Sprites.SetFrameSequence(num - startTime, new Point[8]
		{
			new Point(0, 0),
			new Point(0, 1),
			new Point(0, 2),
			new Point(0, 3),
			new Point(0, 4),
			new Point(0, 5),
			new Point(0, 6),
			new Point(0, 7)
		}, 5, 0, 0));
		animationSegmentWithActions2.Then(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions3.Then(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions4.Then(new Actions.Sprites.Fade(0f, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions4);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(item);
		_segments.Add(item2);
		_segments.Add(item3);
		_segments.Add(item4);
		_segments.Add(item5);
		_segments.Add(item6);
		_segments.Add(item7);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_Grox_GuideRunningFromZombie(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = 12;
		sceneAnchorPosition.X += num2;
		int num3 = 24;
		Asset<Texture2D> val = TextureAssets.Extra[218];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 3, sceneAnchorPosition + new Vector2((float)(num3 + 60), 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 60));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait((int)animationSegmentWithActions2.DedicatedTimeNeeded));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		animationSegmentWithActions2.Then(new Actions.NPCs.ZombieKnockOnDoor(60));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(60));
		num += 60;
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions3 = new Segments.NPCSegment(num, 22, sceneAnchorPosition + new Vector2(-30f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 60));
		animationSegmentWithActions2.Then(new Actions.NPCs.ZombieKnockOnDoor((int)animationSegmentWithActions3.DedicatedTimeNeeded));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait((int)animationSegmentWithActions3.DedicatedTimeNeeded));
		num += (int)animationSegmentWithActions3.DedicatedTimeNeeded;
		int num4 = 90;
		Segments.EmoteSegment item = new Segments.EmoteSegment(87, num, num4, sceneAnchorPosition + new Vector2(-4f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions2.Then(new Actions.NPCs.ZombieKnockOnDoor(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4 - 1));
		num += num4;
		int num5 = 50;
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(3, num, num5, sceneAnchorPosition + new Vector2(-4f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		val = TextureAssets.Extra[219];
		val2 = val.Frame();
		data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions4 = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(1f));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(-0.4f, 0f), num5));
		animationSegmentWithActions4.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		Segments.EmoteSegment item3 = new Segments.EmoteSegment(134, num, num4, sceneAnchorPosition + new Vector2(0f, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0, new Vector2(-0.6f, 0f));
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(-0.6f, 0f), num4));
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(-0.4f, 0f), num4));
		animationSegmentWithActions4.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(-0.6f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(-0.4f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions4.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions);
		_segments.Add(animationSegmentWithActions4);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(item);
		_segments.Add(item2);
		_segments.Add(item3);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_Grox_ZoologistAndPetsAnnoyGolfer(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_077b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = -28;
		sceneAnchorPosition.X += num2;
		int num3 = 40;
		Asset<Texture2D> val = TextureAssets.Extra[224];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(60));
		_segments.Add(animationSegmentWithActions);
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 633, sceneAnchorPosition + new Vector2(-60f, 0f), _originAtBottom).Then(new Actions.NPCs.ForceAltTexture(3)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions3 = new Segments.NPCSegment(startTime, 656, sceneAnchorPosition + new Vector2((float)(num3 - 60), 0f), _originAtBottom).Then(new Actions.NPCs.Variant(3)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions4 = new Segments.NPCSegment(startTime, 638, sceneAnchorPosition + new Vector2((float)(num3 * 2 - 60), 0f), _originAtBottom).Then(new Actions.NPCs.Variant(2)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions5 = new Segments.NPCSegment(startTime, 637, sceneAnchorPosition + new Vector2((float)(num3 * 3 - 60), 0f), _originAtBottom).Then(new Actions.NPCs.Variant(4)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 60));
		Main.instance.LoadProjectile(748);
		Asset<Texture2D> val3 = TextureAssets.Projectile[748];
		DrawData data2 = new DrawData(val3.Value, Vector2.Zero, null, Color.White, 0f, val3.Size() / 2f, 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions6 = new Segments.SpriteSegment(val3, startTime, data2, sceneAnchorPosition + new Vector2((float)(num3 * 3 - 20), 0f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51)).Then(new Actions.Sprites.Wait(60));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		int num4 = 90;
		float num5 = 0.5f;
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(1f * num5, 0f), num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(0.5f * num5, 0f), num4));
		animationSegmentWithActions4.Then(new Actions.NPCs.Move(new Vector2(0.6f * num5, 0f), num4));
		animationSegmentWithActions5.Then(new Actions.NPCs.Move(new Vector2(0.8f * num5, 0f), num4));
		animationSegmentWithActions6.Then(new Actions.Sprites.SimulateGravity(new Vector2(0.82f * num5, 0f), Vector2.Zero, 0.07f, num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions7 = new Segments.NPCSegment(num, 588, sceneAnchorPosition + new Vector2(-70f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(0.7f * num5, 0f), 60));
		int num6 = (int)animationSegmentWithActions7.DedicatedTimeNeeded;
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(1f * num5, 0f), num6));
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(0.85f * num5, 0f), num6));
		animationSegmentWithActions4.Then(new Actions.NPCs.Move(new Vector2(0.7f * num5, 0f), num6));
		animationSegmentWithActions5.Then(new Actions.NPCs.Move(new Vector2(0.65f * num5, 0f), num6));
		animationSegmentWithActions6.Then(new Actions.Sprites.SimulateGravity(new Vector2(1f * num5, 0f), Vector2.Zero, 0.07f, num6));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num6));
		num += num6;
		int num7 = 90;
		int num8 = num7 * 2 + num7 / 2;
		Segments.EmoteSegment item = new Segments.EmoteSegment(1, num, num7, sceneAnchorPosition + new Vector2(-70f + 42f * num5, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1, new Vector2(1f * num5, 0f));
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(15, num + num7 / 2, num7, sceneAnchorPosition + new Vector2((float)(80 + num6) * num5, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1, new Vector2(1f * num5, 0f));
		animationSegmentWithActions7.Then(new Actions.NPCs.Move(new Vector2(1f * num5, 0f), num8));
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(1f * num5, 0f), num8));
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(0.72f * num5, 0f), num8));
		animationSegmentWithActions4.Then(new Actions.NPCs.Move(new Vector2(0.7f * num5, 0f), num8));
		animationSegmentWithActions5.Then(new Actions.NPCs.Move(new Vector2(0.8f * num5, 0f), num8));
		animationSegmentWithActions6.Then(new Actions.Sprites.SimulateGravity(new Vector2(0.85f * num5, 0f), Vector2.Zero, 0.07f, num8));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num8));
		num += num8;
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(0.5f * num5, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions7.Then(new Actions.NPCs.Move(new Vector2(0.5f * num5, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(0.6f * num5, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions4.Then(new Actions.NPCs.Move(new Vector2(0.7f * num5, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions5.Then(new Actions.NPCs.Move(new Vector2(0.6f * num5, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions6.Then(new Actions.Sprites.SimulateGravity(new Vector2(0.5f * num5, 0f), Vector2.Zero, 0.05f, 120)).With(new Actions.Sprites.Fade(0f, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(animationSegmentWithActions7);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(animationSegmentWithActions4);
		_segments.Add(animationSegmentWithActions5);
		_segments.Add(animationSegmentWithActions6);
		_segments.Add(item2);
		_segments.Add(item);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_Grox_AnglerAndPirateTalkAboutFish(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = 30;
		sceneAnchorPosition.X += num2;
		int num3 = 90;
		Asset<Texture2D> val = TextureAssets.Extra[222];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(60));
		_segments.Add(animationSegmentWithActions);
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 369, sceneAnchorPosition, _originAtBottom).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Wait(60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions3 = new Segments.NPCSegment(startTime, 229, sceneAnchorPosition + new Vector2((float)(num3 + 60), 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(-1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 60));
		Asset<Texture2D> val3 = TextureAssets.Extra[226];
		Rectangle val4 = val3.Frame(1, 8);
		DrawData data2 = new DrawData(val3.Value, Vector2.Zero, val4, Color.White, 0f, val4.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)1);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions4 = new Segments.SpriteSegment(val3, num, data2, sceneAnchorPosition + new Vector2((float)(num3 / 2), 4f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		int num4 = 90;
		int num5 = num4 * 8;
		Segments.EmoteSegment item = new Segments.EmoteSegment(79, num, num4, sceneAnchorPosition + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(65, num + num4, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		Segments.EmoteSegment item3 = new Segments.EmoteSegment(136, num + num4 * 3, num4, sceneAnchorPosition + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item4 = new Segments.EmoteSegment(3, num + num4 * 5, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		Segments.EmoteSegment item5 = new Segments.EmoteSegment(50, num + num4 * 6, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		Segments.EmoteSegment item6 = new Segments.EmoteSegment(15, num + num4 * 6, num4, sceneAnchorPosition + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0, new Vector2(-1f, 0f));
		Segments.EmoteSegment item7 = new Segments.EmoteSegment(2, num + num4 * 7, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0, new Vector2(-1.25f, 0f));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4 * 4)).Then(new Actions.NPCs.ShowItem(num4, 2673)).Then(new Actions.NPCs.Wait(num4))
			.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4 * 2)).Then(new Actions.NPCs.ShowItem(num4, 2480)).Then(new Actions.NPCs.Wait(num4 * 4))
			.Then(new Actions.NPCs.Move(new Vector2(-1.25f, 0f), num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		animationSegmentWithActions4.Then(new Actions.Sprites.SetFrameSequence(num5 + 60, new Point[8]
		{
			new Point(0, 0),
			new Point(0, 1),
			new Point(0, 2),
			new Point(0, 3),
			new Point(0, 4),
			new Point(0, 5),
			new Point(0, 6),
			new Point(0, 7)
		}, 5, 0, 0));
		num += num5;
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(-0.4f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(-0.75f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions4.Then(new Actions.Sprites.Fade(0f, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions4);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(item);
		_segments.Add(item2);
		_segments.Add(item3);
		_segments.Add(item4);
		_segments.Add(item5);
		_segments.Add(item6);
		_segments.Add(item7);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_Grox_WizardPartyGirlDyeTraderAndPainterPartyWithBunnies(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Unknown result type (might be due to invalid IL or missing references)
		//IL_096b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = -35;
		sceneAnchorPosition.X += num2;
		int num3 = 34;
		Asset<Texture2D> val = TextureAssets.Extra[221];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(60));
		_segments.Add(animationSegmentWithActions);
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 227, sceneAnchorPosition, _originAtBottom).Then(new Actions.NPCs.PartyHard()).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Fade(255))
			.With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Wait(60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions3 = new Segments.NPCSegment(startTime, 108, sceneAnchorPosition + new Vector2((float)num3, 0f), _originAtBottom).Then(new Actions.NPCs.PartyHard()).Then(new Actions.NPCs.LookAt(-1)).Then(new Actions.NPCs.Fade(255))
			.With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Wait(60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions4 = new Segments.NPCSegment(startTime, 207, sceneAnchorPosition + new Vector2((float)(num3 * 2 + 60), 0f), _originAtBottom).Then(new Actions.NPCs.PartyHard()).Then(new Actions.NPCs.LookAt(-1)).Then(new Actions.NPCs.Fade(255))
			.With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions5 = new Segments.NPCSegment(startTime, 656, sceneAnchorPosition + new Vector2((float)(num3 * 2), 0f), _originAtBottom).Then(new Actions.NPCs.Variant(1)).Then(new Actions.NPCs.PartyHard()).Then(new Actions.NPCs.LookAt(1))
			.Then(new Actions.NPCs.Fade(255))
			.With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 60));
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions6 = new Segments.NPCSegment(startTime, 540, sceneAnchorPosition + new Vector2((float)(num3 * 4 + 100), 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(-1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 60));
		Asset<Texture2D> val3 = TextureAssets.Extra[238];
		Rectangle val4 = val3.Frame(1, 4);
		DrawData data2 = new DrawData(val3.Value, Vector2.Zero, val4, Color.White, 0f, val4.Size() * new Vector2(0.5f, 1f), 1f, (SpriteEffects)1);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions7 = new Segments.SpriteSegment(val3, num, data2, sceneAnchorPosition + new Vector2(60f, 2f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51));
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions8 = new Segments.SpriteSegment(val3, num, data2, sceneAnchorPosition + new Vector2(150f, 2f)).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 51));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		int num4 = 90;
		int num5 = num4 * 4;
		Segments.EmoteSegment item = new Segments.EmoteSegment(127, num, num4, sceneAnchorPosition + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(6, num + num4, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		Segments.EmoteSegment item3 = new Segments.EmoteSegment(136, num + num4 * 2, num4, sceneAnchorPosition + new Vector2((float)(num3 * 2), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		Segments.EmoteSegment item4 = new Segments.EmoteSegment(129, num + num4 * 3, num4, sceneAnchorPosition + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4 * 2)).Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Wait(num4))
			.Then(new Actions.NPCs.LookAt(-1))
			.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		animationSegmentWithActions6.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), num5 / 3)).Then(new Actions.NPCs.Wait(num5 / 3)).Then(new Actions.NPCs.Move(new Vector2(0.4f, 0f), num5 / 3));
		animationSegmentWithActions5.Then(new Actions.NPCs.Move(new Vector2(-0.6f, 0f), num5 / 3)).Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), num5 / 3)).Then(new Actions.NPCs.Wait(num5 / 3));
		num += num5;
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions9 = new Segments.NPCSegment(num - 60, 208, sceneAnchorPosition + new Vector2((float)(num3 * 5 + 100), 0f), _originAtBottom).Then(new Actions.NPCs.PartyHard()).Then(new Actions.NPCs.LookAt(-1)).Then(new Actions.NPCs.Fade(255))
			.With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 60));
		int num6 = (int)animationSegmentWithActions9.DedicatedTimeNeeded - 60;
		num += num6;
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions6.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions5.Then(new Actions.NPCs.Wait(num6));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num6));
		Segments.EmoteSegment item5 = new Segments.EmoteSegment(128, num, num4, sceneAnchorPosition + new Vector2((float)(num3 * 5 + 40), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		num += num4;
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions4.Then(new Actions.NPCs.LookAt(1)).Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions6.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions5.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions9.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		Segments.EmoteSegment item6 = new Segments.EmoteSegment(128, num, num4, sceneAnchorPosition + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item7 = new Segments.EmoteSegment(128, num, num4, sceneAnchorPosition + new Vector2((float)num3, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item8 = new Segments.EmoteSegment(128, num, num4, sceneAnchorPosition + new Vector2((float)(num3 * 2), 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item9 = new Segments.EmoteSegment(3, num, num4, sceneAnchorPosition + new Vector2((float)(num3 * 5 - 10), 24f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item10 = new Segments.EmoteSegment(0, num, num4, sceneAnchorPosition + new Vector2((float)(num3 * 4 - 20), 24f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions6.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions5.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions9.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		animationSegmentWithActions7.Then(new Actions.Sprites.SetFrameSequence(num - startTime, new Point[4]
		{
			new Point(0, 0),
			new Point(0, 1),
			new Point(0, 2),
			new Point(0, 3)
		}, 10, 0, 0));
		animationSegmentWithActions8.Then(new Actions.Sprites.SetFrameSequence(num - startTime, new Point[4]
		{
			new Point(0, 2),
			new Point(0, 3),
			new Point(0, 0),
			new Point(0, 1)
		}, 10, 0, 0));
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions4.Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions6.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions5.Then(new Actions.NPCs.Move(new Vector2(0.75f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions9.Then(new Actions.NPCs.Move(new Vector2(0.5f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions7.Then(new Actions.Sprites.Fade(0f, 127));
		animationSegmentWithActions8.Then(new Actions.Sprites.Fade(0f, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions7);
		_segments.Add(animationSegmentWithActions8);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(animationSegmentWithActions4);
		_segments.Add(animationSegmentWithActions6);
		_segments.Add(animationSegmentWithActions5);
		_segments.Add(animationSegmentWithActions9);
		_segments.Add(item);
		_segments.Add(item2);
		_segments.Add(item3);
		_segments.Add(item4);
		_segments.Add(item5);
		_segments.Add(item6);
		_segments.Add(item7);
		_segments.Add(item8);
		_segments.Add(item9);
		_segments.Add(item10);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_Grox_DemolitionistAndArmsDealerArguingThenNurseComes(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = -30;
		sceneAnchorPosition.X += num2;
		Asset<Texture2D> val = TextureAssets.Extra[234];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60))
			.Then(new Actions.Sprites.Wait(120));
		_segments.Add(animationSegmentWithActions);
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 38, sceneAnchorPosition, _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 60))
			.Then(new Actions.NPCs.Wait(60));
		int num3 = 90;
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions3 = new Segments.NPCSegment(startTime, 19, sceneAnchorPosition + new Vector2((float)(120 + num3), 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 60))
			.Then(new Actions.NPCs.Wait(60));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		int num4 = 90 * 4;
		int num5 = 90;
		int num6 = 90 / 2;
		int num7 = 90 + 90 / 2;
		int num8 = 90 * 2;
		Segments.EmoteSegment item = new Segments.EmoteSegment(81, num, num5, sceneAnchorPosition + new Vector2(60f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(82, num + num6, num5, sceneAnchorPosition + new Vector2((float)(60 + num3), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		Segments.EmoteSegment item3 = new Segments.EmoteSegment(135, num + num7, num5, sceneAnchorPosition + new Vector2(60f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item4 = new Segments.EmoteSegment(135, num + num8, num5, sceneAnchorPosition + new Vector2((float)(60 + num3), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num4));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num4));
		num += num4;
		int num9 = num3 - 30;
		int num10 = 120;
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions4 = new Segments.NPCSegment(num - num10, 18, sceneAnchorPosition + new Vector2((float)(120 + num9), 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 60))
			.Then(new Actions.NPCs.Wait(20));
		int num11 = (int)animationSegmentWithActions4.DedicatedTimeNeeded - num10;
		num += num11;
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num11));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num11));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num11));
		animationSegmentWithActions4.Then(new Actions.NPCs.LookAt(-1));
		Segments.EmoteSegment item5 = new Segments.EmoteSegment(77, num, num5, sceneAnchorPosition + new Vector2((float)(60 + num9), 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item6 = new Segments.EmoteSegment(15, num + num5, num5, sceneAnchorPosition + new Vector2((float)(60 + num9), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		animationSegmentWithActions4.Then(new Actions.NPCs.LookAt(1));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		Segments.EmoteSegment item7 = new Segments.EmoteSegment(10, num, num5, sceneAnchorPosition + new Vector2(60f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item8 = new Segments.EmoteSegment(10, num, num5, sceneAnchorPosition + new Vector2((float)(60 + num3), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num5));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num5));
		num += num5;
		Vector2 val3 = new Vector2(-1f, 0f);
		Segments.EmoteSegment item9 = new Segments.EmoteSegment(77, num, num5, sceneAnchorPosition + new Vector2((float)(60 + num3), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0, val3);
		Segments.EmoteSegment item10 = new Segments.EmoteSegment(77, num + num5 / 2, num5, sceneAnchorPosition + new Vector2(60f, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0, val3);
		int num12 = num5 + num5 / 2;
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(val3, num12));
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num5 / 2)).Then(new Actions.NPCs.Move(val3, num5));
		animationSegmentWithActions4.Then(new Actions.NPCs.Wait(num5 / 2 + 20)).Then(new Actions.NPCs.Move(val3, num5 - 20));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num12));
		num += num12;
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions4.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions4);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(item);
		_segments.Add(item2);
		_segments.Add(item3);
		_segments.Add(item4);
		_segments.Add(item5);
		_segments.Add(item6);
		_segments.Add(item7);
		_segments.Add(item8);
		_segments.Add(item10);
		_segments.Add(item9);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_TinkererAndMechanic(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		Asset<Texture2D> val = TextureAssets.Extra[237];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2(0f, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60));
		_segments.Add(animationSegmentWithActions);
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 107, sceneAnchorPosition, _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 60))
			.Then(new Actions.NPCs.Wait(60));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait((int)animationSegmentWithActions2.DedicatedTimeNeeded));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		int num2 = 24;
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions3 = new Segments.NPCSegment(num, 124, sceneAnchorPosition + new Vector2((float)(120 + num2), 0f), _originAtBottom).Then(new Actions.NPCs.LookAt(-1)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51))
			.Then(new Actions.NPCs.Move(new Vector2(-1f, 0f), 60));
		num += (int)animationSegmentWithActions3.DedicatedTimeNeeded;
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait((int)animationSegmentWithActions3.DedicatedTimeNeeded));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait((int)animationSegmentWithActions3.DedicatedTimeNeeded));
		int num3 = 120;
		Segments.EmoteSegment item = new Segments.EmoteSegment(0, num, num3, sceneAnchorPosition + new Vector2(60f, 0f) + _emoteBubbleOffsetWhenOnLeft, (SpriteEffects)1);
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(0, num, num3, sceneAnchorPosition + new Vector2((float)(60 + num2), 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num3));
		animationSegmentWithActions3.Then(new Actions.NPCs.Wait(num3));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num3));
		num += num3;
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions3.Then(new Actions.NPCs.Move(new Vector2(-0.5f, 0f), 120)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Fade(0f, 127));
		num += 187;
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(animationSegmentWithActions3);
		_segments.Add(item);
		_segments.Add(item2);
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	private SegmentInforReport PlaySegment_ClothierChasingTruffle(int startTime, Vector2 sceneAnchorPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		sceneAnchorPosition += GetSceneFixVector();
		int num = startTime;
		int num2 = 10;
		sceneAnchorPosition.X += num2;
		Asset<Texture2D> val = TextureAssets.Extra[225];
		Rectangle val2 = val.Frame();
		DrawData data = new DrawData(val.Value, Vector2.Zero, val2, Color.White, 0f, val2.Size() * new Vector2(0.5f, 1f) + new Vector2((float)num2, -42f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> animationSegmentWithActions = new Segments.SpriteSegment(val, num, data, sceneAnchorPosition + _backgroundOffset).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect()).Then(new Actions.Sprites.Fade(0f)).With(new Actions.Sprites.Fade(1f, 60));
		_segments.Add(animationSegmentWithActions);
		Segments.AnimationSegmentWithActions<NPC> animationSegmentWithActions2 = new Segments.NPCSegment(startTime, 160, sceneAnchorPosition + new Vector2(20f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.LookAt(1))
			.Then(new Actions.NPCs.Wait(60))
			.Then(new Actions.NPCs.LookAt(-1));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait((int)animationSegmentWithActions2.DedicatedTimeNeeded));
		num += (int)animationSegmentWithActions2.DedicatedTimeNeeded;
		int num3 = 60;
		Segments.EmoteSegment item = new Segments.EmoteSegment(10, num, num3, sceneAnchorPosition + new Vector2(20f, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num3));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num3));
		num += num3;
		Segments.EmoteSegment item2 = new Segments.EmoteSegment(3, num, num3, sceneAnchorPosition + new Vector2(20f, 0f) + _emoteBubbleOffsetWhenOnRight, (SpriteEffects)0);
		animationSegmentWithActions2.Then(new Actions.NPCs.Wait(num3));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(num3));
		num += num3;
		Vector2 val3 = new Vector2(1.2f, 0f);
		Vector2 val4 = new Vector2(1f, 0f);
		Segments.AnimationSegmentWithActions<NPC> item3 = new Segments.NPCSegment(num, 54, sceneAnchorPosition + new Vector2(-100f, 0f), _originAtBottom).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(val3, 60))
			.Then(new Actions.NPCs.Move(val3, 130))
			.With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions2.Then(new Actions.NPCs.Move(val4, 60)).Then(new Actions.NPCs.Move(val4, 130)).With(new Actions.NPCs.Fade(2, 127));
		animationSegmentWithActions.Then(new Actions.Sprites.Wait(60)).Then(new Actions.Sprites.Wait(130)).With(new Actions.Sprites.Fade(0f, 127));
		int num4 = 10;
		int num5 = 40;
		int timeToPlay = 70;
		Segments.EmoteSegment item4 = new Segments.EmoteSegment(134, num + num4, timeToPlay, sceneAnchorPosition + new Vector2(20f, 0f) + _emoteBubbleOffsetWhenOnLeft + val4 * (float)num4, (SpriteEffects)1, val4);
		Segments.EmoteSegment item5 = new Segments.EmoteSegment(15, num + num5, timeToPlay, sceneAnchorPosition + new Vector2(-100f, 0f) + _emoteBubbleOffsetWhenOnLeft + val3 * (float)num5, (SpriteEffects)1, val3);
		_segments.Add(item3);
		_segments.Add(animationSegmentWithActions2);
		_segments.Add(item);
		_segments.Add(item2);
		_segments.Add(item4);
		_segments.Add(item5);
		num += 200;
		return new SegmentInforReport
		{
			totalTime = num - startTime
		};
	}

	public CreditsRollComposer()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
	}
}
