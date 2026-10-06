using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.Localization;

namespace Terraria.DataStructures;

public class TitleLinkButton
{
	private static Item _fakeItem = new Item();

	public string TooltipTextKey;

	public string LinkUrl;

	public Asset<Texture2D> Image;

	public Rectangle? FrameWhenNotSelected;

	public Rectangle? FrameWehnSelected;

	public void Draw(SpriteBatch spriteBatch, Vector2 anchorPosition)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		Rectangle r = Image.Frame();
		if (FrameWhenNotSelected.HasValue)
		{
			r = FrameWhenNotSelected.Value;
		}
		Vector2 val = r.Size();
		Vector2 val2 = anchorPosition - val / 2f;
		bool flag = false;
		if (Main.MouseScreen.Between(val2, val2 + val))
		{
			Main.LocalPlayer.mouseInterface = true;
			flag = true;
			DrawTooltip();
			TryClicking();
		}
		Rectangle? val3 = (flag ? FrameWehnSelected : FrameWhenNotSelected);
		Rectangle val4 = Image.Frame();
		if (val3.HasValue)
		{
			val4 = val3.Value;
		}
		Texture2D value = Image.Value;
		spriteBatch.Draw(value, anchorPosition, (Rectangle?)val4, Color.White, 0f, val4.Size() / 2f, 1f, (SpriteEffects)0, 0f);
	}

	private void DrawTooltip()
	{
		Item fakeItem = _fakeItem;
		fakeItem.SetDefaults(0);
		string textValue = Language.GetTextValue(TooltipTextKey);
		fakeItem.SetNameOverride(textValue);
		fakeItem.type = 1;
		fakeItem.scale = 0f;
		fakeItem.rare = 8;
		fakeItem.value = -1;
		Main.HoverItem = _fakeItem;
		Main.instance.MouseText("", 0, 0);
		Main.mouseText = true;
	}

	private void TryClicking()
	{
		if (!PlayerInput.IgnoreMouseInterface && Main.mouseLeft && Main.mouseLeftRelease)
		{
			SoundEngine.PlaySound(10);
			Main.mouseLeftRelease = false;
			OpenLink();
		}
	}

	private void OpenLink()
	{
		try
		{
			Platform.Get<IPathService>().OpenURL(LinkUrl);
		}
		catch
		{
			Console.WriteLine("Failed to open link?!");
		}
	}
}
