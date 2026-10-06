using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Renderers;
using Terraria.ID;

namespace Terraria.GameContent;

public class CraftingEffects
{
	private static int _justCraftedItemType;

	private static float _mouseItemGlow;

	public static void OnCraft(Recipe recipe, bool quickCraft)
	{
		_justCraftedItemType = recipe.createItem.type;
		Item createItem = recipe.createItem;
		SpawnEffects_BeforeGrantingItem(recipe, createItem);
		if (!quickCraft)
		{
			_mouseItemGlow = 1f;
		}
	}

	public static void OnCraftItemGranted(Recipe recipe, Item result, bool quickCraft)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		PopupText.NewText(PopupTextContext.ItemCraft, result, Main.LocalPlayer.Center, recipe.createItem.stack);
		SpawnEffects_AfterGrantingItem(recipe, result, quickCraft);
	}

	public static void Update()
	{
		if (_mouseItemGlow > 0f)
		{
			_mouseItemGlow -= 0.035f;
		}
	}

	public static float GetGlow(Item cursorItem)
	{
		if (_mouseItemGlow <= 0f || _justCraftedItemType != cursorItem.type)
		{
			return 0f;
		}
		return _mouseItemGlow;
	}

	private static void SpawnEffects_BeforeGrantingItem(Recipe recipe, Item result)
	{
		SoundEngine.PlaySound(7);
	}

	public static void SpawnEffects_AfterGrantingItem(Recipe recipe, Item result, bool quickCraft)
	{
	}

	private static bool RecipeUsesCraftingStation(Recipe recipe, int tileId)
	{
		return recipe.requiredTile == tileId;
	}

	public static CraftingEffectDetails GetEffectDetails(Item newItem)
	{
		int rare = newItem.rare;
		CraftingEffectDetails result = new CraftingEffectDetails
		{
			Rarity = rare
		};
		if ((newItem.healLife > 0 || newItem.healMana > 0 || newItem.buffType > 0 || ItemID.Sets.IsFood[newItem.type] || ItemID.Sets.SortingPriorityPotionsBuffs[newItem.type] != -1) & newItem.consumable)
		{
			result.Style = PopupEffectStyle.Potion;
			result.Intensity = rare;
		}
		int num;
		if (newItem.GetRollablePrefixes() == null && !newItem.accessory && newItem.bodySlot == -1 && newItem.headSlot == -1 && newItem.legSlot == -1 && (newItem.shoot == 0 || !Main.projHook[newItem.shoot]))
		{
			num = ((newItem.mountType != -1) ? 1 : 0);
			if (num == 0)
			{
				goto IL_00d1;
			}
		}
		else
		{
			num = 1;
		}
		result.Style = PopupEffectStyle.Metal;
		result.Intensity = rare;
		goto IL_00d1;
		IL_00d1:
		if (num != 0 && newItem.magic)
		{
			result.Style = PopupEffectStyle.MagicWeapon;
			result.Intensity = rare;
		}
		if (num != 0 && newItem.melee)
		{
			result.Style = PopupEffectStyle.MeleeWeapon;
			result.Intensity = rare;
		}
		if (num != 0 && newItem.ranged)
		{
			result.Style = PopupEffectStyle.RangedWeapon;
			result.Intensity = rare;
		}
		return result;
	}

	private static void CreateBubbleParticles(int n)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		for (float num = 0f; num < 2f; num += 1f / 12f)
		{
			float num2 = 15f;
			float f = (float)Math.PI * 2f * (num + Main.rand.NextFloat());
			FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(TextureAssets.Bubble, null, f.ToRotationVector2() * (2f + 3f * Main.rand.NextFloat()), Main.MouseScreen + f.ToRotationVector2() * (10f + 40f * Main.rand.NextFloat()));
			fadingParticle.SetTypeInfo(num2);
			fadingParticle.AccelerationPerFrame = fadingParticle.Velocity * (-1f / num2);
			fadingParticle.LocalPosition -= fadingParticle.Velocity * 4f;
			fadingParticle.FadeInNormalizedTime = 0.2f;
			fadingParticle.FadeOutNormalizedTime = 0.7f;
			fadingParticle.Scale = Vector2.One;
			Main.ParticleSystem_OverInventory.Add(fadingParticle);
		}
	}
}
