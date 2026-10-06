using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States;

public class UIWorldSelect : UIState
{
	public static WorldFileData NewlyGeneratedWorld;

	private UIList _worldList;

	private UITextPanel<LocalizedText> _backPanel;

	private UITextPanel<LocalizedText> _newPanel;

	private UIPanel _containerPanel;

	private UIScrollbar _scrollbar;

	private bool _isScrollbarAttached;

	private List<Tuple<string, bool>> favoritesCache = new List<Tuple<string, bool>>();

	private bool skipDraw;

	public override void OnInitialize()
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		UIElement uIElement = new UIElement();
		uIElement.Width.Set(0f, 0.8f);
		uIElement.MaxWidth.Set(650f, 0f);
		uIElement.Top.Set(220f, 0f);
		uIElement.Height.Set(-220f, 1f);
		uIElement.HAlign = 0.5f;
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-110f, 1f);
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
		uIElement.Append(uIPanel);
		_containerPanel = uIPanel;
		_worldList = new UIList();
		_worldList.Width.Set(0f, 1f);
		_worldList.Height.Set(0f, 1f);
		_worldList.ListPadding = 5f;
		uIPanel.Append(_worldList);
		_scrollbar = new UIScrollbar();
		_scrollbar.SetView(100f, 1000f);
		_scrollbar.Height.Set(0f, 1f);
		_scrollbar.HAlign = 1f;
		_worldList.SetScrollbar(_scrollbar);
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.SelectWorld"), 0.8f, large: true);
		uITextPanel.HAlign = 0.5f;
		uITextPanel.Top.Set(-40f, 0f);
		uITextPanel.SetPadding(15f);
		uITextPanel.BackgroundColor = new Color(73, 94, 171);
		uIElement.Append(uITextPanel);
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
		uITextPanel2.Width.Set(-10f, 0.5f);
		uITextPanel2.Height.Set(50f, 0f);
		uITextPanel2.VAlign = 1f;
		uITextPanel2.HAlign = 0f;
		uITextPanel2.Top.Set(-45f, 0f);
		uITextPanel2.OnMouseOver += FadedMouseOver;
		uITextPanel2.OnMouseOut += FadedMouseOut;
		uITextPanel2.OnLeftClick += GoBackClick;
		uIElement.Append(uITextPanel2);
		_backPanel = uITextPanel2;
		UITextPanel<LocalizedText> uITextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("UI.New"), 0.7f, large: true);
		uITextPanel3.CopyStyle(uITextPanel2);
		uITextPanel3.HAlign = 1f;
		uITextPanel3.OnMouseOver += FadedMouseOver;
		uITextPanel3.OnMouseOut += FadedMouseOut;
		uITextPanel3.OnLeftClick += NewWorldClick;
		uIElement.Append(uITextPanel3);
		_newPanel = uITextPanel3;
		Append(uIElement);
	}

	public override void Recalculate()
	{
		if (_scrollbar != null)
		{
			if (_isScrollbarAttached && !_scrollbar.CanScroll)
			{
				_containerPanel.RemoveChild(_scrollbar);
				_isScrollbarAttached = false;
				_worldList.Width.Set(0f, 1f);
			}
			else if (!_isScrollbarAttached && _scrollbar.CanScroll)
			{
				_containerPanel.Append(_scrollbar);
				_isScrollbarAttached = true;
				_worldList.Width.Set(-25f, 1f);
			}
		}
		base.Recalculate();
	}

	private void NewWorldClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.newWorldName = Lang.gen[57].Value + " " + (Main.WorldList.Count + 1);
		Main.menuMode = 888;
		Main.MenuUI.SetState(new UIWorldCreation());
	}

	private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(11);
		Main.menuMode = ((!Main.menuMultiplayer) ? 1 : 12);
	}

	private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
		((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
	}

	private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	public override void OnActivate()
	{
		Main.LoadWorlds();
		UpdateWorldsList();
		if (PlayerInput.UsingGamepadUI)
		{
			UILinkPointNavigator.ChangePoint(3000 + ((_worldList.Count == 0) ? 1 : 2));
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		NewlyGeneratedWorld = null;
	}

	private void UpdateWorldsList()
	{
		_worldList.Clear();
		IOrderedEnumerable<WorldFileData> orderedEnumerable = Main.WorldList.OrderByDescending(CanWorldBeJoinedByActivePlayer).ThenByDescending(IsNewlyGenerated).ThenByDescending((WorldFileData x) => x.IsFavorite)
			.ThenByDescending(HasWorldBeenPlayedByActivePlayer)
			.ThenByDescending((WorldFileData x) => x.LastPlayed)
			.ThenBy((WorldFileData x) => x.Name)
			.ThenBy((WorldFileData x) => x.GetFileName());
		int num = 0;
		foreach (WorldFileData item in orderedEnumerable)
		{
			_worldList.Add(new UIWorldListItem(item, num++, CanWorldBeJoinedByActivePlayer(item), HasWorldBeenPlayedByActivePlayer(item), IsNewlyGenerated(item)));
		}
	}

	private static bool IsNewlyGenerated(WorldFileData file)
	{
		if (NewlyGeneratedWorld != null && file.Path == NewlyGeneratedWorld.Path)
		{
			return file.IsCloudSave == NewlyGeneratedWorld.IsCloudSave;
		}
		return false;
	}

	private static bool CanWorldBeJoinedByActivePlayer(WorldFileData file)
	{
		bool flag = Main.ActivePlayerFileData.Player.difficulty == 3;
		bool flag2 = file.GameMode == 3;
		return flag == flag2;
	}

	private static bool HasWorldBeenPlayedByActivePlayer(WorldFileData file)
	{
		string mapPath;
		return WorldMap.TryGetMapPath(Main.ActivePlayerFileData, file, out mapPath);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected Obj, but got Unknown
		if (skipDraw)
		{
			skipDraw = false;
			return;
		}
		if (UpdateFavoritesCache())
		{
			skipDraw = true;
			Main.MenuUI.Draw(spriteBatch, new GameTime());
		}
		base.Draw(spriteBatch);
		SetupGamepadPoints(spriteBatch);
	}

	private bool UpdateFavoritesCache()
	{
		List<WorldFileData> list = new List<WorldFileData>(Main.WorldList);
		list.Sort((WorldFileData x, WorldFileData y) =>
		{
			if (x.IsFavorite && !y.IsFavorite)
			{
				return -1;
			}
			if (!x.IsFavorite && y.IsFavorite)
			{
				return 1;
			}
			if (x.Name == null)
			{
				return 1;
			}
			return (x.Name.CompareTo(y.Name) != 0) ? x.Name.CompareTo(y.Name) : x.GetFileName().CompareTo(y.GetFileName());
		});
		bool flag = false;
		if (!flag && list.Count != favoritesCache.Count)
		{
			flag = true;
		}
		if (!flag)
		{
			for (int num = 0; num < favoritesCache.Count; num++)
			{
				Tuple<string, bool> tuple = favoritesCache[num];
				if (!(list[num].Name == tuple.Item1) || list[num].IsFavorite != tuple.Item2)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			favoritesCache.Clear();
			foreach (WorldFileData item in list)
			{
				favoritesCache.Add(Tuple.Create(item.Name, item.IsFavorite));
			}
			UpdateWorldsList();
		}
		return flag;
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 2;
		int num = 3000;
		Rectangle val = _backPanel.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(num, val.Center.ToVector2());
		int iD = num + 1;
		val = _newPanel.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(iD, val.Center.ToVector2());
		int num2 = num;
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[num2];
		uILinkPoint.Unlink();
		uILinkPoint.Right = num2 + 1;
		num2 = num + 1;
		uILinkPoint = UILinkPointNavigator.Points[num2];
		uILinkPoint.Unlink();
		uILinkPoint.Left = num2 - 1;
		float num3 = 1f / Main.UIScale;
		Rectangle clippingRectangle = _containerPanel.GetClippingRectangle(spriteBatch);
		Vector2 minimum = clippingRectangle.TopLeft() * num3;
		Vector2 maximum = clippingRectangle.BottomRight() * num3;
		List<SnapPoint> snapPoints = GetSnapPoints();
		for (int i = 0; i < snapPoints.Count; i++)
		{
			if (!snapPoints[i].Position.Between(minimum, maximum))
			{
				snapPoints.Remove(snapPoints[i]);
				i--;
			}
		}
		SnapPoint[,] array = new SnapPoint[_worldList.Count, 6];
		foreach (SnapPoint item in snapPoints.Where((SnapPoint a) => a.Name == "Play"))
		{
			array[item.Id, 0] = item;
		}
		foreach (SnapPoint item2 in snapPoints.Where((SnapPoint a) => a.Name == "Favorite"))
		{
			array[item2.Id, 1] = item2;
		}
		foreach (SnapPoint item3 in snapPoints.Where((SnapPoint a) => a.Name == "Cloud"))
		{
			array[item3.Id, 2] = item3;
		}
		foreach (SnapPoint item4 in snapPoints.Where((SnapPoint a) => a.Name == "Seed"))
		{
			array[item4.Id, 3] = item4;
		}
		foreach (SnapPoint item5 in snapPoints.Where((SnapPoint a) => a.Name == "Rename"))
		{
			array[item5.Id, 4] = item5;
		}
		foreach (SnapPoint item6 in snapPoints.Where((SnapPoint a) => a.Name == "Delete"))
		{
			array[item6.Id, 5] = item6;
		}
		num2 = num + 2;
		int[] array2 = new int[_worldList.Count];
		for (int num4 = 0; num4 < array2.Length; num4++)
		{
			array2[num4] = -1;
		}
		for (int num5 = 0; num5 < array.GetLength(1); num5++)
		{
			int num6 = -1;
			for (int num7 = 0; num7 < array.GetLength(0); num7++)
			{
				if (array[num7, num5] != null)
				{
					uILinkPoint = UILinkPointNavigator.Points[num2];
					uILinkPoint.Unlink();
					UILinkPointNavigator.SetPosition(num2, array[num7, num5].Position);
					if (num6 != -1)
					{
						uILinkPoint.Up = num6;
						UILinkPointNavigator.Points[num6].Down = num2;
					}
					if (array2[num7] != -1)
					{
						uILinkPoint.Left = array2[num7];
						UILinkPointNavigator.Points[array2[num7]].Right = num2;
					}
					uILinkPoint.Down = num;
					if (num5 == 0)
					{
						UILinkPointNavigator.Points[num].Up = (UILinkPointNavigator.Points[num + 1].Up = num2);
					}
					num6 = num2;
					array2[num7] = num2;
					UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2;
					num2++;
				}
			}
		}
		if (PlayerInput.UsingGamepadUI && _worldList.Count == 0 && UILinkPointNavigator.CurrentPoint > 3001)
		{
			UILinkPointNavigator.ChangePoint(3001);
		}
	}
}
