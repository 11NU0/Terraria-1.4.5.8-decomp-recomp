using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UICreativeInfiniteItemsDisplay : UIElement
{
	public enum InfiniteItemsDisplayPage
	{
		InfiniteItemsPickup,
		InfiniteItemsResearch
	}

	private CreativeUnlocksTracker _lastTrackerCheckedForEdits;

	private int _lastCheckedVersionForEdits = -1;

	private UICreativeItemGrid _itemGrid;

	private EntryFilterer<Item, IItemEntryFilter> _filterer;

	private EntrySorter<Item, ICreativeItemSortStep> _sorter;

	private UIElement _containerInfinites;

	private UIElement _containerSacrifice;

	private bool _showSacrificesInsteadOfInfinites;

	public const string SnapPointName_SacrificeSlot = "CreativeSacrificeSlot";

	public const string SnapPointName_SacrificeConfirmButton = "CreativeSacrificeConfirm";

	public const string SnapPointName_InfinitesFilter = "CreativeInfinitesFilter";

	public const string SnapPointName_InfinitesSearch = "CreativeInfinitesSearch";

	public const string SnapPointName_InfinitesSearchCancel = "CreativeInfinitesSearchCancel";

	private List<UIImage> _sacrificeCogsSmall = new List<UIImage>();

	private List<UIImage> _sacrificeCogsMedium = new List<UIImage>();

	private List<UIImage> _sacrificeCogsBig = new List<UIImage>();

	private UIImageFramed _sacrificePistons;

	private UIParticleLayer _pistonParticleSystem;

	private Asset<Texture2D> _pistonParticleAsset;

	private int _sacrificeAnimationTimeLeft;

	private bool _researchComplete;

	private bool _hovered;

	private int _lastItemIdSacrificed;

	private int _lastItemAmountWeHad;

	private int _lastItemAmountWeNeededTotal;

	private List<Item> _itemList = new List<Item>();

	public UICreativeInfiniteItemsDisplay()
	{
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
		_sorter = new EntrySorter<Item, ICreativeItemSortStep>();
		_sorter.AddSortSteps(new List<ICreativeItemSortStep>
		{
			new SortingSteps.ByUnlockStatus(),
			new SortingSteps.ByCreativeSortingId(),
			new SortingSteps.Alphabetical()
		});
		BuildPage();
	}

	private void BuildPage()
	{
		_lastCheckedVersionForEdits = -1;
		RemoveAllChildren();
		SetPadding(0f);
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.Fill,
			Height = StyleDimension.Fill
		};
		uIElement.SetPadding(0f);
		_containerInfinites = uIElement;
		UIElement uIElement2 = new UIElement
		{
			Width = StyleDimension.Fill,
			Height = StyleDimension.Fill
		};
		uIElement2.SetPadding(0f);
		_containerSacrifice = uIElement2;
		BuildInfinitesMenuContents(uIElement);
		BuildSacrificeMenuContents(uIElement2);
		UpdateContents();
		OnUpdate += UICreativeInfiniteItemsDisplay_OnUpdate;
	}

	private void Hover_OnUpdate(UIElement affectedElement)
	{
		if (_hovered)
		{
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	private void Hover_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		_hovered = false;
	}

	private void Hover_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_hovered = true;
	}

	private static UIPanel CreateBasicPanel()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		UIPanel uIPanel = new UIPanel();
		SetBasicSizesForCreativeSacrificeOrInfinitesPanel(uIPanel);
		uIPanel.BackgroundColor *= 0.8f;
		uIPanel.BorderColor *= 0.8f;
		return uIPanel;
	}

	private static void SetBasicSizesForCreativeSacrificeOrInfinitesPanel(UIElement element)
	{
		element.Width = new StyleDimension(0f, 1f);
		element.Height = new StyleDimension(-38f, 1f);
		element.Top = new StyleDimension(38f, 0f);
	}

	private void BuildInfinitesMenuContents(UIElement totalContainer)
	{
		UIPanel uIPanel = CreateBasicPanel();
		totalContainer.Append(uIPanel);
		uIPanel.OnUpdate += Hover_OnUpdate;
		uIPanel.OnMouseOver += Hover_OnMouseOver;
		uIPanel.OnMouseOut += Hover_OnMouseOut;
		_itemGrid = new UICreativeItemGrid();
		UIWrappedSearchBar uIWrappedSearchBar = new UIWrappedSearchBar(GoBackFromVirtualKeyboard);
		uIWrappedSearchBar.CustomOpenVirtualKeyboard = IngameFancyUI.OpenUIState;
		uIWrappedSearchBar.OnSearchContentsChanged += OnSearchContentsChanged;
		uIWrappedSearchBar.SetSearchSnapPoint("CreativeInfinitesSearch", 0);
		uIWrappedSearchBar.SetSearchCancelSnapPoint("CreativeInfinitesSearchCancel", 0);
		uIPanel.Append(uIWrappedSearchBar);
		UIList uIList = new UIList
		{
			Width = new StyleDimension(-25f, 1f),
			Height = new StyleDimension(-28f, 1f),
			VAlign = 1f,
			HAlign = 0f
		};
		uIPanel.Append(uIList);
		float num = 4f;
		UIScrollbar uIScrollbar = new UIScrollbar
		{
			Height = new StyleDimension(-28f - num * 2f, 1f),
			Top = new StyleDimension(0f - num, 0f),
			VAlign = 1f,
			HAlign = 1f
		};
		uIPanel.Append(uIScrollbar);
		uIList.SetScrollbar(uIScrollbar);
		uIList.Add(_itemGrid);
		UICreativeItemsInfiniteFilteringOptions uICreativeItemsInfiniteFilteringOptions = new UICreativeItemsInfiniteFilteringOptions(_filterer, "CreativeInfinitesFilter");
		uICreativeItemsInfiniteFilteringOptions.OnClickingOption += filtersHelper_OnClickingOption;
		uICreativeItemsInfiniteFilteringOptions.Left = new StyleDimension(20f, 0f);
		totalContainer.Append(uICreativeItemsInfiniteFilteringOptions);
		uICreativeItemsInfiniteFilteringOptions.OnUpdate += Hover_OnUpdate;
		uICreativeItemsInfiniteFilteringOptions.OnMouseOver += Hover_OnMouseOver;
		uICreativeItemsInfiniteFilteringOptions.OnMouseOut += Hover_OnMouseOut;
	}

	private void BuildSacrificeMenuContents(UIElement totalContainer)
	{
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		UIPanel uIPanel = CreateBasicPanel();
		uIPanel.VAlign = 0.5f;
		uIPanel.Height = new StyleDimension(170f, 0f);
		uIPanel.Width = new StyleDimension(170f, 0f);
		uIPanel.Top = default;
		totalContainer.Append(uIPanel);
		uIPanel.OnUpdate += Hover_OnUpdate;
		uIPanel.OnMouseOver += Hover_OnMouseOver;
		uIPanel.OnMouseOut += Hover_OnMouseOut;
		AddCogsForSacrificeMenu(uIPanel);
		_pistonParticleAsset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_Spark", (AssetRequestMode)1);
		float pixels = 0f;
		UIImage uIImage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_Slots", (AssetRequestMode)1))
		{
			HAlign = 0.5f,
			VAlign = 0.5f,
			Top = new StyleDimension(-20f, 0f),
			Left = new StyleDimension(pixels, 0f)
		};
		uIPanel.Append(uIImage);
		Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_FramedPistons", (AssetRequestMode)1);
		UIImageFramed uIImageFramed = new UIImageFramed(val, val.Frame(1, 9))
		{
			HAlign = 0.5f,
			VAlign = 0.5f,
			Top = new StyleDimension(-20f, 0f),
			Left = new StyleDimension(pixels, 0f),
			IgnoresMouseInteraction = true
		};
		uIPanel.Append(uIImageFramed);
		_sacrificePistons = uIImageFramed;
		UIParticleLayer pistonParticleSystem = new UIParticleLayer
		{
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(0f, 1f),
			AnchorPositionOffsetByPercents = Vector2.One / 2f,
			AnchorPositionOffsetByPixels = Vector2.Zero
		};
		_pistonParticleSystem = pistonParticleSystem;
		uIImageFramed.Append(_pistonParticleSystem);
		UIElement uIElement = Main.CreativeMenu.ProvideItemSlotElement(0);
		uIElement.HAlign = 0.5f;
		uIElement.VAlign = 0.5f;
		uIElement.Top = new StyleDimension(-15f, 0f);
		uIElement.Left = new StyleDimension(pixels, 0f);
		uIElement.SetSnapPoint("CreativeSacrificeSlot", 0);
		uIImage.Append(uIElement);
		UIText uIText = new UIText("(0/50)", 0.8f)
		{
			Top = new StyleDimension(10f, 0f),
			Left = new StyleDimension(pixels, 0f),
			HAlign = 0.5f,
			VAlign = 0.5f,
			IgnoresMouseInteraction = true
		};
		uIText.OnUpdate += descriptionText_OnUpdate;
		uIPanel.Append(uIText);
		UIPanel uIPanel2 = new UIPanel
		{
			Top = new StyleDimension(0f, 0f),
			Left = new StyleDimension(pixels, 0f),
			HAlign = 0.5f,
			VAlign = 1f,
			Width = new StyleDimension(124f, 0f),
			Height = new StyleDimension(30f, 0f)
		};
		UIText element = new UIText(Language.GetText("CreativePowers.ConfirmInfiniteItemSacrifice"), 0.8f)
		{
			IgnoresMouseInteraction = true,
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		uIPanel2.Append(element);
		uIPanel2.SetSnapPoint("CreativeSacrificeConfirm", 0);
		uIPanel2.OnLeftClick += sacrificeButton_OnClick;
		uIPanel2.OnMouseOver += FadedMouseOver;
		uIPanel2.OnMouseOut += FadedMouseOut;
		uIPanel2.OnUpdate += research_OnUpdate;
		uIPanel.Append(uIPanel2);
		uIPanel.OnUpdate += sacrificeWindow_OnUpdate;
	}

	private void research_OnUpdate(UIElement affectedElement)
	{
		if (affectedElement.IsMouseHovering)
		{
			Main.instance.MouseTextNoOverride(Language.GetTextValue("CreativePowers.ResearchButtonTooltip"), 0, 0);
		}
	}

	private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
	}

	private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	private void AddCogsForSacrificeMenu(UIElement sacrificesContainer)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		UIElement uIElement = new UIElement();
		uIElement.IgnoresMouseInteraction = true;
		SetBasicSizesForCreativeSacrificeOrInfinitesPanel(uIElement);
		uIElement.VAlign = 0.5f;
		uIElement.Height = new StyleDimension(170f, 0f);
		uIElement.Width = new StyleDimension(280f, 0f);
		uIElement.Top = default;
		uIElement.SetPadding(0f);
		sacrificesContainer.Append(uIElement);
		Vector2 val = new Vector2(-10f, -10f);
		AddSymetricalCogsPair(uIElement, new Vector2(22f, 1f) + val, "Images/UI/Creative/Research_GearC", _sacrificeCogsSmall);
		AddSymetricalCogsPair(uIElement, new Vector2(1f, 28f) + val, "Images/UI/Creative/Research_GearB", _sacrificeCogsMedium);
		AddSymetricalCogsPair(uIElement, new Vector2(5f, 5f) + val, "Images/UI/Creative/Research_GearA", _sacrificeCogsBig);
	}

	private void sacrificeWindow_OnUpdate(UIElement affectedElement)
	{
		UpdateVisualFrame();
	}

	private void UpdateVisualFrame()
	{
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.05f;
		float sacrificeAnimationProgress = GetSacrificeAnimationProgress();
		float lerpValue = Utils.GetLerpValue(1f, 0.7f, sacrificeAnimationProgress, clamped: true);
		float num2 = lerpValue * lerpValue;
		num2 *= 2f;
		float num3 = 1f + num2;
		num *= num3;
		float num4 = 1.1428572f;
		float num5 = 1f;
		OffsetRotationsForCogs(2f * num, _sacrificeCogsSmall);
		OffsetRotationsForCogs(num4 * num, _sacrificeCogsMedium);
		OffsetRotationsForCogs((0f - num5) * num, _sacrificeCogsBig);
		int frameY = 0;
		if (_sacrificeAnimationTimeLeft != 0)
		{
			float num6 = 0.1f;
			float num7 = 1f / 15f;
			if (sacrificeAnimationProgress >= 1f - num6)
			{
				frameY = 8;
			}
			else if (sacrificeAnimationProgress >= 1f - num6 * 2f)
			{
				frameY = 7;
			}
			else if (sacrificeAnimationProgress >= 1f - num6 * 3f)
			{
				frameY = 6;
			}
			else if (sacrificeAnimationProgress >= num7 * 4f)
			{
				frameY = 5;
			}
			else if (sacrificeAnimationProgress >= num7 * 3f)
			{
				frameY = 4;
			}
			else
			{
				frameY = ((sacrificeAnimationProgress >= num7 * 2f) ? 3 : ((!(sacrificeAnimationProgress >= num7)) ? 1 : 2));
			}
			if (_sacrificeAnimationTimeLeft == 56)
			{
				SoundEngine.PlaySound(63);
				Vector2 accelerationPerFrame = new Vector2(0f, 0.16350001f);
				for (int i = 0; i < 15; i++)
				{
					Vector2 val = Main.rand.NextVector2Circular(4f, 3f);
					if (val.Y > 0f)
					{
						val.Y = 0f - val.Y;
					}
					val.Y -= 2f;
					_pistonParticleSystem.AddParticle(new CreativeSacrificeParticle(_pistonParticleAsset, null, val, Vector2.Zero)
					{
						AccelerationPerFrame = accelerationPerFrame,
						ScaleOffsetPerFrame = -1f / 60f
					});
				}
			}
			if (_sacrificeAnimationTimeLeft == 40 && _researchComplete)
			{
				_researchComplete = false;
				SoundEngine.PlaySound(64);
			}
		}
		_sacrificePistons.SetFrame(1, 9, 0, frameY, 0, 0);
	}

	private static void OffsetRotationsForCogs(float rotationOffset, List<UIImage> cogsList)
	{
		cogsList[0].Rotation += rotationOffset;
		cogsList[1].Rotation -= rotationOffset;
	}

	private void AddSymetricalCogsPair(UIElement sacrificesContainer, Vector2 cogOFfsetsInPixels, string assetPath, List<UIImage> imagesList)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> val = Main.Assets.Request<Texture2D>(assetPath, (AssetRequestMode)1);
		cogOFfsetsInPixels += -val.Size() / 2f;
		UIImage uIImage = new UIImage(val)
		{
			NormalizedOrigin = Vector2.One / 2f,
			Left = new StyleDimension(cogOFfsetsInPixels.X, 0f),
			Top = new StyleDimension(cogOFfsetsInPixels.Y, 0f)
		};
		imagesList.Add(uIImage);
		sacrificesContainer.Append(uIImage);
		uIImage = new UIImage(val)
		{
			NormalizedOrigin = Vector2.One / 2f,
			HAlign = 1f,
			Left = new StyleDimension(0f - cogOFfsetsInPixels.X, 0f),
			Top = new StyleDimension(cogOFfsetsInPixels.Y, 0f)
		};
		imagesList.Add(uIImage);
		sacrificesContainer.Append(uIImage);
	}

	private void descriptionText_OnUpdate(UIElement affectedElement)
	{
		UIText uIText = affectedElement as UIText;
		bool sacrificeNumbers = Main.CreativeMenu.GetSacrificeNumbers(out var itemIdChecked, out var amountWeHave, out var amountNeededTotal);
		Main.CreativeMenu.ShouldDrawSacrificeArea();
		if (!Main.mouseItem.IsAir)
		{
			ForgetItemSacrifice();
		}
		if (itemIdChecked == 0)
		{
			if (_lastItemIdSacrificed != 0 && _lastItemAmountWeNeededTotal != _lastItemAmountWeHad)
			{
				uIText.SetText($"({_lastItemAmountWeHad}/{_lastItemAmountWeNeededTotal})");
			}
			else
			{
				uIText.SetText("???");
			}
			return;
		}
		ForgetItemSacrifice();
		if (!sacrificeNumbers)
		{
			uIText.SetText("X");
		}
		else
		{
			uIText.SetText($"({amountWeHave}/{amountNeededTotal})");
		}
	}

	private void sacrificeButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SacrificeWhatYouCan();
	}

	public void SacrificeWhatYouCan()
	{
		Main.CreativeMenu.GetSacrificeNumbers(out var itemIdChecked, out var amountWeHave, out var amountNeededTotal);
		int amountWeSacrificed;
		switch (Main.CreativeMenu.SacrificeItem(out amountWeSacrificed))
		{
		case CreativeUI.ItemSacrificeResult.SacrificedAndDone:
			_researchComplete = true;
			BeginSacrificeAnimation();
			RememberItemSacrifice(itemIdChecked, amountWeHave + amountWeSacrificed, amountNeededTotal);
			break;
		case CreativeUI.ItemSacrificeResult.SacrificedButNotDone:
			_researchComplete = false;
			BeginSacrificeAnimation();
			RememberItemSacrifice(itemIdChecked, amountWeHave + amountWeSacrificed, amountNeededTotal);
			break;
		}
	}

	public void StopPlayingAnimation()
	{
		ForgetItemSacrifice();
		_sacrificeAnimationTimeLeft = 0;
		_pistonParticleSystem.ClearParticles();
		UpdateVisualFrame();
	}

	private void RememberItemSacrifice(int itemId, int amountWeHave, int amountWeNeedTotal)
	{
		_lastItemIdSacrificed = itemId;
		_lastItemAmountWeHad = amountWeHave;
		_lastItemAmountWeNeededTotal = amountWeNeedTotal;
	}

	private void ForgetItemSacrifice()
	{
		_lastItemIdSacrificed = 0;
		_lastItemAmountWeHad = 0;
		_lastItemAmountWeNeededTotal = 0;
	}

	private void BeginSacrificeAnimation()
	{
		_sacrificeAnimationTimeLeft = 60;
	}

	private void UpdateSacrificeAnimation()
	{
		if (_sacrificeAnimationTimeLeft > 0)
		{
			_sacrificeAnimationTimeLeft--;
		}
	}

	private float GetSacrificeAnimationProgress()
	{
		return Utils.GetLerpValue(60f, 0f, _sacrificeAnimationTimeLeft, clamped: true);
	}

	public void SetPageTypeToShow(InfiniteItemsDisplayPage page)
	{
		_showSacrificesInsteadOfInfinites = page == InfiniteItemsDisplayPage.InfiniteItemsResearch;
	}

	private void UICreativeInfiniteItemsDisplay_OnUpdate(UIElement affectedElement)
	{
		RemoveAllChildren();
		CreativeUnlocksTracker localPlayerCreativeTracker = Main.LocalPlayerCreativeTracker;
		if (_lastTrackerCheckedForEdits != localPlayerCreativeTracker)
		{
			_lastTrackerCheckedForEdits = localPlayerCreativeTracker;
			_lastCheckedVersionForEdits = -1;
		}
		int lastEditId = localPlayerCreativeTracker.ItemSacrifices.LastEditId;
		if (_lastCheckedVersionForEdits != lastEditId)
		{
			_lastCheckedVersionForEdits = lastEditId;
			UpdateContents();
		}
		if (_showSacrificesInsteadOfInfinites)
		{
			Append(_containerSacrifice);
		}
		else
		{
			Append(_containerInfinites);
		}
		UpdateSacrificeAnimation();
	}

	private void filtersHelper_OnClickingOption()
	{
		UpdateContents();
	}

	private void UpdateContents()
	{
		_itemList.Clear();
		Main.LocalPlayerCreativeTracker.ItemSacrifices.ForEachItemWithResearchProgress((int type) =>
		{
			Item item = ContentSamples.ItemsByType[type];
			if (_filterer.FitsFilter(item))
			{
				_itemList.Add(item);
			}
		});
		_itemList.Sort(_sorter);
		_itemGrid.SetContentsToShow(_itemList);
	}

	private void OnSearchContentsChanged(string contents)
	{
		_filterer.SetSearchFilter(contents);
		UpdateContents();
	}

	private static UserInterface GetCurrentInterface()
	{
		UserInterface activeInstance = UserInterface.ActiveInstance;
		if (Main.gameMenu)
		{
			return Main.MenuUI;
		}
		return Main.InGameUI;
	}

	private void GoBackFromVirtualKeyboard()
	{
		IngameFancyUI.Close(quiet: true);
		Main.playerInventory = true;
		Main.CreativeMenu.ResumeMenuFromGamepadSearch();
	}

	public int GetItemsPerLine()
	{
		return _itemGrid.GetItemsPerLine();
	}
}
