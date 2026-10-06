using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent.UI.ResourceSets;

public class FancyClassicPlayerResourcesDisplaySet : IPlayerResourcesDisplaySet, IConfigKeyHolder
{
	private float _currentPlayerLife;

	private float _lifePerHeart;

	private int _playerLifeFruitCount;

	private int _lastHeartFillingIndex;

	private int _lastHeartPanelIndex;

	private int _heartCountRow1;

	private int _heartCountRow2;

	private int _starCount;

	private int _lastStarFillingIndex;

	private float _manaPerStar;

	private float _currentPlayerMana;

	private Asset<Texture2D> _heartLeft;

	private Asset<Texture2D> _heartMiddle;

	private Asset<Texture2D> _heartRight;

	private Asset<Texture2D> _heartRightFancy;

	private Asset<Texture2D> _heartFill;

	private Asset<Texture2D> _heartFillHoney;

	private Asset<Texture2D> _heartSingleFancy;

	private Asset<Texture2D> _starTop;

	private Asset<Texture2D> _starMiddle;

	private Asset<Texture2D> _starBottom;

	private Asset<Texture2D> _starSingle;

	private Asset<Texture2D> _starFill;

	private bool _hoverLife;

	private bool _hoverMana;

	private bool _drawText;

	public string NameKey { get; private set; }

	public string ConfigKey { get; private set; }

	public FancyClassicPlayerResourcesDisplaySet(string nameKey, string configKey, string resourceFolderName, AssetRequestMode mode)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		NameKey = nameKey;
		ConfigKey = configKey;
		if (configKey == "NewWithText")
		{
			_drawText = true;
		}
		else
		{
			_drawText = false;
		}
		string text = "Images\\UI\\PlayerResourceSets\\" + resourceFolderName;
		_heartLeft = Main.Assets.Request<Texture2D>(text + "\\Heart_Left", mode);
		_heartMiddle = Main.Assets.Request<Texture2D>(text + "\\Heart_Middle", mode);
		_heartRight = Main.Assets.Request<Texture2D>(text + "\\Heart_Right", mode);
		_heartRightFancy = Main.Assets.Request<Texture2D>(text + "\\Heart_Right_Fancy", mode);
		_heartFill = Main.Assets.Request<Texture2D>(text + "\\Heart_Fill", mode);
		_heartFillHoney = Main.Assets.Request<Texture2D>(text + "\\Heart_Fill_B", mode);
		_heartSingleFancy = Main.Assets.Request<Texture2D>(text + "\\Heart_Single_Fancy", mode);
		_starTop = Main.Assets.Request<Texture2D>(text + "\\Star_A", mode);
		_starMiddle = Main.Assets.Request<Texture2D>(text + "\\Star_B", mode);
		_starBottom = Main.Assets.Request<Texture2D>(text + "\\Star_C", mode);
		_starSingle = Main.Assets.Request<Texture2D>(text + "\\Star_Single", mode);
		_starFill = Main.Assets.Request<Texture2D>(text + "\\Star_Fill", mode);
	}

	public void Draw()
	{
		Player localPlayer = Main.LocalPlayer;
		SpriteBatch spriteBatch = Main.spriteBatch;
		PrepareFields(localPlayer);
		DrawLifeBar(spriteBatch);
		DrawManaBar(spriteBatch);
	}

	private void DrawLifeBar(SpriteBatch spriteBatch)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)(Main.screenWidth - 300 + 4), 15f);
		if (_drawText)
		{
			val.Y += 6f;
			DrawLifeBarText(spriteBatch, val + new Vector2(-4f, 3f));
		}
		bool isHovered = false;
		ResourceDrawSettings resourceDrawSettings = default;
		resourceDrawSettings.ElementCount = _heartCountRow1;
		resourceDrawSettings.ElementIndexOffset = 0;
		resourceDrawSettings.TopLeftAnchor = val;
		resourceDrawSettings.GetTextureMethod = HeartPanelDrawer;
		resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
		resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
		resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
		resourceDrawSettings.Draw(spriteBatch, ref isHovered);
		resourceDrawSettings = default;
		resourceDrawSettings.ElementCount = _heartCountRow2;
		resourceDrawSettings.ElementIndexOffset = 10;
		resourceDrawSettings.TopLeftAnchor = val + new Vector2(0f, 28f);
		resourceDrawSettings.GetTextureMethod = HeartPanelDrawer;
		resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
		resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
		resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
		resourceDrawSettings.Draw(spriteBatch, ref isHovered);
		resourceDrawSettings = default;
		resourceDrawSettings.ElementCount = _heartCountRow1;
		resourceDrawSettings.ElementIndexOffset = 0;
		resourceDrawSettings.TopLeftAnchor = val + new Vector2(15f, 15f);
		resourceDrawSettings.GetTextureMethod = HeartFillingDrawer;
		resourceDrawSettings.OffsetPerDraw = Vector2.UnitX * 2f;
		resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
		resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
		resourceDrawSettings.Draw(spriteBatch, ref isHovered);
		resourceDrawSettings = default;
		resourceDrawSettings.ElementCount = _heartCountRow2;
		resourceDrawSettings.ElementIndexOffset = 10;
		resourceDrawSettings.TopLeftAnchor = val + new Vector2(15f, 15f) + new Vector2(0f, 28f);
		resourceDrawSettings.GetTextureMethod = HeartFillingDrawer;
		resourceDrawSettings.OffsetPerDraw = Vector2.UnitX * 2f;
		resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
		resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
		resourceDrawSettings.Draw(spriteBatch, ref isHovered);
		_hoverLife = isHovered;
	}

	private static void DrawLifeBarText(SpriteBatch spriteBatch, Vector2 topLeftAnchor)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = topLeftAnchor + new Vector2(130f, -24f);
		Player localPlayer = Main.LocalPlayer;
		Color val2 = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		string text = Lang.inter[0].Value + " " + localPlayer.statLifeMax2 + "/" + localPlayer.statLifeMax2;
		Vector2 val3 = FontAssets.MouseText.Value.MeasureString(text);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Lang.inter[0].Value, val + new Vector2((0f - val3.X) * 0.5f, 0f), val2, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, localPlayer.statLife + "/" + localPlayer.statLifeMax2, val + new Vector2(val3.X * 0.5f, 0f), val2, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
	}

	private void DrawManaBar(SpriteBatch spriteBatch)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)(Main.screenWidth - 40), 22f);
		_ = _starCount;
		bool isHovered = false;
		ResourceDrawSettings resourceDrawSettings = default;
		resourceDrawSettings.ElementCount = _starCount;
		resourceDrawSettings.ElementIndexOffset = 0;
		resourceDrawSettings.TopLeftAnchor = val;
		resourceDrawSettings.GetTextureMethod = StarPanelDrawer;
		resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
		resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitY;
		resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
		resourceDrawSettings.Draw(spriteBatch, ref isHovered);
		resourceDrawSettings = default;
		resourceDrawSettings.ElementCount = _starCount;
		resourceDrawSettings.ElementIndexOffset = 0;
		resourceDrawSettings.TopLeftAnchor = val + new Vector2(15f, 16f);
		resourceDrawSettings.GetTextureMethod = StarFillingDrawer;
		resourceDrawSettings.OffsetPerDraw = Vector2.UnitY * -2f;
		resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitY;
		resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
		resourceDrawSettings.Draw(spriteBatch, ref isHovered);
		_hoverMana = isHovered;
	}

	private static void DrawManaText(SpriteBatch spriteBatch)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = FontAssets.MouseText.Value.MeasureString(Lang.inter[2].Value);
		Color val2 = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		int num = 50;
		if (val.X >= 45f)
		{
			num = (int)val.X + 5;
		}
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Lang.inter[2].Value, new Vector2((float)(Main.screenWidth - num), 6f), val2, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
	}

	private void HeartPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _heartLeft;
		drawScale = 1f;
		if (elementIndex == lastElementIndex && elementIndex == firstElementIndex)
		{
			sprite = _heartSingleFancy;
			offset = new Vector2(-4f, -4f);
		}
		else if (elementIndex == lastElementIndex && lastElementIndex == _lastHeartPanelIndex)
		{
			sprite = _heartRightFancy;
			offset = new Vector2(-8f, -4f);
		}
		else if (elementIndex == lastElementIndex)
		{
			sprite = _heartRight;
		}
		else if (elementIndex != firstElementIndex)
		{
			sprite = _heartMiddle;
		}
	}

	private void HeartFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _heartLeft;
		if (elementIndex < _playerLifeFruitCount)
		{
			sprite = _heartFillHoney;
		}
		else
		{
			sprite = _heartFill;
		}
		float num = (drawScale = Utils.GetLerpValue(_lifePerHeart * (float)elementIndex, _lifePerHeart * (float)(elementIndex + 1), _currentPlayerLife, clamped: true));
		if (elementIndex == _lastHeartFillingIndex && num > 0f)
		{
			drawScale += Main.cursorScale - 1f;
		}
	}

	private void StarPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _starTop;
		drawScale = 1f;
		if (elementIndex == lastElementIndex && elementIndex == firstElementIndex)
		{
			sprite = _starSingle;
		}
		else if (elementIndex == lastElementIndex)
		{
			sprite = _starBottom;
			offset = new Vector2(0f, 0f);
		}
		else if (elementIndex != firstElementIndex)
		{
			sprite = _starMiddle;
		}
	}

	private void StarFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _starFill;
		float num = (drawScale = Utils.GetLerpValue(_manaPerStar * (float)elementIndex, _manaPerStar * (float)(elementIndex + 1), _currentPlayerMana, clamped: true));
		if (elementIndex == _lastStarFillingIndex && num > 0f)
		{
			drawScale += Main.cursorScale - 1f;
		}
	}

	private void PrepareFields(Player player)
	{
		PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(player);
		_playerLifeFruitCount = playerStatsSnapshot.LifeFruitCount;
		_lifePerHeart = playerStatsSnapshot.LifePerSegment;
		_currentPlayerLife = playerStatsSnapshot.Life;
		_manaPerStar = playerStatsSnapshot.ManaPerSegment;
		_heartCountRow1 = Utils.Clamp((int)((float)playerStatsSnapshot.LifeMax / _lifePerHeart), 0, 10);
		_heartCountRow2 = Utils.Clamp((int)((float)(playerStatsSnapshot.LifeMax - 200) / _lifePerHeart), 0, 10);
		int lastHeartFillingIndex = (int)((float)playerStatsSnapshot.Life / _lifePerHeart);
		_lastHeartFillingIndex = lastHeartFillingIndex;
		_lastHeartPanelIndex = _heartCountRow1 + _heartCountRow2 - 1;
		_starCount = (int)((float)playerStatsSnapshot.ManaMax / _manaPerStar);
		_currentPlayerMana = playerStatsSnapshot.Mana;
		_lastStarFillingIndex = (int)(_currentPlayerMana / _manaPerStar);
	}

	public void TryToHover()
	{
		if (_hoverLife)
		{
			CommonResourceBarMethods.DrawLifeMouseOver();
		}
		if (_hoverMana)
		{
			CommonResourceBarMethods.DrawManaMouseOver();
		}
	}
}
