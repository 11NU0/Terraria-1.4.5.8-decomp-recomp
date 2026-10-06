using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.Net;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.Creative;

public class CreativeUI
{
	public enum ItemSacrificeResult
	{
		CannotSacrifice,
		SacrificedButNotDone,
		SacrificedAndDone
	}

	public const int ItemSlotIndexes_SacrificeItem = 0;

	public const int ItemSlotIndexes_Count = 1;

	private bool _initialized;

	private Asset<Texture2D> _buttonTexture;

	private Asset<Texture2D> _buttonBorderTexture;

	private Item[] _itemSlotsForUI = new Item[1];

	private UserInterface _powersUI = new UserInterface();

	public bool GamepadMoveToSearchButtonHack;

	private UICreativePowersMenu _uiState;

	public bool Enabled { get; private set; }

	public bool Blocked
	{
		get
		{
			if (Main.LocalPlayer.talkNPC != -1)
			{
				return true;
			}
			if (NewCraftingUI.Visible)
			{
				return false;
			}
			if (Main.LocalPlayer.chest == -1)
			{
				return Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity();
			}
			return true;
		}
	}

	public CreativeUI()
	{
		for (int i = 0; i < _itemSlotsForUI.Length; i++)
		{
			_itemSlotsForUI[i] = new Item();
		}
	}

	public void Initialize()
	{
		_buttonTexture = Main.Assets.Request<Texture2D>("Images/UI/Creative/Journey_Toggle", (AssetRequestMode)1);
		_buttonBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Creative/Journey_Toggle_MouseOver", (AssetRequestMode)1);
		_uiState = new UICreativePowersMenu();
		_powersUI.SetState(_uiState);
		_initialized = true;
	}

	public void Update(GameTime gameTime)
	{
		if (Enabled && Main.playerInventory)
		{
			_powersUI.Update(gameTime);
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		if (!_initialized)
		{
			Initialize();
		}
		if (Main.LocalPlayer.difficulty != 3)
		{
			Enabled = false;
		}
		else if (!Blocked)
		{
			Vector2 location = new Vector2(28f, 267f);
			Vector2 val = new Vector2(353f, 258f);
			new Vector2(40f, 267f);
			_ = val + new Vector2(50f, 50f);
			if (Main.screenHeight < 650 && Enabled)
			{
				location.X += 52f * Main.inventoryScale;
			}
			DrawToggleButton(spriteBatch, location);
			if (Enabled)
			{
				_powersUI.Draw(spriteBatch, Main.gameTimeCache);
			}
		}
	}

	public UIElement ProvideItemSlotElement(int itemSlotContext)
	{
		if (itemSlotContext != 0)
		{
			return null;
		}
		return new UIItemSlot(_itemSlotsForUI, itemSlotContext, 30);
	}

	public Item GetItemByIndex(int itemSlotContext)
	{
		if (itemSlotContext != 0)
		{
			return null;
		}
		return _itemSlotsForUI[itemSlotContext];
	}

	public void SetItembyIndex(Item item, int itemSlotContext)
	{
		if (itemSlotContext == 0)
		{
			_itemSlotsForUI[itemSlotContext] = item;
		}
	}

	private void DrawToggleButton(SpriteBatch spritebatch, Vector2 location)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = _buttonTexture.Size();
		Rectangle hitbox = Utils.CenteredRectangle(location + val / 2f, val);
		UILinkPointNavigator.SetPosition(311, hitbox.Center.ToVector2());
		spritebatch.Draw(_buttonTexture.Value, location, (Rectangle?)null, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		Main.LocalPlayer.creativeInterface = false;
		if (hitbox.Contains(Main.MouseScreen.ToPoint()) && !PlayerInput.IgnoreMouseInterface)
		{
			Main.LocalPlayer.creativeInterface = true;
			Main.LocalPlayer.mouseInterface = true;
			if (Enabled)
			{
				Main.instance.MouseTextNoOverride(Language.GetTextValue("CreativePowers.PowersMenuOpen"), 0, 0);
			}
			else
			{
				Main.instance.MouseTextNoOverride(Language.GetTextValue("CreativePowers.PowersMenuClosed"), 0, 0);
			}
			spritebatch.Draw(_buttonBorderTexture.Value, location, (Rectangle?)null, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				ToggleMenu();
			}
		}
		Main.DoStatefulTickSound(ref Main.CreativeMenuMouseOver, Main.LocalPlayer.creativeInterface);
		if (Main.LocalPlayerCreativeTracker.ItemSacrifices.AnyNewUnlocksFromTeammates)
		{
			Utils.DrawNotificationIcon(spritebatch, hitbox);
		}
	}

	public void SwapItem(ref Item item)
	{
		Utils.Swap(ref item, ref _itemSlotsForUI[0]);
	}

	public void CloseMenu()
	{
		if (Enabled)
		{
			Enabled = false;
			StopPlayingSacrificeAnimations();
		}
	}

	public void ResumeMenuFromGamepadSearch()
	{
		Enabled = true;
		GamepadMoveToSearchButtonHack = true;
	}

	public void ToggleMenu()
	{
		Enabled = !Enabled;
		_powersUI.EscapeElements();
		UISliderBase.EscapeElements();
		SoundEngine.PlaySound(12);
		if (Enabled)
		{
			NewCraftingUI.Close(quiet: true, returnToInventory: true);
			Main.LocalPlayer.chest = -1;
			Main.LocalPlayer.tileEntityAnchor.Clear();
			UILinkPointNavigator.ChangePoint(10000);
		}
		else if (_itemSlotsForUI[0].stack > 0)
		{
			Main.LocalPlayer.GetOrDropItem(_itemSlotsForUI[0], GetItemSettings.ReturnItemFromSlot);
			_itemSlotsForUI[0] = new Item();
			StopPlayingSacrificeAnimations();
		}
	}

	public bool IsShowingResearchMenu()
	{
		if (Enabled && _uiState != null)
		{
			return _uiState.IsShowingResearchMenu;
		}
		return false;
	}

	public void SacrificeItemInSacrificeSlot()
	{
		if (_uiState != null)
		{
			_uiState.SacrificeWhatsInResearchMenu();
		}
	}

	public void StopPlayingSacrificeAnimations()
	{
		if (_uiState != null)
		{
			_uiState.StopPlayingResearchAnimations();
		}
	}

	public bool ShouldDrawSacrificeArea()
	{
		if (!_itemSlotsForUI[0].IsAir)
		{
			return true;
		}
		Item mouseItem = Main.mouseItem;
		if (mouseItem.IsAir)
		{
			return false;
		}
		if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(mouseItem.type, out var amountNeeded))
		{
			return false;
		}
		if (Main.LocalPlayerCreativeTracker.ItemSacrifices.GetSacrificeCount(mouseItem.type) < amountNeeded)
		{
			return true;
		}
		return false;
	}

	public bool GetSacrificeNumbers(out int itemIdChecked, out int amountWeHave, out int amountNeededTotal)
	{
		amountWeHave = 0;
		amountNeededTotal = 0;
		itemIdChecked = 0;
		Item item = _itemSlotsForUI[0];
		if (!item.IsAir)
		{
			itemIdChecked = item.type;
		}
		if (!Main.LocalPlayerCreativeTracker.ItemSacrifices.TryGetSacrificeNumbers(item.type, out amountWeHave, out amountNeededTotal))
		{
			return false;
		}
		return true;
	}

	public ItemSacrificeResult SacrificeItem(out int amountWeSacrificed)
	{
		return SacrificeItem(ref _itemSlotsForUI[0], out amountWeSacrificed);
	}

	public ItemSacrificeResult SacrificeItem(ref Item item, out int amountWeSacrificed, bool spawnExcessItem = true, bool onlySacrificeIfItWouldFinishResearch = false)
	{
		int amountNeededTotal = 0;
		int amountWeHave = 0;
		amountWeSacrificed = 0;
		if (!Main.LocalPlayerCreativeTracker.ItemSacrifices.TryGetSacrificeNumbers(item.type, out amountWeHave, out amountNeededTotal))
		{
			return ItemSacrificeResult.CannotSacrifice;
		}
		int num = Utils.Clamp(amountNeededTotal - amountWeHave, 0, amountNeededTotal);
		if (num == 0)
		{
			return ItemSacrificeResult.CannotSacrifice;
		}
		int num2 = Math.Min(num, item.stack);
		bool flag = num2 == num;
		if (onlySacrificeIfItWouldFinishResearch && !flag)
		{
			return ItemSacrificeResult.CannotSacrifice;
		}
		NetPacket packet = NetCreativeUnlocksPlayerReportModule.SerializeSacrificeRequest(Main.myPlayer, item.type, num2);
		NetManager.Instance.SendToServer(packet);
		if (!Main.ServerSideCharacter)
		{
			Main.LocalPlayerCreativeTracker.ItemSacrifices.RegisterItemSacrifice(item.type, num2);
		}
		item.stack -= num2;
		if (item.stack <= 0)
		{
			item.TurnToAir();
		}
		amountWeSacrificed = num2;
		if ((item.stack > 0) & spawnExcessItem)
		{
			item = Main.LocalPlayer.GetItem(item, GetItemSettings.ReturnItemFromSlot);
		}
		if (!flag)
		{
			return ItemSacrificeResult.SacrificedButNotDone;
		}
		return ItemSacrificeResult.SacrificedAndDone;
	}

	public void Reset()
	{
		for (int i = 0; i < _itemSlotsForUI.Length; i++)
		{
			_itemSlotsForUI[i].TurnToAir();
		}
		_initialized = false;
		Enabled = false;
	}
}
