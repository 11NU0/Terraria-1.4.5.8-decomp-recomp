using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Localization;

namespace Terraria.GameContent.Bestiary;

public class CustomEntryIcon : IEntryIcon
{
	private LocalizedText _text;

	private Asset<Texture2D> _textureAsset;

	private Rectangle _sourceRectangle;

	private Func<bool> _unlockCondition;

	public CustomEntryIcon(string nameLanguageKey, string texturePath, Func<bool> unlockCondition)
	{
		_text = Language.GetText(nameLanguageKey);
		_textureAsset = Main.Assets.Request<Texture2D>(texturePath, (AssetRequestMode)1);
		_unlockCondition = unlockCondition;
		UpdateUnlockState(state: false);
	}

	public IEntryIcon CreateClone()
	{
		return new CustomEntryIcon(_text.Key, _textureAsset.Name, _unlockCondition);
	}

	public void Update(BestiaryUICollectionInfo providedInfo, Rectangle hitbox, EntryIconDrawSettings settings)
	{
		UpdateUnlockState(GetUnlockState(providedInfo));
	}

	public void Draw(BestiaryUICollectionInfo providedInfo, SpriteBatch spriteBatch, EntryIconDrawSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Rectangle iconbox = settings.iconbox;
		spriteBatch.Draw(_textureAsset.Value, iconbox.Center.ToVector2() + Vector2.One, (Rectangle?)_sourceRectangle, Color.White, 0f, _sourceRectangle.Size() / 2f, 1f, (SpriteEffects)0, 0f);
	}

	public string GetHoverText(BestiaryUICollectionInfo providedInfo)
	{
		if (GetUnlockState(providedInfo))
		{
			return _text.Value;
		}
		return "???";
	}

	private void UpdateUnlockState(bool state)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		_sourceRectangle = _textureAsset.Frame(2, 1, state.ToInt());
		_sourceRectangle.Inflate(-2, -2);
	}

	public bool GetUnlockState(BestiaryUICollectionInfo providedInfo)
	{
		return providedInfo.UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
	}
}
