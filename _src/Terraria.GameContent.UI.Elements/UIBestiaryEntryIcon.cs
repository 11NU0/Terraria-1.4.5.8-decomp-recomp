using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIBestiaryEntryIcon : UIElement
{
	private BestiaryEntry _entry;

	private Asset<Texture2D> _notUnlockedTexture;

	private bool _isPortrait;

	public bool ForceHover;

	private BestiaryUICollectionInfo _collectionInfo;

	public UIBestiaryEntryIcon(BestiaryEntry entry, bool isPortrait)
	{
		_entry = entry;
		IgnoresMouseInteraction = true;
		OverrideSamplerState = Main.DefaultSamplerState;
		UseImmediateMode = true;
		Width.Set(0f, 1f);
		Height.Set(0f, 1f);
		_notUnlockedTexture = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Locked", (AssetRequestMode)1);
		_isPortrait = isPortrait;
		_collectionInfo = _entry.UIInfoProvider.GetEntryUICollectionInfo();
	}

	public override void Update(GameTime gameTime)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		_collectionInfo = _entry.UIInfoProvider.GetEntryUICollectionInfo();
		CalculatedStyle dimensions = GetDimensions();
		bool isHovered = IsMouseHovering || ForceHover;
		_entry.Icon.Update(_collectionInfo, dimensions.ToRectangle(), new EntryIconDrawSettings
		{
			iconbox = dimensions.ToRectangle(),
			IsPortrait = _isPortrait,
			IsHovered = isHovered
		});
		base.Update(gameTime);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		bool unlockState = _entry.Icon.GetUnlockState(_collectionInfo);
		bool isHovered = IsMouseHovering || ForceHover;
		if (unlockState)
		{
			_entry.Icon.Draw(_collectionInfo, spriteBatch, new EntryIconDrawSettings
			{
				iconbox = dimensions.ToRectangle(),
				IsPortrait = _isPortrait,
				IsHovered = isHovered
			});
		}
		else
		{
			Texture2D value = _notUnlockedTexture.Value;
			spriteBatch.Draw(value, dimensions.Center(), (Rectangle?)null, Color.White * 0.15f, 0f, value.Size() / 2f, 1f, (SpriteEffects)0, 0f);
		}
	}

	public string GetHoverText()
	{
		return _entry.Icon.GetHoverText(_collectionInfo);
	}
}
