using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.UI.Elements;

public class UICyclingImage : UIImage
{
	public int FramesPerCycle;

	private List<Asset<Texture2D>> _textureAssets;

	private int _currentTextureIndex;

	private int _framesCounted;

	public UICyclingImage(List<Asset<Texture2D>> textureAssets)
	{
		FramesPerCycle = 45;
		_textureAssets = textureAssets;
		SetImage(_textureAssets[_currentTextureIndex]);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (++_framesCounted >= FramesPerCycle)
		{
			_framesCounted = 0;
			if (++_currentTextureIndex >= _textureAssets.Count)
			{
				_currentTextureIndex = 0;
			}
			SetImage(_textureAssets[_currentTextureIndex]);
		}
	}
}
