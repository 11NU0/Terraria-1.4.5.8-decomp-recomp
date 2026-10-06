using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIGenProgressBar : UIElement
{
	private Asset<Texture2D> _texOuterCrimson;

	private Asset<Texture2D> _texOuterCorrupt;

	private Asset<Texture2D> _texOuterRandom;

	private Asset<Texture2D> _texOuterLower;

	private float _visualOverallProgress;

	private float _targetOverallProgress;

	private float _visualCurrentProgress;

	private float _targetCurrentProgress;

	private int _smallBarWidth = 508;

	private int _longBarWidth = 570;

	public UIGenProgressBar()
	{
		if (Main.netMode != 2)
		{
			_texOuterCorrupt = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Corrupt", (AssetRequestMode)1);
			_texOuterCrimson = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Crimson", (AssetRequestMode)1);
			_texOuterRandom = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Random", (AssetRequestMode)1);
			_texOuterLower = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Lower", (AssetRequestMode)1);
		}
		Recalculate();
	}

	public override void Recalculate()
	{
		Width.Precent = 0f;
		Height.Precent = 0f;
		Width.Pixels = 612f;
		Height.Pixels = 70f;
		base.Recalculate();
	}

	public void SetProgress(float overallProgress, float currentProgress)
	{
		_targetCurrentProgress = currentProgress;
		_targetOverallProgress = overallProgress;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		if (_texOuterCorrupt.IsLoaded && _texOuterCrimson.IsLoaded && _texOuterRandom.IsLoaded && _texOuterLower.IsLoaded)
		{
			bool flag = WorldGen.crimson;
			bool flag2 = WorldGen.generatingRandomEvil;
			if (WorldGen.drunkWorldGen && Main.rand.Next(2) == 0)
			{
				flag = Main.rand.Next(2) == 0;
				flag2 = Main.rand.Next(4) == 0;
			}
			_visualOverallProgress = _targetOverallProgress;
			_visualCurrentProgress = _targetCurrentProgress;
			CalculatedStyle dimensions = GetDimensions();
			int completedWidth = (int)(_visualOverallProgress * (float)_longBarWidth);
			int completedWidth2 = (int)(_visualCurrentProgress * (float)_smallBarWidth);
			Vector2 val = new Vector2(dimensions.X, dimensions.Y);
			Color val2 = default;
			uint packedValue;
			if (flag2)
			{
				packedValue = 4292696893u;
			}
			else
			{
				packedValue = (flag ? 4286836223u : 4283888223u);
			}
			val2.PackedValue = packedValue;
			DrawFilling2(spriteBatch, val + new Vector2(20f, 40f), 16, completedWidth, _longBarWidth, val2, Color.Lerp(val2, Color.Black, 0.5f), new Color(48, 48, 48));
			val2.PackedValue = 4290947159u;
			DrawFilling2(spriteBatch, val + new Vector2(50f, 60f), 8, completedWidth2, _smallBarWidth, val2, Color.Lerp(val2, Color.Black, 0.5f), new Color(33, 33, 33));
			Rectangle r = GetDimensions().ToRectangle();
			r.X -= 8;
			Texture2D val3;
			if (flag2)
			{
				val3 = _texOuterRandom.Value;
			}
			else
			{
				val3 = (flag ? _texOuterCrimson.Value : _texOuterCorrupt.Value);
			}
			spriteBatch.Draw(val3, r.TopLeft(), Color.White);
			spriteBatch.Draw(_texOuterLower.Value, r.TopLeft() + new Vector2(44f, 60f), Color.White);
		}
	}

	private void DrawFilling(SpriteBatch spritebatch, Texture2D tex, Texture2D texShadow, Vector2 topLeft, int completedWidth, int totalWidth, Color separator, Color empty)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		if (completedWidth % 2 != 0)
		{
			completedWidth--;
		}
		Vector2 val = topLeft + (float)completedWidth * Vector2.UnitX;
		int num = completedWidth;
		Rectangle val2 = tex.Frame();
		while (num > 0)
		{
			if (val2.Width > num)
			{
				val2.X += val2.Width - num;
				val2.Width = num;
			}
			spritebatch.Draw(tex, val, (Rectangle?)val2, Color.White, 0f, new Vector2((float)val2.Width, 0f), 1f, (SpriteEffects)0, 0f);
			val.X -= val2.Width;
			num -= val2.Width;
		}
		if (texShadow != null)
		{
			spritebatch.Draw(texShadow, topLeft, (Rectangle?)new Rectangle(0, 0, completedWidth, texShadow.Height), Color.White);
		}
		spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth, (int)topLeft.Y, totalWidth - completedWidth, tex.Height), (Rectangle?)new Rectangle(0, 0, 1, 1), empty);
		spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth - 2, (int)topLeft.Y, 2, tex.Height), (Rectangle?)new Rectangle(0, 0, 1, 1), separator);
	}

	private void DrawFilling2(SpriteBatch spritebatch, Vector2 topLeft, int height, int completedWidth, int totalWidth, Color filled, Color separator, Color empty)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		if (completedWidth % 2 != 0)
		{
			completedWidth--;
		}
		spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X, (int)topLeft.Y, completedWidth, height), (Rectangle?)new Rectangle(0, 0, 1, 1), filled);
		spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth, (int)topLeft.Y, totalWidth - completedWidth, height), (Rectangle?)new Rectangle(0, 0, 1, 1), empty);
		spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth - 2, (int)topLeft.Y, 2, height), (Rectangle?)new Rectangle(0, 0, 1, 1), separator);
	}
}
