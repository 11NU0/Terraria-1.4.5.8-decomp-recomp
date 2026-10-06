using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Creative;

public class CreativePowersHelper
{
	public class CreativePowerIconLocations
	{
		public static readonly Point Unassigned = new Point(0, 0);

		public static readonly Point Deprecated = new Point(0, 0);

		public static readonly Point ItemDuplication = new Point(0, 0);

		public static readonly Point ItemResearch = new Point(1, 0);

		public static readonly Point TimeCategory = new Point(2, 0);

		public static readonly Point WeatherCategory = new Point(3, 0);

		public static readonly Point EnemyStrengthSlider = new Point(4, 0);

		public static readonly Point GameEvents = new Point(5, 0);

		public static readonly Point Godmode = new Point(6, 0);

		public static readonly Point BlockPlacementRange = new Point(7, 0);

		public static readonly Point StopBiomeSpread = new Point(8, 0);

		public static readonly Point EnemySpawnRate = new Point(9, 0);

		public static readonly Point FreezeTime = new Point(10, 0);

		public static readonly Point TimeDawn = new Point(11, 0);

		public static readonly Point TimeNoon = new Point(12, 0);

		public static readonly Point TimeDusk = new Point(13, 0);

		public static readonly Point TimeMidnight = new Point(14, 0);

		public static readonly Point WindDirection = new Point(15, 0);

		public static readonly Point WindFreeze = new Point(16, 0);

		public static readonly Point RainStrength = new Point(17, 0);

		public static readonly Point RainFreeze = new Point(18, 0);

		public static readonly Point ModifyTime = new Point(19, 0);

		public static readonly Point PersonalCategory = new Point(20, 0);

		static CreativePowerIconLocations()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		}
	}

	public const int TextureIconColumns = 21;

	public const int TextureIconRows = 1;

	public static Color CommonSelectedColor = new Color(152, 175, 235);

	private static Asset<Texture2D> GetPowerIconAsset(string path)
	{
		return Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
	}

	public static UIImageFramed GetIconImage(Point iconLocation)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> powerIconAsset = GetPowerIconAsset("Images/UI/Creative/Infinite_Powers");
		return new UIImageFramed(powerIconAsset, powerIconAsset.Frame(21, 1, iconLocation.X, iconLocation.Y))
		{
			MarginLeft = 4f,
			MarginTop = 4f,
			VAlign = 0.5f,
			HAlign = 1f,
			IgnoresMouseInteraction = true
		};
	}

	public static GroupOptionButton<bool> CreateToggleButton(CreativePowerUIElementRequestInfo info)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, null, Color.White, null, 0.8f);
		groupOptionButton.Width = new StyleDimension(info.PreferredButtonWidth, 0f);
		groupOptionButton.Height = new StyleDimension(info.PreferredButtonHeight, 0f);
		groupOptionButton.ShowHighlightWhenSelected = false;
		groupOptionButton.SetCurrentOption(option: false);
		groupOptionButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
		groupOptionButton.SetColorsBasedOnSelectionState(Main.OurFavoriteColor, Colors.InventoryDefaultColor, 1f, 0.7f);
		return groupOptionButton;
	}

	public static GroupOptionButton<bool> CreateSimpleButton(CreativePowerUIElementRequestInfo info)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, null, Color.White, null, 0.8f);
		groupOptionButton.Width = new StyleDimension(info.PreferredButtonWidth, 0f);
		groupOptionButton.Height = new StyleDimension(info.PreferredButtonHeight, 0f);
		groupOptionButton.ShowHighlightWhenSelected = false;
		groupOptionButton.SetCurrentOption(option: false);
		groupOptionButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
		return groupOptionButton;
	}

	public static GroupOptionButton<T> CreateCategoryButton<T>(CreativePowerUIElementRequestInfo info, T option, T currentOption) where T : IConvertible, IEquatable<T>
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		GroupOptionButton<T> groupOptionButton = new GroupOptionButton<T>(option, null, null, Color.White, null, 0.8f);
		groupOptionButton.Width = new StyleDimension(info.PreferredButtonWidth, 0f);
		groupOptionButton.Height = new StyleDimension(info.PreferredButtonHeight, 0f);
		groupOptionButton.ShowHighlightWhenSelected = false;
		groupOptionButton.SetCurrentOption(currentOption);
		groupOptionButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
		return groupOptionButton;
	}

	public static void AddPermissionTextIfNeeded(ICreativePower power, ref string originalText)
	{
		if (!IsAvailableForPlayer(power, Main.myPlayer))
		{
			string textValue = Language.GetTextValue("CreativePowers.CantUsePowerBecauseOfNoPermissionFromServer");
			originalText = originalText + "\n" + textValue;
		}
	}

	public static void AddDescriptionIfNeeded(ref string originalText, string descriptionKey)
	{
		if (CreativePowerSettings.ShouldPowersBeElaborated)
		{
			string textValue = Language.GetTextValue(descriptionKey);
			originalText = originalText + "\n" + textValue;
		}
	}

	public static void AddUnlockTextIfNeeded(ref string originalText, bool needed, string descriptionKey)
	{
		if (!needed)
		{
			string textValue = Language.GetTextValue(descriptionKey);
			originalText = originalText + "\n" + textValue;
		}
	}

	public static UIVerticalSlider CreateSlider(Func<float> GetSliderValueMethod, Action<float> SetValueKeyboardMethod, Action SetValueGamepadMethod)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		return new UIVerticalSlider(GetSliderValueMethod, SetValueKeyboardMethod, SetValueGamepadMethod, Color.Red)
		{
			Width = new StyleDimension(12f, 0f),
			Height = new StyleDimension(-10f, 1f),
			Left = new StyleDimension(6f, 0f),
			HAlign = 0f,
			VAlign = 0.5f,
			EmptyColor = Color.OrangeRed,
			FilledColor = Color.CornflowerBlue
		};
	}

	public static void UpdateUseMouseInterface(UIElement affectedElement)
	{
		if (affectedElement.IsMouseHovering)
		{
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	public static void UpdateUnlockStateByPower(ICreativePower power, UIElement button, Color colorWhenSelected)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		IGroupOptionButton asButton = button as IGroupOptionButton;
		if (asButton != null)
		{
			button.OnUpdate += (UIElement element) =>
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				UpdateUnlockStateByPowerInternal(power, colorWhenSelected, asButton);
			};
		}
	}

	public static bool IsAvailableForPlayer(ICreativePower power, int playerIndex)
	{
		switch (power.CurrentPermissionLevel)
		{
		default:
			return false;
		case PowerPermissionLevel.CanBeChangedByHostAlone:
			if (Main.netMode == 0)
			{
				return true;
			}
			return Main.countsAsHostForGameplay[playerIndex];
		case PowerPermissionLevel.CanBeChangedByEveryone:
			return true;
		}
	}

	private static void UpdateUnlockStateByPowerInternal(ICreativePower power, Color colorWhenSelected, IGroupOptionButton asButton)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		bool isUnlocked = power.GetIsUnlocked();
		bool flag = !IsAvailableForPlayer(power, Main.myPlayer);
		asButton.SetBorderColor(flag ? Color.DimGray : Color.White);
		if (flag)
		{
			asButton.SetColorsBasedOnSelectionState(new Color(60, 60, 60), new Color(60, 60, 60), 0.7f, 0.7f);
		}
		else if (isUnlocked)
		{
			asButton.SetColorsBasedOnSelectionState(colorWhenSelected, Colors.InventoryDefaultColor, 1f, 0.7f);
		}
		else
		{
			asButton.SetColorsBasedOnSelectionState(Color.Crimson, Color.Red, 0.7f, 0.7f);
		}
	}

	static CreativePowersHelper()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
	}
}
