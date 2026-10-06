using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent.UI.ResourceSets;

public class HorizontalBarsPlayerResourcesDisplaySet : IPlayerResourcesDisplaySet, IConfigKeyHolder
{
	private int _maxSegmentCount;

	private int _hpSegmentsCount;

	private int _mpSegmentsCount;

	private int _hpFruitCount;

	private float _hpPercent;

	private float _mpPercent;

	private byte _drawTextStyle;

	private bool _hpHovered;

	private bool _mpHovered;

	private Asset<Texture2D> _hpFill;

	private Asset<Texture2D> _hpFillHoney;

	private Asset<Texture2D> _mpFill;

	private Asset<Texture2D> _panelLeft;

	private Asset<Texture2D> _panelMiddleHP;

	private Asset<Texture2D> _panelRightHP;

	private Asset<Texture2D> _panelMiddleMP;

	private Asset<Texture2D> _panelRightMP;

	public string NameKey { get; private set; }

	public string ConfigKey { get; private set; }

	public HorizontalBarsPlayerResourcesDisplaySet(string nameKey, string configKey, string resourceFolderName, AssetRequestMode mode)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		NameKey = nameKey;
		ConfigKey = configKey;
		if (configKey == "HorizontalBarsWithFullText")
		{
			_drawTextStyle = 2;
		}
		else if (configKey == "HorizontalBarsWithText")
		{
			_drawTextStyle = 1;
		}
		else
		{
			_drawTextStyle = 0;
		}
		string text = "Images\\UI\\PlayerResourceSets\\" + resourceFolderName;
		_hpFill = Main.Assets.Request<Texture2D>(text + "\\HP_Fill", mode);
		_hpFillHoney = Main.Assets.Request<Texture2D>(text + "\\HP_Fill_Honey", mode);
		_mpFill = Main.Assets.Request<Texture2D>(text + "\\MP_Fill", mode);
		_panelLeft = Main.Assets.Request<Texture2D>(text + "\\Panel_Left", mode);
		_panelMiddleHP = Main.Assets.Request<Texture2D>(text + "\\HP_Panel_Middle", mode);
		_panelRightHP = Main.Assets.Request<Texture2D>(text + "\\HP_Panel_Right", mode);
		_panelMiddleMP = Main.Assets.Request<Texture2D>(text + "\\MP_Panel_Middle", mode);
		_panelRightMP = Main.Assets.Request<Texture2D>(text + "\\MP_Panel_Right", mode);
	}

	public void Draw()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		PrepareFields(Main.LocalPlayer);
		SpriteBatch spriteBatch = Main.spriteBatch;
		int num = 16;
		int num2 = 18;
		int num3 = Main.screenWidth - 300 - 22 + num;
		if (_drawTextStyle == 2)
		{
			num2 += 2;
			DrawLifeBarText(spriteBatch, new Vector2((float)num3, (float)num2));
			DrawManaText(spriteBatch);
		}
		else if (_drawTextStyle == 1)
		{
			num2 += 4;
			DrawLifeBarText(spriteBatch, new Vector2((float)num3, (float)num2));
		}
		Vector2 val = new Vector2((float)num3, (float)num2);
		val.X += (_maxSegmentCount - _hpSegmentsCount) * _panelMiddleHP.Width();
		bool isHovered = false;
		ResourceDrawSettings resourceDrawSettings = default;
		resourceDrawSettings.ElementCount = _hpSegmentsCount + 2;
		resourceDrawSettings.ElementIndexOffset = 0;
		resourceDrawSettings.TopLeftAnchor = val;
		resourceDrawSettings.GetTextureMethod = LifePanelDrawer;
		resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
		resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
		resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
		resourceDrawSettings.Draw(spriteBatch, ref isHovered);
		resourceDrawSettings = default;
		resourceDrawSettings.ElementCount = _hpSegmentsCount;
		resourceDrawSettings.ElementIndexOffset = 0;
		resourceDrawSettings.TopLeftAnchor = val + new Vector2(6f, 6f);
		resourceDrawSettings.GetTextureMethod = LifeFillingDrawer;
		resourceDrawSettings.OffsetPerDraw = new Vector2((float)_hpFill.Width(), 0f);
		resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
		resourceDrawSettings.Draw(spriteBatch, ref isHovered);
		_hpHovered = isHovered;
		isHovered = false;
		Vector2 val2 = new Vector2((float)(num3 - 10), (float)(num2 + 24));
		val2.X += (_maxSegmentCount - _mpSegmentsCount) * _panelMiddleMP.Width();
		resourceDrawSettings = default;
		resourceDrawSettings.ElementCount = _mpSegmentsCount + 2;
		resourceDrawSettings.ElementIndexOffset = 0;
		resourceDrawSettings.TopLeftAnchor = val2;
		resourceDrawSettings.GetTextureMethod = ManaPanelDrawer;
		resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
		resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
		resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
		resourceDrawSettings.Draw(spriteBatch, ref isHovered);
		resourceDrawSettings = default;
		resourceDrawSettings.ElementCount = _mpSegmentsCount;
		resourceDrawSettings.ElementIndexOffset = 0;
		resourceDrawSettings.TopLeftAnchor = val2 + new Vector2(6f, 6f);
		resourceDrawSettings.GetTextureMethod = ManaFillingDrawer;
		resourceDrawSettings.OffsetPerDraw = new Vector2((float)_mpFill.Width(), 0f);
		resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
		resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
		resourceDrawSettings.Draw(spriteBatch, ref isHovered);
		_mpHovered = isHovered;
	}

	private static void DrawManaText(SpriteBatch spriteBatch)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		Color val = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		int num = 180;
		Player localPlayer = Main.LocalPlayer;
		string text = Lang.inter[2].Value + ":";
		string text2 = localPlayer.statMana + "/" + localPlayer.statManaMax2;
		Vector2 val2 = new Vector2((float)(Main.screenWidth - num), 65f);
		string text3 = text + " " + text2;
		Vector2 val3 = FontAssets.MouseText.Value.MeasureString(text3);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, val2 + new Vector2((0f - val3.X) * 0.5f, 0f), val, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text2, val2 + new Vector2(val3.X * 0.5f, 0f), val, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(text2).X, 0f), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
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
		Vector2 val = topLeftAnchor + new Vector2(130f, -20f);
		Player localPlayer = Main.LocalPlayer;
		Color val2 = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		string text = Lang.inter[0].Value + " " + localPlayer.statLifeMax2 + "/" + localPlayer.statLifeMax2;
		Vector2 val3 = FontAssets.MouseText.Value.MeasureString(text);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Lang.inter[0].Value, val + new Vector2((0f - val3.X) * 0.5f, 0f), val2, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, localPlayer.statLife + "/" + localPlayer.statLifeMax2, val + new Vector2(val3.X * 0.5f, 0f), val2, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
	}

	private void PrepareFields(Player player)
	{
		PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(player);
		_hpSegmentsCount = (int)((float)playerStatsSnapshot.LifeMax / playerStatsSnapshot.LifePerSegment);
		_mpSegmentsCount = (int)((float)playerStatsSnapshot.ManaMax / playerStatsSnapshot.ManaPerSegment);
		_maxSegmentCount = 20;
		_hpFruitCount = playerStatsSnapshot.LifeFruitCount;
		_hpPercent = (float)playerStatsSnapshot.Life / (float)playerStatsSnapshot.LifeMax;
		_mpPercent = (float)playerStatsSnapshot.Mana / (float)playerStatsSnapshot.ManaMax;
	}

	private void LifePanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _panelLeft;
		drawScale = 1f;
		if (elementIndex == lastElementIndex)
		{
			sprite = _panelRightHP;
			offset = new Vector2(-16f, -10f);
		}
		else if (elementIndex != firstElementIndex)
		{
			sprite = _panelMiddleHP;
		}
	}

	private void ManaPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _panelLeft;
		drawScale = 1f;
		if (elementIndex == lastElementIndex)
		{
			sprite = _panelRightMP;
			offset = new Vector2(-16f, -6f);
		}
		else if (elementIndex != firstElementIndex)
		{
			sprite = _panelMiddleMP;
		}
	}

	private void LifeFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		sprite = _hpFill;
		if (elementIndex >= _hpSegmentsCount - _hpFruitCount)
		{
			sprite = _hpFillHoney;
		}
		FillBarByValues(elementIndex, sprite, _hpSegmentsCount, _hpPercent, out offset, out drawScale, out sourceRect);
	}

	private static void FillBarByValues(int elementIndex, Asset<Texture2D> sprite, int segmentsCount, float fillPercent, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		float num = 1f;
		float num2 = 1f / (float)segmentsCount;
		float t = 1f - fillPercent;
		float lerpValue = Utils.GetLerpValue(num2 * (float)elementIndex, num2 * (float)(elementIndex + 1), t, clamped: true);
		num = 1f - lerpValue;
		drawScale = 1f;
		Rectangle val = sprite.Frame();
		int num3 = (int)((float)val.Width * (1f - num));
		offset.X += num3;
		val.X += num3;
		val.Width -= num3;
		sourceRect = val;
	}

	private void ManaFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		sprite = _mpFill;
		FillBarByValues(elementIndex, sprite, _mpSegmentsCount, _mpPercent, out offset, out drawScale, out sourceRect);
	}

	public void TryToHover()
	{
		if (_hpHovered)
		{
			CommonResourceBarMethods.DrawLifeMouseOver();
		}
		if (_mpHovered)
		{
			CommonResourceBarMethods.DrawManaMouseOver();
		}
	}
}
