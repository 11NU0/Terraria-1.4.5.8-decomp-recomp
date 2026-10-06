using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIWorldCreationPreview : UIElement
{
	private readonly Asset<Texture2D> _BorderTexture;

	private readonly Asset<Texture2D> _BackgroundExpertTexture;

	private readonly Asset<Texture2D> _BackgroundNormalTexture;

	private readonly Asset<Texture2D> _BackgroundMasterTexture;

	private readonly Asset<Texture2D> _BunnyExpertTexture;

	private readonly Asset<Texture2D> _BunnyNormalTexture;

	private readonly Asset<Texture2D> _BunnyCreativeTexture;

	private readonly Asset<Texture2D> _BunnyMasterTexture;

	private readonly Asset<Texture2D> _EvilRandomTexture;

	private readonly Asset<Texture2D> _EvilCorruptionTexture;

	private readonly Asset<Texture2D> _EvilCrimsonTexture;

	private readonly Asset<Texture2D> _SizeSmallTexture;

	private readonly Asset<Texture2D> _SizeMediumTexture;

	private readonly Asset<Texture2D> _SizeLargeTexture;

	private byte _difficulty;

	private byte _evil;

	private byte _size;

	public UIWorldCreationPreview()
	{
		_BorderTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewBorder", (AssetRequestMode)1);
		_BackgroundNormalTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyNormal1", (AssetRequestMode)1);
		_BackgroundExpertTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyExpert1", (AssetRequestMode)1);
		_BackgroundMasterTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyMaster1", (AssetRequestMode)1);
		_BunnyNormalTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyNormal2", (AssetRequestMode)1);
		_BunnyExpertTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyExpert2", (AssetRequestMode)1);
		_BunnyCreativeTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyCreative2", (AssetRequestMode)1);
		_BunnyMasterTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyMaster2", (AssetRequestMode)1);
		_EvilRandomTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewEvilRandom", (AssetRequestMode)1);
		_EvilCorruptionTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewEvilCorruption", (AssetRequestMode)1);
		_EvilCrimsonTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewEvilCrimson", (AssetRequestMode)1);
		_SizeSmallTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeSmall", (AssetRequestMode)1);
		_SizeMediumTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeMedium", (AssetRequestMode)1);
		_SizeLargeTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeLarge", (AssetRequestMode)1);
		Width.Set(_BackgroundExpertTexture.Width(), 0f);
		Height.Set(_BackgroundExpertTexture.Height(), 0f);
	}

	public void UpdateOption(byte difficulty, byte evil, byte size)
	{
		_difficulty = difficulty;
		_evil = evil;
		_size = size;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Vector2 val = new Vector2(dimensions.X + 4f, dimensions.Y + 4f);
		Color val2 = Color.White;
		switch (_difficulty)
		{
		case 0:
		case 3:
			spriteBatch.Draw(_BackgroundNormalTexture.Value, val, Color.White);
			val2 = Color.White;
			break;
		case 1:
			spriteBatch.Draw(_BackgroundExpertTexture.Value, val, Color.White);
			val2 = Color.DarkGray;
			break;
		case 2:
			spriteBatch.Draw(_BackgroundMasterTexture.Value, val, Color.White);
			val2 = Color.DarkGray;
			break;
		}
		switch (_size)
		{
		case 0:
			spriteBatch.Draw(_SizeSmallTexture.Value, val, val2);
			break;
		case 1:
			spriteBatch.Draw(_SizeMediumTexture.Value, val, val2);
			break;
		case 2:
			spriteBatch.Draw(_SizeLargeTexture.Value, val, val2);
			break;
		}
		switch (_evil)
		{
		case 0:
			spriteBatch.Draw(_EvilRandomTexture.Value, val, val2);
			break;
		case 1:
			spriteBatch.Draw(_EvilCorruptionTexture.Value, val, val2);
			break;
		case 2:
			spriteBatch.Draw(_EvilCrimsonTexture.Value, val, val2);
			break;
		}
		switch (_difficulty)
		{
		case 0:
			spriteBatch.Draw(_BunnyNormalTexture.Value, val, val2);
			break;
		case 1:
			spriteBatch.Draw(_BunnyExpertTexture.Value, val, val2);
			break;
		case 2:
			spriteBatch.Draw(_BunnyMasterTexture.Value, val, val2 * 1.2f);
			break;
		case 3:
			spriteBatch.Draw(_BunnyCreativeTexture.Value, val, val2);
			break;
		}
		spriteBatch.Draw(_BorderTexture.Value, new Vector2(dimensions.X, dimensions.Y), Color.White);
	}
}
