using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.Graphics.Renderers;

public class MapHeadRenderer : INeedRenderTargetContent
{
	private bool _anyDirty;

	private PlayerHeadDrawRenderTargetContent[] _playerRenders = new PlayerHeadDrawRenderTargetContent[255];

	private readonly List<DrawData> _drawData = new List<DrawData>();

	public bool IsReady => !_anyDirty;

	public MapHeadRenderer()
	{
		for (int i = 0; i < _playerRenders.Length; i++)
		{
			_playerRenders[i] = new PlayerHeadDrawRenderTargetContent();
		}
	}

	public void Reset()
	{
		_anyDirty = false;
		_drawData.Clear();
		for (int i = 0; i < _playerRenders.Length; i++)
		{
			_playerRenders[i].Reset();
		}
	}

	public void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color))
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		PlayerHeadDrawRenderTargetContent playerHeadDrawRenderTargetContent = _playerRenders[drawPlayer.whoAmI];
		playerHeadDrawRenderTargetContent.UsePlayer(drawPlayer);
		playerHeadDrawRenderTargetContent.UseColor(borderColor);
		playerHeadDrawRenderTargetContent.Request();
		_anyDirty = true;
		_drawData.Clear();
		if (playerHeadDrawRenderTargetContent.IsReady)
		{
			RenderTarget2D target = playerHeadDrawRenderTargetContent.GetTarget();
			_drawData.Add(new DrawData((Texture2D)(object)target, position, null, Color.White, 0f, ((Texture2D)(object)target).Size() / 2f, scale, (SpriteEffects)0));
			RenderDrawData(drawPlayer);
		}
	}

	private void RenderDrawData(Player drawPlayer)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Effect pixelShader = Main.pixelShader;
		SpriteBatch spriteBatch = Main.spriteBatch;
		for (int i = 0; i < _drawData.Count; i++)
		{
			DrawData cdd = _drawData[i];
			if (!cdd.sourceRect.HasValue)
			{
				cdd.sourceRect = cdd.texture.Frame();
			}
			PlayerDrawHelper.SetShaderForData(drawPlayer, drawPlayer.cHead, ref cdd);
			if (cdd.texture != null)
			{
				cdd.Draw(spriteBatch);
			}
		}
		pixelShader.CurrentTechnique.Passes[0].Apply();
	}

	public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
	{
		if (_anyDirty)
		{
			for (int i = 0; i < _playerRenders.Length; i++)
			{
				_playerRenders[i].PrepareRenderTarget(device, spriteBatch);
			}
			_anyDirty = false;
		}
	}

	private void CreateOutlines(float alpha, float scale, Color borderColor)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		if (!(borderColor != Color.Transparent))
		{
			return;
		}
		List<DrawData> collection = new List<DrawData>(_drawData);
		List<DrawData> list = new List<DrawData>(_drawData);
		_drawData.Clear();
		float num = 2f * scale;
		Color val = borderColor;
		val *= alpha * alpha;
		Color black = Color.Black;
		black *= alpha * alpha;
		int colorOnlyShaderIndex = ContentSamples.DyeShaderIDs.ColorOnlyShaderIndex;
		for (int i = 0; i < list.Count; i++)
		{
			DrawData value = list[i];
			value.shader = colorOnlyShaderIndex;
			value.color = black;
			list[i] = value;
		}
		int num2 = 2;
		Vector2 val2;
		for (int j = -num2; j <= num2; j++)
		{
			for (int k = -num2; k <= num2; k++)
			{
				if (Math.Abs(j) + Math.Abs(k) == num2)
				{
					val2 = new Vector2((float)j * num, (float)k * num);
					for (int l = 0; l < list.Count; l++)
					{
						DrawData item = list[l];
						ref Vector2 position = ref item.position;
						position += val2;
						_drawData.Add(item);
					}
				}
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			DrawData value2 = list[m];
			value2.shader = colorOnlyShaderIndex;
			value2.color = val;
			list[m] = value2;
		}
		val2 = Vector2.Zero;
		num2 = 1;
		for (int n = -num2; n <= num2; n++)
		{
			for (int num3 = -num2; num3 <= num2; num3++)
			{
				if (Math.Abs(n) + Math.Abs(num3) == num2)
				{
					val2 = new Vector2((float)n * num, (float)num3 * num);
					for (int num4 = 0; num4 < list.Count; num4++)
					{
						DrawData item2 = list[num4];
						ref Vector2 position2 = ref item2.position;
						position2 += val2;
						_drawData.Add(item2);
					}
				}
			}
		}
		_drawData.AddRange(collection);
	}
}
