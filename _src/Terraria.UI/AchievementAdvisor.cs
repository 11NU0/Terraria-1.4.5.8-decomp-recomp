using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameInput;

namespace Terraria.UI;

public class AchievementAdvisor
{
	private List<AchievementAdvisorCard> _cards = new List<AchievementAdvisorCard>();

	private Asset<Texture2D> _achievementsTexture;

	private Asset<Texture2D> _achievementsBorderTexture;

	private Asset<Texture2D> _achievementsBorderMouseHoverFatTexture;

	private Asset<Texture2D> _achievementsBorderMouseHoverThinTexture;

	private AchievementAdvisorCard _hoveredCard;

	public bool CanDrawAboveCoins
	{
		get
		{
			if (Main.screenWidth >= 1000 && !PlayerInput.UsingGamepad)
			{
				return !PlayerInput.SteamDeckIsUsed;
			}
			return false;
		}
	}

	public void LoadContent()
	{
		_achievementsTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievements", (AssetRequestMode)1);
		_achievementsBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", (AssetRequestMode)1);
		_achievementsBorderMouseHoverFatTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders_MouseHover", (AssetRequestMode)1);
		_achievementsBorderMouseHoverThinTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders_MouseHoverThin", (AssetRequestMode)1);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
	}

	public void DrawOneAchievement(SpriteBatch spriteBatch, Vector2 position, bool large)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		List<AchievementAdvisorCard> bestCards = GetBestCards(1);
		if (bestCards.Count < 1)
		{
			return;
		}
		AchievementAdvisorCard hoveredCard = bestCards[0];
		float num = 0.35f;
		if (large)
		{
			num = 0.75f;
		}
		_hoveredCard = null;
		DrawCard(bestCards[0], spriteBatch, position + new Vector2(8f) * num, num, out var hovered);
		if (hovered)
		{
			_hoveredCard = hoveredCard;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				Main.ingameOptionsWindow = false;
				IngameFancyUI.OpenAchievementsAndGoto(_hoveredCard.achievement);
				SoundEngine.PlaySound(12);
			}
		}
		Main.DoStatefulTickSound(ref Main.achievementAdvisorMouseOver, hovered);
	}

	public void Update()
	{
		_hoveredCard = null;
	}

	public void DrawOptionsPanel(SpriteBatch spriteBatch, Vector2 leftPosition, Vector2 rightPosition)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		List<AchievementAdvisorCard> bestCards = GetBestCards();
		_hoveredCard = null;
		int num = bestCards.Count;
		if (num > 5)
		{
			num = 5;
		}
		bool hovered;
		for (int i = 0; i < num; i++)
		{
			DrawCard(bestCards[i], spriteBatch, leftPosition + new Vector2((float)(42 * i), 0f), 0.5f, out hovered);
			if (hovered)
			{
				_hoveredCard = bestCards[i];
			}
		}
		for (int j = 5; j < bestCards.Count; j++)
		{
			DrawCard(bestCards[j], spriteBatch, rightPosition + new Vector2((float)(42 * j), 0f), 0.5f, out hovered);
			if (hovered)
			{
				_hoveredCard = bestCards[j];
			}
		}
		if (_hoveredCard != null)
		{
			if (_hoveredCard.achievement.IsCompleted)
			{
				_hoveredCard = null;
			}
			else if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				Main.ingameOptionsWindow = false;
				IngameFancyUI.OpenAchievementsAndGoto(_hoveredCard.achievement);
			}
		}
	}

	public void DrawMouseHover()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (_hoveredCard != null)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Main.UIScaleMatrix);
			PlayerInput.SetZoom_UI();
			Item item = Main.DisplayAndGetFakeItem(ItemRarityColor.StrongRed10);
			item.SetNameOverride(_hoveredCard.achievement.FriendlyName.Value);
			item.ToolTip = ItemTooltip.FromLanguageKey(_hoveredCard.achievement.Description.Key);
		}
	}

	private void DrawCard(AchievementAdvisorCard card, SpriteBatch spriteBatch, Vector2 position, float scale, out bool hovered)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		hovered = false;
		if (Main.MouseScreen.Between(position, position + card.frame.Size() * scale) && !PlayerInput.IgnoreMouseInterface)
		{
			Main.LocalPlayer.mouseInterface = true;
			hovered = true;
		}
		Color val = Color.White;
		if (!hovered)
		{
			val = new Color(220, 220, 220, 220);
		}
		Vector2 val2 = new Vector2(-4f) * scale;
		Vector2 val3 = new Vector2(-8f) * scale;
		Texture2D value = _achievementsBorderMouseHoverFatTexture.Value;
		if (scale > 0.5f)
		{
			value = _achievementsBorderMouseHoverThinTexture.Value;
			val3 = new Vector2(-5f) * scale;
		}
		Rectangle frame = card.frame;
		frame.X += 528;
		spriteBatch.Draw(_achievementsTexture.Value, position, (Rectangle?)frame, val, 0f, Vector2.Zero, scale, (SpriteEffects)0, 0f);
		spriteBatch.Draw(_achievementsBorderTexture.Value, position + val2, (Rectangle?)null, val, 0f, Vector2.Zero, scale, (SpriteEffects)0, 0f);
		if (hovered)
		{
			spriteBatch.Draw(value, position + val3, (Rectangle?)null, Main.OurFavoriteColor, 0f, Vector2.Zero, scale, (SpriteEffects)0, 0f);
		}
	}

	private List<AchievementAdvisorCard> GetBestCards(int cardsAmount = 10)
	{
		List<AchievementAdvisorCard> list = new List<AchievementAdvisorCard>();
		for (int i = 0; i < _cards.Count; i++)
		{
			AchievementAdvisorCard achievementAdvisorCard = _cards[i];
			if (!achievementAdvisorCard.achievement.IsCompleted && achievementAdvisorCard.IsAchievableInWorld())
			{
				list.Add(achievementAdvisorCard);
				if (list.Count >= cardsAmount)
				{
					break;
				}
			}
		}
		return list;
	}

	public bool HasAvailableCardToDisplay()
	{
		for (int i = 0; i < _cards.Count; i++)
		{
			AchievementAdvisorCard achievementAdvisorCard = _cards[i];
			if (achievementAdvisorCard.IsAchievableInWorld() && !achievementAdvisorCard.achievement.IsCompleted)
			{
				return true;
			}
		}
		return false;
	}

	public void Initialize()
	{
		float num = 1f;
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("TIMBER"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("BENCHED"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("OBTAIN_HAMMER"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("NO_HOBO"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("YOU_CAN_DO_IT"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("OOO_SHINY"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("HEAVY_METAL"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("MATCHING_ATTIRE"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("HEART_BREAKER"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("I_AM_LOOT"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("HOLD_ON_TIGHT"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("STAR_POWER"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("EYE_ON_YOU"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("SMASHING_POPPET"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("WHERES_MY_HONEY"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("STING_OPERATION"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("BONED"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("DUNGEON_HEIST"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("ITS_GETTING_HOT_IN_HERE"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("MINER_FOR_FIRE"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("STILL_HUNGRY"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("ITS_HARD"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("BEGONE_EVIL"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("EXTRA_SHINY"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("HEAD_IN_THE_CLOUDS"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("BUCKETS_OF_BOLTS"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("DRAX_ATTAX"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("PHOTOSYNTHESIS"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("GET_A_LIFE"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("THE_GREAT_SOUTHERN_PLANTKILL"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("TEMPLE_RAIDER"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("LIHZAHRDIAN_IDOL"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("ROBBING_THE_GRAVE"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("OBSESSIVE_DEVOTION"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("STAR_DESTROYER"), num++));
		_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("CHAMPION_OF_TERRARIA"), num++));
		_cards.OrderBy((AchievementAdvisorCard x) => x.order);
	}
}
