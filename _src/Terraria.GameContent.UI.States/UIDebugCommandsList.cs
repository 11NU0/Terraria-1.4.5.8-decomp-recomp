using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Testing.ChatCommands;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.States;

public class UIDebugCommandsList : UIState
{
	private readonly UIList _commandsList = new UIList();

	private readonly List<UIDebugCommandItem> _commands = new List<UIDebugCommandItem>();

	public UIDebugCommandsList()
	{
		BuildPage();
	}

	public override void OnDeactivate()
	{
	}

	private void BuildPage()
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		RemoveAllChildren();
		UIElement uIElement = new UIElement();
		uIElement.Width.Set(0f, 0.8f);
		uIElement.MaxWidth.Set(800f, 0f);
		uIElement.MinWidth.Set(600f, 0f);
		uIElement.Top.Set(220f, 0f);
		uIElement.Height.Set(-220f, 1f);
		uIElement.HAlign = 0.5f;
		Append(uIElement);
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-110f, 1f);
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.95f;
		uIElement.Append(uIPanel);
		UIWrappedSearchBar uIWrappedSearchBar = new UIWrappedSearchBar(() =>
		{
			UserInterface.ActiveInstance.SetState(this);
		})
		{
			Width = new StyleDimension(200f, 0f),
			Top = new StyleDimension(20f, 0f)
		};
		uIWrappedSearchBar.OnSearchContentsChanged += searchbar_OnSearchContentsChanged;
		uIPanel.Append(uIWrappedSearchBar);
		_commandsList.Width.Set(-25f, 1f);
		_commandsList.Height.Set(-60f, 1f);
		_commandsList.VAlign = 1f;
		_commandsList.ListPadding = 5f;
		uIPanel.Append(_commandsList);
		UITextPanel<string> uITextPanel = new UITextPanel<string>("Debug Commands", 1f, large: true);
		uITextPanel.HAlign = 0.5f;
		uITextPanel.Top.Set(-33f, 0f);
		uITextPanel.SetPadding(13f);
		uITextPanel.BackgroundColor = new Color(73, 94, 171);
		uIElement.Append(uITextPanel);
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
		uITextPanel2.Width.Set(-10f, 0.5f);
		uITextPanel2.Height.Set(50f, 0f);
		uITextPanel2.VAlign = 1f;
		uITextPanel2.HAlign = 0.5f;
		uITextPanel2.Top.Set(-45f, 0f);
		uITextPanel2.OnMouseOver += FadedMouseOver;
		uITextPanel2.OnMouseOut += FadedMouseOut;
		uITextPanel2.OnLeftClick += GoBackClick;
		uIElement.Append(uITextPanel2);
		UIScrollbar uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(100f, 1000f);
		uIScrollbar.Height.Set(0f, 1f);
		uIScrollbar.HAlign = 1f;
		uIPanel.Append(uIScrollbar);
		_commandsList.SetScrollbar(uIScrollbar);
		PopulateCommandsList();
	}

	private void searchbar_OnSearchContentsChanged(string searchContents)
	{
		if (searchContents == null)
		{
			searchContents = string.Empty;
		}
		string text = searchContents.ToLowerInvariant().Trim();
		bool flag = string.IsNullOrWhiteSpace(text);
		_commandsList.Clear();
		foreach (UIDebugCommandItem command in _commands)
		{
			if (flag || DoesCommandMatchSearch(text, command))
			{
				_commandsList.Add(command);
			}
		}
	}

	private static bool DoesCommandMatchSearch(string lowerContents, UIDebugCommandItem command)
	{
		IDebugCommand command2 = command.Command;
		if (command2.Name.ToLowerInvariant().Contains(lowerContents))
		{
			return true;
		}
		if (command2.Description != null && command2.Description.ToLowerInvariant().Contains(lowerContents))
		{
			return true;
		}
		if (command2.HelpText != null && command2.HelpText.ToLowerInvariant().Contains(lowerContents))
		{
			return true;
		}
		return false;
	}

	private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		IngameFancyUI.Close();
	}

	private static void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
		((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
	}

	private static void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	private void PopulateCommandsList()
	{
		List<IDebugCommand> list = ChatManager.DebugCommands.Commands.ToList();
		list.Sort((IDebugCommand x, IDebugCommand y) => StringComparer.OrdinalIgnoreCase.Compare(x.Name, y.Name));
		int num = 0;
		foreach (IDebugCommand item2 in list)
		{
			UIDebugCommandItem item = new UIDebugCommandItem(item2, num++);
			_commands.Add(item);
			_commandsList.Add(item);
		}
	}

	private static void DrawMouseOver()
	{
		Item item = new Item();
		item.SetDefaults(0);
		item.SetNameOverride("Dev Commands");
		item.type = 1;
		item.scale = 0f;
		item.rare = 10;
		Main.HoverItem = item;
		Main.instance.MouseText("", 0, 0);
		Main.mouseText = true;
	}
}
