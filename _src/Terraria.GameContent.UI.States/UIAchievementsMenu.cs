using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States;

public class UIAchievementsMenu : UIState
{
	private UIList _achievementsList;

	private List<UIAchievementListItem> _achievementElements = new List<UIAchievementListItem>();

	private List<UIToggleImage> _categoryButtons = new List<UIToggleImage>();

	private UIElement _backpanel;

	private UIElement _outerContainer;

	public void InitializePage()
	{
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		RemoveAllChildren();
		_categoryButtons.Clear();
		_achievementElements.Clear();
		_achievementsList = null;
		bool flag = true;
		int num = flag.ToInt() * 100;
		UIElement uIElement = new UIElement();
		uIElement.Width.Set(0f, 0.8f);
		uIElement.MaxWidth.Set(800f + (float)num, 0f);
		uIElement.MinWidth.Set(600f + (float)num, 0f);
		uIElement.Top.Set(220f, 0f);
		uIElement.Height.Set(-220f, 1f);
		uIElement.HAlign = 0.5f;
		_outerContainer = uIElement;
		Append(uIElement);
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-110f, 1f);
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
		uIPanel.PaddingTop = 0f;
		uIElement.Append(uIPanel);
		_achievementsList = new UIList();
		_achievementsList.Width.Set(-25f, 1f);
		_achievementsList.Height.Set(-50f, 1f);
		_achievementsList.Top.Set(50f, 0f);
		_achievementsList.ListPadding = 5f;
		uIPanel.Append(_achievementsList);
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Achievements"), 1f, large: true);
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
		_backpanel = uITextPanel2;
		List<Achievement> list = Main.Achievements.CreateAchievementsList();
		for (int i = 0; i < list.Count; i++)
		{
			UIAchievementListItem item = new UIAchievementListItem(list[i], flag);
			_achievementsList.Add(item);
			_achievementElements.Add(item);
		}
		UIScrollbar uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(100f, 1000f);
		uIScrollbar.Height.Set(-50f, 1f);
		uIScrollbar.Top.Set(50f, 0f);
		uIScrollbar.HAlign = 1f;
		uIPanel.Append(uIScrollbar);
		_achievementsList.SetScrollbar(uIScrollbar);
		UIElement uIElement2 = new UIElement();
		uIElement2.Width.Set(0f, 1f);
		uIElement2.Height.Set(32f, 0f);
		uIElement2.Top.Set(10f, 0f);
		Asset<Texture2D> texture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Categories", (AssetRequestMode)1);
		for (int j = 0; j < 4; j++)
		{
			UIToggleImage uIToggleImage = new UIToggleImage(texture, 32, 32, new Point(34 * j, 0), new Point(34 * j, 34));
			uIToggleImage.Left.Set(j * 36 + 8, 0f);
			uIToggleImage.SetState(value: true);
			uIToggleImage.OnLeftClick += FilterList;
			_categoryButtons.Add(uIToggleImage);
			uIElement2.Append(uIToggleImage);
		}
		uIPanel.Append(uIElement2);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		base.Draw(spriteBatch);
		for (int i = 0; i < _categoryButtons.Count; i++)
		{
			if (_categoryButtons[i].IsMouseHovering)
			{
				string text = "";
				text = i switch
				{
					3 => Language.GetTextValue("Achievements.ChallengerCategory"), 
					1 => Language.GetTextValue("Achievements.CollectorCategory"), 
					2 => Language.GetTextValue("Achievements.ExplorerCategory"), 
					0 => Language.GetTextValue("Achievements.SlayerCategory"), 
					-1 => Language.GetTextValue("Achievements.NoCategory"), 
					_ => Language.GetTextValue("Achievements.NoCategory"), 
				};
				float x = FontAssets.MouseText.Value.MeasureString(text).X;
				Vector2 val = new Vector2((float)Main.mouseX, (float)Main.mouseY) + new Vector2(16f);
				if (val.Y > (float)(Main.screenHeight - 30))
				{
					val.Y = Main.screenHeight - 30;
				}
				if (val.X > (float)Main.screenWidth - x)
				{
					val.X = Main.screenWidth - 460;
				}
				Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, text, val.X, val.Y, new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), Color.Black, Vector2.Zero);
				break;
			}
		}
		SetupGamepadPoints(spriteBatch);
	}

	public void GotoAchievement(Achievement achievement)
	{
		_achievementsList.Goto((UIElement element) => element is UIAchievementListItem uIAchievementListItem && uIAchievementListItem.GetAchievement() == achievement);
	}

	private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		Main.menuMode = 0;
		IngameFancyUI.Close();
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
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	private void FilterList(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		_achievementsList.Clear();
		foreach (UIAchievementListItem achievementElement in _achievementElements)
		{
			if (_categoryButtons[(int)achievementElement.GetAchievement().Category].IsOn)
			{
				_achievementsList.Add(achievementElement);
			}
		}
		Recalculate();
	}

	public override void OnActivate()
	{
		InitializePage();
		if (Main.gameMenu)
		{
			_outerContainer.Top.Set(220f, 0f);
			_outerContainer.Height.Set(-220f, 1f);
		}
		else
		{
			_outerContainer.Top.Set(120f, 0f);
			_outerContainer.Height.Set(-120f, 1f);
		}
		_achievementsList.UpdateOrder();
		if (PlayerInput.UsingGamepadUI)
		{
			UILinkPointNavigator.ChangePoint(3002);
		}
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 3;
		int num = 3000;
		Rectangle val = _backpanel.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(num, val.Center.ToVector2());
		int iD = num + 1;
		val = _outerContainer.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(iD, val.Center.ToVector2());
		int num2 = num;
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[num2];
		uILinkPoint.Unlink();
		uILinkPoint.Up = num2 + 1;
		num2++;
		UILinkPoint uILinkPoint2 = UILinkPointNavigator.Points[num2];
		uILinkPoint2.Unlink();
		uILinkPoint2.Up = num2 + 1;
		uILinkPoint2.Down = num2 - 1;
		for (int i = 0; i < _categoryButtons.Count; i++)
		{
			num2 = (UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2 + 1);
			int iD2 = num2;
			val = _categoryButtons[i].GetInnerDimensions().ToRectangle();
			UILinkPointNavigator.SetPosition(iD2, val.Center.ToVector2());
			UILinkPoint uILinkPoint3 = UILinkPointNavigator.Points[num2];
			uILinkPoint3.Unlink();
			uILinkPoint3.Left = ((i == 0) ? (-3) : (num2 - 1));
			uILinkPoint3.Right = ((i == _categoryButtons.Count - 1) ? (-4) : (num2 + 1));
			uILinkPoint3.Down = num;
		}
	}
}
