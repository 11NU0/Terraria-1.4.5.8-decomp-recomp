using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public abstract class AWorldListItem : UIPanel
{
	protected WorldFileData _data;

	protected int _glitchFrameCounter;

	protected int _glitchFrame;

	protected int _glitchVariation;

	private void UpdateGlitchAnimation(UIElement affectedElement)
	{
		_ = _glitchFrame;
		int minValue = 3;
		int num = 3;
		if (_glitchFrame == 0)
		{
			minValue = 15;
			num = 120;
		}
		if (++_glitchFrameCounter >= Main.rand.Next(minValue, num + 1))
		{
			_glitchFrameCounter = 0;
			_glitchFrame = (_glitchFrame + 1) % 16;
			if ((_glitchFrame == 4 || _glitchFrame == 8 || _glitchFrame == 12) && Main.rand.Next(3) == 0)
			{
				_glitchVariation = Main.rand.Next(7);
			}
		}
		(affectedElement as UIImageFramed).SetFrame(7, 16, _glitchVariation, _glitchFrame, 0, 0);
	}

	protected void GetDifficulty(out string expertText, out Color gameModeColor)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		expertText = "";
		gameModeColor = Color.White;
		if (_data.GameMode == 3)
		{
			expertText = Language.GetTextValue("UI.Creative");
			gameModeColor = Main.creativeModeColor;
			return;
		}
		int num = 1;
		switch (_data.GameMode)
		{
		case 1:
			num = 2;
			break;
		case 2:
			num = 3;
			break;
		}
		if (_data.ForTheWorthy)
		{
			num++;
		}
		switch (num)
		{
		default:
			expertText = Language.GetTextValue("UI.Normal");
			break;
		case 2:
			expertText = Language.GetTextValue("UI.Expert");
			gameModeColor = Main.mcColor;
			break;
		case 3:
			expertText = Language.GetTextValue("UI.Master");
			gameModeColor = Main.hcColor;
			break;
		case 4:
			expertText = Language.GetTextValue("UI.Legendary");
			gameModeColor = Main.legendaryModeColor;
			break;
		}
	}

	protected Asset<Texture2D> GetIcon()
	{
		if (_data.ZenithWorld)
		{
			return GetSeedIcon("Everything", hardmodeVariants: true, evilVariants: false);
		}
		if (_data.DrunkWorld)
		{
			return GetSeedIcon("CorruptionCrimson", hardmodeVariants: true, evilVariants: false);
		}
		if (_data.ForTheWorthy)
		{
			return GetSeedIcon("FTW");
		}
		if (_data.NotTheBees)
		{
			return GetSeedIcon("NotTheBees");
		}
		if (_data.Anniversary)
		{
			return GetSeedIcon("Anniversary");
		}
		if (_data.DontStarve)
		{
			return GetSeedIcon("DontStarve");
		}
		if (_data.RemixWorld)
		{
			return GetSeedIcon("Remix");
		}
		if (_data.NoTrapsWorld)
		{
			return GetSeedIcon("Traps");
		}
		if (_data.SkyblockWorld)
		{
			return GetSeedIcon("Skyblock", hardmodeVariants: false, evilVariants: false);
		}
		return GetSeedIcon("");
	}

	protected List<Asset<Texture2D>> GetIcons()
	{
		List<Asset<Texture2D>> list = new List<Asset<Texture2D>>();
		if (_data.DrunkWorld)
		{
			list.Add(GetSeedIcon("CorruptionCrimson", hardmodeVariants: true, evilVariants: false));
		}
		if (_data.ForTheWorthy)
		{
			list.Add(GetSeedIcon("FTW"));
		}
		if (_data.NotTheBees)
		{
			list.Add(GetSeedIcon("NotTheBees"));
		}
		if (_data.Anniversary)
		{
			list.Add(GetSeedIcon("Anniversary"));
		}
		if (_data.DontStarve)
		{
			list.Add(GetSeedIcon("DontStarve"));
		}
		if (_data.RemixWorld)
		{
			list.Add(GetSeedIcon("Remix"));
		}
		if (_data.NoTrapsWorld)
		{
			list.Add(GetSeedIcon("Traps"));
		}
		if (_data.SkyblockWorld)
		{
			list.Add(GetSeedIcon("Skyblock", hardmodeVariants: false, evilVariants: false));
		}
		if (list.Count > 0)
		{
			return list;
		}
		list.Add(GetSeedIcon(""));
		return list;
	}

	protected UIElement GetIconElement()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (_data.ZenithWorld)
		{
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/IconEverythingAnimated", (AssetRequestMode)1);
			UIImageFramed uIImageFramed = new UIImageFramed(val, val.Frame(7, 16));
			uIImageFramed.Left = new StyleDimension(4f, 0f);
			uIImageFramed.OnUpdate += UpdateGlitchAnimation;
			return uIImageFramed;
		}
		return new UICyclingImage(GetIcons())
		{
			Left = new StyleDimension(4f, 0f)
		};
	}

	private Asset<Texture2D> GetSeedIcon(string seed, bool hardmodeVariants = true, bool evilVariants = true)
	{
		string text = "Images/UI/Icon";
		if (hardmodeVariants)
		{
			text += (_data.IsHardMode ? "Hallow" : "");
		}
		if (evilVariants)
		{
			text += (_data.HasCorruption ? "Corruption" : "Crimson");
		}
		text += seed;
		return Main.Assets.Request<Texture2D>(text, (AssetRequestMode)1);
	}
}
