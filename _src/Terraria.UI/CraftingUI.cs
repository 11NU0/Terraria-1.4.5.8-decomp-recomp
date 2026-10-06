using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI.Gamepad;

namespace Terraria.UI;

public class CraftingUI : ICraftingUI
{
	public static float[] availableRecipeY = new float[Recipe.maxRecipes];

	private static NewCraftingUI.RecipeFilter _lastFilter;

	private static float inventoryScale
	{
		get
		{
			return Main.inventoryScale;
		}
		set
		{
			Main.inventoryScale = value;
		}
	}

	private static int numAvailableRecipes => Main.numAvailableRecipes;

	private static int focusRecipe
	{
		get
		{
			return Main.focusRecipe;
		}
		set
		{
			Main.focusRecipe = value;
		}
	}

	private static int mouseX => Main.mouseX;

	private static int mouseY => Main.mouseY;

	private static Color inventoryBack
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return Main.inventoryBack;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			Main.inventoryBack = value;
		}
	}

	private static bool recFastScroll
	{
		get
		{
			return Main.PipsFastScroll;
		}
		set
		{
			Main.PipsFastScroll = value;
		}
	}

	private static int recStart
	{
		get
		{
			return Main.recStart;
		}
		set
		{
			Main.recStart = value;
		}
	}

	public static bool AnyAdvancedGridVisible => NewCraftingUI.Visible;

	public static string CraftingWindowTextKey
	{
		get
		{
			if (Player.Settings.CraftingGridControl != Player.Settings.CraftingGridMode.Classic)
			{
				return "GameUI.CraftingWindow";
			}
			return "GameUI.CraftingWindowClassic";
		}
	}

	public static string CraftingWindowTextTipKey
	{
		get
		{
			if (Player.Settings.CraftingGridControl != Player.Settings.CraftingGridMode.Classic)
			{
				return "GameUI.CraftingWindowTip";
			}
			return "GameUI.CraftingWindowClassicTip";
		}
	}

	public static NewCraftingUI.RecipeFilter RecipeFilterHack
	{
		get
		{
			if (!Main.playerInventory || Main.PipsCurrentPage != Main.PipPage.Recipes || Player.Settings.CraftingGridControl != Player.Settings.CraftingGridMode.Classic)
			{
				return null;
			}
			return _lastFilter;
		}
	}

	public CraftingUI()
	{
		for (int i = 0; i < availableRecipeY.Length; i++)
		{
			availableRecipeY[i] = 65 * i;
		}
	}

	public void VisuallyRepositionRecipes(int oldRecipe)
	{
		float num = availableRecipeY[Main.focusRecipe] - availableRecipeY[oldRecipe];
		for (int i = 0; i < availableRecipeY.Length; i++)
		{
			availableRecipeY[i] -= num;
		}
	}

	public static void ClearHacks()
	{
		_lastFilter = null;
	}

	public void OpenCloseFilter(NewCraftingUI.RecipeFilter filter)
	{
		if (Main.playerInventory && Main.PipsCurrentPage == Main.PipPage.Recipes && Main.PipsUseGrid)
		{
			_lastFilter = null;
			IngameUIWindows.CloseAll();
			return;
		}
		_lastFilter = filter;
		IngameUIWindows.CloseAll(quiet: true);
		Player.OpenInventory();
		Main.PipsUseGrid = true;
		Main.PipsCurrentPage = Main.PipPage.Recipes;
	}

	public void DrawRecipesList(SpriteBatch spriteBatch, int adjY, int middleY, Color craftingTipColor)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
		UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
		if (numAvailableRecipes > 0)
		{
			string text = Lang.inter[25].Value;
			if (RecipeFilterHack != null)
			{
				text = RecipeFilterHack.GetWindowDescription();
			}
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, new Vector2(76f, (float)(414 + adjY)), craftingTipColor, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		}
		AdjustRecipeOffsets();
		for (int i = 0; i < Recipe.maxRecipes; i++)
		{
			if (i >= numAvailableRecipes || Math.Abs(availableRecipeY[i]) > (float)middleY)
			{
				continue;
			}
			inventoryScale = 100f / (Math.Abs(availableRecipeY[i]) + 100f);
			if ((double)inventoryScale < 0.75)
			{
				inventoryScale = 0.75f;
			}
			if (recFastScroll)
			{
				inventoryScale = 0.75f;
			}
			GetItemSlotColors(middleY, 100f, i, out var inventoryAlpha, out var inventoryColor);
			int num = (int)(46f - 26f * inventoryScale);
			int num2 = (int)(410f + availableRecipeY[i] * inventoryScale - 30f * inventoryScale + (float)adjY);
			if (!Main.LocalPlayer.creativeInterface && mouseX >= num && (float)mouseX <= (float)num + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num2 && (float)mouseY <= (float)num2 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
			{
				Main.HoverOverCraftingItemButton(i);
			}
			if (numAvailableRecipes <= 0)
			{
				continue;
			}
			inventoryAlpha -= 50.0;
			if (inventoryAlpha < 0.0)
			{
				inventoryAlpha = 0.0;
			}
			if (i == focusRecipe)
			{
				UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = 0;
				if (PlayerInput.SettingsForUI.HighlightThingsForMouse)
				{
					ItemSlot.DrawGoldBGForCraftingMaterial = true;
				}
			}
			else
			{
				UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
			}
			Color val = inventoryBack;
			inventoryBack = new Color((int)(byte)inventoryAlpha, (int)(byte)inventoryAlpha, (int)(byte)inventoryAlpha, (int)(byte)inventoryAlpha);
			ItemSlot.Draw(spriteBatch, ref Main.recipe[Main.availableRecipe[i]].createItem, 22, new Vector2((float)num, (float)num2), inventoryColor);
			inventoryBack = val;
		}
		inventoryScale = 0.6f;
		if (numAvailableRecipes <= 0)
		{
			return;
		}
		UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
		UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
		for (int j = 0; j < Recipe.maxRequirements; j++)
		{
			Recipe recipe = Main.recipe[Main.availableRecipe[focusRecipe]];
			Item inv = recipe.requiredItem[j];
			if (inv.type == 0)
			{
				UILinkPointNavigator.Shortcuts.CRAFT_CurrentIngredientsCount = j + 1;
				break;
			}
			int num3 = 80 + j * 40;
			int num4 = 380 + adjY;
			Color lightColor = inventoryBack;
			double num5 = (float)(lightColor.A + 50) - Math.Abs(availableRecipeY[focusRecipe]) * 2f;
			if (num5 != 0.0)
			{
				if (mouseX >= num3 && (float)mouseX <= (float)num3 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num4 && (float)mouseY <= (float)num4 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
				{
					Main.craftingHide = true;
					Main.LocalPlayer.mouseInterface = true;
					ItemSlot.HoverOverrideClick(inv, 22);
					SetRecipeMaterialDisplayName(recipe, inv);
				}
				num5 -= 50.0;
				if (num5 < 0.0)
				{
					num5 = 0.0;
				}
				UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = 1 + j;
				Color val2 = inventoryBack;
				inventoryBack = new Color((int)(byte)num5, (int)(byte)num5, (int)(byte)num5, (int)(byte)num5);
				Vector2 position = new Vector2((float)num3, (float)num4);
				lightColor = default;
				ItemSlot.Draw(spriteBatch, ref inv, 22, position, lightColor);
				inventoryBack = val2;
				continue;
			}
			break;
		}
	}

	public static void DrawGridToggle(SpriteBatch spriteBatch, int craftX, int craftY, int gamepadPointId)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		if (_lastFilter != null && (!_lastFilter.CanRemainOpen() || Main.PipsCurrentPage != Main.PipPage.Recipes || !Main.playerInventory))
		{
			_lastFilter = null;
		}
		UILinkPointNavigator.SetPosition(gamepadPointId, new Vector2((float)craftX, (float)craftY));
		bool flag = mouseX > craftX - 15 && mouseX < craftX + 15 && mouseY > craftY - 15 && mouseY < craftY + 15 && !PlayerInput.IgnoreMouseInterface;
		if (Main.PipsCurrentPage == Main.PipPage.Recipes)
		{
			Utils.DrawSelectedCraftingBarIndicator(spriteBatch, craftX, craftY);
		}
		bool flag2 = Player.Settings.CraftingGridControl == Player.Settings.CraftingGridMode.Classic;
		int num = 2;
		if (NewCraftingUI.Visible)
		{
			num = 4;
		}
		if (Main.PipsCurrentPage == Main.PipPage.Recipes && Main.PipsUseGrid)
		{
			num = 0;
		}
		num += flag.ToInt();
		spriteBatch.Draw(TextureAssets.CraftToggle[num].Value, new Vector2((float)craftX, (float)craftY), (Rectangle?)null, Color.White, 0f, TextureAssets.CraftToggle[num].Value.Size() / 2f, 1f, (SpriteEffects)0, 0f);
		if (flag)
		{
			Main.instance.MouseTextNoOverride(Language.GetTextValue(CraftingWindowTextTipKey), 0, 0);
			Main.player[Main.myPlayer].mouseInterface = true;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				if (!Main.TryChangePipsPage(Main.PipPage.Recipes))
				{
					if (flag2)
					{
						NewCraftingUI.Close(quiet: true, returnToInventory: true);
						Main.PipsUseGrid = !Main.PipsUseGrid;
					}
					else
					{
						Main.PipsUseGrid = false;
						bool anyAdvancedGridVisible = AnyAdvancedGridVisible;
						NewCraftingUI.ToggleInInventory();
						if (anyAdvancedGridVisible)
						{
							UILinkPointNavigator.ChangePoint(11001);
						}
					}
					Main.mouseLeftRelease = false;
				}
				SoundEngine.PlaySound(12);
			}
			if (Main.mouseRight && Main.mouseRightRelease)
			{
				Main.mouseRightRelease = false;
				SoundEngine.PlaySound(12);
				switch (Player.Settings.CraftingGridControl)
				{
				case Player.Settings.CraftingGridMode.Classic:
					Player.Settings.CraftingGridControl = Player.Settings.CraftingGridMode.Modern;
					break;
				case Player.Settings.CraftingGridMode.Modern:
					Player.Settings.CraftingGridControl = Player.Settings.CraftingGridMode.Classic;
					break;
				}
			}
		}
		Main.DoStatefulTickSound(ref Main.GridToggleMouseOverCrafting, flag);
	}

	public static void DrawCraftFromNearbyChestsToggle(SpriteBatch spriteBatch, int toggleNearbyX, int toggleNearbyY, int gamepadPointId)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.SetPosition(gamepadPointId, new Vector2((float)toggleNearbyX, (float)toggleNearbyY));
		bool flag = mouseX > toggleNearbyX - 15 && mouseX < toggleNearbyX + 15 && mouseY > toggleNearbyY - 15 && mouseY < toggleNearbyY + 15 && !PlayerInput.IgnoreMouseInterface;
		int num = 2 - Player.Settings.CraftFromNearbyChests.ToInt() * 2 + flag.ToInt();
		int num2 = 1;
		spriteBatch.Draw(TextureAssets.ChestCraft[num].Value, new Vector2((float)toggleNearbyX, (float)toggleNearbyY), (Rectangle?)null, Color.White, 0f, TextureAssets.ChestCraft[num].Value.Size() / 2f, (float)num2, (SpriteEffects)0, 0f);
		if (flag)
		{
			Main.instance.MouseTextNoOverride(Language.GetTextValue(Player.Settings.CraftFromNearbyChests ? "GameUI.CraftFromNearbyChestsOn" : "GameUI.CraftFromNearbyChestsOff"), 0, 0);
			Main.player[Main.myPlayer].mouseInterface = true;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				Player.Settings.CraftFromNearbyChests = !Player.Settings.CraftFromNearbyChests;
				NewCraftingUI.RefreshGrid();
				SoundEngine.PlaySound(12);
				Main.SaveSettings();
			}
		}
		Main.DoStatefulTickSound(ref Main.nearbyCraftingMouseOver, flag);
	}

	private void GetItemSlotColors(int middleY, float fadeInValue, int recipeIndex, out double inventoryAlpha, out Color inventoryColor2)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		Color val = inventoryBack;
		inventoryAlpha = val.A + 50;
		double num = 255.0;
		if (Math.Abs(availableRecipeY[recipeIndex]) > (float)middleY - fadeInValue)
		{
			inventoryAlpha = (double)(150f * (fadeInValue - (Math.Abs(availableRecipeY[recipeIndex]) - ((float)middleY - fadeInValue)))) * 0.01;
			num = (double)(255f * (fadeInValue - (Math.Abs(availableRecipeY[recipeIndex]) - ((float)middleY - fadeInValue)))) * 0.01;
		}
		new Color((int)(byte)inventoryAlpha, (int)(byte)inventoryAlpha, (int)(byte)inventoryAlpha, (int)(byte)inventoryAlpha);
		inventoryColor2 = new Color((int)(byte)num, (int)(byte)num, (int)(byte)num, (int)(byte)num);
	}

	private void AdjustRecipeOffsets()
	{
		DrawRecipes_AdjustRecipeOffsetSnappy();
	}

	private void DrawRecipes_AdjustRecipeOffsetSnappy()
	{
		int num = 65;
		float amount = (float)num / 10f;
		float num2 = availableRecipeY[focusRecipe];
		float original = num2 * 0.97f;
		original = Utils.MoveTowards(original, 0f, amount);
		if (recFastScroll)
		{
			original = 0f;
		}
		availableRecipeY[focusRecipe] = original;
		int num3 = (int)(num2 / (float)num);
		int num4 = (int)(original / (float)num);
		if (num3 != num4)
		{
			SoundEngine.PlaySound(12);
		}
		for (int i = 0; i < numAvailableRecipes; i++)
		{
			_ = availableRecipeY[i];
			int num5 = (i - focusRecipe) * num;
			availableRecipeY[i] = original + (float)num5;
		}
		if (num2 == 0f)
		{
			recFastScroll = false;
		}
	}

	private void DrawRecipes_AdjustRecipeOffset(int recipeIndex)
	{
		int num = 65;
		float amount = (float)num / 10f;
		int num2 = (recipeIndex - focusRecipe) * num;
		if (availableRecipeY[recipeIndex] == (float)num2)
		{
			recFastScroll = false;
			return;
		}
		if (availableRecipeY[recipeIndex] == 0f && !recFastScroll)
		{
			SoundEngine.PlaySound(12);
		}
		if (recFastScroll)
		{
			availableRecipeY[recipeIndex] = num2;
		}
		else
		{
			availableRecipeY[recipeIndex] = Utils.MoveTowards(availableRecipeY[recipeIndex], num2, amount);
		}
	}

	public static void SetRecipeMaterialDisplayName(Recipe recipe, Item material)
	{
		Item item = material.Clone();
		ItemSlot.MouseHover(item, 22);
		item = Main.HoverItem;
		if (recipe.ProcessGroupsForText(material.type, out var theText))
		{
			item.SetNameOverride(theText);
		}
		Main.hoverItemName = item.Name;
		if (material.stack > 1)
		{
			Main.hoverItemName = Main.hoverItemName + " (" + material.stack + ")";
		}
	}

	public void DrawRecipesGrid(SpriteBatch spriteBatch)
	{
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
		UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
		int num = 42;
		inventoryScale = 0.75f;
		int num2 = 340;
		int num3 = 310;
		int num4 = (Main.screenWidth - num3 - 280) / num;
		int num5 = (Main.screenHeight - num2 - 20) / num;
		UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow = num4;
		UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn = num5;
		int num6 = 0;
		int num7 = 0;
		int num8 = num3;
		int num9 = num2;
		int num10 = num3 - 20;
		int num11 = num2 + 2;
		if (recStart > numAvailableRecipes - num4 * num5)
		{
			recStart = numAvailableRecipes - num4 * num5;
			if (recStart < 0)
			{
				recStart = 0;
			}
		}
		if (recStart > 0)
		{
			if (mouseX >= num10 && mouseX <= num10 + TextureAssets.CraftUpButton.Width() && mouseY >= num11 && mouseY <= num11 + TextureAssets.CraftUpButton.Height() && !PlayerInput.IgnoreMouseInterface)
			{
				Main.LocalPlayer.mouseInterface = true;
				if (Main.mouseLeftRelease && Main.mouseLeft)
				{
					recStart -= num4;
					if (recStart < 0)
					{
						recStart = 0;
					}
					SoundEngine.PlaySound(12);
					Main.mouseLeftRelease = false;
				}
			}
			spriteBatch.Draw(TextureAssets.CraftUpButton.Value, new Vector2((float)num10, (float)num11), (Rectangle?)null, new Color(200, 200, 200, 200), 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		}
		if (recStart < numAvailableRecipes - num4 * num5)
		{
			num11 += 20;
			if (mouseX >= num10 && mouseX <= num10 + TextureAssets.CraftUpButton.Width() && mouseY >= num11 && mouseY <= num11 + TextureAssets.CraftUpButton.Height() && !PlayerInput.IgnoreMouseInterface)
			{
				Main.LocalPlayer.mouseInterface = true;
				if (Main.mouseLeftRelease && Main.mouseLeft)
				{
					recStart += num4;
					SoundEngine.PlaySound(12);
					if (recStart > numAvailableRecipes - num4)
					{
						recStart = numAvailableRecipes - num4;
					}
					Main.mouseLeftRelease = false;
				}
			}
			spriteBatch.Draw(TextureAssets.CraftDownButton.Value, new Vector2((float)num10, (float)num11), (Rectangle?)null, new Color(200, 200, 200, 200), 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		}
		for (int i = recStart; i < Recipe.maxRecipes && i < numAvailableRecipes; i++)
		{
			int num12 = num8;
			int num13 = num9;
			Color lightColor = inventoryBack;
			double num14 = lightColor.A + 50;
			double num15 = 255.0;
			new Color((int)(byte)num14, (int)(byte)num14, (int)(byte)num14, (int)(byte)num14);
			new Color((int)(byte)num15, (int)(byte)num15, (int)(byte)num15, (int)(byte)num15);
			if (mouseX >= num12 && (float)mouseX <= (float)num12 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num13 && (float)mouseY <= (float)num13 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
			{
				Main.LocalPlayer.mouseInterface = true;
				if (Main.mouseLeftRelease && Main.mouseLeft)
				{
					focusRecipe = i;
					recFastScroll = true;
					Main.PipsUseGrid = false;
					SoundEngine.PlaySound(12);
					Main.mouseLeftRelease = false;
					if (PlayerInput.UsingGamepadUI)
					{
						UILinkPointNavigator.ChangePage(9);
						Main.LockCraftingForThisCraftClickDuration();
					}
				}
				Main.craftingHide = true;
				Item createItem = Main.recipe[Main.availableRecipe[i]].createItem;
				Main.HoverItem = createItem.Clone();
				ItemSlot.MouseHover(22);
				Main.hoverItemName = createItem.Name;
				if (createItem.stack > 1)
				{
					Main.hoverItemName = Main.hoverItemName + " (" + createItem.stack + ")";
				}
			}
			if (numAvailableRecipes > 0)
			{
				num14 -= 50.0;
				if (num14 < 0.0)
				{
					num14 = 0.0;
				}
				UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = i - recStart;
				Color val = inventoryBack;
				inventoryBack = new Color((int)(byte)num14, (int)(byte)num14, (int)(byte)num14, (int)(byte)num14);
				ref Item createItem2 = ref Main.recipe[Main.availableRecipe[i]].createItem;
				Vector2 position = new Vector2((float)num12, (float)num13);
				lightColor = default;
				ItemSlot.Draw(spriteBatch, ref createItem2, 22, position, lightColor);
				inventoryBack = val;
			}
			num8 += num;
			num6++;
			if (num6 >= num4)
			{
				num8 = num3;
				num9 += num;
				num6 = 0;
				num7++;
				if (num7 >= num5)
				{
					break;
				}
			}
		}
	}

	public void ScrollCraftingList(int mouseWheel)
	{
		focusRecipe += mouseWheel;
		if (focusRecipe > numAvailableRecipes - 1)
		{
			focusRecipe = numAvailableRecipes - 1;
		}
		if (focusRecipe < 0)
		{
			focusRecipe = 0;
		}
	}

	public void ScrollCraftingGrid(int mouseWheel, int width)
	{
		if (mouseWheel < 0)
		{
			recStart -= width;
			if (recStart < 0)
			{
				recStart = 0;
			}
			return;
		}
		recStart += width;
		SoundEngine.PlaySound(12);
		if (recStart > numAvailableRecipes - width)
		{
			recStart = numAvailableRecipes - width;
		}
	}
}
