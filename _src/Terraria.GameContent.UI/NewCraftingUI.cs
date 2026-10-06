using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI;

public class NewCraftingUI : UIState
{
	private class RecipeEntry
	{
		public readonly int index;

		public int availableIndex = -1;

		public int gridIndex = -1;

		public bool Available => availableIndex >= 0;

		public Recipe Recipe => Main.recipe[index];

		public RecipeEntry(int index)
		{
			this.index = index;
		}
	}

	private class ItemGrid : UIDynamicItemCollection<RecipeEntry>
	{
		private readonly NewCraftingUI parent;

		public ItemGrid(NewCraftingUI parent)
		{
			this.parent = parent;
		}

		protected override Item GetItem(RecipeEntry entry)
		{
			return entry.Recipe.createItem;
		}

		protected override void DrawSlot(SpriteBatch spriteBatch, RecipeEntry entry, Vector2 pos, bool hovering)
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			Item inv = entry.Recipe.createItem;
			int context = 41;
			if (PlayerInput.UsingGamepad && IsGridPoint(UILinkPointNavigator.CurrentPoint))
			{
				ItemSlot.DrawSelectionHighlightForGridSlot = false;
				UILinkPointNavigator.Shortcuts.ItemSlotShouldHighlightAsSelected = hovering;
			}
			else
			{
				ItemSlot.DrawSelectionHighlightForGridSlot = entry.index == parent._selectedRecipeIndex;
				UILinkPointNavigator.Shortcuts.ItemSlotShouldHighlightAsSelected = false;
			}
			if (hovering)
			{
				parent._hoveredEntry = entry;
				parent.HandleCraftSlot(entry, 41);
			}
			ItemSlot.Draw(spriteBatch, ref inv, context, pos, entry.Available ? Color.White : DisabledSlotColor);
		}
	}

	public interface RecipeFilter
	{
		string GetWindowDescription();

		bool Accepts(Recipe recipe);

		bool CanRemainOpen();

		bool Matches(RecipeFilter other);
	}

	public abstract class TileBasedRecipeFilter : RecipeFilter
	{
		public readonly int tileType;

		public readonly int tileStyle;

		public TileBasedRecipeFilter(int tileType, int tileStyle)
		{
			this.tileType = tileType;
			this.tileStyle = tileStyle;
		}

		public string GetWindowDescription()
		{
			string mapObjectName = Lang.GetMapObjectName(MapHelper.TileToLookup(tileType, tileStyle));
			return Language.GetTextValue("CombineFormat.Crafting", mapObjectName);
		}

		public abstract bool Accepts(Recipe recipe);

		public bool CanRemainOpen()
		{
			return Main.LocalPlayer.adjTile[tileType];
		}

		public bool Matches(RecipeFilter other)
		{
			if (other is TileBasedRecipeFilter)
			{
				return Matches(this, (TileBasedRecipeFilter)other);
			}
			return false;
		}

		private static bool Matches(TileBasedRecipeFilter a, TileBasedRecipeFilter b)
		{
			if (a.tileType == b.tileType)
			{
				return a.tileStyle == b.tileStyle;
			}
			return false;
		}
	}

	public class CraftStationRecipeFilter : TileBasedRecipeFilter
	{
		private bool[] acceptTileTypes;

		public CraftStationRecipeFilter(int tileType, int tileStyle)
			: base(tileType, tileStyle)
		{
			acceptTileTypes = new bool[TileID.Count];
			AcceptTileType(tileType);
		}

		private void AcceptTileType(int tileType)
		{
			acceptTileTypes[tileType] = true;
			List<int> list = Recipe.TileCountsAs[tileType];
			if (list == null)
			{
				return;
			}
			foreach (int item in list)
			{
				AcceptTileType(item);
			}
		}

		public override bool Accepts(Recipe recipe)
		{
			if (!recipe.DoesNotNeedTileOrLiquid)
			{
				if (recipe.requiredTile >= 0)
				{
					return acceptTileTypes[recipe.requiredTile];
				}
				return false;
			}
			return true;
		}
	}

	public class WaterSourceRecipeFilter : TileBasedRecipeFilter
	{
		public WaterSourceRecipeFilter(int tileType, int tileStyle)
			: base(tileType, tileStyle)
		{
		}

		public override bool Accepts(Recipe recipe)
		{
			if (!recipe.DoesNotNeedTileOrLiquid)
			{
				return recipe.needWater;
			}
			return true;
		}
	}

	private static UserInterface _ui = new UserInterface();

	private static NewCraftingUI _instance;

	private bool _openedWithoutFilter;

	private RecipeFilter _filter;

	private int? _selectedRecipeIndex;

	private RecipeEntry _hoveredEntry;

	private string _missingRequirementsTooltipText;

	private ItemGrid _itemGrid;

	private UIText _text;

	private UIWrappedSearchBar _searchBar;

	private UIElement _gridContainer;

	private bool _gamepadMoveToSearchButtonHack;

	private bool _gamepadMoveToGridEntryHack;

	private bool _gamepadReturnToGridEntry;

	private EntryFilterer<Item, IItemEntryFilter> _filterer;

	public const string SnapPointName_Search = "NewCraftingUISearch";

	public const string SnapPointName_SearchCancel = "NewCraftingUISearchCancel";

	public const string SnapPointName_Filters = "NewCraftingUIFilters";

	private static List<string> _missingObjects = new List<string>();

	private static readonly Color DisabledSlotColor = new Color(160, 160, 160, 255);

	private List<RecipeEntry> _recipes = new List<RecipeEntry>(Recipe.maxRecipes);

	private List<RecipeEntry> _filteredRecipes = new List<RecipeEntry>(Recipe.maxRecipes);

	private RecipeEntry[] _recipeListLookup;

	private Item _resetForGuideItem;

	private UIGamepadHelper _helper;

	public static bool Visible => _ui.CurrentState != null;

	private RecipeEntry SelectedEntry
	{
		get
		{
			if (!_selectedRecipeIndex.HasValue)
			{
				return null;
			}
			return _recipeListLookup[_selectedRecipeIndex.Value];
		}
	}

	private int GetNumRecipes()
	{
		int result = Main.numAvailableRecipes;
		if (Main.InGuideCraftMenu && Main.guideItem.IsAir)
		{
			result = 0;
		}
		return result;
	}

	public NewCraftingUI()
	{
		UILinkPage page = UILinkPointNavigator.Pages[24];
		page.OnSpecialInteracts += GetGamepadInstructions;
		page.UpdateEvent += () =>
		{
			PlayerInput.GamepadAllowScrolling = true;
		};
		page.EnterEvent += () =>
		{
			if (GetNumRecipes() == 0)
			{
				page.CurrentPoint = 20030;
			}
			else
			{
				page.CurrentPoint = (Main.InGuideCraftMenu ? 20020 : 20000);
			}
		};
		_filterer = new EntryFilterer<Item, IItemEntryFilter>();
		List<IItemEntryFilter> list = new List<IItemEntryFilter>
		{
			new ItemFilters.Weapon(),
			new ItemFilters.Armor(),
			new ItemFilters.Vanity(),
			new ItemFilters.BuildingBlock(),
			new ItemFilters.Furniture(),
			new ItemFilters.Accessories(),
			new ItemFilters.MiscAccessories(),
			new ItemFilters.Consumables(),
			new ItemFilters.Tools(),
			new ItemFilters.Materials()
		};
		List<IItemEntryFilter> list2 = new List<IItemEntryFilter>();
		list2.AddRange(list);
		list2.Add(new ItemFilters.MiscFallback(list));
		_filterer.AddFilters(list2);
		_filterer.SetSearchFilterObject(new ItemFilters.BySearch());
		HAlign = 0f;
		VAlign = 0f;
		Left = new StyleDimension(20f, 0f);
		Top = new StyleDimension(312f, 0f);
		Width = new StyleDimension(490f, 0f);
		Height = new StyleDimension(-350f, 1f);
		SetPadding(0f);
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.Fill,
			Height = StyleDimension.Fill
		};
		uIElement.SetPadding(0f);
		BuildInfinitesMenuContents(uIElement);
		Append(uIElement);
	}

	private void BuildInfinitesMenuContents(UIElement totalContainer)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		UIPanel uIPanel = new UIPanel
		{
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(-38f, 1f),
			Top = new StyleDimension(38f, 0f),
			PaddingRight = 8f
		};
		uIPanel.BackgroundColor = Utils.ShiftBlueToCyanTheme(uIPanel.BackgroundColor);
		uIPanel.BorderColor = Utils.ShiftBlueToCyanTheme(uIPanel.BorderColor);
		uIPanel.BackgroundColor *= 0.8f;
		uIPanel.BorderColor *= 0.8f;
		totalContainer.Append(uIPanel);
		UIText uIText = new UIText("")
		{
			Left = new StyleDimension(-1f, 0f),
			Top = new StyleDimension(-2f, 0f)
		};
		uIPanel.Append(uIText);
		_text = uIText;
		UIWrappedSearchBar uIWrappedSearchBar = new UIWrappedSearchBar(GoBackFromVirtualKeyboard, null, UIWrappedSearchBar.ColorTheme.Red)
		{
			Top = new StyleDimension(-4f, 0f),
			HAlign = 1f
		};
		uIWrappedSearchBar.CustomOpenVirtualKeyboard = (UIState state) =>
		{
			IngameFancyUI.OpenUIState(state, closeIngameWindows: false);
		};
		uIWrappedSearchBar.OnSearchContentsChanged += OnSearchContentsChanged;
		uIWrappedSearchBar.SetSearchSnapPoint("NewCraftingUISearch", 0);
		uIWrappedSearchBar.SetSearchCancelSnapPoint("NewCraftingUISearchCancel", 0);
		uIPanel.Append(uIWrappedSearchBar);
		_searchBar = uIWrappedSearchBar;
		UIElement uIElement = (_gridContainer = new UIElement
		{
			Width = StyleDimension.Fill,
			Height = StyleDimension.Fill,
			VAlign = 1f
		});
		uIPanel.Append(uIElement);
		UIHorizontalSeparator uIHorizontalSeparator = new UIHorizontalSeparator
		{
			Width = new StyleDimension(-8f, 1f),
			HAlign = 0.5f,
			Color = new Color(89, 116, 213, 255) * 0.9f
		};
		uIHorizontalSeparator.Color = Utils.ShiftBlueToCyanTheme(uIHorizontalSeparator.Color);
		uIElement.Append(uIHorizontalSeparator);
		UIList uIList = new UIList
		{
			Width = new StyleDimension(-20f, 1f),
			Height = new StyleDimension(-7f, 1f),
			VAlign = 1f,
			HAlign = 0f
		};
		uIElement.Append(uIList);
		float num = 4f;
		UIScrollbar uIScrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Cyan)
		{
			AutoHide = true,
			Height = new StyleDimension((0f - num) * 2f - 11f, 1f),
			Top = new StyleDimension(0f - num, 0f),
			VAlign = 1f,
			HAlign = 1f
		};
		uIElement.Append(uIScrollbar);
		uIList.SetScrollbar(uIScrollbar);
		uIList.Add(_itemGrid = new ItemGrid(this));
		UICreativeItemsInfiniteFilteringOptions uICreativeItemsInfiniteFilteringOptions = new UICreativeItemsInfiniteFilteringOptions(_filterer, "NewCraftingUIFilters", UICreativeItemsInfiniteFilteringOptions.ColorTheme.Cyan)
		{
			HAlign = 0.5f
		};
		uICreativeItemsInfiniteFilteringOptions.OnClickingOption += ResetRecipes;
		totalContainer.Append(uICreativeItemsInfiniteFilteringOptions);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (IsMouseHovering)
		{
			Main.LocalPlayer.mouseInterface = true;
		}
		UpdateCraftAreaSize();
		UpdateText();
		UpdateContents();
		base.Draw(spriteBatch);
	}

	private void UpdateCraftAreaSize()
	{
		int num = (Main.InGuideCraftMenu ? 130 : 77);
		if (_gridContainer.Height.Pixels != (float)(-num))
		{
			_gridContainer.Height.Pixels = -num;
			_gridContainer.Recalculate();
		}
	}

	private void UpdateText()
	{
		string text = ((_filter != null) ? _filter.GetWindowDescription() : Lang.inter[25].Value);
		if (text != _text.Text)
		{
			_text.SetText(text);
			_text.Recalculate();
			_searchBar.Width = new StyleDimension(0f - _text.GetOuterDimensions().Width - 10f, 1f);
			_searchBar.Recalculate();
		}
	}

	protected override void DrawChildren(SpriteBatch spriteBatch)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		_hoveredEntry = null;
		_missingRequirementsTooltipText = null;
		base.DrawChildren(spriteBatch);
		if (PlayerInput.UsingGamepad && _hoveredEntry != null)
		{
			_selectedRecipeIndex = _hoveredEntry.index;
		}
		Vector2 val = GetInnerDimensions().ToRectangle().TopLeft() + new Vector2(24f, 73f);
		if (Main.InGuideCraftMenu)
		{
			if (DrawRecipeSlot(spriteBatch, Main.guideItem, 7, val + new Vector2(0f, 58f), enabled: true, 1f))
			{
				ItemSlot.Handle(ref Main.guideItem, 7);
			}
			string text = (Main.guideItem.IsAir ? Lang.inter[24].Value : (Lang.inter[21].Value + " " + Main.guideItem.Name));
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, val + new Vector2(52f, 73f), Color.White, 0f, Vector2.Zero, Vector2.One, -1f, 1f);
		}
		int i = 0;
		RecipeEntry recipeEntry = _hoveredEntry ?? SelectedEntry;
		if (recipeEntry != null)
		{
			Recipe recipe = recipeEntry.Recipe;
			if (Main.InGuideCraftMenu)
			{
				string recipeRequirementsText = Main.GetRecipeRequirementsText(recipe, explicitNone: false);
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, recipeRequirementsText, val + new Vector2(52f, 36f), Color.White, 0f, Vector2.Zero, Vector2.One, -1f, 1f);
			}
			if (DrawRecipeSlot(spriteBatch, recipe.createItem, 42, val, recipeEntry.Available, 1f))
			{
				HandleCraftSlot(recipeEntry, 42);
			}
			spriteBatch.Draw(Main.Assets.Request<Texture2D>("Images/UI/Craft", (AssetRequestMode)1).Value, val + new Vector2(47f, 13f), (Rectangle?)null, Color.White, 0f, Vector2.Zero, 0.8f, (SpriteEffects)0, 0f);
			for (; i < Recipe.maxRequirements && !recipe.requiredItem[i].IsAir; i++)
			{
			}
			val += new Vector2(72f, 22f);
			float num = Math.Min(11f / (float)i, 1f);
			for (int j = 0; j < i; j++)
			{
				Item item = recipe.requiredItem[j];
				int availableItemCount = Recipe.GetAvailableItemCount(recipe.requiredItemQuickLookup[j]);
				bool flag = Main.InGuideCraftMenu || availableItemCount >= item.stack;
				Vector2 val2 = val + new Vector2((float)(j * 34), -16f) * num;
				UILinkPointNavigator.Shortcuts.NewCraftingUI_MaterialIndex = j;
				if (DrawRecipeSlot(spriteBatch, item, 43, val2, flag, 0.7f * num))
				{
					ItemSlot.HoverOverrideClick(item, 43);
					CraftingUI.SetRecipeMaterialDisplayName(recipe, item);
				}
				if (!Main.InGuideCraftMenu)
				{
					DrawOwnedItemCount(spriteBatch, availableItemCount, flag, val2, num);
				}
			}
		}
		int num2 = 42;
		int num3 = 285;
		if (Main.LocalPlayer.difficulty == 3 && !Main.CreativeMenu.Blocked)
		{
			num2 += 40;
		}
		CraftingUI.DrawGridToggle(spriteBatch, num2, num3, 20030);
		num2 += 40;
		if (!Main.InGuideCraftMenu)
		{
			CraftingUI.DrawCraftFromNearbyChestsToggle(spriteBatch, num2, num3, 20031);
		}
		SetupGamepadPoints(recipeEntry != null, i);
	}

	private void HandleCraftSlot(RecipeEntry entry, int context)
	{
		Recipe recipe = entry.Recipe;
		bool flag = _selectedRecipeIndex != entry.index || (PlayerInput.UsingGamepad && context == 41);
		if (!entry.Available | flag)
		{
			if ((!ItemSlot.HoverOverrideClick(recipe.createItem, context) & flag) && ((Main.mouseLeft && Main.mouseLeftRelease) || (Main.mouseRight && Main.mouseRightRelease)))
			{
				_selectedRecipeIndex = entry.index;
				if (entry.Available)
				{
					Main.focusRecipe = entry.availableIndex;
				}
				UILinkPointNavigator.ChangePoint(20000);
				_gamepadReturnToGridEntry = true;
				Main.stackSplit = 15;
				Main._preventCraftingBecauseClickWasUsedToChangeFocusedRecipe = true;
				SoundEngine.PlaySound(12);
			}
			ItemSlot.MouseHover(recipe.createItem, context);
		}
		else
		{
			Main.HoverOverCraftingItemButton(Main.focusRecipe);
		}
		if (!entry.Available)
		{
			_missingRequirementsTooltipText = GetReasonForRecipeNotAvailable(recipe);
		}
	}

	private string GetReasonForRecipeNotAvailable(Recipe recipe)
	{
		_missingObjects.Clear();
		recipe.PlayerMeetsEnvironmentConditions(Main.LocalPlayer, _missingObjects);
		if (_missingObjects.Count > 0)
		{
			return Lang.inter[22].Value + " " + string.Join(", ", _missingObjects);
		}
		return Language.GetTextValue("GameUI.NotEnoughMaterials");
	}

	internal static void AddTooltipLines(Item hoverItem, ref int numLines, string[] lineText, Color[] lineColors)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		if (_instance == null || (_instance._missingRequirementsTooltipText != null && !hoverItem.IsAir))
		{
			lineText[numLines] = _instance._missingRequirementsTooltipText;
			lineColors[numLines] = new Color(255, 140, 160, 255);
			numLines++;
		}
	}

	private void DrawOwnedItemCount(SpriteBatch spriteBatch, int owned, bool enough, Vector2 mpos, float mscale)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		mpos += new Vector2(3f, 32f) * mscale;
		string text = ((owned > 999) ? "999+" : owned.ToString());
		Color baseColor = (enough ? new Color(144, 238, 144, 255) : new Color(255, 140, 160, 255));
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, mpos, baseColor, 0f, Vector2.Zero, Vector2.One * 0.8f * mscale, -1f, 1f);
	}

	private bool DrawRecipeSlot(SpriteBatch spriteBatch, Item item, int context, Vector2 pos, bool enabled, float scale)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		Color inventoryBack = Main.inventoryBack;
		Main.inventoryBack = Color.White * (191f / 255f);
		float inventoryScale = Main.inventoryScale;
		Main.inventoryScale *= scale;
		ItemSlot.Draw(spriteBatch, ref item, context, pos, enabled ? Color.White : DisabledSlotColor);
		Main.inventoryScale = inventoryScale;
		Main.inventoryBack = inventoryBack;
		Rectangle val = new Rectangle((int)pos.X, (int)pos.Y, (int)((float)TextureAssets.InventoryBack.Width() * scale), (int)((float)TextureAssets.InventoryBack.Height() * scale));
		return val.Contains(Main.MouseScreen.ToPoint());
	}

	private void ResetRecipes()
	{
		_resetForGuideItem = (Main.InGuideCraftMenu ? Main.guideItem : null);
		_gamepadReturnToGridEntry = false;
		_gamepadMoveToGridEntryHack = false;
		Array.Resize(ref _recipeListLookup, Recipe.maxRecipes);
		if (_recipes.Count != 0)
		{
			_recipes.Clear();
			_filteredRecipes.Clear();
			Array.Clear(_recipeListLookup, 0, _recipeListLookup.Length);
		}
	}

	private void UpdateContents()
	{
		Recipe.UpdateRecipeList();
		if (Main.InGuideCraftMenu && Main.guideItem != _resetForGuideItem)
		{
			ResetRecipes();
		}
		foreach (RecipeEntry recipe in _recipes)
		{
			recipe.availableIndex = -1;
		}
		int numRecipes = GetNumRecipes();
		bool flag = _filteredRecipes.Count == 0;
		for (int i = 0; i < numRecipes; i++)
		{
			int num = Main.availableRecipe[i];
			RecipeEntry recipeEntry = _recipeListLookup[num];
			if (recipeEntry == null)
			{
				recipeEntry = (_recipeListLookup[num] = new RecipeEntry(num));
				_recipes.Add(recipeEntry);
				if (FitsFilter(recipeEntry.Recipe))
				{
					recipeEntry.gridIndex = _filteredRecipes.Count;
					_filteredRecipes.Add(recipeEntry);
				}
			}
			recipeEntry.availableIndex = i;
		}
		if (SelectedEntry == null)
		{
			_selectedRecipeIndex = null;
		}
		else if (_filter != null && !_filter.Accepts(SelectedEntry.Recipe))
		{
			_selectedRecipeIndex = null;
		}
		else if (SelectedEntry.Available)
		{
			Main.focusRecipe = SelectedEntry.availableIndex;
		}
		if (_itemGrid.Count != _filteredRecipes.Count || (flag && _filteredRecipes.Count > 0))
		{
			_itemGrid.SetContentsToShow(_filteredRecipes);
		}
	}

	private bool FitsFilter(Recipe recipe)
	{
		if (_filterer.FitsFilter(recipe.createItem))
		{
			if (_filter != null)
			{
				return _filter.Accepts(recipe);
			}
			return true;
		}
		return false;
	}

	private void OnSearchContentsChanged(string contents)
	{
		_filterer.SetSearchFilter(contents);
		ResetRecipes();
	}

	public override void Update(GameTime gameTime)
	{
		if (_filter != null && !_filter.CanRemainOpen())
		{
			Close(quiet: false, returnToInventory: true);
		}
		else
		{
			base.Update(gameTime);
		}
	}

	private void SetupGamepadPoints(bool craftSlotVisible, int materialCount)
	{
		UILinkPage uILinkPage = UILinkPointNavigator.Pages[24];
		int currentID = 20050;
		List<SnapPoint> snapPoints = GetSnapPoints();
		UILinkPage uILinkPage2 = UILinkPointNavigator.Pages[0];
		UILinkPoint uILinkPoint = uILinkPage2.LinkMap[300];
		UILinkPoint uILinkPoint2 = ((!craftSlotVisible) ? null : uILinkPage.LinkMap[20000]);
		UILinkPoint uILinkPoint3 = ((!Main.InGuideCraftMenu) ? null : uILinkPage.LinkMap[20020]);
		UILinkPoint uILinkPoint4 = ((Main.LocalPlayer.difficulty != 3 || Main.CreativeMenu.Blocked) ? null : uILinkPage2.LinkMap[311]);
		UILinkPoint uILinkPoint5 = uILinkPage.LinkMap[20030];
		UILinkPoint uILinkPoint6 = (Main.InGuideCraftMenu ? null : uILinkPage.LinkMap[20031]);
		UILinkPoint uILinkPoint7 = _helper.MakeLinkPointFromSnapPoint(currentID++, snapPoints.First((SnapPoint pt) => pt.Name == "NewCraftingUISearch"));
		UILinkPoint uILinkPoint8 = _helper.MakeLinkPointFromSnapPoint(currentID++, snapPoints.First((SnapPoint pt) => pt.Name == "NewCraftingUISearchCancel"));
		if (_gamepadMoveToSearchButtonHack)
		{
			_gamepadMoveToSearchButtonHack = false;
			UILinkPointNavigator.ChangePoint(uILinkPoint7.ID);
		}
		List<SnapPoint> orderedPointsByCategoryName = _helper.GetOrderedPointsByCategoryName(snapPoints, "NewCraftingUIFilters");
		UILinkPoint[] array = _helper.CreateUILinkStripHorizontal(ref currentID, orderedPointsByCategoryName);
		uILinkPoint7.Up = array[0].ID;
		uILinkPoint7.Right = uILinkPoint8.ID;
		for (int num = 0; num < array.Length; num++)
		{
			UILinkPoint upSide = ((num == 10) ? uILinkPoint : uILinkPage2.LinkMap[40 + (int)Math.Round((float)(num * 10) / 11f)]);
			_helper.PairUpDown(upSide, array[num]);
			array[num].Down = uILinkPoint7.ID;
		}
		int num2 = 0;
		if (uILinkPoint4 != null)
		{
			_helper.PairUpDown(uILinkPoint4, array[num2]);
			_helper.PairUpDown(uILinkPage2.LinkMap[40], uILinkPoint4);
			num2++;
		}
		_helper.PairLeftRight(uILinkPoint4, uILinkPoint5);
		_helper.PairUpDown(uILinkPoint5, array[num2]);
		_helper.PairUpDown(uILinkPage2.LinkMap[40 + num2], uILinkPoint5);
		num2++;
		_helper.PairLeftRight(uILinkPoint5, uILinkPoint6);
		if (uILinkPoint6 != null)
		{
			_helper.PairUpDown(uILinkPoint6, array[num2]);
			_helper.PairUpDown(uILinkPage2.LinkMap[40 + num2], uILinkPoint6);
		}
		_helper.PairLeftRight(uILinkPoint6 ?? uILinkPoint5, uILinkPoint);
		_helper.PairUpDown(uILinkPoint2, uILinkPoint3);
		_helper.PairUpDown(uILinkPoint7, uILinkPoint2 ?? uILinkPoint3);
		uILinkPoint8.Left = uILinkPoint7.ID;
		uILinkPoint8.Up = uILinkPoint7.Up;
		uILinkPoint8.Down = uILinkPoint7.Down;
		UILinkPoint uILinkPoint9 = uILinkPoint3 ?? uILinkPoint2 ?? uILinkPoint7;
		UILinkPoint uILinkPoint10 = null;
		List<SnapPoint> orderedPointsByCategoryName2 = _helper.GetOrderedPointsByCategoryName(snapPoints, "DynamicItemCollectionSlot");
		if (orderedPointsByCategoryName2.Count > 0)
		{
			int currentID2 = 20100;
			int itemsPerLine = _itemGrid.GetItemsPerLine();
			UILinkPoint[,] array2 = _helper.CreateUILinkPointGrid(ref currentID2, orderedPointsByCategoryName2, itemsPerLine, uILinkPoint9, null, null, null);
			_helper.KeepCurrentPointInRangeAfterGridScroll(array2);
			uILinkPoint10 = array2[0, 0];
			if (SelectedEntry != null && SelectedEntry.gridIndex >= 0)
			{
				int num3 = SelectedEntry.gridIndex - orderedPointsByCategoryName2[0].Id;
				if (num3 >= 0 && num3 < orderedPointsByCategoryName2.Count)
				{
					uILinkPoint10 = array2[num3 % itemsPerLine, num3 / itemsPerLine];
					if (_gamepadMoveToGridEntryHack)
					{
						UILinkPointNavigator.ChangePoint(uILinkPoint10.ID);
					}
				}
			}
		}
		uILinkPoint9.Down = uILinkPoint10?.ID ?? (-1);
		_gamepadMoveToGridEntryHack = false;
		UILinkPoint rightSide = ((materialCount == 0) ? null : uILinkPage.LinkMap[20001]);
		UILinkPoint uILinkPoint11 = ((materialCount == 0) ? null : uILinkPage.LinkMap[20001 + materialCount - 1]);
		_helper.PairLeftRight(uILinkPoint2, rightSide);
		for (int num4 = 0; num4 < Recipe.maxRequirements - 1; num4++)
		{
			UILinkPoint uILinkPoint12 = uILinkPage.LinkMap[20001 + num4];
			_helper.PairLeftRight(uILinkPoint12, uILinkPage.LinkMap[20001 + num4 + 1]);
			uILinkPoint12.Down = uILinkPoint2?.Down ?? (-1);
			uILinkPoint12.Up = uILinkPoint2?.Up ?? (-1);
		}
		if (uILinkPoint11 != null)
		{
			uILinkPoint11.Right = -1;
		}
	}

	private static bool IsGridPoint(int point)
	{
		if (point >= 20100)
		{
			return point < 21000;
		}
		return false;
	}

	private static bool IsMaterialPoint(int point)
	{
		if (point >= 20001)
		{
			return point < 20016;
		}
		return false;
	}

	private string GetGamepadInstructions()
	{
		if (!Visible)
		{
			return "";
		}
		bool flag = true;
		int num = -1;
		if (UILinkPointNavigator.CurrentPoint == 20000)
		{
			num = 42;
		}
		else if (UILinkPointNavigator.CurrentPoint == 20020)
		{
			num = 7;
		}
		else if (IsMaterialPoint(UILinkPointNavigator.CurrentPoint))
		{
			num = 43;
		}
		else if (_hoveredEntry != null)
		{
			num = 41;
		}
		else
		{
			flag = false;
		}
		if (num != 42 && num != 43)
		{
			_gamepadReturnToGridEntry = false;
		}
		string text = "";
		if (_gamepadReturnToGridEntry)
		{
			text += PlayerInput.BuildCommand(Language.GetTextValue("UI.Back"), PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]);
			Main.LocalPlayer.releaseInventory = false;
			if (PlayerInput.AllowExecutionOfGamepadInstructions && PlayerInput.Triggers.JustPressed.Inventory)
			{
				_gamepadMoveToGridEntryHack = true;
			}
		}
		else
		{
			text += PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]);
		}
		text += PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		if (!flag || _hoveredEntry != null)
		{
			text += PlayerInput.BuildCommand(Lang.misc[53].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			if (_hoveredEntry != null && _hoveredEntry.Available && !Main.InGuideCraftMenu)
			{
				string quickCraftGamepadInstructions = ItemSlot.GetQuickCraftGamepadInstructions(_hoveredEntry.Recipe);
				if (quickCraftGamepadInstructions != null)
				{
					if (PlayerInput.AllowExecutionOfGamepadInstructions && PlayerInput.Triggers.Current.Grapple)
					{
						_selectedRecipeIndex = _hoveredEntry.index;
					}
					text += quickCraftGamepadInstructions;
				}
			}
		}
		else if (num == 42)
		{
			RecipeEntry selectedEntry = SelectedEntry;
			if (selectedEntry != null && selectedEntry.Available)
			{
				text += ItemSlot.GetCraftSlotGamepadInstructions();
			}
		}
		if (num == 7)
		{
			return text + ItemSlot.GetGamepadInstructions(ref Main.guideItem, num);
		}
		if (!flag)
		{
			return text + ItemSlot.GetGamepadInstructions(43);
		}
		return text + ItemSlot.GetGamepadInstructions(num);
	}

	private void GoBackFromVirtualKeyboard()
	{
		IngameFancyUI.Close(quiet: true);
		_gamepadMoveToSearchButtonHack = true;
	}

	public static void Close(bool quiet = false, bool returnToInventory = false)
	{
		if (Visible)
		{
			_ui.SetState(null);
			Main.PipsFastScroll = true;
			if (!returnToInventory)
			{
				Main.playerInventory = false;
			}
			if (!quiet)
			{
				SoundEngine.PlaySound(11);
			}
		}
	}

	public static void Open(bool quiet = false, RecipeFilter filter = null)
	{
		if (!Visible)
		{
			if (!Main.playerInventory || (Main.LocalPlayer.chest == -1 && !Main.InGuideCraftMenu))
			{
				IngameUIWindows.CloseAll(quiet: true);
			}
			Main.playerInventory = true;
			Main.PipsCurrentPage = Main.PipPage.Recipes;
			Main._preventCraftingBecauseClickWasUsedToChangeFocusedRecipe = true;
			if (_instance == null)
			{
				_instance = new NewCraftingUI();
			}
			_instance.SetFilter(filter);
			_ui.SetState(_instance);
			if (!quiet)
			{
				SoundEngine.PlaySound(10);
			}
		}
	}

	public override void OnActivate()
	{
		_instance._openedWithoutFilter = _filter == null;
		_selectedRecipeIndex = ((Main.numAvailableRecipes < 0) ? ((int?)null) : new int?(Main.availableRecipe[Main.focusRecipe]));
		ResetRecipes();
		_searchBar.SetContents("");
		_filterer.ActiveFilters.Clear();
		UILinkPointNavigator.ChangePage(24);
	}

	public override void OnDeactivate()
	{
		_filter = null;
		_selectedRecipeIndex = null;
		_hoveredEntry = null;
		_missingRequirementsTooltipText = null;
		UILinkPointNavigator.ChangePoint(1500);
	}

	public static void ToggleInInventory(bool quiet = false)
	{
		if (Visible)
		{
			Close(quiet, returnToInventory: true);
		}
		else
		{
			Open(quiet);
		}
	}

	public static void OpenCloseFilter(RecipeFilter filter)
	{
		if (!Visible)
		{
			Open(quiet: false, filter);
		}
		else if (_instance._filter == null || !_instance._filter.Matches(filter))
		{
			if (Main.InGuideCraftMenu)
			{
				Main.LocalPlayer.SetTalkNPC(-1);
				Main.InGuideCraftMenu = false;
				Main.LocalPlayer.dropItemCheck();
			}
			SoundEngine.PlaySound(12);
			_instance.SetFilter(filter);
		}
		else if (_instance._openedWithoutFilter)
		{
			SoundEngine.PlaySound(12);
			_instance.SetFilter(null);
		}
		else
		{
			Close(quiet: false, returnToInventory: true);
		}
	}

	private void SetFilter(RecipeFilter filter)
	{
		_filter = filter;
		ResetRecipes();
	}

	public static void UpdateUI(GameTime gameTime)
	{
		if (Visible && !Main.inFancyUI)
		{
			_ui.Update(gameTime);
		}
	}

	public static void DrawUI(SpriteBatch spriteBatch)
	{
		if (Visible && !Main.inFancyUI)
		{
			_ui.Draw(spriteBatch, Main.gameTimeCache);
		}
	}

	public static void RefreshGrid()
	{
		if (Visible)
		{
			_instance.ResetRecipes();
		}
	}

	static NewCraftingUI()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
	}
}
